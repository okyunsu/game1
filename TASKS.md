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

**Sprint 3**, Sprint 2 Gate 사용자 통과·진입 기준일 **2026-10-03**. 기존 이력: 2026-09-16 최초 착수, 2026-09-17 N1 기술 구현·Unity 검증 완료. 설계 기준 v0.2 유지.
S1-01~05·07·08 DONE, S1-06 DONE, S1-09 DONE(임시 그레이박스 v1 사용자 수락), S1-10 REVIEW. 야간 DEC-S2-PRE 예외로 T1~T3 전용 테스트 씬 구현 REVIEW, T4 E1 CombatTest 한정 REVIEW. Tilemap DEFERRED·ROOM-001 OPEN. S1-G DONE(사용자 조건부 통과).

일정 지연에 따라 오늘부터 다시 진행한다. 당장 수행할 작업량·단기 목표·Sprint 종료일·최종 완료일은 아직 정하지 않는다. 사용자가 토큰 사용량을 확인하고 업그레이드 여부를 판단한 뒤 진행 규모를 정한다. 기존 Roadmap은 순서와 범위의 참고로 유지하며 이번 일정 갱신으로 새 구현 작업을 승인하거나 기존 작업을 재실행하지 않는다.

## Current Tasks

2026-10-03 G2: S2-day exe 사용자 확인으로 S2-01~06·K1·S1-10·T1~T4·점프 일관성 DONE, Sprint 2 Gate 통과. 현재 Target 15방(A06/B06/C05 보류). 승인 실행 P0→G2→P3→S3-01~05→최종 빌드. P0 GitHub 연결 실패로 로컬 커밋만 진행, 최종 push 생략.

2026-10-03 실행 결과: P0 push 성공. K1 PASS/REVIEW, K2 2회 실패 후 복원/BLOCKED, K3 PASS/S2-03 REVIEW, S2-day 빌드/실행 PASS. 최종 push는 인계 커밋 후 실행. 기존 배치·튜닝 보존, B01 지정 셸 외 콘텐츠 미승인.

2026-10-03 야간 종료: G0·P0 완료, S2-01/02/04 REVIEW, S2-03 BLOCKED(두 번 실패·전체 복원). 최종 Menu+A01~A05 빌드/실행 PASS. B01·격자 통과·더블 점프·후속 방·보스·아트 미포함, 기존 배치/튜닝 보존, push 없음.

2026-10-02 G0 사용자 결정: Sprint 1 Gate 조건부 통과(S1-G DONE). S1-01~09 DONE, S1-10 빌드·exe 기술 PASS, T1~T4 테스트 씬 기술 PASS가 근거. S1-10 REVIEW 유지. 이월 체감 검토: S1-10 exe 플레이, 키보드/패드 점프 일관성, T1 대시 감각, T2 피격·넉백, T3 공격 판정, T4 E1. 다음 통합 빌드에서 사용자 확인. 승인 범위 G0→P0→S2-01~04→성공 단계 최종 빌드만, 기존 배치 보존·push 금지. B01 내부 콘텐츠·더블 점프·B02 이후·보스·아트 미승인.

이전 S1-10 실행 기록(2026-10-02): S1-06 DONE — CAM-001 수평 선행 0u 적용 후 직접 플레이에서 카메라 문제 없음. S1-09 DONE — A01→A04 테스트용 이동 경로로 수락. 현재 배치는 임시 그레이박스 v1, 이후 실제 진행 때 수정 예정. C3 자동 왕복 FAIL은 자동 입력 방식의 한계로 기록하며 사용자 직접 왕복 성공 보고를 근거로 레이아웃 결함으로 보지 않는다. 자동 실패 증거는 보존한다. 이번 승인 범위는 S1-10 빌드·exe 검증만이며 S1-G TODO 유지, Gate 판단·레벨/T1~T4 수정·Sprint 2 미실시, 로컬 커밋만.

2026-10-02 승인 C0~C4만 완료: 사용자 A01 보존, 카메라 lead0, A02~A04 지정 좌표 배치, C3 보고 전용 검증, CombatTest E1 재시도. push·S1-10 빌드·S1-G·Sprint 2 콘텐츠 미실시.

2026-10-01 야간 승인 DEC-S2-PRE: T0 재설정 후 T1 Dash, T2 Health, T3 Attack(T2 성공), T4 E1(T2·T3 성공)를 순차 검증·로컬 커밋. push 없음. Sprint 1 Gate 전 시스템 선행은 전용 테스트 씬에만 허용하며 T0 이외 A01~A04 통합 금지.

상태: TODO(미착수), DOING(진행), REVIEW(기술 구현·관련 검증 완료, 사용자 판단 대기), DONE(완료 조건·필요한 사용자 수락 충족), BLOCKED(차단), DEFERRED(승인된 제외).
의존성 `:REVIEW`는 유효한 기술 검증을 갖춘 REVIEW 또는 DONE에서 충족한다. `:DONE`은 DONE만 허용하며 생략 시에도 DONE으로 해석한다. DEFERRED는 의존성을 자동 충족하지 않는다. 실행에는 별도 범위 승인이 필요하다.

| ID | Task | Status | Dependency | User Review | Done Criteria |
| --- | --- | --- | --- | --- | --- |
| S1-01 | Unity Project Setup | DONE | 없음 | — | 정확한 Editor·패키지 고정, 기본 폴더·Boot/Menu 구성, 빈 씬 Play·Windows 빌드 성공, 실행 안내 |
| S1-02 | Input System | DONE | S1-01:DONE | — | 키보드·패드, Gameplay/UI 분리, 전환·해제 Pause·재개 시 입력 유출 없음 |
| S1-03 | 좌우 이동·기본 점프·낙하 | DONE | S1-02:DONE | 가감속·급반전·공중 제어 | Rigidbody2D·PlayerTuning·Prefab·단순 검증 공간, 벽 접지 없음, Inspector 반영, 기본 조작 수락 |
| S1-04 | Coyote Time / Jump Buffer | DONE | S1-03:REVIEW | 발판 끝·착지 직전 입력 | 경계 안팎 허용·만료, 단일 소비·전환 초기화, 입력 보정 수락 |
| S1-05 | Variable Jump Height | DONE | S1-04:REVIEW | 높이 차이·낙하감 | Jump Cut·하강 배율·최대 속도 조정 가능, 짧은/긴 점프 구분, 30/60/120fps 확인·사용자 수락 |
| S1-06 | Camera | DONE | S1-03:DONE, S1-04:DONE, S1-05:DONE | 급반전·낙하 시야 | Cinemachine 추적·방 경계, 떨림·시야 밖 필수 착지 없음, 설정 안내 |
| S1-07 | Block Greybox / Room Structure | DONE | S1-01:DONE, S1-03:DONE, S1-04:DONE, S1-05:DONE | — | DEC-SLICE-USER 빈 방 기준 양방향·안전 도착·단일 플레이어·입력 초기화 PASS 이력. C3 자동 입력 한계는 사용자 직접 왕복 성공 보고로 레이아웃 결함으로 보지 않음. Tilemap DEFERRED |
| S1-07-T | Tilemap 도입 | DEFERRED | 별도 사용자 결정 | Editor GUI 확인 | DEC-ROOM-B: 이번 Sprint 제외, 사용자 GUI 확인 후 별도 Task 결정 |
| S1-08 | Checkpoint / 최소 사망 복귀 | DONE | S1-05:REVIEW, S1-07:DONE | — | CP-A01/A03·Kill Zone·0.6초 안전 복귀·사망 중 입력/전환 차단·3초 내 조작, 능력 없는 초기 상태 검증; HP/디스크 저장은 Sprint 2 |
| S1-09 | 첫 Vertical Slice 통합 | DONE | S1-06:REVIEW, S1-08:DONE | 사용자 직접 배치 | A01 사용자 배치 v1·A02~A04 사용자 설계 좌표 v1: 테스트용 이동 경로로 사용자 수락(2026-10-02). 임시 그레이박스이며 실제 진행 때 수정 예정 |
| S1-10 | Slice 검증·실행 안내 | DONE | S1-09:REVIEW | — | 그레이박스 v1 Windows x64 4방 빌드·exe 전환/복귀 PASS. 사용자 exe 실행 5분 확인 대기 |
| S1-G | Sprint 1 Gate | DONE | 2026-10-02 사용자 조건부 결정 | 이동·Slice 수락과 다음 범위 판단 | 활성 검토 항목 수락, 문제·잔여 일정 확인, 사용자 통과 결정 기록; 통과 전 Sprint 2 착수 금지 |

