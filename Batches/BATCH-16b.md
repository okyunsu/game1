이번 실행은 Batch 16b(Batch 16 이어하기: V5 층 A 방 생성·전달)만 수행한다. AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다. push는 하지 않는다.

## 목적
- Batch 16에서 T1(생성기 확장)이 C# 컴파일 오류로 BLOCKED되어 V5 Scene·빌드·런처가 만들어지지 못했다. 이번 배치는 **같은 목표를 작은 단계로 다시 진행**한다. Batch 16의 Task 번호와 합격 기준을 그대로 잇는다.
- 방 구조·수치·좌표는 고치지 않는다(`Design/` 읽기 전용). 데이터 문제가 보이면 기록해 Claude에게 돌려준다.
- 층 B·C, V4 방, 보스, 게임 규칙은 건드리지 않는다.

## 읽을 파일
AGENTS.md, TASKS.md(2절·3절·최신 인계), `Batches/BATCH-16.md`(Task 상세·사전 결정·TASKS 갱신 절 — 이번 배치의 기준 문서이며 아래에서 달라진 부분만 이 문서가 우선), `Design/v5/WORLD_V5.md`, `Design/v5/rooms_v5_A.json`, `Design/v5/world_v5_A.json`, `Design/v5/ascii_A.txt`, `Validation/B16-summary.md`(실패·보정 원문), `Validation/B16-hardcoded.md`(ID·좌표 점검 — 입력으로 사용), `Validation/B16-room-rules-v5-A.md`.

## Batch 16과 달라진 점
1. 설계 데이터가 고쳐졌다(Claude): A05 도착 지점 x=3.5(문 쪽), A04 높이 12(world rect 포함), A02 감실·굴뚝 아래 선반에 `조각` 표식 추가, world 문 ID `A02-A06` 통일. `Design/v5/`의 수정된 4개 파일이 작업 트리에 있다.
2. 생성기 확장을 **한 번에 하지 않는다.** 아래 T1을 a→b→c로 나누고, 코드를 고칠 때마다 컴파일을 확인한 뒤 다음으로 간다.
3. **C# 문자열에 Windows 경로·역슬래시를 직접 쓰지 않는다.** 경로는 `Path.Combine`과 슬래시(`/`)로 만들고, 정규식·역슬래시가 필요하면 `@"..."` 축자 문자열을 쓴다. 이 규칙은 B16 T1 컴파일 오류(`B09Authoring.cs(51,…) CS1056/CS1010`)의 직접 원인 대응이다.
4. 검사기(T4)는 등불·가시 포고 노드를 계산에 넣는다.

## 시작 조건
- HEAD가 B16 태그를 포함해야 한다(`git merge-base --is-ancestor B16 HEAD`). B16 위에 push되지 않은 커밋이 있어도 정상이다(push 안 된 `6860a5f`와 `7a0dc19` 등).
- 작업 트리 변경이 `Batches/BATCH-16b.md`(미추적)와 `Design/v5/` 아래 수정된 파일뿐이어야 한다. 그 외 변경이 있으면 시작하지 않고 멈춘다.
- 위 파일만 먼저 커밋한다("docs: fix V5 floor A design data and add Batch 16b instructions"). 이 커밋 직후 HEAD가 Base다. 시작 시각과 Base를 기록한다.

## 예산·큐 규칙
- 예산: Task(Backlog 포함) 최대 12개 또는 **6시간** 중 먼저 도달하는 쪽. 시작 후 5시간 30분이 지나면 새 Task를 시작하지 않고 진행 중인 것만 끝낸 뒤 종료 처리(회귀·카드·요약·합치기)를 한다. Backlog까지 끝나면 시간이 남아도 종료한다.
- Task가 BLOCKED되면 그 Task 변경만 되돌리고 다음으로 간다. 선행이 BLOCKED면 SKIPPED.
- 재시도는 Task당 같은 문제 1회, 검사 도구 보정은 Task당 1회. 두 번째 실패는 되돌리고 실패 기록(원문 포함)만 커밋한다. **T1은 하위 단계(a·b·c)마다 따로 센다.**
- 회귀는 `Tools/Run-Regression.ps1 -Quiet`(Task마다), 마지막에 기본 방식 1회.
- 기존 Validation은 덮어쓰지 않고 새 파일(`B16b-*`)로 저장한다. `B16-*` 파일은 수정하지 않는다.
- Batch Backlog가 끝나면 TASKS 8절 Approved Backlog를 확인하고 비어 있으면 종료한다. 7절 "다음 배치 후보"는 승인된 작업이 아니다.

## 사전 결정
`Batches/BATCH-16.md`의 사전 결정 표를 그대로 따른다(입력 데이터, 출력 `Assets/Scenes/V5/`, `-v5` 모드, `oneway`·`stub`·`patrol`, 능력 문, 시작 지점, fixture, 지도). 이번에 추가·구체화하는 것은 아래뿐이다.

