# 개발 작업과 기록

## Roadmap

| Sprint | 목표 | 주요 결과물·종료 판단 |
| --- | --- | --- |
| Sprint 1 — Movement / Vertical Slice | 이동감과 짧은 플레이 구간 | 이동·카메라·4방·체크포인트, 사용자 이동감 수락과 Slice 검증 |
| Sprint 2 — Dash / Combat / Core Loop | 첫 능력 재방문 루프 | Dash·G-D·기본 공격·E1·피격·저장, 획득→귀환→게이트→사망/재실행 안정성과 탐색 경험 |
| Sprint 3 — World / Double Jump / Boss | 시작부터 엔딩까지 연결 | Double Jump·G-J·나머지 월드/적·보스·엔딩, Content Complete 완주 |
| Sprint 4 — Playtest / Polish | 테스트 기반 개선과 출시 검수 | PRD 기준 관찰 테스트·튜닝·최소 연출·최종 빌드·개선 사례, Release Candidate 검수 |

각 Sprint 종료 시 사용자 판단 후 다음 Sprint를 세부 Task로 나눈다. Gate 미통과 시 해당 구간을 수정한다. 범위의 단일 원본은 [PRD](PRD.md)다. 늦어도 Sprint 2 종료 시 잔여 일정과 완주 전망을 판단하고, 위험하면 사용자에게 PRD Minimum Scope 적용 여부를 요청한다. 승인된 축소만 작업에 반영한다.

## Current Sprint

**Sprint 1**, 진행 재개 기준일 **2026-09-30**. 기존 이력: 2026-09-16 최초 착수, 2026-09-17 N1 기술 구현·Unity 검증 완료. 설계 기준 v0.2 유지.
S1-01~05 **DONE**. 2026-09-30 사용자가 Editor·N1 빌드에서 Movement Review를 수락했다. N2-A 승인: S1-06 Camera REVIEW, S1-07 Room Structure DONE(DEC-ROOM-B), Tilemap DEFERRED·ROOM-001 OPEN. S1-08 이후와 Sprint 2는 미승인이다.

일정 지연에 따라 오늘부터 다시 진행한다. 당장 수행할 작업량·단기 목표·Sprint 종료일·최종 완료일은 아직 정하지 않는다. 사용자가 토큰 사용량을 확인하고 업그레이드 여부를 판단한 뒤 진행 규모를 정한다. 기존 Roadmap은 순서와 범위의 참고로 유지하며 이번 일정 갱신으로 새 구현 작업을 승인하거나 기존 작업을 재실행하지 않는다.

## Current Tasks

2026-09-30 승인 범위: Movement Review 문서 기록 → S1-06 → S1-07(N2-A). 각 단계 검증·별도 커밋·기존 origin/main push. S1-08 이후는 실행하지 않는다. 2026-10-01 DEC-ROOM-B 승인으로 S1-07 블록 그레이박스를 진행한다.

상태: TODO(미착수), DOING(진행), REVIEW(기술 구현·관련 검증 완료, 사용자 판단 대기), DONE(완료 조건·필요한 사용자 수락 충족), BLOCKED(차단), DEFERRED(승인된 제외).
의존성 `:REVIEW`는 유효한 기술 검증을 갖춘 REVIEW 또는 DONE에서 충족한다. `:DONE`은 DONE만 허용하며 생략 시에도 DONE으로 해석한다. DEFERRED는 의존성을 자동 충족하지 않는다. 실행에는 별도 범위 승인이 필요하다.