### 야간 시스템 작업 — DEC-S2-PRE

| ID | Task | Status | Dependency | User Review | Done Criteria |
| --- | --- | --- | --- | --- | --- |
| T1 | Dash / DashTest | DONE | DEC-S2-PRE | 거리·방향·공중 대시 감각 | 18u/s·0.16s·0.45s, 권한·벽·입력 차단·취소·30/60/120fps |
| T2 | Health / CombatTest | DONE | DEC-S2-PRE | HP·무적·넉백·복귀 체감 | HP5·피해1·무적1s·넉백4/3·lock0.12s, 사망 복귀·KillZone 즉사 |
| T3 | 기본 공격 / CombatTest | DONE | T2:REVIEW | 판정·타이밍·이동 유지 | J/X, 피해1·range1.1·height1.2·startup0.08·active0.10·cooldown0.35·타겟당1회·방향 고정·취소 |
| T4 | E1 순찰형 / CombatTest | DONE | T2:REVIEW, T3:REVIEW | 순찰·접촉·피격 감각 | C4 재시도 PASS, HP2·이동1.8·접촉1·넉백3, 벽/낭떠러지·2타 제거·사망/재입장 초기화 |
### Sprint 2 승인 작업 — 2026-10-02

| ID | Task | Status | Dependency | User Review / Done Criteria |
| --- | --- | --- | --- | --- |
| S2-01 | 실제 방 Dash·HP·Attack·E1 통합 | DONE | S1-G:DONE | Player 기본 Dash false, A04 Arena E1 1개, 공격·HP·사망·방 전환·T1~T4·필수 회귀 PASS 후 REVIEW |
| S2-02 | A05 대시 획득·안전 연습 | DONE | S2-01:REVIEW | 지정 36×14 셸·A04 연결·획득 x10·5u 간격 연습(2026-10-03 사용자 Dash 필요/간격 확인 완료), 획득/재입장/사망 유지·지정 점프 측정 PASS 후 REVIEW |
| S2-03 | A03 G-D·B01 자리 | DONE | S2-02:REVIEW | 보유+대시 중만 격자 양방향 통과, 일반 벽 유지·선반 출구·B01 CP와 안내만, 회귀 PASS 후 REVIEW |
| S2-04 | 단일 슬롯 저장·Menu 이어하기 | DONE | S2-02:REVIEW | 능력/마지막 CP만 저장, Menu 새 게임/이어하기·손상 안전 처리·종료/재개 검사 PASS 후 REVIEW |
| S2-05 | Core Loop 통합 관찰·재방문 검증 | DONE | 사용자 결정 G2 | S2-day 사용자 플레이로 대체, 이월 T1~T4·점프 일관성 수락 |
| S2-06 | Sprint 2 빌드·Gate 판단 | DONE | 사용자 결정 G2 | S2-day exe 플레이로 대체; Sprint 2 Gate 통과(2026-10-03) |

이번 구현은 표의 S2-01~04와 성공 단계 최종 빌드만 승인. 향후 Sprint 2 로드맵 범위는 제안 TODO이며 콘텐츠 추가를 승인하지 않음. 기존 블록·스폰·CP·문 위치 보존, 새 방/출구/스폰만 지정 범위 추가. 실패 1회 수정 후 재실패 시 단계 복원·의존 단계 건너뜀. 회귀는 S1-02/04/07/08, 자동 이동 휴리스틱 사용 금지.
### Sprint 3 승인 작업 — 2026-10-03 (P3)

승인 원문: [S3 실행 명세](Validation/S3-approved-request.txt). 이번 범위는 S3-01~05와 성공 단계 Windows x64 빌드만. C지역·보스·엔딩·선택 방·아트·K2·A01~A05 배치 변경 없음. 단계별 구현→Unity 검증→S1-02/04/07/08 회귀→로컬 커밋. 실패 1회 수정 후 재실패는 해당 단계 복원·실패 기록 커밋, 의존 단계 건너뜀. Validation 기존 파일 덮어쓰기 금지.

| ID | Task | Status | Dependency | User Review / Done Criteria |
| --- | --- | --- | --- | --- |
| S3-01 | 더블 점프 시스템 | TODO | G2:DONE | Player 기본 보유 false, DoubleJumpTest true/3.6u 턱; 추가 속도12·재입력·접지회복·Coyote 후1회·대시/전환 회복 없음·저장. 30/60/120fps 일반/더블 최대 발 높이 및 G-J 기준 MEASURE, 3번째/홀드/미보유 차단 |
| S3-02 | E2 돌진형 적 | TODO | G2:DONE | CombatTest 1개, E2Tuning HP3/감지5u/예고0.6s/돌진0.4s·6u/s/회복1s/피해1/넉백3u/s. 방향고정·벽정지·3타·무적·대시 거리회피·사망/재입장 초기화 |
| S3-03 | B01 G-J 턱·B02 | TODO | S3-01:REVIEW, S3-02:REVIEW | B01 기존 보존, 턱(30.5,4.1)/(9,1), 위출구(34.25,5.6) B05/FromLeft, 아래(34.25,2) B02/FromLeft, FromUpper(28,5.41)/FromRight(32.5,1.81). B02 36×14 셸·SafeStep(11,2)/(4,2)·E2 x24 왼쪽. 일반/대시 우회불가·더블 도달·왕복·CP-B01 복귀 |
| S3-04 | E3 사격형·B03 | TODO | S3-03:REVIEW | E3Tuning HP2/이동0/감지7u/예고0.7s/주기2s/탄속5u/s/피해1. 고정 방향·벽뒤/화면밖 발사금지·탄 벽/전환 제거. CombatTest 1개. B03 Cover(14,2)/(1,2), TurretBase(26,2)/(4,2), 위 E3 왼쪽. 타이밍·엄폐·2타·피해/무적·B02 왕복 |
| S3-05 | B04 획득·연습·B05 자리 | TODO | S3-01:REVIEW, S3-03:REVIEW | B04 x10 접촉 획득·3초 안내·CP 보존·이미 보유 미등장·저장. Low(18,2)/(3,2), High(25,3.3)/(4,1) 일반 불가/더블 가능 MEASURE. B05 Left(1.75,6) B01/FromUpper, 바닥 FromLeft(3.5,1.81), 이름/다음 Sprint 안내만. B01↔B05·사망/종료/Continue 보유 유지 |
| S3-06 (제안) | C01~C04 | TODO | 별도 사용자 승인 | 이번 실행 미승인 |
| S3-07 (제안) | C06 보스 | TODO | 별도 사용자 승인 | 이번 실행 미승인 |
| S3-08 (제안) | 엔딩 | TODO | 별도 사용자 승인 | 이번 실행 미승인 |
| S3-09 (제안) | Sprint 3 Gate | TODO | 별도 사용자 승인 | Content Complete 검증·사용자 판단, 이번 실행 미승인 |

