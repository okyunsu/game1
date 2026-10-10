# B07 T25 — PARTIAL
전체 회귀1회: 기본13/13 PASS. RouteAll R09 입력 소진과 Pogo P03 미완주 FAIL/Error0. R09는 T0 첫 회귀에도 같은 위치에서 발생했고 P03는 기존 간헐 이슈다. 다른 기본 영역 회귀 없음.
V2 전체 고정 입력: 새 빌드 두 번 모두 앞21구간 PASS 후 W22 보스에서 실제 사망. 이전 B06b-delivery 같은 입력은23구간 전체 PASS. 원인 미확정이며 회귀 의심으로 기록한다. 고정 입력 보스 진입 위치 차이 .082u가 관측되지만 이것만으로 원인을 확정하지 않는다. BossEncounter/기본 및 V2 Scene/기존 Tuning 보호 유지, 범위 밖 게임 규칙 수정 없이 PARTIAL.
원본 Validation/_raw/B07/Regression/RouteAll-60fps-B07.txt
FAIL RouteRunner actual=exe
MEASURE continuous start deviation R09=0.000u; no placement performed
FAIL System.Exception: R09 fixed-tick replay recording exhausted room=B04 pos=(9.20, 3.21)
Error/Exception/Assert=0
원본 Validation/_raw/B07/Regression/PogoTest-60fps-B07.txt
FAIL N9PogoRoute actual=exe
FAIL System.Exception: P03 input goal pogos=1
Error/Exception/Assert=0
원본 Validation/_raw/B07/T25-V2-followup.txt
FAIL B05World actual=exe
MEASURE V2-W22 start deviation=0.082
FAIL frame-observed actual death V2-W22
MEASURE V2-W22 goal=Ending ticks=285 scene=C06 pos=(13.45, 2.53)
FAIL System.Exception: V2-W22 fixed replay continuous Ending
Error/Exception/Assert=0
