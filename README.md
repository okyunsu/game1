# 잔광의 길

막힌 길을 기억하고 새 이동 능력으로 돌아와 정상에 도달하는 작은 탐색 게임.

- 장르: 2D 액션 플랫폼 / Metroidvania-lite
- 플랫폼: Windows PC, 키보드·게임패드
- Unity: **6000.3.24f1 (4e7b9b5b6244)** / C#
- 패키지: Input System **1.20.0** 사용. Cinemachine **3.1.7** 사용.

## 현재 상태

2026-10-03 Sprint 2 Gate 사용자 통과, 현재 Sprint 3 / Target 15방. S3-01~05 기술 검증 PASS·체감 REVIEW. Dash·Double Jump·E1/E2/E3·단일 슬롯 저장, A01~A05/B01~B05를 구현했다. 현재 A/B 10방만 빌드에 포함하며 C지역·보스·엔딩·선택 방·아트는 이번 실행에서 제외했다. 상태와 증거의 원본은 [TASKS](TASKS.md)다.

## Unity에서 실행

1. Unity Hub의 Add로 이 저장소 루트 폴더를 등록한다.
2. 지정한 Unity Editor 버전으로 연다.
3. `Assets/Scenes/A01.unity`를 열고 Play한다. 메뉴 `Afterglow > Open Slice`도 가능하다.
4. Play를 누르고 Game 뷰에 포커스를 둔 뒤 조작한다.

`MovementTest.unity`는 이동 회귀 검사용이다(`Afterglow > Open Movement Test`). Slice는 A01에서 시작하며 밝은 문을 통과해 A04까지 이동한다. 접촉한 CP-A01/A03에서 사망 후 복귀한다.

레벨 배치는 Assets/Prefabs/Level 프리팹, 크기는 Rect Tool 또는 Scale로 조절한다. 지형은 Simple Sprite·1×1u 콜라이더 기준이다. Scene Play 변경은 Stop 후 다시 반영·저장한다.

## 로컬 빌드 실행

현재 통합 빌드: `Builds/S3-B/Afterglow-S3.exe` (Windows x64, Unity 6000.3.24f1). 같은 폴더의 데이터와 DLL을 함께 보존한다. Menu·A01~A05·B01~B05 포함, 테스트 씬 제외. Builds는 Git 제외이므로 새 checkout에는 exe가 없다. 이전 S2-day/S2-night/Slice/N1 빌드는 과거 검증용이다.

이동 ←/→(보조 A/D), 점프 Z(보조 Space)/패드 A, 공격 X(보조 J)/패드 X, Dash 획득 후 C(보조 K/Left Shift)/패드 B. Double Jump 획득 후 공중에서 점프 키를 놓고 다시 누른다. Pause Esc/Menu, 재개 Z/Enter 또는 X/Esc, Menu ↑/↓·Z/Enter·X/Esc. 종료는 창 닫기/Alt+F4.

A05 Dash → A03 격자 → B01 턱 관찰 → B02 E2 → B03 E3 → B04 Double Jump → B01 높은 출구 → B05 자리까지 플레이할 수 있다. B04는 왼쪽으로 되돌아간다. B05는 왼쪽 높은 문으로 B01에 돌아가며 오른쪽 출구는 없다. A01~A05 기존 배치는 보존했다. 대시 카메라 밀림 CAM-002는 아트 적용 후 확인 예정(DEFERRED), 기존 감쇠0.15s 유지.

사용자 확인: B01 3.6u 턱 높이, E2 예고 가시성·회피 여유, E3 엄폐 체감, B04 추가점프/연습 높이, A05 획득부터 B05까지 자연스러운 전체 흐름 시간. 지정 입력/배치 exe 검증과 저장·별도 프로세스 Continue는 통과했으며 실제 게임패드·자연스러운 완주 시간은 이번 자동 검증에 포함하지 않았다.

## 저장·이어하기

Menu의 이어하기 / 새 게임을 방향키·Enter 또는 패드 D-pad·A로 선택한다. 새 게임은 기존 저장이 있으면 확인을 요청하며 Esc/패드 B로 취소할 수 있다. 유효한 저장이 없거나 손상됐으면 이어하기가 비활성이고 안내 후 새 게임을 선택한다.

단일 파일: `%USERPROFILE%\AppData\LocalLow\Afterglow\Afterglow\progress.json` (`Application.persistentDataPath`). 현재 저장 값은 형식 버전·Dash/Double Jump 보유·마지막 CP-A01/CP-A03/CP-B01이며 HP·좌표·적 상태는 저장하지 않는다. 체크포인트 접촉·능력 획득 때만 자동 저장한다. 실행 파일을 닫은 뒤 이 파일을 지우면 초기화되고, 새 게임으로도 초기화할 수 있다. 손상 파일은 명시적 새 게임 선택 시 `.invalid-시간` 이름으로 보존한다. 쓰기 실패는 화면에 표시하고 다음 저장에서 재시도한다.

