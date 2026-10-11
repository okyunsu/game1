import json, sys
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

W = json.load(open(sys.argv[1], encoding="utf-8"))
rooms, doors = W["rooms"], W["doors"]
errors = []

# 1) rooms must not overlap (touching edges allowed)
ids = list(rooms)
for i, a in enumerate(ids):
    ax0, ay0, ax1, ay1 = rooms[a]["rect"]
    for b in ids[i + 1:]:
        bx0, by0, bx1, by1 = rooms[b]["rect"]
        if ax0 < bx1 and bx0 < ax1 and ay0 < by1 and by0 < ay1:
            errors.append(f"OVERLAP {a} {b}")

# 2) each door must lie on a shared edge of both rooms, inside both spans
def on_edge(r, edge, at, s0, s1):
    x0, y0, x1, y1 = r
    if edge == "x":
        return at in (x0, x1) and y0 <= s0 and s1 <= y1
    return at in (y0, y1) and x0 <= s0 and s1 <= x1

for d in doors:
    if d["edge"] == "elevator": continue
    for k in ("a", "b"):
        if not on_edge(rooms[d[k]]["rect"], d["edge"], d["at"], *d["span"]):
            errors.append(f"DOOR-NOT-ON-EDGE {d['id']} room={d[k]}")
    ra, rb = rooms[d["a"]]["rect"], rooms[d["b"]]["rect"]
    if d["edge"] == "x" and not ((ra[2] == d["at"] == rb[0]) or (ra[0] == d["at"] == rb[2])):
        errors.append(f"DOOR-SIDE-MISMATCH {d['id']}")
    if d["edge"] == "y" and not ((ra[3] == d["at"] == rb[1]) or (ra[1] == d["at"] == rb[3])):
        errors.append(f"DOOR-SIDE-MISMATCH {d['id']}")

# 3) every room reachable in the door graph
adj = {r: set() for r in rooms}
for d in doors:
    adj[d["a"]].add(d["b"]); adj[d["b"]].add(d["a"])
seen, stack = set(), ["A01"]
while stack:
    r = stack.pop()
    if r in seen: continue
    seen.add(r); stack += adj[r] - seen
for r in rooms:
    if r not in seen: errors.append(f"UNREACHABLE {r}")

print("\n".join(errors) if errors else "PASS: no overlaps, every door on a shared edge of both rooms, all rooms connected")

# render
colors = {"A": "#7A93A6", "B": "#C07A3E", "C": "#A9C7DA", "S": "#EDEDED", "Z": "#5B4A6E", "T": "#FFE9A8"}
fig, ax = plt.subplots(figsize=(13, 10.4), dpi=110)
fig.patch.set_facecolor("#14191d"); ax.set_facecolor("#14191d")
for rid, r in rooms.items():
    x0, y0, x1, y1 = r["rect"]
    ax.add_patch(Rectangle((x0, y0), x1 - x0, y1 - y0, fc=colors[r["layer"]], ec="#14191d", lw=2, alpha=.9))
    ax.text((x0 + x1) / 2, (y0 + y1) / 2 + 2, rid, ha="center", va="center", fontsize=11, weight="bold", color="#111", zorder=5)
    ax.text((x0 + x1) / 2, (y0 + y1) / 2 - 3, r["role"].split("(")[0], ha="center", va="center", fontsize=7.5, color="#222", zorder=5)
dc = {"both": "#ffffff", "down": "#ff5a5a", "shortcut": "#5aff8a"}
for fid, f in W.get("features", {}).items():
    x0, y0, x1, y1 = f["rect"]
    ax.add_patch(Rectangle((x0, y0), x1 - x0, y1 - y0, fc="#ffd23f", ec="#14191d", lw=1, alpha=.85, zorder=3))
    ax.text(x1 + 1.5, y0 + 4, "엘리베이터", color="#ffd23f", fontsize=8, weight="bold", zorder=4)
for d in doors:
    if d["edge"] == "elevator": continue
    s0, s1 = d["span"]
    if d["edge"] == "x":
        ax.plot([d["at"], d["at"]], [s0, s1], color=dc[d["type"]], lw=5, solid_capstyle="butt")
        lx, ly = d["at"], s1 + 1.5
    else:
        ax.plot([s0, s1], [d["at"], d["at"]], color=dc[d["type"]], lw=5, solid_capstyle="butt")
        lx, ly = s1 + 1.5, d["at"] + 1.5
    if d.get("gate"):
        ax.text(lx, ly, d["gate"], fontsize=7, color="#ffd23f", weight="bold")
ax.plot([], [], color="#fff", lw=5, label="문(양방향)")
ax.plot([], [], color="#ff5a5a", lw=5, label="한 방향 낙하")
ax.plot([], [], color="#5aff8a", lw=5, label="숏컷(안쪽에서만 열림)")
ax.text(0, -27, "노란 글씨 = 능력 게이트 / A 회색 = 최하층, B 주황 = 중층, C 하늘 = 상층, S 흰색 = 거짓 정상, T 노랑 = 진짜 정상, Z 보라 = 지하", color="#ddd", fontsize=9)
ax.legend(loc="upper left", facecolor="#222", labelcolor="#eee", fontsize=9)
ax.set_xlim(-5, 330); ax.set_ylim(-30, 232); ax.set_aspect("equal")
ax.tick_params(colors="#777"); [s.set_color("#333") for s in ax.spines.values()]
plt.tight_layout(); plt.savefig(sys.argv[2], facecolor=fig.get_facecolor())
