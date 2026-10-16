이번 실행은 Batch 17(V5 층 A 1차 플레이 피드백 반영: 데이터 draft2 재생성·원인 측정·검사 규칙 추가)만 수행한다. AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다. push는 하지 않는다.

## 목적
- 사용자 직접 플레이(2026-10-16)에서 나온 3건을 처리한다. (1) A02에서 처음 오른쪽으로 갔다가 왼쪽으로 돌아갈 수 없음 (2) A05에서 카메라가 방 중앙에 고정됨 (3) 일부 구간 천장이 캐릭터 키보다 낮음. 추가 요청: (4) 몹이 나오는 방은 바닥 평지에도 구덩이를 넣어 점프가 어느 정도 들어가게 한다(draft2에 반영됨).
- (3)과 (2)의 추정 원인은 Claude가 설계 데이터(`Design/v5/` draft2)에서 고쳤다. **이번 배치는 그 데이터로 Scene을 다시 만들고, (1)(2)의 원인을 측정으로 확인하고, 검사 규칙을 추가한다.**
- 방 구조·수치·좌표는 고치지 않는다(`Design/` 읽기 전용). 데이터 문제가 보이면 기록해 Claude에게 돌려준다.

## 읽을 파일
AGENTS.md, TASKS.md(2절·3절·최신 인계), `Design/v5/WORLD_V5.md`(특히 3절 규칙, **9절 draft2 변경**), `Design/v5/rooms_v5_A.json`, `Design/v5/world_v5_A.json`, `Design/v5/ascii_A.txt`, `Validation/B16b-summary.md`, `Validation/B16b-handoff.md`, `Assets/Scripts/RoomCameraRig.cs`, `Assets/Scripts/RoomSession.cs`, `Assets/Scripts/RoomExit.cs`, `Assets/Scripts/V5Launch.cs`.

## 시작 조건
- 작업 트리 변경이 `Batches/BATCH-17.md`(미추적)와 `Design/v5/` 아래 5개 파일(rooms_v5_A.json, world_v5_A.json, WORLD_V5.md, ascii_A.txt, floorA_v5.png)뿐이어야 한다. 그 외 변경이 있으면 시작하지 않고 멈춘다.
- 위 파일만 먼저 커밋한다("docs: V5 floor A draft2 (headroom, camera height, trench floor) and Batch 17 instructions"). 이 커밋 직후 HEAD가 Base다. 시작 시각과 Base를 기록한다.
- push 안 된 이전 커밋이 있어도 정상이다.

## 예산·큐 규칙
- 예산: Task(Backlog 포함) 최대 10개 또는 **5시간** 중 먼저 도달하는 쪽. 4시간 30분이 지나면 새 Task를 시작하지 않고 종료 처리(회귀·카드·요약)를 한다.
- Task가 BLOCKED되면 그 Task 변경만 되돌리고 다음으로 간다. 선행이 BLOCKED면 SKIPPED.
- 재시도는 Task당 같은 문제 1회. 두 번째 실패는 되돌리고 실패 기록(원문 포함)만 커밋한다.
- 회귀는 `Tools/Run-Regression.ps1 -Quiet`(Task마다), 마지막에 기본 방식 1회.
- 기존 Validation은 덮어쓰지 않고 새 파일(`B17-*`)로 저장한다.
- **C# 문자열에 Windows 경로·역슬래시를 직접 쓰지 않는다**(`Path.Combine`과 `/`, 필요하면 `@"..."`). 코드를 고칠 때마다 컴파일을 확인한다.
- Batch Backlog가 끝나면 TASKS 8절 Approved Backlog를 확인하고 비어 있으면 종료한다.

## 사전 결정
| 결정 | 답 | 막히면 대체(비우면 멈춤) |
| --- | --- | --- |
| 데이터 | `Design/v5/` draft2. A05 높이 12, A04 world y 11~23, A03→A04 위쪽 문 y13 | |
| 출력 | `Assets/Scenes/V5/` 6방 덮어쓰기 재생성(V5 폴더만). V4 Scene·Resources 변경 0 | |
| 원인 측정 순서 | **T1은 Scene 재생성 전**의 현재 V5 Scene으로 먼저 측정한다. 재생성 후 T3에서 같은 측정을 반복한다 | |
| 플레이어 크기 | `Assets/Prefabs/Player.prefab` BoxCollider2D 0.7×1.6. 코드에 상수로 박지 않고 프리팹에서 읽는다 | 읽기 실패 시 0.7×1.6을 쓰고 요약에 적는다 |
| 카메라 화면 높이 | `RoomCameraRig.orthographicSize`×2 = 11 | |

## 허용 범위
`Assets/Scenes/V5/**`, V5 생성기·검증 코드(Assets/Editor, Assets/Scripts/Verification, `V5*.cs`), `Tools/Check-RoomRules.py`, `Launchers/current/*V5*`, `Validation/B17-*`, `Batches/BATCH-17.md`, TASKS.md, README.md, 빌드 산출물.
수정 금지: `Design/`(읽기 전용), V4 Scene·Resources·V4 런처, `PlayerTuning.asset` 등 수치 에셋, 보스·층 B·C, 이전 `Validation/B*-*.md`. **`RoomCameraRig.cs`·`PlayerMotor.cs`·`RoomSession.cs`는 T1 원인 측정에서 "생성기·Scene 연결이 아니라 이 파일의 로직이 원인"으로 확인된 경우에 한해 최소 수정 1건만 허용한다.** 그 경우 변경 전후 V4 회귀 13/13과 V4 방 SHA 변화 0을 증명한다. 확인되지 않으면 고치지 않고 기록한다.

