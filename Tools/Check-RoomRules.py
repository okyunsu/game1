"""Read-only room measurements and V5 draft geometry checks.

V5 graph: WORLD_V5 section 3 supplies jump=2.5u, ordinary step limit=2u,
pogo bounce=2u. Lantern AND spike nodes use authored top y+.5. A pogo
edge may rise 2u + downAttackHeight read from the existing CombatTuning
asset (the downward hit can start above the target); this is an optimistic
reach envelope, not a simulated trajectory. Horizontal gap limit=3u.
Every authored spawn is checked separately. Fall spawns land on the first
solid/platform/intact seal/cover/breakable top below their x/y. Ordinary and pogo-assisted graphs are
compared. Walls, head collision, enemy motion and input timing are NOT
solved by the graph; runtime route measurements remain separate evidence.
"""
import argparse
import hashlib
import json
import re
import uuid
from pathlib import Path

KINDS = {
    "enemies": "적", "hazard": "가시", "pad": "내려찍기 발판",
    "steam": "증기", "lifts": "승강기", "crumble": "무너지는 발판",
    "breakable": "부서지는 벽", "seal": "봉인", "cover": "덮개",
    "climb": "벽 점프 벽", "pogo": "포고",
}

# B19: retain failures tied to observed bugs; speculative geometry is a warning.
# These are checker severities, not edits to the authored game rules or values.
UNPROVEN_RULES = {'door-safety', 'overlap', 'ability-gate', 'height', 'arrival',
                  'reachability', 'pit-patrol', 'pit-width'}


def rule_level(level, rule):
    return 'WARN' if level == 'FAIL' and rule in UNPROVEN_RULES else level


def floor_faces(room):
    """Authored horizontal solid/platform tops, merged on the same y plane."""
    planes = {}
    for x, y, w, h in room.get("solid", []) + room.get("platform", []):
        top = round(y + h, 6)
        if w <= h or top >= room["size"][1]:
            continue  # vertical blocks and ceiling outer faces
        planes.setdefault(top, []).append((x, x + w))
    faces = 0
    for intervals in planes.values():
        end = None
        for left, right in sorted(intervals):
            if end is None or left > end:
                faces += 1
            end = max(right, end if end is not None else right)
    return faces, sorted(planes)


def category(room):
    if "허브" in room.get("title", ""):
        return "허브"
    if any("엔딩 트리거" in m.get("t", "") for m in room.get("marks", [])):
        return "엔딩"
    if "자리만" in room.get("title", ""):
        return "자리만(정상)"
    return "일반"


def report(source):
    raw = source.read_bytes()
    rooms = json.loads(raw.decode("utf-8-sig"))["rooms"]
    total_area = sum(r["size"][0] * r["size"][1] for r in rooms.values())
    lines = ["# V4 방 규칙 기준선", "", f"원본: `{source.as_posix()}`",
             f"SHA256: `{hashlib.sha256(raw).hexdigest()}`", "",
             f"방 수: {len(rooms)}, 합계 면적: {total_area:g} u².", "",
             "측정만 하며 합격·불합격을 판정하지 않는다. V5 참고 기준은 비허브 1,000u²당 요소3개, 높이20 초과는 바닥 면2개다.",
             "요소 합계는 아래 11종의 JSON 배열 길이 합이다. 일반 solid/platform은 밀도 요소에 넣지 않는다. 적에는 보스, 포고에는 가시 포고가 포함된다.",
             "바닥 면은 solid/platform 중 폭>높이이고 상단이 방 천장보다 낮은 수평 조각을 같은 y에서 겹침·맞닿음 병합해 센다. 별도로 서로 다른 상단 높이 수도 표시한다. 올라설 수 있음·가림·도달 가능성 판정은 하지 않는다.",
             "허브는 title의 허브, 엔딩은 marks의 엔딩 트리거로 구분한다. 자리만인 정상은 별도로 표시한다.", "",
             "| 방 | 구분 | 크기(u) | 면적(u²) | " + " | ".join(KINDS.values()) + " | 요소 합 | /1,000u² | 문 | 높이>20 바닥 면 | 상단 높이 수 | 상단 높이(u) |",
             "|---|---|---|---:|" + "---:|" * (len(KINDS) + 6)]
    totals = {k: 0 for k in KINDS}
    door_total = 0
    for room_id, room in rooms.items():
        width, height = room["size"]
        area = width * height
        counts = [len(room.get(k, [])) for k in KINDS]
        for key, count in zip(KINDS, counts):
            totals[key] += count
        doors = len(room.get("doors", []))
        door_total += doors
        faces, levels = floor_faces(room)
        row = [room_id, category(room), f"{width:g}×{height:g}", f"{area:g}",
               *map(str, counts), str(sum(counts)), f"{sum(counts) * 1000 / area:.3f}", str(doors),
               str(faces) if height > 20 else "—", str(len(levels)) if height > 20 else "—",
               ", ".join(f"{v:g}" for v in levels) if height > 20 else "—"]
        lines.append("| " + " | ".join(row) + " |")
    lines += ["", f"요소 합계: {sum(totals.values())}, 방별 문 표식 합계: {door_total} (양쪽 방의 표식을 각각 센다).",
              "종류별 합계: " + ", ".join(f"{KINDS[k]}={v}" for k, v in totals.items()) + ".", ""]
    return "\n".join(lines)


def intersects(a, b):
    return a[0] < b[0]+b[2]-1e-6 and b[0] < a[0]+a[2]-1e-6 and a[1] < b[1]+b[3]-1e-6 and b[1] < a[1]+a[3]-1e-6


def gap(a, b):
    return max(0, a[0]-b[0]-b[2], b[0]-a[0]-a[2])


def attack_height():
    tuning = Path('Assets/ScriptableObjects/CombatTuning.asset').read_text(encoding='utf-8-sig')
    return float(re.search(r'(?m)^\s*downAttackHeight:\s*([\d.]+)', tuning).group(1))


