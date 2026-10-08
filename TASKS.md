# 개발 작업과 기록

## Current Sprint

Batch05: Sprint3 콘텐츠 작업 연장. 승인된 V2는 18방 별도 후보이며 기존 게임의 수락 상태는 유지한다. V2 완성 후 사용자 자연 완주로 Gate를 판단한다.


**Sprint 3**, 진입2026-10-03. 수락된 Target15방 + 선택 A06/B06/C05 후보로 실행18방. A06/C05 사람시간·체감 PARTIAL, B06 새 진입/왕복/CP/적초기화·전체R01~R17 기술PASS/REVIEW. PRD/GAME_DESIGN Target18 확정 조건은 아직 미충족.
S3-01~05 DONE, S3-06~10·S2-04·F2R·LOGR·U1 REVIEW 유지. Sprint3 Gate는 사용자 자연 완주·체감 판단 대기, 다음 Sprint 확장 미착수.
F1-R REVIEW: 명중0.05s/Pause·사망·0초 기술PASS 유지, 체감 대기. P03 진단 산출물 DONE, 게임 변경0·원본10/10/보정후9/10, 간헐 실패 이슈 OPEN.
H1 기본값·기존2패턴·H2/H3 보존. AV1 사운드/형태·명도 후보 기술PASS/REVIEW. B2 돌진3fps·변형60/Pause·사망복귀 검증PASS, 잔여 도약회피 전체fps·동시HP0 및 감각으로 전체 PARTIAL.
## Current Tasks

2026-10-08 **Batch05 기술 작업 종료 / 콘텐츠 REVIEW**. START_COMMIT `fe4b664b4f495f0cfdee9e1a1231287d3ff8d318`, 시작 clean / Unity 닫힘. 최대8시간 안에 A→B→C→전체 V2→QA 승인 Backlog를 소진했다. 기본 교체·Sprint3 Gate는 대기한다.

| Task | 상태 | 결과 |
|---|---|---|
| B05-A CC0 스킨 | REVIEW | Kenney Pixel Platformer, Tilemap 1회 저장·재개방 PASS, Variant collider diff0, S1-02/04·실제 공격/피격 PASS |
| A01~A06 V2 | REVIEW | 폭60/60/72/90/72/60u, 필수·선택 각6/6 재생 PASS, Dash 게이트 유지 |
| B01~B06 V2 | REVIEW | 폭84/72/90/72/60/60u, 각6/6 PASS, G-J·B06 Dash/포고 검증 PASS |
| C01~C06 V2 | REVIEW | 폭72/84/90/54/72u+20×36u 아레나, 각6/6 PASS, 기존 보스→엔딩 PASS |

끝난 문서·지원·혼합/전체 빌드·QA Task와 방별 커밋은 [Batch05 Task 인덱스](Archive/B05-completed-task-index.md). 이전 Batch04 Task·인계·수락 대기는 [이전 Archive](Archive/B04-tasks-and-handoff-20261008.md). 기본·AV1·B2 수락과 V2 수락을 합치지 않는다.

## Bugs

### B04-REPLAY-EDGE — R11 초기 기록 소진 / OPEN(제품 회귀 미확정)
초기 AV1에서 시작편차 .110u로 R11 기록을 소진. 기본/수정r2/명도delivery/최종r4 연속 R01~R17은 PASS, 실제회귀 확정 아님. 기존 입력 라이브러리/게임 변경0. 원문은 [최종 요약](Validation/B04-final-summary.md), 재현성 탐색은 다음 승인 범위.


### B03-P03-BOUNDARY — 기록 소진/목표 판정 경계 / OPEN
원본 독립P03 10/10, 중립5틱 후보9/10. 실패는 점프 소비1틱 차이·대시 높이 차이 이후 착지 직전 입력 종료; 종료 뒤 goal 로그. 입력 보정1회 사용/추가 수정 없음. 보호된 게임 변경0. [진단](Validation/B03-P03-summary.md).

### B03-BOSS-VALIDATION — 패턴별 검증 / PARTIAL
B03 당시 자동 승인 검토가 4번째 fixture를 거부한 이력은 보존. BATCH-04 추가3접근 승인 후 새 접근1개+slow 첫보정으로 돌진30/60/120·fast/slow60 회피0/실패1, 도약/돌진 Pause·재개·CP-C04/플레이어HP·보스HP/첫패턴초기화 PASS. B04-C DONE. 남은 전체 BOSS-2 범위는 최종 도약 회피 전체fps·동시HP0 미검증과 감각 수락. [이전](Validation/B03-A-summary.md)·[추가 검증](Validation/B04-C-summary.md).

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

