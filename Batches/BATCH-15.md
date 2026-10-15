이번 실행은 Batch 15(월드 지도 기능)만 수행한다. 무인 장시간 실행이다(사용자 취침 중). AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다.
push는 하지 않는다.

## 목적
- 직접 플레이 테스트 중 길을 잃지 않도록 개발 빌드에 월드 지도를 넣는다. 층 A V5 파일럿(다음 배치)에서도 데이터 파일만 바꿔 그대로 쓸 수 있어야 한다.
- 이번 배치는 지도 기능만 한다. V5 방 데이터·Scene 생성은 하지 않는다(좌표 데이터가 아직 없음).

## 시작 조건
- HEAD가 B14 태그를 포함해야 한다(`git merge-base --is-ancestor B14 HEAD`). 로컬에는 B14 위에 문서 커밋 2개("docs: add rationale to decision records", "docs: align PRD and GAME_DESIGN with V4 / B3 state")가 있을 수 있다. 정상이다.
- 미추적 파일이 Batches/BATCH-15.md 하나뿐이면 먼저 그것만 커밋한다("docs: add Batch 15 instructions"). 그 외 변경이 있으면 시작하지 않고 멈춘다.
- 시작 시각과 시작 HEAD(=Base)를 기록한다.

## 예산·큐 규칙
- 예산: Task(Backlog 포함, Backlog 5의 a·b·c는 각각 1개로 셈) 최대 14개 또는 **6시간(실행 상한)** 중 먼저 도달하는 쪽. **본 Task(T1~T7)가 끝났는데 시작 후 5시간 30분이 지나지 않았다면 Backlog를 위에서부터 계속한다(시간이 남는 한 진행).** 시작 후 5시간 30분이 지나면 새 Task를 시작하지 않고 진행 중인 Task만 끝낸 뒤 종료 처리(회귀·카드·요약·합치기)를 한다. Task마다 로컬 커밋하므로 상한에 걸려 중단돼도 마지막 커밋까지는 남는다. Backlog까지 모두 끝나면 시간이 남아도 종료한다(시간을 채우려고 범위를 늘리지 않는다).
- Task가 BLOCKED되면 그 Task 변경만 되돌리고 다음 Task로 간다. 선행이 BLOCKED면 SKIPPED.
- 재시도는 Task당 같은 문제 1회. 검사 도구 보정은 Task당 1회.
- 회귀는 Tools/Run-Regression.ps1 -Quiet. 배치 마지막에만 기본 방식도 1회.
- Batch Backlog가 끝나면 AGENTS대로 TASKS 8절 Approved Backlog를 확인한다. 현재 "추가 작업 없음"이므로 비어 있으면 종료한다. 7절 "다음 배치 후보"는 승인된 작업이 아니므로 진행하지 않는다.

## 사전 결정
| 결정 | 답 | 막히면 대체(비우면 멈춤) |
| --- | --- | --- |
| 지도 위치 원본 | `Design/world_*.json`의 rooms(rect=[x0,y0,x1,y1], 월드 좌표)와 doors. 기본 파일은 `Design/world_v7.json`. Design/ 파일은 읽기만 한다 | |
| 런타임 데이터 | Editor 메뉴 `Afterglow > Build Map Layout From Design`이 world json을 `Assets/Maps/<파일명>.json`(TextAsset)으로 복사·정리한다(좌표를 계산하거나 바꾸지 않음). 런타임은 이 TextAsset만 읽는다. 지도 파일 경로는 Inspector가 아닌 한 곳(상수 또는 인자 `-map-layout=<이름>`)에서 바꾼다. 기본 v7 | 인자 방식이 어려우면 상수 한 곳 |
| 플레이어 위치 | 방 로컬 좌표(원점=방 왼쪽 아래)에 rect의 (x0,y0)을 더해 월드 좌표로 변환 | |
| 보이는 범위 | **방문한 방만** 그린다(`ProgressSave.HasVisited`). 방문한 방의 문은 모두 그린다. 문 너머 미방문 방은 그리지 않고 문 끝에 "?"만 표시한다. 이유: 숨은 방(예: A06)이 지도에 먼저 드러나지 않게 하기 위함 | |
| 지도 상태 | M 키로 끔 → 작게 → 크게 → 끔 순환. **기본 작게**. 작게: 화면 우상단 약 240×150px, 현재 방 중심. 크게: 방문한 방 전체가 들어오는 자동 스케일, 화면의 약 70%, 반투명 | |
| 게임 진행 | **지도를 열어도 게임은 멈추지 않는다.** Pause·timeScale·입력 맵을 건드리지 않는다(과거 Pause 버그 이력 때문) | |
| 문 표시 | 일반 문=흰 짧은 선. 능력 문=능력 약자와 색: Dash=D, WallJump=W, DJ=J, Slam=S, Boss=B. 한 방향 낙하(type down)=아래 화살표. 지름길(type shortcut)=점선. 엘리베이터는 선으로 그리지 않는다 | 색·모양은 에이전트가 정하고 인계에 적는다 |
| 현재 방 표시 | 현재 방은 밝은 테두리, 플레이어는 점 하나. 방 ID 글자를 방 안에 표시 | |
| 그리기 방식 | 기존 프로젝트 방식(OnGUI)과 외부 패키지·에셋 추가 금지 | |
| 적용 범위 | `V4SceneMap.Enabled`(-v4)이고 `DEVELOPMENT_BUILD \|\| UNITY_EDITOR`일 때만. Release 빌드에는 들어가지 않는다 | |
| 인자 | `-map-all`: 모든 방과 문을 표시(검증·개발용). `-no-map`: 지도 완전히 끔(길 찾기 시간 측정용) | |
| 기존 V4MiniMap | 지도 텍스트 목록(M, GridText)이 새 지도와 M 키를 두고 충돌하지 않게 한다. 기존 `GridText`·`Visible`을 부르는 검사가 있으면 그 공개 멤버는 유지하고 M 처리만 새 지도로 넘긴다 | |
| 건드리지 않는 것 | 게임 규칙·PlayerTuning·씬·V3 안내(목표 줄·등불·F1)·보스·저장 형식 | |