| ID | Task | Status | Dependency | User Review | Done Criteria |
| --- | --- | --- | --- | --- | --- |
| S1-01 | Unity Project Setup | DONE | 없음 | — | 정확한 Editor·패키지 고정, 기본 폴더·Boot/Menu 구성, 빈 씬 Play·Windows 빌드 성공, 실행 안내 |
| S1-02 | Input System | DONE | S1-01:DONE | — | 키보드·패드, Gameplay/UI 분리, 전환·해제 Pause·재개 시 입력 유출 없음 |
| S1-03 | 좌우 이동·기본 점프·낙하 | DONE | S1-02:DONE | 가감속·급반전·공중 제어 | Rigidbody2D·PlayerTuning·Prefab·단순 검증 공간, 벽 접지 없음, Inspector 반영, 기본 조작 수락 |
| S1-04 | Coyote Time / Jump Buffer | DONE | S1-03:REVIEW | 발판 끝·착지 직전 입력 | 경계 안팎 허용·만료, 단일 소비·전환 초기화, 입력 보정 수락 |
| S1-05 | Variable Jump Height | DONE | S1-04:REVIEW | 높이 차이·낙하감 | Jump Cut·하강 배율·최대 속도 조정 가능, 짧은/긴 점프 구분, 30/60/120fps 확인·사용자 수락 |
| S1-06 | Camera | REVIEW | S1-03:DONE, S1-04:DONE, S1-05:DONE | 급반전·낙하 시야 | Cinemachine 추적·방 경계, 떨림·시야 밖 필수 착지 없음, 설정 안내 |
| S1-07 | Block Greybox / Room Structure | DONE | S1-01:DONE, S1-03:DONE, S1-04:DONE, S1-05:DONE | — | DEC-ROOM-B: Ground/Hazard는 Ground 레이어 BoxCollider2D·SpriteRenderer 블록, 출입구·Spawn ID, A01~A04 양방향 연결·안전 도착·단일 플레이어·입력 초기화, 배치 편집 가능; Tilemap DEFERRED |
| S1-07-T | Tilemap 도입 | DEFERRED | 별도 사용자 결정 | Editor GUI 확인 | DEC-ROOM-B: 이번 Sprint 제외, 사용자 GUI 확인 후 별도 Task 결정 |
| S1-08 | Checkpoint / 최소 사망 복귀 | TODO | S1-05:REVIEW, S1-07:DONE | — | CP-A01/A03·Kill Zone·최대 HP 안전 복귀, 능력 없는 초기 상태 검증; 디스크 저장은 Sprint 2 |
| S1-09 | 첫 Vertical Slice 통합 | TODO | S1-06:REVIEW, S1-08:DONE | 이동만으로 15분 플레이·조정 | A01~A04의 5~10분 이동 구간·안내·A03 닫힌 게이트 외형, 왕복·사망 복귀, 공격/능력 미구현 표시·사용자 수락 |
| S1-10 | Slice 검증·실행 안내 | TODO | S1-09:REVIEW | — | Windows 빌드 구간 재현, 경계 입력·충돌 회귀, 명백한 런타임 오류 없음, 결과·실행 경로 인계 |
| S1-G | Sprint 1 Gate | TODO | S1-06:DONE, S1-09:DONE, S1-10:DONE | 이동·Slice 수락과 다음 범위 판단 | 활성 검토 항목 수락, 문제·잔여 일정 확인, 사용자 통과 결정 기록; 통과 전 Sprint 2 착수 금지 |

## Human Review

### 2026-09-30 Movement Review — 사용자 수락 완료

- S1-03/04/05: 사용자가 Editor와 N1 빌드에서 확인 후 모두 수락, DONE.
- 관찰: "점프 거리가 일관적이지 않은 체감. 사용자는 키보드 입력(래피드 트리거 가능성) 또는 체감 문제로 판단하고 수락. S1-09 Slice 플레이에서 패드 A와 비교해 재확인."
- PlayerTuning 초기값을 사용자 수락값으로 기록(변경 없음): Move Speed 6u/s, Acceleration 80u/s², Deceleration 100u/s², Air Control 0.9, Jump Velocity 12u/s, Gravity 30u/s², Fall Multiplier 1.5, Max Fall Speed 18u/s, Jump Cut 0.5, Coyote 0.10s, Buffer 0.12s.
- 시작 직후 점프·Alt+Tab·긴 Pause 후 점프: **사용자 보고 기준 정상**. 세부 항목별 검증·래피드 트리거 원인 확정은 아님.
- 아래 N1 검토 절차는 재현용으로 유지한다. 현재 이동감 검토는 완료했으며 카메라는 S1-09 Slice에서 함께 검토한다.


**Player Movement Review — S1-03 / S1-04 / S1-05, 각각 DONE(사용자 수락)** (약 15~20분).
세 Task를 한 번에 플레이하되 각각 수락 또는 수정 의견을 기록한다. 모두 DONE이 되기 전 Camera와 본격 Room 작업은 시작하지 않는다.

