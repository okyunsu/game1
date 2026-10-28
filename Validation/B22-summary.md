# Batch 22 — 보스1과 draft4.1 고리

| 종료 검사 | 이번 실행 결과 |
| --- | --- |
| Batch-Card | [T9 후 카드](_raw/22/final-batch-card.txt): 38파일, 범위 밖0·깨진 링크0·없는 빌드/런처0. 기준 HEAD `5154bf5`; 이후 종료 기록2파일만 추가 |
| 별도 작업 트리 | [검사 원문](_raw/22/final-worktree.txt): 이번 미커밋0, 시작 사용자 변경8개·미추적 D41 그대로. 시작/종료 사용자 diff patch SHA 동일 |
| 보호 파일 | 시작 정책96파일 SHA 변경0. Design·기본 튜닝·Player.prefab·ProjectSettings 변경0 |
| 현재 소스 회귀 | [B22-final](_raw/regression/B22-final/result.txt): **REGRESSION 13/13**. 카드의 과거 행을 대신 쓰지 않음 |
| 실제 배포 | Development 오류0/경고0·[런처14개](_raw/22/smoke-all-summary.txt) PASS/Error0·몸1.6/1.3 고리와 Continue PASS |


2026-10-28 시작, 10-29 종료. Base `e437320`, 기존 로컬 origin/main 대비 2커밋은 보존했다. fetch/push/squash 없음. 사용자 지정 [06 지시문](../Design/proposals-1028/06_CODEX_BATCH22_BOSS1.md)을 실행 승인으로 사용했다. 별도 Batches/BATCH-22.md는 없고, 허용 범위에 Batches가 없어 복제하지 않았다. 실행 11 Task(T0~T9, BL1), Ready Queue0.

Claude 설계 변경: 시작 커밋 `e437320`과 시작 작업 트리의 Design/proposals-1028 4개·Design/v5 3개·GAME_DESIGN, D41 검증 문서. 작성 출처는 문서 표기(Claude)이며 사용자 대화 전문은 없다. 기존 사용자 변경은 별도 보존했고 이번 Design·게임 규칙 원본 수정0이다. 승인 근거·추천값의 구분은 TASKS 2-1 참조.

## 새로 플레이 가능한 범위

[Play-V5-Loop](../Launchers/current/Play-V5-Loop.cmd): A03 체크포인트 → 오른쪽 계단 → A04 위 길 또는 아래 포고 길 → 보스1 → 열린 바닥 → A05 대시 → 도랑 → A03 대시 연습 → 대시 문 선반. B1 처치는 ID `B1-A04`로 즉시 저장되며 최종 보스 C06·Slam·엔딩과 독립이다.

[BossTest_B1](../Launchers/current/Play-BossTest-B1.cmd)은 별도 아레나에서 시작하고, 사망하면 실제 CP-A03으로 복귀한다. 다시 A04로 가면 재시도한다. 모든 V4/V5 런처의 최신 빌드는 `Builds/B22-delivery/Afterglow.exe`. [런처14개](_raw/22/smoke-all-summary.txt)는 각각 실제 실행 PASS·Error0이다. 직전 B21은 보존했고 더 오래된 B12 슬롯105파일은 [SHA 확인 후 보존](_raw/22/retired-B12-slot-manifest.json)하고 빌드만 정리했다([기록](_raw/22/housekeeping.txt)). 인자 없는 V5에는 마지막 안전 바닥 복귀가 기본이며 모서리 두 후보는 계속 꺼짐이다.

## Task와 실제 근거

