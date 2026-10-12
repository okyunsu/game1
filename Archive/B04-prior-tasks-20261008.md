Batch02 종료 이력은 [Archive](../Archive/B02-tasks-20261008.md)에 보존한다.

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


2026-10-06 **S3T-01 종료 — PARTIAL**: 선택 경로5곳·세션 발견 표시 기술 확인/REVIEW, A-1은 조정 후 제외/복원. R05 연속 귀환 입력 교체 및 재생PASS, R06 기존 시작 위치 가정 불일치BLOCKED(새 배치 원인 아님/수정 승인 밖). 기본13/13PASS, 배포exe PogoTestPASS(최종 묶음의 P03 일회 실패는 보존), 전체 RouteAllFAIL. DIFF-002/PT-002/BOSS-2 기록, 보존 목록 밖 기존 빌드69개 정리. [사용자 QA](validation-2026-10/S3T01-user-qa.md), [최종 요약](validation-2026-10/S3T01-final-summary.txt). **플레이 전 확인 필요**, 전체 완주 검증 빌드로 안내하지 않음.

2026-10-06 **N9 종료 — PARTIAL**: ATK2/3/4·PogoTest 기술 확인/REVIEW, S5-hard 최종 exe 지정 동작·후보3개 보스 재시도PASS. 최종 회귀14/15(기본13+PogoPASS, RouteR05FAIL). 이전 N8 경로 목표 오판을 발견해 완주 판단 철회/경로BLOCKED, Editor Pogo 재생BLOCKED. 사용자 감각·후보 선택 대기. [최종 인계](validation-2026-10/N9-final-handoff.md).

2026-10-06 **N8 종료 — PARTIAL / 경로 후속 BLOCKED**: N9 CSV 감사로 R05 사망을 놓친 이전 연속 완주 판단은 무효이며, 경로 Task를 BLOCKED로 되돌림. 아래 PASS 표기는 당시 검사 출력의 이력이다. 상향 공격 REVIEW, 포고·PogoTest·S5-hard BLOCKED(보호된 사망 처리 연결부 변경 필요). 동일 기록의 실제 exe 전체 완주 60fps 3회·30/120fps·패드 PASS, 기본 회귀13개와 추가 RouteAll-60fps PASS, 사망 화면8장 검증. N7 soak 기존 증거 검토 완료. [최종 인계](../Validation/N8-final-handoff.md).

2026-10-05 **N7 PARTIAL / 공격 입력 범위 답변 대기**: ATK1 회귀13/13·실검사FAIL(input Asset keyboard y absent), 공격 실험 원복; Hard 후보 회귀13/13·보스 처치PASS, V2/V3 PASS, V4 촬영KNOWN-LIMIT, V5 원문/공격 의존BLOCKED, V1 실제120분 진행 중. [현재 인계](validation-2026-10/N7-current-handoff.txt).

2026-10-05 **N6 중단 — 사용자 결정**: 초기 작업 트리는 깨끗하지만 HEAD가 N5 인계 커밋 baaaae0와 동일하여 첨부 §0의 이후 조건을 엄격히 충족하지 못함. 사용자가 시작 조건 유지·중단을 선택함. 구현·Unity 검사·120분 soak 미실시. [검토 기록](validation-2026-10/N6-preflight-review.txt).

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
| S3-06 | B02 E2 소개 위치 수정 | REVIEW | G3:DONE | E2 x17/왼쪽/바닥, 발판 위 감지4.787u·반복 돌진 벽앞0.140u 정지·피해 없음·바닥 피해·B01/B02/B03 왕복 및 변경 위험에 비례한 최소 검증(../AGENTS.md Validation) |
| S3-07 | B05 연결·C01 휴식·C02 조합 | REVIEW | G3:DONE | 지정 배치·CP-C01 저장/사망·왕복·바닥 더블 최대발5.905<6.8·Start 일반5.535<6.8/더블 착지·5u 간격 일반실패/대시성공·스폰 비겹침·변경 위험에 비례한 최소 검증(../AGENTS.md Validation) |
| S3-08 | C03 종합 이동·C04 휴식 | REVIEW | S3-07:REVIEW | 지정 E1/E3/엄폐·CP-C04 저장/사망·C02/C03/C04 왕복·CP→C06 입구1.280s·변경 위험에 비례한 최소 검증(../AGENTS.md Validation), Right는 C06 생성까지 비활성 |
| F1 | 최소 공격 타격 피드백(FX-001) | BLOCKED | 사용자 승인 | v2 첫120fps 명중FAIL→검사1회보정→효과PASS, 필수T2 .12s 넉백잠금FAIL. 전체복원, 실험빌드 사용자 판단 대기 |
| S3-09 | C06 최종 보스 | REVIEW | S3-08:REVIEW | v2 작성 실패 1회 보정 후 시야/회피/회복 X명중/20타/사망/동시프레임/Pause/문 왕복과 변경 위험에 비례한 최소 검증(../AGENTS.md Validation). 사용자 체감 대기. F1 미적용 |
| S3-10 | 엔딩·저장·타이틀 흐름 | REVIEW | S3-09:REVIEW | v2 실제 exe 처치즉시저장/2초빛/엔딩/타이틀/즉시종료후Continue/새게임초기화/가상패드 UI와 변경 위험에 비례한 최소 검증(../AGENTS.md Validation). 엔딩 표현 사용자 판단 대기 |
| S3-G | Sprint 3 Gate | TODO | 사용자 판단 | Content Complete 미충족, 다음 Sprint 미착수 |

새 방 공통 셸 36×14u/바닥윗면y1/좌우벽/경계/카메라5.5·0·0.15/방이름/Door 표시/트리거1×2/바닥스폰y1.81. B02~B04 왼쪽출구(1.75,2), 양쪽 있는 방 오른쪽(34.25,2), FromLeft(3.5,1.81)/FromRight(32.5,1.81). G-J 높이는 일반최대+0.8以上/더블최대−0.5以下 측정 후 조건 불충족 시 범위 안 가장 가까운 값만 적용·결정 기록. S3-05의 B03 연결은 S3-04 실패 시 활성화하지 않고 독립 구현 가능한 부분만 보수적으로 처리한다.


### 후속 후보와 기록 위치

- **BOSS-2 PARTIAL** — B2 후보 실제exe 처치 PASS, 추가 패턴 검증 자동승인 거부. 기본2패턴·H1값 유지, 후보 채택은 사용자 판단.
- U1 / F2R / LOGR: REVIEW, 기존 사용자 체감 수락 대기. [기술 결과](validation-2026-10/TM-U1-final-handoff.md).
- CAM-002 / K2: DEFERRED, 아트 적용 후 재확인. 기존 0.15s 유지.
- 완료 Task 의존성 및 관찰·결정·리소스 원문: [Archive](../Archive/TASKS_ARCHIVE.md). DONE 의존성은 Archive의 완료 행을 함께 확인한다.

