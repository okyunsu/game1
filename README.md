# 잔광의 길

막힌 길을 기억하고 새 이동 능력으로 돌아와 정상에 도달하는 작은 탐색 게임.

- 장르: 2D 액션 플랫폼 / Metroidvania-lite
- 플랫폼: Windows PC, 키보드·게임패드
- Unity: **6000.3.24f1 (4e7b9b5b6244)** / C#
- 패키지: Input System **1.20.0** 사용. Cinemachine **3.1.7**은 선정만 했으며 미설치.

## 현재 상태

진행 재개 기준일은 **2026-09-30**이다. 당장 수행할 작업량·단기 목표·완료일은 토큰 사용량 확인 후 정하며, 기존 구현·검증 이력은 유지한다.

Sprint 1의 N1 기술 구현·검증 완료. S1-01/02는 DONE, S1-03/04/05는 **Player Movement Review** 대기다.
현재는 이동 검증 공간이며, 추적 카메라·본격 Room·전투·능력·저장·엔딩은 아직 없다.
진행 상태와 검증 증거의 원본은 [TASKS](TASKS.md)다.

## Unity에서 실행

1. Unity Hub의 Add로 이 저장소 루트 폴더를 등록한다.
2. 지정한 Unity Editor 버전으로 연다.
3. `Assets/Scenes/MovementTest.unity`를 연다. 메뉴 `Afterglow > Open Movement Test`도 가능하다.
4. Play를 누르고 Game 뷰에 포커스를 둔 뒤 조작한다.

`Boot`와 `Menu` Scene은 초기 구성용이다. 현재 검토는 `MovementTest`에서 시작한다.

## 로컬 빌드 실행

빌드가 있는 PC에서는 `Builds/N1/Afterglow.exe`를 실행한다.
같은 폴더의 데이터와 DLL이 필요하며 종료는 창 닫기 또는 Alt+F4다.
Builds는 Git에서 제외하므로 저장소를 새로 내려받으면 실행 파일은 포함되지 않는다.

## 현재 조작

| 행동 | 키보드 | 게임패드 (Xbox 표기) |
| --- | --- | --- |
| 이동 | A/D 또는 ←/→ | 왼쪽 스틱 또는 D-pad |
| 점프 | Space | A |
| Pause | Esc | Menu |
| 재개 | Enter 또는 Esc | A 또는 B |

Gameplay / UI 입력은 분리되어 있다. Attack과 Dash는 향후 입력 이름이며 현재 구현되지 않았다.

## 주요 Asset

| 위치 | 용도 |
| --- | --- |
| `Assets/Scenes/MovementTest.unity` | Player·평지·발판·벽이 있는 검토 공간 |
| `Assets/Prefabs/Player.prefab` | PlayerMotor·Rigidbody2D·BoxCollider2D |
| `Assets/ScriptableObjects/PlayerTuning.asset` | 이동·가감속·점프·중력·입력 보정 수치 |
| `Assets/Input/PlayerControls.inputactions` | Gameplay / UI 입력 |
| `Assets/Scripts/PlayerMotor.cs`, `PlayerInputReader.cs`, `PlayerTuning.cs` | 이동·입력·설정 코드 |

수치 실험 전 기존 값을 기록한다. ScriptableObject 변경은 Play 중에도 남을 수 있으므로 **Stop 후** 원하는 값 또는 이전 값을 명시적으로 반영하고 저장한다.
Scene의 Play 중 배치 변경은 되돌아가므로 Stop 후 다시 입력한다. 후보 수치의 기본값 채택은 사용자 수락 후 진행한다.

## 다음 사용자 확인

좌우 급반전·공중 제어, 발판 끝 점프·착지 직전 입력, 짧은/긴 점프와 낙하감을 한 번에 검토한다.
실제 패드 조작·탈착과 Pause 재개도 확인한다. 세 Task를 각각 수락하거나 수정 의견을 남긴다.
검토 순서와 Inspector 항목은 [TASKS의 Human Review](TASKS.md#human-review)를 따른다.

## 문서

- [PRD](PRD.md): 제품 범위·Target / Minimum·완료 조건
- [GAME_DESIGN](GAME_DESIGN.md): 게임 규칙·레벨·튜닝 가설
- [TASKS](TASKS.md): 현재 작업·검토·검증 증거
- [AGENTS](AGENTS.md): 에이전트 작업 규칙
