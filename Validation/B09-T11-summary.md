# B09-T11 — T0 전후 회귀 진단

상태 DONE (사전 결정의 진단 기록 기준). 제품 코드 수정 없음.
T0 직전 e3b2352 / 직후 efd6406을 별도 detached worktree에서 각각 Release 빌드했다.

| 검증 | 직전 | 직후 |
|---|---|---|
| V2 전체 고정 입력 W01~W23 | PASS | PASS |
| W22 보스 고정 입력 | 1102 ticks, start deviation0.000 | 동일 |
| RouteAll | R01 초기 위치편차.500u로 소진 | 동일 |
| 분리한 R09 동일 고정 입력 | 299 ticks PASS | 동일 |

T0가 W22/R09 실패 원인이라는 증거 없음. 승인된 조건부 회귀 수정은 시행하지 않는다.
RouteAll은 R09 도달 전 R01에서 중단되어 연속 R09 결과는 미실시. 세 번째 접근으로 동일 관찰 fixture를 두 worktree에만 추가해 R09를 분리했다. 고정 입력 원본과 제품 코드 불변. 분리 PASS는 전체 회귀 PASS를 뜻하지 않는다.

원문 핵심:
MEASURE V2-W22 start deviation=0.000
MEASURE V2-W22 goal=Ending ticks=1102 scene=Ending pos=(0.00, 0.00)
MEASURE continuous start deviation R01=0.500u; no placement performed
FAIL System.Exception: R01 fixed-tick replay recording exhausted room=A01 pos=(34.86, 3.74)
MEASURE R09 fixture start deviation=0.0001405478
MEASURE R09 ticks=299 goal=B04 pos=(3.53, 1.81) failed=False

원문 `_raw/B09/T11-{before,after}-{v2,base-route,R09-isolated}.txt` 및 빌드 로그. 모두 Error/Exception/Assert=0.
B07 새 빌드에서만 보였던 회귀 의심의 전체 원인은 미확정으로 유지한다. 원본 worktree/로그/빌드는 raw에 보존한다.
