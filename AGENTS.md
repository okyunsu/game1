# AI 작업 규칙

## Source of Truth

사용자의 명시적 지시를 우선한다. 문서 충돌은 아래 순서로 판단하고 TASKS의 Decisions Needed에 남긴다.

1. PRD.md — 제품 범위와 완료 조건
2. GAME_DESIGN.md — 게임 규칙과 설계
   - V5 방 구조 상세 원본은 Design/v5/WORLD_V5.md와 승인 데이터다. 구현 에이전트는 기본 읽기 전용이며 상위 규칙과 충돌하면 Decisions Needed에 남긴다.
3. TASKS.md — 현재 작업·상태·검증 기록
4. README.md — 실행 안내

이 파일은 에이전트 실행 절차만 다룬다.

실행 조건(예산·재시도·허용 범위·합격 기준)은 사용자가 승인한 이번 Batches/BATCH-<번호>.md가 이 파일의 기본값보다 우선한다. 없는 항목은 이 파일을 따른다. 이 우선순위는 사용자 지시·PRD 범위·게임 규칙·실행환경 권한을 우회하는 허가가 아니다. 보호 파일·규칙 예외는 사전 결정에 명시되어야 한다.

## Session Start

1. TASKS.md 0절 “지금”부터 읽는다.
2. git status --short와 git log --oneline origin/main..HEAD로 실제 상태를 대조한다. origin/main은 로컬 참조이고 fetch하지 않으면 원격 최신이라고 쓰지 않는다. 차이는 실제 상태를 기준으로 기록한다.
3. Batch 실행이면 승인된 Batches/BATCH-<번호>.md 전체, 그 외에는 TASKS 9절 Latest Handoff를 읽는다.
4. 필요한 문서의 해당 절만 추가한다. 쓰기 작업 종료 시 0절을 갱신한다. 읽기 전용 검토에서는 변경 없이 차이만 보고한다.

인계 세부 절차는 `.agents/skills/metro-handoff/SKILL.md`를 따른다. Claude도 같은 원본을 사용한다.

## Context Rule

작업 시작 시 이번 Batch 지시와 현재 Task를 먼저 확인하고, 작업에 필요한 문서만 추가로 읽는다. 매 작업마다 문서 전체를 재독하지 않는다.

- 범위·완료 조건이 필요하면 PRD.md
- 게임 규칙·수치·플레이 동작이 필요하면 GAME_DESIGN.md의 해당 절
- 상태·의존성·Bugs·Decisions Needed·이전 결과가 필요하면 TASKS.md
- 실행 방법이나 프로젝트 구조가 필요하면 README.md

작업 중 불확실성이나 문서 충돌이 생기면 필요한 문서를 추가로 확인한다. Sprint 변경, Scope 변경, 설계 충돌, 사용자의 전체 검토 요청 때는 관련 문서 전체를 다시 확인한다.

## Progress Rule

현재 단계가 Prototype / Vertical Slice / Content Production일 때는 완벽성보다 플레이 가능한 범위의 확장을 우선한다.

- 이미 기본 동작이 가능한 기능의 추가 엣지케이스 검증보다 아직 없는 필수 기능·콘텐츠 구현을 우선한다.
- 비차단 Known Issue는 기록하고 진행한다.
- 검증 도구를 새로 만드는 작업은 현재 Task 완료에 반드시 필요할 때만 한다.
- 자동화된 검증 구축 자체를 별도 제품 목표로 취급하지 않는다.
- 검증 도구·자동화 구축 비용이 실제 기능 구현 비용보다 커지기 시작하면 더 단순한 검증 방법으로 전환한다.
- Batch 종료 시 검증량보다 새로 플레이 가능해진 범위, 완료된 기능, 줄어든 주요 리스크를 우선 보고한다.

## Work Rule

- 기존 파일·사용자 변경·의존성을 확인하고 현재 승인된 Batch 안의 작업만 수행한다.
- 관련 없는 기능·리팩터링·불필요한 패키지·미래 확장용 구조를 추가하지 않는다.
- 이미 정상 확인된 환경은 새로운 문제나 변경이 없으면 반복 조사하지 않는다.
- 의존성 충족은 Batch 밖 작업의 승인을 뜻하지 않는다.
- Git 이력의 일회성 생성 스크립트를 복원하거나 재실행하지 않는다.

## Environment

