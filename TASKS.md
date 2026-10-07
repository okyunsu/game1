# 개발 작업과 기록

## Current Sprint

**Sprint 3**, 진입2026-10-03. 수락된 Target15방 + 선택 A06/B06/C05 후보로 실행18방. A06/C05 사람시간·체감 PARTIAL, B06 새 진입/왕복/CP/적초기화·전체R01~R17 기술PASS/REVIEW. PRD/GAME_DESIGN Target18 확정 조건은 아직 미충족.
S3-01~05 DONE, S3-06~10·S2-04·F2R·LOGR·U1 REVIEW 유지. Sprint3 Gate는 사용자 자연 완주·체감 판단 대기, 다음 Sprint 확장 미착수.
F1-R REVIEW: 명중0.05s/Pause·사망·0초 기술PASS 유지, 체감 대기. P03 진단 산출물 DONE, 게임 변경0·원본10/10/보정후9/10, 간헐 실패 이슈 OPEN.
H1 기본값·기존2패턴·H2/H3 보존. B2 후보 실제exe 확장 적용·처치/Ending/타이틀PASS, 패턴별 전체 검증 미완료/PARTIAL.
## Current Tasks

2026-10-08 **Batch04 진행 중**. START_COMMIT `3ae8cc368377f9f8815bfd7ddac083565b813a42`, clean/Unity 닫힘. 07:38 KST 시작, 예상2~4시간/최대8시간. B04-A 사운드·B04-B 형태/애니메이션 DOING, B04-C 돌진 검증 REVIEW(첫 접근+slow 첫 보정 모두 PASS), B04-FINAL 기본/AV1 빌드·회귀 TODO. 후보만 적용/기본값·보호 파일 불변.

2026-10-08 **Batch03 종료 — PARTIAL**. START_COMMIT `c60d938dc7772e7c54b67ae1fe39c365e43f1288`, 시작 clean/Unity 닫힘. 예상4~6시간/예산최대8시간, 실제 약40분. 승인 트랙·Backlog4항목 처리; 패턴 검사 추가 승인 경로만 남음. 후보 수치만 에이전트 결정, 기본 채택·Sprint Gate 사용자 권한 유지.

| ID | 상태 | 완료 조건 |
|---|---|---|
| B03-0 | DONE | 후보 규칙 지정 문단 및 Backlog 등록 |
| BOSS-2/B03-A | PARTIAL | 후보 exe 처치/엔딩/타이틀 PASS; 패턴 검증 자동승인 거부/미실시 항목 [요약](Validation/B03-A-summary.md) |
| B03-B06 | REVIEW | 실제exe 진입/왕복10.217s·CP/적초기화·RouteAll R01~R17 PASS [요약](Validation/B03-B06-summary.md) |
| B03-FINAL | DONE | 기본/B2 실제exe Error0, 기본13/13+RouteAll R01~R17 PASS [통합](Validation/B03-final-summary.md) |
| B03-COMPARE | PARTIAL | 각3회 처치·피격·관찰패턴 표 완료; 돌진 예고 중 처치로 회피율N/A [측정](Validation/B03-compare-summary.md) |
| B03-P03 | DONE | 원본10/10·중립5틱 후보9/10, 입력위상/목표판정 가설·해시 동일, 이슈 미해결 [진단](Validation/B03-P03-summary.md) |
| B03-VARIANTS | REVIEW | fast/slow 실제 수치·처치/Ending/타이틀 PASS/Error0 [후보](Validation/B03-variants-summary.md) |
| B03-ROOMTIME | DONE | 실패2회 포함 A06 16.401s/C05 16.351s, Scene 불변 [측정](Validation/B03-roomtime-summary.md) |

Batch02 종료 이력은 [Archive](Archive/B02-tasks-20261008.md)에 보존한다.

아래 Batch01 및 이전 기록은 당시 상태의 이력이다. 현재 F1-R은 위 REVIEW를 따른다.
2026-10-07 **Batch 01** — F1-R 히트스톱 PARTIAL(동시 Pause/사망 실행 검증 미실시), 문서 최신화 DONE, 확인용 빌드 DONE, R06 입력 교체 DONE/전체 RouteAll PASS·사망0. 기본13 회귀1회 13/13 PASS.
START_COMMIT: 50bb3324f88a6c5b2dc2657e8fe4bcadbf3c5f2a. 기존 게임 규칙·배치·보호 코드 불변. 과거 F1 및 N8/N9/S3T-01 실패 기록은 당시 이력이며 이번 검증과 구분한다.

| ID | Task | Status | Dependency / 실제 결과 |
| --- | --- | --- | --- |
| F1-R / B01-A | 명중 히트스톱만 재개 | PARTIAL | 30/60/120fps 명중·빗나감·중첩, T2v2·보스 PASS. 동시 Pause/사망 미실시 |
| B01-B | Current Sprint·§18 키 표기 최신화 | DONE | 기록된 상태·실제 바인딩만 반영 |
| B01-C | Windows x64 확인 빌드 | DONE | A PARTIAL 기반 허용, 실제 exe 명중·보스3회 재시도→엔딩/Error0 |
| S3T01-R06 / B01-R06 | 정상 귀환 시작 입력 교체 | DONE | 텔레포트 없는 기록, 별도 RouteAll R01~R17/Error0/사망0 |
| B01-REG | 기본13 회귀1회 | DONE | 기존 Run-Regression 기본 부분 재사용, 13/13 PASS |


