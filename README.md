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

**S1-10 보류: 사용자 배치 후 재빌드.** 기존 Builds/Slice는 폐기된 자동 레이아웃 기준이므로 현재 빈 방과 다르다. 이전 N1 이동 회귀 빌드는 `Builds/N1/Afterglow.exe`에 보존한다.
같은 폴더의 데이터와 DLL이 필요하며 종료는 창 닫기 또는 Alt+F4다.
Builds는 Git에서 제외하므로 저장소를 새로 내려받으면 실행 파일은 포함되지 않는다.

## 전용 시스템 테스트 (REVIEW)

- Assets/Scenes/Test/DashTest.unity: Dash, K 또는 Left Shift / 패드 B.
- Assets/Scenes/Test/CombatTest.unity: HP·피해·KillZone·CP 복귀·더미 공격, J / 패드 X. 공격 범위는 PlayerAttack Gizmo.
- 이동은 기존 키/패드 입력을 사용한다. 테스트 카메라는 고정이다. 기능은 A01~A04에 통합하지 않았으며 E1은 없다.
- 이동/Dash 수치는 PlayerTuning.asset, HP/공격 수치는 CombatTuning.asset에서 조정한다. 기존 초기값은 보존하고 사용자 수락 전 임의 기본값 변경을 하지 않는다.

## 현재 조작

| 행동 | 키보드 | 게임패드 (Xbox 표기) |
| --- | --- | --- |
| 이동 | A/D 또는 ←/→ | 왼쪽 스틱 또는 D-pad |
| 점프 | Space | A |
| Pause | Esc | Menu |
| 재개 | Enter 또는 Esc | A 또는 B |

Gameplay / UI 입력은 분리되어 있다. Dash/Attack은 위 테스트 씬에서만 기능을 검토하며 UI 맵의 B 취소는 유지한다.

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
