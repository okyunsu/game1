"""Summarize human play CSVs without changing the source recordings."""
import argparse
import csv
import re
from collections import defaultdict, deque
from pathlib import Path


def summarize(path):
    totals = defaultdict(float)
    windows = defaultdict(deque)
    loops = []
    with path.open(encoding="utf-8-sig", newline="") as stream:
        for row in csv.DictReader(stream):
            if row["event"] != "room_exit":
                continue
            details = row["details"]
            target = re.search(r"(?:^|;)to=([^;]+)", details)
            duration = re.search(r"room_active_seconds=([0-9.]+)", details)
            if duration:
                totals[row["room"]] += float(duration[1])
            if not target:
                continue
            pair = tuple(sorted((row["room"], target[1])))
            now = float(row["elapsed_active"])
            window = windows[pair]
            window.append((now, row["room"], target[1]))
            while window and now - window[0][0] > 3:
                window.popleft()
            # Three full round trips = six alternating directed crossings.
            if len(window) >= 6:
                tail = list(window)[-6:]
                if all(a[1] == b[2] and a[2] == b[1] for a, b in zip(tail, tail[1:])):
                    if not loops or loops[-1][0] != pair or tail[0][0] > loops[-1][2] + 3:
                        loops.append([pair, tail[0][0], now])
                    else:
                        loops[-1][2] = now
    return totals, loops


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("source", nargs="?", default="Playtest")
    parser.add_argument("--output", required=True)
    args = parser.parse_args()
    source, output = Path(args.source), Path(args.output)
    if output.exists():
        parser.error("Preserve existing evidence: choose a new output path")
    paths = sorted(source.glob("*.csv")) if source.is_dir() else [source]
    rows = ["# 사람 플레이 CSV 요약", "", "체류 시간은 기록된 room_exit의 활성 시간 합계이며 종료 직전 열린 방은 포함하지 않는다.", ""]
    for path in paths:
        totals, loops = summarize(path)
        rows += [f"## {path.name}", "", "| 방 | 체류 시간(s) |", "|---|---:|"]
        rows += [f"| {room} | {seconds:.3f} |" for room, seconds in sorted(totals.items())]
        rows += [""]
        rows += [f"전환 루프 의심: {pair[0]}↔{pair[1]} / {start:.3f}–{end:.3f}s / 3초 창 안 3회 이상 왕복" for pair, start, end in loops]
        if not loops:
            rows += ["전환 루프 의심 없음."]
        rows += [""]
    output.parent.mkdir(parents=True, exist_ok=True)
    output.write_text("\n".join(rows), encoding="utf-8")


if __name__ == "__main__":
    main()
