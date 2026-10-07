# 운영 정리 검토·결과 — 2026-10-07

사용자의 “검토 후 진행해줘”에 따라 첨부 CODEX_운영정리_프롬프트.md의 6개 항목을 실행했다. 초기 작업 트리는 깨끗했다. START_COMMIT: `c62565948a0dc49c39850ba4668eb510b434f90d`. 모든 비교는 이 커밋 기준이며 origin/main이나 이전 로컬 작업을 이번 변경으로 집계하지 않았다.

## 검토 판단

- 새 AGENTS는 기존 PROC-001을 대체한다. Batch 승인, PARTIAL, 미검증과 BLOCKED 구분, 위험도별 최소 검증, 원시 자료 보존이 새 기준이다. 기존 PROC-001은 TASKS Archive의 역사 기록이다.
- 게임 내용 보존 지시를 우선했다. PRD와 GAME_DESIGN의 기능별 수락 기준, FPS·게이트·튜닝 검증 요구, 관찰 인원·시간 목표는 설계 기준으로 유지했다. 명확히 분리 가능한 중복 에이전트 절차 문단은 발견하지 못해 두 문서는 수정하지 않았다.
- 오래된 PARTIAL·REVIEW·BLOCKED 문단은 일부 후속 결과와 중복되지만 상태 정리를 추정하지 않고 현재 TASKS에 유지했다. 최신 S3T-01 및 Decisions Needed를 먼저 확인한다.
- 승인된 문서 작업만 수행했다. Unity·게임 회귀·빌드·검증 도구 제작·파일 삭제·push는 수행하지 않았다.

## 항목별 로컬 커밋

| 항목 | 커밋 | 메시지 |
| --- | --- | --- |
| 1 AGENTS 교체 | 2af84ed | docs: replace agent procedures with batch workflow |
| 2 _raw ignore | 443ee0e | chore: ignore untracked raw validation evidence |
| 3 TASKS 정리 | ffa3520 | docs: archive completed task history and simplify active tracking |
| 4 나머지 문서 구성 | 18e299e | docs: describe batch and archive document structure |
| 5 도구 목록 | 50a11e7 | docs: inventory existing verification tools without modifying them |
| 6 Batch 템플릿 | 33a2755 | docs: add the approved batch template verbatim |

보고·실행 지표를 별도 마감 커밋으로 남긴다. 최종 커밋 해시는 Git 이력 및 최종 응답에 표시한다.

## 문서별 변경

- [AGENTS](../AGENTS.md): 첨부 지정 구간 전체 교체, 내용 변경 없음.
- [.gitignore](../.gitignore): Validation/_raw/ 한 줄 추가. 추적 파일은 변경·이동하지 않음.
- [TASKS](../TASKS.md): 지정된 5개 최상위 섹션만 유지, 7개 상태·현재 인계 및 한 번의 도구 목록 반영.
- [TASKS Archive](TASKS_ARCHIVE.md): 완료/해결/과거 기록 원문 보존, 루트 상대 링크만 ../로 보정.
- [README](../README.md): 문서 구성 목록만 변경. 실행 안내는 그대로 유지.
- [BATCH_TEMPLATE](../BATCH_TEMPLATE.md): 첨부 지정 템플릿 그대로 생성.
- [PROCESS_METRICS](../PROCESS_METRICS.md): 이번 문서 실행 한 줄 추가. 이전 집계는 재계산하지 않음.
- [PRD](../PRD.md), [GAME_DESIGN](../GAME_DESIGN.md), [LEVEL_PROPOSALS](../LEVEL_PROPOSALS.md): 변경 없음. PRD/GAME_DESIGN에서 삭제한 문단 없음.

## Archive로 옮긴 섹션·내용

파일 이동은 없었다. 아래 TASKS 원문을 Archive/TASKS_ARCHIVE.md로 옮겼다.

- Roadmap, Human Review, Validation Records, Backlog, Playtest Records, Design Decision / Before-After, Resource Register, 기존 Latest Handoff 전체. BOSS-2 TODO는 Current Tasks에도 유지하고 열린 후보·관찰 수락은 새 인계에 연결했다.
- Current Tasks의 완료 행: S1-01~10 및 S1-G(S1-07-T DEFERRED 제외), T1~T4, S2-01~03/05/06, S3-01~05. 기존 의존성·수치·완료 조건 행을 보존했다.
- Current Tasks의 이전 실행 문단: night4 및 앞선 TM/U0/U1, D0/F2R/LOGR, F2, v2, G0~G3, C0~C4, 야간 선행 승인/수락 이력. 최신 N5/N6/N7/N8/N9/S3T-01 요약은 유지했다.
- 해결된 Bugs: N9-UI-001, N8-POGO-001, Q1-SAVE-001, S2-03-001, BUG-001, BUG-002, CLEAN-001, SLICE-001, NIGHT-T4-001. Q1 뒤의 미해결 UI-CAP-001은 현재 Bugs에 따로 보존했다.
- 확정/해소된 Decisions: DEC-POGO-001, N8-POGO-ORDER, N6 시작 기준의 사용자 중단 결정, DEC-F2R-LOGR, S1-06/N2-A 기록 및 해결된 이전 배치·시야·Slice·Room 결정 문단. 현재 F1·LOG-001·K2 및 열린 질문은 유지했다.
- 모든 원문에서 비어 있지 않은 각 줄을 현재 TASKS 또는 Archive와 대조했다. 아래 변경 6줄 외에는 누락 없음. Archive에 새로운 분류 표제를 붙였고 링크 대상은 유지했다.