2026-10-06 **S3T-01 종료 — PARTIAL**: 선택 경로5곳·세션 발견 표시 기술 확인/REVIEW, A-1은 조정 후 제외/복원. R05 연속 귀환 입력 교체 및 재생PASS, R06 기존 시작 위치 가정 불일치BLOCKED(새 배치 원인 아님/수정 승인 밖). 기본13/13PASS, 배포exe PogoTestPASS(최종 묶음의 P03 일회 실패는 보존), 전체 RouteAllFAIL. DIFF-002/PT-002/BOSS-2 기록, 보존 목록 밖 기존 빌드69개 정리. [사용자 QA](Validation/S3T01-user-qa.md), [최종 요약](Validation/S3T01-final-summary.txt). **플레이 전 확인 필요**, 전체 완주 검증 빌드로 안내하지 않음.

2026-10-06 **N9 종료 — PARTIAL**: ATK2/3/4·PogoTest 기술 확인/REVIEW, S5-hard 최종 exe 지정 동작·후보3개 보스 재시도PASS. 최종 회귀14/15(기본13+PogoPASS, RouteR05FAIL). 이전 N8 경로 목표 오판을 발견해 완주 판단 철회/경로BLOCKED, Editor Pogo 재생BLOCKED. 사용자 감각·후보 선택 대기. [최종 인계](Validation/N9-final-handoff.md).

2026-10-06 **N8 종료 — PARTIAL / 경로 후속 BLOCKED**: N9 CSV 감사로 R05 사망을 놓친 이전 연속 완주 판단은 무효이며, 경로 Task를 BLOCKED로 되돌림. 아래 PASS 표기는 당시 검사 출력의 이력이다. 상향 공격 REVIEW, 포고·PogoTest·S5-hard BLOCKED(보호된 사망 처리 연결부 변경 필요). 동일 기록의 실제 exe 전체 완주 60fps 3회·30/120fps·패드 PASS, 기본 회귀13개와 추가 RouteAll-60fps PASS, 사망 화면8장 검증. N7 soak 기존 증거 검토 완료. [최종 인계](Validation/N8-final-handoff.md).

2026-10-05 **N7 PARTIAL / 공격 입력 범위 답변 대기**: ATK1 회귀13/13·실검사FAIL(input Asset keyboard y absent), 공격 실험 원복; Hard 후보 회귀13/13·보스 처치PASS, V2/V3 PASS, V4 촬영KNOWN-LIMIT, V5 원문/공격 의존BLOCKED, V1 실제120분 진행 중. [현재 인계](Validation/N7-current-handoff.txt).

2026-10-05 **N6 중단 — 사용자 결정**: 초기 작업 트리는 깨끗하지만 HEAD가 N5 인계 커밋 baaaae0와 동일하여 첨부 §0의 이후 조건을 엄격히 충족하지 못함. 사용자가 시작 조건 유지·중단을 선택함. 구현·Unity 검사·120분 soak 미실시. [검토 기록](Validation/N6-preflight-review.txt).

2026-10-04 **N5 최신: 1부 PASS/S2-04 REVIEW, T-B/T-C PASS, T-F 57유효PNG, T-A/T-D/T-E/T-G 일부 KNOWN-LIMIT. S4-pre3 빌드 PASS, 단계별회귀104/104. 자연 전체 완주/60분 안정성 미검증. Latest Handoff 우선.**

상태: TODO(미착수), DOING(구현 또는 기술 검증 진행), PARTIAL(일부 완료 조건이 남았지만 후속 개발에 사용 가능), REVIEW(기술 구현·관련 검증 완료, 사용자 감각·설계 판단 대기), DONE(완료 조건·필요한 사용자 수락 충족), BLOCKED(외부 의존성·사용자 결정·범위 변경 필요), DEFERRED(의도적으로 미룸).
의존성 `:REVIEW`는 유효한 기술 검증을 갖춘 REVIEW 또는 DONE에서 충족한다. `:DONE`은 DONE만 허용하며 생략 시에도 DONE으로 해석한다. DEFERRED는 의존성을 자동 충족하지 않는다. 실행에는 별도 범위 승인이 필요하다.

| ID | Task | Status | Dependency | User Review | Done Criteria |
| --- | --- | --- | --- | --- | --- |
| S1-07-T | Tilemap 도입 | DEFERRED | 별도 사용자 결정 | Editor GUI 확인 | DEC-ROOM-B: 이번 Sprint 제외, 사용자 GUI 확인 후 별도 Task 결정 |

### 야간 시스템 작업 — DEC-S2-PRE

| ID | Task | Status | Dependency | User Review | Done Criteria |
| --- | --- | --- | --- | --- | --- |
### Sprint 2 승인 작업 — 2026-10-02

| ID | Task | Status | Dependency | User Review / Done Criteria |
| --- | --- | --- | --- | --- |
| S2-04 | 단일 슬롯 저장·Menu 이어하기 | REVIEW | S2-02:REVIEW | N5 Part1: 예외 필터 수정, 손상 4종 실제 exe/기존 저장 검사/회귀13/13 PASS. 사용자 수락 대기 |

### Sprint 3 승인 작업 — 2026-10-03 (P3 및 G3 후속)


