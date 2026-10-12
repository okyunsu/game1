# B07 T18 C05 / REVIEW
2026-10-11 00:47–00:51 interrupted; resumed08:00–08:27 KST. 48×30u,5 beats,one 3u pit29..32,zero enemies. Actual DJ pickup12/y1.8; practice22/top4.6/width8. Summit silhouette40 is noncolliding. Optional wall16.25/bottom7/top13; rest20/top13/width3,launch24/top13/width3,Pogo31/y13.7,reward38/top17.6/width4. Main takeoff17 WalkUnder prevents automatic obstacle jump before the intended double-jump action.
Prior a1 main PASS/optional FAIL; a2 optional PASS/main FAIL. Same approach takeoff corrections r3/r4 failed; r5 uses explicit no-auto-jump approach and actual acquisition succeeds. Fixed replay first consumed its final tick before the asynchronous room transition; neutral20-tick tail only (no movement/ability/placement) corrects verifier completion boundary.
Release builds errors0/warnings0. Final main base/Short record and fixed replay PASS, optional base/Short fixed replay PASS. Full-room C05-overview.png inspected.
```text
EVIDENCE C05-main-base-a2.txt
FAIL B07RoomVerifier actual=exe
MEASURE route seconds=35.0546713 ticks=1754 stage=2 pos=(22.07, 1.82) pogos=0 wallJumps=0 hp=4
FAIL System.Exception: C05 main actual input record
Error/Exception/Assert=0
EVIDENCE C05-main-r4-record.txt
FAIL B07RoomVerifier actual=exe
MEASURE route seconds=45.0704453 ticks=2251 stage=2 pos=(22.07, 1.88) pogos=0 wallJumps=0 hp=4 doubleJump=True
FAIL System.Exception: C05 main actual input record
Error/Exception/Assert=0
EVIDENCE C05-main-r5-replay.txt
FAIL B07RoomVerifier actual=exe
MEASURE route seconds=7.8965003 ticks=393 stage=0 pos=(9.00, 8.81) pogos=0 wallJumps=0 hp=4 doubleJump=True
FAIL System.Exception: C05 main actual input replay
Error/Exception/Assert=0
EVIDENCE C05-main-r5-replay-final.txt
MEASURE route seconds=7.9799874 ticks=393 stage=0 pos=(9.00, 8.81) pogos=0 wallJumps=0 hp=4 doubleJump=True
PASS C05 main actual input replay
Error/Exception/Assert=0
EVIDENCE C05-main-r5-short-replay.txt
MEASURE route seconds=7.9140203 ticks=391 stage=0 pos=(8.89, 8.81) pogos=0 wallJumps=0 hp=4 doubleJump=True
PASS C05 main actual input replay
Error/Exception/Assert=0
EVIDENCE C05-optional-optional-replay.txt
MEASURE route seconds=11.4738217 ticks=571 stage=0 pos=(37.72, 18.42) pogos=1 wallJumps=5 hp=4 doubleJump=True
PASS C05 optional actual input replay
Error/Exception/Assert=0
EVIDENCE C05-optional-optional-short-replay.txt
MEASURE route seconds=10.0415211 ticks=499 stage=0 pos=(37.77, 18.42) pogos=1 wallJumps=3 hp=4 doubleJump=True
PASS C05 optional actual input replay
Error/Exception/Assert=0
```
Raw inputs/logs/images retained. Continuous acquisition checked separately by T24. Human difficulty and summit prospect remain REVIEW.