- Batch 시작 시 Tools/Preflight.ps1을 실행한다. 문서·분석·Python·운영 도구만 바꾸는 Batch는 -SkipUnity로 Git·환경 정보만 확인한다.
- Unity와 Unity 호출 스크립트(회귀·빌드·executeMethod), Unity 검증 exe는 샌드박스 밖 실행 경로를 사용한다. 편집·분석은 샌드박스 안에서 한다. 환경의 별도 권한 검사가 요구되면 해당 절차를 따르며 우회하지 않는다.
- Preflight는 LOCALAPPDATA·Hub 프로세스·작업 트리·로컬 origin/main..HEAD와 Unity 기동 종료 코드·timeout을 기록한다. Hub 실행 여부만으로 성공/실패를 판정하지 않는다. 동일 프로젝트 Unity가 이미 열려 있으면 두 번째 Editor를 실행하지 않고 Unity 경로만 중단한다.
- exit198이면 재조사·반복 실행하지 않고 needs-unity=yes Task와 그 의존 경로만 BLOCKED(외부 의존성)로 기록한다. no Task는 계속한다. 사용자가 환경을 고쳤거나 다른 원인 증거가 생기면 재확인할 수 있다.
- 다른 종료 코드·실행 파일 부재·timeout은 라이선스 문제로 단정하지 말고 실제 원인·영향을 기록한다. 환경 실패가 전체 제품 구현 실패는 아니다.
- 결과와 원본 로그는 Validation/_raw/<Batch>/의 새 실행 폴더에 보존한다. 실패 원문·미실시는 Task/Batch 요약에 옮긴다. 기존 결과는 덮어쓰지 않는다.

## Batch Approval

사용자는 개별 Task가 아니라 Batch(여러 Task·트랙의 묶음)를 승인한다. Batch에는 예산, 트랙, 사전 결정 표, 허용 범위, Backlog, 종료 조건이 포함된다.

- Batch 안의 모든 Task를 끝낼 때까지 진행한다. 중간에 사용자에게 묻고 멈추지 않는다.
- 한 Task가 BLOCKED이면 그 Task와 그 결과에 의존하는 Task만 중단하고, 의존하지 않는 다음 Task로 넘어간다.
- 사전 결정 표에 있는 항목은 표대로 진행한다. "막히면 대체" 칸은 사용자가 적은 대체안만 유효하다. 칸이 비어 있거나 표에 없는 설계 결정이 필요하면 해당 경로만 멈추고 Decisions Needed에 기록한다.
- Batch의 Task가 모두 끝나거나 BLOCKED이면, 예산이 남아 있는 한 Batch에 적힌 Backlog를 위에서부터 진행한다.
- Backlog 항목도 적힌 합격 기준을 따른다.
- Batch의 Backlog까지 소진되면 TASKS의 Ready Queue를 위에서부터 진행한다. Queue 각 행은 사용자 승인·합격 기준·허용 범위·needs-unity·선행을 갖추며 플레이 수락 대기 작업은 넣지 않는다. 실행 행의 범위는 종료 검사 허용 목록에 합친다. 필수 필드가 빠지면 그 행만 중단하고 다음 독립 행으로 간다. 그것도 소진되면 종료한다. Batch 밖 작업은 하지 않는다.

예산: 시간을 기본 기준으로 하고 Task 수는 안전 상한(명시 없으면 20)으로 쓴다. 둘 중 하나에 도달하면 새 Task를 시작하지 않고 현재 Task를 안전하게 정리한다. 종료 처리 시간을 먼저 확보한다. 시간이 남고 상한 미도달이면 Batch Backlog → Ready Queue 순으로 진행한다. Task를 쪼개 상한을 우회하지 않는다. Queue가 비면 종료하며 새 제품 작업을 임의로 승인하지 않는다.

재시도 한도:

- 같은 원인 가설과 같은 해결 접근은 최초 실패 후 1회 재시도한다(총 2회 시도). Batch가 다르게 정하면 따른다.
- 원인을 재진단해 실질적으로 다른 접근을 택하면 새 시도로 본다.
- Task당 서로 다른 접근은 최대 3개까지 허용한다.
- 검증 도구·자동화 자체의 실패는 제품 구현 실패와 별도로 취급한다.
- 검증 도구를 고치는 비용이 현재 Task 구현 비용보다 커질 경우, 자동 검증을 중단하고 더 단순한 검증 방법으로 대체하거나 미검증 항목으로 남긴다.
- 재시도 한도를 넘기거나 진행에 설계 변경·Batch 범위 확대·사용자 결정·외부 의존성이 필요하면 해당 경로만 BLOCKED로 기록한다.

