# B07 T9 — B01 / REVIEW
2026-10-10 23:33–23:42 KST. 84×20u hub: CP-B01 x20, three approved signs, solid decorative upper gate42/8.5/6×7 on forecourt top5, three noncolliding people, 4u safe-bottom downward pit34..38→A03 FromPit, east B02, shared B04 shortcut. No enemies/lethal pits.

B04 entry69/8.81, landing66..78/top8. Wall74/9.5/h3 Side.Right denies attacks from B01 approach on left, shares V3-shortcut-B04 saved ID. B04 return door80/10.5 avoids accidentally turning around on main ground path. Intended access marked wall55.75/y5.5/0.5×5, starts3 above floor so main ground route remains open; steps60/3.5,63/5,66/6.5 width4. Marked wall leaves a2 return safe drop gap before floor.

First main waited in air and first return met access wall; raw failures preserved. Corrected main DashAir continuation and moved access wall2u left. Final Development B07-B01-a2 exit0. Actual record and fixed replay both routes PASS/Error0:
```
MEASURE route seconds=12.7133107 ticks=630 stage=0 pos=(3.61, 1.81) pogos=0
PASS B01 main actual input replay
MEASURE route seconds=6.3479496 ticks=312 stage=0 pos=(29.89, 11.89) pogos=0
PASS B01 return actual input replay
```
Failure excerpts:
```
MEASURE route seconds=30.0577069 ticks=1502 stage=1 pos=(39.95, 3.31) pogos=0
FAIL System.Exception: B01 main actual input record
MEASURE route seconds=30.0537489 ticks=1500 stage=1 pos=(58.36, 4.32) pogos=0
FAIL System.Exception: B01 return actual input record
```
Screenshot Validation/_raw/B07/B01-overview.png inspected. Raw B01-* preserved. Return fixture starts FromB04, actual input then falls through downward exit; no traversal placement.

Not performed: reverse-pit and locked-side wall combinations T22, human finding pit REVIEW. Existing player tuning/default/V2 assets unchanged.