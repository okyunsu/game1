"""Build room interiors (local coords) for world_v7. Doors/arrivals come from the world map."""
import json

W = json.load(open("world_v7.json", encoding="utf-8"))
ROOMS = W["rooms"]


def P(x0, x1, top, th=.5):
    return [x0, top - th, x1 - x0, th]


def doors_for(rid):
    x0, y0, x1, y1 = ROOMS[rid]["rect"]
    w, h = x1 - x0, y1 - y0
    out, spawns = [], []
    for d in W["doors"]:
        if rid not in (d["a"], d["b"]) or d["edge"] == "elevator":
            continue
        other = d["b"] if d["a"] == rid else d["a"]
        s0, s1 = d["span"]
        arrive = d["type"] in ("both", "shortcut") or (d["type"] == "down" and d["b"] == rid)
        if d["edge"] == "x":
            lx = 0 if d["at"] == x0 else w
            ly0, ly1 = s0 - y0, s1 - y0
            out.append({"rect": [lx - (0 if lx == 0 else 1), ly0, 1, ly1 - ly0], "to": other, "id": d["id"]})
            if d.get("gate"): out[-1]["gate"] = d["gate"]
            if arrive:
                spawns.append({"id": d["id"], "from": other, "x": 3.5 if lx == 0 else w - 3.5, "y": ly0,
                               "dir": 1 if lx == 0 else -1, "door_x": .5 if lx == 0 else w - .5})
        else:
            top = d["at"] == y1
            lx0, lx1 = s0 - x0, s1 - x0
            out.append({"rect": [lx0, h - 1 if top else 0, lx1 - lx0, 1], "to": other, "id": d["id"]})
            if d.get("gate"): out[-1]["gate"] = d["gate"]
            if top and d["type"] == "down" and d["b"] == rid:
                out[-1]["down_only"] = True
            if arrive and not top:  # arriving from below through a floor hole: stand beside it
                right = lx1 + 2.5 + .35
                if right + 3 <= w - 1:
                    spawns.append({"id": d["id"], "from": other, "x": right, "y": 1, "dir": 1, "door_x": lx1 - .1})
                else:
                    spawns.append({"id": d["id"], "from": other, "x": lx0 - 2.85, "y": 1, "dir": -1, "door_x": lx0 + .1})
            elif arrive and top:  # arriving from above: fall in through the ceiling hole
                spawns.append({"id": d["id"], "from": other, "x": (lx0 + lx1) / 2, "y": h - 1 - 1.7, "dir": 0, "fall": True})
    return out, spawns, [w, h]


def shell(w, h, doors, floor_top=1, holes=()):
    """Walls/floor/ceiling with gaps where doors are."""
    s = []
    # floor
    gaps = sorted([(d["rect"][0], d["rect"][0] + d["rect"][2]) for d in doors if d["rect"][1] == 0 and d["rect"][3] == 1] + list(holes))
    x = 0
    for g0, g1 in gaps:
        if g0 > x: s.append([x, 0, g0 - x, floor_top])
        x = g1
    if x < w: s.append([x, 0, w - x, floor_top])
    # ceiling
    gaps = sorted((d["rect"][0], d["rect"][0] + d["rect"][2]) for d in doors if d["rect"][1] == h - 1 and d["rect"][3] == 1)
    x = 0
    for g0, g1 in gaps:
        if g0 > x: s.append([x, h - 1, g0 - x, 1])
        x = g1
    if x < w: s.append([x, h - 1, w - x, 1])
    for side, wx in (("L", 0), ("R", w - 1)):
        gaps = sorted((d["rect"][1], d["rect"][1] + d["rect"][3]) for d in doors
                      if d["rect"][2] == 1 and d["rect"][0] == wx and d["rect"][3] > 1)
        y = floor_top
        for g0, g1 in gaps:
            if g0 > y: s.append([wx, y, 1, g0 - y])
            y = g1
        if y < h - 1: s.append([wx, y, 1, h - 1 - y])
    return s


R = {}


def _named(items):
    names, rects = {}, []
    for i, it in enumerate(items):
        if isinstance(it, tuple):
            names[it[0]] = i
            it = it[1]
        rects.append(list(it))
    return names, rects


def room(rid, title, **kw):
    doors, spawns, size = doors_for(rid)
    doors += kw.pop("extra_doors", [])
    r = {"title": title, "size": size, "doors": doors, "spawns": spawns}
    ft = kw.pop("floor_top", 1)
    base = shell(size[0], size[1], doors, ft, kw.pop("holes", ()))
    xnames, xs = _named(kw.pop("extra_solid", []))
    r["solid"] = base + xs
    pnames, r["platform"] = _named(kw.pop("platform", []))
    for k in ("climb", "seal", "cover", "breakable", "hazard", "enemies", "marks", "chimneys", "pad", "pogo", "lifts", "steam"):
        r[k] = kw.pop(k, [])
    r["crumble"] = [pnames[n] for n in kw.pop("crumble", [])]

    def tok(t):
        name, _, ab = t.partition("@")
        if name.startswith("F:"):
            x = float(name[2:])
            i = next(i for i, (a, y, w, h) in enumerate(base) if y == 0 and h == ft and a <= x <= a + w)
            name = f"solid{i}~{x:g}"
        elif name.startswith("X:"):
            k = name[2:]
            name = f"solid{len(base) + (xnames[k] if k in xnames else int(k))}"
        elif name.startswith("p:"):
            k = name[2:]
            name = f"platform{pnames[k] if k in pnames else int(k)}"
        return name + ("@" + ab if ab else "")
    r["routes"] = {k: [tok(t) for t in v] for k, v in kw.pop("routes").items()}
    r["notes"] = kw.pop("notes", [])
    r["gate_check"] = kw.pop("gate_check", {})
    r["gates"] = kw.pop("gates", {})
    drop = set(kw.pop("no_spawn", []))
    fixes = kw.pop("spawn_fix", {})
    r["spawns"] = [s for s in r["spawns"] if s["id"] not in drop] + kw.pop("spawns", [])
    for s in r["spawns"]:
        s.update(fixes.get(s["id"], {}))
    assert not kw, f"{rid}: unknown keys {list(kw)}"
    R[rid] = r
    return r


def sid(i): return f"platform{i}"


def ids(r, kind):
    return [f"{kind}{i}" for i in range(len(r[kind]))]
