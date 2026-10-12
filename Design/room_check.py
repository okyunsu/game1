"""Room interior checker + renderer for 메트로 1 (Design data, no Unity needed).

Coordinates: room-local units, origin = room rect lower-left in world_v6.json.
Physics from PlayerTuning defaults (preset 1): moveSpeed 6, jumpVelocity 12,
gravity 30 (x1.5 when falling), player 0.7 x 1.6.
"""
import json, math, sys
import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from matplotlib import font_manager
from matplotlib.patches import Rectangle

for f in font_manager.findSystemFonts():
    if "NotoSansCJK" in f and "Regular" in f:
        font_manager.fontManager.addfont(f)
        plt.rcParams["font.family"] = font_manager.FontProperties(fname=f).get_name()
        break

SPEED, JUMP_V, G, FALL_MUL = 6.0, 12.0, 30.0, 1.5
POGO_V = 11.0   # CombatTuning.pogoVelocity (repo value)
RUN_JUMP_MEASURED = 4.56   # B09-T1: running jump, same height
DASH_JUMP_MEASURED = 5.81  # B09-T1: dash jump distance
PW, PH = 0.7, 1.6
PEAK = JUMP_V ** 2 / (2 * G)            # 2.4u
SAFE = 0.8                              # project rule: required <= 80% of max
HEAD = 2.2                              # required headroom above a standing surface
ARRIVE_IN, ARRIVE_FREE = 2.5, 3.0


def reach(rise, peak=PEAK):
    """Horizontal distance covered when landing at +rise (center travel)."""
    if rise > peak:
        return None
    t_up = math.sqrt(2 * peak / G)
    t_down = math.sqrt(2 * (peak - rise) / (G * FALL_MUL))
    return SPEED * (t_up + t_down)


def solids(room):
    out = []
    for kind in ("solid", "platform", "climb", "seal", "cover", "breakable", "pad"):
        for i, r in enumerate(room.get(kind, [])):
            out.append((kind, i, r))
    return out


def surfaces(room):
    """Standing surfaces: floor/platform tops (x0, x1, top, id)."""
    s = []
    for kind in ("solid", "platform", "pad"):
        for i, (x, y, w, h) in enumerate(room.get(kind, [])):
            if w >= 1.0:
                s.append((x, x + w, y + h, f"{kind}{i}"))
    return s