| ID | Task | Status | Dependency | User Review / Done Criteria |
| --- | --- | --- | --- | --- |
| S3-06 | B02 E2 소개 위치 수정 | REVIEW | G3:DONE | E2 x17/왼쪽/바닥, 발판 위 감지4.787u·반복 돌진 벽앞0.140u 정지·피해 없음·바닥 피해·B01/B02/B03 왕복 및 변경 위험에 비례한 최소 검증(AGENTS.md Validation) |
| S3-07 | B05 연결·C01 휴식·C02 조합 | REVIEW | G3:DONE | 지정 배치·CP-C01 저장/사망·왕복·바닥 더블 최대발5.905<6.8·Start 일반5.535<6.8/더블 착지·5u 간격 일반실패/대시성공·스폰 비겹침·변경 위험에 비례한 최소 검증(AGENTS.md Validation) |
| S3-08 | C03 종합 이동·C04 휴식 | REVIEW | S3-07:REVIEW | 지정 E1/E3/엄폐·CP-C04 저장/사망·C02/C03/C04 왕복·CP→C06 입구1.280s·변경 위험에 비례한 최소 검증(AGENTS.md Validation), Right는 C06 생성까지 비활성 |
| F1 | 최소 공격 타격 피드백(FX-001) | BLOCKED | 사용자 승인 | v2 첫120fps 명중FAIL→검사1회보정→효과PASS, 필수T2 .12s 넉백잠금FAIL. 전체복원, 실험빌드 사용자 판단 대기 |
| S3-09 | C06 최종 보스 | REVIEW | S3-08:REVIEW | v2 작성 실패 1회 보정 후 시야/회피/회복 X명중/20타/사망/동시프레임/Pause/문 왕복과 변경 위험에 비례한 최소 검증(AGENTS.md Validation). 사용자 체감 대기. F1 미적용 |
| S3-10 | 엔딩·저장·타이틀 흐름 | REVIEW | S3-09:REVIEW | v2 실제 exe 처치즉시저장/2초빛/엔딩/타이틀/즉시종료후Continue/새게임초기화/가상패드 UI와 변경 위험에 비례한 최소 검증(AGENTS.md Validation). 엔딩 표현 사용자 판단 대기 |
| S3-G | Sprint 3 Gate | TODO | 사용자 판단 | Content Complete 미충족, 다음 Sprint 미착수 |

새 방 공통 셸 36×14u/바닥윗면y1/좌우벽/경계/카메라5.5·0·0.15/방이름/Door 표시/트리거1×2/바닥스폰y1.81. B02~B04 왼쪽출구(1.75,2), 양쪽 있는 방 오른쪽(34.25,2), FromLeft(3.5,1.81)/FromRight(32.5,1.81). G-J 높이는 일반최대+0.8以上/더블최대−0.5以下 측정 후 조건 불충족 시 범위 안 가장 가까운 값만 적용·결정 기록. S3-05의 B03 연결은 S3-04 실패 시 활성화하지 않고 독립 구현 가능한 부분만 보수적으로 처리한다.


### 후속 후보와 기록 위치

- **BOSS-2 PARTIAL** — B2 후보 실제exe 처치 PASS, 추가 패턴 검증 자동승인 거부. 기본2패턴·H1값 유지, 후보 채택은 사용자 판단.
- U1 / F2R / LOGR: REVIEW, 기존 사용자 체감 수락 대기. [기술 결과](Validation/TM-U1-final-handoff.md).
- CAM-002 / K2: DEFERRED, 아트 적용 후 재확인. 기존 0.15s 유지.
- 완료 Task 의존성 및 관찰·결정·리소스 원문: [Archive](Archive/TASKS_ARCHIVE.md). DONE 의존성은 Archive의 완료 행을 함께 확인한다.
## Bugs

### B03-P03-BOUNDARY — 기록 소진/목표 판정 경계 / OPEN
원본 독립P03 10/10, 중립5틱 후보9/10. 실패는 점프 소비1틱 차이·대시 높이 차이 이후 착지 직전 입력 종료; 종료 뒤 goal 로그. 입력 보정1회 사용/추가 수정 없음. 보호된 게임 변경0. [진단](Validation/B03-P03-summary.md).

### B03-BOSS-VALIDATION — 패턴별 검증 / PARTIAL
자동 승인 검토가 패턴 fixture 4번째 실행을 접근3개 소진으로 거부. 30/60/120fps 지정 회피/실패1회·Pause/사망/동시프레임 최종 버전 미검증. 실제 후보 처치와 기본 회귀는 독립 PASS; 추가 검증 승인 필요. [상세](Validation/B03-A-summary.md).

### N9-ROUTE-001 — R05 사망 및 이전 완주 판정 무효 / BLOCKED
기존 FixedUpdate 관찰이 TimeScale0 사망을 놓치고 CP-A03 복귀를 R05 목표로 인정했다. N8 6회 및 N9 초기60fps 3회 CSV의 실제 A04 사망 확인으로 의도한 R05 출구·G-D 통과를 포함한 연속 완주 판정을 철회한다. 새 프레임 관찰과 입력 후보3개도 R05에서 사망; 도구 보정3회 한도 도달로 경로 수정 중단. 게임·보호 파일 추가 수정 없이 후보 비교는 독립 진행. Editor P01 고정 입력 재생도 목표 소진으로 BLOCKED(실제 exe Pogo3연속PASS와 구분). [상세](Validation/N9-route-recheck.txt).


