# 개발 작업과 기록

## Current Sprint

Sprint3 / Batch10 예산 종료(2026-10-12 19:06 KST 기준). 재개에서도 새 예산을 시작하지 않았다. 브랜치 review/batch05-v2, 로컬 커밋만/push0. T2 70경로 전체 저장 재생2회 PASS, 기본13+RouteAll+Pogo PASS. 자연 S02 완주·전 연결/고립 판정·V2 W22는 미완료. 기본 채택/감각 수락/Sprint Gate 미실시. 다음 Batch는 사용자 승인 후 진행.

## Current Tasks

| Task | 상태 | 현재 결과 |
|---|---|---|
| B10-T1 | DONE | 장치9개·기존 경로10개 저장 입력/전체 경로2회 PASS |
| B10-T2 | DONE | 재개 설계 70/70 저장 전체 입력2회 PASS; A02/wall·C02/low·Z01/up_A05 해소. r54~r58 이전 Scene 결과 제외; r59/r61/r63 현재 Scene·설계 hash 확인 |
| B10-T3 | DONE | covered floor trigger inset 수정 47b20ac, Claude 설계 수정 662cc0c / V4 재생성 24ceeda, 21방 검사 PASS |
| B10-T4 | PARTIAL | 실제 전체 왕복 저장2회 PASS34건, 나머지 탐색/판정 미완료 |
| B10-T5 | PARTIAL | 최소 능력 다른 출구 저장2회 PASS36입구, 고립0 미증명 |
| B10-T6 | PARTIAL | 새 게임·대시·벽 점프·B05 자연 등반→C02; S02 미완주 |
| B10-T7 | DONE | 보스전→Slam→천장→S01 저장 입력2회 연속 PASS |
| B10-T8 | DONE | 동일 HEAD 과거 빌드 고정 입력2회 PASS; 빌드 구성으로 범위 축소, 원인 커밋 미확정/수정0 |
| B10-T9 | DONE | 작성된 시작→끝 16구간, 전체 저장 입력2회 PASS; 장치 작동은 T1 별도 |
| B10-T10 | PARTIAL | 70경로30fps43PASS/27FAIL,120fps59PASS/11FAIL; 자연 미실시 |
| B10-T11 | DONE | 27 실제 입력 조합 unexpected=0, Error0 |
| B10-T12 | PARTIAL | Claude 보고서·실제 Scene4방 촬영; 남은 분기 물리 원인 미확정 |
| B10-T13 | PARTIAL | 기본13+RouteAll+Pogo PASS, V2 W22사망; 전달1개·4기동PASS, 자연 저장2개 미확보 |
## Batch10 Backlog 결과

| 항목 | 상태 | 결과 |
|---|---|---|
| BL1 | DONE | V4 회귀 등록1회63/69 PASS,6 FAIL |
| BL2 | DONE | 장치19경로 기본17/후보1 11/후보2 12 PASS, 기본값 유지 |
| BL3 | PARTIAL | C01까지 완료 구간 시간, 전체 미완주 |
| BL4 | BLOCKED | 참조된 B09 Backlog4~9가 BATCH-09에 없음(1~3만 있음), 합격 기준 결정 필요 |

## Bugs

### B10 종료 — 반복 안정성/자연 연결/입구 판정
T2 70경로는 성공 입력 전체2회 증거 확보. 같은 입력을 다시60fps/다른fps 재생하면 일부 실패하므로 안정성 전체 PASS와 구분한다. 자연 B05 양쪽 굴뚝 회복 실제 PASS 뒤 C02 도랑/상자 접근 미완료. 왕복34·최소 입구탈출36 PASS, 미완료 분기의 확대 전수 소진은 예산 종료로 미실시. Claude 보고서에 실제 좌표/원문 실패를 보존. r54~r58 잘못된 구 Scene harness 결과는 제외했고 r59+ native Unity 빌드/hash 확인을 사용했다. 원본 삭제0. 권한 승인 대기 뒤19:06 초과를 확인해 탐색/큐를 종료하고 인계만 정리했다.