def player_size():
    """Read the authored player BoxCollider2D, never a duplicated gameplay value."""
    text = Path('Assets/Prefabs/Player.prefab').read_text(encoding='utf-8-sig')
    box = next(p for p in text.split('--- !u!') if p.startswith('61 '))
    match = re.search(r'm_Size: \{x: ([\d.]+), y: ([\d.]+)\}', box)
    return tuple(map(float, match.groups()))


def floor_pits(room):
    """Shallow floor sections, excluding authored damaging trenches."""
    return [r for r in room.get('solid', []) if abs(r[1]) < 1e-6 and 0 < r[3] <= .4+1e-6
            and not any(min(r[0]+r[2], h[0]+h[2])-max(r[0], h[0]) > 1e-6 for h in room.get('hazard', []))]


def headroom_pairs(room):
    rects = room.get('solid', [])+room.get('platform', [])
    for i, a in enumerate(rects):
        top = a[1]+a[3]
        if top >= room['size'][1]-1e-6:
            continue
        for j, b in enumerate(rects):
            overlap = min(a[0]+a[2], b[0]+b[2])-max(a[0], b[0])
            clearance = b[1]-top
            if i != j and overlap > 1e-6 and clearance > 1e-6:
                # A solid joined directly above makes that part of the lower top
                # internal, rather than a standing face. Test only exposed spans.
                spans = [(max(a[0], b[0]), min(a[0]+a[2], b[0]+b[2]))]
                for cover in room.get('solid', []):
                    if abs(cover[1]-top) > 1e-6:
                        continue
                    spans = [(lo, hi) for l, r in spans for lo, hi in
                             ((l, min(r, cover[0])), (max(l, cover[0]+cover[2]), r)) if hi-lo > 1e-6]
                if spans:
                    yield i, j, clearance


def draft2_checks(room):
    findings = []
    def fail(rule, detail):
        findings.append((rule_level('FAIL', rule), rule, detail))
    _, height = player_size()
    required = height+.6  # WORLD_V5 draft2: body height plus jump clearance.
    if room['size'][1] < 12:
        fail('room-height', f'height={room["size"][1]:g} minimum=12 cameraHeight=11')
    for i, j, clearance in headroom_pairs(room):
        if clearance < required-1e-6:
            fail('headroom', f'surface={i} overhead={j} clearance={clearance:g} required={required:g} playerHeight={height:g}')
    pits = floor_pits(room)
    trenches = room.get('hazard', [])
    for zone in trenches+pits:
        for i, block in enumerate(room.get('solid', [])):
            if block is zone or abs(block[1]) > 1e-6 or block[1]+block[3] <= .4+1e-6:
                continue
            if min(block[0]+block[2], zone[0]+zone[2])-max(block[0], zone[0]) > 1e-6:
                fail('covered-trench', f'solid={i} x={block[0]:g}..{block[0]+block[2]:g} zone={zone[0]:g}..{zone[0]+zone[2]:g}')
    for pit in pits:
        if pit[2] > 3+1e-6:
            fail('pit-width', f'x={pit[0]:g} width={pit[2]:g} maximum=3')
        for i, enemy in enumerate(room.get('enemies', [])):
            patrol = enemy.get('patrol', [])
            if len(patrol) == 2 and .4-1e-6 <= enemy['y'] <= 1+.1 and min(max(patrol), pit[0]+pit[2])-max(min(patrol), pit[0]) > 1e-6:
                fail('pit-patrol', f'enemy={i} patrol={patrol} pit={pit[0]:g}..{pit[0]+pit[2]:g} feet={enemy["y"]:g}')
    return findings


