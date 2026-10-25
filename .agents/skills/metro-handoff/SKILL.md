---
name: metro-handoff
description: 메트로 1(game1) 저장소의 세션·Batch 시작과 종료, 현재 상태 확인과 인계를 위해 TASKS의 지금 절과 실제 Git 상태를 맞추고 결정·플레이 기록을 연결한다.
---

# metro-handoff

인계의 원본은 TASKS.md다. 이 skill은 읽기·기록만 하며 게임 코드·Scene·설계·수치를 변경하거나 새 Batch를 승인하지 않는다.

## 시작

1. TASKS.md `## 0. 지금`을 읽고 `git status --short`, `git log --oneline -5`, `git log --oneline origin/main..HEAD`, `git describe --tags --abbrev=0`으로 대조한다. origin/main은 로컬 참조이며 fetch하지 않았으면 원격 최신이라고 쓰지 않는다.
2. 실제 상태가 다르면 실제를 기준으로 차이를 기록한다. Batch 실행은 승인받은 Batches/BATCH-<번호>.md 전체와 그 파일이 가리킨 TASKS 절, 기획·리뷰는 TASKS 2·4·9절과 최신 요약을 읽는다. PRD·GAME_DESIGN·Design은 필요한 절만 추가한다.
3. 현재 HEAD/태그·미푸시 수·대기 주체·이번 범위·기록과 차이를 간단히 알린 뒤 승인 작업을 계속한다. 읽기 전용 검토는 문서 갱신·커밋 없이 차이만 보고한다.

## 종료

1. TASKS 0절을 15줄 이내로 갱신한다. 날짜·기록 주체, 진행 상태, 사용자/Claude/Codex 차례, Queue 수, 막힌 경로를 적는다. HEAD는 이 문서를 포함한 커밋과 자기참조가 되므로 기준 해시와 `git log -1` 확인법을 적어도 된다. 미푸시는 커밋 수와 Batch 수를 구분한다.
2. Codex가 Batch를 끝내면 9절을 AGENTS Handoff 순서로 갱신한다. 지난 Task 표는 Archive로 옮기고 상대 링크를 고친다. 미해결 수락·잔여 작업과 기존 플레이 순서는 현재 TASKS에 유지한다. 과거 상태를 새 DONE으로 바꾸지 않는다.
3. 이번에 확인된 사용자 결정만 2-1절에 날짜·출처·결정·근거로 기록하고 해당 대기 항목을 정리한다. 전달된 결정은 전달 출처를 명시한다. Claude 제안·측정 결과를 사용자 승인으로 바꾸거나 원문 없는 인용을 만들지 않는다.
4. 플레이 피드백은 10절에 날짜·방·피드백·변경 전후·근거를 기록한다. 결과를 아직 확인하지 않았으면 개선 완료라고 쓰지 않는다.
5. 승인된 쓰기 작업일 때만 이번에 갱신한 문서를 명시 add하여 로컬 커밋한다. 독립 인계는 `docs: handoff <날짜> <요약>`, Batch 안이면 Task/종료 커밋에 포함한다. push하지 않는다. 읽기 전용 요청에는 파일을 쓰지 않는다.

## 역할과 동시 편집

- Codex: 승인 범위 내 TASKS 전체·Validation, README는 실행 안내 변경이 승인된 경우만.
- Claude: 승인 범위 내 TASKS 0·2·2-1·10절, Batches/·Design/·PRD·GAME_DESIGN. 게임 구현은 별도 명시 지시가 있어야 한다.
- TASKS 0절이 Batch 진행 중이면 다른 에이전트는 같은 문서를 동시에 쓰지 않는다. 중단 표시가 오래됐다는 추측만으로 진행 중 상태를 지우지 않는다.