### B10-RESUME / 현재 설계 재판정
Claude 수정은 재개 문서의 명시 승인으로 적용했다. 21방 검사 PASS. A01/A02/A05/A06/B01/B02/B03/C01/C02/Z01의 이전 PASS는 무효, T2-resume 로그만 새 근거로 쓴다. Scene 차이: A01/A02/A05/A06/B02/B03/C02/Z01 변경, B01/C01은 경로 데이터만 변경, 나머지11방0. raw: resume-scene-differences-r40.txt. 재개 후 자연 실행은 실제 대시·벽 점프를 얻었으며 B04 해치에서 중단 후 수정 도구로 재실행 중. 이전 중단의 자동 승인 검토 오류는 이번 로컬 커밋/Unity 실행이 승인되어 해소됐다. 예산 종료19:06, 새 예산 시작 아님.

### B10-FAST-CLOCK / 검증 제외
무제한 렌더링 r19 실험에서 scaled/unscaled 입력 시계 차이를 실측했다. 해당 실험의 PASS/BLOCKED-GEOMETRY는 최종 판정에 쓰지 않는다. 원본 보존, 정상 60fps 후보 분할로 재검증한다. r17 이전 T5는 gate 별칭/진행 누적 상태를 보완하기 전 결과이므로 최소 능력 합격 근거에서 제외한다. clock-fast/normal-r22 원본 참조.

### B09-DISTANCE / OPEN — T6 BLOCKED
15개 낙하 도착 약2.2u, Z01→S02 2u로 승인2.5u 기준 미달. 실측 통행 불가로 확인하지 못해 데이터 이동 예외 미적용. [목록·실제 출구 검사](Validation/B09-T6-summary.md).

### B09-INPUT-COVERAGE / PARTIAL
전 연결31 판정 미통과,70경로41 미완주, 자연 A03/보스 입력 재시도 실패. 고립 보조17 PASS는 최소 입구 탈출 증명이 아니며 C06 우회 PASS는 철회했다. disabled trigger 우회는 V4 guard 수정/물리 검사로 해결. [왕복](Validation/B09-T7-summary.md)·[경로](Validation/B09-T8-summary.md)·[고립 한계](Validation/B09-T9-summary.md)·[자연/저장](Validation/B09-T10-summary.md). 추가 전체 합격 주장 없음.

### B09-REGRESSION / OPEN — 원인 미확정
기본13 PASS, RouteAll R01/Pogo P01 입력 미완주. 현재 V2 W22 실제 사망(진입편차.082u). T0 직전/직후는 모두W22·분리R09 PASS여서 T0 원인 아님을 단정하지 않고 원인 입증 실패로 유지. 조건부 회귀 수정0. [진단](Validation/B09-T11-summary.md)·[현재 회귀](Validation/B09-T12-summary.md).


### B07-C03-OPTIONAL / OPEN
필수 기본/Short·DJ 귀환은 PASS. 첫 선택 도전은 포고2회 후 높은 착지 미완주(stage6), 재개 승인 접근+보정2회 소진. 두 번째 선택 후보는 발견 기록/재생 PASS로 별도 유지. [증거](Validation/B07-R-C03-summary.md).

### B07-V2-BOSS-REPLAY / OPEN — 회귀 의심
새 빌드 V2 고정 입력이 W22 보스에서 두 번 실제 사망(tick285, 진입편차.082u), 이전 B06b 빌드는 전체 PASS. 원인 미확정. 기본13·V3 보스 실제 입력은 PASS. 추가 핵심 변경 없이 PARTIAL로 보존한다. [비교 결과](Validation/B07-T25-summary.md).