| Task | 상태 | 결과·원문 |
| --- | --- | --- |
| T0 | DONE | 전달 결정9개·실행 승인·B21 처리 반영. D-BODY/D-CORNER-FIX 두 결정 유지 |
| T1 | DONE | [구 version1·ID 배열·초기화](_raw/22/T1-save/result.txt). [기존 C06 즉시 저장](_raw/22/T1-completion/kill-now.txt)·[별도 프로세스 Continue/엔딩](_raw/22/T1-completion/continue.txt) PASS |
| T2 | DONE | [기존 회복 실제 X](_raw/22/T2-boss-recovery/recovery.txt), B3 [8패턴](_raw/22/T2-B3-cycle-clean/B3Cycle.txt)·[2페이즈/사망](_raw/22/T2-B3-phase/B3Phase.txt)·[Slam/엔딩](_raw/22/T2-B3-ending-clean/B3Ending.txt) PASS·Error0 |
| T3 | DONE | 새 B1 Asset HP14/피해1/단일 페이즈·도약/충격파/돌진만 관측, 포고1회 vy11.000u/s. [생성](_raw/22/T3-candidate/result.txt)·[런타임](_raw/22/T4-complete/result.txt) |
| T4 | DONE | [BossTest_B1](_raw/22/T4-complete/result.txt): 등장0.800s·상단 통과 무반응·게이트/덮개·사망→CP-A03·HP14/패턴 초기화·무능력 실제 처치8.500s HP3·즉시 저장·재진입 억제 PASS |
| T5 | REVIEW | V5 기본 복귀 실측, [V4 기본 컴포넌트 없음](_raw/22/T5-v4/result.txt). 기존 DamageBlock·V4 Scene 변경0. 최종 체감 수락은 고리 플레이 |
| T6 | DONE | [고정 계약·잘못된 spawn fixture 거절](_raw/22/T6-fixtures/result.txt), [CP 하나·6방 잠금/벽0](_raw/22/T7-generate/result.txt) |
| T7 | DONE | 아래 모든 표 PASS. 정적 [FAIL0/WARN16·self-test](_raw/22/T7-static.md), Scene [FAIL0/WARN0](_raw/22/T7-clearance/result.txt) |
| T8 | REVIEW | [전체 연속 고리](_raw/22/T8-loop-running-edge/result.txt)46.731s·보스전8.618s·Continue PASS·Error0. 고정 입력 실측, 사용자 감각 수락 별도 |
| T9 | DONE | README·TASKS 3/9절·새 런처2개·조정 위치·플레이 전 질문2개/기록 칸 |
| BL1 | DONE | [몸1.3 고리](_raw/22/BL1-body13/result.txt)46.716s·보스전8.600s·Continue PASS·Error0. 참고 측정만, 기본값 변경0 |

### 이동 실측

| 구간 | 무능력 | 대시/포고 | 피해·복귀/조건 |
| --- | --- | --- | --- |
| A04 새 섬 직행 | 통과0/5 | 등불2개 포고로 보상5/5 | 직행 왼쪽 복귀5/5. 포고 각2회, HP4→4·복귀0. [원문](_raw/22/T7-a04-brake/result.txt) |
| A05 도랑6u | 통과0/10 | 대시10/10 | 동/서 각5회·진입한 쪽 복귀10/10. [원문](_raw/22/T7-a05/result.txt) |
| A03 1단계 평지5.5u | 0/5 | 5/5 | [세 단계30회 원문](_raw/22/T7-practice/result.txt) |
| A03 2단계 상승2u·간격4.5u | 0/5 | 5/5 | 개별 발판 초기 pose만 fixture, 이동은 입력 |
| A03 3단계 상승2u·간격4.5u | 0/5 | 5/5 | 선반 도달·접지 확인 |

| 고리 단계 | 몸1.6 | 몸1.3 |
| --- | --- | --- |
| A03 오른쪽 계단·실제 A04 문 | PASS | PASS |
| A04 실제 일반 적 포함 위 길·진입 잠금 | PASS | PASS |
| 무능력 B1 처치·덮개 제거 | PASS | PASS |
| A05 실제 낙하·DashPickup | PASS | PASS |
| 얕은 구덩이·6u 도랑 대시·A03 문 | PASS | PASS |
| CP-A03 접촉·서쪽 마을·S0 | PASS | PASS |
| 대시 연습3개·대시 문 선반 | PASS | PASS |
| 저장·제목·Continue | PASS | PASS |