새 방 공통 셸 36×14u/바닥윗면y1/좌우벽/경계/카메라5.5·0·0.15/방이름/Door 표시/트리거1×2/바닥스폰y1.81. B02~B04 왼쪽출구(1.75,2), 양쪽 있는 방 오른쪽(34.25,2), FromLeft(3.5,1.81)/FromRight(32.5,1.81). G-J 높이는 일반최대+0.8以上/더블최대−0.5以下 측정 후 조건 불충족 시 범위 안 가장 가까운 값만 적용·결정 기록. S3-05의 B03 연결은 S3-04 실패 시 활성화하지 않고 독립 구현 가능한 부분만 보수적으로 처리한다.

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

### S2-03-001 — 격자 재시도 2회 실패 / 구현 복원 (2026-10-03)

- 첫 S2-03-gate.txt: 미보유/보유 걷기·점프·넉백 차단, 대시 양방향 PASS 뒤 일반 벽 시험이 기존 오른쪽 출구를 밟아 B01 출구 찾기 실패. 해당 최초 벽 PASS는 방 전환으로 오염된 결과여서 무효.
- 한 번 허용된 수정: 벽 시험 위치를 출구 위 y10으로 이동, 방 유지 검사 추가, 재입장 시 격자 참조 재획득. S2-03-gate-rerun.txt에서도 일반 벽 검사 실패. 현재 상태·공중 사용권/낙하/출구 간섭 가능성을 더 조사하지 않고 중단. 일반 벽 관통 버그로 확정하지 않음.
- S2-03의 PlayerDash 변경·A03 추가·B01·DashGate·씬 목록·검사 확장을 모두 S2-02 기준으로 복원/삭제. 격자 통과 기능·B01은 배포하지 않음. 원래 A03 격자와 기존 배치 유지.
- S2-01·02 커밋 보존. S2-04는 S2-02에만 의존하므로 독립 진행 가능. 2026-10-03 K3 사용자 승인 재개, 지정 출구 배치에서 격자·왕복·복귀 PASS. 첫/두 번째 실패 증거 보존, 복원 회귀 S1-02/04/07/08-night-S2-03-rollback.txt.


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

### RESET-001 — 첫 재설정 실패 (3d84246)

Sliced + BoxCollider2D.autoTiling에서 SpriteRenderer 크기 변경 후 콜라이더 불일치. 최초 API 컴파일 오류 수정 후 두 번째 검증 실패로 변경 복원. 이번 T0는 Simple·1×1u·Transform Scale 방식 사용. 증거: Validation/S1-09-reset-first-failure.txt, S1-09-reset-prefabs.txt, S1-09-reset-stopped.txt.

### SLICE-001

- ID / 관련 Task / Status / 심각도 / 환경: SLICE-001 / S1-07·09·10 / OPEN / Major / Unity 6000.3.24f1 Windows x64 Mono Slice 실행 파일, 가상 Keyboard 자동 검사.
- 재현: A01→A04 이동 후 A04·A03 역방향 통과, A02 FromRight 도착 후 마지막 발판 중심 x=40.00으로 걷기. 첫 전체 검사에서 실패; 재빌드 후 A02 역방향 집중 검사에서도 같은 실패.
- 실제 측정: 집중 검사에서 최종 위치 (42.37, 54.61), 속도 (0,0), 입력 해제 후 Move=(0,0), Paused=False·Transition=False. 목표와 x 차이 2.37u. 이 값만으로 입력 또는 충돌 원인을 확정하지 않는다.
- 조사·변경: 자동 검사에 위치/속도/입력 진단 추가, Unity 앱 내부의 실제 입력 장치를 격리(검사 종료 시 복원)한 뒤 새 빌드로 재현. 정상 플레이에는 CLI 검사를 실행하지 않는다. 입력 격리 후에도 실패하여 원인 미확정. 게임 규칙·수치 변경 없음.
- 검증: 빌드 2회 Succeeded, errors=0·warnings=0. 첫 런타임 검사에서 순방향 A04 도착 약 367.92초, A04 복귀·A03 역방향 완료 약 593.08초. 체크포인트·반복 사망 검사는 두 실행에서 통과했지만 전체 왕복은 실패. 파일 첫 줄의 `FAIL checkpoint/death`는 검사 전체 결과이며 사망 검사 자체의 실패를 뜻하지 않음.
- 처리·영향: 동일 오류 반복 중단 규칙에 따라 추가 조사/수정 중단. S1-10 BLOCKED, 영향받는 S1-07·09는 DOING으로 되돌림. S1-08 DONE 유지. 수정 재개 판단과 왕복 재검증이 필요하며 S1-G·Sprint 2 미착수.
- 증거: [전체 빌드 실행 실패](Validation/S1-10-runtime.txt), [집중 재현 실패](Validation/S1-10-focused.txt), [첫 빌드](Validation/S1-10-build.txt), [진단 빌드](Validation/S1-10-build-02.txt). 원시 로그는 로컬 Logs/S1-10-runtime.log·S1-10-focused.log.
### NIGHT-T4-001 — 야간 E1 검증 실패 / C4 재시도 해결

- T4 / REVIEW / 테스트 검증 원인 해결(2026-10-02). 이전 69f3cc2에서 두 번 실패 후 구현을 복원한 이력과 실패 파일은 보존.
- 첫 실패 재현 측정: body-only 이동 후 Rigidbody x=20, Transform x=34. Transform 기준 공격자 배치로 공격 범위 x=33.10~34.20, 실제 적 콜라이더 x=19.50~20.50. HP 2 유지·속도 0. 이전 실패와 일치하는 검사 배치 원인을 재현했으며, 기존 실패 로그만으로 당시 내부 좌표를 확정할 수는 없음.
- 수정: 테스트의 Transform·Rigidbody 위치를 함께 맞추고 Physics2D.SyncTransforms. 변경 후 실제 J 공격 HP 2→1, 적용 직후·후속 샘플 모두 넉백 X=3u/s. 공격/이동/피격 수치 변경 없음.
- 두 번째 실패 경로: 실행기 DontDestroyOnLoad로 수명 보장. 이 변경만 반영한 T1~T3 재검증 PASS, 최종 C4에서 CombatTest→DashTest→CombatTest 왕복과 E1 정리·재생성 PASS.
- E1 코드·프리팹·E1Tuning은 CombatTest에만 보존. 순찰 x=15.975~23.031, 왕복 2회 방향 전환, 벽·낭떠러지 회전, 실제 공격 2타 제거, 접촉 피해·무적 중 중복 방지, KillZone 제거, 플레이어 사망 후 1개 HP2 초기화 PASS.
- 증거: [재시도](Validation/Verification-T4-retry.txt), Verification-night-T1-C4.txt·T2-C4.txt·T3-C4.txt. 회귀 S1-02/04-night-C4.txt. 사용자 감각 수락 전 REVIEW.

## Decisions Needed

CAM-002 / K2 **DEFERRED** — 사용자 2026-10-03: 아트 적용 후 재확인, 우선순위 낮음. Sprint 4 Polish 후보. 기존 0.15s 유지, 이번 재시도 없음. 이전 두 번째 비교 실패는 절대 좌표 비교 영향 추정; 상대 카메라-플레이어 거리 2.06u/2.08u. 재시도는 상대 거리로 비교.


