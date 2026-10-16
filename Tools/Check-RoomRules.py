"""Read-only room measurements and opt-in V5 draft geometry checks."""
import argparse
import hashlib
import json
from pathlib import Path

KINDS = {
    "enemies": "적", "hazard": "가시", "pad": "내려찍기 발판",
    "steam": "증기", "lifts": "승강기", "crumble": "무너지는 발판",
    "breakable": "부서지는 벽", "seal": "봉인", "cover": "덮개",
    "climb": "벽 점프 벽", "pogo": "포고",
}


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


def v5_checks(room):
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
        size_ok = 30 <= w <= 40 and 24 <= h <= 30
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
    surfaces = [r for r in solid+platforms if r[2] > r[3] and r[1]+r[3] < h-1e-6]
    if h > 20:
        for level in sorted({r[1]+r[3] for r in surfaces}):
            if not any(level <= m['y'] <= level+2 for m in items):
                add('WARN', 'floor-item', f'top={level:g} no item mark within 2u')
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
        above = [r[1]-top for r in solid+platforms if r is not platform and gap(platform, r) == 0 and r[1] >= top-1e-6]
        if above and min(above) < 2:
            add('WARN', 'headroom', f'platform={i} clearance={min(above):g}')
        lower = [r for r in surfaces if r is not platform and r[1]+r[3] < top-1e-6 and gap(platform, r) <= 3]
        if lower:
            delta = min(top-r[1]-r[3] for r in lower)
            pogo = any(abs(p['x']-(platform[0]+platform[2]/2)) <= platform[2]/2+3 and p['y'] < top for p in room.get('pogo', []))
            if delta > 2+1e-6 and not pogo:
                add('FAIL', 'height', f'platform={i} nearest lower rise={delta:g} (static candidate)')
        else:
            add('WARN', 'horizontal-gap', f'platform={i} no lower surface within 3u')
    tops = sorted({r[1]+r[3] for r in platforms})
    if tops and tops[-1]-tops[0] >= 4 and not room.get('pogo') and not room.get('enemies'):
        add('WARN', 'ascent', '>=4u ascent with platforms only; branch requires human review')
    # Conservative over-approximation: permit falls and 2u rises, plus authored pogo nodes.
    nodes = [(r[0], r[1]+r[3], r[2], 0) for r in surfaces]
    nodes += [(p['x']-.5, p['y']+.5, 1, 0) for p in room.get('pogo', [])]
    start = next((s for s in room.get('spawns', []) if s['id'] == 'start'), None)
    if start:
        reached = {i for i, r in enumerate(nodes) if r[0] <= start['x'] <= r[0]+r[2] and -0.1 <= start['y']-r[1] <= 2}
        changed = True
        while changed:
            changed = False
            for i, a in enumerate(nodes):
                if i not in reached:
                    continue
                for j, b in enumerate(nodes):
                    if j not in reached and gap(a, b) <= 3 and b[1]-a[1] <= 2:
                        reached.add(j)
                        changed = True
        for i, r in enumerate(nodes[:len(surfaces)]):
            if i not in reached:
                add('FAIL', 'reachability', f'surface={i} x={r[0]:g} top={r[1]:g} absent in 2u/3u static graph')
        for d in doors:
            if d.get('gate') or d.get('stub') or d.get('oneway') == 'in' or d.get('down_only'):
                continue
            if not any(i in reached and gap(r, d['rect']) <= 3 and -0.1 <= d['rect'][1]-r[1] <= 2 for i, r in enumerate(nodes)):
                add('FAIL', 'reachability', f'door={d["id"]} absent in static graph')
    else:
        add('WARN', 'reachability', 'no start mark; cross-room arrival reachability unverified')
    return findings, elements


def v5_report(source):
    rooms = json.loads(source.read_text(encoding='utf-8-sig'))['rooms']
    lines = ['# V5 층 A 방 규칙 검사', '', f'원본: `{source.as_posix()}` (읽기 전용)', '',
             '정적 기하 검사 후보 결과. FAIL도 설계 확정 판정이 아니며 Unity 물리·적 포고·공중 조작·벽 가림·능력 문 실제 동작은 미검증이다.',
             '높이·도달성은 일반 점프 2u/가로 3u의 보수적 그래프. 포고 노드는 높이 y+.5이며 타격 높이·관성은 모델링하지 않는다. 시작 없는 방은 도달성 WARN. 능력 문 검사는 알려진 gate 이름만 확인하며 실제 차단은 T3에서 확인해야 한다.',
             '밀도 요소=아이템 marks(조각/사당)+기존 11종. 높이20 초과 층은 수평 상단별 아이템 marks를 검사한다.', '',
             '| 방 | 면적 u² | 요소 | /1000u² | 문 | FAIL | WARN | 판정 |', '|---|---:|---:|---:|---:|---:|---:|---|']
    details = []
    for key, room in rooms.items():
        checks, elements = v5_checks(room)
        area = room['size'][0]*room['size'][1]
        fails = sum(c[0] == 'FAIL' for c in checks)
        warns = len(checks)-fails
        lines.append(f'| {key} | {area:g} | {elements} | {elements*1000/area:.3f} | {len(room.get("doors", []))} | {fails} | {warns} | {"FAIL" if fails else "WARN" if warns else "PASS"} |')
        details.extend(f'{level} {key} {rule}: {detail}' for level, rule, detail in checks)
    return '\n'.join(lines+['', '```text']+details+['```', ''])


def self_test():
    fake = {'title': 'fake', 'size': [20, 12], 'solid': [[0, 0, 20, 1]],
            'platform': [[1, 2, 3, .3], [8, 5, 3, .3]],
            'doors': [{'id': 'fake-door', 'rect': [1, 1, 1, 3]}],
            'enemies': [{'x': 2, 'y': 1, 'patrol': [1, 3]}],
            'spawns': [{'id': 'start', 'x': 5, 'y': 1}]}
    temporary = Path('Validation/_raw/B16/room-rules-fake.json')
    temporary.parent.mkdir(parents=True, exist_ok=True)
    with temporary.open('x', encoding='utf-8') as out:
        json.dump(fake, out)
    rules = {r[1] for r in v5_checks(json.loads(temporary.read_text(encoding='utf-8')))[0] if r[0] == 'FAIL'}
    assert {'door-safety', 'overlap', 'height'} <= rules, rules
    return 'PASS self-test synthetic door-safety / overlap / height detected'


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("source", nargs="?", type=Path, default=Path("Design/rooms_v2.json"))
    parser.add_argument("--output", type=Path)
    parser.add_argument("--v5", action="store_true")
    parser.add_argument("--self-test", action="store_true")
    args = parser.parse_args()
    text = v5_report(args.source) if args.v5 else report(args.source)
    if args.self_test:
        text += '\n'+self_test()+'\n'
    if args.output:
        with args.output.open("x", encoding="utf-8", newline="\n") as output:
            output.write(text)
        print(f"Saved {args.output}")
    else:
        print(text)