Editor에서 A01을 직접 열고 Play하면 기존 디스크 저장을 읽거나 덮어쓰지 않는 새 게임 상태다. 저장 검토는 Menu 또는 Windows 빌드에서 한다. 테스트 CLI의 `-save-path`는 자동 검증용 격리 파일이며 일반 실행의 저장 위치를 바꾸지 않는다.
## 전용 시스템 테스트 (REVIEW)

- Assets/Scenes/Test/DoubleJumpTest.unity: 보유 true, 3.6u 턱. 실제 Player.prefab은 보유 false.
- CombatTest는 E1 외 E2/E3 검증용 적도 포함한다. 실제 E2는 B02, E3는 B03.
- Double Jump 속도: PlayerTuning.asset/Double Jump Velocity(12u/s). E2/E3: E2Tuning.asset/E3Tuning.asset.
- 배치: B01_GJLedge·B02_SafeStep·B03_Cover/TurretBase·B04_PracticeLow/High. 측정 보정된 High의 Y는5.335(윗면5.835), 다른 배치/튜닝 변경 없음.
- Scene 배치를 Play 중 바꾸면 Stop 후 Scene에 다시 반영·저장한다. ScriptableObject는 Play 종료 후 원하는 값 또는 이전 값으로 명시적으로 저장한다.

- Assets/Scenes/Test/DashTest.unity: Dash, K 또는 Left Shift / 패드 B.
- Assets/Scenes/Test/CombatTest.unity: HP·피해·KillZone·CP 복귀·더미 공격, J / 패드 X. 공격 범위는 PlayerAttack Gizmo.
- 이동은 기존 키/패드 입력을 사용한다. 테스트 카메라는 고정이다. 기본 공격·체력은 실제 방에 통합됐고 A04 Arena에 E1이 있다. 실제 게임에서 Dash는 A05 획득 전까지 비활성이다.
- 이동/Dash 수치는 PlayerTuning.asset, HP/공격 수치는 CombatTuning.asset에서 조정한다. 기존 초기값은 보존하고 사용자 수락 전 임의 기본값 변경을 하지 않는다.

## 현재 조작

| 행동 | 키보드 | 게임패드 (Xbox 표기) |
| --- | --- | --- |
| 이동 | ←/→ (보조 A/D) | 왼쪽 스틱 또는 D-pad |
| 점프 | Z (보조 Space) | A |
| Pause | Esc | Menu |
| 재개 | Z/Enter 또는 X/Esc | A 또는 B |

| 공격 | X (보조 J) | X |
| 대시 | C (보조 K / Left Shift) | B |
| 메뉴 이동 / 확인 / 취소 | ↑/↓ / Z·Enter / X·Esc | D-pad / A / B |

Gameplay / UI 입력은 분리되어 있다. LCtrl은 다음 능력 예약이며 바인딩하지 않는다. 게임패드 바인딩은 유지한다.

## 주요 Asset

| 위치 | 용도 |
| --- | --- |
| `Assets/Scenes/A01.unity`~`A04.unity` | 편집 가능한 블록 Slice·출입구·체크포인트·위험 영역 |
| `Assets/Scenes/MovementTest.unity` | 이동 회귀 검사용 공간 |
| `Assets/Prefabs/RoomBlock.prefab`, `Checkpoint.prefab`, `KillZone.prefab` | 블록·체크포인트·사망 트리거 공통 원본 |
| `Assets/Prefabs/Player.prefab` | PlayerMotor·Rigidbody2D·BoxCollider2D |
| `Assets/ScriptableObjects/PlayerTuning.asset` | 이동·가감속·점프·중력·입력 보정·복귀 지연(0.6초) |
| `Assets/Input/PlayerControls.inputactions` | Gameplay / UI 입력 |
| `Assets/Scripts/PlayerMotor.cs`, `PlayerInputReader.cs`, `PlayerTuning.cs` | 이동·입력·설정 코드 |

수치 실험 전 기존 값을 기록한다. ScriptableObject 변경은 Play 중에도 남을 수 있으므로 **Stop 후** 원하는 값 또는 이전 값을 명시적으로 반영하고 저장한다.
Scene의 Play 중 배치 변경은 되돌아가므로 Stop 후 다시 입력한다. 후보 수치의 기본값 채택은 사용자 수락 후 진행한다.

## 다음 사용자 확인

B01 턱 높이·E2 소개/예고/회피 공간·E3 엄폐·B04 Double Jump와 보정 연습 높이, A05 획득부터 B05까지 전체 흐름 시간을 확인한다. S3-01~05는 REVIEW이며 체감 수락 후 DONE으로 변경한다. 실제 패드 조작·탈착은 직접 확인이 남았다.
수락/수정 의견은 [TASKS의 Latest Handoff](TASKS.md#latest-handoff)를 따른다. C01 이후·보스·엔딩·선택 방·아트·K2 재시도는 별도 승인 전 추가하지 않는다.

## 문서

- [PRD](PRD.md): 제품 범위·Target / Minimum·완료 조건
- [GAME_DESIGN](GAME_DESIGN.md): 게임 규칙·레벨·튜닝 가설
- [TASKS](TASKS.md): 현재 작업·검토·검증 증거
- [AGENTS](AGENTS.md): 에이전트 작업 규칙