- Scene: `Assets/Scenes/MovementTest.unity` (실행 방법은 [README](README.md#unity에서-실행)).
- GameObject: `Player`, `Floor`, `Low Platform`, `Middle Platform`, `High Platform`, `Left Wall`, `Right Wall`.
- Prefab: `Assets/Prefabs/Player.prefab`; PlayerMotor의 Tuning/Ground Layers, Rigidbody2D·BoxCollider2D 확인.
- 설정 원본: `Assets/ScriptableObjects/PlayerTuning.asset`.
- Inspector: Move Speed, Acceleration, Deceleration, Air Control, Jump Velocity, Gravity, Fall Gravity Multiplier, Max Fall Speed, Jump Cut Multiplier, Coyote Time, Jump Buffer. 초기값·탐색 범위는 GAME_DESIGN 22절을 따른다. N1은 초기값 그대로이며 후보 채택은 없다.

| 순서 | 조작 | 확인할 결과 |
| --- | --- | --- |
| 1 | 평지에서 A/D·방향키, 스틱·D-pad로 급반전·손 떼기·공중 방향 변경 | 반응·제동·공중 제어감 (S1-03) |
| 2 | 발판 끝에서 걸어 떨어진 직후/늦게 점프, 공중에서 벽에 붙기 | 짧은 이탈만 점프 허용, 벽을 바닥으로 오인하지 않음 (S1-03/04) |
| 3 | 착지 직전/충분히 일찍 점프 입력, 버튼 계속 홀드 | 직전 입력은 착지 시 1회, 이른 입력은 만료, 다음 착지에서 반복 안 함 (S1-04) |
| 4 | 짧게 누르기와 길게 누르기를 반복 | 높이 차이·낙하 속도·착지 제어감 (S1-05) |
| 5 | Esc/Menu Pause 후 Enter/Esc 또는 A/B 재개, 실제 패드 분리 | 재개 입력이 점프로 새지 않음, 분리 시 Pause·키보드 복구 (S1-02 회귀) |
| 6 | 이전 값을 적고 Inspector에서 한 값씩 변경·비교 | 실제 플레이 반영, Task별 수락/반려와 원하는 수치 기록 |

Play 중 설정 Asset 변경은 남을 수 있다. Stop 후 이전 값 또는 사용자 수락 값을 명시적으로 반영하고 저장한다. Scene 변경은 Stop 후 다시 입력한다. 후보 실험과 기본값 확정을 구분한다.

## Bugs

2026-09-30: 아래 2건을 수정하고 지정된 N1 자동 검사에서 재검증했다. 실제 패드·수동 화면/입력·감각 수락은 미실시이며 전체 게임 무오류를 뜻하지 않는다.

### BUG-001

- ID / 관련 Task / Status / 심각도: BUG-001 / S1-04·S1-05 / DONE(버그 합격 기준 충족, 이동감 Task는 REVIEW 유지) / Major.
- 빌드: Unity 6000.3.24f1, Input System 1.20.0, Fixed Delta 0.02s, N1 Windows x64 Mono 재빌드.
- 재현·원인: 입력 콜백의 프레임 시각과 FixedUpdate의 물리 시각 혼용. Dynamic 입력 처리 지연과 프레임 단위 홀드 예약으로 Coyote/Buffer 경계와 Jump Cut 시점이 달라짐.
- 기대·이전 실제: 동일 시계의 0.08s 허용/0.12s 거부 및 동일 0.10s 홀드의 높이 편차 ≤0.1u. 이전 Coyote 측정은 0.0800s 허용/0.1234s 거부. 이전 짧은 높이는 30/60/120fps에서 1.944/1.596/1.596u(홀드 길이 불일치), 편차 0.348u.
- 수정: ctx.time과 fixedUnscaledTime의 동일 이벤트 시계 사용. 미래 입력은 해당 물리 시각까지 대기, JumpSequence 단일 소비 유지. 실제 press/release 간격으로 cut 물리 틱을 계산하고 늦게 전달된 해제의 초과 상승 속도·변위를 Rigidbody2D에서 보정. Dynamic Update·Pause 재개 유지. PlayerTuning·게임 규칙 변경 없음.
- 재검증: 30/60/120fps 각각 Coyote 정확히 0.0800s 허용/0.1200s 거부, Buffer 착지 0.1000s 전 허용/0.1400s 전 만료. 실제 이벤트 홀드 0.100000s, 짧은 높이 모두 1.464u(편차 0.000u), 긴 높이 모두 2.520u. 벽 접지·중복 점프·홀드 재사용 없음. 이전의 홀드 조건이 달라 높이 절댓값 감소만으로 게임 감각 개선을 단정하지 않음.
- 증거: [S1-04 재검증](Validation/S1-04-rerun.txt), [S1-05 재검증](Validation/S1-05-rerun.txt), [이전 S1-05](Validation/S1-05-results.txt).

### BUG-002

- ID / 관련 Task / Status / 심각도: BUG-002 / S1-02 / DONE / Major.
- 빌드: 위와 같은 N1 재빌드.
- 재현·원인: Pause → Player 비활성화 → 재활성화. 이전 OnDisable은 timeScale만 복구해 Paused=true가 남고 Move=0, 입력 맵과 상태 불일치.
- 기대·이전 실제: 재활성화 뒤 Paused=false·GamepadDisconnected=false·timeScale=1·이동/점프 가능이어야 하나 이전 코드에서는 Pause 상태 잔존.
- 수정: OnDisable에서 두 플래그·임시 입력 초기화, OnEnable에서 상태와 Gameplay 맵 일치·timeScale 복구.
- 재검증: 기존 입력 검사 16건에 disable 플래그/시간 복원·이동·점프 3건 추가, 총 19건 통과. 이 버그의 수정 전 재현은 사용자 보고·기존 코드 분석이며 수정 전 자동 측정은 새로 실행하지 않음.
- 증거: [S1-02 재검증](Validation/S1-02-rerun.txt).

### CLEAN-001

- 관련 / Status: N1 일회성 도구 / DONE.
- 순서: N1Verification.Input 대상을 MovementTest로 변경한 뒤 Boot의 Player 프리팹 인스턴스와 루트 참조 제거. MovementSetup/InputSetup/ProjectSetup 및 각 .meta 삭제. N1Build/N1Verification 유지.
- 안전성: 초기값 덮어쓰기·빌드 씬 되돌림·컴포넌트 중복 생성 경로 제거. AGENTS Work Rule에 Git 이력의 생성 스크립트 복원/재실행 금지 추가.
- 검증: MovementTest 대상 S1-02/04/05 검증과 정리 후 N1 빌드 성공. 생성 도구 6파일 부재·Boot Player 참조 부재·보존 도구 존재·기존 PlayerTuning/Prefab/빌드 씬 설정 변경 없음 정적 확인. 같은 검증을 정리용으로 다시 반복하지 않음.

해결 기록: Unity 미설치(ENV-001), 검색 인덱스 초기화 전 Play 예외(TEST-001), 배치 입력 포커스(TEST-002), 가상 입력 시각·물리 틱·접지 시험 초기화(TEST-003)는 설치 또는 검증 절차 수정 후 재검증했다. 게임 규칙 변경으로 우회하지 않았다. 원시 시도 로그는 로컬 `Logs/*attempt.txt`에 보존되어 있다.

등록 양식: `ID / 관련 Task / Status / 심각도 / 빌드 / 재현 단계 / 기대·실제 / 수정·재검증 결과 / 증거`.
심각도: Blocker=크래시·진행 불가·저장 손실, Major=핵심 기능 오류, Minor=진행 가능한 표시·연출 문제.

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

## Decisions Needed

DEC-ROOM-B(2026-10-01) 사용자 승인: 블록 그레이박스로 S1-07 완료 기준을 대체, Tilemap DEFERRED. ROOM-001은 원인 미확정 OPEN 유지. Tilemap 도입은 사용자 Editor GUI 확인 후 별도 Task로 결정. S1-08 이후 미승인.

### S1-06 / N2-A (2026-09-30)

- Cinemachine 3.1.7: 공식 registry의 최소 Editor 2022.3, 설치된 package.json 3.1.7, manifest/lock 고정. 6000.3.24f1에서 resolve·컴파일·Play 성공.
- 기술 검증: [S1-06 결과](Validation/S1-06-N2-A.txt). 1280×720·1920×1080 렌더와 실제 뷰포트 경계/낙하 시야 검사, 급반전·즉시 카메라 재배치 확인. 사용자 감각 수락은 S1-09 예정.
- 초기 오류: CameraState API·ManualUpdate 모드 조건 수정 후 성공. 로컬 `Logs/S1-06-author.log`, `S1-06-verify.log` 실패 기록 보존; 최종 `Logs/S1-06-verify-snap.log`. 생성에 사용한 임시 작성 스크립트는 삭제해 재실행/덮어쓰기 경로를 남기지 않음.
## Validation Records

아래 기존 실제 결과는 보존한다. 2026-09-30 지정된 S1-02/04/05와 N1 빌드만 재실행했고 새 파일에 기록했다. S1-01·S1-03 단독 검증은 반복하지 않았다.
검증 환경: Windows 11, Unity **6000.3.24f1 (4e7b9b5b6244)**, Input System **1.20.0**, 고정 물리 간격 **0.02초**. Hub 3.21.3 설치 확인 완료. 아래 N1 검증 당시 Cinemachine은 미설치였으며, N2-A에서 3.1.7 설치·호환·Camera 기술 검증을 완료했다.

| 검증 | 실제 결과 | 증거 |
| --- | --- | --- |
| S1-01 빈 씬 Play·Windows 빌드 | Play 진입→2초 실행→Edit 복귀; Succeeded, 오류/경고 0 | [Play](Validation/S1-01-play.txt), [Build](Validation/S1-01-build.txt) |
| S1-02 입력·장치 표시 회귀 | 키보드·가상 패드·맵 분리·Pause·연결 해제/복구·홀드, 16개 통과 | [결과](Validation/S1-02-results.txt) |
| S1-03 기본 이동 | 입력 포함 27개 통과, 벽 접지·벽 점프 없음, 급반전·공중 제어·착지·Inspector 적용 | [결과](Validation/S1-03-results.txt) |
| S1-04 입력 보정 | Coyote 경계·착지 Buffer·중복 소비 검사 통과 | [결과](Validation/S1-04-results.txt) |
| S1-05 최종 이동 회귀 | 입력·이동·보정·가변 점프·하강·Inspector, 총 96개 assertion 통과 | [결과](Validation/S1-05-results.txt) |
| N1 Windows x64 Mono 빌드 | Succeeded, 오류/경고 0, Development Build=false | [결과](Validation/N1-build.txt), 로컬 `Builds/N1/Afterglow.exe` |
| 빌드 기동·표시 | 기존 로그상 창 생성·렌더러/입력 초기화 기록. 화면 수동 확인 미실시(정적 표시를 확인했다는 파일 증거 없음) | 로컬 `Player.log`; 수동 이동·Pause 결과 확인은 미완료 |

### 2026-09-30 지정 재검증

| 검증 | 실제 결과 | 증거 |
| --- | --- | --- |
| S1-02 | 기존 입력 + Pause 비활성화/재활성화, 19건 통과 | [새 결과](Validation/S1-02-rerun.txt) |
| S1-04 | 30/60/120fps Coyote 0.08 허용/0.12 거부, Buffer 0.10 허용/0.14 만료, 63건 통과 | [새 결과](Validation/S1-04-rerun.txt) |
| S1-05 | 동일 0.100000s 홀드, 짧은 높이 모두 1.464u·편차 0.000u, 긴 높이 2.520u, 127건 통과 | [새 결과](Validation/S1-05-rerun.txt) |
| N1 Windows x64 Mono | Succeeded, errors=0, warnings=0, Development Build=false | [새 결과](Validation/N1-build-rerun.txt) |

위 검증은 Unity 가상 Keyboard/Gamepad 자동 검사이며 실제 패드·수동 입력·화면 검수나 재미 수락을 대체하지 않는다. 원시 로그는 로컬 `Logs/S1-02-rerun-authorized.log`, `S1-04-rerun.log`, `S1-05-rerun.log`, `N1-build-rerun.log`. 최초 sandbox 실행은 라이선스 접근 실패로 검사를 시작하지 못했고 권한이 있는 실행에서 성공했다. 이전 Validation 파일은 덮어쓰지 않았다.

이하 수치는 **수정 전 기록**이다.

최종 측정: Coyote 이탈 후 **0.0800초 허용 / 0.1234초 거부**. Buffer는 착지 점프 **0.0999초 전 허용 / 약 0.147초 전 만료**, 홀드 재사용 없음. 평균 렌더 **30.0 / 60.0 / 120.0fps**에서 관련 검사 통과.
긴 점프는 모두 **2.520u**, 짧은 점프는 각 **1.944 / 1.596 / 1.596u**. 프레임 단위 입력 예약으로 짧은 홀드의 실제 길이가 달라 **동일 입력의 FPS 간 궤적 일치 증거는 아니다**.
하강 가속도 **45u/s²**, 상한 **18u/s**. 복제 설정의 상한을 16으로 변경하면 다음 물리 틱에 반영되었으며 원본 초기값은 보존했다.

원시 로그: 로컬 `Logs/S1-01-final.log`, `S1-02-final.log`, `S1-03-verify.log`, `S1-04-final.log`, `S1-05-final.log`, `N1-build.log`. 최종 입력·이동·빌드 로그에서 C# 컴파일 오류·Exception 없음. Player 로그는 `%USERPROFILE%/AppData/LocalLow/Afterglow/Afterglow/Player.log`. Logs와 Builds는 Git 제외이며 요약 증거는 위 Validation 파일로 보존한다.

**미실시·한계:** 물리 게임패드 조작·탈착, 사용자 이동감/재미 수락, 빌드 수동 입력·Pause 동작 확인, 해상도별 검수. 화면 수동 확인 미실시. 기존 정적 화면 확인 주장은 파일 증거가 없어 삭제했다. 미구현인 카메라·방 전환·사망/리스폰과의 실제 통합은 검증하지 않았다.

### 2026-10-01 DEC-ROOM-B — 블록 Room 및 입력 회귀

| 검증 | 실제 결과 | 새 증거 |
| --- | --- | --- |
| S1-07 전체 Room | 71건 PASS. A01→A02→A03→A04→A03→A02→A01 물리 출입구 왕복, FromLeft/FromRight 지정 Spawn 유지·접지·단일 플레이어/GameState·이전 씬 정리·Polygon 경계. 잘못된 씬/Spawn/Room ID 롤백·Pause 보호·전환 입력 누수 없음·새 점프 1회. Ground/Hazard 블록 규약 확인 | [최종 결과](Validation/S1-07-N2-A-blocks-final.txt) |
| S1-02 회귀 | 19건 PASS. 입력·Pause·패드 분리·비활성화/재활성화 후 이동·점프·timeScale=1 | [결과](Validation/S1-02-N2-A-blocks.txt) |
| S1-04 회귀 | 63건 PASS. 30/60/120fps Coyote 0.08 허용/0.12 거부, Buffer 0.10 허용/0.14 만료·중복 소비/홀드 재사용 없음 | [결과](Validation/S1-04-N2-A-blocks.txt) |

컴파일·런타임 Error/Exception 검사 통과. Unity 6000.3.24f1, 가상 입력 자동 검사이며 실제 패드·사용자 화면/감각 검토는 미실시. 이번 범위의 빌드·S1-01/03/05 단독 검사는 실행하지 않음. Hazard는 위치·트리거 표시만 구성했고 사망/체크포인트 동작은 S1-08 미승인으로 미구현. 원시 로그는 로컬 Logs/S1-07-blocks-final.log, Logs/S1-02-blocks-regression.log, Logs/S1-04-blocks-regression.log. 이전 Validation은 보존.

## Playtest Records

관찰 플레이테스트 기록: 아직 없음. 위 기술 검증과 사용자 감각 검토를 구분한다. 다음 양식은 빈 서식이며 실시 증거가 아니다.

`Test ID / 일시 / 빌드·수치 버전 / 참가자 코드·숙련도 / 입력 장치 / 전체 활동 시간 / A·B·C 시간 / 방별 사망·반복 사망 / 길 잃음 위치 / 능력 후 첫 행동 / 게이트 기억·복귀 / 보스 재시도 / 방향 오판 / 관찰 메모 / 증거 위치`

## Design Decision / Before-After

### CASE-001 — 입력 시각 일치 / 첫 사례 후보 (사용자 미수락)

- 관련: BUG-001, S1-04/05. 의도는 경계 입력의 일관성과 FPS별 동일 홀드 조작감 유지.
- 초기 구현 문제: 프레임 시각/물리 시각 혼용과 프레임 단위 홀드 도구. 수정 전 Coyote 0.0800s 허용/0.1234s 거부, 짧은 점프 1.944/1.596/1.596u(편차 0.348u, 동일 홀드 아님).
- 가설·수정: 공통 이벤트 시계와 실제 해제 시간 기반 Cut 보정, 테스트는 동일 0.10s 이벤트 홀드. 수치 튜닝 변경 없음.
- 이후: Coyote 30/60/120fps 모두 0.0800s 허용/0.1200s 거부. 짧은 점프 1.464/1.464/1.464u(편차 0.000u), 긴 점프 2.520u 유지.
- 판단·한계: 기술 합격 기준은 충족. 기존 홀드 시간이 달랐으므로 전후 높이는 동일 조건 A/B 실험이 아니며, 재미·체감 개선은 사용자 검토 전인 후보다. 기본값 채택·게임 튜닝 개선 확정이 아니다.
- 증거: [이전 결과](Validation/S1-05-results.txt), [새 경계 결과](Validation/S1-04-rerun.txt), [새 높이 결과](Validation/S1-05-rerun.txt). 다음 사용자 검토에서 세 이동 Task를 개별 수락/반려한다.

`Decision ID / 관련 Task·방·시스템 / 기획 의도 / 초기 값·구현 / Test ID·문제 근거 / 수정 가설 / 승인자·일시 / 변경 값·파일 / 동일 구간 재검증 / 실제 결과·한계 / 유지·되돌림 / 반영 문서 / 증거 위치`

## Resource Register

`리소스 / 출처 URL 또는 직접 제작 / 라이선스·확인일 / 사용 위치 / 표기 의무 / 실제 표기 위치`

2026-09-17: `Assets/Art/Block.png`는 직접 생성한 2×2 흰색 도형 텍스처로 플레이어·발판에 재사용한다. 외부 아트·오디오 및 외부 리소스 표기 의무 없음.

## Latest Handoff

2026-10-01 — DEC-ROOM-B 승인 범위 완료.

- **완료·상태:** S1-07 DONE, S1-07-T Tilemap DEFERRED. ROOM-001 OPEN(원인 미확정, 배치 Tile 에셋 생성 실패 savedTile=False, 대안 B 우회 승인). S1-03/04/05 DONE·S1-06 REVIEW 유지. S1-08 이후 미착수.
- **주요 파일:** A01~A04 Scene, RoomBlock Prefab, RoomSession/Definition/Exit/Spawn, GameState, PlayerInputReader, N1/N2Verification. PRD Technical Boundary·S1-07 DoD에 DEC-ROOM-B 반영. RoomMinimalTemp 씬/Tile와 .meta 삭제, tilemap 모듈 유지. 임시 블록 작성 도구 삭제, 로컬 Logs 사본 보존.
- **실제 검증:** [Room](Validation/S1-07-N2-A-blocks-final.txt) 71건, [S1-02](Validation/S1-02-N2-A-blocks.txt) 19건, [S1-04](Validation/S1-04-N2-A-blocks.txt) 63건 PASS. 6방향 출입구·지정 Spawn·안전 도착·단일 플레이어/상태·오류 ID 롤백·Pause 보호·점프 입력 초기화·카메라 Polygon 경계, 30/60/120fps 입력 경계 유지. Error/Exception 없음. 실제 패드·수동 화면·감각 검토·새 빌드는 미실시.
- **수정 위치·사용자 확인:** Unity에서 A01을 열고 Play하면 단일 세션/플레이어가 자동 생성됨. 각 방 root의 RoomDefinition, Exit Left/Right의 도착 Scene·Room·Spawn ID, Spawn Entry/FromLeft/FromRight의 위치를 Inspector에서 조정. Ground/Left Wall/Right Wall/Hazard 블록의 Transform·BoxCollider2D·SpriteRenderer 편집, 공통 원본은 RoomBlock Prefab. Camera Boundary의 Polygon과 Main Camera > RoomCameraRig 설정 편집. Scene의 Play 중 변경은 Stop 후 다시 입력·저장, 공통 Prefab 변경은 원본을 편집하거나 Overrides Apply로 반영. MovementTest는 회귀용으로 보존.
- **문제·한계:** 초기 블록 전환 검사에서 비활성 Rigidbody의 Transform 잔류 및 방 Start의 Entry 재배치를 수정 후 최종 왕복 통과. 실패/중간 증거 보존. Hazard는 트리거 표시만 있고 사망 동작은 아직 없음. Tilemap 조사는 종료, 도입은 사용자 Editor GUI 확인 후 별도 Task 결정.
- **다음 작업:** N2-B(S1-08 Checkpoint → S1-09 Slice → S1-10 빌드)는 S1-07 DONE·S1-06 REVIEW를 선행 상태로 준비됨. 별도 사용자 승인 전 실행하지 않음. 사용자 기존 PackageManagerSettings.asset 미추적 파일 보존.