def draft3_checks(room):
    """Read-only draft3 margin, enemy support and pogo climb candidates."""
    findings = []
    solids = room.get('solid', [])
    surfaces = solids + room.get('platform', [])
    required = player_size()[1] + .6
    for i, a in enumerate(surfaces):
        top = a[1]+a[3]
        if top >= room['size'][1]-1e-6:
            continue
        for j, b in enumerate(surfaces):
            clearance = b[1]-top
            if i == j or not 1e-6 < clearance < required-1e-6 or gap(a, b) > .9+1e-6:
                continue
            # Remove internal faces joined directly to a solid above.
            spans = [(a[0], a[0]+a[2])]
            for cover in solids:
                if abs(cover[1]-top) > 1e-6:
                    continue
                spans = [(lo, hi) for l, r in spans for lo, hi in
                         ((l, min(r, cover[0])), (max(l, cover[0]+cover[2]), r)) if hi-lo > 1e-6]
            if any(gap((lo, 0, hi-lo, 0), b) <= .9+1e-6 for lo, hi in spans):
                findings.append(('FAIL', 'edge-headroom', f'surface={i} overhead={j} gap={gap(a,b):g} clearance={clearance:g} required={required:g}'))
    def supported_intervals(y):
        return sorted((a[0], a[0]+a[2]) for a in surfaces+room.get('seal', [])+room.get('cover', [])+room.get('breakable', []) if abs(a[1]+a[3]-y) < .075)
    def covered(lo, hi, intervals):
        end = lo
        for left, right in intervals:
            if right < end-1e-6:
                continue
            if left > end+1e-6:
                break
            end = max(end, right)
            if end >= hi-1e-6:
                return True
        return False
    for i, enemy in enumerate(room.get('enemies', [])):
        # Authored y is the foot; E1/E2/E3 prefabs use a 1x1 body.
        # Read the actual prefab collider rather than changing enemy tuning.
        kind = enemy.get('type', 'E1').split('-')[0][:2]
        if kind not in ('E1', 'E2', 'E3'):
            findings.append(('MEASURE', 'enemy-shape', f'enemy={i} type={kind} unsupported static body; inspect in Unity'))
            continue
        prefab = Path('Assets/Prefabs')/(kind+'.prefab')
        shape = next(p for p in prefab.read_text(encoding='utf-8-sig').split('--- !u!') if p.startswith('61 '))
        width, height = map(float, re.search(r'm_Size: \{x: ([\d.]+), y: ([\d.]+)\}', shape).groups())
        body = (enemy['x']-width/2, enemy['y']+.01, width, height-.01)
        overlap = [j for j, a in enumerate(solids) if intersects(body, a)]
        if overlap:
            findings.append(('FAIL', 'enemy-inside-solid', f'enemy={i} x={enemy["x"]:g} feet={enemy["y"]:g} solid={overlap}'))
        spans = supported_intervals(enemy['y'])
        if not covered(enemy['x'], enemy['x'], spans):
            findings.append(('FAIL', 'enemy-support', f'enemy={i} x={enemy["x"]:g} feet={enemy["y"]:g} no standing surface'))
        patrol = enemy.get('patrol', [])
        if len(patrol) == 2 and not covered(min(patrol), max(patrol), spans):
            findings.append(('FAIL', 'patrol-support', f'enemy={i} patrol={patrol} feet={enemy["y"]:g} leaves standing surface'))
    # Vertical chains are local geometric candidates, not an input route proof.
    lanterns = sorted((p for p in room.get('pogo', []) if p.get('kind') == 'lantern'), key=lambda p:p['y'])
    chains = []
    for p in lanterns:
        predecessors = [chain for chain in chains if 0 < p['y']-chain[-1]['y'] <= 2+attack_height()+1e-6 and abs(p['x']-chain[-1]['x']) <= 4]
        chain = max(predecessors, key=len, default=[])+[p]
        chains.append(chain)
        if len(chain) > 2:
            findings.append(('WARN', 'pogo-chain', 'consecutive lanterns='+','.join(str(n.get('id', n['x'])) for n in chain)))
    for a in surfaces:
        top = a[1]+a[3]
        nearby = [p for p in room.get('pogo', []) if abs(p['x']-(a[0]+a[2]/2)) <= max(4, a[2]/2+3) and top < p['y']+.5]
        if not nearby:
            continue
        high = max(p['y']+.5 for p in nearby)
        if high-top >= 4 and not any(r[2] <= 2.5 and top < r[1]+r[3] <= high+1e-6 and gap(a,r) <= 4 for r in room.get('platform', [])):
            findings.append(('WARN', 'pogo-only-climb', f'fromTop={top:g} toTop={high:g} rise={high-top:g} no small platform in local climb'))
    return findings


def double_jump_height():
    """Optimistic held-jump envelope, read-only authored tuning (not a route proof)."""
    text = Path('Assets/ScriptableObjects/PlayerTuning.asset').read_text(encoding='utf-8-sig')
    def value(name):
        return float(re.search(r'(?m)^\s*'+name+r':\s*([\d.]+)', text).group(1))
    return (value('jumpVelocity')**2+value('doubleJumpVelocity')**2)/(2*value('gravity'))


def room_capabilities(room_id, rooms):
    # A gate implies ownership only when EVERY actual incoming route requires it.
    # Reverse one-way markers, stubs and absent arrival spawns are not entrances.
    room = rooms[room_id]
    arrivals = {s['id'] for s in room.get('spawns', [])}
    incoming = [d for other in rooms.values() for d in other.get('doors', [])
                if d.get('to', '').split('(')[0] == room_id and d['id'] in arrivals
                and not d.get('stub') and d.get('oneway') != 'in' and not d.get('down_only')]
    required = {incoming[0]['gate']} if len(incoming) == 1 and incoming[0].get('gate') else set()
    # Authored climb surfaces are evaluated as conditional wall-jump routes.
    # This does not claim the player already owns WallJump on first arrival.
    if room.get('climb'):
        required.add('WallJump')
    return required


def approach_checks(room):
    """B23 strict rule: floating target, rise >=1.5u within a normal jump.
    Crumble entries are platforms at author time, solids are never targets.
    """
    findings=[]
    for kind, rects in [('solid', room.get('solid', [])), ('platform', room.get('platform', []))]:
        for i, a in enumerate(rects):
            if a[2] < a[3] or a[1]+a[3] >= room['size'][1]-.5:
                continue
            for j, b in enumerate(room.get('platform', [])):
                rise=b[1]+b[3]-a[1]-a[3]
                if b[1] > a[1]+a[3] and 1.5-1e-6 <= rise <= 2.6+1e-6 and gap(a,b) < 1.8-1e-6:
                    findings.append(('FAIL','approach-headroom',f'from={kind}{i} top={a[1]+a[3]:g} to=platform{j} bottom={b[1]:g} rise={rise:g} gap={gap(a,b):g} minimum=1.8'))
    return findings


def transport_surfaces(room):
    """Virtual endpoints for optimistic lift/steam reachability, not solid geometry."""
    lifts = [[lift['x0'], y, lift['x1']-lift['x0'], 0]
             for lift in room.get('lifts', []) for y in (lift['lo'], lift['hi'])]
    steam = [[jet['x0'], y, jet['x1']-jet['x0'], 0]
             for jet in room.get('steam', []) for y in (jet['y0'], jet['top'])]
    return lifts+steam


