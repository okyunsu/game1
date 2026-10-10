# B07 T17 — C03 / PARTIAL
2026-10-11 00:39–00:47 KST. 72×20u,5 beats,one3u pit34..37. E1×2 (one allowed additional E1 for continuous pogo),E2×1,E3×1 at24/28/43/52. Marked wall5.75/bottom1/top9; C05 entry3.5/top8/width2 with doorway3.5/y9; return9/top8/width4. G-J67/top4.6/width10 (3.6u above floor) and C04 doorway70/y5.8. Combat bypass44/top3/width8,cover49, Pogo reward32/top3.7/width4. Bypass ends48,14u before G-J; marked wall isolated at far left.

Attempt a1 main base2.9686952s and Short2.4992817s PASS; revisit immediately reentered overlapping C05 exit and optional also entered C05. Approach a2 separated entry and return; entry landing underside blocked climb, revisit died jumping under optional landing. Approach a3 narrowed landing to2u for head clearance; Short record PASS, base still collides after longer horizontal kick, revisit retains takeoff ceiling issue. Same-cause a4 clearance/launch adjustment was rejected by automatic approval review for retry-limit exhaustion. Unexecuted source adjustments reverted; a3 Scene preserved. No bypass or further positive route attempts made.

B07-C03-a3 build errors0/warnings0. Actual final evidence (raw C03-* preserved):
```
MEASURE route seconds=35.069074 ticks=1751 stage=1 pos=(5.14, 1.82) pogos=0 wallJumps=14 hp=4
FAIL System.Exception: C03 main actual input record
MEASURE route seconds=2.1328983 ticks=101 stage=2 pos=(3.47, 1.81) pogos=0 wallJumps=3 hp=4
PASS C03 main actual input record
MEASURE route seconds=6.700196 ticks=312 stage=2 pos=(36.72, -2.68) pogos=0 wallJumps=0 hp=0
FAIL System.Exception: C03 revisit actual input record
MEASURE route seconds=35.0537574 ticks=1750 stage=1 pos=(5.14, 1.82) pogos=0 wallJumps=16 hp=4
FAIL System.Exception: C03 optional actual input record
```
All final runs Error/Exception/Assert=0 (FAIL assertions are verifier Check exceptions captured in result, not Unity errors). Full-room C03-overview.png inspected. Fixed replays and optional discovery not performed/completed. Final source useful but not accepted for continuous traversal. Further correction/execution needs explicit retry approval. Suggested concrete correction: move left landing to2/top8/width2 and doorway2/y9, narrow pogo landing to2u then take off near33.5 beyond its underside; not applied. C05 and other independent rooms continue.
