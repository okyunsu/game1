이번 실행은 Batch 18(V5 층 A 2차 플레이 피드백: 끼임·적 박힘 원인 측정, draft3 재생성, 검사 규칙 보강)만 수행한다. AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다. push는 하지 않는다.

## 목적
사용자 직접 플레이(2026-10-18, 스크린샷 3장)에서 나온 문제를 처리한다.
1. 블록·발판 사이에 **끼이는 곳이 여전히 많다.** 스크린샷에서 플레이어가 블록 가장자리에 서면 옆 발판 아래에 머리가 걸린다(블록 오른쪽 끝과 발판 왼쪽 끝 사이 가로 0.5, 블록 윗면~발판 아랫면 1.5).
2. A02 중간 선반의 블록 안에 **적이 박혀 있다**(적 체력 표시가 블록 안에 보임).
3. 층 사이 이동이 **등불 포고뿐인 구간이 너무 많다.** 작은 발판을 섞는다.
- 원인 1·2는 Claude의 설계 데이터 실수다(draft2 기준). 이번 배치는 draft3 데이터로 Scene을 다시 만들고, **실제 Scene에서 끼임·박힘을 자동으로 찾는 검사**를 만든다.
- 방 구조·수치·좌표는 고치지 않는다(`Design/` 읽기 전용). 데이터 문제가 보이면 기록해 Claude에게 돌려준다.

## 읽을 파일
AGENTS.md, TASKS.md(2절·3절·최신 인계), `Design/v5/WORLD_V5.md`(**9절·10절**), `Design/v5/rooms_v5_A.json`, `Design/v5/world_v5_A.json`, `Design/v5/ascii_A.txt`, `Validation/B17-summary.md`, `Validation/B17-probe.md`, `Validation/B17-room-rules-v5-A.md`, `Assets/Editor` 아래 V5 생성기, `Tools/Check-RoomRules.py`.

## 시작 조건
- 작업 트리 변경이 `Batches/BATCH-18.md`(미추적)와 `Design/v5/` 아래 파일(rooms_v5_A.json, world_v5_A.json, WORLD_V5.md, ascii_A.txt, floorA_v5.png)뿐이어야 한다. 그 외 변경이 있으면 시작하지 않고 멈춘다.
- 위 파일만 먼저 커밋한다("docs: V5 floor A draft3 (clearance margin, enemy inside block, fewer pogo-only climbs) and Batch 18 instructions"). 이 커밋 직후 HEAD가 Base다. 시작 시각과 Base를 기록한다.
- HEAD가 `B17` 태그를 포함해야 한다. push 안 된 이전 커밋이 있어도 정상이다.
- **Unity Editor 라이선스를 먼저 확인한다**(Batch 17 종료 시 exit 198). 시작 시 Unity 실행 확인에 실패하면 어떤 Task도 시작하지 않고 `Validation/B18-blocked.md`에 원문만 남기고 종료한다.

## 예산·큐 규칙
- 예산: Task(Backlog 포함) 최대 12개 또는 **6시간** 중 먼저 도달하는 쪽. 5시간 30분이 지나면 새 Task를 시작하지 않고 종료 처리를 한다.
- Task가 BLOCKED되면 그 Task 변경만 되돌리고 다음으로 간다. 선행이 BLOCKED면 SKIPPED. 재시도는 Task당 같은 문제 1회.
- 회귀는 `Tools/Run-Regression.ps1 -Quiet`(Task마다), 마지막에 기본 방식 1회.
- 기존 Validation은 덮어쓰지 않고 새 파일(`B18-*`)로 저장한다.
- C# 문자열에 Windows 경로·역슬래시를 직접 쓰지 않는다. 코드를 고칠 때마다 컴파일을 확인한다.

## 허용 범위
`Assets/Scenes/V5/**`, V5 생성기·검증 코드(Assets/Editor, Assets/Scripts/Verification, `V5*.cs`), `Tools/Check-RoomRules.py`, `Launchers/current/*V5*`, `Validation/B18-*`, `Batches/BATCH-18.md`, TASKS.md, README.md, 빌드 산출물.
수정 금지: `Design/`(읽기 전용), V4 Scene·Resources·V4 런처, 수치 에셋(`PlayerTuning.asset` 등), 보스·층 B·C, 이전 `Validation/B*-*.md`, 핵심 로직(`PlayerMotor.cs`, `RoomSession.cs`, `RoomCameraRig.cs`). T1에서 원인이 이 파일들의 로직으로 확인되면 고치지 않고 기록해 Claude에게 돌려준다.