전체 고리는 시작 CP fixture 이후 위치/HP/튜닝/보스 상태를 쓰지 않는 실제 입력이다. A04 위 길에서 일반 적 피해1, 보스전 피해1, A05 이동에서 피해1을 받았으며 A03 체크포인트 접촉으로 실제 HP4가 복원됐다. 고리 시간에는 처방 입력의 빠른 경로와 메뉴 처리가 포함되며 사람의 탐색/첫 승리 시간 증거는 아니다. B1 테스트의 모든 패턴 관측은 별도 HP100 fixture, 포고는 별도 공중 회복 위치 fixture다. 기본 HP 전투 승리는 별도이며 fixture를 섞어 승리로 세지 않았다.

### 검사 비교와 실행 환경

- [Preflight](_raw/22/preflight-20261028-233344-907-c819619e/preflight.txt) Hub licensing client=True·Unity exit0. Unity `C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe`. Unity/호출 스크립트/검증 exe는 승인된 샌드박스 밖 실행, 편집·분석은 내부 실행.
- Python `C:/Users/gosu3/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe`. 명령: `Tools/Check-RoomRules.py Design/v5/rooms_v5_A.json --v5 --self-test --world Design/v5/world_v5_A.json --self-test-output-dir Validation/_raw/22/static-fixtures --output Validation/_raw/22/T7-static.md`. 이번 max-step 실측 JSON은 없으므로 trench-escape MEASURE 미실시. 과거 측정치를 이번 측정처럼 전달하지 않았다.
- 정적 FAIL0/WARN16은 시작 D41 문서와 같으며 모두 A03 게이트의 도달/높이 후보다. 의도적 위반 self-test의 FAIL 원문은 실제 데이터 FAIL과 구분한다. 정적 도달 후보를 실제 완주 증거로 쓰지 않았다.
- Scene 검사: `V5Batch18.Clearance -verify-b18-clearance`, 대상 `Assets/Scenes/V5/*.unity`. 같은 B19 판정 규칙으로 실제 FAIL0/WARN0·overhead0. 지형이 바뀌어 B21의 후보69 총합과 단순 개수 비교하지 않는다. [방별 MEASURE](_raw/22/T7-clearance/result.txt)에 닫힌 문/측면/구덩이 후보 보존.
- [배포와 동일한 클린 빌드](_raw/22/build7/result.txt) Development errors0/warnings0. B1 테스트·V5 6방·기존 V4 전체를 명시 포함. 후보 주입은 `-b21-candidates -b17-draft2-map -b11-candidates`. V5 기본 Asset/Scene에 후보 크기나 모서리 보정을 채택하지 않았다.
- [전체 기본 회귀13/13](_raw/regression/B22-final/result.txt). Release·실제 패드·사람 첫 승리/감각 수락·Sprint Gate 미실시.

## 실패 원문과 보완

원본 실행은 모두 새 경로에 보존했다. 제품 설정·기하를 바꾸어 기준을 낮추지 않았다.

```text
MEASURE B1 fight seconds=6.852 defeated=False playerHP=0 pogos=6 patterns=1,2 phaseTransitions=0 body=1.60
FAIL System.Exception: actual inputs defeat B1 without dash/wall/double jump
MEASURE B1 fight seconds=8.517 defeated=True playerHP=3 pogos=0 patterns=1,2 phaseTransitions=0 body=1.60
FAIL System.Exception: actual top pogo bounced
```

[첫 전투](_raw/22/T4-boss1/result.txt)는 지속 점프/추적 입력으로 사망했다. 패턴별 회피·회복 반격이라는 다른 접근은 승리했지만, [두 번째 검사](_raw/22/T4-boss2/result.txt)가 전투 안 포고를 추가로 강제해 실패했다. 계약의 포고 동작은 별도 fixture로 직접 확인했고 전투 승리에 포고 사용을 필수로 추가하지 않았다. 최종 T4-complete는 두 기준 모두 직접 통과했다.