def arrival_graph(room, surfaces, capabilities=()):
    terrain = [(r[0], r[1]+r[3], r[2], 0) for r in surfaces]
    steam_begin=len(terrain)-2*len(room.get('steam',[]))
    lift_begin=steam_begin-2*len(room.get('lifts',[]))
    lift_edges={(i,i+1) for i in range(lift_begin,steam_begin,2)}
    lift_edges |= {(b,a) for a,b in list(lift_edges)}
    # Steam only adds a bottom -> top edge. Falling uses the ordinary envelope.
    # This ignores on/off timing, obstacles and real jet velocity; never route proof.
    steam_edges={(i,i+1) for i in range(steam_begin,len(terrain),2)}
    targets = [(p['x']-.5, p['y']+.5, 1, 0) for p in room.get('pogo', [])]
    walls = room.get('climb', []) if 'WallJump' in capabilities else []
    nodes = terrain+targets+[(r[0], r[1]+r[3], r[2], 0) for r in walls]
    result = []
    def explore(seeds, use_pogo):
        reached = set(seeds)
        changed = True
        while changed:
            changed = False
            for i in list(reached):
                a = nodes[i]
                rise = 2+attack_height() if len(terrain) <= i < len(terrain)+len(targets) else 2.5
                if 'DJ' in capabilities:
                    rise = max(rise, double_jump_height())
                for j, b in enumerate(nodes):
                    if j in reached or (len(terrain) <= j < len(terrain)+len(targets) and not use_pogo):
                        continue
                    # Reaching any point near the bottom of an authored climb
                    # face grants its full vertical interval in this envelope.
                    # Wall attachment uses the player's side, not just the foot:
                    # the authored collider can contact a wall above foot level.
                    bottom = walls[j-len(terrain)-len(targets)][1]-player_size()[1] if j >= len(terrain)+len(targets) else b[1]
                    if (i,j) in lift_edges or (i,j) in steam_edges or gap(a, b) <= 3+1e-6 and bottom-a[1] <= rise+1e-6:
                        reached.add(j)
                        changed = True
        return reached
    for spawn in room.get('spawns', []):
        x, y = spawn['x'], spawn['y']
        landing = [(i, r) for i, r in enumerate(terrain) if r[0] <= x <= r[0]+r[2] and r[1] <= y+.1]
        if spawn.get('fall'):
            top = max((r[1] for _, r in landing), default=None)
            seeds = {i for i, r in landing if top is not None and abs(r[1]-top) < 1e-6}
        else:
            seeds = {i for i, r in landing if y-r[1] <= .25}
        ordinary, pogo = explore(seeds, False), explore(seeds, True)
        result.append((spawn, seeds, ordinary, pogo, nodes))
    return result


def v5_checks(room, capabilities=()):
    """Static candidates, never a substitute for Unity physics or natural input."""
    findings = []
    def add(level, rule, detail):
        findings.append((level, rule, detail))
    w, h = room['size']
    title = room.get('title', '')
    hub = '허브' in title
    if hub:
        size_ok = w <= 50 and h <= 24
    elif '겹침' in title or '척추' in title:
        size_ok = 28 <= w <= 40 and 24 <= h <= 36
    elif '갈래' in title or '폐수로' in title:
        size_ok = w <= 60 and h <= 20
    else:
        size_ok = w <= 50 and h <= 16
    if not size_ok:
        add('WARN', 'size', f'{w}x{h} title-derived room class')
    items = [m for m in room.get('marks', []) if any(t in m.get('t', '') for t in ('조각', '사당'))]
    elements = len(items)+len(room.get('enemies', []))+sum(len(room.get(k, [])) for k in KINDS if k != 'enemies')
    if not hub and elements*1000/(w*h) < 3:
        add('WARN', 'density', f'{elements*1000/(w*h):.3f}/1000u2')
    solid, platforms = room.get('solid', []), room.get('platform', [])
    intact = solid+platforms+room.get('seal', [])+room.get('cover', [])+room.get('breakable', [])
    surfaces = [r for r in intact if r[2] > r[3] and r[1]+r[3] < h-1e-6]
    surfaces.extend(transport_surfaces(room))
    if h > 20:
        for name, lo, hi in [('lower', 0, h/2), ('upper', h/2, h)]:
            if not any(lo <= m['y'] < hi for m in items):
                add('WARN', 'floor-item', f'{name} half y=[{lo:g},{hi:g}) has no item mark')
    doors = room.get('doors', [])
    for i, door in enumerate(doors):
        rect = door['rect']
        for j, enemy in enumerate(room.get('enemies', [])):
            xs = [enemy['x']] + enemy.get('patrol', [])
            # Door safety applies to enemies whose occupied vertical band meets the door.
            if enemy['y'] < rect[1]+rect[3] and enemy['y']+1 > rect[1]:
                for x in xs:
                    distance = max(0, rect[0]-x, x-rect[0]-rect[2])
                    if distance < 2:
                        add('FAIL', 'door-safety', f'door={i}:{door["id"]} enemy={j} x={x:g} distance={distance:g}')
        for j, block in enumerate(solid+platforms):
            if intersects(rect, block):
                add('FAIL', 'overlap', f'door={i}:{door["id"]} block={j}')
        if door.get('gate') and door['gate'] not in ('DJ', 'Dash', 'Slam', 'WallJump', 'Boss'):
            add('FAIL', 'ability-gate', f'unknown gate={door["gate"]}')
    for i, platform in enumerate(platforms):
        for j, block in enumerate(solid):
            if intersects(platform, block):
                add('FAIL', 'overlap', f'platform={i} solid={j}')
        top = platform[1]+platform[3]
        lower = [r for r in surfaces if r is not platform and r[1]+r[3] < top-1e-6 and gap(platform, r) <= 3]
        if lower:
            delta = min(top-r[1]-r[3] for r in lower)
            pogo = any(abs(p['x']-(platform[0]+platform[2]/2)) <= platform[2]/2+3 and p['y'] < top for p in room.get('pogo', []))
            limit = max(2, double_jump_height()) if 'DJ' in capabilities else 2
            climb = 'WallJump' in capabilities and any(gap(platform, r) <= 3 and r[1] <= top <= r[1]+r[3]+2.5 for r in room.get('climb', []))
            if delta > limit+1e-6 and not pogo and not climb:
                add('FAIL', 'height', f'platform={i} nearest lower rise={delta:g} (static candidate)')
        else:
            add('WARN', 'horizontal-gap', f'platform={i} no lower surface within 3u')
    tops = sorted({r[1]+r[3] for r in platforms})
    if tops and tops[-1]-tops[0] >= 4 and not room.get('pogo') and not room.get('enemies'):
        add('WARN', 'ascent', '>=4u ascent with platforms only; branch requires human review')
    base_graph = {s['id']: (ordinary, reached) for s, _, ordinary, reached, _ in arrival_graph(room, surfaces)}
    for spawn, seeds, ordinary, reached, nodes in arrival_graph(room, surfaces, capabilities):
        name = spawn['id']
        base_ordinary, base_pogo = base_graph[name]
        add('MEASURE', 'arrival', f'start={name} fall={bool(spawn.get("fall"))} seed={sorted(seeds)} noAbilityOrdinary={len(base_ordinary)} noAbilityPogo={len(base_pogo)} capabilities={",".join(sorted(capabilities)) or "none"} ordinary={len(ordinary)} pogo={len(reached)}')
        if not seeds:
            add('FAIL', 'arrival', f'start={name} no landing surface')
        for i, r in enumerate(nodes[:len(surfaces)]):
            if i not in reached:
                add('FAIL', 'reachability', f'start={name} surface={i} x={r[0]:g} top={r[1]:g} absent in pogo envelope')
            elif i not in ordinary:
                add('MEASURE', 'pogo-route', f'start={name} surface={i} x={r[0]:g} top={r[1]:g}')
        for door in doors:
            if door.get('gate') or door.get('stub') or door.get('oneway') == 'in' or door.get('down_only'):
                continue
            door_rise = double_jump_height() if 'DJ' in capabilities else 2.5
            if not any(i in reached and gap(r, door['rect']) <= 3 and door['rect'][1]-r[1] <= max(door_rise, 2+attack_height() if len(surfaces) <= i < len(surfaces)+len(room.get('pogo', [])) else door_rise) for i, r in enumerate(nodes)):
                add('FAIL', 'reachability', f'start={name} door={door["id"]} absent in pogo envelope')
    findings.extend(draft2_checks(room))
    findings.extend(draft3_checks(room))
    findings.extend(approach_checks(room))
    return [(rule_level(level, rule), rule, detail) for level, rule, detail in findings], elements


