# 잔광의 길 (가제)

대시와 더블 점프로 기억해 둔 길을 다시 열며 폐쇄된 관측소의 정상에 도달하는 소규모 2D 탐색 액션 게임.

| 항목 | 기준 |
| --- | --- |
| 장르 | 2D 액션 플랫포머 / Metroidvania-lite, 이동·탐색 중심 |
| 목표 플레이타임 | Target 첫 플레이 60~120분; Minimum은 실제 시간 기록 |
| 개발 기간 | 개발 착수일부터 약 4주 / 4개 Sprint |
| 엔진 | Unity 6.3 LTS / 6000.3.24f1 (4e7b9b5b6244) |
| 언어 / 플랫폼 | C# / Windows PC, 키보드·게임패드 |
| 제작 | 1인 기획·검수 + AI 개발 에이전트 구현 |
| 현재 개발 단계 | 2026-09-17 N1 기술 구현·검증 완료, Player Movement Review 대기 |
| 현재 Sprint | Sprint 1 N1(S1-01~05) 승인. 실행 상태의 원본은 TASKS.md |

운영 기준 v0.2: 기술적 구현 완료(IMPLEMENTED)와 사용자 수락(DONE)을 분리한다. 현재 범위는 Target 3지역·18방·적 3종이며, 일정 위험 시 Minimum 3지역·11방·적 2종으로 전환하는 기준을 PRD에 둔다. 초기 수치는 검증 가설이다.

## 핵심 게임 루프

탐색 → 통과할 수 없는 경로 발견 → 이동 능력 획득 → 기존 지역 재방문 → 새 경로 개방 → 최종 구역 → 보스 → 엔딩.

## 문서 구조

- [PRD.md](PRD.md): 최상위 범위, 개발 제약, 제품 완료 조건.
- [GAME_DESIGN.md](GAME_DESIGN.md): 게임 규칙, 방 연결, 초기 수치, 테스트 기준.
- [TASKS.md](TASKS.md): 4주 일정, 실행 가능한 작업, 검토·결정 큐, 증거 기록.
- [AGENTS.md](AGENTS.md): AI 작업 범위, 문서 우선순위, 인계 규칙.

요구사항 우선순위는 PRD → GAME_DESIGN → TASKS → README다. AGENTS는 작업 절차를 규정한다.

## Unity 실행

1. Unity Hub에서 **Add > Add project from disk**로 이 저장소 폴더(`C:\Users\gosu3\OneDrive\문서\ChatGPT\game1`)를 등록한다. `Assets` 하위 폴더를 선택하지 않는다.
2. Editor **6000.3.24f1**로 연다. 설치 위치: `C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe`. 다른 Editor로 업그레이드하지 않는다.
3. `Assets/Scenes/MovementTest.unity`를 열거나 메뉴 **Afterglow > Open Movement Test**를 선택한다.
4. Play를 누르고 Game 뷰에 포커스를 준다. 노란 도형이 플레이어이며 청록색 도형은 바닥·벽·발판이다.
5. 다시 시작하려면 Stop 후 Play. 저장·체크포인트·리스폰은 아직 구현하지 않았다.

현재 기본 빌드 진입점은 **MovementTest**다. Boot/Menu는 초기 구성용이며 완성된 시작 메뉴가 아니다. 테스트 공간은 고정 카메라를 사용하며 본격 Room/Tilemap과 카메라 추적은 다음 승인 범위다.

Unity Hub 3.21.3 설치 위치: `C:\Program Files\Unity Hub\Unity Hub.exe`.
Input System **1.20.0**은 실제 설치·Play Mode 검증했으며 manifest/lock에 고정했다. [공식 6.3 호환 목록](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.inputsystem.html).
Cinemachine **3.1.7**은 [공식 호환 목록](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.cinemachine.html)으로 선정만 했고, N1에서는 설치하지 않았다. S1-06에서 실제 검증한다.

## Windows 검토 빌드

- 실행 파일: `Builds/N1/Afterglow.exe` (N1 / 2026-09-17 / Windows x64 Mono).
- 같은 폴더의 `Afterglow_Data`와 DLL 등을 함께 유지한다. exe만 복사하지 않는다.
- 빌드 오류·경고 0, Development Build 해제. Editor 폴더의 자동 검증 코드·가상 장치·검증용 임시 지형은 포함되지 않는다.
- 실행 시 MovementTest로 바로 진입한다. 종료는 창 닫기 또는 Alt+F4.
- 저장 파일은 아직 없다. Unity 로그는 `%USERPROFILE%\AppData\LocalLow\Afterglow\Afterglow\Player.log`.
- `Builds/`, `Library/`, `Logs/`, `UserSettings/`는 로컬 생성물로 Git에서 제외한다.

## 현재 조작