def check(room):
    errs, notes = [], []
    routes = room.get("routes") or {"main": room.get("route", [])}
    W, H = room["size"]
    # 1) headroom above every standing surface
    for x0, x1, top, sid in surfaces(room):
        for kind, i, (x, y, w, h) in solids(room):
            if f"{kind}{i}" == sid:
                continue
            if x < x1 and x + w > x0 and top + PH <= y < top + HEAD:
                errs.append(f"HEADROOM {sid}(top {top}) under {kind}{i} bottom {y} gap {y-top:.2f} (walkable but bumps head)")
    # 2) required route steps: rise and gap within 80% of max
    for rname, route in routes.items():
     for a, b in zip(route, route[1:]):
         a = a.split("@")[0]
         b, _, ab = b.partition("@")
         if a.split("~")[0] == b.split("~")[0]:
             continue   # walking along the same floor piece
         pa, pb = lookup(room, a), lookup(room, b)
         rise = pb[2] - pa[2]
         gap = max(pb[0] - pa[1], pa[0] - pb[1], 0)
         if ab == "lift":
             continue
         if ab == "steam":
             cols = [s for s in room.get("steam", []) if pa[0] < s["x1"] and s["x0"] < pa[1]]
             if not cols:
                 errs.append(f"[{rname}] STEAM {a} is not over a vent")
             elif not any(pb[0] - 1 <= s["x1"] and s["x0"] <= pb[1] + 1 and pb[2] <= s["top"] - .5 for s in cols):
                 errs.append(f"[{rname}] STEAM {a}->{b} target not beside/below the column top")
             continue
         if ab == "slam":
             if rise > SLAM_RISE * SAFE + 1e-6 or gap > reach(0) * SAFE:
                 errs.append(f"[{rname}] SLAM {a}->{b} rise {rise:.2f} gap {gap:.2f} exceeds {SLAM_RISE*SAFE:.2f}/{reach(0)*SAFE:.2f}")
             if rise <= DJ_REAL + .2:
                 errs.append(f"[{rname}] GATE-LEAK slam step {a}->{b} rise {rise:.2f} is reachable with double jump")
             continue
         if ab:
             cap = {"dash": (PEAK * SAFE, min((reach(max(rise, 0)) or 0) * SAFE + 18 * .16 * .9, DASH_JUMP_MEASURED * .95 if rise <= .5 else 99)),
                    "dj": (2 * PEAK * SAFE, (reach(max(rise - PEAK, 0)) or 0) * 1.6 * SAFE),
                    "wj": (99, 99)}[ab]
             if rise > cap[0] + 1e-6 or gap > cap[1] + 1e-6:
                 errs.append(f"[{rname}] {ab.upper()} {a}->{b} rise {rise:.2f} gap {gap:.2f} exceeds {cap[0]:.2f}/{cap[1]:.2f}")
             # gate leak: an ability step must be impossible without the ability (real physics + body width + 0.2 margin)
             real = SPEED * (math.sqrt(2 * PEAK / G) + math.sqrt(2 * max(PEAK - rise, 0) / (G * FALL_MUL))) + PW + .2
             if rise <= 0:
                 real = min(real, RUN_JUMP_MEASURED + PW)   # B09-T1 measured flat running jump
             if ab == "dash" and gap <= real and rise <= PEAK:
                 errs.append(f"[{rname}] GATE-LEAK dash step {a}->{b} gap {gap:.2f} is jumpable without dash (needs > {real:.2f})")
             if ab == "dj" and room.get("gate_check", {}).get(rname) and rise <= PEAK + .2 and gap <= real:
                 errs.append(f"[{rname}] GATE-LEAK dj step {a}->{b} is reachable without double jump")
             continue
         peak = POGO_V ** 2 / (2 * G) if a.startswith(("pogo:", "enemy:")) else PEAK
         if rise > peak * SAFE + 1e-6:
             errs.append(f"[{rname}] RISE {a}->{b} {rise:.2f} > {peak*SAFE:.2f}")
             continue
         lim = (reach(max(rise, 0), peak) or 0) * SAFE
         if rise < 0:
             lim = SPEED * 1.2  # dropping down: generous
         if gap > lim + 1e-6:
             errs.append(f"[{rname}] GAP {a}->{b} gap {gap:.2f} rise {rise:.2f} > limit {lim:.2f}")
         ok, how = simulate(room, a, b)
         if not ok:
             errs.append(f"[{rname}] ARC {a}->{b} no collision-free jump found ({how})")
    # 2b) no 4+ identical jumps in a row (78-platform incident rule)
    for rname, route in routes.items():
     sig = []
     for a, b in zip(route, route[1:]):
         pa, pb = lookup(room, a.split("@")[0]), lookup(room, b.split("@")[0])
         sig.append((pb[2] - pa[2], max(pb[0] - pa[1], pa[0] - pb[1], 0), pb[1] - pb[0]))
     run = 1
     for i in range(1, len(sig)):
         same = abs(sig[i][0] - sig[i-1][0]) < .15 and abs(sig[i][1] - sig[i-1][1]) < .3 and abs(sig[i][2] - sig[i-1][2]) < .5
         run = run + 1 if same else 1
         if run == 4:
             errs.append(f"REPEAT 4 identical jumps ending at {route[i+1].split('@')[0]} (rise {sig[i][0]:.1f}, gap {sig[i][1]:.1f}, width {sig[i][2]:.1f})")
    # 3) required landing width
    for pid in {p for r in routes.values() for p in r if ":" not in p.split("@")[0]}:
        p = lookup(room, pid.split("@")[0])
        if p[1] - p[0] < 2.0 - 1e-6:
            errs.append(f"NARROW {pid} width {p[1]-p[0]:.2f} < 2")
    # 4) arrivals: inside >=2.5 from door, on floor, free floor >=3 in travel dir
    for sp in room.get("spawns", []):
        x, y, d = sp["x"], sp["y"], sp["dir"]
        if sp.get("fall"):
            below = [s for s in surfaces(room) if s[0] <= x <= s[1] and s[2] < y]
            if not below:
                errs.append(f"SPAWN {sp['id']} falls into nothing")
            else:
                land = max(below, key=lambda s: s[2])
                if land[1] - land[0] < 3:
                    errs.append(f"SPAWN {sp['id']} lands on narrow {land[3]} ({land[1]-land[0]:.1f}u)")
            continue
        under = [s for s in surfaces(room) if s[0] <= x - PW / 2 and x + PW / 2 <= s[1] and abs(s[2] - y) < 1e-6]
        if not under:
            errs.append(f"SPAWN {sp['id']} not on a surface")
            continue
        s = under[0]
        free = (s[1] - x) if d > 0 else (x - s[0]) if d < 0 else max(s[1] - x, x - s[0])
        door = sp.get("door_x")
        if door is not None and abs(x - door) < ARRIVE_IN:
            errs.append(f"SPAWN {sp['id']} only {abs(x-door):.1f}u from door")
        if free < ARRIVE_FREE:
            errs.append(f"SPAWN {sp['id']} free floor {free:.1f} < {ARRIVE_FREE} in travel direction")
    # 4b) every door must be used by a route (side: route surface at door bottom by the wall; ceiling: ceil: node; floor: route surface touching the hole)
    used = {p.split("@")[0].split("~")[0] for r in routes.values() for p in r}
    surf = {s[3]: s for s in surfaces(room)}
    for d in room.get("doors", []):
        if d.get("passive"):
            continue
        x, y, w, h = d["rect"]
        did = d.get("id", d["to"])
        if w == 1:   # side door
            ok = any(p in surf and abs(surf[p][2] - y) < .05 and (surf[p][0] <= 1.01 if x == 0 else surf[p][1] >= W - 1.01) for p in used)
        elif y == H - 1:
            ok = f"ceil:{did}" in used or d.get("down_only")
        else:
            ok = any(p in surf and surf[p][2] < 3 and (abs(surf[p][1] - x) < .05 or abs(surf[p][0] - (x + w)) < .05) for p in used)
        if not ok:
            errs.append(f"DOOR {did} not reached by any route")
    # 4c) lift shafts and steam columns must be clear
    for l in room.get("lifts", []):
        for kind, i, (x, y, w, h) in solids(room):
            if x < l["x1"] and x + w > l["x0"] and y + h > l["lo"] + .01 and y < l["hi"] + PH:
                errs.append(f"LIFT {l['id']} shaft blocked by {kind}{i}")
    for s in room.get("steam", []):
        for kind, i, (x, y, w, h) in solids(room):
            if x < s["x1"] and x + w > s["x0"] and y + h > s["y0"] + .01 and y < s["top"]:
                errs.append(f"STEAM {s['id']} column blocked by {kind}{i}")
    # 5) chimneys: inner width
    for c in room.get("chimneys", []):
        if abs(c["inner"] - 4) > 1e-6:
            errs.append(f"CHIMNEY {c['id']} inner {c['inner']} != 4")
    return errs, notes