def shortcut_checks(room_id, rooms, world):
    """B20: observed A03->A05 bypass, with explicit checker-only lock metadata.

    closed_until_wall_broken on the incoming room door must name an existing
    persistent wall key. This annotation alone does not implement runtime locks.
    The barrier room is determined by actual breakable/seal geometry, not by
    treating the world a/b order as a direction rule.
    """
    findings = []
    for link in world.get('doors', []):
        if link.get('type') != 'shortcut':
            continue
        for destination in (link['a'], link['b']):
            target = rooms.get(destination, {})
            barriers = [(kind, i, r) for kind in ('breakable', 'seal')
                        for i, r in enumerate(target.get(kind, []))]
            if not barriers:
                continue
            origin = link['b'] if destination == link['a'] else link['a']
            entrances = [d for d in rooms.get(origin, {}).get('doors', [])
                         if d['id'] == link['id'] and d.get('to') == destination]
            if room_id == origin:
                keys = {f'V5-{destination}-{kind}{i}' for kind, i, _ in barriers}
                if not entrances or entrances[0].get('closed_until_wall_broken') not in keys:
                    findings.append(('FAIL', 'shortcut-closed',
                                     f'door={link["id"]} from={origin} to={destination} missing valid closed_until_wall_broken key'))
            if room_id != destination:
                continue
            arrival = next((s for s in target.get('spawns', []) if s['id'] == link['id'] and s.get('from') == origin), None)
            door = next((d for d in target.get('doors', []) if d['id'] == link['id']), None)
            if arrival is None or door is None:
                continue  # missing-arrival candidates remain covered by existing checks
            axis = 0 if link.get('edge') == 'x' else 1
            entry = door['rect'][axis] + door['rect'][axis+2]/2
            spawn = arrival['x'] if axis == 0 else arrival['y']
            bypassed = []
            for kind, i, rect in barriers:
                lo, hi = rect[axis], rect[axis] + rect[axis+2]
                across = entry <= lo+1e-6 and spawn >= hi-1e-6 or entry >= hi-1e-6 and spawn <= lo+1e-6
                # Only barriers crossing this arrival corridor, not unrelated walls.
                overlaps = (arrival['y'] < rect[1]+rect[3] and arrival['y']+player_size()[1] > rect[1]) if axis == 0 else (rect[0] <= arrival['x'] <= rect[0]+rect[2])
                if across and overlaps:
                    bypassed.append(f'{kind}{i}')
            if bypassed:
                lock = entrances[0].get('closed_until_wall_broken') if entrances else None
                # Runtime contract is ProgressSave.IsWallBroken, not SealFloor.
                # The key must unlock every barrier actually crossed by this
                # spawn; an unrelated but existing wall key is insufficient.
                locked = all(name.startswith('breakable') and
                             lock == f'V5-{destination}-{name}' for name in bypassed)
                detail = f'door={link["id"]} from={origin} spawn=({arrival["x"]:g},{arrival["y"]:g}) already past {",".join(bypassed)}'
                if locked:
                    detail += f'; 잠금으로 해소 key={lock}; runtime lock NOT VERIFIED'
                findings.append(('MEASURE' if locked else 'FAIL', 'shortcut-spawn-bypass', detail))
    return findings


def trench_measurements(room, max_step):
    """Compare authored lip depth to an explicitly supplied B20 measured limit.

    A geometric escape estimate ignores damage, knockback and head clearance;
    it is deliberately MEASURE and never a new game-rule FAIL.
    """
    if max_step is None:
        return []
    solids = room.get('solid', [])
    candidates = [('trench', i, r[0], r[0]+r[2], r[1]) for i, r in enumerate(room.get('hazard', []))]
    for i, r in enumerate(solids):
        if abs(r[1]) < 1e-6 and r[3] <= .4001 and not any(h[0] < r[0]+r[2] and r[0] < h[0]+h[2] for h in room.get('hazard', [])):
            candidates.append(('pit', i, r[0], r[0]+r[2], r[1]+r[3]))
    rows = []
    for kind, i, left, right, bottom in candidates:
        lips = [r[1]+r[3] for r in solids if abs(r[0]+r[2]-left)<1e-5 or abs(r[0]-right)<1e-5]
        lips = [y for y in lips if y > bottom+1e-6]
        if lips:
            depth = min(lips)-bottom
            escape = 'possible' if depth <= max_step+1e-6 else 'not-in-measured-envelope'
            rows.append(('MEASURE', 'trench-escape', f'{kind}={i} x={left:g}..{right:g} depth={depth:g} measuredMaxStep={max_step:g} noAbilityJumpEscape={escape}; damage/headroom not solved'))
    return rows