K2 / CAM-002 BLOCKED: 두 번째 검증에서 평상시·반전 비교 FAIL로 승인된 실패 규칙에 따라 전체 복원. 카메라 기본0.15s 유지. 시간 기반 입력의 물리 틱 차이 가능성은 미확정이며 추가 수정/재시도는 별도 승인 필요. 대시 밀림 체감은 미해결.

2026-10-03 K3 재개 승인: 사용자 관찰상 기존 출구/오른쪽 벽 간섭. 새 선반 출구 x30,y6·FromShelf x27,y5.81로 지정, 기존 바닥 출구 보존. S2-03-retry.txt 기술 PASS, 체감 수락 전 REVIEW. B01은 지정 셸/CP/돌아오는 문/안내뿐. CP-B01을 단일 슬롯 유효 ID 및 이어하기 씬 매핑에 추가(새 CP 복귀를 위한 최소 연결).


S2-02: Editor 직접 Play에서 새 방 전환이 가능하도록 EditorBuildSettings에 A05를 추가했다(현재 승인에서 해당 파일 수정 금지 없음). 배포 빌드는 스크립트의 명시 씬 목록 사용. 일반 최대 점프 MEASURE는 같은 높이 플레이어 중심 이동거리 기준이며 발판 끝 콜라이더 걸침·Coyote 극단 입력의 차단을 전수 증명하지 않음. 5u 지정 배치/수치는 유지. 2026-10-03 사용자 A05 플레이에서 점프만으로 불가·Dash 필요 확인 완료.


2026-10-02 P0 검사 보정: S1-04 마지막 중복 점프 비교가 120fps에서 물리 틱 없이 실행되어 첫 회귀 실패. 게임 코드/수치 변경 없이 입력→FixedUpdate 대기를 1회 보완, P0-rerun S1-02/04/07/08 PASS. 첫 실패 로그와 assertion 보존. 사용자 추가 판단 불필요, 검사 보정만 적용.

DEC-S2-PRE (2026-10-01 사용자 승인): Sprint 1 Gate 전 T1~T4 시스템 선행 구현을 DashTest/CombatTest 전용 씬에서만 허용. A01~A04 통합·S1-G·빌드·Sprint 2 콘텐츠 확장은 승인하지 않음. Task별 검증·입력 회귀, 실패 1회 수정 후 재실패 시 해당 Task 복원, 의존 Task 건너뜀, 로컬 커밋만.

DEC-SLICE-USER (2026-10-01 사용자 결정): A02~A04 각각 78개 발판·약 54u 반복 경로 폐기. PRD 반복 이동 금지와 GAME_DESIGN 10절 방별 역할에 불일치. 사용자 직접 Scene 배치로 전환. 빈 방 36×14u·Simple/Scale 프리팹 제공. 기존 SLICE-001 역방향 오류는 폐기 레이아웃에 속하므로 별도 조사하지 않음.


SLICE-001: DEC-SLICE-USER로 폐기된 레이아웃 문제로 처리. T0 빈 방 왕복은 재검증 PASS, 기존 실패 증거는 보존하며 별도 조사하지 않는다.

DEC-ROOM-B(2026-10-01) 사용자 승인: 블록 그레이박스로 S1-07 완료 기준을 대체, Tilemap DEFERRED. ROOM-001은 원인 미확정 OPEN 유지. Tilemap 도입은 사용자 Editor GUI 확인 후 별도 Task로 결정. S1-08 이후 미승인.

### S1-06 / N2-A (2026-09-30)

- Cinemachine 3.1.7: 공식 registry의 최소 Editor 2022.3, 설치된 package.json 3.1.7, manifest/lock 고정. 6000.3.24f1에서 resolve·컴파일·Play 성공.
- 기술 검증: [S1-06 결과](Validation/S1-06-N2-A.txt). 1280×720·1920×1080 렌더와 실제 뷰포트 경계/낙하 시야 검사, 급반전·즉시 카메라 재배치 확인. 사용자 감각 수락은 S1-09 예정.
- 초기 오류: CameraState API·ManualUpdate 모드 조건 수정 후 성공. 로컬 `Logs/S1-06-author.log`, `S1-06-verify.log` 실패 기록 보존; 최종 `Logs/S1-06-verify-snap.log`. 생성에 사용한 임시 작성 스크립트는 삭제해 재실행/덮어쓰기 경로를 남기지 않음.
## Validation Records

### S2-day 최종 빌드 — PASS

[S2-day-build.txt](Validation/S2-day-build.txt): Windows x64 Succeeded, 오류/경고0, Menu,A01~A05,B01 정확히7씬. [S2-day-runtime.txt](Validation/S2-day-runtime.txt): exe 실제 두 프로세스, Menu Z 새 게임→기존 모든 문 및 선반 B01 양방향 직접 접촉/안전 도착, A02 구멍→CP-A01, CP-A03·CP-B01 접촉/사망·B01 다른 방 사망 복귀, A05 획득 저장, 종료→Z 이어하기 Dash+CP-B01/좌표 유지 PASS·FAIL0·Error/Exception/Assert0. 상세 S2-day-runtime-day.txt/daycontinue.txt, S2-day-Player-day.log/daycontinue.log. 사용자 슬롯은 변경하지 않고 Logs/S2-day-slot.json 격리 슬롯 사용. 자동 이동 로봇 없음. K1·K3 포함, 실패 K2는 기존0.15s. S1-02/04/07/08-night-S2-day.txt 최종 회귀 별도.

### K3 — S2-03 재시도 PASS / REVIEW

[S2-03-retry.txt](Validation/S2-03-retry.txt) PASS·FAIL 0. 보유/미보유 걷기·점프·넉백·미보유 Dash 차단, 보유 Dash 양방향 통과/종료 충돌 복원. 선반 출구 x29.5~30.5,y5~7, 바닥 출구 x33.75~34.75,y1~3, 벽 여유4.5u, FromShelf(27,5.81) 비중첩. A03↔B01 왕복·도착·CP-B01 물리 접촉/사망 안전 복귀. 바닥→오른쪽 선반 점프 발 최고2.385u<5, 격자높이6u. 일반 벽은 출구 간섭 없는 기존 DashTest T1 30/60/120fps PASS를 증거로 사용. S1-02/04/07/08-night-K3.txt 회귀 별도. 기존 A01~A05 배치·Level 원본 보존. 일회성 작성 코드 삭제.

### K2 — 2회 실패 / 복원

K2-camera.txt: 배치 모드 WaitForEndOfFrame 대기 중단, K2-camera-first-timeout.log 보존. 검사 대기만 1회 수정한 K2-camera-rerun.txt: 대시 차이 1.089→0.684u 감소, Y 유지·0.2s 복원 PASS, 평상시/반전 좌표 비교 FAIL. 시간 기반 입력의 물리 틱 차이 가능성이 있으나 추가 수정하지 않고 RoomCameraRig·GAME_DESIGN·검사 코드를 K1 기준으로 복원. N2 Camera 전체는 미실시. S1-02/04/07/08-night-K2-rollback.txt로 복원 회귀 확인. K2 BLOCKED, 0.15s 유지.

### K1 — 새 키 기술 검증