### D0-T2-001 / D0-T4-001 / D0-S2-001 — 기준 검사 실패
실제 기준 상태(표시 미적용)에서 T2 잠금 assertion FAIL, T4 처치된 Rigidbody 참조FAIL, S2 기존통합 HP persists in A04 FAIL 재현. T2v2는 CP x5.5 인접 x6 검사 간섭을 피한30/60/120fps 직접측정PASS. 이전 F1/F2가 원인이라는 결론의 근거로 기존 T2 assertion을 쓰지 않음(F1 독립 영향이 모두 해명된 것은 아님). F2R의 같은 기준 결과는 새 명세대로 회귀 아님. [D0 요약](Validation/D0-summary.txt), [추가 S2 기준](Validation/D0-S2-integration.txt), [F2R 결과·도구한계](Validation/F2R-summary.txt).

### F2-T2-001 — 필수 넉백 잠금 검사 재실패 / 전체복원 (2026-10-04)
- F2 초기 더미 표시 바인딩 실패를 새 표시 스크립트에서 1회 보정한 뒤, 30/60/120fps 좌우 표시·판정 일치·명중/빗나감·번쩍임·E1 처치효과 검사 PASS.
- T2 기존 검사에서 물리 피격→여러 프레임 입력/FixedUpdate 대기 후 `KnockbackLocked` FAIL. 검사만 60fps로 고정해도 동일 FAIL. 원인이 검사 대기인지 실제 동작인지 미확정, 명세의 애매하면 A 규칙으로 전체복원. 입력·이동·피격·GameState 및 기존 수치는 변경하지 않았음.
- F2 Asset/새 런타임·작성·검사 코드 삭제, 7개 프리팹/표시 호출/기존 검사 도구 원복. 성공한 표시 검사만으로 REVIEW 판정하지 않음. T1/T3/T4/S2-01/보스회복·E2 경고중 표시는 실패 중단으로 미실시.
- 증거: [표시 첫실패](Validation/F2-feedback.txt), [표시 보정PASS](Validation/F2-feedback-night-F2-rerun.txt), [T2 첫실패](Validation/Verification-night-T2-F2.txt), [T2 재실패](Validation/Verification-night-T2-F2-rerun.txt).

### ROOM-001

- ID / 관련 Task / Status / 심각도 / 환경: ROOM-001 / S1-07 / OPEN / Major / Unity 6000.3.24f1 Editor 배치 Play.
- 재현: 작성한 A01에서 오른쪽 출입구로 A02 진입 후 접지 검사. 전환·동일 플레이어 유지·이전 방 정리는 통과했으나 바닥 Collider bounds가 0이고 플레이어가 낙하했다.
- 기대·실제: Tilemap 지면에 안전하게 착지해야 하나, 저장된 씬의 셀이 비어 있었다. Tile Asset 로드는 정상인데 재지정·저장 진단에서도 GetUsedTilesCount=0이 반복됐다. 근본 원인은 미확정.
- 처리: AGENTS Stop Conditions의 동일 오류 해결 반복 조건으로 중단. S1-07 코드·씬·패키지/설정 변경은 검증된 HEAD로 되돌렸으며, 실패 작업 사본과 patch는 로컬 `Logs/S1-07-blocked-work/`에 보존했다. 기존 이동·Camera 구현과 사용자 변경은 보존했다.
- 재검증: S1-07 실패. 전체 왕복·잘못된 ID·입력 누수 및 S1-02/04 통합 회귀는 완료하지 못했다. DONE으로 처리하지 않음. 수치·규칙 변경 없음.
- 2026-09-30 재개 승인 후 최소 재현: H1 시작 시 module 없음(추가 후 manifest/lock PASS), H2 CreateAsset·저장·저장된 Tile 재로드 PASS, H3 Grid 부모 PASS. H4 SetDirty·MarkSceneDirty·SaveScene 호출/저장은 성공했으나 재개방 GetUsedTilesCount=0·셀 3개 모두 없음으로 FAIL. 저장 이전에도 count=0, cell0 없음. 원인 미확정이며 추가 조사 중단. [최소 재현 결과](Validation/ROOM-001-minimal.txt).
- 최소 재현 실패 씬 `Assets/Scenes/RoomMinimalTemp.unity`와 단일 Tile `Assets/RoomMinimalTemp.asset`을 증거로 보존(빌드 씬 미등록). 실행용 임시 스크립트는 삭제, 로컬 `Logs/ROOM-001-minimal-repro.cs`·실행 로그 보존. S1-07 patch는 복원하지 않음.
- 2026-10-01 H5 재확인: 현재 61e02db에서 시작 전 Library/PackageCache 및 ProjectCache/projectResolution에 tilemap 1.0.0 반영 확인, 실행 중 Unity 없음. 새 Unity 6000.3.24f1 프로세스 검사에서 SetTile 직후 0셀·저장 재개방 후 0셀. H5 FAIL(성공 조건 미충족). 단, CreateAsset/SaveAssets 후 저장된 Tile 재로드도 실패(savedTile=False)하여 올바른 Tile을 사용한다는 선행 조건 미충족. 같은 세션 추가 가설을 독립적으로 반증한 결과는 아니며 원인 미확정이다. 요청된 0셀 중단 조건에 따라 추가 조사·S1-07 복원 미실행. [H5 결과](Validation/ROOM-001-h5.txt).
- DEC-ROOM-B(2026-10-01 사용자 승인): 원인 미확정, 배치 모드 Tile 에셋 생성 실패(savedTile=False), 대안 B로 우회 승인. ROOM-001 OPEN 유지, Tilemap은 이번 Sprint DEFERRED. 사용자 Editor GUI 확인 후 별도 Task로 도입 결정. 로컬 patch로 기존 전환·GameState·입력 초기화 구조 복원, 지형만 블록으로 대체. RoomMinimalTemp 씬/Tile(.meta 포함) 삭제, 실패 Validation 보존, tilemap 모듈 유지.
- 대안 B 구현 검사에서 비활성 Rigidbody 위치만 지정할 때 출입구 연속 진입·방 Start의 Entry 재배치 문제가 관찰됨. Spawn 배치 시 Transform도 함께 이동하고 최초 진입에서만 EnterInitial 호출하도록 수정. 수치·연결 변경 없음. 최초 실패와 중간 통과·최종 검사 증거를 분리 보존.
- 대안 B 완료: [최종 Room 검사](Validation/S1-07-N2-A-blocks-final.txt) 71건, [S1-02 회귀](Validation/S1-02-N2-A-blocks.txt) 19건, [S1-04 회귀](Validation/S1-04-N2-A-blocks.txt) 63건 통과. S1-07 DONE이며 ROOM-001은 Tile 에셋 문제 미해결로 OPEN 유지. [최초 블록 검사 실패](Validation/S1-07-blocks-first-attempt.txt), [Spawn 검사 보강 전 통과](Validation/S1-07-N2-A-blocks.txt)도 보존.
- 증거: [실패 결과](Validation/S1-07-N2-A-blocked.txt). 로컬 `Logs/S1-07-tile-diagnostic.log`, `Logs/S1-07-persist-tiles.log`.

