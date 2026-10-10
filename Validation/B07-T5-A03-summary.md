# B07 T5 — A03 / REVIEW
2026-10-10 23:03–23:16 KST. 60×18u hub, CP-A03, preserved B06 signs, A02/A04 links, elevated G-D→B01, lower shared directional A05 shortcut, FromPit arrival at30/12. Enemies0, lethal pits0; lower tunnel has safe floor y-3 and return step top-1.

Final checkpoint deck center29.25/top2.5/width10.5 ends34.5. G-D approaches center38/top4.5/width4,41/top6/width3,42/top7.5/width3. Deck49/top7.5/width8, lattice44/9 size1×3; separator above10.5. Deck end creates clearance for ground passage underneath. First three main records failed (a1 gate, a2 gate, a3 underside). Inputs and logs preserved; a4 main, dash, return all record and replay PASS/Error0.
```
MEASURE route seconds=9.9305208 ticks=491 stage=0 pos=(3.61, 1.81) pogos=0
PASS A03 main actual input replay
MEASURE route seconds=4.897011 ticks=240 stage=0 pos=(3.61, 1.81) pogos=0
PASS A03 dash actual input replay
MEASURE route seconds=4.6474143 ticks=228 stage=0 pos=(25.97, 13.81) pogos=0
PASS A03 return actual input replay
```
Build B07-A03-a4: Development exit0. Final screenshot Validation/_raw/B07/A03-overview.png inspected. Raw A03-*-a4 files plus earlier attempts preserved. Tests use isolated save paths, fixture start only; traversal uses actual keyboard ticks.

Not yet tested: no-Dash combinations and directional wall denial (T22); continuous full route (T24). Human hub feel/sign readability remain REVIEW. V3RoomRoutes adds WalkUnder at end of enum for explicit walk-through route; normal gameplay has no controller behavior change.