# B07 T21 S01 / REVIEW
2026-10-11 08:06–08:38 KST. 40×42u candidate bounds; core upper climb x16..40 (24u), top plaza spans40u with a safe entry opening34..40 and floor0..34/top39. No enemy/discovery. S01 PogoTarget instance removes its inherited KillZone component: this room is intentionally nonlethal; original target prefab and other rooms untouched.
Five beats: introduction landing9/top3/width6; DJ16/top6.6/width4; marked wall20.25/bottom8/top20; rest25/top20/width6 → Dash → Pogo31/y20.5 → onward34/top23; final stairs29/35 top25..35/width3; vertical DJ through opening then plaza32→central trigger20/y40.
Approach1 initially blocked at wall completion, head-safe staircase correction did not fix wall-end input; approach2 opens plaza entry (base record/replay PASS, Short still fails wall-end landing); approach3 explicitly settles on wall top and releases jump, with two corrections (restore safe original rest height and remove target hazard). Final base/Short record and fixed replay PASS/Error0. No retry remains.
```text
EVIDENCE S01-main-a3-record.txt
FAIL B07RoomVerifier actual=exe
MEASURE route seconds=90.0704113 ticks=4501 stage=19 pos=(27.97, 37.46) pogos=1 wallJumps=6 hp=4 doubleJump=True
FAIL System.Exception: S01 main actual input record
Error/Exception/Assert=0
EVIDENCE S01-main-a5-record.txt
FAIL B07RoomVerifier actual=exe
MEASURE route seconds=9.1184214 ticks=453 stage=5 pos=(29.74, 20.04) pogos=0 wallJumps=6 hp=0 doubleJump=True
FAIL System.Exception: S01 main actual input record
Error/Exception/Assert=0
EVIDENCE S01-main-a6-short.txt
FAIL B07RoomVerifier actual=exe
MEASURE route seconds=60.0711903 ticks=3001 stage=6 pos=(31.05, 3.58) pogos=0 wallJumps=6 hp=4 doubleJump=True
FAIL System.Exception: S01 main actual input record
Error/Exception/Assert=0
EVIDENCE S01-main-a7-base-replay.txt
MEASURE route seconds=20.4457634 ticks=918 stage=0 pos=(0.00, 0.00) pogos=0 wallJumps=0 hp=0 doubleJump=False
PASS S01 main actual input replay
Error/Exception/Assert=0
EVIDENCE S01-main-a7-short-replay.txt
MEASURE route seconds=19.9124552 ticks=890 stage=0 pos=(0.00, 0.00) pogos=0 wallJumps=0 hp=0 doubleJump=False
PASS S01 main actual input replay
Error/Exception/Assert=0
Pogo sequence log:
Pogo sequence=1 vy=11
```
After Ending unloads the player, old final counters display0/HP0; this is not a death. Success observes EndingMenu, and frame death/respawn checking remained active. Actual input JSON retains all jump/dash/down-attack actions. Full-room overview refreshed/inspected in T27. Empty summit atmosphere and human rhythm remain REVIEW.
