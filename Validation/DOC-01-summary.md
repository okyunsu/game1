# DOC-01 문서·기록 정리 — 종료 요약

BATCH CARD (마지막 Task 커밋 79e74c6 기준, T5 이후 종료 기록은 미커밋):

```text
BATCH CARD f1a6f5a..HEAD (79e74c6)
변경 파일: 8 (+389 / -186 줄, 바이너리 0)
  Scripts: 0
  Scenes: 0
  Prefabs: 0
  Editor: 0
  Validation: 1
  문서: 7
  기타: 0
허용 범위: AGENTS.md, CLAUDE.md, Batches/BATCH-DOC-01.md, Batches/BATCH_TEMPLATE.md, TASKS.md, DECISIONS.md, Archive/TASKS_ARCHIVE.md, Validation/DOC-01-summary.md
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 27 / REVIEW 20 / DOING 0 / BLOCKED 5 / PARTIAL 17
결정 대기(## 2. 결정 대기 표의 D- 행): 4
열린 버그(## 4. 열린 버그 표 행): 7
직접 플레이 확인 대기(## 3. 목록 항목): 14
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 13/13 (Validation/_raw/regression/B25-delivery-final/result.txt)
```

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

## T3 상태 보존

| 대상 | REVIEW | PARTIAL | BLOCKED |
| --- | ---: | ---: | ---: |
| 이동 전 B21~B24 | 8 | 6 | 2 |
| 이동 후 현재 목록의 B21~B24 | 8 | 6 | 2 |
| 이동 전 현재 Task 표 전체 | 20 | 16 | 4 |
| 이동 후 현재 Task 표 전체 | 20 | 16 | 4 |

PASS 상태별 행/Task ID/상태/결과 원문 전후 동일, B25 본문·표 원문 동일. REVIEW 총 21개는 표 REVIEW 20개 + 기존 본문 B20-T4 1개이며, 보관 표·Handoff 중복 언급은 합산하지 않는다. 과거 해소/대체 여부를 이번 정리에서 재판정하지 않음. 직접 플레이 목록·순서 불변.

## T4 결정 장부

PASS 기존 결정 ID 44개 각1회 보존, DOC-01 D1~D11/D1-3 추가. D-DRAFT5·D-CLIMB-GAP·D-HUB-LIFT는 제안, D-BIG-BATCH는 개별 날짜 미확인 전달로 구분해 별도 절 보존. 추천값의 실행 승인과 감각 수락을 구분하고 원문 출처 한계를 유지했다. TASKS2-1은 링크만 남겼다.

T1 이후 D9에 따른 추가 교체: AGENTS Handoff의 “실제 사용자 사전 결정을 TASKS 2-1에 옮긴다” → DECISIONS.md만 기록·TASKS2-1 링크(남은 위치: AGENTS Handoff, DECISIONS). 템플릿·CLAUDE 결정 경로도 함께 정합화했다. 읽기 전용 `.agents/skills/metro-handoff/SKILL.md`의 예전 2-1 기록 안내는 사용자 D9와 갱신 AGENTS가 우선하며 스킬 파일은 수정하지 않았다.

## T5 저장소 전체 참조 목록 (이동 전)

`rg -n --hidden --no-ignore -F -g !.git/** <파일명> .`로 Git 제외 raw·Library도 검색했다. 바이너리 파일은 rg 기본 동작에 따라 본문 검색 대상이 아니며 Git 내부는 제외했다. 아래는 파일별 행 번호이며 원본 기록의 과거 경로 문자열은 당시 증거로 보존한다.