def v5_report(source, world_source=None, max_step=None):
    rooms = json.loads(source.read_text(encoding='utf-8-sig'))['rooms']
    world_source = world_source or source.with_name(source.name.replace('rooms_', 'world_', 1))
    world = json.loads(world_source.read_text(encoding='utf-8-sig')) if world_source.is_file() else {}
    lines = ['# V5 층 A 방 규칙 검사', '', f'원본: `{source.as_posix()}` (읽기 전용)', f'SHA256: `{hashlib.sha256(source.read_bytes()).hexdigest()}`', '',
             '정적 기하 검사 후보 결과. FAIL도 설계 확정 판정이 아니며 Unity 물리·적 포고·공중 조작·벽 가림·능력 문 실제 동작은 미검증이다.',
             f'계산: WORLD_V5 3절 점프2.5u·일반 단차2u·포고2u, 기존 CombatTuning downAttackHeight={attack_height():g}u. 포고 상단=y+.5, 포고 뒤 상승 여유=2+타격높이. 등불·가시 둘 다 포함, 가로 간격3u. 일반 경로와 포고 경로를 별도로 계산한다.',
             '모든 spawns를 각각 시작점으로 검사한다. fall은 x 아래 첫 고체/발판/온전한 봉인·덮개·부서지는 벽 상단을 착지점으로 삼는다. 벽 가림·충돌·적 이동·입력 타이밍을 풀지 않는 낙관적 경로 후보이며 실제 문 gate 차단은 T3에서 검사한다.',
             f'B19: noAbilityOrdinary/noAbilityPogo는 능력 없는 계산이며 capabilities 결과와 구분한다. climb 방은 WallJump 보유를 가정한 벽 전체 길이 경로다(첫 도착 소유 주장 아님). 유효 입구가 하나이고 gate가 있으면 그 능력을 요구한다. DJ 높이는 PlayerTuning의 (jumpVelocity²+doubleJumpVelocity²)/(2×gravity)={double_jump_height():g}u를 읽기 전용으로 계산한다. 연속 벽 접촉에는 프리팹 몸 높이를 포함한다. 가로3u·충돌·입력 제한은 여전히 검사기 한계다.',
             '밀도 요소=아이템 marks(조각/사당)+기존 11종. 높이20 초과는 아래·위 절반에 아이템 각1개를 검사한다.', '',
             f'draft2 추가 규칙: 몸체 {player_size()}u를 프리팹에서 읽음. 머리 여유=키+.6u, 방 높이≥12, 도랑 위 바닥 금지는 관찰 버그 근거 FAIL. 구덩이 순찰 교차·폭>3은 WARN. 맞닿은 고체의 내부 면은 서 있는 면에서 제외한다.', '',
             'B23: 승강기 lo↔hi 상면을 양방향 연결한 낙관적 도달 후보. B25: 증기 x0~x1의 y0→top 단방향 상승 연결 추가(주기·실속도·벽 가림 미검증, 가상 끝점은 고체가 아님). 접근 머리 충돌은 떠 있는 목표 발판, 오름1.5~2.6u·간격1.8u 미만 FAIL. 블록 목표 제외. 실제 물리 검증 별도.', '',
             'B19 근거 없는 정적 후보는 WARN: door-safety, overlap, ability-gate, height, arrival, reachability, pit-patrol, pit-width. 후보 출력은 유지하며 실제 플레이 불가능으로 확정하지 않는다.', '',
             'draft3 추가 규칙: 가장자리 가로0.9u 이내 구조물 머리 여유2.2u, 적 솔리드 침투·서 있는 면·순찰 전구간 지지 FAIL. 오름4u 이상에서 작은 발판 없는 포고 구간·등불3연속은 WARN(기하 후보).', '',
             'B21: shortcut 반대편 진입의 closed_until_wall_broken 저장 키가 실제 우회된 breakable과 일치하면 spawn-bypass는 MEASURE(잠금으로 해소). 누락·잘못된 키·봉인 우회는 FAIL을 유지한다. 표시는 검사 계약이며 런타임 잠금 구현을 보장하지 않는다. 도랑·구덩이 탈출은 --max-step-evidence의 실측치와 비교한 MEASURE(피해·머리 충돌 제외). 실측 JSON 미지정 시 trench-escape MEASURE는 미실시.', '',
             '| 방 | 면적 u² | 요소 | /1000u² | 문 | FAIL | WARN | 판정 |', '|---|---:|---:|---:|---:|---:|---:|---|']
    details = []
    for key, room in rooms.items():
        checks, elements = v5_checks(room, room_capabilities(key, rooms))
        checks += shortcut_checks(key, rooms, world)
        checks += trench_measurements(room, max_step)
        area = room['size'][0]*room['size'][1]
        fails = sum(c[0] == 'FAIL' for c in checks)
        warns = sum(c[0] == 'WARN' for c in checks)
        lines.append(f'| {key} | {area:g} | {elements} | {elements*1000/area:.3f} | {len(room.get("doors", []))} | {fails} | {warns} | {"FAIL" if fails else "WARN" if warns else "PASS"} |')
        details.extend(f'{level} {key} {rule}: {detail}' for level, rule, detail in checks)
    return '\n'.join(lines+['', '```text']+details+['```', ''])


