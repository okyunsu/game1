# B07 T12 — B04 / REVIEW
2026-10-10 23:58–2026-10-11 00:07 KST. 40×30u, no enemies/lethal falls. Actual WallJumpPickup10/1.8 acquired in main route starting without wall jump. Practice walls17.75/22.25, thickness0.5: inner4u, top17, left starts3/right starts1. Summit31/top17/width18 extends to exit38/y18, directional wall34/y18.5/h3 Side.Left shares V3-shortcut-B04; B01 destination69/y8.81. Roof bottom20 blocks walking over wall.

Optional one-sided marked wall5.75/y6/0.5×6 starts3/top9, discovery8/y9.8 on6..10/top9. Ground route passes underneath. Optional fixture has wall jump; no-wall fixture does not cross pickup10 and performs zero wall jumps.

First base/Short records climbed and broke wall but fell after narrow summit; extended summit floor to exit. Failure excerpts preserved:
```
MEASURE route seconds=65.0590301 ticks=3245 stage=6 pos=(38.10, 1.82) pogos=0 wallJumps=7 hp=4
FAIL System.Exception: B04 main actual input record
MEASURE route seconds=65.0567082 ticks=3240 stage=6 pos=(38.05, 1.82) pogos=0 wallJumps=6 hp=4
FAIL System.Exception: B04 main actual input record
```
Final Development B07-B04-a2 exit0. Actual records and fixed replays all PASS/Error0:
```
MEASURE route seconds=11.431285 ticks=556 stage=0 pos=(69.11, 8.81) pogos=0 wallJumps=7 hp=4
PASS B04 main actual input replay
MEASURE route seconds=10.6988616 ticks=520 stage=0 pos=(69.11, 8.81) pogos=0 wallJumps=7 hp=4
PASS B04 main actual input replay
MEASURE route seconds=2.8134579 ticks=138 stage=0 pos=(7.67, 9.82) pogos=0 wallJumps=3 hp=4
PASS B04 optional actual input replay
MEASURE route seconds=25.0583061 ticks=1250 stage=1 pos=(5.97, 1.82) pogos=0 wallJumps=0 hp=4
PASS ability gate denies optional route
```
Negative actually ran25s (first command-line limit wins), not15s. Short only runtime clone3.5/.06; restored Asset in Cleanup. Full-room screenshot Validation/_raw/B07/B04-overview.png inspected. Raw B04-* preserved.

Human wall jump/Short feel REVIEW. Additional gate combinations and locked-side wall T22, full route T24 not performed here. No original tuning/default/V2 changes.