[K1-keys.txt](Validation/K1-keys.txt): 새 방향키/Z/X/C·보조 키·Menu Z/X·Pause Z/X PASS, Z/Space 높이 동일. 게임패드 바인딩 유지; S1-02/04/07/08-night-K1.txt 회귀 결과 별도. [P0 push](Validation/P0-push-20261003.txt) 16개 커밋 main push 성공.

### 최종 S2-night — PASS (2026-10-03)

[S2-night-build.txt](Validation/S2-night-build.txt): Succeeded, errors=0·warnings=0, Menu/A01/A02/A03/A04/A05 정확히 6개. [S2-night-runtime.txt](Validation/S2-night-runtime.txt): 첫 줄 PASS·FAIL 0, 실제 exe Menu 시작·새 게임·8개 문 왕복·Spawn 위치·A02 물리 낙하 CP-A01/HP5 복귀·CP-A03 복귀·Error/Exception/Assert 0. [Player.log](Validation/S2-night-Player.log)에도 오류 없음. DashTest/CombatTest/MovementTest/B01 제외. S1-02/04/07/08-night-S2-night.txt 모두 PASS. 커밋 5029ad0, 빌드 위치 Builds/S2-night/Afterglow-S2.exe. 사용자 통합 체감·실물 패드 검토는 미실시.

### S2-04 저장·이어하기 — PASS / REVIEW (2026-10-03)

[S2-04-save.txt](Validation/S2-04-save.txt) PASS·FAIL 0. 격리 슬롯으로 실제 exe 5개 프로세스 순차 실행(acquire/continue/continuecp/new/corrupt): 새 게임→A05 접촉 획득→프로세스 종료→이어하기 Dash 유지, CP-A03 접촉→종료→A03 CP 좌표 복원, 3회 사망 저장 유지, 패드 A/B 새 게임 확인/취소·초기화, 손상 안내·Continue 비활성·명시 새 게임·원본 `.invalid-*` 보존. 각 단계별 S2-04-save-*.txt·S2-04-save-build.txt(errors/warnings0) 보존. S2-04-editor-direct.txt는 Editor A01 기존 저장 무시/비기록, 저장 쓰기 실패 원본 보존·재시도 PASS. S1-02/04/07/08-night-S2-04.txt 모두 PASS. CP 접촉 HP 회복은 GAME_DESIGN14 기존 규칙에 맞춰 적용. 실제 체감 검토 전 REVIEW.


### S2-02 A05 — PASS / REVIEW (2026-10-03)

S2-02-A05.txt 첫 거리 검사 FAIL(점프 0.10초 뒤 Dash 4.968u), 지정 입력 0.30초 뒤 Dash로 한 번 보정한 S2-02-A05-rerun.txt PASS·FAIL 0. 일반 최대 같은 높이 중심 이동 4.680u<5, 점프+Dash 6.888u>5. 수치/5u 간격 변경 없음. A04/A05 실제 출구 왕복·도착 ID, 획득 전/후·3초 안내·CP 유지·재입장 미등장·사망 CP-A03+Dash 유지 PASS. S1-02/04/07/08-night-S2-02-rerun.txt 모두 PASS. 작성 도구 삭제.


### S2-01 실제 방 통합 — PASS / REVIEW

S2-01-integration.txt PASS·FAIL 0: 네 방 J/X·미보유 Dash 무시, HP 유지, A04 Arena E1 x12.996~23.004/y3.515 순찰·2타 제거·접촉/무적·HP0/CP-A03 복귀·A02 KillZone 즉사·재입장 초기화. 기존 이동 설정 보존, 테스트 variants는 base 상속으로 중복 제거. T1~T3 Verification-night-T1/T2/T3-S2-01.txt 및 T4 Verification-T4-night-S2-01-T4.txt PASS. S1-06-lead0-night-S2-01.txt·S1-02/04/07/08-night-S2-01.txt 모두 PASS. 실제 감각 수락 미실시. 작성 도구 삭제.


### 2026-10-02 S1-10 — 그레이박스 v1 빌드 / REVIEW

- [S1-10-build-v1.txt](Validation/S1-10-build-v1.txt) 및 최종 [S1-10-build-v1-rerun.txt](Validation/S1-10-build-v1-rerun.txt): Windows x64 Mono Succeeded, errors=0·warnings=0. BuildPlayerOptions에서 A01/A02/A03/A04 순서만 명시, EditorBuildSettings 미수정. A01 시작·테스트 씬 제외.
- 최초 빌드 후 실행 검사 준비에서 A03에 KillZone이 없음을 확인해, CP-A03 접촉 후 기존 RoomSession.Die를 호출하도록 검사 코드를 바로잡고 재빌드. 빌드·런타임 검증 실패는 없었으며 두 빌드 결과 모두 보존.
- [S1-10-runtime-v1.txt](Validation/S1-10-runtime-v1.txt) 첫 줄 PASS·FAIL 0·exe exit=0. 실제 exe에서 포함 씬 4개 및 순서 일치, DashTest/CombatTest/MovementTest 로드 불가, A01 Entry (3.50,1.81) 시작 확인.
- 출구 트리거에 플레이어 직접 배치: A01→A02→A03→A04→A03→A02→A01. 실제 트리거 콜백→RoomSession 전환을 사용하고 각 도착 Spawn ID·위치·단일 플레이어 확인. 순방향 FromLeft=(3.50,1.81), 역방향 FromRight=(32.50,1.81), 전환 0.020~0.040초. 휴리스틱 자동 이동·경로 재미 검사는 실행하지 않음.
- A02 구멍의 실제 KillZone 낙하→CP-A01/A01 (3.50,1.81), 낙하 포함 0.922초. A03 CP-A03 실제 접촉 후 기존 사망 흐름 호출→CP-A03 (16.00,1.81), 0.600초. 사망 입력 차단·복귀 후 입력 상태 해제 PASS. A03에 위험 지역 추가 없음.
- 런타임 Error/Exception/Assert 0. 별도 [Player.log 보존본](Validation/S1-10-player-v1.log)에서 Exception/Error/Assertion 문자열 없음. 사용자 exe 5분 조작·실물 패드 확인 미실시, S1-10 REVIEW. S1-06/09 사용자 수락 DONE, S1-G TODO.
- 보호 확인: A01~A04·Level 원본·모든 지정 tuning·Player.prefab·MovementTest·Packages·ProjectVersion·EditorBuildSettings 변경 없음. 기존 S1-10 기록은 폐기 레이아웃 기준으로 보존.


### 2026-10-02 C3 — 배치 보고만 / 자동 경로 FAIL

- [S1-09-greybox-check.txt](Validation/S1-09-greybox-check.txt) 및 [해석 정정·한계](Validation/S1-09-greybox-check-notes.txt). 씬·배치·수치 수정 없음. S1-02/04-night-C3.txt 회귀 PASS.
- 자동 순방향 A01은 (7.64,2.44), 역방향 A04는 (24.37,1.93)에서 각각 35초 제한까지 미도달. 뒤 방 통과 시간은 미측정. 휴리스틱 입력 검사 실패만으로 사용자 경로 불가능·지형 결함을 확정하지 않음.
- 모든 RoomSpawn의 0.8×1.6u 플레이어 범위 Ground 겹침 없음, CP-A01·CP-A03 두 개뿐·빈/중복 ID 없음. 수집한 비트리거 지형 박스의 Ground 레이어 PASS; 수집 단계에서 트리거를 제외하므로 잘못 트리거로 지정한 지형까지 전수 통과로 해석하지 않음.
- A02 실제 구멍 낙하→CP-A01(A01 3.50,1.81) 복귀 0.796초 PASS. A03 바닥→선반 4u·선반→격자 상단 6u, 연속 선반의 아래 통로 차단을 확인. 보수적 이산 점프 상한 2.640u(기존 실측 2.520u)보다 높아 오른쪽 선반 직행 차단을 구조로 확인. 원본의 연속 추정 2.400u를 이산 상한이라고 쓴 부분은 별도 notes에서 정정, 원본 보존.
- 사용자 순·역방향 플레이 및 모든 방 통과 시간·착지 시야는 미검증. S1-09 DOING, S1-06 REVIEW, S1-10 TODO. 배치 수정 승인은 별도 필요.