사용자에게만 있는 권한:

- 보호 파일 수정
- PRD 범위 변경
- GAME_DESIGN 규칙 변경
- Sprint Gate 통과
- 감각·설계 수락(REVIEW → DONE)

단, Batch의 사전 결정 표에 명시적으로 허용된 범위는 예외다.

## Definition of Done (Task)

아래를 충족하면 DONE이다. 사용자 감각·설계 수락만 남으면 REVIEW, 유용한 안정 결과에 일부 기준이 남으면 PARTIAL, 필수 구현·기술 검증을 외부 의존성 등으로 진행할 수 없으면 BLOCKED로 기록하고 다음 독립 Task로 간다. DONE만 다음 Task 진행 조건으로 삼지 않는다.

1. 이번 변경을 대상으로 Batch 합격 기준을 직접 확인한다. 수치 Task는 이번 실행 로그, 문서·조사는 요구 산출물·근거·참조 확인으로 증명한다. 기준이 전혀 없으면 해당 경로만 Decisions Needed에 올린다. 수치가 없다는 이유로 문서·조사·승인된 후보 실험을 막지 않는다.
2. 게임 코드/에셋 변경은 컴파일 오류0과 핵심 동작을 확인하고 연결 회귀는 Validation 위험도에 비례해 수행한다. Batch가 요구하거나 핵심/통합 변경이면 전체 회귀를 실행한다. 문서·운영 스크립트는 자체를 확인하며 Unity 검증을 요구하지 않는다.
3. 이번 Task의 허용 범위 밖 변경0. 커밋 전 git diff·git status로 미추적/미커밋까지 확인하고 기존 사용자 변경은 구분해 보존한다. Batch-Card는 커밋 간 검사이므로 미커밋 검사를 대신하지 않는다.
4. 증거에 실제 FAIL·핵심 MEASURE 원문, 미실시와 한계를 기록한다. 이전 실행 결과는 비교 기준이며 이번 통과 증거를 대신하지 않는다. 영향 없는 영역은 재실행하지 않아도 된다.
5. TASKS Current Tasks 상태·결과를 갱신하고 이번 Task 파일만 로컬 커밋한다.

측정 자체가 목적이면 나쁜 결과도 정해진 측정·기록을 완료했을 때 DONE이다. 기능 구현 실패를 측정 완료로 바꿔 DONE 처리하지 않는다.

## Git Rule

- Batch 시작 전 작업 트리를 확인한다.
- 작업 트리가 깨끗하지 않아도 기존 사용자 변경의 범위와 출처를 식별할 수 있고 현재 Batch 변경과 충돌하지 않으면 보존한 채 진행한다. 커밋할 때는 이번 작업 파일만 add한다.
- 기존 변경과 Batch 변경의 경계를 구분할 수 없거나 충돌 위험이 높을 때만 해당 경로를 BLOCKED로 둔다.
- Task마다 로컬 커밋한다.
- push는 하지 않는다. 사용자가 직접 한다.

Task가 완료 조건 일부를 충족하지 못했더라도 현재 구현이 후속 개발에 유용하고 프로젝트를 불안정하게 만들지 않으면 즉시 되돌리지 않는다.

- 후속 개발에 사용할 수 있지만 완료 조건 일부가 남으면 PARTIAL 상태로 두고, 남은 문제와 영향 범위를 Task별 결과에 적는다.
- 기술 구현과 검증은 끝났고 사용자 판단만 남았으면 REVIEW 상태로 둔다.
- 컴파일 불가, 진행 불가, 저장 손상, 주요 회귀, 후속 작업을 방해하는 불안정 상태처럼 현재 구현을 유지하는 비용이 큰 경우에만 해당 Task의 변경을 되돌린다.
- 되돌릴 경우 그 Task의 변경만 되돌리고 실패 기록(원인·시도·영향)을 남긴다.
- 반쯤 깨진 변경 위에서 다음 Task를 시작하지 않는다.

## Designer Control

플레이 감각·밸런스 조정에 쓰는 값(이동·점프·대시·전투·적 수치 등)은 Inspector 또는 기존 설정 Asset 중 한 곳에서 사용자가 직접 조정할 수 있게 하고, 단위·Tooltip을 붙인다. 같은 값을 두 곳에 중복 노출하지 않는다. 이 값들은 하드코딩하지 않는다.

여러 시스템·오브젝트·Scene이 공유하는 값만 설정 Asset(ScriptableObject)으로 분리한다. 기존 설정 Asset(예: PlayerTuning)이 있으면 거기에 둔다. 한 구현 내부에서만 쓰는 값은 별도 설정 구조로 승격하지 않는다.

