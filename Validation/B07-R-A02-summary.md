# B07 R-A02 / PARTIAL
2026-10-11 08:06–08:26 KST. Explicit BATCH-07-RESUME retry approach: separated zigzag platforms, <=2u height steps, >=1.5u horizontal gaps, >=2.2u local overhead clearance, and marked shaft bottom lowered to13u. Original PlayerTuning preserved. Final centers6/12, width3, top3/5/7/9/11/13; deck11/top13/width4. Shaft inner4u, bottom13/top45, no internal platforms. A03 exit9.4/y14; FromA03 12.6/y13.81 prevents immediate return-trigger overlap.
Initial new approach: centers5/11 first step covered Entry; revisit tried crossing solid right shaft at deck level. Same approach corrections: centers6/12 with narrower deck; then edge takeoffs7.7/10.3 and separated return Spawn. Two corrections exhausted. Mandatory input still fails step5; revisit performs15 wall jumps and reaches upper segment, but wall-sliding dash goes outward before summit landing and falls. No further modification/execution on this route.
Release B07-resume-a6 build errors0/warnings0. Final actual evidence:
```text
FAIL B07RoomVerifier actual=exe
MEASURE A02 dimensions=(30.00, 48.00) fallPits=0 enemies=0 route=main short=False
MEASURE route seconds=60.0565206 ticks=3000 stage=8 pos=(5.90, 8.49) pogos=0 wallJumps=0 hp=4 doubleJump=False
FAIL System.Exception: A02 main actual input record
Error/Exception/Assert=0
FAIL B07RoomVerifier actual=exe
MEASURE A02 dimensions=(30.00, 48.00) fallPits=0 enemies=0 route=revisit short=False
MEASURE route seconds=60.0552656 ticks=3000 stage=2 pos=(13.14, 16.21) pogos=0 wallJumps=15 hp=4 doubleJump=False
FAIL System.Exception: A02 revisit actual input record
Error/Exception/Assert=0
```
Fixed replay and Short success not performed: no successful route to replay. Original full overview retained; final overview refreshed by inventory Task. T24 may only skip A02 as explicitly approved.
Recommended next approved geometry: width4, centers6.5/12 (1.5u edge gap), deck11/width4 and Entry4 clear of first underside; revisit normal jump before dash to detach from shaft. Not applied: retry budget exhausted. Useful isolated candidate retained, not accepted as mandatory traversal.