## 허용 범위
수정 가능: Assets/Scripts(새 지도 스크립트·V4MiniMap의 M 처리·V4SceneMap·PlaytestLogger(Backlog만)·Verification 폴더), Assets/Editor, Assets/Maps(새 폴더), Assets/Scenes/_tmp-gen-check/(Backlog 1의 임시 생성 확인용, 끝나면 삭제), Launchers/current/Start-V4.ps1, Tools/Check-RoomRules.py(새 파일, Backlog 3), README.md(지도 사용법 2줄 이내), TASKS.md, Validation/(B15-summary.md, B15-room-rules-v4-baseline.md + _raw), Builds(Housekeeping), Batches/BATCH-15.md
(Backlog 5는 검사 코드(Assets/Scripts/Verification)만 바꾼다. 보스 코드·기본 BossTuning·BossCandidates는 변경 금지.)
수정 금지: Assets/Scenes(위 임시 폴더 제외), Assets/Prefabs(기존), Assets/ScriptableObjects, Assets/BossCandidates, Assets/Candidates, Design/, GAME_DESIGN.md, PRD.md, AGENTS.md, Packages, ProjectSettings(스크립트 정의 심볼 제외), Archive/

## Task
유형은 feature / verification / cleanup / doc 중 하나다(기능 비중 집계용 표시). 요약 파일에 유형별 개수를 적는다.

| Task | 유형 | 내용 | 합격 기준 |
|---|---|---|---|
| T1 | feature | 지도 레이아웃 생성기(Editor 메뉴). world json → `Assets/Maps/world_v7.json` | 방 21개·문 수가 원본과 같음(엘리베이터 처리는 요약에 기록), 재생성 시 SHA256 차이 0, 다른 world json(world_v6.json)으로도 같은 방식으로 생성됨을 1회 확인한 뒤 그 산출물은 삭제 |
| T2 | feature | 런타임 지도: 방 사각형·방 ID·현재 방 강조·플레이어 점, M 키 끔/작게/크게 순환(기본 작게), 방문한 방만 표시 | 새 게임 직후 방 1개만 표시. 방 전환마다 방문 방 수가 1씩 증가하고 현재 방·점이 이동. M 순환 3상태 확인. 게임 시간·입력 맵 변화 0. -v4 아닐 때 지도 코드 실행 0 |
| T3 | feature | 문 표시(능력 약자·색, 한 방향·지름길, 미방문 "?"), `-map-all`, `-no-map` | `-map-all`에서 방 21개와 문 전부(엘리베이터 제외) 표시. 능력 문 약자가 json의 gate와 일치(표로 기록). `-no-map`에서 지도 오브젝트 0. 미방문 방은 기본 모드에서 그려지지 않음 |
| T4 | verification | 지도 검증 `-verify-b15-map`(Verification 폴더, 개발 빌드 전용)과 실제 exe 10초 실행에서 M 3회 순환 | 검증 PASS, Error/Exception 0. 스크린샷은 1회만 시도하고 실패하면 KNOWN-LIMIT로 기록(재시도 없음). 회귀 -Quiet 13/13 |
| T5 | feature | 전달용 Development 빌드 갱신 + `Start-V4.ps1`에 `-NoMap` 스위치(→ `-no-map`). 빌드는 B13/B14에서 쓴 방식을 그대로 따른다. 빌드 폴더가 바뀌면 Start-V4.ps1의 경로와 TASKS의 런처 링크를 같은 커밋에서 갱신 | Play-V4·Play-V4-AfterDash·Boss-B3 런처 시작 로그 정상·10초 Error0. `-NoMap` 시 시작 로그에 map off 1줄. Housekeeping(최신 1 + 직전 1) |
| T6 | doc | TASKS.md 갱신(아래 TASKS 갱신 절)과 README 2줄 | 깨진 링크 0, TASKS 결정 대기 8개 이하 |
| T7 | verification | 마지막 점검: 기본 회귀 13/13 1회, Batch-Card, 모든 런처 시작 로그 | 허용 범위 밖 0, 깨진 링크 0, 없는 빌드·런처 0 |