### RESET-001 — 첫 재설정 실패 (3d84246)

Sliced + BoxCollider2D.autoTiling에서 SpriteRenderer 크기 변경 후 콜라이더 불일치. 최초 API 컴파일 오류 수정 후 두 번째 검증 실패로 변경 복원. 이번 T0는 Simple·1×1u·Transform Scale 방식 사용. 증거: Validation/S1-09-reset-first-failure.txt, S1-09-reset-prefabs.txt, S1-09-reset-stopped.txt.

### N5 검증 도구 한계 (2026-10-04; 제품 버그 판정 없음)

- T-A: checkpoint 쓰기 실패 경로 2회 실패. A01 x8 목표 앞 지형에 걸림; 권한은 복원. folder ACL/복구 후 다음 쓰기는 미검증. Ending Continue 반복은 PASS.
- T-D: G-D 좌표 보정에도 시작 높이1.81이 선반 높이5보다 낮아 유효하지 않음. 나머지32칸 결과는 4fps에서 일관됨. 제품 게이트 우회 BUG로 해석하지 않는다.
- T-E: Pause 고정틱 대기 보정 후 A04(없음)의 KillZone 선택으로 재실패. 60분 안정성·메모리/오브젝트·행동 수 기준은 미검증. T-E 재실패 이후 도구를 추가 수정하지 않음.
- T-F: 화면56조합/D3D12 probe/PNG57장 PASS; 포커스 강제 상실·복귀 미검증. 기본 focus 설정을 보존했고 IgnoreFocus로 대신하지 않음.
- T-G: 자연 입력 경로 1회 보정 후 B01 상단 B05 대신 하단 B02로 진입. 두 능력 자연 획득까지 확인; 새 게임→엔딩 자동 완주 미검증.
- 추가 도구 보정은 이번 큐의 1회 보정 한도를 넘으므로 후속 승인 범위에서 판단한다. 사용자 전체 플레이·감각 수락과 Sprint3 Gate는 계속 대기한다.

- UI-CAP-001 (2026-10-04): 새 런타임 캡처 도구 ScreenCapture/EncodeToPNG 모듈 참조 불가. 검사1회보정 재실패로 촬영 경로 중단, 컴파일을 막는 부분만 제거. 기존 Packages 보호. U1 표시 상태검사는 독립 PASS; 1280×720/1920×1080 6장 및 시각 검수 미실시. 다음 판단: 사용자 exe 가독성 확인 또는 별도 승인 촬영 도구 경로.

## Decisions Needed

B02의 보스 수치·B06 진입 범위 결정은 BATCH-03 사용자 승인으로 해소. [원문 이력](Archive/B03-resolved-decisions.md).

### B01-HITSTOP — 기술 미검증 해소 / 체감 REVIEW
B02-C에서 히트스톱 중 Pause·치명피격 사망·0초를 기존 진입점으로 실행PASS/Error0, 0.6초 복귀실측0.616894s. 0.05s 유지/조정/끄기 사용자 체감만 대기. [결과](Validation/B02-C-summary.md), [해소 전 이력](Archive/B01-latest-handoff-20261007.md).
### S3T01-POGO — P03 고정 입력 간헐 실패 / REVIEW
B02 승인반복5회 중 P03 4PASS/1FAIL(Error0), 안정적반복통과 미확정, 입력/게임 수정없음. [최신](Validation/B02-pogo5-summary.md).
기존 묶음 첫 실행 PogoPASS, 최종 묶음 P03FAIL/Error0, 같은 배포exe의 기존 검사 재실행은 P01/P02/P03·사망복귀·능력 회복 PASS/Error0이다. PogoTest 씬·공격/포고 수치는 불변. 원인 미확정이며 안정적 반복 통과로 확정하지 않았다. 새 도구·게임 복원·Pogo 입력 수정 없이 증거를 보존한다. [최종 묶음 실패](Validation/PogoTest-60fps-S3T01-final.txt), [배포 재실행](Validation/S3T01-pogo-delivery.txt).