### 2026-10-02 C4 — E1 재시도 PASS / REVIEW

- NightTestRunner 수명 수정만 적용한 T1·T2·T3 각각 PASS: Verification-night-T1-C4.txt·T2-C4.txt·T3-C4.txt.
- 원인 재현과 수정 후 실제 공격 HP/속도, 순찰·벽·낭떠러지·접촉·KillZone·사망·방 재입장: [Verification-T4-retry.txt](Validation/Verification-T4-retry.txt) PASS, FAIL 0, 런타임 Error/Exception/Assert 0.
- S1-02/04-night-C4.txt 입력·이동 회귀 PASS. CombatTest 외 E1 배치 없음, 기존 튜닝·Player.prefab·MovementTest·Packages·Level 프리팹 원본 보존. 실물 입력·사용자 감각 검토 미실시.


### 2026-10-02 C2 사용자 설계 좌표

C2-layout.txt PASS. A02 4개·A03 3개·A04 2개만 Level prefab으로 추가. 이전 오브젝트 snapshot 모두 동일, A01은 저장하지 않음. GateGD 범위 x16~17/y5~11. S1-02/04-night-C2.txt 회귀 PASS. S1-09 DOING 유지. 배치 작성 도구 삭제, Scene이 배치 원본.


### 2026-10-02 C0/C1

C0 A01 사용자 배치 무수정 보존 커밋 1c55594. 원본 SHA256 9AFE8F4AFD0287CE37379CDA0FE492069B2ADA042B3C9831E98FA6EAAA5BC46C. S1-02/04-night-C0.txt PASS.
C1 S1-06-lead0.txt 첫 줄 PASS·FAIL 0, 네 Scene lead0·size5.5·damping0.15 MEASURE, 기존 카메라 경계/낙하/급반전 검사 유지. S1-02/04-night-C1.txt PASS. A01 원본 변경 없음(이미 lead0); A02~A04는 해당 필드만 변경. CameraTest는 저장하지 않고 검사 시 lead0 적용. 구조 경고는 사용자 관찰로 기록, 이번 배치 실행에서는 동일 경고 미관측; 구조 수정 없음. S1-06 REVIEW 유지.


### 야간 T4 — FAIL / 복원

Verification-night-T4.txt 첫 공격 검사 실패. 수정 1회 후 두 번째 검사는 결과 저장 없이 중단(T4-rerun-timeout.log, Verification-night-T4-rerun-stopped.txt). 기능 변경을 전부 복원, S1-02-night-T4-rollback.txt·S1-04-night-T4-rollback.txt PASS. 런타임 E1 완전 검증 성공으로 기록하지 않음. 기능 커밋 없음; 실패 기록 커밋 69f3cc2.


### 야간 T3 — PASS / REVIEW

Verification-night-T3.txt: startup 전 무피해·2콜라이더 더미에 1타·홀드 무연사·쿨다운·시작 방향 고정·공중 수평 이동·공격→대시·피격/Pause 취소·패드 X PASS. S1-02-night-T3.txt·S1-04-night-T3.txt 입력 회귀 PASS. CombatTuning에 지정 초기값 추가, CombatTestPlayer에만 PlayerAttack, Scene 더미 추가. 판정 Gizmo 제공. A01~A04 미통합, 수동 감각 검토 미실시.


### 야간 T2 — PASS / REVIEW

Verification-night-T2.txt: 실제 동시 피해 블록 접촉 1HP, 무적 중 중복 거부, 좌우 넉백 X4/Y3·입력 제한, 피격 대시 취소, HP0→CombatTest CP 복귀→HP5, 무적 중 KillZone 즉사 PASS. S1-08-night-T2.txt 기존 사망 회귀, S1-02-night-T2.txt·S1-04-night-T2.txt 입력 회귀 PASS. CombatTuning 초기값과 CombatTestPlayer variant만 사용. A01~A04 및 기존 Player.prefab 설정 변경 없음.


### 야간 T1 — PASS / REVIEW

Validation/Verification-night-T1.txt: 30/60/120fps 거리 모두 2.880u, 쿨다운·공중 재사용·벽·종료 낙하·권한 false·Pause/전환/사망 취소·패드 B Gameplay/UI 검증 PASS. S1-02-night-T1.txt·S1-04-night-T1.txt 회귀 PASS. DashTestPlayer variant만 권한 true, 기존 Player.prefab·MovementTest·A01~A04 설정 유지. PlayerTuning 기존 값 유지, dashSpeed/Duration/Cooldown 필드만 추가. 수동 감각 검토 미실시.


### 2026-10-01 야간 T0 — PASS

Simple/Transform Scale 프리팹 6종과 36×14u 빈 방 저장. Validation/Verification-night-T0.txt: Landing Scale(6,1) 콜라이더 월드 6×1u·실제 착지 PASS. S1-07-night-T0.txt 전체 왕복·안전 도착 PASS, S1-08-night-T0.txt 체크포인트·물리 A02 KillZone 및 안전 방 직접 사망 요청 PASS, S1-02-night-T0.txt·S1-04-night-T0.txt 입력 회귀 PASS. 보호 파일 변경 없음. 일회성 작성 도구 삭제. 사용자 Rect Tool GUI 조작은 미실시.


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

### 2026-10-01 S1-08 / N2-B

- CP-A01/A03 Prefab: 접촉 활성화·고유 ID·안전 Spawn·활성 색상. KillZone Prefab: 입력/물리 차단 → 실시간 0.6초 대기 → 마지막 CP 또는 A01 Entry 복귀. 다른 방 복귀는 기존 RoomSession 전환 재사용. HP·저장 없음.
- [체크포인트/사망](Validation/S1-08-N2-B.txt) 64건 PASS, 5회 사망 복귀 0.633~0.662초. 기본·같은 방·다른 방·반복 사망·입력 차단·안전 도착·복귀 조작 확인.
- 회귀: [S1-02](Validation/S1-02-N2-B-S1-08.txt) 19건, [S1-04](Validation/S1-04-N2-B-S1-08.txt) 63건, [S1-07](Validation/S1-07-N2-B-S1-08.txt) 71건 PASS. 런타임 Error/Exception 없음. 가상 입력 자동 검사이며 실제 패드·수동 화면은 미실시. 기존 Validation 보존.

### 2026-10-01 S1-09 / N2-B

