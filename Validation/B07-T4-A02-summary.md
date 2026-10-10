# B07 T4 — A02 / PARTIAL
2026-10-10 22:46–23:03 KST. Candidate Scene only; original tuning and base/V2 scenes preserved.

Implemented 30×48u shaft, six 4u landings to 13u deck, A01/A03 exits, 4u inner-width marked chimney from y15 to45 with no internal platforms, summit light/landing/B05 exit. Fall pits0, enemies0; whole-room screenshot Validation/_raw/B07/A02-overview.png.

Unity authoring/build B07-A02-a1/a2/a3 completed with exit0. Actual input attempts failed; Error/Exception/Assert=0. Initial attempt and two same-approach retries preserved. End-state excerpts:
```
MEASURE route seconds=75.0716941 ticks=3754 stage=1 pos=(9.64, 4.88) pogos=0
FAIL System.Exception: A02 main actual input record
MEASURE route seconds=50.0645426 ticks=2501 stage=2 pos=(9.64, 3.82) pogos=0
FAIL System.Exception: A02 main actual input record
MEASURE route seconds=50.0618582 ticks=2501 stage=3 pos=(9.64, 3.81) pogos=0
FAIL System.Exception: A02 main actual input record
MEASURE route seconds=50.0602962 ticks=2501 stage=1 pos=(17.53, 13.82) pogos=0
FAIL System.Exception: A02 revisit actual input record
```
Main gets stuck below second step; moving back and adding run-up did not solve it. Revisit remains at deck below chimney. Level viability versus input generator remains unresolved. An additional geometry/input experiment was rejected by automatic approval review as exceeding retries; unverified edits were reverted and no rejected execution was bypassed.

Not performed: successful replay, Short chimney replay, no-wall-jump gate denial. This partial candidate is usable for scene inspection and further authorized development, but does not establish A02 traversal. T24 full route depends on this path; independent room tasks continue. Raw files: Validation/_raw/B07/A02-*.txt/log/inputs.json and overview.png.