### S3T01-LEVEL — 승인으로 해결된 범위 충돌 / A-1 제외
DEC-LEVEL-001의 사용자 승인으로 GAME_DESIGN의 PogoTarget 전용 씬 제한과 수집 표시 금지 문구를 이번 선택 경로·세션 발견 표시 범위에 한해 갱신했다. 진행·성장·저장 보상은 계속 제외한다. A-1은 추가 선반의 단방향 처리와 두 진입 입력 보정 후에도 지정 입력이 실패해 승인된 제외 규칙을 적용했다. 실제 접근성/게임 결함으로 확정하지 않음. 기존 A04는 복원했고 이번 범위에서 재시도하지 않는다. 다시 적용하려면 별도 사용자 판단이 필요하다. [실패](Validation/S3T01-A-1-shelf-a3.txt).

### N9-ROUTE-REPLAY — 추가 경로 입력 탐색 범위 / BLOCKED
프레임 관찰이 R05 실제 사망을 발견해 기존 경로 검증이 무효. 검사 도구3회 보정과 동일 사망 반복 한도로 승인 경로 중단; 추가 자동 입력 탐색은 다음 요청에서 범위를 정한다. 보호된 게임 파일·기존 배치 변경 없이 조사했으며, 현재 후보 난이도 비교에는 고정 원본 입력을 사용한다. Editor Pogo 재생의 추가 보정도 같은 한도에 따라 보류. [실패 증거](Validation/N9-route-recheck.txt).


### 2026-10-04 Q-night4 후속 판단
- Q1-SAVE-001 N5 Part1에서 수정·재검증 완료. S2-04 REVIEW, 사용자 수락 대기. T-A 쓰기 실패 확장 검증은 도구 한계로 별도 미검증.
- Q6 A/B/C를 각각2분 비교한 후 선호 후보 결정. 현재 원본 기본값은 미변경이며 후보 선택과 원본 적용은 명시 결정으로 구분함.
- Q2 전체 디스플레이/포커스·Q3 144fps 및 전체 구간·Q4 연속 입력 경로·Q5 실제45분 반복/내부 수집은 기존 도구로 미완료. 검증 도구 확장이 필요하면 별도 범위를 정하고 승인할 것. 출구 직접 배치 시간/유휴 메모리를 요청 측정의 대체 PASS로 쓰지 않음.
- Q7은 한 번의 보정에서도 검정 화면으로 실패해 추가 촬영 시도를 중단. 유효 스크린샷 없음. U1 가독성/완주·Sprint3 Gate는 사용자 판단 대기 유지.


### LOG-001 — 정확한 사망 원인 기록과 구독 전용 제한
LOG BLOCKED/미구현. RoomSession 진행 이벤트 없음, PlayerHealth.Hit은 원인·source 없는 Action이며 KillZone도 직접 Die만 호출함. 근처 적/접촉체 추정은 동시 피격·무적 상황에서 실제 사망 원인을 보장하지 못함. 기존 로직은 그대로 유지함. 필요 판단: 관찰 전용 이벤트 추가를 허용할지, 또는 사망 원인 unknown 및 일부 기록 보장 축소를 허용할지. [인터페이스 확인 및 미실시 내역](Validation/LOG-playtest.txt).

현재 v2 판단: F1 필수 T2 넉백잠금0.12s 검사 실패 원인·공격 손맛/피드백 후보 수락. 효과 이벤트 30/60/120fps는 PASS이나 단계는 전체복원/BLOCKED다. Builds/Experimental-F1/Afterglow.exe 사용자 직접 플레이 후 재개 범위를 결정한다. 보스 좁은방 시야/회복 공격 이전 차단은 v2로 해결. 보스·엔딩 기술PASS는 체감/Gate 수락을 뜻하지 않으며 사용자 완주 후 Sprint3 Gate 판단. 선택방/에셋/K2/다음Sprint 미승인.

2026-10-03 v2 F1: 효과 검사 첫120fps 명중FAIL → 방향 입력을 FixedUpdate까지 유지하도록 1회 보정 → 30/60/120fps 효과 이벤트 PASS. 필수 T2 회귀 knockback lock0.12s FAIL로 단계 실패/전체 복원. 추가 재시도 없음. 실험 Builds/Experimental-F1/Afterglow.exe 유지(기동 Error/Exception/Assert0), 사용자가 공격 손맛을 직접 판단. S1-02/04/07/08/N2카메라/T1 PASS, T3/T4/S2-01 미실시. 독립 S3-09 진행; 보스 F1 미적용. 증거 F1-feedback-v2.txt, F1-feedback-v2-rerun.txt, Verification-night-T2-F1-v2.txt, Experimental-F1-v2-build/runtime.txt.

CAM-002 / K2 **DEFERRED** — 사용자 2026-10-03: 아트 적용 후 재확인, 우선순위 낮음. Sprint 4 Polish 후보. 기존 0.15s 유지, 이번 재시도 없음. 이전 두 번째 비교 실패는 절대 좌표 비교 영향 추정; 상대 카메라-플레이어 거리 2.06u/2.08u. 재시도는 상대 거리로 비교.

이전 K2 / CAM-002 기술 실패 기록(현재 DEFERRED): 두 번째 검증에서 평상시·반전 비교 FAIL로 승인된 실패 규칙에 따라 전체 복원. 카메라 기본0.15s 유지. 시간 기반 입력의 물리 틱 차이 가능성은 미확정이며 추가 수정/재시도는 별도 승인 필요. 대시 밀림 체감은 미해결.


## Approved Backlog