| 대상 | 참조 파일 | 행 |
| --- | --- | --- |
| WORLD_V3.md | `Archive/GAME_DESIGN-v0.2-2026-10-15.md` | 17, 89 |
| WORLD_V3.md | `Archive/validation-2026-10/B06-preflight-review.md` | 7, 8 |
| WORLD_V3.md | `Batches/BATCH-06.md` | 6, 8, 28, 35, 36, 98 |
| WORLD_V3.md | `Batches/BATCH-06B.md` | 8 |
| WORLD_V3.md | `Batches/BATCH-07.md` | 6, 9, 19 |
| WORLD_V3.md | `Batches/BATCH-DOC-01.md` | 27, 64 |
| WORLD_V3.md | `DECISIONS.md` | 20 |
| WORLD_V3.md | `GAME_DESIGN.md` | 11, 47, 129 |
| WORLD_V3.md | `Validation/_raw/B09/worktrees/after/BATCH-06.md` | 6, 8, 28, 35, 36, 98 |
| WORLD_V3.md | `Validation/_raw/B09/worktrees/after/BATCH-06B.md` | 8 |
| WORLD_V3.md | `Validation/_raw/B09/worktrees/after/GAME_DESIGN.md` | 11, 83 |
| WORLD_V3.md | `Validation/_raw/B09/worktrees/after/Validation/B06-preflight-review.md` | 7, 8 |
| WORLD_V3.md | `Validation/_raw/B09/worktrees/before/BATCH-06.md` | 6, 8, 28, 35, 36, 98 |
| WORLD_V3.md | `Validation/_raw/B09/worktrees/before/BATCH-06B.md` | 8 |
| WORLD_V3.md | `Validation/_raw/B09/worktrees/before/GAME_DESIGN.md` | 11, 83 |
| WORLD_V3.md | `Validation/_raw/B09/worktrees/before/Validation/B06-preflight-review.md` | 7, 8 |
| WORLD_V3.md | `Validation/_raw/B10/bisect/BATCH-06.md` | 6, 8, 28, 35, 36, 98 |
| WORLD_V3.md | `Validation/_raw/B10/bisect/BATCH-06B.md` | 8 |
| WORLD_V3.md | `Validation/_raw/B10/bisect/GAME_DESIGN.md` | 11, 83 |
| WORLD_V3.md | `Validation/_raw/B10/bisect/Validation/B06-preflight-review.md` | 7, 8 |
| WORLD_V3.md | `Validation/_raw/B10/head-control/BATCH-06.md` | 6, 8, 28, 35, 36, 98 |
| WORLD_V3.md | `Validation/_raw/B10/head-control/BATCH-06B.md` | 8 |
| WORLD_V3.md | `Validation/_raw/B10/head-control/GAME_DESIGN.md` | 11, 83 |
| WORLD_V3.md | `Validation/_raw/B10/head-control/Validation/B06-preflight-review.md` | 7, 8 |
| LEVEL_V2.md | `Archive/B05-handoff-20261010.md` | 39 |
| LEVEL_V2.md | `Archive/README-history-2026-10.md` | 27 |
| LEVEL_V2.md | `Archive/TASKS-B20.md` | 174 |
| LEVEL_V2.md | `Archive/docs/V2_QA.md` | 15 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-0-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-A01-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-A02-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-A03-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-A04-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-A05-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-A06-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-B01-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-B02-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-B03-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-B04-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-B05-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-B06-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-C01-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-C02-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-C03-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-C04-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-C05-summary.md` | 5 |
| LEVEL_V2.md | `Archive/validation-2026-10/B05-C06-summary.md` | 5 |
| LEVEL_V2.md | `Batches/BATCH-07.md` | 9 |
| LEVEL_V2.md | `Batches/BATCH-DOC-01.md` | 27, 57 |
| LEVEL_V2.md | `DECISIONS.md` | 20 |
| LEVEL_V2.md | `TASKS.md` | 146 |
| LEVEL_V2.md | `Validation/_raw/21/T3-20261026-001128/TASKS-before.md` | 90 |
| LEVEL_V2.md | `Validation/_raw/21/resume-20261026-210738/TASKS-before-resume.md` | 110 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Archive/B05-handoff-20261010.md` | 39 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/README.md` | 19 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/V2_QA.md` | 15 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-0-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-A01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-A02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-A03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-A04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-A05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-A06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-B01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-B02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-B03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-B04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-B05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-B06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-C01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-C02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-C03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-C04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-C05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/Validation/B05-C06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/after/WORLD_V3.md` | 3 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Archive/B05-handoff-20261010.md` | 39 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/README.md` | 19 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/V2_QA.md` | 15 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-0-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-A01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-A02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-A03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-A04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-A05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-A06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-B01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-B02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-B03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-B04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-B05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-B06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-C01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-C02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-C03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-C04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-C05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/Validation/B05-C06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B09/worktrees/before/WORLD_V3.md` | 3 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Archive/B05-handoff-20261010.md` | 39 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/README.md` | 19 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/V2_QA.md` | 15 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-0-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-A01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-A02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-A03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-A04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-A05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-A06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-B01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-B02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-B03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-B04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-B05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-B06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-C01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-C02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-C03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-C04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-C05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/Validation/B05-C06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/bisect/WORLD_V3.md` | 3 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Archive/B05-handoff-20261010.md` | 39 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/README.md` | 27 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/V2_QA.md` | 15 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-0-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-A01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-A02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-A03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-A04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-A05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-A06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-B01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-B02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-B03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-B04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-B05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-B06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-C01-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-C02-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-C03-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-C04-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-C05-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/Validation/B05-C06-summary.md` | 5 |
| LEVEL_V2.md | `Validation/_raw/B10/head-control/WORLD_V3.md` | 3 |
| LEVEL_V2.md | `Validation/_raw/B13/md-broken-after-A2.txt` | 98 |
| LEVEL_V2.md | `Validation/_raw/B13/md-broken-before.txt` | 98, 669, 954, 1212, 1493 |
| LEVEL_V2.md | `Validation/_raw/B19/T5-TASKS-draft.md` | 152 |
| LEVEL_V2.md | `WORLD_V3.md` | 3 |
| LEVEL_V4.md | `Archive/B10-prior-handoff-20261012.md` | 25 |
| LEVEL_V4.md | `Archive/B10-start-tasks-20261012.md` | 189 |
| LEVEL_V4.md | `Archive/README-history-2026-10.md` | 7 |
| LEVEL_V4.md | `Archive/TASKS-B20.md` | 187 |
| LEVEL_V4.md | `Archive/TASKS-before-2026-10-12.md` | 215 |
| LEVEL_V4.md | `Batches/BATCH-09.md` | 66 |
| LEVEL_V4.md | `Batches/BATCH-10.md` | 9 |
| LEVEL_V4.md | `Batches/BATCH-DOC-01.md` | 27, 58 |
| LEVEL_V4.md | `DECISIONS.md` | 20 |
| LEVEL_V4.md | `GAME_DESIGN.md` | 11, 67, 91, 187, 307 |
| LEVEL_V4.md | `PRD.md` | 43, 89 |
| LEVEL_V4.md | `README.md` | 53 |
| LEVEL_V4.md | `TASKS.md` | 171 |
| LEVEL_V4.md | `Validation/_raw/21/T3-20261026-001128/TASKS-before.md` | 103 |
| LEVEL_V4.md | `Validation/_raw/21/resume-20261026-210738/TASKS-before-resume.md` | 123 |
| LEVEL_V4.md | `Validation/_raw/B10/head-control/README.md` | 7 |
| LEVEL_V4.md | `Validation/_raw/B10/head-control/TASKS.md` | 189 |
| LEVEL_V4.md | `Validation/_raw/B10/write_handoff.py` | 56 |
| LEVEL_V4.md | `Validation/_raw/B13/md-broken-after-A2.txt` | 228, 291, 358 |
| LEVEL_V4.md | `Validation/_raw/B13/md-broken-before.txt` | 228, 291, 359 |
| LEVEL_V4.md | `Validation/_raw/B19/T5-TASKS-draft.md` | 162 |

## T5 중단·자동 승인 거부

| 파일 | 실제 참조 조사 | 결과 |
| --- | --- | --- |
| WORLD_V3.md | GAME_DESIGN 11/47/129행 등, 활성 Tools/Assets 참조0 | 보호 문서 링크를 수정할 수 없어 이동 보류 |
| LEVEL_V4.md | PRD 43/89행·GAME_DESIGN 11/67/91/187/307행 등, 활성 Tools/Assets 참조0 | 보호 문서 링크를 수정할 수 없어 이동 보류 |
| LEVEL_V2.md | 활성 PRD/GAME_DESIGN/Tools/Assets 참조0, TASKS·Archive 등 문서 참조 | git mv 자동 승인 거부, 파일 이동0·링크 변경0 |

`Archive/design-old/` 빈 목적 폴더만 생성했다. Git은 빈 폴더를 추적하지 않는다. 실행되지 않은 이동 명령:

```text
git mv -- LEVEL_V2.md Archive/design-old/LEVEL_V2.md
```

자동 승인 검토 오류 원문:

```text
exec_command failed: CreateProcess { message: "Rejected(\"This action was rejected due to unacceptable risk.\nReason: The requested move is generally covered by D10, but the scan shows references from protected PRD/GAME_DESIGN files that cannot be updated under this Batch, so moving LEVEL_V2.md would leave broken links and exceeds the authorized conditional scope.\nDo not bypass this rejection through a workaround or indirect execution. Continue with a safer alternative, or carry out checks to prove that the action is authorized or low risk before trying again. Complete unaffected work without asking for confirmation. Report anything that remains blocked, clarify why it was blocked by auto-review, inform the user of the risk and ask for approval.\")" }
```

검토 이유는 LEVEL_V2에 대한 실제 PRD/GAME_DESIGN 검색 결과와 일치하지 않는다. 그러나 이번 사용자 중단 조건 “git 명령만 샌드박스 밖 실행을 요청한다. 거부되면 파일 변경은 둔 채 커밋 없이 종료 보고한다”에 따라 재시도·대체 이동 없이 종료했다. 이후 변경은 TASKS·이 요약의 종료 기록뿐이며 미커밋으로 보존한다.

## Task 결과·미실시

| Task | 상태 | 커밋·실제 결과 |
| --- | --- | --- |
| T0 | DONE | d477cd7 — 상태·바이트·Preflight -SkipUnity |
| T1 | DONE | 7f16ce2 — 운영 결정·중복 문장 대조 |
| T2 | DONE | e12dfc7 — 템플릿 필드/앵커 확인 |
| T3 | DONE | 5a2acaa — B21~B24 보관, 상태별 행/결과/B25/플레이 순서 보존 |
| T4 | DONE | 79e74c6 — 44개 결정 ID 보존·제안/미확인4행 분리·DOC-01 결정 |
| T5 | BLOCKED | 전체 참조 조사만 완료·이동0. 조사/차단 기록 미커밋 |
| T6 | TODO·미실시 | 디렉터리 존재만 사전 조회. 삭제 전 크기·파일 수/텍스트 보존 목록 측정, 삭제, regression zip 생성·원본 삭제, 전후 측정 모두 미실시 |
| T7 | TODO·미실시 | README 원문 유지 |
| T8 | PARTIAL | 이 요약·상태/크기·중단 인계만 작성, 종료 커밋·DOC-01 태그 미실시 |

T6 정리 결과: 삭제0/zip생성0. Preflight가 자동 생성한 `_raw/DOC-01/preflight-20261103-105442-481-4900bf9a/`의 통과 원본은 중단 전 그대로 남아 있다. D5 보관 정책에 맞춘 해당 통과 원본 정리도 미실시다. 기존 raw 증거는 손대지 않았다.

PASS 기존 사용자 보존6파일 SHA256 일치·미스테이징, PRD/GAME_DESIGN/README/옛설계3파일의 T0 크기 및 Git Base 정규화 내용 동일. Unity·게임 코드·Scene·Assets·ProjectSettings 수정/실행0. Unity 회귀·V5 정적/Scene 검사는 문서 전용이고 게임 변경0이라 미실시. push/fetch/rebase/squash 없음.

종료 검증 첫 시도는 `AssertionError: PRD.md`로 실패했다. 작업 트리 CRLF 바이트와 Git LF blob의 직접 비교 때문이며, 파일 크기·줄바꿈 정규화 내용·기존 사용자 파일 SHA256로 한 번 재검증하여 통과했다. 제품/문서 내용 오류를 숨긴 것이 아니며 검증 방법의 차이를 명시한다.

## T0 대비 바이트 크기

| 파일 | 시작 bytes | 종료 bytes | 차이 |
| --- | ---: | ---: | ---: |
| AGENTS.md | 24020 | 26909 | +2889 |
| CLAUDE.md | 797 | 887 | +90 |
| GAME_DESIGN.md | 44548 | 44548 | +0 |
| LEVEL_V2.md | 9465 | 9465 | +0 |
| LEVEL_V4.md | 3068 | 3068 | +0 |
| PRD.md | 15875 | 15875 | +0 |
| PROCESS_METRICS.md | 13768 | 13768 | +0 |
| README.md | 16118 | 16118 | +0 |
| RESOURCES.md | 2028 | 2028 | +0 |
| TASKS.md | 54270 | 38453 | -15817 |
| WORLD_V3.md | 11467 | 11467 | +0 |
| Batches/BATCH_TEMPLATE.md | 3452 | 4805 | +1353 |
| DECISIONS.md (신규) | 0 | 14547 | +14547 |

## 결정별 반영 위치

| 결정 | 반영 파일·절 | 상태 |
| --- | --- | --- |
| D1·D1-3 | AGENTS 자율 처리 단계 A/B/C·B-4 | 반영 |
| D2 | AGENTS Batch Approval·BATCH_TEMPLATE 목표/예산/종료 | 반영 |
| D3 | AGENTS Environment·BATCH_TEMPLATE 시작/사전 점검/Unity 없는 목록 | 반영(이번 Unity는 명시 금지·SkipUnity) |
| D4 | AGENTS Review·BATCH_TEMPLATE 인계·TASKS0 REVIEW21/QA 표시 | 반영 |
| D5·D6 | AGENTS Evidence·Environment/Housekeeping 참조·BATCH_TEMPLATE 증거 | 규칙 반영, 이번 raw 정리는 미실시 |
| D7 | DOC-01 T6 | 미실시 |
| D8 | TASKS1-1 미해결 목록·Archive/TASKS_ARCHIVE DOC-01 보관 절 | 반영 |
| D9 | DECISIONS·TASKS2-1 링크·AGENTS Handoff·CLAUDE 역할·템플릿 결정 경로 | 반영 |
| D10 | DOC-01 T5·TASKS 결정 대기 D-DOC-OLDLINK | 참조 조사, 이동 차단 |
| D11 | AGENTS Git Rule | 사용자 직접 push 규칙 유지 |

Ruling: REVIEW 수는 현재 Task 표·미해결 표의 고유 ID와 기존 본문 B20-T4를 합산 / Handoff·보관 표의 중복 수를 제외하기 위함 / 잘못된 과거 REVIEW 상태가 있으면 사용자 수락 근거로 재판정 필요(이번에는 변경하지 않음).

## 별도 작업 트리 확인

PASS 문서 대상 `git diff --check`, 커밋 범위 밖0·staged0·TASKS 깨진 링크0·없는 빌드/런처0. 카드의 마지막 REGRESSION 행은 과거 B25 실행이며 DOC-01 통과 증거가 아니다. 카드의 표 REVIEW20과 TASKS0의21 차이는 기존 본문 B20-T4 1개다.

중단 후 `git status --short`:

```text
 M Design/v5/WORLD_V5.md
 M Design/v5/rooms_v5_AB.json
 M TASKS.md
 M Validation/DOC-01-summary.md
