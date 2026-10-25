이번 실행은 사용자 승인 Batch <이름>만 수행한다. AGENTS.md를 따른다. Ready Queue는 승인·기준·범위가 갖춰진 행만 진행한다.

## Batch <이름>
- 예산: N시간, 종료 정리 N분 확보. Task 안전 상한 N개(기본20). 둘 중 하나에 도달하면 새 Task를 시작하지 않는다.
- 시작: Tools/Preflight.ps1 -Batch <번호>. 모든 Task needs-unity=no면 -SkipUnity. Unity는 승인된 샌드박스 밖 경로로 실행.
- 재시도: 최초 실패 후 같은 접근1회(총2회), 접근 최대3개. 다른 값이면 명시.
- 트랙(서로 독립):
  - A: <Task들> — 합격 기준: <수치>
  - B: <Task들> — 합격 기준: <수치>
- 사전 결정:

| 결정 | 답 | 날짜 | 출처·사용자 승인 근거 | 막히면 대체(사용자가 정한 대안만) |
| --- | --- | --- | --- | --- |
| | | | | |

날짜·출처가 있는 실제 사용자 결정은 시작 기록에서 TASKS 2-1에 옮긴다. Claude 제안·측정 결과는 사용자 승인과 구분한다.

## Task

| Task | 유형 | needs-unity | 선행 | 내용 | 합격 기준 |
| --- | --- | --- | --- | --- | --- |
| <ID> | <feature/verification/doc/tool> | <yes/no> | <ID 또는 없음> | <파일·동작·산출물> | <수치 또는 확인 가능한 산출물 조건·확인 방법·기대 결과> |

- Base 커밋: <배치 시작 커밋 해시 또는 이전 배치 태그(예: B10)>
- 허용 범위: 수정 가능 <파일·폴더> / 수정 금지 <파일·폴더>
- Backlog(위에서부터, 각 합격 기준 포함):
  1. <ID·유형·needs-unity·선행·내용> — 합격 기준: <...>, 허용 범위: <...>
- 종료 조건: 위 전부 완료·BLOCKED, 또는 예산 소진

## Queue·검증·인계

- Batch Task·Backlog가 완료 또는 중단되면 남은 시간·상한 안에서 Ready Queue 위부터. 실행 행의 허용 범위를 종료 카드에 합친다. 승인되지 않은 다음 Batch 후보는 실행하지 않는다. Queue가 비면 종료한다.
- Task마다 실제 검증·TASKS 갱신·이번 파일만 로컬 커밋. 수락만 남으면 REVIEW, 유용한 안정 결과의 기준 일부 잔여는 PARTIAL. 의존하지 않는 다음 Task는 계속한다.
- 게임 변경은 위험도별 기능·연결 회귀와 종료 기본 회귀. 문서/도구만이면 자체 검증으로 대체하고 Unity 미실시 명시.
- V5 영향이 있으면 AGENTS Housekeeping의 정적/Scene 명령에 실행파일·대상·기준 결과·새 출력 경로를 채운다. 기존/새 FAIL과 self-test를 분리한다. 정적 exit0만으로 통과하지 않는다.
- 필요한 Task 요약 최대1개씩, Batch 요약1개. 원본은 Validation/_raw/<번호>/에 보존. FAIL·핵심 MEASURE 원문·미실시·Ruling(결정/이유/틀렸을 때 비용)을 적는다.
- Task 커밋 후 Batch-Card -Base <Base> -Allowed <Batch와 실행 Queue 범위>, 별도 git status/diff로 미커밋·미추적도 확인. 기존 사용자 변경은 분리 보존.
- TASKS 0·Current Tasks·Latest Handoff 갱신. 과거 이력 보관 시 미해결 항목·직접 플레이 순서는 유지.
- origin/main..HEAD에 다른 Batch 미푸시 커밋이 있으면 합치지 않는다. push하지 않는다. 게임 태그는 게임 Batch일 때만 정한다.
- 제출 전 모호한 합격 기준·실행 불가능한 명령·누락된 선행/범위·미승인 답을 확인한다. 문서 Task에 불필요한 수치를 강요하지 않는다.