## TASKS 검증 절차 변경 전후

게임 동작·좌표·수치·사용자 수락 조건은 바꾸지 않았다. 과거 실제 PASS 결과는 Validation과 Archive에 그대로 남아 있다.

| Task | 변경 전 절차 문구 | 변경 후 절차 문구 |
| --- | --- | --- |
| S3-06 | B01/B02/B03 왕복 및 필수 회귀 PASS | B01/B02/B03 왕복 및 변경 위험에 비례한 최소 검증(AGENTS.md Validation) |
| S3-07 | 스폰 비겹침·필수 회귀 PASS | 스폰 비겹침·변경 위험에 비례한 최소 검증(AGENTS.md Validation) |
| S3-08 | CP→C06 입구1.280s·필수 회귀 PASS | CP→C06 입구1.280s·변경 위험에 비례한 최소 검증(AGENTS.md Validation) |
| S3-09 | 문 왕복과 필수 회귀 PASS | 문 왕복과 변경 위험에 비례한 최소 검증(AGENTS.md Validation) |
| S3-10 | 가상패드 UI와 필수 회귀 PASS | 가상패드 UI와 변경 위험에 비례한 최소 검증(AGENTS.md Validation) |

별도로 상태 설명 1줄에 PARTIAL을 추가하고 새 AGENTS의 7개 상태와 맞췄다. 의존성 :DONE은 Archive의 완료 행을 확인하며 자동 수락하지 않는다.

## 검증 도구 목록

요청대로 표는 별도 파일에 중복 작성하지 않았다. [TASKS Latest Handoff](../TASKS.md#latest-handoff)의 맨 아래에 이름·모든 포함 파일 위치·목적·새 기준의 사용 시점을 계열별로 기록했다. 삭제 여부는 사용자 결정이며 다음 Batch 인계에서 목록을 뺀다. Editor 작성·빌드 도구와 세 개 열기 메뉴도 구분했다.

Validation 파일 목록은 실행 스크립트, 승인 원문·로그/CSV/입력 JSON·실패 자료, 인계·QA, 화면 증거로 확인했다. 기존 파일은 이동·이름 변경·수정하지 않았다.

## 발견했지만 손대지 않은 점

- PROCESS_METRICS는 현재 TASKS 이력 및 Validation/S3SW-user-qa.md에서 링크한다. 운영 자료이지만 현재 링크된 파일 이동 금지 예외로 위치를 유지했다. LEVEL_PROPOSALS도 현재 QA와 배치 이력에서 쓰는 설계 자료이므로 유지했다. Archive/docs/로 옮길 적격 파일은 없었다.
- PRD의 관찰 인원·최종 빌드 요구, GAME_DESIGN의 기능별 테스트·게이트 영상·변경 후 거리 검증·Playtest Metrics는 설계/수락 기준과 운영이 섞여 있어 그대로 유지했다.
- GAME_DESIGN §18의 구키 표기, README의 과거 빌드/상태 안내, N7~N8의 이후 해소된 일부 상태, LOG-001과 후속 LOGR의 관계는 이번 문서 정리에서 추정해 고치지 않았다. 현재 TASKS의 최신 요약·열린 질문과 Archive 원문으로 판단한다.
- S3T01-LEVEL/A-1은 범위 충돌은 해소됐지만 재적용 판단이 남아 있어 Decisions Needed에 유지했다. ROOM-001·RESET-001·F2 원인 기록 등 애매한 문제도 보존했다.
- 기존 RouteAll R06 차단과 Pogo 일회 실패는 이번 문서 변경의 게임 회귀로 판정하지 않았다. 새 플레이 결과를 만들지 않았으며 **플레이 전 확인 필요** 표시를 유지했다.
- GAME_DESIGN의 “TASKS의 Playtest Records / Design Decision” 문구는 이제 Archive에 보존된 이력을 가리킨다. 설계 기준 문단에 섞여 있어 수정하지 않았고, TASKS Current Tasks에서 Archive로 연결했다.

## 실제 검증

- AGENTS와 BATCH_TEMPLATE: 첨부 지정 구간과 본문 일치 PASS(줄바꿈 형식만 정규화 비교).
- TASKS: 최상위 5섹션 확인 PASS. 원문 각 비공백 줄 보존 검사 PASS, 예외는 상태 설명1줄·보고한 검증 절차5줄뿐.
- TASKS / Archive / README: 상대 파일 링크 대상 존재 확인. Archive 이동에 따른 Validation 링크 대상 유지.
- START_COMMIT 이후 Assets / ProjectSettings / Packages / Validation / PRD / GAME_DESIGN diff 0.
- Unity·게임 테스트·빌드: 미실시. 새 검증 도구 파일0, 삭제 파일0, 외부 push0.