## Backlog (본 Task 뒤, 시간이 남는 한 위에서부터)
1. (feature) V5 대비 일반화 — Scene 생성기와 방 목록의 데이터화. 지금 `Afterglow > Build V4 From Design`은 데이터 파일(rooms_v2.json)과 출력 폴더(Assets/Scenes/V4/)가 고정이고 `V4SceneMap.Rooms`는 방 ID 21개가 코드에 박혀 있다. 이것을 (a) 데이터 파일 경로와 출력 폴더를 인자로 받는 생성기, (b) 방 목록을 지도 레이아웃(T1의 Assets/Maps/*.json)에서 읽는 방식으로 바꾼다. 기본값은 지금과 같아서 V4 동작 변화 0이어야 한다. 다음 배치에서 V5 데이터 파일만 넣으면 같은 도구로 Scene이 나오게 하는 것이 목적이다. — 합격 기준: V4 21개 Scene을 기본 설정으로 재생성했을 때 SHA256 차이 0, 같은 데이터를 임시 출력 폴더 `Assets/Scenes/_tmp-gen-check/`에 생성한 결과가 V4와 요소 수 일치 표로 같음(확인 후 임시 폴더 삭제), `-v4` 방 전환·저장·Continue 회귀 13/13. 지도 레이아웃의 방 목록과 `V4SceneMap.Rooms`가 어긋나면 BLOCKED로 두고 기록
2. (feature) PlaytestLogger 기록 확장(한 번에): (a) 방 이벤트 — 방 진입·퇴장 시각, 사용한 문 ID(어느 갈래로 나갔는지), 방별 체류 시간, 사망한 방. (b) 지도 사용 — 지도 열기 횟수, 큰 지도를 연 총 시간. 이미 기록되는 항목은 중복하지 않고, **기존 열의 순서와 이름은 바꾸지 않는다.** V5 파일럿에서 갈래 선택·헤맴·숨은 곳 발견·지도 의존도를 숫자로 보기 위한 기록이다. — 합격 기준: 방 3개를 오가는 자동 입력 검사에서 진입·퇴장 쌍이 문 이동 횟수와 일치, 지도 열기 횟수가 M 조작과 일치, 기존 열 diff 0, 회귀 13/13
3. (verification) 방 규칙 점검 스크립트 `Tools/Check-RoomRules.py`: `Design/rooms_*.json`을 읽어 방별 면적, 요소 수(적·가시·내려찍기 발판·증기·승강기·무너지는 발판·부서지는 벽·봉인·덮개·벽 점프 벽·포고), 1,000u²당 요소 수, 높이 20 초과 방의 바닥 면 개수, 방마다 문 개수를 표로 낸다. 판정은 하지 않고 기준선만 만든다(V5 설계 기준: 비허브 방 1,000u²당 3개 이상, 높이 20 초과는 바닥 면 2개 이상). 허브·엔딩 방은 표에서 구분 표시. Python이 없으면 이 항목은 SKIPPED. — 합격 기준: rooms_v2.json 21방 표가 `Validation/B15-room-rules-v4-baseline.md`로 저장되고, 합계 면적과 방 수가 데이터와 일치. Design/ 파일은 읽기만 함
4. (feature) 지도 보강: (a) 플레이어가 능력을 가졌으면 그 능력의 문 표시를 "열림" 색으로 바꾼다(`ProgressSave`의 기존 능력 플래그만 사용, **새 저장 필드 금지**). 새 능력을 얻은 뒤 어디로 돌아갈지 지도에서 보이게 하는 것이 목적이다. (b) 마지막 체크포인트가 있는 방에 CP 표시. CP ID와 방 ID의 대응이 데이터와 맞지 않으면 (b)만 생략하고 기록한다. — 합격 기준: 능력 0개에서는 기존 색 유지, 대시 보유 시 D 문만 열림 색(검증 코드로 상태 확인), CP 표시가 저장 후 재개에서도 유지, -v4가 아닐 때 영향 0, 회귀 13/13
5. (verification) 기본 보스 B3 보강 검증 — BATCH-13 Track B 중 미실행분을 기본 보스 B3 기준으로 한다. 각각 독립 Task이며 하나가 BLOCKED여도 나머지는 진행한다. 검사 코드(Verification 폴더)만 바꾸고 지형·보스 코드·BossTuning은 바꾸지 않는다. 검사 도구 보정은 항목당 1회. (a) 추적 연타 회피 검증 보완(열린 버그 B12-CHASE-VERIFY 관련): 검사 시작 위치를 아레나 중앙 쪽으로 옮기거나 "점프+더블 점프로 넘기" 회피로 확인 — 합격: 60fps 의도한 회피 피해 0, 회복 반격, Pause, 사망 복귀, 10회 Error0, 30/120fps 회피 피해 0. (b) 2페이즈 30/120fps: 전환 1회·추적 4회·도약 3회·회복 시간 기록 — 합격: 30/120 각각 전환 1회·Error0, 회복 측정값 기록. (c) 8패턴 연속 3바퀴 soak(60fps, 플레이어 무적 fixture 허용) — 합격: Error0, 바퀴 끝마다 남은 탄·낙하물·파동 오브젝트 수 0(수를 기록). B3-rec10은 D-B3에서 취소되어 제외. 결과로 B12-CHASE-VERIFY를 닫을 수 있으면 TASKS 4절 해당 행의 상태만 갱신한다.

Backlog는 1 → 5 순서로 진행한다. 1번이 BLOCKED여도 2번 이후는 계속한다. 5번까지 모두 끝나면 종료한다.

## TASKS 갱신 (T6)
- 결정 기록(2-1)의 **D-B3 행 근거 칸**에 "Batch14 T1 적용"이 문장 중간에 끼어 있다. 의미를 바꾸지 않고 문장을 나눈다. 예: "사용자 직접 플레이 후 결정. 진행 속도가 나쁘지 않았고, 마지막 보스라 난이도가 더 높아도 된다고 판단했다. (적용: Batch14 T1)". 다른 칸은 건드리지 않는다.
- 결정 기록(2-1)에 새 행을 추가한다.
  - ID: `D-V5-PILOT`
  - 결정: "방 구조 V5 재설계의 첫 파일럿으로 층 A(A01~A06)만 새 방 문법(방 안 열린 두 갈래·겹침·보이지만 못 가는 곳, 방 크기 축소, 능력 없이 도는 고리 2개)으로 만든다. A06은 더블 점프 전용(A02 벽 점프 문과 B06 구멍 진입은 닫음). 플레이한 뒤 수정한다. 설계 문서와 방 데이터는 Claude 쪽에서 작성한다."
  - 날짜: 2026-10-15
  - 근거: "사용자 결정. V4는 선형이고 방이 크며 비어 있다고 판단함(부서지는 벽 21방 중 1곳, 허브·엔딩 방을 뺀 저밀도 방 존재). 21방 전체 제작 전에 방 문법을 6방으로 먼저 확인한다."
- Latest Handoff에 "직접 플레이 확인"을 쓴다: Play-V4로 새 게임 → M 키 3상태 → 방 전환 시 지도 변화 → 큰 지도가 화면을 얼마나 가리는지 → `-NoMap`(Play-V4.cmd -NoMap) → 약 10분.
- 결정 대기 8개 이하 유지. 지도 수락(REVIEW → DONE)은 사용자가 한다.

## 배치 공통 합격 기준
- Task마다 -Quiet 13/13. 마지막에 기본 13/13 1회.
- Tools/Batch-Card.ps1 -Base <시작 HEAD> -Allowed <위 허용 범위>: 허용 범위 밖 0, TASKS 깨진 링크 0, 없는 빌드·런처 0.
- Release 빌드에 지도 코드가 들어가지 않음(검증 코드 분리 B14 방식 유지).

## 종료
- Task별 로컬 커밋. 종료 시 `origin/main..HEAD`가 시작 HEAD 이후의 이번 배치 커밋뿐이면 AGENTS대로 `git reset --soft <시작 HEAD>`로 합쳐 "Batch 15: world map" 한 커밋으로 만들고 태그 B15를 붙인다.
- 시작 HEAD 앞에 push되지 않은 로컬 커밋(문서 커밋 2개)이 있으면 그것들은 합치지 않는다. 시작 HEAD 이후 Task 커밋만 합치고 태그 B15는 그 커밋에 붙인다.
- Validation/B15-summary.md 맨 위에 회귀·카드 결과와 유형별 Task 개수를 붙인다.
