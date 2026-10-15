"""Read-only room measurements; no design verdicts or source edits."""
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


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("source", nargs="?", type=Path, default=Path("Design/rooms_v2.json"))
    parser.add_argument("--output", type=Path)
    args = parser.parse_args()
    text = report(args.source)
    if args.output:
        with args.output.open("x", encoding="utf-8", newline="\n") as output:
            output.write(text)
        print(f"Saved {args.output}")
    else:
        print(text)