### B05-V2 — 그래픽·방 방향·기본 교체 / REVIEW

CC0 픽셀 스타일 유지/교체, A지역 밀도·필수/선택 난이도 방향, V2 기본 교체 시점은 Latest Handoff 질문지로 판단한다. 후보 좌표는 LEVEL_V2에 기록했고 기본 Scene·수치 채택은 하지 않았다. 사람 방 시간·CP→주요 도전30~90초는 미실측이며 기술 PASS와 구분한다. 새 규칙 변경·외부 차단 없음.


### B04-AV1 — 후보 수락 / REVIEW
사운드/형태/명도 각각 채택·조정·폐기, B2/fast/slow 선택, 기존 히트스톱/선택방 수락 및 Sprint Gate 판단 시점은 Latest Handoff 질문지를 따른다. 기본 채택·규칙 변경0.

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

Batch05 승인 큐 소진: B지역6방 → C지역6방 → 전체 V2 연속 완주·저장 → QA 안내 완료. 남은 것은 사용자 콘텐츠 수락이다. Batch 밖 작업·다음 Sprint·기본 교체는 착수하지 않았다.

## Latest Handoff

2026-10-08 **Batch05 종료 / 18방 V2 콘텐츠 REVIEW**. 새 플레이 범위는 넓어진 A/B/C 18방, 방별 선택 발견18개, CC0 픽셀 스킨·지역 배경과 별도 V2 빌드다. A지역 혼합 빌드와 전체 V2를 모두 보존했다. 최종 exe에서 새 게임→능력2종→보스→엔딩→타이틀 연속23구간 및 CP-C04 저장/이어하기 PASS/Error0, 기본13/13+기존 RouteAll 1회 PASS. 최대8시간 안에 승인 Backlog까지 소진하여 종료. 로컬 Task 커밋만 수행, push0.

### 결정 질문지

| 질문 / 선택지 | AI 추천과 이유 | 풀리는 Task |
|---|---|---|
| 에셋 팩 스타일 **유지 / 다른 CC0 팩으로 교체** | 유지 후 직접 A를 확인. 지형·인물·적·배경이 한 팩으로 통일되고 물리 변경 없이 적용됨 | B05-A 스타일 수락 또는 후속 아트 Batch |
| V2 A지역 방향 **유지 / 밀도·필수·선택 배치 수정** | 후보 유지 후 밀도와 동선에 집중. 안전 바닥과 높은 선택 경로는 기술적으로 통과했으나 재미·시간은 미실측 | A01~A06 REVIEW 수락 또는 배치 수정 |
| V2 기본 교체 **지금 / 전체 자연 완주 수락 후 / 후보 유지** | 전체 자연 완주와 Sprint3 Gate 판단 후. B/C·보스·저장 체감도 함께 확인 필요 | 기본 Scene 채택·Sprint3 Gate 사용자 판단 |