배치·영역은 Scene을 원본으로 두고, 반복 사용하는 오브젝트는 Prefab으로 관리한다. 인계에는 사용자가 조정할 가능성이 있는 값의 위치와 Play 종료 후 값 반영 방법만 안내한다.

## Design Change

PRD의 범위나 GAME_DESIGN의 규칙·능력·진행 구조 변경은 사용자 결정이 필요하다.

사용자가 Batch의 사전 결정 표에 적은 답은 사용자 결정으로 본다.

Prototype Value는 명시된 탐색 범위 안에서 이전 값을 보존한 후보 실험이 가능하다.

가설·전후 값·실제 결과를 TASKS에 기록하며 사용자 승인 없이 후보를 기본값으로 확정하지 않는다.

후보 작업(기본 Asset·기본 Scene·기본 동작을 바꾸지 않고 후보 Asset·후보 빌드에서만 켜지는 실험)은 Batch에 정해지지 않은 수치·위치·타이밍을 에이전트가 정해 진행한다. 정한 값과 이유를 Handoff의 Task별 결과에 적는다. 미정 수치만을 이유로 후보 경로를 멈추지 않는다. 기본값 채택과 게임 규칙 변경은 계속 사용자 결정이다.

설계·범위가 아닌 구현 세부(임시 파일명, 승인된 측정의 샘플 간격 등)는 에이전트가 정하고 진행한다. Batch 요약에 `Ruling: 결정 / 이유 / 틀렸을 때 비용`을 남긴다. 조작키·체감·밸런스·저장 계약처럼 사용자 경험이나 규칙을 바꾸는 선택은 자동으로 사소한 판단에 넣지 않는다. 후보 탐색은 위 후보 작업 규칙을 따른다.

## Task Status

- TODO: 아직 시작하지 않음.
- DOING: 구현 또는 기술 검증 진행 중.
- PARTIAL: 완료 조건 일부가 남았지만 현재 결과를 후속 개발에 쓸 수 있고 진행을 막지 않음.
- REVIEW: 기술 구현과 관련 검증이 끝났고 사용자 감각·설계 판단만 남음.
- DONE: 완료 조건과 필요한 사용자 수락을 모두 충족함.
- BLOCKED: 더 진행하려면 외부 의존성·사용자 결정·범위 변경이 필요함.
- DEFERRED: 의도적으로 미룸.

## Review

상태와 의존성 표기는 TASKS를 따른다.

REVIEW는 기술 구현과 관련 Unity 검증이 완료되어 사용자 판단을 기다리는 상태다.

감각·설계 수락이 필요한 Task는 사용자가 수락한 뒤 DONE으로 바꾼다.

별도 사용자 판단이 없는 Task는 완료 조건과 필요한 검증을 충족하면 DONE으로 둔다.

감각·설계 수락 전이라도 후속 구현은 REVIEW를 기반으로 진행한다.

수치는 설정 Asset의 현재 값을 임시값으로 쓰고 하드코딩하지 않는다.

단, 그 수치에 맞춰 공간을 확정하는 작업(레벨 배치, 간격·높이 확정 등)은 수락 후에 한다.

수락 대기 항목은 Latest Handoff의 "직접 플레이 확인" 목록에 쌓는다.

회귀로 선행 검증이 실제로 무효가 된 경우에만 해당 Task를 DOING 또는 BLOCKED로 되돌린다.

영향이 없는 후속 Task까지 함께 중단하지 않는다.

## Validation

실행하지 않은 테스트를 통과했다고 기록하지 않는다.

실제로 수행한 검증과 수행하지 못한 검증을 구분해 기록한다.

구현 Task는 완료 조건을 확인하기에 충분한 최소 검증을 직접 실행한다.

검증 강도는 변경 위험에 비례하며 이미 검증된 관련 없는 영역까지 확대하지 않는다.

예시:

- Low Risk: 컴파일 + 해당 기능 핵심 동작 확인
- Medium Risk: 해당 기능 + 직접 연결된 시스템 회귀
- High Risk: 저장, 진행, Scene 전환, Player core 등 핵심 시스템 변경 시 관련 핵심 회귀
- Milestone: Vertical Slice, Sprint 종료, Core Loop 통합, Content Complete, Release Candidate에서 전체 회귀

