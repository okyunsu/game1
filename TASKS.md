# 개발 작업과 기록

## Current Sprint

Batch05: Sprint3 콘텐츠 작업 연장. 승인된 V2는 18방 별도 후보이며 기존 게임의 수락 상태는 유지한다. V2 완성 후 사용자 자연 완주로 Gate를 판단한다.


**Sprint 3**, 진입2026-10-03. 수락된 Target15방 + 선택 A06/B06/C05 후보로 실행18방. A06/C05 사람시간·체감 PARTIAL, B06 새 진입/왕복/CP/적초기화·전체R01~R17 기술PASS/REVIEW. PRD/GAME_DESIGN Target18 확정 조건은 아직 미충족.
S3-01~05 DONE, S3-06~10·S2-04·F2R·LOGR·U1 REVIEW 유지. Sprint3 Gate는 사용자 자연 완주·체감 판단 대기, 다음 Sprint 확장 미착수.
F1-R REVIEW: 명중0.05s/Pause·사망·0초 기술PASS 유지, 체감 대기. P03 진단 산출물 DONE, 게임 변경0·원본10/10/보정후9/10, 간헐 실패 이슈 OPEN.
H1 기본값·기존2패턴·H2/H3 보존. AV1 사운드/형태·명도 후보 기술PASS/REVIEW. B2 돌진3fps·변형60/Pause·사망복귀 검증PASS, 잔여 도약회피 전체fps·동시HP0 및 감각으로 전체 PARTIAL.
## Current Tasks

2026-10-08 **Batch05 진행 중** — START_COMMIT `fe4b664b4f495f0cfdee9e1a1231287d3ff8d318`, 시작 clean / Unity 닫힘. 최대8시간, 보수적 예산 종료 시각 17:00 KST. Sprint3 콘텐츠 연장, 기본 교체·Gate는 사용자 판단 대기.

| Task | 상태 | 현재 결과 |
|---|---|---|
| B05-0 문서 | DONE / `ab16417` | 승인한 PRD/GAME_DESIGN 추가·Resource Register·LEVEL_V2 기준 |
| B05-A 아트 | REVIEW / `214f2d3` | CC0 팩·스킨 Variant·Collider diff0·S1-02/04·스킨 전투 PASS, 스타일 수락 대기 |
| B05-I 매핑/빌드 | DONE / `19c93f3` | V2 전용 Scene 매핑·기본13/13+RouteAll PASS·CP-A03 Continue PASS |
| A01~A06 V2 | REVIEW | 필수·선택 각6/6 지정 입력 재생/사망/왕복 PASS/Error0, 사람 수락 대기 |
| A지역 혼합 연속 | DONE | 전달 B05-V2-A: A01→A05 Dash→A03 G-D→기존 B01 연속8구간 재생 PASS/Error0 |

이전 Batch04 Task·인계와 수락 대기는 [Archive](Archive/B04-tasks-and-handoff-20261008.md). 기본·AV1·B2 후보 수락을 이번 V2 수락과 합치지 않는다.
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

Batch05 A지역 종료 후 순서대로, 예산 남으면 계속:
1. B01~B06 V2 — Dash 보유, B04 Double Jump, B01→B05 G-J, B06 Dash·포고만.
2. C01~C06 V2 — 두 능력, C06 기존 BossEncounter 기준·패턴·수치 유지.
3. V2 18방 전체 빌드 — 새 게임→엔딩→타이틀 연속 입력, 저장/이어하기 1회, Error0.
4. V2 QA 안내 1쪽 — 지역별 확인·시간 가설·기존 비교.
## Latest Handoff

2026-10-08 **Batch04 기술 완료 / AV1 후보 REVIEW**, 인계 포함 약40분/예산8시간. 새 플레이 범위: 후보10종 효과음, 역할별 기하형·최소 애니메이션·잔상/사망조각, 명도 개선. 기본18방·튜닝·보호 로직은 보존. 기본13/13·기본/AV1 최종exe 전체R01~R17 PASS/Error0. B2 돌진 주요 검증 위험 해소, 감각/기본채택/Sprint Gate 미수락. [최종 증거](Validation/B04-final-summary.md).

### 결정 질문지

| 질문/선택지 | AI 추천과 이유 | 답으로 진행할 Task |
|---|---|---|
| AV1 사운드·형태 각각 **채택 / 조정 / 폐기** | 후보 유지 후 청감/가독성 조정. 기술적으로 격리·움직임중립이나 선호는 직접 판단 | AV1 REVIEW 수락 또는 다음 후보 조정 |
| 보스 **기본 / B2 / fast / slow / 수치조정** | B2를 기준으로 fast/slow 각5분 비교. 자동 회피와 사람 난이도는 다름 | 보스 후보 수락/기본채택 범위, 잔여 BOSS-2 검증 |
| 히트스톱 **.05s 유지 / 조정 / 끄기**, A06/B06/C05 **수락 / 조정** | .05s·의도적 선택입구는 우선 유지, 방 시간/재미는 플레이 후 결정 | F1-R·선택방 수락/후속배치 |
| Sprint3 Gate **오늘 자연완주 후 / 다음 완주 후 / 대기 유지** | 자연 전체완주·체감 확인 후 판단. 이번 기술PASS를 Gate로 대체하지 않음 | Sprint3 Gate 사용자 판단 |

### 직접 플레이 확인 — 수락 대기 순서, 총55분