def simulate(room, a, b, dt=1/240):
    """Try real jump arcs from surface a to b. Player holds toward b, releases once over b."""
    pa, pb = lookup(room, a), lookup(room, b)
    obstacles = [r for k, i, r in solids(room)]
    direction = 1 if (pb[0] + pb[1]) / 2 >= (pa[0] + pa[1]) / 2 else -1
    starts = [pa[0] + PW / 2 + k * .25 for k in range(int((pa[1] - pa[0] - PW) / .25) + 1)]
    lo, hi = pb[0] + PW / 2 + .15, pb[1] - PW / 2 - .15
    if pb[2] < pa[2]:   # dropping down: the landing spot must be clear of the source surface
        right = (max(lo, pa[1] + PW / 2 + .05), hi)
        left = (lo, min(hi, pa[0] - PW / 2 - .05))
        lo, hi = max((right, left), key=lambda r: r[1] - r[0])
        if hi < lo:
            return False, "no landing spot beside the source"
    last = "no takeoff"
    v0 = POGO_V if a.startswith(("pogo:", "enemy:")) else JUMP_V
    for sx in starts:
        x, y, vy, t = sx, pa[2], v0, 0.0
        while t < 2.5:
            t += dt
            vx = 0 if lo <= x <= hi else SPEED * (1 if x < lo else -1)
            g = G * (FALL_MUL if vy <= 0 else 1)
            vy = max(vy - g * dt, -18)
            nx, ny = x + vx * dt, y + vy * dt
            if vy < 0 and y >= pb[2] >= ny and pb[0] - PW / 2 < nx < pb[1] + PW / 2:
                return True, f"takeoff x={sx:.2f}"
            hit = next(((ox, oy) for (ox, oy, ow, oh) in obstacles
                        if nx - PW / 2 < ox + ow and nx + PW / 2 > ox and ny < oy + oh and ny + PH > oy), None)
            if hit:
                last = f"blocked at x={nx:.1f} y={ny:.1f}"
                break
            x, y = nx, ny
            if y < min(pa[2], pb[2]) - 4:
                last = "fell short"
                break
    return False, last


