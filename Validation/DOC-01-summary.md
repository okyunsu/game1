# DOC-01 문서·기록 정리 — 종료 요약

BATCH CARD: 종료 T8에서 Task 커밋 뒤 실행 결과를 이 위치에 기록한다.

- 사용자 2026-11-03 직접 승인, Base `f1a6f5a`. 시간90분/Task9, Unity·게임 수정·push 없음.
- B 단계 처리: 없음. A 단계: 문서·raw 정리만 수행하며 부록 A 게임/도구 수정 없음.
- Git 최초 쓰기 실패: `git add -- Batches/BATCH-DOC-01.md TASKS.md` → `fatal: Unable to create 'C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/.git/index.lock': Permission denied`. 사용자 지시에 따라 Git만 권한 상승하여 T0 커밋 성공.
- PASS Preflight -SkipUnity: HEAD=f1a6f5a / 기존 변경4 / 로컬 origin 미푸시24 / Unity NOT RUN. sandbox LOCALAPPDATA WARN, Hub0, licensingClient unknown은 참고 정보.

## T1 삭제·통합·변경 대조

새 자율 처리 절의 A1~5/B1~4/C1~7은 사용자 부록과 동일. 결정 밖 규칙의 의미 변경0을 목표로 아래 변경을 문장별 대조했다. 삭제 대신 참조를 남겼으며 기존 사용자 전용 권한·보호·재시도·수락·push 규칙을 유지했다.

