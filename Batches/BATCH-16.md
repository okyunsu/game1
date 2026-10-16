이번 실행은 Batch 16(V5 층 A 파일럿 방 생성)만 수행한다. AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다. push는 하지 않는다.

## 목적
- Claude가 작성한 V5 층 A 데이터(`Design/v5/`)로 방 6개(A01~A06)를 만들어 직접 플레이할 수 있는 개발 빌드를 낸다. 플레이한 뒤 사용자가 수정 방향을 정한다.
- 이번 배치는 **생성·검사·전달**만 한다. 방 구조·수치·좌표를 고치지 않는다(`Design/` 읽기 전용). 데이터 문제가 보이면 고치지 말고 기록해 Claude에게 돌려준다.
- 층 B·C, V4 방, 보스, 게임 규칙은 건드리지 않는다.

## 읽을 파일
AGENTS.md, TASKS.md(2절·3절·최신 인계), `Design/v5/WORLD_V5.md`(전체), `Design/v5/rooms_v5_A.json`, `Design/v5/world_v5_A.json`, `Design/v5/ascii_A.txt`, `Design/v5/floorA_v5.png`(도면), Batch 15 요약(`Validation/B15-summary.md`)의 구현·데이터 계약 절.

## 시작 조건
- HEAD가 B15 태그를 포함해야 한다(`git merge-base --is-ancestor B15 HEAD`). B15 위에 push되지 않은 커밋이 있어도 정상이다.
- 미추적 파일이 `Batches/BATCH-16.md`와 `Design/v5/` 아래 파일뿐이어야 한다. 그 외 변경이 있으면 시작하지 않고 멈춘다.
- 시작하면 위 미추적 파일만 먼저 커밋한다("docs: add V5 floor A design data and Batch 16 instructions"). 이 커밋 직후 HEAD가 Base다. 시작 시각과 Base를 기록한다.

## 예산·큐 규칙
- 예산: Task(Backlog 포함) 최대 10개 또는 **6시간** 중 먼저 도달하는 쪽. 시작 후 5시간 30분이 지나면 새 Task를 시작하지 않고 진행 중인 것만 끝낸 뒤 종료 처리(회귀·카드·요약·합치기)를 한다. Backlog까지 끝나면 시간이 남아도 종료한다.
- Task가 BLOCKED되면 그 Task 변경만 되돌리고 다음으로 간다. 선행이 BLOCKED면 SKIPPED.
- 재시도는 Task당 같은 문제 1회, 검사 도구 보정은 Task당 1회. 두 번째 실패는 되돌리고 실패 기록(원문 포함)만 커밋한다.
- 회귀는 `Tools/Run-Regression.ps1 -Quiet`(Task마다), 마지막에 기본 방식 1회.
- 기존 Validation은 덮어쓰지 않고 새 파일로 저장한다.
- Batch Backlog가 끝나면 TASKS 8절 Approved Backlog를 확인하고, 비어 있으면 종료한다. 7절 "다음 배치 후보"는 승인된 작업이 아니다.