SLAM_RISE = 7.5          # slam pad bounce height (only reacts to a slam landing)
DJ_REAL = 2 * PEAK       # best rise with double jump


def lookup(room, pid):
    if pid.startswith("pogo:"):   # lantern / spike: feet height at the bounce
        t = next(t for t in room["pogo"] if t["id"] == pid[5:])
        return (t["x"] - .6, t["x"] + .6, t["y"] + .6)
    if pid.startswith("enemy:"):  # pogo on a (slow) flying enemy
        e = next(e for e in room["enemies"] if e.get("id") == pid[6:])
        return (e["x"] - .6, e["x"] + .6, e["y"] + 1.6)
    if pid.startswith("lift:"):
        _, lid, end = pid.split(":")
        l = next(l for l in room["lifts"] if l["id"] == lid)
        return (l["x0"], l["x1"], l[end])
    if pid.startswith("ceil:"):
        d = next(d for d in room["doors"] if d.get("id") == pid[5:])
        x, y, w, h = d["rect"]
        return (x, x + w, y - PH + .6)   # feet height where the head is 0.6u into the hole
    pid, _, near = pid.partition("~")
    kind = "".join(ch for ch in pid if ch.isalpha())
    idx = int("".join(ch for ch in pid if ch.isdigit()))
    x, y, w, h = room[kind][idx]
    if near:   # floor piece clipped to +-3u around the named x
        n = float(near)
        return (max(x, n - 3), min(x + w, n + 3), y + h)
    return (x, x + w, y + h)