사용자 체감 확인이나 에이전트가 직접 할 수 없는 확인이 완료 조건에 있으면, 기술적으로 확인 가능한 항목까지 검증한 뒤 REVIEW로 두고 Latest Handoff의 "직접 플레이 확인"에 남긴다.

미검증은 BLOCKED가 아니다.

컴파일 실패, 필수 도구 부재, 해결되지 않은 의존성 등으로 구현이나 기술 검증 자체를 더 진행할 수 없을 때만 BLOCKED로 둔다.

문서·결정 작업에는 게임 실행 조건을 적용하지 않는다.

작은 Task마다 전체 검증을 반복하지 않는다.

전체 회귀는 아래 시점에 집중한다.

- Vertical Slice
- Sprint 종료
- Core Loop 통합
- Content Complete
- Release Candidate

기존 Validation 파일은 덮어쓰지 않고 새 파일로 저장한다.

## Evidence

- Task별 Validation 요약은 필요할 때 최대 1개, Batch 종료 요약은 1개 커밋한다. 같은 내용을 복제하지 않으며 원본 자동 산출물은 _raw/에 둔다.
- 원본 로그에 FAIL·MEASURE 줄이 있으면 실패 줄과 핵심 수치를 원문 그대로 넣고, 미실시 항목을 적는다.
- 원본 로그·CSV·json·PNG는 `Validation/_raw/`에 남기고 커밋하지 않는다(.gitignore 대상).
- 원본 자료는 삭제하지 않는다.
- TASKS.md에는 Current Sprint, Current Tasks, Bugs, Decisions Needed, Ready Queue, Latest Handoff를 둔다.
- 끝난 Task 이력과 해결된 Bugs·Decisions는 Archive/로 옮긴다.

## Handoff

Batch 종료 시 TASKS의 Latest Handoff를 아래 순서로 갱신한다.

- 시작 기록에서 날짜·출처가 있는 실제 사용자 사전 결정을 TASKS 2-1에 옮긴다. Claude 설계안·에이전트 제안은 사용자 결정과 구분하며 “출처=Claude”는 승인 증거가 아니다.
- 시작점에 포함된 관련 Design/ 변경이 있으면 Task별 결과 첫 줄에 “Claude 설계 변경”과 커밋/파일을 기록한다. 작성 주체가 미확인이면 단정하지 않는다.
- 과거 표 보관 시 미해결 REVIEW·PARTIAL·BLOCKED와 기존 직접 플레이 순서는 현재 TASKS에 유지한다. 종료 시 0절도 갱신한다.

5분 안에 읽을 수 있게 유지하고 상세 증거는 링크한다.

1. 결정 질문지
   - 질문마다 선택지
   - AI 추천과 이유
   - 그 답이 풀어 주는 Task

2. 직접 플레이 확인
   - 확인할 Scene
   - 볼 항목
   - 예상 시간
   - 수락 대기 항목이 쌓인 순서대로

3. Task별 결과
   - 상태
   - 커밋
   - 주요 변경 파일
   - 실제 검증 결과
   - 문제

4. 다음 Batch 후보

필요한 사용자 결정이 없으면 1을 생략한다.

수락 대기 항목이 없으면 2를 생략한다.

## Stop Conditions

아래 상황에서는 해당 경로만 중단하고 TASKS의 Bugs 또는 Decisions Needed에 문제·영향·필요 판단을 적는다.

- 게임 규칙 변경이 필요함
- 승인 밖 기능 추가가 필요함
- 사용자만 수행할 수 있는 조작·판단이 필수임
- 재시도 한도를 초과함
- Batch 예산을 명백히 초과함
- 신규 시스템이나 설계 변경이 필요한 수준으로 범위가 증가함
- 외부 의존성 때문에 더 이상 진행할 수 없음

추가 보호 조건:

- 허용 범위 밖 또는 `Tools/protected-files.txt` 해당 변경이 필요하면 **수정 전에** 해당 경로를 중단하고 파일·위치·이유를 기록한다. Batch 사전 결정의 명시 예외만 허용한다. 안전한 기존 결과는 PARTIAL 등으로 보존한다.
- .unity·.prefab·.asset·.meta는 텍스트로 직접 편집하지 않는다. Unity Editor와 executeMethod 생성기/API로만 변경한다. 새 에셋은 Unity가 만든 .meta와 함께 커밋하며 GUID 수동 변경·.meta 단독 삭제를 하지 않는다.
- 이미 위반했다면 그 Task의 위반 변경만 제거한다. 사용자 변경·다른 Task 결과는 보존한다. 전체 되돌리기는 Git Rule의 불안정 조건에 해당할 때만 한다.
- git push·reset --hard·clean·stash drop/pop/apply, 이미 push된 커밋 합치기는 실행하지 않는다. 저장 형식·ProjectSettings 변경은 사용자 명시 승인 없이는 중단한다. 로컬 reset --soft는 Housekeeping의 한정된 조건을 따른다.