B04 승인 Backlog: 1. AV1 음량 표/6dB 편차 조정 2. 명도 대비 표/후보색 조정 3. 1920×1080 지정9방 촬영(검정 보정1회) 4. 1페이지 QA 순서. 트랙/최종 뒤 순서대로 진행.

## Latest Handoff

2026-10-08 **Batch03 PARTIAL 종료**. 예상4~6시간/예산8시간, 실제 약40분. START_COMMIT `c60d938dc7772e7c54b67ae1fe39c365e43f1288`; 시작clean/Unity닫힘, Task별 로컬커밋/push없음. 새 플레이 범위: B06 선택 진입 복원, B2·fast·slow 보스 후보. 기본 소스 Unity회귀13/13·18방 exe R01~R17 전부PASS/Error0. 후보3종 실제exe 처치→Ending→타이틀PASS/Error0. **B2 전체 패턴 기술 검증은 미완료**, 사람 수락/Sprint Gate 아님.

### 결정 질문지

1. **B2 남은 검증**: 새 Batch에서 재시도 한도를 새로 정해 패턴 검사 보정·재검증 / 현재 PARTIAL 유지·직접 플레이만 / 후보 폐기. 추천 첫안: 빌드·처치는 통과했지만 최종30/60/120fps 지정회피/실패1회·Pause/사망/동시프레임 확인이 남았다. [AGENTS Batch Approval](AGENTS.md#batch-approval)의 `Task당 서로 다른 접근은 최대 3개까지 허용한다`를 근거로 자동 승인 검토가 4번째 pattern fixture 실행을 거부했으며 우회하지 않았다. 답이 BOSS-2 기술 검증·돌진 비교 수치 경로를 푼다.
2. **보스 후보 선택**: 검증 후 B2 / B2-fast / B2-slow 채택, 수치 조정, 폐기. 추천 B2를 기준으로 먼저 직접 비교: 자동포고 전략에서는 B2가 더 빨리 처치됐고 fast/slow의 사람 난이도는 미확인. 기본 채택은 이번에 하지 않았다. 답이 후보 수락/후속 조정 Task를 푼다.
3. **B06·A06·C05**: 방별 유지 / 의미 있는 도전을 더해 수정. 추천 B06의 의도적 Up 입구는 유지(필수경로 간섭 해소), 방 분량·난이도는 직접 플레이 후 판단. A06/C05는 의도적 실패2회를 섞어도 자동16.4s 수준이므로 사람1~3분을 충족하는지 확인해야 한다. 시간 채우기 반복 배치는 하지 않았다. 답이 선택방 시간·체감 수락 또는 새 배치 Task를 푼다.
4. **히트스톱 .05s**: 유지 / 조정 / 끄기. 추천 우선 유지: 이전 기술검증은 PASS, 손맛은 직접 판단. 답이 F1-R REVIEW 수락/조정 Task를 푼다.

### 직접 플레이 확인 (수락 대기 순서)

QA `.cmd`를 Explorer에서 더블클릭하면 매회 **QA 전용 저장 슬롯을 초기화**한다. Menu에서 Z 이어하기. 같은 폴더 Data/DLL 유지. 표준 exe 실행도 가능하다.

1. **히트스톱 5분** — 기본 [QA-C06](Builds/B03-main-final/QA-C06.cmd) 또는 A04. 공격 명중 .05s 정지가 손맛에 맞는지. 원본 FeedbackTuning.asset의 hitStopSeconds(s); Play 종료 후 값을 다시 반영·저장·재빌드.
2. **A06·C05 합10분** — 기본 [QA-A06](Builds/B03-main-final/QA-A06.cmd): CP-A03→오른쪽 A04, 왼쪽 출구 위 문(x2,y5.6)에 점프 후 왼쪽 이동. [QA-C05](Builds/B03-main-final/QA-C05.cmd): CP-C04→왼쪽 C03, 동일 위 문. 발견 접촉·귀환·도전성·사람1~3분 확인. A06은 Dash/DJ 없음, C05 둘 다 소유. [시간 보조](Validation/B03-roomtime-summary.md).
3. **B06 5분** — 기본 [QA-B06](Builds/B03-main-final/QA-B06.cmd): CP-B01→오른쪽 B02→B03, x7 청록색 `↑ 선택 대시 도전` 앞에서 ↑/스틱 위. Dash·포고로 발견→같은 입구 귀환, 선택 진입이 명확한지/필수 이동을 가로채지 않는지. DJ 없음. 자동왕복10.217s/CP·E3초기화PASS, 사람 시간·체감 대기.
4. **B2 C06 10분** — [QA-C06 B2](Builds/B03-B2-final/QA-C06.cmd): CP-C04에서 오른쪽 보스. 고정 근접→도약→파동→돌진, 먼 위치 걷기, 예고 가독성/대시·점프 회피/접촉 피해/정상 재시도 확인. 최종 패턴별 기술검증은 아직 PARTIAL이므로 직접 확인과 구분. [상세](Validation/B03-A-summary.md).
5. **fast/slow 비교 10분** — [fast](Builds/B03-B2-fast-final/QA-C06.cmd), [slow](Builds/B03-B2-slow/QA-C06.cmd). 빠름/느림 예고·이동 조합 중 원하는 압박/학습감을 비교. [실제 수치](Validation/B03-variants-summary.md).

발견은 세션한정/저장안함, HUD분모9·현재최대8/9(A-1 제외). 기본은 기존 보스2패턴·기본튜닝 유지. 배치 원본은 Scene(B03 새 OptionalRoomEntry/B06 내부), 후보 수치 원본은 Assets/BossCandidates/B2 또는 B2-fast/slow/BossTuning.asset. Inspector Tooltip 단위를 따른다. Play에서 바꾼 값은 Stop 후 해당 Scene/Asset에 원하는 값을 다시 반영·저장하고 재빌드한다.

### Task별 결과

| Task | 상태 / 커밋 | 주요 파일·실제 검증·문제 |
|---|---|---|
| B03-0 | DONE / 53ae9d1 | AGENTS 지정 문단만·TASKS Backlog4항목 등록; 문서diff확인 [증거](Validation/B03-0-summary.md) |
| BOSS-2 | PARTIAL / e426445 | BossTuning/Encounter/Wave, B2 Asset, build/verifier. 실제 후보exe20.468s/피격0·엔딩/타이틀PASS. 패턴 fixture 실패/자동승인 거부, 최종30/60/120·Pause/사망/동시프레임 미실시 [증거](Validation/B03-A-summary.md) |
| B06 | REVIEW / 3a32d07 | 새 OptionalRoomEntry·B03 추가문·B06 Scene, 실제입력왕복10.217s·CP-B01/E3초기화·전체R01~R17PASS. 초기 disabled RoomExit 간섭 수정, 사람수락대기 [증거](Validation/B03-B06-summary.md) |
| 마무리 | DONE / 9e9dc21 | 기본/B2 최종빌드21씬·errors/warnings0, 기본13/13·RouteAllPASS; README/이력Archive. 캐시 탓에 첫 후보에 기본보스가 들어간 이력 제외, CleanBuildCache+실제flag확인 [증거](Validation/B03-final-summary.md) |
| 비교3회 | PARTIAL / 7860a2d | verifier 지정입력 각각3처치; 평균기본21.661s/B2 20.417s(게임시간), 유효피격0. 돌진 예고 중 처치로 회피율N/A, 최초 B2 입력사망 로그 보존 [표](Validation/B03-compare-summary.md) |
| 변형2개 | REVIEW / f49613e | 새 fast/slow Asset, 실제exe수치·처치/Ending/타이틀PASS/Error0. fast22.301s/피격1, slow20.417s/피격1; slow 첫 사망 후 첫재시도PASS [표](Validation/B03-variants-summary.md) |
| P03 진단 | DONE / 89a8e9f | 새 diagnostic/입력후보. 원본10/10·중립5틱 후보9/10, 실패입력·좌표·속도 기록/가설1개, 게임해시동일. 안정성 해소 아님 [증거](Validation/B03-P03-summary.md) |
| 방 시간보조 | DONE / d66f866 | 새 verifier, 각 첫실패2회 포함 A06 16.401s/C05 16.351s, 실제exe/Error0. Scene 변경0·사람시간 미확인 [표](Validation/B03-roomtime-summary.md) |

**에이전트가 정한 후보값/이유**: 벽여유 .1u와 표적 clamp(벽 관통 방지), 표적/방향은 예고 시작 시 snapshot(예고 후 추적 없음). 이동 보스는 후보에서 trigger+접촉 겹침 판정(정적 collider 이동의 바닥 밀림 방지), 도약 비행 접촉 제외/착지 피해. B06 입구(7,2)·청록색·Up 선택(기존 물체 이동/필수 입력 간섭 없이 의도적 진입). fast/slow에서 파동·걷기·돌진·도약 이동까지 ±20%; 도약비행 .5/.75s(같은 높이·속도 배율), 모든 예고 ∓20%. 기본 수치 채택0.

START_COMMIT 대비 게임 변경: BossEncounter.cs/BossTuning.cs/BossWave.cs, 새 BossCandidates B2/fast/slow Asset/.meta, B03 Scene(새 문/표시만), 새 B06 Scene/.meta, 새 OptionalRoomEntry.cs/.meta. 새 Verification은 B03BossVerifier/RoomVerifier/P03Diagnostic/RoomTime, Data 입력 후보, Editor B03Build/B03Room(.meta 포함). **기본 튜닝·기존Prefab·모든 금지 Scene/Player core/GameState/KillZone·Input·Packages/ProjectSettings·PRD/GAME_DESIGN diff0**. B03 기존 오브젝트 삭제/이동0, B06 내부는 B02 raw 원본과 동일. 18방 전부수락 조건 미충족이라 Target15 문구 미변경.

원본 로그/실패/각틱trace/CSV/해시/빌드증거는 Validation/_raw/B03/에 보존(미커밋), Task당 요약1개 커밋. 기존 raw 삭제/덮어쓰기0. 표준 저장 대신 검사별save-path 사용, QA는별도슬롯. 이전 인계는 [Archive](Archive/B02-latest-handoff-20261008.md).

### 다음 Batch 후보

1. 별도 승인으로 B2 남은 패턴검증·Pause/사망/동시프레임과 돌진 비교 보정, 후보3종 체감 선택.
2. B06/A06/C05 자연 플레이 후 유지/의미 있는 도전 추가 범위 결정, 히트스톱 수락.
3. P03 기록 종료/착지 판정 경계의 검사 방식 또는 더 긴 중립 대기 승인 검토(이번 보정1회 한도 소진, 게임 변경은 별도 승인).
4. 자연 전체 완주 후 Sprint3 Gate 사용자 판단. 다음 Sprint 대규모 콘텐츠 확장 미착수.