## 사전 결정
| 결정 | 답 | 막히면 대체(비우면 멈춤) |
| --- | --- | --- |
| 입력 데이터 | `Design/v5/rooms_v5_A.json`(방), `Design/v5/world_v5_A.json`(배치·문). 형식은 V4 `rooms_v2.json`/`world_v7.json`과 같다. 기존 생성기 인자(`-room-design`, `-room-output`, `-map-design`)로 넣는다 | |
| 출력 위치 | Scene은 `Assets/Scenes/V5/<ID>_V5.unity`. **`Assets/Scenes/V4/`는 건드리지 않는다**(V4 21개 SHA256 불변) | |
| V5 실행 모드 | 인자 `-v5`. `-v4`와 같은 구조(방 전환·저장·지도·안내)로 V5 방 6개를 쓴다. 방 목록·지도 레이아웃은 V5용 데이터(`Assets/Maps/world_v5_A.json`)에서 읽는다. V4 실행은 변화 0 | 기존 코드가 `-v4`에 강하게 묶여 있으면 `-v4`와 같은 심볼로 처리하고 `-v5`일 때 데이터 선택만 바꾼다. 방법을 TASKS에 기록 |
| 새 문 종류 `oneway` | world 문 `type:"oneway"`(a→b 한쪽만). a 방의 문(`"oneway":"out"`)은 열려 있고 b 방(`"oneway":"in"`)의 문 자리는 이쪽에서 통과 불가이며 도착 지점(spawn)만 있다 | |
| 문 `stub:true` | 반대편 방이 없다. 문 표식(능력 약자·색)은 그리되 **열리지 않는다**(접촉해도 전환 0). 지도에서는 "?" 대신 닫힌 문 표시(Backlog 1) | |
| 적 `patrol:[x0,x1]` | 방 로컬 x 범위. 적 순찰이 이 범위를 넘지 않는다. 기존 적 프리팹이 범위를 지원하지 않으면 기존 방식(스폰 위치 기준 기본 순찰)으로 생성하고, **기본 순찰이 patrol 범위를 벗어나는 적의 목록**을 `Validation/B16-patrol-gap.md`에 기록한다(코드·프리팹 변경 금지) | |
| ID별 특수 처리 | V4 생성 코드가 방 ID·좌표를 직접 쓰는 곳(예: A05 대시 사당, 조각, 체크포인트)은 먼저 목록으로 `Validation/B16-hardcoded.md`에 정리한다. V5에서도 같은 ID는 같은 역할로 만들되, **위치는 새 데이터의 `marks`·`seal`·`breakable`·`pogo`를 따른다.** 코드가 V4 좌표를 고정으로 쓰고 있어 새 데이터와 어긋나면 데이터를 따르게 최소 수정하고 기록한다 | 따르게 만들 수 없는 항목은 생략하고 요약에 적는다(예: 조각 위치) |
| 아이템·조각 | 위치 원본은 `marks`의 글(`조각`, `사당`)과 좌표. V4와 같은 아이템 종류·보상 처리를 쓴다 | |
| 능력 문 | gate(DJ·Dash·Slam·WallJump) 규칙은 V4와 같다. `stub`이 아닌 gate 문은 A01-A06(DJ)뿐이다 | |
| 시작 지점 | A01의 `start` | |
| 능력 fixture(플레이용) | 개발 빌드 인자로 시작 방과 보유 능력을 줄 수 있게 한다(`-start-room=A03 -give=dash` 등). 기존 B11/B12 fixture 방식이 있으면 그것을 확장하고, 새 저장 필드는 만들지 않는다 | 기존 방식이 맞지 않으면 런처 전용 임시 저장 파일을 쓰고 그 사실을 기록 |
| 지도 | Batch 15 지도를 V5 레이아웃으로 사용(`-map-layout=world_v5_A`). 좌표 계산·변경 없음 | |
| 건드리지 않는 것 | PlayerTuning·보스·V3 안내 규칙·저장 형식·V4 Scene/방 목록 | |

## 허용 범위
수정 가능: Assets/Scripts(생성기 입력 확장, V5 방 목록, 문 종류 oneway/stub, 적 patrol 처리, Verification 폴더), Assets/Editor, Assets/Scenes/V5(새 폴더), Assets/Maps(world_v5_A.json 추가), Assets/Resources 아래 **V5 전용 새 파일**(V4 파일 수정 금지), Launchers/current(V5 런처 추가, 기존 런처는 경로 갱신만), Tools/Check-RoomRules.py, README.md(V5 실행 2줄 이내), TASKS.md, Validation/(B16-*.md + _raw), Builds(Housekeeping), Batches/BATCH-16.md
수정 금지: Assets/Scenes/V4, Assets/Scenes 기타, Assets/Prefabs(기존), Assets/ScriptableObjects, Assets/BossCandidates, Assets/Candidates, **Design/(v5 포함, 읽기 전용)**, GAME_DESIGN.md, PRD.md, AGENTS.md, Packages, ProjectSettings(스크립트 정의 심볼 제외), Archive/

## Task
유형은 feature / verification / cleanup / doc 중 하나다. 요약 파일에 유형별 개수를 적는다.