## 사전 결정
| 결정 | 답 | 막히면 대체 |
| --- | --- | --- |
| 데이터 | `Design/v5/` draft3 | |
| 플레이어 몸 | `Player.prefab` BoxCollider2D (0.7×1.6)를 읽어 쓴다 | 읽기 실패 시 0.7×1.6, 요약에 적는다 |
| 끼임 판정 | 서 있을 수 있는 면(솔리드·발판 윗면)마다 가장자리 양옆 몸 반폭(0.35)까지 포함한 **서 있는 자리**를 0.25 간격으로 만들고, 그 자리에서 몸 박스(점프 여유를 위해 높이 +0.6까지)가 **막는 콜라이더**와 겹치면 끼임 후보. 겹치는 대상이 한 방향 통과 발판이면 따로 표시 | |
| 한 방향 통과 | 발판 콜라이더가 한 방향 통과(PlatformEffector2D)인지, 옆·아래에서 막는지 V4 방과 V5 방에서 비교 | |

## Task
| Task | 유형 | 내용 | 합격 기준 |
|---|---|---|---|
| T1 | verification | **현재(draft2) V5 Scene**에서 실제 끼임·박힘 원인 측정. (a) A02 스크린샷 위치(블록 오른쪽 끝, 발판 왼쪽 끝 사이)에서 플레이어를 가장자리 양옆에 세우고 접촉 콜라이더 이름·한 방향 통과 여부·이동 가능 여부 기록 (b) A02 중간 선반 적(x19,y9)이 블록 콜라이더(x18~20,y9~10.8)와 겹치는지 OverlapBox로 확인 (c) V5 발판 콜라이더 설정(Effector, usedByEffector, surfaceArc, 두께)을 V4 방 1개와 나란히 표로 비교 | `Validation/B18-probe.md`에 표와 원문. 원인을 "데이터(간격·배치)"와 "콜라이더 설정"으로 분류. 코드·Scene 변경 0 |
| T2 | feature | 끼임·박힘 자동 검사 `-verify-b18-clearance` 추가(개발 빌드 전용, 가능하면 Scene 검사만이라 Editor 없이 exe 불필요): (1) 위 끼임 판정을 V5 6방 전체 서 있는 자리에 실행해 후보를 방·좌표·부딪히는 콜라이더 이름과 함께 표로 출력 (2) 적 생성 위치(몸 박스)와 막는 콜라이더가 겹치면 FAIL (3) 적 순찰 양끝에서 몸 박스가 막는 콜라이더와 겹치거나 발밑에 서 있는 면이 없으면 FAIL | 원문 줄 `MEASURE`/`FAIL`로 출력. **draft2 Scene에서 A02 적 박힘과 스크린샷의 끼임 위치가 FAIL로 잡혀야 한다**(잡지 못하면 검사 규칙이 틀린 것이므로 보정하고 T1 결과와 맞춘다). `Validation/B18-clearance-before.md` 저장 |
| T3 | feature | draft3 Scene 6개 재생성(`Assets/Scenes/V5/`) + 요소 수 표 | 방 6개 생성. 방별 요소 수(고체·발판·**작은 발판(폭≤2.5)**·적·등불·가시·문·봉인·부서지는 벽·조각·바닥 구덩이)가 데이터와 같음(표). 등불은 A03 1개·A04 4개·A06 가시 3개. V4 SHA256 변화 0. `-verify-b16-v5` 문 전환 검사 재통과 |
| T4 | verification | T2 검사를 draft3 Scene에서 재실행 | 끼임 후보 0, 적 박힘 0, 순찰 끝 이상 0. 남는 후보가 있으면 방·좌표·콜라이더를 표로 기록하고 Claude에게 돌려준다(Design 수정 금지) |
| T5 | feature | `Tools/Check-RoomRules.py` 규칙 보강 (1) 서 있는 면 가장자리에서 가로 **0.9 이내**의 위 구조물까지 머리 여유 2.2 미만이면 FAIL(블록·발판 가로 간격 1.0 이상 규칙) (2) 적 위치가 솔리드 안이거나 서 있는 면이 없으면 FAIL, 순찰 구간이 서 있는 면을 벗어나면 FAIL (3) **포고만으로 오르는 구간**: 오름 ≥4인 구간 안에 작은 발판(폭≤2.5)이 하나도 없거나, 연속한 등불 포고가 2개를 넘으면 WARN | draft3 데이터에서 FAIL 0. 일부러 만든 위반 JSON(블록 옆 가로 0.5 발판, 블록 안 적, 등불 3연속)이 각각 잡힘. `Validation/B18-room-rules-v5-A.md` 저장 |
| T6 | feature | 전달용 Development 빌드 갱신과 V5 런처 3개 | 세 런처 시작 로그 정상·10초 Error0. V4 런처 변화 0. Housekeeping(최신 1 + 직전 1) |
| T7 | doc | TASKS.md·README 갱신: 원인(데이터 실수 2건), 새 검사, 사용자 재플레이 항목(A02 블록·발판 구간 끼임 없음, A02 선반 적, 작은 발판 오르기 난이도, 등불 구간) | 결정 대기 8개 이하, 깨진 링크 0 |
| T8 | verification | 마지막 점검: 기본 회귀 13/13 1회, Batch-Card, 모든 런처 시작 로그 | 허용 범위 밖 0, 깨진 링크 0 |