- [실제 경로 검사](Validation/S1-09-N2-B-05.txt) 1,532건 PASS. A01→A02→A03→A04를 입력으로 이동하며 모든 필수 발판 도달·비행·단일 점프 입력 확인. 최대 점프 거리 4.501u, 간격 상한 80%=3.601u, 실제 간격 1.0~3.0u·착지 폭 2u 이상. 순방향 이동 368.20초(6분 8초). 자동 조작 기준이며 첫 사용자 플레이 시간·재미는 REVIEW 대기.
- A01 안전 연습·A02 가변 점프/낙하·A03 CP 허브/G-D 외형·A04 넓은 접근 공간. A02~A04 첫 위험 틈 및 하단 Kill Zone. 밝은 문 테두리·방 이름·공격/능력 미구현 안내 추가. 블록 Prefab과 Scene이 배치 원본이며 일회성 작성/수정 도구는 삭제.
- [최종 카메라 렌더](Validation/S1-09-camera-02.txt) 4방 경계 PASS. [A01 출구](Validation/Slice/A01-static-02.png), [A03 허브](Validation/Slice/A03-static-02.png)를 열어 테두리·게이트 분리 표시 확인. 정적 월드 렌더만이며 GUI·사용자 출구 인지 수락 증거는 아님. 최초 렌더도 보존.
- 복귀 지연 0.6s를 PlayerTuning.asset의 Death / respawn 그룹에 연결(값 변경 없음, 기존 이동 값 보존). [체크포인트 통합 회귀](Validation/S1-08-N2-B-integration.txt) 64건 PASS.
- 초기 실패 증거 S1-09-N2-B.txt, -02/-03/-04.txt 보존: 천장 안 최대거리 측정 → 열린 평지로 수정; 방향 전환 발판 천장/측면 간섭 → 폭 2u로 수정; 위층 위험 영역이 아래층 점프와 겹침 → 상층 영역 제거, 첫 위험·하단 영역 유지. 규칙·수치 변경 없이 실제 궤적 재검증 통과.

### 2026-10-01 S1-10 / N2-B — 폐기된 레이아웃 기준 (과거 BLOCKED)

- Windows x64 Mono, A01 시작·A01~A04 포함, Development Build=false. [최초 빌드](Validation/S1-10-build.txt)와 [진단 빌드](Validation/S1-10-build-02.txt) 모두 Succeeded, errors=0·warnings=0.
- [실제 실행 파일 전체 검사](Validation/S1-10-runtime.txt): 체크포인트/사망·순방향 A01→A04·A04 복귀·A03 역방향 통과, A02 역방향 첫 걷기 실패. A02→A01 복귀는 미실시.
- [실제 실행 파일 집중 재현](Validation/S1-10-focused.txt): 체크포인트/사망 통과, 입력 장치 격리 후 A02 FromRight 걷기 같은 실패. 목표 x=40.00, 실제 x=42.37. 이후 점프 검사는 미실시. 동일 오류 반복으로 중단, 전체 왕복 PASS로 기록하지 않음.
- README에 Editor A01 Play·MovementTest 회귀 용도·로컬 Slice 경로와 검증 실패 한계 추가. 기존 Validation 전부 보존. 로컬 Builds/Slice는 Git 제외이며 완성/수락 빌드가 아님.
## Playtest Records

### 2026-10-03 S2-night 사용자 exe 관찰

WASD가 불편하며 방향키·Z/X/C 선호, 공격 X가 편함(KEY-001). Dash 중 카메라 밀림 관찰(CAM-002). A05 턱 간 5u는 점프만으로 못 건너고 Dash 필요: 설계 의도대로 사용자 확인 완료, 간격 유지. A03 오른쪽 출구가 벽에 붙어 정지 후 전환: 사용자는 S2-03 실패 원인을 출구/벽 간섭으로 판단. K3는 새 선반 출구를 x30으로 지정, 기존 문 위치 보존.

### OBS-001 — 출구 인지 / Before–After 후보

2026-10-01 사용자 Editor 플레이(A01~A04 블록 그레이박스). 출구가 보이지 않아 방이 하나뿐인 것으로 인식. 원인: RoomExit 트리거에 시각 표시 없음.

S1-09에서 문 형태·밝은 테두리 및 방 이름 표시로 개선한 뒤, 같은 항목(출구 발견·방 전환 인지)을 사용자에게 재확인한다. 현재 Before 관찰만 있으며 개선 효과는 미수락 후보다.


첫 사용자 관찰은 OBS-001에 기록했다. 자동 기술 검증과 사용자 감각 검토를 구분한다. 아래 양식은 빈 서식이다.

`Test ID / 일시 / 빌드·수치 버전 / 참가자 코드·숙련도 / 입력 장치 / 전체 활동 시간 / A·B·C 시간 / 방별 사망·반복 사망 / 길 잃음 위치 / 능력 후 첫 행동 / 게이트 기억·복귀 / 보스 재시도 / 방향 오판 / 관찰 메모 / 증거 위치`

## Design Decision / Before-After

### KEY-001 — 사용자 키 배치 변경

Before: A/D·Space·J·K/Shift, WASD 불편·공격 X 선호. 변경: 방향키·Z·X·C 기본, 기존 키 보조 유지·패드 불변·LCtrl 다음 능력 예약(미바인딩). After: K1 자동 지정 입력에서 Z/Space 높이 1.464u(0.10s), 2.520u(0.50s) 동일, 메뉴 Z 확인/X 취소 PASS. 조작감은 사용자 재확인 대기.

### CAM-002 — 대시 추적 감쇠 가설

Before: 평소 X/Y 감쇠 0.15s, 사용자 Dash 중 카메라 밀림 관찰. 승인 변경: Dash X만 0.03s, 종료 후 0.2s에 0.15s 복원. Y·평소 값 유지. After: K2 재시도 대시 X 최대 차이 1.089→0.684u로 감소했으나 평상시/반전 비교 FAIL(17.64/16.03/15.58 대 17.54/15.88/15.46). 최초 배치 모드 대기 실패 후 검사 대기 1회 수정, 재실패하여 K2 코드/설계 변경 전체 복원. 기본값 0.15s 유지. 새 승인 전 재수정하지 않음.

### CASE-001 — 입력 시각 일치 / 첫 사례 후보 (사용자 미수락)

- 관련: BUG-001, S1-04/05. 의도는 경계 입력의 일관성과 FPS별 동일 홀드 조작감 유지.
- 초기 구현 문제: 프레임 시각/물리 시각 혼용과 프레임 단위 홀드 도구. 수정 전 Coyote 0.0800s 허용/0.1234s 거부, 짧은 점프 1.944/1.596/1.596u(편차 0.348u, 동일 홀드 아님).
- 가설·수정: 공통 이벤트 시계와 실제 해제 시간 기반 Cut 보정, 테스트는 동일 0.10s 이벤트 홀드. 수치 튜닝 변경 없음.
- 이후: Coyote 30/60/120fps 모두 0.0800s 허용/0.1200s 거부. 짧은 점프 1.464/1.464/1.464u(편차 0.000u), 긴 점프 2.520u 유지.
- 판단·한계: 기술 합격 기준은 충족. 기존 홀드 시간이 달랐으므로 전후 높이는 동일 조건 A/B 실험이 아니며, 재미·체감 개선은 사용자 검토 전인 후보다. 기본값 채택·게임 튜닝 개선 확정이 아니다.
- 증거: [이전 결과](Validation/S1-05-results.txt), [새 경계 결과](Validation/S1-04-rerun.txt), [새 높이 결과](Validation/S1-05-rerun.txt). 다음 사용자 검토에서 세 이동 Task를 개별 수락/반려한다.

`Decision ID / 관련 Task·방·시스템 / 기획 의도 / 초기 값·구현 / Test ID·문제 근거 / 수정 가설 / 승인자·일시 / 변경 값·파일 / 동일 구간 재검증 / 실제 결과·한계 / 유지·되돌림 / 반영 문서 / 증거 위치`

### CASE-002 — 출구 인지 / Before–After 후보 (사용자 재확인 전)