## Task
| Task | 유형 | 내용 | 합격 기준 |
|---|---|---|---|
| T1 | verification | **현재(재생성 전) V5 Scene** 원인 측정. (a) A02: `A01-A02` 도착에서 입력으로 오른쪽 x≈10까지 이동(첫 발판 위 포함) 후 왼쪽으로 x≈1.5까지 이동. 프레임마다 플레이어 위치·속도·접지·충돌한 콜라이더 이름·문 트리거 진입 여부를 기록. 멈춘 지점과 원인 후보(막는 콜라이더, 문 잠금, 도착 직후 입력 잠금 등) (b) A05: `A04-A05`로 떨어져 들어온 직후 5초와 사당(대시 획득) 직후 5초 동안 카메라 위치·Follow 대상·Confiner 상태·플레이어 입력 가능 여부를 프레임마다 기록, 화면 캡처 2장 (c) 방 높이 11 이하 방이 카메라 위치가 고정되는지 비교: A05(10)와 A04(12)에서 같은 측정 | `Validation/B17-probe.md`에 표와 원문. 원인이 "코드 로직"인지 "Scene 연결·데이터"인지 한 줄로 분류. 코드·Scene 변경 0 |
| T2 | feature | V5 6방 Scene 재생성(draft2) + 요소 수 표 | 방 6개 생성, 방별 요소 수(고체·발판·적·등불·가시·문·봉인·부서지는 벽·조각·**바닥 구덩이 6개: A01 1, A02 1, A03 2, A04 1, A05 1**)가 데이터와 같음(표). **A04 도랑 구간(x22~40)에 바닥 고체 없음(도랑 바닥 y0.4), A05 도랑(x6~12) 같음.** V4 SHA256 변화 0 |
| T3 | verification | T1 (a)(b)(c)를 **재생성 후 Scene**에서 반복하고 `-verify-b16-v5` 문 전환 검사 재실행 | 결과를 `B17-probe.md`에 "재생성 후" 절로 이어 씀. (a) 오른쪽 갔다 왼쪽 복귀 가능/불가 (b) A05에서 카메라가 플레이어를 따라가는지(방 중앙 고정 아님) (c) 문 14방향·stub 닫힘·일방 문 전환 모두 기존과 같음 (d) 구덩이 6개: 구덩이에 빠진 뒤 점프로 올라올 수 있음(올라오는 데 걸린 시간 기록), 구덩이 앞에서 달려 건너기 가능 |
| T4 | feature | `Tools/Check-RoomRules.py`에 규칙 추가: (1) **머리 위 여유**: 서 있는 면(솔리드·발판 윗면) 위에서 겹치는 솔리드·발판 아랫면까지 거리 <2.2면 FAIL(맞닿은 솔리드는 제외). 키는 프리팹 값 사용 (2) **방 높이** <12면 FAIL (3) 바닥 솔리드가 hazard·도랑 구간을 덮으면 FAIL (4) 순찰 구간이 바닥 구덩이(바닥 y0~0.4 구간)를 가로지르면 FAIL, 구덩이 폭 3.0 초과 FAIL | draft2 데이터로 위반 0. 일부러 만든 위반 JSON(발판 간격 1.4 · 방 높이 10 · 도랑 위 바닥 · 구덩이 위를 지나는 순찰)이 각각 FAIL로 잡힘. `Validation/B17-room-rules-v5-A.md` 저장 |
| T5 | feature | 전달용 Development 빌드 갱신과 V5 런처 3개(`Play-V5.cmd`, `Play-V5-AfterDash.cmd`, `Play-V5-DJ.cmd`) | 세 런처 시작 로그 정상·10초 Error0. V4 런처 변화 0. Housekeeping(최신 1 + 직전 1) |
| T6 | doc | TASKS.md·README 갱신: 이번 피드백 3건의 원인(측정 결과)과 상태, 사용자 재플레이 항목(A02 오른쪽→왼쪽 복귀, A05 카메라, 낮은 곳 없는지) | 결정 대기 8개 이하, 깨진 링크 0 |
| T7 | verification | 마지막 점검: 기본 회귀 13/13 1회, Batch-Card, 모든 런처 시작 로그 | 허용 범위 밖 0, 깨진 링크 0 |

## Backlog (본 Task 뒤)
1. (verification) 자동 입력 경로 `-verify-b16-route`: A01 시작 → A02 바닥 → A03 거리 → 동쪽 위(블록+발판+적+등불 포고) → A04 위 통로(블록+발판+적+등불) → 구멍 → A05 → 대시 획득. 구간당 120초. 막히면 그 구간만 BLOCKED(수치 수정 금지, 막힌 방·좌표·상황 기록). 측정만 한다. — 합격: 통과 구간 목록과 막힌 위치, FAIL/막힘 줄 원문
2. (doc) `Validation/B17-v5-draft1-vs-draft2.md`: 방 크기·총면적·적 수·등불 수·블록 수·머리 위 여유 최솟값 비교표. 계산만. — 합격: 합계 일치

## 종료 처리
- 이번 배치 커밋만 `git reset --soft <Base>`로 합쳐 하나로 만들고(Base 이전 커밋은 건드리지 않는다) `B17` 태그를 단다.
- `Validation/B17-summary.md`: Task별 결과(합격/BLOCKED/SKIPPED), 유형별 개수, T1 원인 분류, 열린 항목, 사용자가 해야 할 일. Codex 요약과 Validation 로그가 다르면 로그 원문을 우선해 적는다.
- push는 사용자가 한다.
