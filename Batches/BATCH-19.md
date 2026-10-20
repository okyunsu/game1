이번 실행은 Batch 19(V5 층 A 검사 규칙 정리: 가짜 후보 줄이기, 근거 있는 규칙만 유지, 더블 점프 경로 원인 측정)만 수행한다. AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다. push는 하지 않는다. **Scene·Design·게임 수치는 바꾸지 않는다.**

## 목적
- Batch 18 결과 끼임 후보 69건 중 머리 위 구조물 후보는 0이고, 나머지(문 6·같은 높이 측면 43·구덩이 가장자리 20)는 벽·블록·닫힌 문 옆에 서는 자리라 **진행 불가와 무관한 후보**다. 이 가짜 후보 때문에 "후보 0"이 영영 안 되는 상태를 푼다.
- 규칙을 **더 늘리지 않는다.** 근거(실제 겪은 버그) 있는 규칙은 FAIL, 근거 없는 규칙은 WARN/MEASURE로 내린다. 규칙이 과도해지는 것이 이번 점검의 걱정이었다.
- 정적 검사 FAIL 22건(A02 벽 점프 면, A06 더블 점프 전용 방)이 검사기가 능력 게이트를 몰라서 생기는지 확인하고, 모르는 부분만 반영한다.

## 읽을 파일
AGENTS.md, TASKS.md(2절·3절·최신 인계), `Design/v5/WORLD_V5.md`(9~11절), `Design/v5/rooms_v5_A.json`, `Validation/B18-summary.md`, `B18-clearance-after.md`, `B18-room-rules-v5-A.md`, `B18-dj-route.md`, `B18-hop-limits.md`, `Tools/Check-RoomRules.py`, V5 Scene 검사 코드(`-verify-b18-clearance`).

## 시작 조건
- 작업 트리 변경이 `Batches/BATCH-19.md`(미추적)와 `Design/v5/WORLD_V5.md`뿐이어야 한다. 그 외 변경이 있으면 시작하지 않고 멈춘다. 이 두 파일만 먼저 커밋하고 그 직후 HEAD를 Base로 기록한다.
- 시작 전 Unity 실행 확인(`-batchmode -quit`, 종료 코드 0). 실패하면 `Validation/B19-blocked.md`에 원문만 남기고 종료한다.
- HEAD가 `B18` 태그를 포함해야 한다.

## 예산·큐 규칙
- 최대 7 Task 또는 **3시간**. 2시간 30분이 지나면 새 Task를 시작하지 않는다.
- BLOCKED는 그 Task만 되돌리고 다음으로. 재시도는 같은 문제 1회. 회귀는 `Tools/Run-Regression.ps1 -Quiet`(Task마다)와 마지막 기본 방식 1회.
- 기존 Validation은 덮어쓰지 않고 `B19-*`로 저장한다. 역슬래시 문자열 금지, 코드 수정마다 컴파일 확인.
- push 안 된 이전 커밋이 있으면 합치기(squash)를 하지 않고 Task 커밋을 보존한다(B18과 같은 방식). 태그 `B19`는 마지막 커밋에.

## 허용 범위
`Assets/Editor/**`, `Assets/Scripts/Verification/**`, `Assets/Scripts/V5*.cs`, `Tools/Check-RoomRules.py`, `Validation/B19-*`, TASKS.md, README.md, `Batches/BATCH-19.md`, 빌드 산출물, V5 런처.
수정 금지: `Design/`(WORLD_V5.md 시작 커밋 제외), V4 Scene·Resources·Prefab, `Assets/Scenes/V5/**`, 수치 에셋, 보스, 층 B·C, `PlayerMotor.cs`·`RoomSession.cs`·`RoomCameraRig.cs`, 이전 `Validation/B*-*.md`.