- Before: OBS-001, 2026-10-01 사용자 Editor 플레이에서 출구가 보이지 않아 1방으로 인식. RoomExit 트리거만 있고 표시 없음.
- 가설·After 구현: 색과 형태를 함께 쓰는 밝은 3면 문 테두리, A01~A04 방 이름·안내, 공격/능력 미구현 표시. 출입구 ID·연결·게임 수치 변경 없음.
- 폐기된 레이아웃 기준 기술 증거: [6분 8초 순방향 검사](Validation/S1-09-N2-B-05.txt), [출구 렌더](Validation/Slice/A01-static-02.png). 벽/게이트에 표시가 가려지는 초기 렌더를 수정하고 최종 정적 표시 확인.
- 판단: 구현·기술 검증 완료, 출구 발견·방 전환 인지 개선 효과는 사용자에게 같은 항목으로 재확인해야 함. 재미·이해도 개선 확정은 아니며 후보 유지.

### CASE-003 — DEC-SLICE-USER / 사용자 직접 배치

- 변경 전: A02~A04 각 78개 반복 발판, 약 54u 높이. 자동 순방향 368.20초(Validation/S1-09-N2-B-05.txt)는 반복으로 목표 시간을 채운 근거이며 좋은 레벨 흐름의 증거가 아님.
- 사용자 판단: 반복 이동 금지·방별 역할 위반으로 폐기. 역방향 실패 기록은 폐기된 레이아웃 기준으로 보존.
- 변경 후: 36×14u 빈 방·배치 프리팹, 사용자 직접 레벨 제작 대기. 시간·동선·재미 개선은 미검증이며 Before-After 후보.

### CAM-001 — 사용자 관찰로 선행 1u → 0u

- Before: 선행 1u·감쇠 0.15s에서 좌우 전환 시 급이동.
- 가설: 방향 전환 시 기준점 2u 순간 이동.
- 변경: 선행 1→0 단일 변경, 직교 5.5u·감쇠 0.15s 유지.
- After: 사용자 A01 키보드 플레이에서 해소 보고(2026-10-02). 남은 확인: 착지 지점이 화면 밖으로 나가는 구간. S1-06 REVIEW 유지.
- CinemachineCamera가 Brain 하위인 구조 경고는 기록만 하고 구조 변경하지 않음.

## Resource Register

`리소스 / 출처 URL 또는 직접 제작 / 라이선스·확인일 / 사용 위치 / 표기 의무 / 실제 표기 위치`

2026-09-17: `Assets/Art/Block.png`는 직접 생성한 2×2 흰색 도형 텍스처로 플레이어·발판에 재사용한다. 외부 아트·오디오 및 외부 리소스 표기 의무 없음.

## Latest Handoff

2026-10-03 P0→K1→K2→K3→S2-day 종료. main. K1·K3 기술 PASS/체감 REVIEW, K2 BLOCKED(2회 실패 후 복원). S2-03 REVIEW, S2-01/02/04 REVIEW 유지. S2-02 A05 5u 간격은 사용자 확인 완료. Sprint 2 Gate·더블 점프·B01 콘텐츠·후속 방·아트 미실시.

| 단계 | 결과 / 커밋 | Validation |
| --- | --- | --- |
| P0 push | 성공; 기존16커밋 a2f8afc→7b48ef4, main/origin 일치 | P0-push-20261003.txt |
| K1 키 | PASS / REVIEW, 3c70614 | K1-keys.txt |
| K2 카메라 | 2회 FAIL→전체 복원, e9ed368 | K2-camera.txt, K2-camera-rerun.txt, K2-camera-first-timeout.log |
| K3 격자/B01 | PASS / REVIEW, a46b72d | S2-03-retry.txt |
| 최종 빌드 | PASS, b7adde2 | S2-day-build.txt, S2-day-runtime.txt, S2-day-runtime-day.txt/daycontinue.txt, S2-day-Player-day.log/daycontinue.log |

- **회귀:** S1-{02,04,07,08}-night-K1.txt / K2-rollback.txt / K3.txt / S2-day.txt 모두 실제 Unity PASS. 지정 입력/트리거 직접 배치 사용, 자동 이동 로봇 없음. 물리 게임패드 실사용은 미실시(가상 장치·바인딩 유지 확인).
- **실행:** `Builds/S2-day/Afterglow-S2.exe`, Menu+A01~A05+B01 정확히7씬, 테스트 씬 제외. 빌드 오류/경고0, 실제 exe 두 프로세스 Error/Exception/Assert0. 모든 문 왕복/안전 도착·A02 구멍 CP-A01 복귀·CP-A03/CP-B01 사망 복귀·B01 다른 방 사망 복귀·실제 종료→Z 이어하기 Dash/CP-B01/좌표 유지 PASS. 검사 저장은 Logs/S2-day-slot.json 격리 슬롯이며 사용자 슬롯 변경 없음.
- **수정 위치:** 키는 Assets/Input/PlayerControls.inputactions. ←/→, Z 점프, X 공격, C 대시, Esc Pause; 메뉴 ↑/↓·Z/Enter 확인·X/Esc 취소. 기존 A/D·Space·J·K/Shift 보조, 패드 유지, LCtrl 미바인딩/다음 능력 예약. Z/Space 짧은 높이1.464u·긴 높이2.520u 동일. 안내 및 README 조작 표 갱신.
- **격자/배치:** A03 기존 GateGD 인스턴스에 DashGate 추가, 보유+Dash 중만 양방향 통과. 새 선반 출구(30,6), 트리거1×2, FromShelf(27,5.81). 기존 바닥 출구 그대로(y1~3); 새 출구는 y5~7, 오른쪽 벽과4.5u 여유. B01은36×14셸·왼쪽 문·CP-B01(x6)·다음 Sprint 안내만. ProgressSave에 CP-B01 유효 ID/Continue 씬 연결. A01~A05 기존 배치/튜닝/Level 원본/Player.prefab/MovementTest/Packages/ProjectVersion 보존. A03 diff는 새 요소 추가뿐. 새 방 Editor 전환을 위해 EditorBuildSettings에 B01 추가; 배포 빌드는 명시 목록 사용. 일회성 작성 스크립트 삭제.
- **미해결 K2:** 대시 최대 X 차이1.089→0.684u 감소했지만 평상시/반전 좌표 비교가 FAIL. 최초 batchmode 프레임 종료 대기 실패 후 검사 대기만1회 수정했고, 재실패에서 전체 복원. 시간 기반 검사 틱 차이 가능성은 미확정; 추가 수정을 하지 않음. 카메라는 기존 선행0/직교5.5/감쇠0.15, dashDamping 필드 미배포. N2 Camera 전체 검사는 미실시. CAM-002 대시 밀림 체감은 해결되지 않음.
- **사용자 확인:** 새 키 조작감(짧고 긴 Z 점프 포함), 현재 대시 카메라 체감, A05 획득 후 A03 격자 양방향 C 통과→선반 문→B01 및 돌아오기, CP-B01 사망/종료/이어하기. Inspector/씬 수정은 Play 종료 후 다시 입력하고 저장; Level 원본 Apply는 의도할 때만 수행.
- **push:** P0 성공. 이 인계 커밋 이후 승인된 최종 `git push origin main` 한 번을 실행하고 origin/main 동기화 여부를 최종 응답으로 보고한다. pull·merge·force 없음. 실패하면 오류를 여기에 추가하고 재push하지 않는다.
- **다음:** K1/S2-03 체감 수락 및 K2 재시도는 사용자 판단 대기. 새 콘텐츠 자동 착수 없음.