### B07-A02-TRAVERSE / OPEN
A02 필수 계단 및 재방문 굴뚝 입력 미완주. T4 PARTIAL, T24 연속 완주는 이 경로에 의존. 독립 방 제작은 계속. 이전 자동 승인 거절 이력 보존. 2026-10-11 재개에서 구체 접근 추가 승인 및 보정2회 실행 후 필수/굴뚝 미완주, PARTIAL 유지. [재개 증거](Validation/B07-R-A02-summary.md). 추가 실행 중단. [증거](Validation/B07-T4-A02-summary.md).


### B04-REPLAY-EDGE — R11 초기 기록 소진 / OPEN(제품 회귀 미확정)
2026-10-10 Batch06b 지정 회귀에서 R01~R10 PASS 후 같은 R11 시작편차0.110u/입력소진 재현, Error0. 전체 경로 PASS/새 제품 회귀로 확정하지 않음. [실제 결과](Validation/B06b-T1-summary.md).

초기 AV1에서 시작편차 .110u로 R11 기록을 소진. 기본/수정r2/명도delivery/최종r4 연속 R01~R17은 PASS, 실제회귀 확정 아님. 기존 입력 라이브러리/게임 변경0. 원문은 [최종 요약](Validation/B04-final-summary.md), 재현성 탐색은 다음 승인 범위.


### B03-P03-BOUNDARY — 기록 소진/목표 판정 경계 / OPEN
2026-10-10 Batch06b 지정 회귀 P01/P02 PASS 후 P03 input goal pogos=1 FAIL/Error0. 사전 결정대로 기준 간헐 실패와 비교만 기록, 해결 주장 없음. [실제 결과](Validation/B06b-T1-summary.md).

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

### B10 — 남은 기준/범위 결정
T5의 “다른 문” 조건이 A01/A01-A02(능력0), A05/A05-A03(대시), A06/A02-A06(대시+벽), C06/C04-C06(보스 전), S01/C06-S01(엔딩 구간)에 기존 gate/끝방 구조와 충돌한다. 같은 문 귀환·자연 획득·엔딩 후 재개를 탈출로 허용할지 / 문 조건을 유지하고 설계 변경을 별도로 승인할지 결정 필요. AI 추천은 실제 진행 가능성을 기준으로 정의를 명확히 정하되 현재 PASS에 소급 적용하지 않는 것. B09 Backlog4~9 합격 기준도 원문 필요. B10 거리16개 예외/세로 문 수평3u는 이번 Batch 사전 결정대로 적용됨. 기본 채택·장치 수치 수락은 계속 사용자 권한.


### B09 — 기준 충돌·다음 검증 결정
거리16개: 원본 spawn 유지 후 낙하 예외 허용 / .3~.5u 이내 이동을 별도 승인. 세로 방향키 유지3u: 전환0 기준으로 분리 / 이동 입력 추가. 입구별 최소 능력과 수동 검증 범위를 다음 Batch에 명시할 필요. 자세한 선택지와 추천은 Latest Handoff. 기본 채택/장치 감각/프리셋 기본 채택은 사용자 권한이다.


### B07-FINAL — 다음 후보 수정·채택 결정

A02 계단/굴뚝, C03 첫 포고 도전과 V2 보스 고정 입력 회귀 의심은 현재 PARTIAL이다. 추가 시도는 다음 Batch의 구체 범위 승인이 필요하다. 후보 유지·수정 후 검토 / 우선 직접 플레이 중 선택, AI 추천은 필수 A02와 V2 원인 진단 우선. V3 기본 채택과 Short/아트 방향은 사용자 수락까지 대기. Latest Handoff 질문지를 따른다. 이전 재시도 승인 대기는 [해소 이력](Archive/B07-resolved-decisions-20261011.md)으로 옮겼다.
### B06 — 벽 점프·가이드 수락 / REVIEW

B1/B2 기술 PASS, 기본 프리셋1/끈적2/빠름3 손맛 선택과 V2 가이드 문구 톤/길찾기 수락 대기. 원본 튜닝 기본값 채택 없음. 직접 조정하려면 WallJumpTestCourse 프리셋 또는 PlayerTuning 신규5필드 위치를 따른다. RoomExit 허용 목록 누락은 사용자 ‘Batch06 실행 + RoomExit 최소 수정 승인’ 답변으로 해소했다. Task8 예산과 Backlog 추가 실행은 구분한다. V3 레벨 치수 확정은 벽 점프 감각 수락 후.


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

