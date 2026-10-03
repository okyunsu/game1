# 잔광의 길

막힌 길을 기억하고 새 이동 능력으로 돌아와 정상에 도달하는 작은 탐색 게임.

- 장르: 2D 액션 플랫폼 / Metroidvania-lite
- 플랫폼: Windows PC, 키보드·게임패드
- Unity: **6000.3.24f1 (4e7b9b5b6244)** / C#
- 패키지: Input System **1.20.0** 사용. Cinemachine **3.1.7** 사용.

## 현재 상태

진행 재개 기준일은 **2026-09-30**이다. 당장 수행할 작업량·단기 목표·완료일은 토큰 사용량 확인 후 정하며, 기존 구현·검증 이력은 유지한다.

Sprint 1: S1-07/08 검증 완료, S1-09 DOING(사용자 배치 대기), S1-10 TODO. S1-06 카메라는 REVIEW.
현재 A01~A04는 사용자 배치용 빈 방이며 카메라·방 전환·체크포인트·최소 사망 복귀가 있다. Dash·HP·공격은 Test 씬에만 선행 구현됐다. E1은 검증 실패로 복원했으며 디스크 저장·엔딩은 없다.
진행 상태와 검증 증거의 원본은 [TASKS](TASKS.md)다.

## Unity에서 실행

1. Unity Hub의 Add로 이 저장소 루트 폴더를 등록한다.
2. 지정한 Unity Editor 버전으로 연다.
3. `Assets/Scenes/A01.unity`를 열고 Play한다. 메뉴 `Afterglow > Open Slice`도 가능하다.
4. Play를 누르고 Game 뷰에 포커스를 둔 뒤 조작한다.

`MovementTest.unity`는 이동 회귀 검사용이다(`Afterglow > Open Movement Test`). Slice는 A01에서 시작하며 밝은 문을 통과해 A04까지 이동한다. 접촉한 CP-A01/A03에서 사망 후 복귀한다.

레벨 배치는 Assets/Prefabs/Level 프리팹, 크기는 Rect Tool 또는 Scale로 조절한다. 지형은 Simple Sprite·1×1u 콜라이더 기준이다. Scene Play 변경은 Stop 후 다시 반영·저장한다.

## 로컬 빌드 실행

현재 통합 빌드: `Builds/S2-night/Afterglow-S2.exe` (Windows x64). Menu에서 새 게임 / 이어하기로 시작하며 포함 씬은 Menu·A01~A05다. 같은 폴더의 데이터와 DLL이 필요하고 종료는 창 닫기 또는 Alt+F4다. Builds는 Git 제외이므로 새로 내려받은 저장소에는 exe가 없다.

이동: A/D·←/→ 또는 패드 왼쪽 스틱/D-pad. 점프: Space/A. 공격: J/X. A05 대시 획득 후 K·Left Shift/B. Pause: Esc/Menu, 재개: Enter/Esc 또는 A/B. 메뉴: 방향키/Enter 또는 D-pad/A, 새 게임 확인 취소: Esc/B.

현재 제한: 임시 그레이박스 v1, A04 E1 한 마리와 A05 획득/연습만 통합. G-D 구현은 S2-03 두 번 검증 실패 후 복원하여 A03 격자는 여전히 막힌 상태이며 B01은 포함하지 않는다. 더블 점프·후속 방·보스·아트는 미구현이다. 다음 격자 재개에는 별도 사용자 판단이 필요하다.

사용자 약 30분 확인: Menu 새 게임/이어하기, A01~A05 이동·카메라·키보드/패드 점프 일관성, A04 공격·피격·넉백·E1, A05 대시 획득·점프 후 대시 연습, 사망/종료 후 저장 유지. 5u 연습 간격의 지정 입력 측정은 일반 점프 4.680u·0.30초 뒤 대시 조합 6.888u이며, 발판 끝 걸침/Coyote를 포함한 모든 일반 점프 시도 차단을 증명한 값은 아니다.

이전 `Builds/Slice/Afterglow-Slice.exe`는 S1-10 4방 이동 빌드이며 이번 통합 결과는 S2-night에서 확인한다. `Builds/N1/Afterglow.exe`는 이전 이동 회귀용이다.
## 저장·이어하기

Menu의 이어하기 / 새 게임을 방향키·Enter 또는 패드 D-pad·A로 선택한다. 새 게임은 기존 저장이 있으면 확인을 요청하며 Esc/패드 B로 취소할 수 있다. 유효한 저장이 없거나 손상됐으면 이어하기가 비활성이고 안내 후 새 게임을 선택한다.

단일 파일: `%USERPROFILE%\AppData\LocalLow\Afterglow\Afterglow\progress.json` (`Application.persistentDataPath`). 현재 저장 값은 형식 버전·Dash 보유·마지막 CP-A01/CP-A03뿐이며 HP·좌표·적 상태는 저장하지 않는다. 체크포인트 접촉·능력 획득 때만 자동 저장한다. 실행 파일을 닫은 뒤 이 파일을 지우면 초기화되고, 새 게임으로도 초기화할 수 있다. 손상 파일은 명시적 새 게임 선택 시 `.invalid-시간` 이름으로 보존한다. 쓰기 실패는 화면에 표시하고 다음 저장에서 재시도한다.

Editor에서 A01을 직접 열고 Play하면 기존 디스크 저장을 읽거나 덮어쓰지 않는 새 게임 상태다. 저장 검토는 Menu 또는 Windows 빌드에서 한다. 테스트 CLI의 `-save-path`는 자동 검증용 격리 파일이며 일반 실행의 저장 위치를 바꾸지 않는다.
## 전용 시스템 테스트 (REVIEW)

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

S1-06 카메라의 급반전·낙하 시야, Slice 동선과 첫 플레이 5~10분 목표를 확인한다. 밝은 문을 출구로 알아보는지, A01~A04 표시로 방 전환을 알아보는지 다시 확인한다(OBS-001).
점프 일관성을 키보드와 패드 A로 비교한다. 실제 패드 조작·탈착과 Pause 재개도 확인한다.
수락/수정 의견은 [TASKS의 Latest Handoff](TASKS.md#latest-handoff)를 따른다. S1-G와 Sprint 2는 아직 진행하지 않는다.

## 문서

- [PRD](PRD.md): 제품 범위·Target / Minimum·완료 조건
- [GAME_DESIGN](GAME_DESIGN.md): 게임 규칙·레벨·튜닝 가설
- [TASKS](TASKS.md): 현재 작업·검토·검증 증거
- [AGENTS](AGENTS.md): 에이전트 작업 규칙