| 결정 | 답 | 막히면 대체(비우면 멈춤) |
| --- | --- | --- |
| T1a 프로브 | 코드 수정 없이 기존 생성기 CLI(`-room-design=Design/v5/rooms_v5_A.json -room-output=Assets/Scenes/_tmp-gen-check -map-design=Design/v5/world_v5_A.json`)로 V5 데이터를 임시 폴더에 생성해 본다. 성공하면 결과를, 실패하면 오류 원문을 `Validation/B16b-probe.md`에 기록한다. **이 단계에서는 제품 코드를 바꾸지 않는다** | |
| `stub` 문 | 반대편 방이 없는 문. 프로브 오류에서 "대상 방 없음"류가 나오면 그 처리를 T1b에서 최소 수정으로 넣는다 | |
| `oneway` 문 | `Batches/BATCH-16.md` 사전 결정과 같다. 방 데이터 쪽 문은 `"oneway":"out"`(A06) / `"in"`(A02) | |
| `patrol` | 기존 E1 프리팹의 `leftPoint`/`rightPoint`는 null이다. **프리팹은 바꾸지 않는다.** 생성 시 Scene 안에 순찰 끝점용 빈 Transform 2개를 만들어 인스턴스의 필드에 연결한다(원본 Scene·Prefab 변경 0, V4 방과 같은 적은 기존 방식 유지). 연결할 수 없으면 기존 방식으로 생성하고 `Validation/B16-patrol-gap.md`를 **새 이름 `B16b-patrol-gap.md`**로 기록 | |
| 검사 도구 인자 | `Tools/Check-RoomRules.py` 기본 입력은 그대로 두고 추가 인자 없이도 층 A 검사가 돌아야 한다 | |

## 허용 범위
`Batches/BATCH-16.md`의 허용 범위와 같다. 추가: Validation/B16b-*.md, Batches/BATCH-16b.md.
수정 금지: `Batches/BATCH-16.md`의 수정 금지 목록과 같다(Design/ 읽기 전용 포함). **`Validation/B16-*.md`는 읽기만 한다.**

## Task
유형은 feature / verification / cleanup / doc 중 하나다. 요약 파일에 유형별 개수를 적는다.

| Task | 유형 | 내용 | 합격 기준 |
|---|---|---|---|
| T1a | verification | 프로브: 코드 수정 없이 V5 데이터를 기존 생성기로 임시 폴더에 생성해 보고 오류·경고를 기록 | `Validation/B16b-probe.md`에 성공/실패와 원문. V4 Scene·Resources 변경 0(git 비교). 임시 폴더는 확인 후 삭제 |
| T1b | feature | 프로브에서 나온 오류에 대응하는 **최소 수정**(stub 대상 방 없음, oneway, 문 종류 인식)을 한 번에 하나씩, 수정마다 컴파일 확인 | 컴파일 오류 0, V4 기본 재생성 SHA256 변화 0(21 Scene + Resources 기존 파일), 임시 출력으로 V5 데이터 생성 오류 0 |
| T1c | feature | 적 `patrol` 연결(위 사전 결정) | V4 기본 재생성 SHA256 변화 0, 임시 출력에서 지정 patrol 있는 적의 끝점 연결 수가 데이터와 같음(표) |
| T2 | feature | V5 방 6개 Scene 생성(`Assets/Scenes/V5/`)과 `-v5` 실행(방 전환·문·스폰·저장·지도·적·장치·아이템) | 방 6개 생성. 방별 요소 수(고체·발판·적·등불/가시·문·봉인·부서지는 벽·장치·**조각 아이템**)가 데이터와 같음(표). `stub` 문 4개 닫힘. **A02 조각 2개(감실·굴뚝 아래 선반)와 A04 섬·A06 조각이 생성됨** |
| T3 | verification | `-verify-b16-v5`(개발 빌드 전용)와 실제 exe 실행: 문 전환 검사 | (a) 비-stub 문 전환이 모두 되고 도착 지점이 맞음(**A05↔A03 도착 x는 문 쪽(약 3.5)**, A01↔A02, A02↔A03 아래·위, A03→A04, A04 구멍→A05, A05 벽 지름길→A03) (b) stub 문 4개 전환 0 (c) A06→A02 일방: A06에서 나가면 A02 감실에 도착, A02 감실 쪽 문으로는 A06에 못 들어감 (d) A01 천장 구멍(DJ)은 DJ 보유 시만 열림 (e) 10초 Error/Exception 0 |
| T4 | verification | `Tools/Check-RoomRules.py` 개선과 재검사: (1) 등불·가시 포고 노드를 계산에 넣어 A01 높은 선반처럼 포고로 가는 곳이 일반 점프 그래프에서 끊긴 것을 "포고 경로"로 구분(타격 높이·튕김은 `Design/v5/WORLD_V5.md` 3절 수치 사용, 계산 방식을 파일 머리에 적는다) (2) 방마다 `spawns`의 도착 지점(`fall` 제외 + `fall` 착지점)을 시작점으로 도달성 검사 (3) 높이 20 초과는 위·아래 절반에 아이템 각 1개 이상 | `Validation/B16b-room-rules-v5-A.md` 저장(B16 파일은 유지). 가짜 위반 JSON 자체 검사 재통과. 데이터 위반은 고치지 않고 그대로 기록. Python 없으면 SKIPPED |
| T5 | feature | 전달용 Development 빌드 갱신과 V5 런처 3개(`Play-V5.cmd`, `Play-V5-AfterDash.cmd`, `Play-V5-DJ.cmd`), 모두 `-NoMap` 지원. Release에는 V5 지도·검증 코드 미포함(B14 방식) | 세 런처 시작 로그 정상·10초 Error0. 기존 V4 런처 변화 0·시작 로그 정상. Housekeeping(최신 1 + 직전 1) |
| T6 | doc | TASKS.md·README 갱신 | Batch 16 결과(T1 BLOCKED·Backlog SKIPPED)를 Batch 16b가 이어받았다고 쓴다. 결정 대기 8개 이하, 깨진 링크 0 |
| T7 | verification | 마지막 점검: 기본 회귀 13/13 1회, Batch-Card, 모든 런처 시작 로그 | 허용 범위 밖 0, 깨진 링크 0, 없는 빌드·런처 0 |