Batch09 세 항목 처리 완료(후보 감각 REVIEW 유지). 이전 승인 Backlog 소진, Batch08/다음 Sprint/기본 채택 착수0.

## Latest Handoff

B10 진행: 예산 종료 / 완료 T1/T2/T3/T7/T8/T9/T11·BL1/BL2 / PARTIAL T4/T5/T6/T10/T12/T13·BL3 / 다음 승인 Batch에서 남은 경로만 진행

현재 설계 70경로 모두 저장된 전체 입력2회 PASS. 왕복34건·최소 능력 탈출36입구, B05 자연 등반을 넘어 C02까지 진행했다. 전체 완주/고립0는 미증명이다. 기본13·RouteAll·Pogo PASS, V2 W22 실제 사망. Development1개 오류·경고0, 기존 B09 실행기는 유지. 보호 게임/튜닝 수정0, push0.

### 1. 결정 질문지

| 결정 선택지 | AI 추천·이유 | 풀리는 Task |
|---|---|---|
| 입구 탈출에 같은 문 귀환·자연 획득/엔딩 재개 허용 / 다른 문 기준 유지 후 설계 별도 승인 | 진행 가능한 종착/획득 구간을 별도로 정의. 현재 gate를 무단 변경하지 않음 | T5 설계 의존5곳 |
| 남은 자동 경로 재시도 Batch 승인 / 먼저 직접 플레이 | C02 자연 연결·입구 경유 계획을 이어서 검증. 70경로는 이미 통과해 새 지형 변경부터 할 근거가 없음 | T4/T5/T6/T10/T12/T13 |
| 장치 기본 유지 / 후보1 / 후보2 / 수정 | 기본 유지 추천. 반복 재생17/19가 후보11/19·12/19보다 높고 감각 미수락 | BL2/장치 감각 |
| Claude 수정 수락 / 수정 요청, 낙하 예외 유지 / 취소 | 실제70경로 통과 증거로 수정 유지 추천; 감각 판단은 사용자 | CHANGES_B10/낙하16개 예외 |
| B09 Backlog4~9 원문/기준 제공 / 이번 범위에서 제외 | 참조 문서에는1~3만 있어 합격 기준 먼저 확정 | BL4 |

V2 원인은 빌드 구성으로 좁혔으나 커밋/변경 줄 미확정이므로 회귀 수정 채택안은 아직 없다.

### 2. 직접 플레이 확인

이전 B04/B05/B06/B06b/V3 수락 순서는 [이전 인계](Archive/B09-prior-tasks-20261012.md)를 유지한다. B09 결과는 [이관 인계](Archive/B10-prior-handoff-20261012.md)에 보존했다.

1. [장치 시험 코스](Launchers/B10/Play-V4-Mechanics.cmd),10분:6장치·E4·Slam 손맛. 시작→끝 입력2회/장치 단위 PASS, F 후보는 Play 종료하면 사라짐.
2. [새 게임](Launchers/B10/Play-V4.cmd),25~30분:A01부터 V4 경로/되돌아가기. 자동 자연 진행은 C02까지이며 S02 완주 보장은 없음.
3. 기존 [B09 AfterBoss](Launchers/B09/Play-V4-AfterBoss.cmd),5~10분:기존 전체 능력 fixture 실행기. 자연 저장과 구분해 Slam/숨은 길 감각 확인.
4. [대시 후 실제 저장](Launchers/B10/Play-V4-AfterDash.cmd),5분:타이틀 이어하기→CP-A03, 대시 복원.
5. [벽 점프 후 실제 저장](Launchers/B10/Play-V4-AfterWall.cmd),5분:타이틀 이어하기→CP-B01, 대시+벽 점프 복원.