?? Design/proposals-1103-B26/
?? Validation/D61-room-rules-v5-AB.md
```

중단 후 `git log --oneline -5`:

```text
79e74c6 docs(DOC-01 T4): move sourced user decisions into dedicated ledger
5a2acaa docs(DOC-01 T3): archive past batches and preserve unresolved task rows
e12dfc7 docs(DOC-01 T2): align batch template with approved workflow
7f16ce2 docs(DOC-01 T1): apply approved operating rules and consolidate duplicates
d477cd7 docs(DOC-01 T0): record approved scope and starting state
```

로컬 DOC-01 태그는 생성하지 않았다. 기존 사용자 변경 외 미커밋은 TASKS.md와 이 요약뿐이며, 승인 거부 이후에는 어떤 Git 쓰기 명령도 실행하지 않았다.

## DOC-01 재개 — 사용자 2026-11-03 직접 지시

T5: 보호 문서가 참조해 이동 보류(사용자 결정 2026-11-03), 다음 GAME_DESIGN 수정 때 함께 처리

이번 실행은 직전 T8 기록 커밋·T6·T7·종료 태그만 수행한다. T5 이동은 실행하지 않는다. 권한/자동 승인 거부는 해당 항목만 건너뛰고 원문 기록 후 다음 항목 진행, 전체 종료는 Git 커밋 자체가 불가능할 때만 한다. 기존 Batch 문서의 이전 전체 중단 규칙보다 이 사용자 재개 지시가 우선한다.

## T6 재개 — 삭제 전 측정

- 삭제 대상은 B09/B10 내부의 이름이 Builds/worktrees/python인 폴더이며, 포함 관계는 상위 폴더 한 번만 합산한다. bisect·head-control에서는 Builds 하위만 대상으로 삼는다. .txt/.csv/.md/.json은 같은 상대 경로에 그대로 보존한다.
- 전체 raw 시작: 2372.6 MB / 63772파일(숨김 파일 포함, 1MB=1,048,576 bytes).

| 폴더 | MB | 파일 수 | 보존 텍스트 수 | 삭제 대상 수 |
| --- | ---: | ---: | ---: | ---: |
| Validation/_raw/B09/python | 119.7 | 3385 | 63 | 3322 |
| Validation/_raw/B09/worktrees | 1015.7 | 20612 | 4854 | 15758 |
| Validation/_raw/B10/bisect/Builds | 267.6 | 696 | 6 | 690 |
| Validation/_raw/B10/head-control/Builds | 89.3 | 232 | 2 | 230 |


T6 삭제 명령 첫 시도는 PowerShell 문자열의 Markdown fence 파싱으로 실행 전 실패했다. 삭제된 파일0. 한 번 수정하여 재시도한다.
```text
ParserError:
Line |
  42 |  … ($failures -join "`n") + "`n```" } else { $report += "`nPASS 대상 절대경로 …
     |                                                           ~~~~~~
     | Unexpected token '`nPASS' in expression or statement.
```

### B09/B10 정리 결과

| 폴더 | 삭제 파일 | 줄어든 MB | 보존 텍스트 | SHA256 일치 | 남은 파일 | 남은 MB |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Validation/_raw/B09/python | 3322 | 118.2 | 63 | 63 | 63 | 1.5 |
| Validation/_raw/B09/worktrees | 15758 | 932.9 | 4854 | 4854 | 4854 | 82.8 |
| Validation/_raw/B10/bisect/Builds | 690 | 267.6 | 6 | 6 | 6 | 0.0 |
| Validation/_raw/B10/head-control/Builds | 230 | 89.3 | 2 | 2 | 2 | 0.0 |

PASS 대상 절대경로 B09/B10 내부·reparse0, 텍스트 동일 경로·해시 전부 보존, 범위 밖 삭제0.


### regression 압축

PASS 기준 LastWriteTime < 2026-10-31 00:00(로컬), 기존 zip 제외. 원본 4635 / zip 4635 / 압축 내부 SHA256 일치 4635, 확인 후 원본 4635 파일 삭제. 각 원본 상대 경로를 zip에 유지했고 10/31 이후 파일은 유지했다.

| 원본 파일 수 | 원본 MB | zip 파일 수 | zip MB | 검증 후 삭제 수 |
| ---: | ---: | ---: | ---: | ---: |
| 4635 | 66.6 | 4635 | 17.0 | 4635 |


### T6 전체 전후

| 구분 | MB | 파일 수 |
| --- | ---: | ---: |
| 정리 전 | 2372.6 | 63772 |
| 정리 후 | 915.1 | 39138 |
| 감소 | 1457.5 | 24634 |


PASS T6: 지정 경계 내 비텍스트20,000개 삭제·보존 텍스트4,925개 경로/해시 동일, regression4,635개 압축 수/해시 일치 후 원본 삭제, 전체 raw 감소1,457.5MB. raw 파일은 Git 미추적이며 커밋 대상에서 제외한다.

## T7 README 정리

PASS 현재 Batch/Sprint 진행 문단과 과거 B19/B20/B21 결과를 TASKS 링크로 교체. B25/직전B24 빌드 경로는 실행 절에 유지. 모든 런처 링크의 목록·순서, 기존 실행/조작 본문, 최신 B25 Loop·Graybox·보스 시험장·저장/초기화·조정 위치·기록칸 문단이 원문과 일치함을 직접 비교했다. 이번 변경은 안내 중복만 제거하며 게임 동작·설계·검증 판정을 바꾸지 않는다.