def render(rid, room, path):
    W, H = room["size"]
    scale = 0.32
    fig, ax = plt.subplots(figsize=(max(6, W * scale), max(4, H * scale) + 0.8), dpi=100)
    fig.patch.set_facecolor("#14191d"); ax.set_facecolor("#1c2328")
    col = {"solid": "#6f8796", "platform": "#9fb4c2", "climb": "#00e5ff", "seal": "#b07cff", "cover": "#c9a26b", "breakable": "#c9a26b"}
    for (x, y, w, h) in room.get("hazard", []):
        ax.add_patch(Rectangle((x, y), w, h, fc="#ff3b3b", ec="none", alpha=.8))
    for s in room.get("steam", []):
        ax.add_patch(Rectangle((s["x0"], s["y0"]), s["x1"] - s["x0"], s["top"] - s["y0"], fc="#cfefff", ec="#cfefff", alpha=.18, ls=":"))
        ax.text((s["x0"] + s["x1"]) / 2, s["top"] + .3, "증기", ha="center", fontsize=7, color="#cfefff")
    for l in room.get("lifts", []):
        ax.add_patch(Rectangle((l["x0"], l["lo"]), l["x1"] - l["x0"], l["hi"] - l["lo"], fc="none", ec="#ffd23f", ls="--", lw=1))
        ax.add_patch(Rectangle((l["x0"], l["lo"] - .4), l["x1"] - l["x0"], .4, fc="#ffd23f", ec="none"))
        lx, ly = l["lever"]
        ax.plot(lx, ly + .6, marker="^", ms=8, color="#ffd23f"); ax.text(lx, ly + 1.4, "레버", ha="center", fontsize=6, color="#ffd23f")
    for t in room.get("pogo", []):
        ax.plot(t["x"], t["y"] - .4, marker="D", ms=7, color="#ff9f43" if t.get("kind") != "spike" else "#ff3b3b")
        if t.get("kind") != "spike":
            ax.plot([t["x"], t["x"]], [t["y"], room["size"][1] - 1], color="#666", lw=.6)
    crumble = set(room.get("crumble", []))
    for kind, i, (x, y, w, h) in solids(room):
        fc = "#ff9f43" if kind == "platform" and i in crumble else ("#ff7eb6" if kind == "pad" else col.get(kind, "#999"))
        ax.add_patch(Rectangle((x, y), w, h, fc=fc, ec="#111", lw=.5))
        if kind == "platform":
            ax.text(x + w / 2, y + h + .25, f"p{i}", ha="center", fontsize=6, color="#ddd")
    for d in room.get("doors", []):
        x, y, w, h = d["rect"]
        ax.add_patch(Rectangle((x, y), w, h, fc="none", ec="#ffffff", lw=1.5, ls="--"))
        ax.text(x + w / 2, y + h + .4, d["to"], ha="center", fontsize=7, color="#fff")
    for sp in room.get("spawns", []):
        ax.add_patch(Rectangle((sp["x"] - PW / 2, sp["y"]), PW, PH, fc="#f3c35a", ec="none"))
        ax.annotate("", (sp["x"] + 1.6 * sp["dir"], sp["y"] + .8), (sp["x"], sp["y"] + .8),
                    arrowprops=dict(arrowstyle="->", color="#f3c35a"))
    for e in room.get("enemies", []):
        ax.plot(e["x"], e["y"] + .5, marker="o" if not e.get("id") else "v", ms=7, color="#ff5a5a")
        ax.text(e["x"], e["y"] + 1.3, e["type"], ha="center", fontsize=6, color="#ff9a9a")
    for m in room.get("marks", []):
        ax.plot(m["x"], m["y"], marker=m.get("m", "*"), ms=10, color=m.get("c", "#ffd23f"))
        ax.text(m["x"], m["y"] + 1, m["t"], ha="center", fontsize=7, color=m.get("c", "#ffd23f"))
    rcol = ["#7CFC9A", "#ffa94d", "#74c0fc", "#f783ac"]
    for k, (rn, route) in enumerate((room.get("routes") or {"main": room.get("route", [])}).items()):
        rt = [lookup(room, p.split("@")[0]) for p in route]
        if not rt: continue
        ax.plot([(a + b) / 2 for a, b, _ in rt], [t + .9 + .25 * k for _, _, t in rt], color=rcol[k % 4], lw=1.2, alpha=.85, label=rn)
    if room.get("routes"):
        ax.legend(loc="upper right", fontsize=6, facecolor="#222", labelcolor="#eee")
    ax.set_xlim(-1, W + 1); ax.set_ylim(-1, H + 2); ax.set_aspect("equal")
    ax.set_title(f"{rid} — {room['title']}", color="#eee", fontsize=10)
    ax.tick_params(colors="#666", labelsize=6); [s.set_color("#333") for s in ax.spines.values()]
    plt.tight_layout(); plt.savefig(path, facecolor=fig.get_facecolor()); plt.close(fig)


if __name__ == "__main__":
    data = json.load(open(sys.argv[1], encoding="utf-8"))
    outdir = sys.argv[2]
    total = 0
    for rid, room in data["rooms"].items():
        errs, notes = check(room)
        total += len(errs)
        print(f"== {rid}: {'PASS' if not errs else str(len(errs)) + ' issue(s)'}")
        for e in errs:
            print("   " + e)
        render(rid, room, f"{outdir}/{rid}.png")
    print(f"max jump {PEAK:.2f}u, required rise <= {PEAK*SAFE:.2f}u, flat gap limit {reach(0)*SAFE:.2f}u")
    sys.exit(1 if total else 0)