def self_test(output_root=Path('Validation/_raw/room-rules-self-test')):
    approach_room={'size':[20,12],'solid':[[0,0,4,1]],'platform':[[5,2.5,2,.5]]}
    assert len(approach_checks(approach_room))==1
    approach_room['platform'][0][0]=5.8
    assert not approach_checks(approach_room), '1.8u boundary'
    approach_room['solid'].append(approach_room['platform'].pop())
    assert not approach_checks(approach_room), 'solid targets excluded'
    lift_room={'lifts':[{'x0':2,'x1':4,'lo':1,'hi':10}], 'spawns':[{'id':'low','x':3,'y':1}]}
    graph=arrival_graph(lift_room,[[0,0,6,1],[2,1,2,0],[2,10,2,0]])
    assert 2 in graph[0][2], 'lift low to high'
    lift_room['spawns'][0]['y']=10
    graph=arrival_graph(lift_room,[[0,0,6,1],[2,1,2,0],[2,10,2,0]])
    assert 1 in graph[0][2], 'lift high to low'
    steam_room={'steam':[{'x0':2,'x1':4,'y0':1,'top':10}],
                'spawns':[{'id':'low','x':1,'y':1}]}
    geometry=[[0,0,6,1],[5,9.5,2,.5]]
    graph=arrival_graph(steam_room,geometry+transport_surfaces(steam_room))
    assert 1 in graph[0][2], 'steam enables high shelf without pogo or wall jump'
    assert 1 not in arrival_graph({'spawns':steam_room['spawns']},geometry)[0][2], 'no jet no high shelf'
    remote={'steam':[{'x0':20,'x1':22,'y0':1,'top':10}], 'spawns':steam_room['spawns']}
    assert 1 not in arrival_graph(remote,geometry+transport_surfaces(remote))[0][2], 'remote jet requires reaching bottom'
    mixed={'lifts':[{'x0':2,'x1':4,'lo':1,'hi':10}],
           'steam':[{'x0':5,'x1':7,'y0':10,'top':20}],
           'spawns':[{'id':'low','x':1,'y':1},{'id':'high','x':3,'y':10}]}
    graph=arrival_graph(mixed,[[0,0,6,1]]+transport_surfaces(mixed))
    assert 4 in graph[0][2] and 1 in graph[1][2], 'mixed steam ascent and bidirectional lift preserve indices'
    run = output_root/uuid.uuid4().hex
    fake = {'title': 'fake', 'size': [20, 12], 'solid': [[0, 0, 20, 1]],
            'platform': [[1, 2, 3, .3], [8, 5, 3, .3]],
            'doors': [{'id': 'fake-door', 'rect': [1, 1, 1, 3]}],
            'enemies': [{'x': 2, 'y': 1, 'patrol': [1, 3]}],
            'spawns': [{'id': 'start', 'x': 5, 'y': 1}]}
    temporary = run/'room-rules-fake.json'
    temporary.parent.mkdir(parents=True, exist_ok=True)
    with temporary.open('x', encoding='utf-8') as out:
        json.dump(fake, out)
    rules = {r[1] for r in v5_checks(json.loads(temporary.read_text(encoding='utf-8')))[0] if r[0] == 'WARN'}
    assert {'door-safety', 'overlap', 'height'} <= rules, rules
    sealed = {'title': 'sealed', 'size': [16, 6], 'solid': [[0, 0, 5, 1], [9, 0, 7, 1]],
              'seal': [[5, 0, 4, 1]], 'spawns': [{'id': 'start', 'x': 2, 'y': 1}]}
    assert not any(rule == 'reachability' and level in ('FAIL', 'WARN') for level, rule, _ in v5_checks(sealed)[0])
    from copy import deepcopy
    base = {'title': 'fake', 'size': [20, 12], 'solid': [[0, 0, 7, 1], [7, 0, 2.5, .4], [9.5, 0, 10.5, 1]], 'platform': []}
    examples = {}
    examples['headroom'] = deepcopy(base)
    examples['headroom']['platform'] = [[1, 2.4, 3, .3]]  # 1.4u above floor.
    examples['room-height'] = deepcopy(base)
    examples['room-height']['size'][1] = 10
    examples['covered-trench'] = deepcopy(base)
    examples['covered-trench']['hazard'] = [[12, .4, 4, .3]]
    examples['pit-patrol'] = deepcopy(base)
    examples['pit-patrol']['enemies'] = [{'x': 5, 'y': 1, 'patrol': [4, 12]}]
    examples['pit-width'] = deepcopy(base)
    examples['pit-width']['solid'][1][2] = 3.5
    folder = run/'draft2'
    folder.mkdir(parents=True)
    results = []
    for rule, room in examples.items():
        path = folder/(rule+'.json')
        path.write_text(json.dumps({'rooms': {'fake': room}}, indent=2), encoding='utf-8')
        failures = draft2_checks(json.loads(path.read_text(encoding='utf-8'))['rooms']['fake'])
        assert any(r == rule for _, r, _ in failures), failures
        results.extend(f'{level} synthetic {r}: {detail}' for level, r, detail in failures)
    assert not draft2_checks(base)
    draft3 = {
        'edge-headroom': {'size':[20,12], 'solid':[[0,0,20,1],[5,1,2.5,2]], 'platform':[[8,4.5,3,.5]]},
        'enemy-inside-solid': {'size':[20,12], 'solid':[[0,0,20,1],[5,1,2,2]], 'enemies':[{'x':6,'y':1,'type':'E1','patrol':[4,8]}]},
        'enemy-support': {'size':[20,12], 'solid':[[0,0,5,1]], 'enemies':[{'x':8,'y':1,'type':'E1','patrol':[3,8]}]},
        'pogo-chain': {'size':[20,16], 'solid':[[0,0,20,1]], 'pogo':[{'id':str(i),'kind':'lantern','x':5+i,'y':3+2*i} for i in range(3)]},
    }
    folder = run/'draft3'
    folder.mkdir(parents=True)
    for rule, room in draft3.items():
        path = folder/(rule+'.json')
        path.write_text(json.dumps({'rooms':{'fake':room}}, indent=2), encoding='utf-8')
        checks = draft3_checks(json.loads(path.read_text(encoding='utf-8'))['rooms']['fake'])
        expected_level = 'WARN' if rule == 'pogo-chain' else 'FAIL'
        assert any(r == rule and level == expected_level for level, r, _ in checks), (rule, checks)
        if rule == 'enemy-support':
            assert any(r == 'patrol-support' for _, r, _ in checks), checks
        results.extend(f'{level} synthetic {r}: {detail}' for level, r, detail in checks)
    assert not draft3_checks(base)
    spike_only = deepcopy(draft3['pogo-chain'])
    for target in spike_only['pogo']:
        target['kind'] = 'spike'
    assert any(rule == 'pogo-only-climb' for _, rule, _ in draft3_checks(spike_only))
    results.append('PASS spike-only rise>=4 climb without small platform detected; lantern chain remains lantern-only')
    fixture = json.loads(Path('Tools/fixtures/B20-shortcut.json').read_text(encoding='utf-8'))
    def shortcut_rows(data):
        return [row for key in data['rooms'] for row in shortcut_checks(key, data['rooms'], data['world'])]
    assert not shortcut_rows(fixture)
    missing = deepcopy(fixture)
    del missing['rooms']['A03']['doors'][0]['closed_until_wall_broken']
    assert [r[1] for r in shortcut_rows(missing)] == ['shortcut-closed']
    bypass = deepcopy(fixture)
    bypass['rooms']['A05']['spawns'][0]['x'] = 3.5
    assert [(r[0], r[1]) for r in shortcut_rows(bypass)] == [('MEASURE', 'shortcut-spawn-bypass')]
    bad_key = deepcopy(fixture)
    bad_key['rooms']['A03']['doors'][0]['closed_until_wall_broken'] = 'missing-wall-key'
    assert [r[1] for r in shortcut_rows(bad_key)] == ['shortcut-closed']
    swapped = deepcopy(fixture)
    swapped['world']['doors'][0].update(a='A03', b='A05')
    assert not shortcut_rows(swapped)
    seal = deepcopy(bypass)
    seal['rooms']['A05']['seal'] = seal['rooms']['A05'].pop('breakable')
    seal['rooms']['A03']['doors'][0]['closed_until_wall_broken'] = 'V5-A05-seal0'
    assert [r[1] for r in shortcut_rows(seal)] == ['shortcut-spawn-bypass']
    results.append('PASS B20 fixed shortcut fixture FAIL=0; missing lock/bad key/seal detected; valid locked bypass is MEASURE; a/b order independent')
    fixture21 = json.loads(Path('Tools/fixtures/B21-shortcut.json').read_text(encoding='utf-8'))
    assert [(r[0], r[1]) for r in shortcut_rows(fixture21)] == [('MEASURE', 'shortcut-spawn-bypass')]
    for name, key in [('unlocked', None), ('bad-key', 'missing-wall-key'),
                      ('unrelated-key', 'V5-A05-breakable1')]:
        broken = deepcopy(fixture21)
        entrance = broken['rooms']['A03']['doors'][0]
        if key is None:
            del entrance['closed_until_wall_broken']
        else:
            entrance['closed_until_wall_broken'] = key
        if name == 'unrelated-key':
            broken['rooms']['A05']['breakable'].append([8, 1, 1, 3])
        rows = shortcut_rows(broken)
        assert ('FAIL', 'shortcut-spawn-bypass') in [(r[0], r[1]) for r in rows], rows
        results.extend(f'{level} B21-fixture-{name} {rule}: {detail}' for level, rule, detail in rows)
    swapped21 = deepcopy(fixture21)
    swapped21['world']['doors'][0].update(a='A03', b='A05')
    assert shortcut_rows(swapped21) == shortcut_rows(fixture21)
    results.extend(f'{level} B21-fixture-locked {rule}: {detail}' for level, rule, detail in shortcut_rows(fixture21))
    results.extend(f'{level} B21-fixture-seal {rule}: {detail}' for level, rule, detail in shortcut_rows(seal))
    results.append('PASS B21 locked spawn bypass MEASURE; unlocked/bad-key/unrelated-key/seal bypass FAIL; a/b order independent')
    return '\n'.join(['PASS self-test synthetic door-safety / overlap / height WARN detected; intact seal traversal retained', *results, 'PASS B23 approach threshold/solid exclusion and bidirectional lift fixtures', 'PASS B25 steam bottom-to-top / absent and remote jet rejection / mixed lift-steam fixtures', 'PASS draft2 five synthetic rules detected; draft3 four JSON fixtures detected; clean fixture PASS'])


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("source", nargs="?", type=Path, default=Path("Design/rooms_v2.json"))
    parser.add_argument("--output", type=Path)
    parser.add_argument("--v5", action="store_true")
    parser.add_argument("--self-test", action="store_true")
    parser.add_argument("--self-test-output-dir", type=Path, default=Path('Validation/_raw/room-rules-self-test'), help="Root for unique self-test fixture output")
    parser.add_argument("--world", type=Path, help="World JSON used for shortcut barrier checks")
    parser.add_argument("--max-step-evidence", type=Path, help="B20 measured JSON containing maxStep; no hardcoded substitute")
    args = parser.parse_args()
    max_step = json.loads(args.max_step_evidence.read_text(encoding='utf-8-sig'))['maxStep'] if args.max_step_evidence else None
    text = v5_report(args.source, args.world, max_step) if args.v5 or args.source.name.startswith('rooms_v5') else report(args.source)
    if args.self_test:
        text += '\n'+self_test(args.self_test_output_dir)+'\n'
    if args.output:
        with args.output.open("x", encoding="utf-8", newline="\n") as output:
            output.write(text)
        print(f"Saved {args.output}")
    else:
        print(text)
