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
S1-01~05 **DONE**. 2026-09-30 사용자가 Editor·N1 빌드에서 Movement Review를 수락했다. N2-A(S1-06 Camera·S1-07 Room Structure) 승인, S1-08 이후와 Sprint 2는 미승인이다.

일정 지연에 따라 오늘부터 다시 진행한다. 당장 수행할 작업량·단기 목표·Sprint 종료일·최종 완료일은 아직 정하지 않는다. 사용자가 토큰 사용량을 확인하고 업그레이드 여부를 판단한 뒤 진행 규모를 정한다. 기존 Roadmap은 순서와 범위의 참고로 유지하며 이번 일정 갱신으로 새 구현 작업을 승인하거나 기존 작업을 재실행하지 않는다.

## Current Tasks

2026-09-30 승인 범위: Movement Review 문서 기록 → S1-06 → S1-07(N2-A). 각 단계 검증·별도 커밋·기존 origin/main push. S1-08 이후는 실행하지 않는다.

상태: TODO(미착수), DOING(진행), REVIEW(기술 구현·관련 검증 완료, 사용자 판단 대기), DONE(완료 조건·필요한 사용자 수락 충족), BLOCKED(차단), DEFERRED(승인된 제외).
의존성 `:REVIEW`는 유효한 기술 검증을 갖춘 REVIEW 또는 DONE에서 충족한다. `:DONE`은 DONE만 허용하며 생략 시에도 DONE으로 해석한다. DEFERRED는 의존성을 자동 충족하지 않는다. 실행에는 별도 범위 승인이 필요하다.

| ID | Task | Status | Dependency | User Review | Done Criteria |
| --- | --- | --- | --- | --- | --- |
| S1-01 | Unity Project Setup | DONE | 없음 | — | 정확한 Editor·패키지 고정, 기본 폴더·Boot/Menu 구성, 빈 씬 Play·Windows 빌드 성공, 실행 안내 |
| S1-02 | Input System | DONE | S1-01:DONE | — | 키보드·패드, Gameplay/UI 분리, 전환·해제 Pause·재개 시 입력 유출 없음 |
| S1-03 | 좌우 이동·기본 점프·낙하 | DONE | S1-02:DONE | 가감속·급반전·공중 제어 | Rigidbody2D·PlayerTuning·Prefab·단순 검증 공간, 벽 접지 없음, Inspector 반영, 기본 조작 수락 |
| S1-04 | Coyote Time / Jump Buffer | DONE | S1-03:REVIEW | 발판 끝·착지 직전 입력 | 경계 안팎 허용·만료, 단일 소비·전환 초기화, 입력 보정 수락 |
| S1-05 | Variable Jump Height | DONE | S1-04:REVIEW | 높이 차이·낙하감 | Jump Cut·하강 배율·최대 속도 조정 가능, 짧은/긴 점프 구분, 30/60/120fps 확인·사용자 수락 |
| S1-06 | Camera | TODO | S1-03:DONE, S1-04:DONE, S1-05:DONE | 급반전·낙하 시야 | Cinemachine 추적·방 경계, 떨림·시야 밖 필수 착지 없음, 설정 안내 |
| S1-07 | Basic Tilemap / Room Structure | TODO | S1-01:DONE, S1-03:DONE, S1-04:DONE, S1-05:DONE | — | Ground/Hazard/출입구·Spawn ID, A01~A04 양방향 연결, 플레이어 중복·벽 끼임 없음, 배치 편집 가능 |
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

## Decisions Needed

현재 없음. 기존 push 차단은 사용자 승인 후 원격 main=a967297로 동기화하여 해소했다. N2-A 승인됨. 수치 변경 없음. S1-09에서 키보드 점프 체감을 패드 A와 비교해 재확인하며 N2-B는 아직 미승인이다.

## Validation Records

아래 기존 실제 결과는 보존한다. 2026-09-30 지정된 S1-02/04/05와 N1 빌드만 재실행했고 새 파일에 기록했다. S1-01·S1-03 단독 검증은 반복하지 않았다.
검증 환경: Windows 11, Unity **6000.3.24f1 (4e7b9b5b6244)**, Input System **1.20.0**, 고정 물리 간격 **0.02초**. Hub 3.21.3 설치 확인 완료. Cinemachine 3.1.7은 호환 버전 선정만 했으며 미설치다.

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

2026-09-30 — Movement Review 사용자 수락 기록.

- **완료·상태:** S1-03/04/05 DONE. Editor·N1 빌드 확인을 사용자 보고로 기록. N2-A 승인, S1-06/07 아직 미착수.
- **주요 파일:** TASKS만 변경. PlayerTuning 값 변경 없음, 기존 초기값을 사용자 수락값으로 기록.
- **검증:** 문서 상태·수락 기록 정적 확인. 새 게임 테스트 미실행. 시간 기준 점프 정상은 사용자 보고이며 세부 시험 결과로 확장하지 않음.
- **사용자 확인:** 점프 거리 체감 메모는 S1-09 패드 A 비교 대상으로 유지. 카메라 사용자 검토도 S1-09 예정.
- **문제:** 기존 push 차단 해소·원격 반영 완료. 확인되지 않은 입력 장치 원인을 확정하지 않음.
- **다음 작업:** 문서 커밋 push 후 S1-06 Camera. N2-B(S1-08→S1-09→S1-10)는 N2-A 완료·인계 이후 별도 승인 필요.
