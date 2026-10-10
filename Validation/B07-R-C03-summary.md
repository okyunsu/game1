# B07 R-C03 / PARTIAL (mandatory routes now pass)
2026-10-11 08:06–08:38 KST. Resume-approved corrections applied: C05 landing2/top8/width2,door2/y9; pogo shelf32/top3.7/width2; pit takeoff33.5. Base climb initially fell into C02; air Dash to left landing corrects input and main record/fixed replay pass for base and Short. DJ revisit record/fixed replay also pass.
Optional: two actual enemy pogos occur, but narrow final landing remains missed. Same approved geometry approach with input correction (bounce-height wait then dash) still fails. Initial approach and two input corrections exhausted; no further optional modification/execution. Mandatory C05 acquisition and G-J progression no longer blocked by C03.
```text
EVIDENCE C03-main-resume-record.txt
FAIL B07RoomVerifier actual=exe
MEASURE route seconds=2.966249 ticks=143 stage=2 pos=(3.97, 1.81) pogos=0 wallJumps=3 hp=4 doubleJump=False
FAIL System.Exception: C03 main actual input record
Error/Exception/Assert=0
EVIDENCE C03-main-resume2-replay.txt
MEASURE route seconds=2.4641466 ticks=118 stage=0 pos=(3.89, 1.81) pogos=0 wallJumps=3 hp=4 doubleJump=False
PASS C03 main actual input replay
Error/Exception/Assert=0
EVIDENCE C03-main-resume2-short-replay.txt
MEASURE route seconds=2.0798278 ticks=99 stage=0 pos=(3.89, 1.81) pogos=0 wallJumps=3 hp=4 doubleJump=False
PASS C03 main actual input replay
Error/Exception/Assert=0
EVIDENCE C03-revisit-resume-revisit-replay.txt
MEASURE route seconds=13.5963283 ticks=655 stage=0 pos=(4.11, 1.81) pogos=0 wallJumps=0 hp=2 doubleJump=True
PASS C03 revisit actual input replay
Error/Exception/Assert=0
EVIDENCE C03-optional-resume-optional.txt
FAIL B07RoomVerifier actual=exe
MEASURE route seconds=45.0567349 ticks=2244 stage=6 pos=(32.01, 1.82) pogos=2 wallJumps=3 hp=3 doubleJump=False
FAIL System.Exception: C03 optional actual input record
Error/Exception/Assert=0
EVIDENCE C03-optional-resume3-optional.txt
FAIL B07RoomVerifier actual=exe
MEASURE route seconds=45.0702714 ticks=2238 stage=6 pos=(27.99, 1.82) pogos=2 wallJumps=3 hp=2 doubleJump=False
FAIL System.Exception: C03 optional actual input record
Error/Exception/Assert=0
```
Build errors0/warnings0. Current full overview inspected by T27. Old T17 evidence preserved. Optional fixed replay not performed: no successful discovery record. No protected files or enemy tuning changed.