| 행동 | 키보드 | 게임패드 |
| --- | --- | --- |
| 이동 | A/D, ←/→ | 왼쪽 스틱, D-pad |
| 점프 | Space (짧게/길게) | 남쪽 A (짧게/길게) |
| Pause | Esc | Menu |
| Resume / 취소 | Enter 또는 Esc | A 또는 B |

`Assets/Input/PlayerControls.inputactions`: Gameplay의 `Move`, `Jump`, `Pause`와 UI의 `Navigate`, `Submit`, `Cancel`을 분리했다. 현재 Pause는 재개만 가능한 최소 구현이며 전체 타이틀·메뉴는 후속 범위다. 향후 `Gameplay/Attack`, `Gameplay/Dash` 이름을 사용하되 이번에는 Action·기능을 생성하지 않았다.

## Inspector 조정과 검토

- 공통 수치 원본: `Assets/ScriptableObjects/PlayerTuning.asset`.
- 플레이어 원본: `Assets/Prefabs/Player.prefab`. Scene의 `Player`는 이 Prefab의 인스턴스다.
- 이동 6u/s, 가속 80u/s², 감속 100u/s², 공중 제어 0.9, 점프 초기 속도 12u/s, 중력 30u/s², 하강 배율 1.5, 최대 낙하 18u/s, Jump Cut 0.5, Coyote 0.10초, Buffer 0.12초.
- Inspector 필드에 Tooltip·단위·권장 범위가 있으며 다음 물리 틱에 적용된다. 공유 값은 PlayerTuning, 배치·Collider 크기·Ground Layers 참조는 Scene/Prefab에서 조정한다.
- Play 전 원본값을 기록한다. ScriptableObject 변경은 Play 종료 후에도 남을 수 있으므로 **Stop 후 확정값 또는 원래 값으로 명시적으로 반영하고 저장**한다. Scene 배치 변경은 Stop 후 다시 반영한다.
- v0.2 수치는 검증 가설이며 감각적으로 최종 수락된 값이 아니다. 수락 전 후보값을 새 기본값으로 저장하지 않는다.

검토 순서·Task별 수락 항목은 [TASKS.md의 Player Movement Review](TASKS.md#human-review-queue)에 모았다. S1-03/04/05는 REVIEW이며 각각 수락·반려한다. S1-06 이후는 이번 실행에서 시작하지 않는다.

## 검증 증거와 재실행

`Validation/`에는 실제 Unity 실행 결과만 보관한다. 최종 이동 검증 96개 assertion, 실제 평균 30/60/120fps 조건, 기본 입력 회귀 16개, Windows 빌드 결과를 확인할 수 있다. 물리 timestep은 0.02초다.

실제 패드 손 조작·탈착, 사용자 이동감 수락, Windows 빌드 화면 직접 검수는 미실시다. 화면 확인은 사용자의 물리 Escape 입력으로 중단됐다. 짧은 점프 자동 입력은 프레임 단위 예약이어서 FPS별 홀드 길이가 동일하지 않으며 동일 궤적 보증으로 해석하지 않는다. 상세 한계·측정값은 TASKS Latest Handoff에 기록했다.

Editor를 닫은 뒤 저장소에서 다음 PowerShell 명령으로 이동 검증을 재실행할 수 있다. 검증 중 생성한 가상 장치·임시 지형은 Play 종료 시 제거된다. 설정 변경은 복제한 Asset에서 수행한다.

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe' -batchmode -projectPath $PWD.Path -executeMethod N1Verification.VariableJump -logFile "$PWD\Logs\S1-05-repeat.log"
```

결과는 `Logs/S1-05-results.txt`에 기록된다. 검증 루틴이 Play 종료 후 Editor를 종료하므로 `-quit`를 추가하지 않는다. 입력만 검사하려면 `N1Verification.Input`을 사용한다.

검토 빌드 재생성:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe' -batchmode -quit -projectPath $PWD.Path -executeMethod N1Build.Build -logFile "$PWD\Logs\N1-build.log"
```

`ProjectSetup`, `InputSetup`, `MovementSetup`은 최초 구성용이며 사용자 조정 후 다시 실행하지 않는다. 씬/에셋의 최초 상태 생성 코드다.

## 프로젝트 폴더

```text
Assets/
  Scenes/              # MovementTest, Boot, Menu
  Scripts/             # PlayerInputReader, PlayerMotor, PlayerTuning
  Input/               # Gameplay / UI 입력 Asset
  Prefabs/             # Player, 마찰 없는 물리 Material
  ScriptableObjects/   # PlayerTuning 공통 수치 원본
  Art/                 # 직접 만든 도형 텍스처
  Editor/              # 초기 구성·Unity Play 검증·빌드 도구
  Tilemaps/ Animations/ Audio/ UI/  # 후속 범위용 빈 구조
Packages/              # manifest와 lock
ProjectSettings/       # Unity 버전·설정
Validation/            # 실제 실행 요약 증거
```