## Backlog (본 Task 뒤)
1. (verification) 자동 입력 경로 `-verify-b16-route` 재실행: A03 동쪽(작은 발판 3개), A04 상부(블록→발판→작은 발판→걸어서 도착)를 포함해 이번에는 **구간별 막힌 원인을 접촉 콜라이더 이름으로 기록**한다. 구간당 120초, 측정만 한다. — 합격: 통과 구간 목록, 막힌 위치·상황 원문
2. (doc) `Validation/B18-v5-draft2-vs-draft3.md`: 방별 등불 수·작은 발판 수·블록 수·머리 여유 최솟값·가로 간격 최솟값 비교표. 계산만. — 합격: 합계 일치
3. (verification) 실제 점프 한계 측정 `-verify-b18-hop`(측정만, 수치·튜닝 수정 금지): 평지 기준으로 오름 0 / +1.0 / +1.5 / +2.0 / +2.5에서 건널 수 있는 **최대 가로 간격**을 0.25 단위로 입력 점프(달리기 점프, 제자리 점프 각각)로 측정해 표로 기록. 같은 방식으로 등불 포고 1회 후 도달 높이와 가로 거리. — 합격: `Validation/B18-hop-limits.md`에 표와 원문. 이 표는 Claude가 작은 발판 간격(현재 가로 1.0~1.5, 오름 1.5~2.0)과 간격 규칙 3.0을 다시 정하는 근거로 쓴다
4. (verification) 능력 보유 경로 `-verify-b16-route` 확장: DJ를 가진 상태로 A01 천장 구멍 → A06 → 가시 포고 → 왼쪽 조각 → 일방 문 → A02 감실 → 조각. 구간당 120초, 막힌 원인은 접촉 콜라이더 이름으로 기록. 측정만 한다. — 합격: 통과 구간 목록, 막힌 위치·상황 원문

Backlog는 1 → 2 → 3 → 4 순서. 앞이 BLOCKED여도 뒤는 계속한다. Backlog까지 끝나면 시간이 남아도 종료한다.

## 종료 처리
- 이번 배치 커밋만 `git reset --soft <Base>`로 합쳐 하나로 만들고(Base 이전 커밋은 건드리지 않는다) `B18` 태그를 단다.
- `Validation/B18-summary.md`: Task별 결과(합격/BLOCKED/SKIPPED), 유형별 개수, T1 원인 분류, 열린 항목, 사용자가 해야 할 일. Codex 요약과 Validation 로그가 다르면 로그 원문을 우선한다.
- push는 사용자가 한다.
