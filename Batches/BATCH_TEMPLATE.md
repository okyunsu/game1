이번 실행은 사용자 승인 Batch <이름>만 수행한다. AGENTS.md를 따른다. Ready Queue는 승인·기준·범위가 갖춰진 행만 진행한다.

## Batch <이름>
- 목표 상태: <이번 Batch 종료 때 확인할 제품·문서 상태와 판정 방법>.
- 시간 예산: N시간, 종료 정리 N분 확보. Task 안전 상한 N개(기본30)는 안전장치다. 시간 또는 안전 상한에 도달하면 새 Task를 시작하지 않는다.
- 사용자 사전 점검: <Unity Hub에 로그인한 상태로 실행을 유지했는지 확인 / Unity 없는 Batch는 해당 없음>.
- 시작: 첫 단계 Tools/Preflight.ps1 -Batch <번호>로 Unity 실행 확인. 모든 Task needs-unity=no면 -SkipUnity. 실패하면 Unity 필요 Task와 의존 경로만 BLOCKED, 아래 목록은 계속한다. Unity는 승인된 샌드박스 밖 경로로 실행.
- Unity 없이 가능한 작업: <Task ID·선행·산출물·합격 기준. 없으면 없음>.
- 자율 처리 단계: [AGENTS A/B/C](../AGENTS.md#자율-처리-단계). Batch 허용 범위·사전 결정과 실행환경 권한 안에서 적용한다. B는 종료 보고 맨 위, C는 해당 경로만 중단하고 Decisions Needed.
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
- 종료 조건: 목표 상태 도달 또는 시간 예산 소진. 목표 도달·Task 완료/BLOCKED 후에도 시간과 안전 상한이 남으면 Backlog → Ready Queue. 둘 다 소진되면 종료하며 임의로 새 작업을 만들지 않는다.

## Queue·검증·인계

- Batch Task·Backlog가 완료 또는 중단되면 남은 시간·상한 안에서 Ready Queue 위부터. 실행 행의 허용 범위를 종료 카드에 합친다. 승인되지 않은 다음 Batch 후보는 실행하지 않는다. Queue가 비면 종료한다.
- Task마다 실제 검증·TASKS 갱신·이번 파일만 로컬 커밋. 수락만 남으면 REVIEW, 유용한 안정 결과의 기준 일부 잔여는 PARTIAL. 의존하지 않는 다음 Task는 계속한다.
- 게임 변경은 위험도별 기능·연결 회귀와 종료 기본 회귀. 문서/도구만이면 자체 검증으로 대체하고 Unity 미실시 명시.
- V5 영향이 있으면 AGENTS Housekeeping의 정적/Scene 명령에 실행파일·대상·기준 결과·새 출력 경로를 채운다. 기존/새 FAIL과 self-test를 분리한다. 정적 exit0만으로 통과하지 않는다.
- 필요한 Task 요약 최대1개씩, Batch 요약1개. 통과 실행은 한 줄 요약, 원본은 FAIL/BLOCKED/REVIEW 근거일 때만 Validation/_raw/<번호>/에 저장한다. 빌드/worktree/dll/Python 환경은 임시 폴더에서 쓰고 종료 시 삭제한다. regression 원본은 최근2 Batch만 펼치고 이전 zip(파일 수 확인 후 원본 삭제). FAIL·핵심 MEASURE 원문·미실시·Ruling(결정/이유/틀렸을 때 비용)을 적는다.
- Task 커밋 후 Batch-Card -Base <Base> -Allowed <Batch와 실행 Queue 범위>, 별도 git status/diff로 미커밋·미추적도 확인. 기존 사용자 변경은 분리 보존.
- TASKS 0·Current Tasks·Latest Handoff 갱신. 0절에 REVIEW 수를 적고 8 초과면 “다음 콘텐츠 Batch 전에 사용자 QA 1회 필요” 표시. 과거 이력 보관 시 미해결 항목·직접 플레이 순서는 유지.
- origin/main..HEAD에 다른 Batch 미푸시 커밋이 있으면 합치지 않는다. push하지 않는다. 게임 태그는 게임 Batch일 때만 정한다.
- 제출 전 모호한 합격 기준·실행 불가능한 명령·누락된 선행/범위·미승인 답을 확인한다. 문서 Task에 불필요한 수치를 강요하지 않는다.