| Task | 유형 | 내용 | 합격 기준 |
|---|---|---|---|
| T1 | feature | 생성기 확장: V5 데이터 입력, 문 종류 `oneway`·`stub`, 적 `patrol`, 방 목록·지도 레이아웃의 V5 선택(`-v5`). 먼저 `Validation/B16-hardcoded.md`(방 ID·좌표 고정 위치 목록)를 작성 | V4 21개 Scene·Resources 기존 파일 SHA256 변화 0(기본 설정 재생성 포함), V4 회귀 13/13. 임시 출력(`Assets/Scenes/_tmp-gen-check/`)으로 V5 데이터를 생성해 오류 0, 확인 후 임시 폴더 삭제 |
| T2 | feature | V5 방 6개 Scene 생성(`Assets/Scenes/V5/`)과 `-v5` 실행(방 전환·문·스폰·저장·지도·적·장치·아이템) | 방 6개 생성. 방별 요소 수(고체·발판·적·등불/가시·문·봉인·부서지는 벽·장치)가 데이터와 같음(표로 기록). `stub` 문 4개(A01-Z01, A05-Z01, A02-B05, A03-B01)는 닫힘 |
| T3 | verification | `-verify-b16-v5`(개발 빌드 전용)와 실제 exe 실행: 문 전환 검사 | (a) 비-stub 문 전환이 모두 되고 도착 지점이 맞음(A01↔A02, A02↔A03 아래·위, A03→A04, A04 구멍→A05, A05 벽 지름길→A03) (b) stub 문 4개 전환 0 (c) A06→A02 일방: A06에서 나가면 A02 감실에 도착, A02 감실 쪽 문으로는 A06에 못 들어감 (d) A01 천장 구멍(DJ)은 DJ 보유 시만 열림 (e) 10초 Error/Exception 0 |
| T4 | verification | `Tools/Check-RoomRules.py`에 `WORLD_V5.md` 3절의 규칙을 구현하고 층 A 결과를 `Validation/B16-room-rules-v5-A.md`로 저장. 규칙은 FAIL/WARN으로 나눈다. 가짜 위반 데이터(임시 파일)로 각 규칙이 잡히는지 자체 검사 1회 | 층 A 6방 표(면적·요소 수·1,000u²당 요소 수·문 수·판정). 자체 검사에서 위반 데이터의 **문 안전 구역·겹침·높이차** 위반이 검출됨. **데이터 위반이 나오면 데이터를 고치지 말고 그대로 기록**(Design 읽기 전용). Python이 없으면 SKIPPED |
| T5 | feature | 전달용 Development 빌드 갱신과 V5 런처: `Play-V5.cmd`(A01 시작), `Play-V5-AfterDash.cmd`(A03 시작, 대시 보유), `Play-V5-DJ.cmd`(A01 시작, 더블 점프 보유 — A06 확인용), 모두 `-NoMap` 스위치 지원. Release 빌드에는 V5 지도·검증 코드를 넣지 않는다(B14 방식) | 세 런처 시작 로그 정상·10초 Error0. 기존 V4 런처(Play-V4 등) 변화 0·시작 로그 정상. Housekeeping(최신 1 + 직전 1) |
| T6 | doc | TASKS.md·README 갱신(아래 TASKS 갱신 절) | 깨진 링크 0, 결정 대기 8개 이하 |
| T7 | verification | 마지막 점검: 기본 회귀 13/13 1회, Batch-Card, 모든 런처 시작 로그 | 허용 범위 밖 0, 깨진 링크 0, 없는 빌드·런처 0 |

