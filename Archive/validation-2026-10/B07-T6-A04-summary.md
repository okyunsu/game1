# B07 T6 — A04 / PARTIAL
2026-10-10 23:13–23:21 KST. A03 handoff previously wrote23:16; actual T5 finish/start T6 was23:13, corrected in TASKS without rewriting existing evidence.

90×18u, safe continuous floor, two original E1 with scene patrol endpoints±1.2u, main route to A05, continuous enemy pogo optional reward, cracked-wall secret alcove. Enemy tuning/base/V2 assets preserved. No lethal falls. Pogo landing x42/top3.7/width4, reward42/4.5; this rise2.7u exceeds normal jump from floor. Secret at x67, floor top5, roof8..9, back wall75.25/y6.5/0.5×3; cracked front67/y6.5/1×3. Approach61/top3.5,65/top5 leaves ground route clear. High secret is intended revisit with abilities; no baseline value adopted.

Final B07-A04-a3 Development build exit0. Final actual input replay from passing a2 recordings:
```
MEASURE route seconds=14.5180517 ticks=718 stage=0 pos=(3.61, 1.81) pogos=0
PASS A04 main actual input replay
MEASURE route seconds=6.4629205 ticks=313 stage=0 pos=(41.75, 4.51) pogos=2
PASS A04 optional actual input replay
```
Both Error/Exception/Assert=0. Whole-room screenshot Validation/_raw/B07/A04-overview.png. Main a1 failed at low secret approach; raising alcove cleared it. Optional a1 performed two pogo hits but missed high ledge; closer lower landing solved it. Original secret a1 passed; after elevating it to keep main path clear, secret a2 failed:
```
MEASURE route seconds=30.0700359 ticks=1498 stage=5 pos=(61.04, 1.81) pogos=0
FAIL System.Exception: A04 secret actual input record
```
Error0. Final back wall closes rear bypass; main/pogo replay remains PASS. Additional secret approach rebuild was rejected by automatic approval review as exceeding retry limit. Reverted unverified route correction; did not bypass rejection. No subsequent secret retry.

Not verified: final secret approach/discovery and full route. Secret does not block main/optional; retain useful Scene as PARTIAL and continue independent rooms. Raw attempts and inputs are preserved under Validation/_raw/B07/A04-*.