# 개발 작업과 기록

## Current Sprint

**Sprint 3**, 진입일 **2026-10-03**. 최신 TM/U0 DONE/U1 REVIEW/S4-pre2 빌드·실제exe·회귀13/13PASS, 촬영6장 미실시/사용자 UI 검토 대기(2026-10-04). Sprint3 Gate 판단 유지. 사용자 S2-day exe 플레이로 Sprint 2 Gate 통과, S1-10·T1~T4·점프 일관성·K1·S2-01~03·05~06 DONE, S2-04 REVIEW(N5 Part1 재검증 PASS). 현재 Target 15방(A06/B06/C05 보류), 설계 v0.2 유지.
2026-10-04 최신 추가 승인 D0/F2R/LOGR 종료: 기준T2 실패 재현·T2v2 PASS, F2R/LOGR REVIEW, S4-pre 빌드·실제exe·최종회귀PASS. 아래 S3 후보/사용자 Gate 상태 유지.
G3로 S3-01~05 DONE/B04 보정 수락, S3-06~08 REVIEW 유지. 승인 F1→S3-09→S3-10→최종 빌드 v2 종료. F1 효과 이벤트 재검사PASS 뒤 T2 회귀FAIL로 복원/BLOCKED, 실험빌드 보존. S3-09/10 기술PASS/REVIEW, 최종15방+Ending 빌드/실제exe/회귀PASS. Content Complete 후보, Sprint 3 Gate는 사용자 완주 후 판단. 선택방/아트/사운드/흔들림/K2/기존배치 변경 없음(C04 Right 활성화만), push 없음.


## Current Tasks

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

- **BOSS-2 TODO** — 보스 이동·패턴 확장(사용자 설계 후). DIFF-002에 따른 후속 작업, 이번 운영 정리 범위 밖.
- U1 / F2R / LOGR: REVIEW, 기존 사용자 체감 수락 대기. [기술 결과](Validation/TM-U1-final-handoff.md).
- CAM-002 / K2: DEFERRED, 아트 적용 후 재확인. 기존 0.15s 유지.
- 완료 Task 의존성 및 관찰·결정·리소스 원문: [Archive](Archive/TASKS_ARCHIVE.md). DONE 의존성은 Archive의 완료 행을 함께 확인한다.
## Bugs

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

### S3T01-R06 — 기존 자동 경로 시작 가정 수정 범위 / BLOCKED
새 R05는 사망 없이 A05→A04→A03으로 귀환했으나, R06 기존 기록은 A03의 이전 체크포인트 근처 (15.9712,1.8148)에서 시작한 입력이다. 실제 정상 귀환은 FromRight (32.5,1.81)이고 재생 시작 편차16.418~16.500u에서 A04로 다시 나간다. A03/B01 및 게이트는 이번에 수정하지 않아 새 선택 배치 때문인 회귀로 확인되지 않았다. 다른 구간은 새 배치 때문에 깨진 경우만 수정하라는 승인 범위에 따라 R06 입력은 보존하고 중단했다. 필요 판단: 정상 귀환 시작점에 맞춘 R06 입력 교체 허용 여부. G-D 연속 통과·전체 완주 PASS는 미확인. [실제 배포 재생](Validation/S3T01-route-delivery.txt), [범위·9회 시도 요약](Validation/S3T01-route-summary.txt).

### S3T01-POGO — P03 고정 입력 일회 실패 / REVIEW
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

- 2026-10-04 UI 문서 불일치: GAME_DESIGN §18의 구키(Space/J/K)와 최신 K1/TM-U1 명세 충돌. 최신 사용자 명시 키(방향키/Z/X/C)를 표시 원본으로 적용. GAME_DESIGN 원문 변경은 이번 범위에 포함하지 않음.

### LOG-001 — 정확한 사망 원인 기록과 구독 전용 제한
LOG BLOCKED/미구현. RoomSession 진행 이벤트 없음, PlayerHealth.Hit은 원인·source 없는 Action이며 KillZone도 직접 Die만 호출함. 근처 적/접촉체 추정은 동시 피격·무적 상황에서 실제 사망 원인을 보장하지 못함. 기존 로직은 그대로 유지함. 필요 판단: 관찰 전용 이벤트 추가를 허용할지, 또는 사망 원인 unknown 및 일부 기록 보장 축소를 허용할지. [인터페이스 확인 및 미실시 내역](Validation/LOG-playtest.txt).

현재 v2 판단: F1 필수 T2 넉백잠금0.12s 검사 실패 원인·공격 손맛/피드백 후보 수락. 효과 이벤트 30/60/120fps는 PASS이나 단계는 전체복원/BLOCKED다. Builds/Experimental-F1/Afterglow.exe 사용자 직접 플레이 후 재개 범위를 결정한다. 보스 좁은방 시야/회복 공격 이전 차단은 v2로 해결. 보스·엔딩 기술PASS는 체감/Gate 수락을 뜻하지 않으며 사용자 완주 후 Sprint3 Gate 판단. 선택방/에셋/K2/다음Sprint 미승인.

