# B07 T13 — B05 / REVIEW
2026-10-11 00:07–00:23 KST. 24×64u, two 4u inner-width chimneys, no enemies/lethal falls. Lower walls7.75/12.25 top34 (left starts3/right1), rests9/top17,33/width2. Upper walls17.75/22.25, left39..64/right36.5..63.5, rests19/top49,61/width2. Rest heights16/32/48 above initial floor. Closed-gate reverse window4/25/4×6 noncolliding, approved sign. Pogo targets16/34.7 and4/34.7; forward landing20/top36/width4, optional3/top36.5/width4 and discovery3/37.3.

Initial main performed14 wall jumps and one pogo but missed forward landing; Short missed first rest. Failure excerpts retained:
```
MEASURE route seconds=80.0621204 ticks=4003 stage=7 pos=(20.11, 4.25) pogos=1 wallJumps=14 hp=4
FAIL System.Exception: B05 main actual input record
MEASURE route seconds=80.072393 ticks=4001 stage=2 pos=(8.97, 1.82) pogos=0 wallJumps=6 hp=4
FAIL System.Exception: B05 main actual input record
```
Raised lower wall tops32→34 and route transfer heights18.6→19.2/34.2→35.2 to give rest landing margin. Lowered forward landing36.5→36; still3u above33 rest, above single jump reach, requiring pogo. Waits for ground contact on rests and starts air dash from airborne transition if needed; fixture only, no motor/tuning changes.

Final Development B07-B05-a2 exit0. Actual record and fixed replay main base/Short and optional PASS/Error0:
```
MEASURE route seconds=20.6144635 ticks=1025 stage=0 pos=(3.61, 1.81) pogos=1 wallJumps=26 hp=4
PASS B05 main actual input replay
MEASURE route seconds=16.9964984 ticks=845 stage=0 pos=(3.61, 1.81) pogos=1 wallJumps=23 hp=4
PASS B05 main actual input replay
MEASURE route seconds=12.7974325 ticks=637 stage=0 pos=(3.31, 37.48) pogos=1 wallJumps=16 hp=4
PASS B05 optional actual input replay
MEASURE route seconds=15.0584426 ticks=751 stage=1 pos=(11.63, 1.82) pogos=0 wallJumps=0 hp=4
PASS ability gate denies optional route
```
Double jump false in all positive route fixtures; main base recording contains1 actual C dash tick. Short runtime clone3.5/.06 restored in Cleanup. Full-room screenshot Validation/_raw/B07/B05-overview.png inspected. Raw B05-* logs/inputs preserved.

Human combination rhythm/rest readability REVIEW. Entire world T24 separate and A02-dependent. Original tuning/default/V2 assets preserved.