```text
MEASURE A04 summary directFailed=5/5 leftReturn=5/5 pogoSuccess=0/5
FAIL System.Exception: A04 direct0/5 left return5/5 pogo reward5/5
FAIL System.Exception: GoX timeout target=15.5 actual=(32.86, 0.42)
MEASURE loop jump=upper village landed=False end=(14.3810, 5.0150) hp=4
FAIL System.Exception: physical jump upper village
```

[A04 첫 레시피](_raw/22/T7-a04/result.txt)는 첫 등불을 지나친 뒤 하강해 포고0이었다. 첫/둘째 등불 위에서 가로 속도를 제어하는 방식으로 각 포고2회·보상5/5를 얻었다. [고리1](_raw/22/T8-loop1/result.txt)은 얕은 구덩이 점프 누락, [고리2](_raw/22/T8-loop2/result.txt)는 S0 밑 머리 충돌로 실패했다. 세 번째 접근은 서쪽 플랫폼 끝 x16.85에서 달리며 이륙해 상단 마을에 도달했다. 모서리 후보를 켜거나 좌표를 바꾸지 않았다.

```text
FAIL System.Exception: Timeout room ready
ArgumentException: The scene is invalid.
Error/Exception/Assert=1
```

초기 [B3 Cycle](_raw/22/T2-B3-cycle/B3Cycle.txt)·[Ending](_raw/22/T2-B3-ending/B3Ending.txt)은 동일하게 `Missing or duplicate initial Spawn ID` 뒤 CP-C04 진입 전에 중단했다. 새 후보 주입 인자를 기존 증분 Scene 빌드가 반영하지 않은 검증 준비 실패이며 보스 패턴 회귀는 실행되지 않았다. CleanBuildCache 뒤 실제 B3 cycle/phase/ending·기존 B3 런처가 모두 PASS였다. T2 동작 실패로 바꾸어 기록하거나 무관한 제품 코드를 되돌리지 않았다.

## 사용자 확인과 조정 위치

고리 1회 10~15분을 예상 안내한다. 자동 경로는 약47초, 보스 약8.6초로 짧았다. 이 수치는 익숙한 입력의 기술 실측이며 C-5 목표(첫 승리2~5분·숙련1분 안팎)의 사용자 수락 증거가 아니다. HP14·피해1·아레나 계약은 유지했다.

플레이 전: ① A04 전투 진입과 처치 후 구멍으로 내려갈 이유가 읽히는가? ② A05 이후 A03 연습을 마쳤을 때 대시가 손에 익는가? 기록: 고리 시간____ / 보스 시도____ / 막힌 곳____.

`Assets/BossCandidates/B1/BossTuning.asset`에 HP/피해/패턴·속도7.5u/s, Scene의 BossEncounter에 아레나/게이트/트리거·등장0.8s가 있다. `Assets/Candidates/B21HazardReturn.asset`의 기존 접지0.1s·여백0.5u·정지0.12s는 변경0. Play 값은 종료 전 기록하고 Edit 모드에서 명시 저장 후 재빌드한다. 기본 몸은1.6, `-BodyHeight 1.3`은 비교만 한다.

Ruling: B1 돌진7.5u/s는 승인 범위7~8의 중간, 보행 앵커29/44는 고정 아레나 안 회피 공간을 남긴 후보다. 잘못됐을 때 비용은 후보 튜닝 재조정·재빌드이며 계약 좌표를 변경하지 않는다. 새 B1 폴더의 B1.meta는 새 Asset에 Unity 생성 .meta를 함께 커밋하라는 AGENTS 규칙에 따라 허용 목록에 좁게 포함했다. 생성기는 허용된 Assets/Scripts/Editor에 두고 기존 Assets/Editor 생성기는 읽기/호출만 했다. 이전 인계 상세는 기존 B21 요약 링크로 보존하며 Archive는 이번 허용 범위 밖이라 새 이동을 하지 않았다.