## Task
| Task | 유형 | 내용 | 합격 기준 |
|---|---|---|---|
| T1 | verification | B18 끼임 후보 69건을 범주별로 다시 센다: (a) 머리 위(차단 구조물 아랫면 > 발 높이) (b) 닫힌 문 (c) 같은 높이 측면(벽·블록 옆) (d) 구덩이 가장자리 (e) 양옆 틈(몸 폭 0.7 + 여유보다 좁은 가로 틈). 코드 수정 없이 기존 로그를 분석해도 된다 | `Validation/B19-candidates.md`에 범주별 개수와 방별 표. **범주 (a)는 draft3에서 0**이어야 하고, draft2(`git show B17:Design/v5/rooms_v5_A.json` 기준, 임시 폴더 생성 후 삭제)에서는 **4건(A01 solid7↔platform0, A02 solid13↔platform0, A06 solid6↔platform2, A06 solid7↔platform3)**이어야 한다 |
| T2 | feature | `-verify-b18-clearance`의 판정 정리: (a) 머리 위 = **FAIL 유지**(실제 버그 4건과 연결) (b)~(d) = **MEASURE로만 출력**(FAIL 아님, 개수는 요약에 표시) (e) 양옆 틈 = **WARN**(draft2·draft3 모두 0건이라 실제 버그 근거가 없다). 적 박힘·순찰·지지면 이상은 그대로 FAIL | draft3 Scene에서 FAIL 0·WARN 0. **draft2 데이터를 임시 생성한 Scene에서 머리 위 FAIL 4건이 그대로 잡힌다**(잡지 못하면 규칙이 너무 좁은 것이므로 원복). 일부러 만든 틈 0.5 fixture는 WARN으로 잡힘. 임시 Scene은 확인 후 삭제 |
| T3 | feature | `Tools/Check-RoomRules.py` 능력 게이트 반영(근거 있는 부분만): (1) `climb`(벽 점프 면)이 있는 방은 그 면을 아래에서 위까지 오를 수 있는 길로 본다 (2) 문에 `gate`가 있고 그 문이 유일 입구인 방(A06 더블 점프)은 도달성을 **해당 능력 보유 상태**로 계산한다. 더블 점프 높이는 `PlayerTuning.asset`을 읽기만 하고 코드에 박지 않는다 (3) 능력 없이 갈 수 있다고 가정한 계산과 능력 보유 계산을 구분해 출력 | 기존 FAIL 22건 중 몇 건이 사라지는지 표로 기록. **남는 FAIL은 원인을 한 줄씩 적는다**(검사기 한계 / 실제 의심). 새 FAIL 0. 위반 fixture 4개 재검출. `Validation/B19-room-rules-v5-A.md` |
| T4 | verification | 더블 점프 자연 경로 원인 측정(측정만): A01에서 더블 점프로 A06 천장 구멍(x30~34,y15)으로 오르는 구간이 120초 동안 x24.95·발 높이 4.39에서 멈춘 이유. 프레임마다 위치·속도·접지·입력·접촉 콜라이더·다음 목표 좌표를 기록하고, 자동 입력의 목표 좌표가 draft3 Scene과 맞는지(발판이 draft2에서 옮겨졌는지), 더블 점프로 y15에 닿는 높이인지 확인 | `Validation/B19-dj-route.md`에 원인 분류(자동 입력 목표가 낡음 / 입력 타이밍 / 물리적으로 못 닿음)와 원문. 자동 입력 목표가 낡았으면 **검증 코드의 목표 좌표만** 갱신해 재측정하고, 물리적으로 못 닿는 것이면 수정하지 않고 좌표·수치를 기록해 Claude에게 돌린다 |
| T5 | doc | TASKS.md·README: D-V5-CLEARANCE 결과(후보 69건 범주, 규칙 정리 내용) 반영. 규칙 목록을 "FAIL(근거 버그 있음)"과 "WARN/MEASURE(근거 없음·취향)"로 나눠 `Validation/B19-rules-ledger.md`에 표로 정리하고, 각 FAIL 규칙에 근거가 된 실제 사례를 적는다 | 결정 대기 8개 이하, 깨진 링크 0. 근거 없는 FAIL 규칙 0 |
| T6 | verification | 마지막 점검: 기본 회귀 13/13 1회, Batch-Card, V5 런처 3개 시작 로그 | 허용 범위 밖 0, 깨진 링크 0 |

## 종료 처리
- `Validation/B19-summary.md`: Task별 결과, 유형별 개수, 규칙 정리 전후 개수(FAIL/WARN/MEASURE), T3에서 사라진 FAIL 수와 남은 FAIL 원인, T4 원인 분류, 열린 항목, 사용자가 할 일.
- Codex 요약과 로그가 다르면 로그 원문을 우선한다. push는 사용자가 한다.