다른 독립 Task와 Backlog는 계속한다.

Sprint Gate를 통과하기 전 다음 Sprint의 대규모 콘텐츠 확장은 하지 않는다.

단, 현재 Sprint의 완료를 위해 승인된 통합·아트·연출·콘텐츠 작업은 계속 진행할 수 있다.

## Housekeeping

- 빌드는 Builds/에 최신 1개와 직전 1개만 남기고, 새 빌드를 만든 뒤 그보다 오래된 빌드는 삭제한다.
- 실행용 바로가기는 Launchers/current만 쓰고 배치별 폴더를 만들지 않는다.
- 배치 지시문은 Batches/BATCH-<번호>.md에 둔다. 작업 시 이번 배치 파일만 읽는다.
- 자동 실행 로그·csv·스크린샷은 Validation/_raw/에 둔다(git 제외). Validation/에는 필요한 Task 요약 최대1개씩과 Batch 종료 요약1개만 커밋한다(Evidence와 동일).
- 게임 코드·에셋을 바꾼 Batch는 종료 시 Tools/Run-Regression.ps1을 실행한다. 문서·운영 도구만 바꾸면 자체 최소 검증으로 대체하고 Unity 회귀 미실시를 명시한다. 모든 Batch는 Task 커밋 후 Tools/Batch-Card.ps1 -Base <시작 커밋> -Allowed <Batch 및 실행 Queue 허용 범위>와 별도 git status/diff 확인을 수행해 요약 맨 위에 붙인다. 카드의 과거 REGRESSION 행을 이번 실행 증거로 읽지 않는다.
- Design/v5·V5 Scene 또는 그 동작에 영향을 주는 생성기/런타임/검사기를 바꾼 Batch는 아래 기존 검사를 추가한다. Python·Unity 실행파일, 대상 Scene/빌드·기준 결과·새 출력 경로를 Batch에 명시한다.
  - 정적: `<Python> Tools/Check-RoomRules.py Design/v5/rooms_v5_A.json --v5 --self-test --world Design/v5/world_v5_A.json --output Validation/_raw/<Batch>/<새이름>.md`. 도랑 탈출 측정이 필요하면 대상에 유효한 측정 JSON을 `--max-step-evidence <경로>`로 전달한다. 없으면 그 MEASURE는 미실시로 표시하며 값을 만들지 않는다.
  - Scene: Unity의 `-batchmode -quit -projectPath <저장소> -executeMethod V5Batch18.Clearance -verify-b18-clearance -verify-output <새출력> -logFile <새로그>`로 Assets/Scenes/V5/*.unity를 검사한다. 단순 exe 인자만으로 V5 검사라고 가정하지 않는다. exe 방식이면 소스와 일치하는 Development 빌드와 V5 Scene 매핑을 명시한다.
  - 종료 코드뿐 아니라 원문 FAIL·WARN·MEASURE 및 self-test 결과를 확인한다. 정적 검사기는 데이터 FAIL이 있어도 exit0일 수 있다. 같은 규칙 버전·대상 기준과 비교해 기존/해소/새 FAIL을 구분한다. 규칙·샘플링이 달라졌으면 개수만으로 회귀 판정하지 않는다. 정적 도달 후보는 실제 완주 증명이 아니다.
- 결정 대기는 최대 8개. 초과하면 새 기능 Task를 시작하지 말고 사용자에게 정리를 요청한다.
- 결정 대기 항목은 존재하는 런처·빌드를 가리킨다. 빌드를 삭제하거나 대체할 때는 그 빌드를 가리키는 TASKS 항목을 같은 커밋에서 갱신한다.
- 배치 진행 중에는 Task마다 로컬 커밋한다. 배치 종료 후 push 직전에 git reset --soft <배치 시작 커밋>으로 하나로 합쳐 커밋한다. 메시지 첫 줄은 "Batch <번호>: <요약>", 본문에 Task별 상태 표를 넣는다. 그 커밋에 태그 B<번호>를 붙인다. 합치기는 origin/main..HEAD가 이번 배치 커밋뿐일 때만 하고, 이미 push된 커밋은 절대 합치지 않는다.