## Backlog (본 Task 뒤 위에서부터)
1. (feature) 지도 보강: `stub` 문은 닫힌 문 표시(×), `oneway`는 화살표(a→b)로 표시. b 방이 없는 문은 "?"를 그리지 않는다. — 합격: `-map-all`에서 V5 6방·문 12개 표시가 데이터와 일치(표), V4 지도 변화 0, 회귀 13/13
2. (verification) 자동 입력 경로 검사 `-verify-b16-route`: 실제 입력으로 A01 시작 → A02 바닥 통로 → A03 거리 → 동쪽 위(발판+등불 포고) → A04 위 통로 → 구멍 → A05 → 대시 획득까지 진행. 구간당 120초. 막히면 그 구간만 BLOCKED로 기록(수치 수정 금지, 막힌 방·좌표·상황 기록). — 합격: 통과 구간 목록과 막힌 위치 기록. FAIL/막힘 줄 원문 포함. 측정만 한다
3. (doc) `Validation/B16b-v4-vs-v5-floorA.md`: 수정된 데이터로 V4 대 V5 비교표 갱신(방 크기·총면적·적 수·장치 수·문 수·1,000u²당 요소 수). 계산만, 판정·의견 없음. — 합격: 두 데이터 합계와 표 합계 일치

Backlog는 1 → 3 순서. 앞이 BLOCKED여도 뒤는 계속한다.

## TASKS 갱신 (T6)
`Batches/BATCH-16.md`의 TASKS 갱신 절을 따르되 다음을 바꾼다.
- D-V5-PILOT 행에 "Batch 16(생성 BLOCKED) → Batch 16b에서 이어함"을 덧붙인다(결정·날짜 칸은 바꾸지 않는다).
- 플레이 가능 여부를 실제대로 쓴다. T2·T3·T5가 합격하기 전에는 "V5 플레이 가능"이라고 쓰지 않는다.
- Latest Handoff "직접 플레이 확인"의 V5 목록은 `Batches/BATCH-16.md`와 같다(T5 합격 시에만).
- 열린 버그·결정 대기 수는 실제 값만 적는다.

## 배치 공통 합격 기준
- Task마다 -Quiet 13/13. 마지막에 기본 13/13 1회.
- `Tools/Batch-Card.ps1 -Base <시작 HEAD> -Allowed <허용 범위>`: 허용 범위 밖 0, TASKS 깨진 링크 0, 없는 빌드·런처 0.
- V4 Scene 21개·V4 Resources 기존 파일 SHA256 불변. Release 빌드에 V5 지도·검증 코드 없음.

## 종료
- Task별 로컬 커밋. 종료 시 `origin/main..HEAD`가 Base 이후의 이번 배치 커밋뿐이면 `git reset --soft <Base>`로 합쳐 "Batch 16b: V5 floor A pilot (continued)" 한 커밋으로 만들고 태그 B16b를 붙인다. **Base 앞에 push되지 않은 커밋(`6860a5f`, `7a0dc19` 등)은 합치지 않는다.**
- `Validation/B16b-summary.md` 맨 위에 회귀·카드 결과, 유형별 Task 개수, 아래 세 가지를 쓴다.
  1. 기획자용 3문단: 어떻게 테스트됐나(자동 검증이 못 보는 것 포함) / 어떻게 발전되나 / 기획자의 역할.
  2. 데이터 문제 목록(데이터를 고치지 않고 Claude에게 돌려줄 항목: 검사기 FAIL/WARN, patrol 연결 실패, 막힌 경로 위치 등).
  3. FAIL/막힘 줄 원문(요약 문장으로 대체하지 않는다).
- T2·T3·T5 중 하나라도 합격하지 못하면 요약 첫 줄에 "V5 플레이 불가"를 쓰고 막힌 원인과 첫 실패 원문을 적는다.
