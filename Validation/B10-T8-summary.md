# B10 T8 — 별도 worktree 회귀 원인 범위 축소

상태: DONE(원인 미확정 시 범위 축소 기록). 원인 커밋 확정과 회귀 수정은 **미달성**. 조건부 기본/V2 수정0.

현재 작업 트리를 변경하지 않고 `_raw/B10/bisect`에서 `efd6406..d1e9ce0`을 git bisect했다. 조사 시작 HEAD는 d1e9ce0이며 B10 새 커밋들은 조사 대상에 넣지 않았다. bisect 원문 `_raw/B10/T8-bisect-session.txt`, 비교 빌드와 원본 로그 보존. bisect worktree는 기록 후 reset했다.

| ref/조건 | 실제 결과 | 근거 |
|---|---|---|
| efd6406 / B06 Release + b05-v2 | V2 W01~W23 PASS, W22 1102 ticks | T8-efd-v2-r1.txt |
| 06c138d / 같은 빌드 | V2 전체 PASS | T8-mid-v2-r1.txt |
| d62aa7c / 같은 빌드 | V2 전체 PASS | T8-d62aa7c-v2-r1.txt |
| ed9ff22 / 같은 빌드 | V2 전체 PASS | T8-ed9ff22-v2-r1.txt |
| 원래 HEAD d1e9ce0 / B06 Release + b05-v2 | **같은 고정 입력 전체 2회 PASS**, W22 1102 ticks | T8-head-control-v2-r1.txt, T8-head-control-fixed-r4.txt |
| B10-release-r6 / V4 포함 빌드, runtime -v2 | W22 실제 사망, 285 ticks | T8-head-v2-r1.txt |
| efd6406 / 기본 RouteAll R01 | 이미 입력 소진 FAIL | T8-efd-base-R01-r1.txt |

원문 핵심:

```text
MEASURE V2-W22 start deviation=0.082
FAIL frame-observed actual death V2-W22
MEASURE V2-W22 goal=Ending ticks=285 scene=C06 pos=(13.45, 2.53)
FAIL System.Exception: V2-W22 fixed replay continuous Ending
```

```text
MEASURE V2-W22 goal=Ending ticks=1102 scene=Ending pos=(0.00, 0.00)
PASS V2-W22 fixed replay continuous Ending
PASS new game continuous world complete; no placement or ability grants
Error/Exception/Assert=0
```

```text
MEASURE continuous start deviation R01=0.500u; no placement performed
FAIL System.Exception: R01 fixed-tick replay recording exhausted room=A01 pos=(34.86, 3.74)
```

## 좁힌 범위와 한계

W22는 같은 HEAD가 이전 빌드 방식에서 성공하므로 단순 소스 커밋 bisect의 bad endpoint가 유효하지 않다. 통과 중간 ref들은 과거 방식, 실패 endpoint는 다른 방식으로 비교하면 문서 커밋을 잘못 원인으로 지목할 수 있다. 남은 비교 범위는 **V4/V3를 포함하는 Scene 목록, B05Build의 `-b05-v2` Scene 처리, B09LegacyMode의 runtime mapping과 고정 입력 시작 위상**이다. 특정 변경 줄을 원인으로 입증하지 못했다. 진입 편차 .082u만으로 사망 원인을 확정할 수 없다(동일 편차로 성공한 대조도 있다).

R01은 범위의 good endpoint인 efd6406에서도 이미 실패해 이 범위에서 최초 실패 커밋을 찾을 수 없다. 더 이전 이력 또는 기록 시작 조건 조사가 필요하지만 이번 조건부 수정의 근거가 아니다.

추가 실행 `T8-head-control-v2-r3`은 앞 부분만 고정 입력, 뒤는 대응 입력을 기록한 실행으로 W22 성공했으나 위 고정 입력 2회 증거에 포함하지 않는다. `r2`는 잘못된 opt-in 옵션으로 검사가 시작되지 않아 미실시로 보존하고 해당 프로세스만 종료했다.

미실시: 확정 원인에 따른 기본/V2 수정, 수정 후 전체 회귀. Batch 마지막 T13의 기본13/RouteAll/Pogo/V2 실제 결과를 별도로 남긴다.
