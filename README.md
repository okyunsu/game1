# 잔광의 길

막힌 길을 기억하고 새 이동 능력으로 돌아와 정상에 도달하는 작은 탐색 게임.

- 장르: 2D 액션 플랫폼 / Metroidvania-lite
- 플랫폼: Windows PC, 키보드·게임패드
- Unity: **6000.3.24f1 (4e7b9b5b6244)** / C#
- 패키지: Input System **1.20.0** 사용. Cinemachine **3.1.7** 사용.

## 현재 상태

2026-10-04 Sprint 3 / Target 15방. 기존 보스·엔딩 후보에 F2R 공격/적 피격 표시와 LOGR 플레이테스트 CSV를 더한 S4-pre 빌드다. F2R·LOGR REVIEW, S3-06~10 REVIEW이며 Sprint 3 Gate는 사용자 판단 대기다. 이름이 S4-pre여도 Sprint 4 튜닝·콘텐츠를 시작한 것은 아니다. 상태와 증거의 원본은 [TASKS](TASKS.md)다.

## Unity에서 실행

1. Unity Hub의 Add로 이 저장소 루트 폴더를 등록한다.
2. 지정한 Unity Editor 버전으로 연다.
3. `Assets/Scenes/A01.unity`를 열고 Play한다. 메뉴 `Afterglow > Open Slice`도 가능하다.
4. Play를 누르고 Game 뷰에 포커스를 둔 뒤 조작한다.

`MovementTest.unity`는 이동 회귀 검사용이다(`Afterglow > Open Movement Test`). Slice는 A01에서 시작하며 밝은 문을 통과해 A04까지 이동한다. 접촉한 CP-A01/A03에서 사망 후 복귀한다.

레벨 배치는 Assets/Prefabs/Level 프리팹, 크기는 Rect Tool 또는 Scale로 조절한다. 지형은 Simple Sprite·1×1u 콜라이더 기준이다. Scene Play 변경은 Stop 후 다시 반영·저장한다.

## 로컬 빌드 실행

S4-pre에서 확인할 것: `Builds/S4-pre/Afterglow-S4-pre.exe`에서 공격 표시·적 HP칸/흰 번쩍임·처치 효과·보스 상단 HP 막대의 가시성을 확인한다.
새 게임을 시작하면 CSV를 자동 기록하며 사망 원인은 `unrecorded`, 활동 시간에서 Pause를 제외한다. 이어하기는 새 기록 세션을 시작하지 않는다.
CSV 실제 폴더: `C:\Users\gosu3\AppData\LocalLow\Afterglow\Afterglow\Playtest` (타 PC에서는 해당 사용자의 LocalLow/Afterglow/Afterglow/Playtest).

현재 통합 빌드: `Builds/S4-pre/Afterglow-S4-pre.exe` (Windows x64, Unity 6000.3.24f1). 같은 폴더의 Afterglow-S4-pre_Data와 DLL을 함께 보존한다. Menu·15플레이방·Ending 포함, 테스트 씬 제외. Builds는 Git 제외이므로 새 checkout에는 exe가 없다. 기존 `Builds/S3-complete/Afterglow.exe`는 표시 추가 전 기준으로 보존했다.

이동 ←/→(보조 A/D), 점프 Z(보조 Space)/패드 A, 공격 X(보조 J)/패드 X, Dash 획득 후 C(보조 K/Left Shift)/패드 B. Double Jump 획득 후 공중에서 점프 키를 놓고 다시 누른다. Pause Esc/Menu, 재개 Z/Enter 또는 X/Esc, Menu ↑/↓·Z/Enter·X/Esc. Pause에서는 ↑/↓로 재개/타이틀을 고르고 Z/Enter/패드 A로 확인한다. 종료는 타이틀 Quit 또는 창 닫기/Alt+F4.

A05 Dash → A03 격자 → B01 → B02/B03 → B04 Double Jump → B01 높은 출구 → B05 → C01~C04 → C06 보스 → Ending으로 연결된다. B04에서는 왼쪽으로 되돌아간다. C06에서 x5를 넘으면 전투 시작, 사망 시 CP-C04 복귀. 보스 처치 후 약2초 빛 표현과 엔딩을 거쳐 타이틀로 돌아간다. 기존 배치와 공통 카메라5.5/0/.15는 유지했다.