보스 전/엔딩 후 실행기 파일은 있지만 자연 저장이 없으므로 실행 불가 안내. 네 단계 모두 준비됐다고 보지 않는다. 위 실행기를 재실행하면 원본 단계 저장을 전용 slot에 다시 복사한다. 조정 값 위치/Play 종료 후 반영은 [LEVEL_V4](LEVEL_V4.md).

### 3. Task별 결과

| Task·상태 | 커밋 | 주요 변경·실제 검증·문제 |
|---|---|---|
| T1 DONE | f9824fc | [봇](Validation/B10-T1-summary.md), 장치9·경로10 저장2회 |
| T2 DONE | a340493 | [70경로](Validation/B10-T2-summary.md), 재개 변경 방 재판정70/70 |
| T3 DONE | 5719d3e / 47b20ac / 662cc0c / 24ceeda | [생성기/승인 데이터](Validation/B10-T3-summary.md),21방 PASS |
| T4 PARTIAL | 545efa3 | [왕복](Validation/B10-T4-summary.md), 전체 입력34건2회 PASS |
| T5 PARTIAL | 92f9c7f | B10JumpSearch/[입구표](Validation/B10-T5-summary.md),36입구2회 PASS·나머지 미판정 |
| T6 PARTIAL | 654ca66 | [자연 진행](Validation/B10-T6-summary.md), B05→C02, S02 미완주 |
| T7 DONE | d17df92 | [보스](Validation/B10-T7-summary.md),승리→Slam→S01 2회 |
| T8 DONE(범위 좁힘) | f47e265 | [진단](Validation/B10-T8-summary.md),동일 HEAD 과거빌드2회 PASS, 원인 커밋 미확정 |
| T9 DONE | 06f8835 | [시험 코스](Validation/B10-T9-summary.md),연속 전체2회 |
| T10 PARTIAL | 58a1489 | [70경로fps](Validation/B10-T10-summary.md),30fps43/70·120fps59/70, 자연 미실시 |
| T11 DONE | 9bc23d1 | [입력27조합](Validation/B10-T11-summary.md),의도치 발동0 |
| T12 PARTIAL | 6605b98 | [Claude 보고서](Validation/B10-geometry-report.md),좌표/원문/4방8스크린샷, 물리 원인 일부 미확정 |
| T13 PARTIAL | 이 인계의 커밋 | B10StageVerifier/Launchers/B10/[전달](Validation/B10-T13-summary.md),기본15PASS·V2 W22FAIL·4기동PASS·자연저장2개 없음 |
| BL1 DONE | 949beb3 | [V4회귀 등록](Validation/B10-BL1-summary.md),1회63/69 PASS |
| BL2 DONE | 850c890 | [19경로후보비교](Validation/B10-BL2-summary.md),기본값 유지 |
| BL3 PARTIAL | 67f35a1 | [자동 체류시간](Validation/B10-BL3-summary.md),완료 구간만/PRD60~120분 달성 주장 없음 |
| BL4 BLOCKED | 이 인계의 커밋 | 참조 Backlog4~9 원문/합격 기준 없음 |

원본은 Validation/_raw/B10/에 보존·커밋 제외. 전달 Build는 B10-delivery 한 개. 마지막 r75~r78은 opt-in 입력 계획만 바뀐 추가 Release 검사이며 보호 게임 동작 변경0. Sprint Gate/감각 REVIEW→DONE 수락을 대신하지 않았다.

### 4. 다음 Batch 후보

C02 상자 경유→대시 자연 입력부터 새 게임 재시도 → C03/C05/C04→보스→엔딩→Z01→S02 → 실제 보스 전/엔딩 후 저장 확보 → 자연 입력30/120. 동시에 남은 입구/왕복의 경유 노드·넓은 도약 위치 탐색, 실제 막힘을 입증한 뒤 승인 한도 데이터 수정. V2 W22 빌드 구성 비교는 별도 기록을 바탕으로 진행. 기본 수치 채택·Sprint Gate는 사용자 수락 후.