| 삭제·통합 또는 교체 전 문장 | 반영 후·남은 위치(절) | 근거 |
| --- | --- | --- |
| - Batch 시작 시 Tools/Preflight.ps1을 실행한다. 문서·분석·Python·운영 도구만 바꾸는 Batch는 -SkipUnity로 Git·환경 정보만 확인한다. | Environment — - Batch 첫 단계에서 Tools/Preflight.ps1로 Unity 실행을 먼저 확인한다. 실패하면 Unity가 필요한 Task와 그 의존 경로만 BLOCKED로 두고, Batch의 “Unity 없이 가능한 작업” 목록을 진행한다. 문서·분석·Python·운영 도구만 바꾸는 Batch는 -SkipUnity로 Git·환경 정보만 확인하며 Unity를 실행하지 않는다. | D3 |
| - 결과와 원본 로그는 Validation/_raw/<Batch>/의 새 실행 폴더에 보존한다. 실패 원문·미실시는 Task/Batch 요약에 옮긴다. 기존 결과는 덮어쓰지 않는다. | Evidence / Environment — - 실행 결과·실패 원문·미실시의 기록과 원본 보관은 Evidence를 따른다. 기존 결과는 덮어쓰지 않는다. | D5·중복 통합 |
| - Batch 안의 모든 Task를 끝낼 때까지 진행한다. 중간에 사용자에게 묻고 멈추지 않는다. | Batch Approval — - Batch 목표 상태 도달 또는 시간 예산 소진을 종료 기준으로 삼고, 아래 안전 상한·중단 조건 안에서 진행한다. 중간에 사용자에게 묻고 전체 실행을 멈추지 않는다. | D2 |
| - Batch의 Task가 모두 끝나거나 BLOCKED이면, 예산이 남아 있는 한 Batch에 적힌 Backlog를 위에서부터 진행한다. | Batch Approval — - Batch 목표 상태에 도달하거나 Task가 모두 끝나거나 BLOCKED여도, 시간이 남고 안전 상한 미도달이면 Batch Backlog를 위에서부터 진행한다. | D2 |
| 예산: 시간을 기본 기준으로 하고 Task 수는 안전 상한(명시 없으면 20)으로 쓴다. 둘 중 하나에 도달하면 새 Task를 시작하지 않고 현재 Task를 안전하게 정리한다. 종료 처리 시간을 먼저 확보한다. 시간이 남고 상한 미도달이면 Batch Backlog → Ready Queue 순으로 진행한다. Task를 쪼개 상한을 우회하지 않는다. Queue가 비면 종료하며 새 제품 작업을 임의로 승인하지 않는다. | Batch Approval — 예산: 시간을 기본 기준으로 하고 Task 수는 안전 상한(명시 없으면 30)으로만 쓴다. 목표 상태 도달 뒤에도 시간이 남으면 Batch Backlog → Ready Queue 순으로 계속한다. 시간 예산 또는 안전 상한에 도달하면 새 Task를 시작하지 않고 현재 Task를 안전하게 정리한다. 종료 처리 시간을 먼저 확보한다. Task를 쪼개 상한을 우회하지 않는다. Backlog·Queue가 소진되면 종료하며 새 제품 작업을 임의로 승인하지 않는다. | D2 |
| 5. TASKS Current Tasks 상태·결과를 갱신하고 이번 Task 파일만 로컬 커밋한다. | Git Rule / Definition of Done (Task) — 5. TASKS Current Tasks 상태·결과를 갱신하고 Git Rule에 따라 커밋한다. | 중복 통합 |
| - Batch 시작 전 작업 트리를 확인한다.<br> | Session Start / Git Rule — - 시작 상태 확인은 Session Start를 따른다.<br> | 중복 통합 |
| REVIEW는 기술 구현과 관련 Unity 검증이 완료되어 사용자 판단을 기다리는 상태다.<br><br>감각·설계 수락이 필요한 Task는 사용자가 수락한 뒤 DONE으로 바꾼다.<br><br>별도 사용자 판단이 없는 Task는 완료 조건과 필요한 검증을 충족하면 DONE으로 둔다. | Task Status / Definition of Done (Task) / Batch Approval / Review — 상태 정의·완료 판정은 Task Status와 Definition of Done (Task)을 따른다. 감각·설계 수락(REVIEW → DONE)은 Batch Approval의 사용자 권한이다.<br><br>REVIEW 대기 상한은 8개다. Batch 종료 시 Current Tasks·미해결 목록의 고유 Task ID를 기준으로 REVIEW 개수를 TASKS 0절에 적는다. 8개를 넘으면 “다음 콘텐츠 Batch 전에 사용자 QA 1회 필요”라고 표시한다. | D4·중복 통합 |
| 작은 Task마다 전체 검증을 반복하지 않는다.<br><br>전체 회귀는 아래 시점에 집중한다.<br><br>- Vertical Slice<br>- Sprint 종료<br>- Core Loop 통합<br>- Content Complete<br>- Release Candidate | Validation — Milestone — 작은 Task마다 전체 검증을 반복하지 않는다. 전체 회귀는 위 Milestone 시점에 집중한다. | 중복 통합 |
| - Task별 Validation 요약은 필요할 때 최대 1개, Batch 종료 요약은 1개 커밋한다. 같은 내용을 복제하지 않으며 원본 자동 산출물은 _raw/에 둔다.<br>- 원본 로그에 FAIL·MEASURE 줄이 있으면 실패 줄과 핵심 수치를 원문 그대로 넣고, 미실시 항목을 적는다.<br>- 원본 로그·CSV·json·PNG는 `Validation/_raw/`에 남기고 커밋하지 않는다(.gitignore 대상).<br>- 원본 자료는 삭제하지 않는다. | Evidence — - Task별 Validation 요약은 필요할 때 최대 1개, Batch 종료 요약은 1개 커밋한다. 같은 내용을 복제하지 않는다. 통과한 실행은 요약 한 줄만 남긴다.<br>- 원본 로그에 FAIL·MEASURE 줄이 있으면 실패 줄과 핵심 수치를 원문 그대로 요약에 넣고, 미실시 항목을 적는다. 이전 실행 결과를 이번 통과 증거로 대신하지 않는다.<br>- 원본 로그·CSV·json·PNG는 FAIL·BLOCKED·REVIEW의 근거일 때만 `Validation/_raw/<Batch>/`의 새 실행 폴더에 저장하고 커밋하지 않는다(.gitignore 대상). 기존 결과는 덮어쓰지 않는다.<br>- `_raw`에 빌드·worktree·dll·Python 환경을 넣지 않는다. 검증용 사본은 임시 폴더에 만들고 Batch 종료 시 삭제한다.<br>- regression 원본은 최근 2개 Batch분만 풀어 두고 이전 것은 zip으로 묶는다. 압축 파일 수와 원본 수가 일치하는지 확인한 뒤 압축한 원본을 삭제한다.<br>- 원본 자료는 삭제하지 않는다.<br>- 예외: Git 커밋으로 다시 만들 수 있는 빌드·worktree·도구 환경 사본은 원본 증거가 아니며 정리할 수 있다. | D5·D6 |
| - git push·reset --hard·clean·stash drop/pop/apply, 이미 push된 커밋 합치기는 실행하지 않는다. 저장 형식·ProjectSettings 변경은 사용자 명시 승인 없이는 중단한다. 로컬 reset --soft는 Housekeeping의 한정된 조건을 따른다. | Git Rule / Stop Conditions — - reset --hard·clean·stash drop/pop/apply, 이미 push된 커밋 합치기는 실행하지 않는다. push는 Git Rule을 따른다. 저장 형식·ProjectSettings 변경은 사용자 명시 승인 없이는 중단한다. 로컬 reset --soft는 Housekeeping의 한정된 조건을 따른다. | D11·중복 통합 |
| - 자동 실행 로그·csv·스크린샷은 Validation/_raw/에 둔다(git 제외). Validation/에는 필요한 Task 요약 최대1개씩과 Batch 종료 요약1개만 커밋한다(Evidence와 동일). | Evidence / Housekeeping — - 검증 요약·원본 보관·regression 압축·임시 사본 정리는 Evidence를 따른다. | D5·중복 통합 |
| - 배치 진행 중에는 Task마다 로컬 커밋한다. 배치 종료 후 push 직전에 | Git Rule / Housekeeping — - 배치 진행 중 커밋은 Git Rule을 따른다. 배치 종료 후 push 직전에 | 중복 통합 |
