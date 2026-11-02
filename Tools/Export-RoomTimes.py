"""Summarize human PlaytestLogger CSVs without Unity or gameplay changes."""
import argparse
import csv
import math
import re
from pathlib import Path


def number(value, label):
    result = float(value)
    if not math.isfinite(result) or result < 0:
        raise ValueError(f'{label} must be finite and nonnegative')
    return result


def room_times(source):
    totals = {}
    current = None
    last_active = 0.0

    def finish(active, complete, details=''):
        nonlocal current
        room, entered = current
        seconds = active-entered
        measured = re.search(r'(?:^|;)room_active_seconds=([^;]+)', details)
        if measured:
            seconds = number(measured[1], 'room_active_seconds')
        if seconds < -0.00001:
            raise ValueError('room duration is negative')
        item = totals.setdefault(room, {'visits': 0, 'completed_visits': 0,
                                       'incomplete_visits': 0, 'active_seconds': 0.0})
        item['visits'] += 1
        item['completed_visits' if complete else 'incomplete_visits'] += 1
        item['active_seconds'] += max(0, seconds)
        current = None

    with source.open(encoding='utf-8-sig', newline='') as stream:
        reader = csv.DictReader(stream)
        required = {'event', 'room', 'elapsed_active', 'details'}
        if not required.issubset(reader.fieldnames or []):
            raise ValueError('expected a PlaytestLogger CSV with event,room,elapsed_active,details')
        for row in reader:
            active = number(row['elapsed_active'], 'elapsed_active')
            if active < last_active:
                raise ValueError(f'elapsed_active goes backwards at line {reader.line_num}')
            last_active = active
            event, room = row['event'], row['room']
            if event == 'room_enter':
                if not room:
                    raise ValueError(f'empty room at line {reader.line_num}')
                if current is not None:
                    # Missing exit: the time to this entry is an estimate, flagged incomplete.
                    finish(active, False)
                current = (room, active)
            elif event in ('room_exit', 'summary_current_room'):
                if current is None or current[0] != room:
                    raise ValueError(f'unmatched {event} at line {reader.line_num}')
                finish(active, True, row['details'])
        if current is not None:
            # A crash or unfinished recording only proves time up to its last row.
            finish(last_active, False)
    if not totals:
        raise ValueError('no room visits found')
    return totals


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('source', type=Path, help='human PlaytestLogger CSV (one session)')
    parser.add_argument('--output', type=Path,
                        help='new CSV; default Playtest/<source-stem>-room-times.csv')
    args = parser.parse_args()
    output = args.output or Path(__file__).resolve().parent.parent/'Playtest'/f'{args.source.stem}-room-times.csv'
    try:
        totals = room_times(args.source)
        output.parent.mkdir(parents=True, exist_ok=True)
        # Exclusive creation protects the source and all existing measurement results.
        with output.open('x', encoding='utf-8-sig', newline='') as stream:
            writer = csv.writer(stream)
            writer.writerow(['source', 'room', 'visits', 'completed_visits',
                             'incomplete_visits', 'active_seconds'])
            for room, item in sorted(totals.items()):
                writer.writerow([args.source.name, room, item['visits'], item['completed_visits'],
                                 item['incomplete_visits'], f"{item['active_seconds']:.6f}"])
        print(f'PASS rooms={len(totals)} visits={sum(t["visits"] for t in totals.values())} output={output}')
    except (OSError, ValueError, TypeError, csv.Error) as error:
        parser.exit(1, f'FAIL {error}\n')


if __name__ == '__main__':
    main()
