# B07 T3 — A01

REVIEW. 2026-10-10 22:20–22:46 KST. 72u safe opening, sealed decorative gate, CP-A01, different low landings, Double Jump revisit ledge→discovery→A06, A02 mandatory exit. Original unskinned block/player/door prefabs; no enemy/fall pit. Source Scene and actual chosen values in LEVEL_V3.

Actual new Development60fps: required route record PASS11.31s and replay PASS11.35s; optional discovery record PASS6.29s and replay PASS6.31s; no Double Jump15s attempts fail entry/discovery as intended PASS. Error/Exception/Assert0. Full room PNG saved and visually inspected. Build errors0/warnings0.

```text
MEASURE A01 dimensions=(72.00, 16.00) fallPits=0 enemies=0 route=main short=False
MEASURE route seconds=11.3470143 ticks=562 stage=0 pos=(3.61, 1.81) pogos=0
MEASURE route seconds=6.3130423 ticks=312 stage=0 pos=(38.93, 6.20) pogos=0
MEASURE route seconds=15.0538677 ticks=750 stage=1 pos=(39.08, 1.82) pogos=0
```

Initial optional a1 input tool tried walking through a low step (x10.64), corrected with low-obstacle jump approach. a2 reached A06 before collecting discovery, actual candidate geometry flaw; moved reward39 and door42 once, a3 record/replay PASS. Initial raw FAIL messages preserved:
`FAIL System.Exception: A01 optional actual input record`

[Main replay](../../Validation/_raw/B07/A01-main-replay-a3.txt), [optional replay](../../Validation/_raw/B07/A01-optional-replay-a3.txt), [negative gate](../../Validation/_raw/B07/A01-no-doublejump-a3.txt), [overview](../../Validation/_raw/B07/A01-overview.png). Raw attempts and input JSON retained. Human feel/natural time and all-combination gate audit not run here (T22/T24/user REVIEW). A06 remains foundation until its room task; destination exists. New opt-in itinerary/record-replay utility reads Scene metadata and injects real fixed-tick input; normal gameplay uses none of it. No teleport during a route, ability setup only at independent fixture start.