2026-10-03 v2 F1: 효과 검사 첫120fps 명중FAIL → 방향 입력을 FixedUpdate까지 유지하도록 1회 보정 → 30/60/120fps 효과 이벤트 PASS. 필수 T2 회귀 knockback lock0.12s FAIL로 단계 실패/전체 복원. 추가 재시도 없음. 실험 Builds/Experimental-F1/Afterglow.exe 유지(기동 Error/Exception/Assert0), 사용자가 공격 손맛을 직접 판단. S1-02/04/07/08/N2카메라/T1 PASS, T3/T4/S2-01 미실시. 독립 S3-09 진행; 보스 F1 미적용. 증거 F1-feedback-v2.txt, F1-feedback-v2-rerun.txt, Verification-night-T2-F1-v2.txt, Experimental-F1-v2-build/runtime.txt.

CAM-002 / K2 **DEFERRED** — 사용자 2026-10-03: 아트 적용 후 재확인, 우선순위 낮음. Sprint 4 Polish 후보. 기존 0.15s 유지, 이번 재시도 없음. 이전 두 번째 비교 실패는 절대 좌표 비교 영향 추정; 상대 카메라-플레이어 거리 2.06u/2.08u. 재시도는 상대 거리로 비교.

이전 K2 / CAM-002 기술 실패 기록(현재 DEFERRED): 두 번째 검증에서 평상시·반전 비교 FAIL로 승인된 실패 규칙에 따라 전체 복원. 카메라 기본0.15s 유지. 시간 기반 입력의 물리 틱 차이 가능성은 미확정이며 추가 수정/재시도는 별도 승인 필요. 대시 밀림 체감은 미해결.


## Latest Handoff

### 결정 질문지

| 질문 / 풀리는 Task | 선택지 | AI 추천과 이유 |
| --- | --- | --- |
| R06 시작점 입력 교체를 다음 Batch에 포함할까? / S3T01-R06 | 입력 교체 허용 / 보류 | 입력 교체 허용. 정상 귀환과 기존 녹화 시작점의 차이를 해결하며 게임 배치는 유지할 수 있다. 이번에는 실행하지 않았다. |
| BOSS-2의 행동 설계를 언제 확정할까? / BOSS-2 | 사용자 설계 후 승인 / 보류 | 사용자 설계 후 승인. 수치 상향 보류라는 DIFF-002 결정을 따른다. |

### 직접 플레이 확인

이번 실행은 문서만 변경했다. 아래는 기존 수락 대기이며 새 검증 결과가 아니다. S5-levels는 **플레이 전 확인 필요**(RouteAll R06 BLOCKED)다.

| 순서 | Scene / 대상 | 볼 항목 | 예상 시간 |
| --- | --- | --- | --- |
| 1 | Menu / 저장, A04 전투 | S2-04 저장·이어하기, F2R 표시, LOGR 기록 활용 | 10분 |
| 2 | A01 / HUD | U1 HUD·첫 안내·사망 표시 | 5분 |
| 3 | B02, C02~C04, C06, Ending | S3-06~10 난이도·보스 예고·엔딩, 사람 완주 후 Sprint 3 Gate | 완주 시간 별도 |
| 4 | Test/PogoTest, 본편 | 위·아래 공격과 포고 높이·판정·능력 회복 | 10분 |
| 5 | A05, B02, B04, C02, C03 | 선택 경로5곳 재미·발견5/6, A-1 제외 | 15분 |

상세 기존 QA: [S3T-01](Validation/S3T01-user-qa.md), [N9](Validation/N9-final-handoff.md). H1 유지/H2·H3 보존만이라는 최신 DIFF-002 결정을 따른다.

### Task별 결과

운영 정리(2026-10-07): **DONE**, START_COMMIT `c62565948a0dc49c39850ba4668eb510b434f90d`.

- AGENTS 첨부 원문 교체, _raw ignore, TASKS 5섹션·Archive 보존, README 문서 구성, 검증 도구 목록, Batch 템플릿.
- 검증·커밋·변경 전후와 보존 항목: [운영 정리 보고](Archive/OPERATIONS_REVIEW.md).
- Unity·게임 회귀·빌드 미실시(사용자 지시). 이번 게임 기능·수치 변경0.
- 주요 파일: AGENTS.md, .gitignore, TASKS.md, Archive/TASKS_ARCHIVE.md, README.md, BATCH_TEMPLATE.md.
- 구현 세부 판단: 완료 의존성은 Archive의 원문 행을 확인하고, 불명확한 상태는 현재 문서에 유지했다.

### 다음 Batch 후보

1. S3T01-R06 입력 교체 범위 결정 후 진행.
2. REVIEW 직접 QA와 Sprint 3 Gate 판단.
3. BOSS-2는 사용자 설계·별도 Batch 승인 후 진행.

### 사용자 QA 요약 1쪽

- 무엇이 바뀌었나: Batch 승인·위험도별 최소 검증으로 전환하고 완료 이력을 Archive로 분리했다. 게임 변경0.
- 어디서 확인하나: AGENTS의 Batch Approval/Validation, TASKS 현재 상태와 Archive 원문, BATCH_TEMPLATE의 예산·사전 결정 표.
- 의심 지점3개: (1) 미해결 PARTIAL·BLOCKED 보존 (2) DONE 의존성과 옮긴 증거 링크 유지 (3) REVIEW·미검증 구분을 사용자 수락으로 오해하지 않는지.