이전 AV1/B2/히트스톱 질문과 수락 대기는 [Batch04 인계](Archive/B04-tasks-and-handoff-20261008.md#latest-handoff)에 보존했다.

### 직접 플레이 확인 — 수락 대기 순서

1. 이전 Batch04 대기: 히트스톱→기본 선택방→B2→fast/slow→AV1, 계획55분. [기존 실행/QA](Validation/B04-QA-summary.md), 기존 후보 그대로.
2. **V2 A지역 약15~20분**: [Play-V2.cmd](Builds/V2-delivery/Play-V2.cmd) 실행, A01 시작→A05 Dash→A03 G-D, A06 선택 분기. 방의 밀도·휴식/전망·필수 쉬움/선택 어려움·문/적 가독성을 본다. [A01 1920×1080](Validation/_raw/B05/A01-shot-delivery.png), [스킨 테스트](Validation/_raw/B05/SkinTest-shot-delivery.png).
3. V2 B지역15~20분: B04 추가 점프→B01 높은 문→B05, B03 ↑ 입구→B06 Dash/포고. 발견 후 귀환·CP 복귀를 확인한다.
4. V2 C지역15~20분: 밝은 관측탑·C03 선택 분기·C04 CP→C06 기존 전투·높은 선택 전망. [C06 1920×1080](Validation/_raw/B05/C06-shot-delivery-v2.png).

시간은 사람 계획치이며 미실측이다. [1쪽 QA](V2_QA.md)에 조작·비교점·수정 위치를 적었다. 실행기는 별도 `progress-v2-qa.json`을 쓰며 exe 직접 실행은 기존 기본 슬롯을 공유한다. 사진은 실제 Camera 렌더로 HUD 제외.

### Task별 결과

| Task | 상태 / 로컬 커밋 | 주요 파일·실제 검증·문제 |
|---|---|---|
| 문서·에셋·지원 | DONE / `ab16417`, `214f2d3`, `19c93f3`, `09811a4` (스타일 REVIEW) | PRD/GAME_DESIGN 승인 줄, RESOURCES, LEVEL_V2, B05Build/V2SceneMap. Tilemap 저장·재개방1회 PASS, 기본 회귀 PASS. [에셋 증거](Validation/B05-A-summary.md) |
| A01~A06 | REVIEW / 방별6커밋 | 신규 Scene·V2 입력, 필수/발견6/6·사망CP·물리문 왕복 PASS. A03 Dash 미보유 차단 PASS. [방별 커밋·증거](Archive/B05-completed-task-index.md) |
| B01~B06 | REVIEW / 방별6커밋 | 신규 Scene·입력, 각6/6 PASS. B01 G-J 미보유 차단, B06 포고2회/추가 점프 없이 PASS. 문 좌표 정렬 문제 해결, 수치 변경0 |
| C01~C06 | REVIEW / 방별6커밋 | 신규 Scene·입력, 각6/6 PASS. C06 기존 floor/wall/startX·보스 값 유지, 선택 세로7단. 보스 실제 처치 PASS |
| 스킨 최종 적용 | REVIEW / `942f199`, `af7ef0c` | V2 문 시각 자식·C06 잔여 경고 정리. 19 Scene collider diff0, 실제 스킨 공격·피격·적 처치 PASS/Error0. [증거](Validation/B05-door-skin-summary.md) |
| A 혼합 마무리 | DONE / `c53893e` | A01→A05 Dash→A03→기존 B01 8구간 연속 PASS/Error0. [증거](Validation/B05-A-mixed-summary.md) |
| 전체 V2 | DONE / `1e3627c` | 최종 V2-delivery build errors0·연속23구간·저장/Continue PASS/Error0. [실패 포함 증거](Validation/B05-V2-full-summary.md) |
| 1쪽 QA | DONE / `afb8fde` | V2_QA/README/LEVEL_V2, 링크·별도 저장 실행기 확인. [문서 증거](Validation/B05-QA-summary.md) |

방별 설계표와 Codex가 정한 좌표·높이·이유는 [LEVEL_V2](LEVEL_V2.md). 일반 필수 바닥은 안전하게 연결하고 넓은 낮은 턱, 선택은 높은 정밀 탑으로 분리했다. C06은 기존 전투 폭20u를 유지하며 높이36u로 확장했다. 사람 시간·밀도·반복감·손맛은 REVIEW다. 문 왕복은 출구 앞 배치 fixture, 피격 사망은 원본 Health API를 사용했으며 자연 전체 역방향 이동/사람 도전시간의 대체 검사가 아니다. 비차단 이전 Bugs는 유지했다.

**매핑 추가:** RoomSession 변경0. `V2SceneMap`에 A01~C06→같은 ID `_V2` Scene 등록, B05Build가 V2 빌드 사본의 RoomExit/OptionalRoomEntry만 재연결. `ProgressSave.PendingScene`에 Resolve wrapper 추가: CP-A01→A01_V2, CP-A03→A03_V2, CP-B01→B01_V2, CP-C01→C01_V2, CP-C04→C04_V2. Ending 경로·저장 형식·Spawn/문/CP ID 유지. 기본 프로필에서는 기존 경로로 동작한다.

**보호/인계:** 기존 Scene·원본 Prefab·기본 Tuning·core·Packages·ProjectSettings·기존 Validation 입력 diff0. [정정된 보호 검사](Validation/_raw/B05/protected-final-diff-v2.txt). 에셋 출처·CC0·확인일은 [RESOURCES](RESOURCES.md). 조정은 V2 Scene의 배치/카메라/CP/적 Reset, 반복 시각은 V2 Prefab, 이동·전투 값은 기존 Tuning Asset. Play 중 값을 메모하고 Stop 후 다시 반영·저장·V2 재빌드한다. 원본 raw 로그·CSV·PNG 보존, 미실시를 PASS로 기록하지 않았다.

### 다음 Batch 후보

사용자 A/B/C 자연 플레이 의견에 따른 V2 밀도·선택 경로 조정, 그래픽 선호 수정, 전체 수락 후 기본 교체 및 Sprint3 Gate 판단. 모두 다음 승인 범위이며 이번에는 수행하지 않았다.