사용자 확인: 새 게임부터 엔딩까지 자연 완주 시간, C02 조합 난이도, 보스 예고 가시성·회피 여유·첫 승리 시도 횟수, 엔딩 표현과 F2R 공격/피격 표시. FeedbackTuning.asset에서 표시 수치를 바꿀 수 있으며 Play 종료 후 원하는 값/이전 값을 명시 저장한다. 자동 지정 입력은 물리 게임패드·사용자 체감 수락을 대체하지 않는다.

## 저장·이어하기

Menu의 이어하기 / 새 게임을 방향키·Enter 또는 패드 D-pad·A로 선택한다. 새 게임은 기존 저장이 있으면 확인을 요청하며 Esc/패드 B로 취소할 수 있다. 유효한 저장이 없거나 손상됐으면 이어하기가 비활성이고 안내 후 새 게임을 선택한다.

단일 파일: `%USERPROFILE%\AppData\LocalLow\Afterglow\Afterglow\progress.json` (`Application.persistentDataPath`). 현재 저장 값은 형식 버전·Dash/Double Jump 보유·마지막 CP-A01/CP-A03/CP-B01/CP-C01/CP-C04·bossDefeated·endingReached이며 HP·좌표·적 상태는 저장하지 않는다. 체크포인트 접촉·능력 획득·보스 처치·엔딩 도달 때 자동 저장한다. 보스 처치 직후 종료하거나 엔딩 후 이어하기를 선택하면 Ending으로 간다. 실행 파일을 닫은 뒤 이 파일을 지우면 초기화되고, 새 게임으로도 초기화할 수 있다. 손상 파일은 명시적 새 게임 선택 시 `.invalid-시간` 이름으로 보존한다. 쓰기 실패는 화면에 표시하고 다음 저장에서 재시도한다.

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

S3-01~05와 B04 보정 높이는 2026-10-03 사용자 S3-B exe 플레이로 수락(DONE). S3-06~08은 기술 PASS/REVIEW: B02 소개 위치, C02 조합 난이도, C03 종합 이동과 C04 휴식 체감을 확인한다. 실제 패드 조작·탈착과 자연스러운 전체 플레이 시간은 미실시.
수락/수정 의견은 [TASKS의 Latest Handoff](TASKS.md#latest-handoff)를 따른다. S3-09/10 v2는 실제 보스·엔딩과 필수 회귀 PASS/REVIEW다. F1 효과 이벤트 검사는 PASS였으나 T2 회귀 실패로 전체 복원했다. 선택 방·아트·사운드·흔들림·K2는 포함하지 않았다.

## 문서

- [PRD](PRD.md): 제품 범위·Target / Minimum·완료 조건
- [GAME_DESIGN](GAME_DESIGN.md): 게임 규칙·레벨·튜닝 가설
- [TASKS](TASKS.md): 현재 작업·검토·검증 증거
- [AGENTS](AGENTS.md): 에이전트 작업 규칙

## S3-complete v2 (2026-10-03)

실행: `Builds/S3-complete/Afterglow.exe`. [빌드 결과](Validation/S3-complete-v2-build.txt), [실제 exe 결과](Validation/S3-complete-v2-runtime.txt). 15방/17씬, Error/Exception/Assert0. 최초 최종 빌드의 검사 전용 ScreenCapture 컴파일 오류는 호출 제거 1회 보정 후 성공했다.

C06 배치와 전투 시작선·예고 영역은 `Assets/Scenes/C06.unity`, 공통 보스 값은 `Assets/ScriptableObjects/BossTuning.asset`, 반복 충격파는 `Assets/Prefabs/BossWave.prefab`에서 수정한다. 빛 시간(2s)은 C06 보스의 EndingFlow / Light Duration, 엔딩 문구·버튼은 `Assets/Scripts/EndingMenu.cs`다. Scene의 Play 변경은 Stop 후 다시 입력·저장한다. 설정 Asset은 Stop 후 원하는 값 또는 이전 값으로 명시적으로 저장한다. 기존 Player/Combat/E1/E2/E3 값은 변경하지 않았다.

F1 실패 실험: `Builds/Experimental-F1/Afterglow.exe`, Menu+14방이며 보스/엔딩은 없다. 공격 베기 .10s / 흰색 번쩍임 .08s / 실시간 히트스톱 .05s가 들어 있지만 필수 T2 검증에 실패하여 기본 프로젝트에서는 모두 복원했다. 실험 빌드는 사용자 판단용이며 성공 빌드로 간주하지 않는다. 폴더 전체를 보존한다.
