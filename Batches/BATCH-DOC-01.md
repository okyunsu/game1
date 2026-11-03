# DOC-01 문서·기록 정리

- 승인: 사용자 옥윤수, 2026-11-03 이 대화 직접 지시. 목표는 규칙 중복·지난 기록 정리이며 아래 결정 밖 규칙 의미 변경은 금지.
- Base: `f1a6f5a2bf9a7436b6589d576b62f3f4190cf617`. 시작 2026-11-03 10:54 KST. 시간 90분, 종료 정리 10분 확보, Task 안전 상한 9(T0~T8).
- Unity 없이 가능한 작업: 전 Task. Unity 실행 금지, Preflight `-SkipUnity`.
- 허용: AGENTS.md, Batches/BATCH_TEMPLATE.md, 이 지시문, TASKS.md, DECISIONS.md, CLAUDE.md의 결정 기록 경로, README.md, Archive/TASKS_ARCHIVE.md, 루트 옛 설계 문서와 수정 가능한 문서 링크, Validation/DOC-01-summary.md, 명시된 Git 미추적 raw 정리.
- 금지: Unity·게임 코드·Scene·Assets·ProjectSettings·PRD.md·GAME_DESIGN.md·Design/* 수정, push, squash/rebase. Task마다 이번 파일만 로컬 커밋, 종료 로컬 태그 DOC-01.
- 보존: Design/proposals-1103-B26/, Design/v5/WORLD_V5.md, Design/v5/rooms_v5_AB.json, Validation/D61-room-rules-v5-AB.md. 수정·삭제·스테이징 금지.
- 재시도: 같은 오류는 1회만. Git 쓰기 권한 실패 시 Git 명령만 권한 상승 요청; 거부되면 파일을 보존하고 커밋 없이 종료.
- 종료: T0~T8의 목표 도달 또는 BLOCKED 기록, 90분 소진. 문서 전용 범위 밖 Backlog/Queue 실행 없음.

## 사전 결정

모두 사용자 2026-11-03 직접 지시. 부록 A/B/C 목록은 이번 요청 원문 그대로 AGENTS에 반영한다.

| ID | 결정 |
| --- | --- |
| D1 / D1-3 | 자율 처리 A 승인 없이 / B 처리 후 종료 보고 맨 위 / C 해당 경로 중단·Decisions Needed. GAME_DESIGN과 다른 기존 결함 수정은 재현·비보호·비감각수치 조건부 B-4. |
| D2 | 목표 상태 도달 또는 시간 소진. 기본 Task 안전 상한30, 목표 후 남은 시간에는 Backlog→Ready Queue. 이번 상한은9. |
| D3 | Batch 첫 단계 Unity 확인, 실패 시 필요한 경로만 BLOCKED하고 Unity 없는 목록 진행. 문서 전용은 -SkipUnity. 템플릿에 Hub 로그인 유지 사전 점검. |
| D4 | REVIEW 상한8, 종료 TASKS0에 개수. 초과 시 다음 콘텐츠 Batch 전에 사용자 QA 1회 필요. |
| D5 | 통과는 요약 한 줄, 원본은 FAIL/BLOCKED/REVIEW 근거일 때만 raw. raw에 빌드/worktree/dll/Python 환경 금지; 임시 폴더 사용·종료 삭제. regression 최근2 Batch만 펼치고 이전 zip. |
| D6 | 커밋으로 재생성 가능한 빌드/worktree/도구 환경 사본은 원본 증거가 아니므로 정리 가능. |
| D7 | B09/B10 지정 사본 정리. 내부 .txt/.csv/.md/.json은 같은 상대 경로 보존. regression 2026-10-31 이전 수정 파일은 zip 파일 수 확인 후 원본 삭제. |
| D8 | B21~B24 Current Tasks를 Archive/TASKS_ARCHIVE.md로. 미해결 REVIEW/PARTIAL/BLOCKED는 TASKS 단일 표 유지, B25 유지. |
| D9 | 날짜·출처 있는 사용자 결정을 DECISIONS.md로. TASKS2-1은 링크만. 제안은 구분, 이후 결정은 DECISIONS에만. |
| D10 | WORLD_V3.md / LEVEL_V2.md / LEVEL_V4.md를 Archive/design-old/로 git mv, 링크 보정. Tools/Assets 참조 또는 보호 파일 수정 필요 시 해당 이동 중단·보고. |
| D11 | push는 사용자 직접, 규칙 유지. |

## Tasks

모두 doc/needs-unity=no. 결과·증거·T0 바이트 비교는 [종료 요약](../Validation/DOC-01-summary.md) 하나에 기록한다.

| Task | 내용·합격 기준 |
| --- | --- |
| T0 | 상태·기존 변경 보존·루트 md/템플릿 바이트·Preflight -SkipUnity 기록 |
| T1 | AGENTS D1~D6/D11 반영, 결정 밖 의미 변경0, 모든 삭제·통합에 남은 절 대조 |
| T2 | 템플릿 목표/시간/Unity 없는 작업/사전 점검/자율 처리 참조 |
| T3 | 과거 표 보관·단일 미해결 표·REVIEW 수, 전후 상태 행 수 일치 |
| T4 | 결정 출처·날짜 구분하여 DECISIONS로 이동, TASKS2-1 링크만 |
| T5 | 저장소 세 파일명 참조 목록, 코드/도구·보호 참조 확인 후 가능한 경로만 이동 |
| T6 | 삭제 전 대상 MB/수, 경계 확인, 텍스트 보존·regression zip 수 검증·전후 raw MB/수 |
| T7 | README Batch 진행 문단은 TASKS 링크, 실행·런처·최신 직접 플레이 안내 유지 |
| T8 | 단일 요약·크기 전후·결정 반영·통합 대조·정리 결과·미실시, TASKS0/인계·카드·로컬 태그 |

## T0 시작 기록

- Git 상태 4행은 위 보존 목록과 일치. 그 밖 변경0, staged0. 최신 태그 B25-resume. 로컬 origin/main 기준 기존24커밋(게임 Batch 재개1묶음+별도 아트1), 이번 fetch/push 없음.
- PASS Preflight -SkipUnity: HEAD=f1a6f5a, workingTreeEntries=4, localOriginPendingCommits=24; Unity startup NOT RUN. sandbox LOCALAPPDATA WARN·Hub0·licensingClient unknown은 참고 정보이며 게임 실패로 판정하지 않음.
- 보호·보존 경로는 종료 시 시작 SHA256와 비교한다. 사용자 변경은 한 번도 스테이징하지 않는다.

| 파일 | 시작 bytes |
| --- | ---: |
| AGENTS.md | 24020 |
| CLAUDE.md | 797 |
| GAME_DESIGN.md | 44548 |
| LEVEL_V2.md | 9465 |
| LEVEL_V4.md | 3068 |
| PRD.md | 15875 |
| PROCESS_METRICS.md | 13768 |
| README.md | 16118 |
| RESOURCES.md | 2028 |
| TASKS.md | 54270 |
| WORLD_V3.md | 11467 |
| Batches/BATCH_TEMPLATE.md | 3452 |