## Backlog (본 Task 뒤 위에서부터)
1. (feature) 지도 보강: `stub` 문은 닫힌 문 표시(×), `oneway`는 화살표(a→b)로 표시. 지도 데이터에 b 방이 없는 문은 "?"를 그리지 않는다. — 합격: `-map-all`에서 V5 6방·문 12개(stub 4·oneway 1 포함) 표시가 데이터와 일치(표 기록), V4 지도 변화 0, 회귀 13/13
2. (verification) 자동 입력 경로 검사 `-verify-b16-route`: 실제 입력(점프·이동·아래 공격)으로 A01 시작 → A02 바닥 통로 → A03 거리 → 동쪽 위(발판+등불 포고) → A04 위 통로 → 구멍 → A05 → 대시 획득까지 진행. 한 번에 안 되면 구간별(방 단위) 결과를 기록한다. 시간 제한 구간당 120초. 도중 막히면 그 구간만 BLOCKED로 기록(발판 간격·포고 높이 수치 자체를 고치지 않음, 막힌 위치와 원인을 기록). — 합격: 통과한 구간 목록과 막힌 위치(방·좌표·상황)를 기록. 로그에 FAIL/막힘 줄 원문 포함. 합격/불합격과 무관하게 **측정 결과만** 남긴다
3. (doc) V4 대 V5 비교표 `Validation/B16-v4-vs-v5-floorA.md`: 층 A 6방의 방 크기·총면적·적 수·장치 수·문 수·1,000u²당 요소 수를 V4(rooms_v2.json)와 V5(rooms_v5_A.json)로 나란히 표시. 데이터에서 계산만 한다(판정·의견 없음). — 합격: 두 데이터 합계와 표 합계 일치

Backlog는 1 → 3 순서. 앞이 BLOCKED여도 뒤는 계속한다.

## TASKS 갱신 (T6)
- D-V5-PILOT 행에 설계 데이터 위치(`Design/v5/`)와 Batch 16 생성 사실을 덧붙인다(결정·날짜 칸은 바꾸지 않는다). 새 결정 행은 만들지 않는다.
- 현재 상태: V5 층 A 파일럿 REVIEW(생성·자동 검증 완료, 직접 플레이 대기).
- Latest Handoff "직접 플레이 확인"에 **V5 플레이 목록**을 쓴다(약 20분): `Play-V5.cmd` → A01(등불 포고로 높은 선반, 천장 구멍의 빛, 금 간 바닥 확인) → A02(아래 통로·중간 선반·감실) → A03(두 갈래 올라가기, 대시 문 확인) → A04(위 통로와 도랑) → A05(대시 획득·벽 부수기·지름길) → `Play-V5-AfterDash.cmd`로 대시 문 확인 → `Play-V5-DJ.cmd`로 A06(더블 점프 입장, 나가는 일방 문) → 필요하면 `-NoMap`.
- 사용자가 플레이 뒤 결정할 질문은 `Design/v5/WORLD_V5.md` 7절을 링크로 가리킨다(복사 금지).
- Playtest Records·Before-After 빈 틀은 만들지 않는다(사용자 기록).
- 열린 버그·결정 대기 수는 실제 값만 적는다.

## 배치 공통 합격 기준
- Task마다 -Quiet 13/13. 마지막에 기본 13/13 1회.
- `Tools/Batch-Card.ps1 -Base <시작 HEAD> -Allowed <위 허용 범위>`: 허용 범위 밖 0, TASKS 깨진 링크 0, 없는 빌드·런처 0.
- V4 Scene 21개·V4 Resources 기존 파일 SHA256 불변. Release 빌드에 V5 지도·검증 코드 없음.

## 종료
- Task별 로컬 커밋. 종료 시 `origin/main..HEAD`가 Base 이후의 이번 배치 커밋뿐이면 `git reset --soft <Base>`로 합쳐 "Batch 16: V5 floor A pilot" 한 커밋으로 만들고 태그 B16을 붙인다. Base 앞에 push되지 않은 커밋이 있으면 합치지 않는다.
- `Validation/B16-summary.md` 맨 위에 회귀·카드 결과, 유형별 Task 개수, 아래 세 가지를 쓴다.
  1. 기획자용 3문단: 어떻게 테스트됐나(자동 검증이 못 보는 것 포함) / 어떻게 발전되나 / 기획자의 역할.
  2. 데이터 문제 목록(데이터를 고치지 않고 Claude에게 돌려줄 항목: 검사기 FAIL/WARN, patrol 기본 순찰 이탈, 막힌 경로 위치 등).
  3. FAIL/막힘 줄 원문(요약 문장으로 대체하지 않는다).