[1페이지 QA와 실행 링크](Validation/B04-QA-summary.md). 각 `.cmd`는 해당 폴더 QA 전용 저장만 초기화, Menu Z 이어하기.

1. 기본 C06/A04 **5분** — .05s 명중 히트스톱.
2. 기본 A06/C05 **10분** — A04/C03 왼쪽 출구 위 문(x2,y5.6), 발견/귀환·사람 도전시간.
3. 기본 B06 **5분** — CP-B01→B02→B03 x7 청록 `↑ 선택 대시 도전`, Dash/포고 진입/귀환.
4. B2 C06 **10분** — 예고·4패턴·돌진DJ 회피·사망 재시도. 지정 검증은 PASS, 기존 최종 도약회피 전체fps·동시HP0은 미검증.
5. fast/slow C06 **10분** — 각5분 압박·학습감 비교.
6. AV1 A01/A04/B02/B03/C06 **10분** — 사운드·실루엣·눈/전면/포신·점프/착지/잔상/피격/사망, 무음 예고 확인.
7. 결정 기록 **5분** — 위 선택지. 자연 전체 완주가 남으면 별도로 진행하고 Gate 대기 유지.

### Task별 결과

| Task | 상태 / 커밋 | 주요 파일·실제 검증·문제 |
|---|---|---|
| A 사운드 | REVIEW / `6bdd27b` | PresentationProfile/Bootstrap, Audio/AV1 WAV10, HitFeedback 관찰; 실제exe10클립·Pause0/중복억제/기본0. 청감 대기 [증거](Validation/B04-A-summary.md) |
| B 형태/애니메이션 | REVIEW / `6bdd27b`·`de29a76` | PresentationVisual, 실제효과/Collider diff0. 기본·최종AV1 30/60/120 각150틱 위치·속도해시 동일 [증거](Validation/B04-B-summary.md) |
| 복귀·방향 보정 | DONE / `52dedf5`·`5d901ba` | 원본renderer 재노출 방지, HP0/Respawn child 숨김, Player/E2/E3 시각방향. 실제exe 복귀/사진 PASS [복귀](Validation/B04-B-recovery-summary.md) |
| C B2 검증 | DONE / `087c303` | 새 fixed-tick/지속DJ 접근1개, slow 첫 허용보정. B2 3fps·fast/slow60 회피0/실패1/Pause·재개/CP-C04·HP·패턴초기화 PASS. 기존 BOSS-2 전체 PARTIAL 유지 [증거](Validation/B04-C-summary.md) |
| 마무리 | DONE / `accfde6` | 기본 `B04-main-final`, 후보 `B04-AV1-final-r4`, 기본13/13·각exe R01~R17→타이틀/Error0. 초기AV1 R11 기록소진 원문 보존 [통합](Validation/B04-final-summary.md) |
| 음량·명도 | DONE / `aa343b4`, REVIEW / `5527868` | peak편차1.38dB/6dB outlier0, 후보body 명도·hazardOpacity 개선/기본색0. 청감·가독성 대기 [음량](Validation/B04-volume-summary.md)·[명도](Validation/B04-contrast-summary.md) |
| 사진9장 | DONE / `5d901ba` | 1920×1080 비검정·육안PASS. 실제exe 방카메라 render/HUD·overlay 제외, raw PNG 미커밋 [사진](Validation/B04-shots-summary.md) |
| QA 안내 | DONE / `b89019c` | 실행 링크·비교 항목·55분 순서 [1페이지](Validation/B04-QA-summary.md) |

**후보값/이유**: gain .30/보스.34, pitch ±.04, 중복간격.05s, PCM .13/.26/.32s(짧은 식별신호·예고 우선). 걷기8°, 점프14%/착지20%·.12s, 잔상2~3개/.12s, 사망4조각/.3s·3u/s(작고 짧은 상태 표현). E1 넓이1.18/높이.72, E2 .85/.95, E3 .85/.9(역할별 형태), body RGB 및 위험물opacity1은 [표](Validation/B04-contrast-summary.md)의 player 구별/영역 대비 목적. Source는 **Assets/Candidates/AV1/PresentationProfile.asset** 한 곳. Play 값을 기록하고 종료 후 확인/반영·저장·후보 재빌드. 기본 채택0.

**보호 파일 관찰 추가 라인: 없음(0)**. 비보호 HitFeedback의 `Presented` 이벤트 선언/호출만 추가, 기본 구독자0. 기존 Scene/Prefab/기본Tuning/BossCandidates/Player core/GameState/RoomSession/KillZone/Packages/ProjectSettings/PRD/GAME_DESIGN diff0. 새 후보는 build scene copy만 활성화. 원본/실패/CSV/사진은 Validation/_raw/B04/ 보존, Task별 요약만 커밋, push0.

### 다음 Batch 후보

1. 사용자 AV1 청감·형태·명도 수락/조정 선택 후 승인된 후보만 보정 또는 기본 채택.
2. B2/fast/slow 자연 비교 선택, BOSS-2 잔여 도약회피·동시HP0 검증은 별도 승인 범위 지정.
3. 히트스톱·선택방 수락, 자연 전체 완주 후 Sprint3 Gate 사용자 판단. 다음 Sprint 대규모 콘텐츠는 미착수.
4. 필요 시 P03/R11 기록 경계 민감성 조사 범위 지정. 이번 게임/입력 기록 변경0.



