# 메트로 1

막힌 길을 기억하고 이동 능력을 얻어 돌아오는 Windows 2D 탐색 액션 게임.

## 현재 상태

Sprint 3, Batch 12까지 구현. V4 21방, 대시·더블 점프·벽 점프·Slam,
보스 처치 후 상부 경로와 엔딩이 연결되어 있습니다. 기본 보스는 B3(8패턴·2페이즈, 10/14 채택).
B2-fast는 Boss-B2-fast로 선택할 수 있으며, 자연 전체 플레이와 Sprint Gate는 사용자 확인 대기입니다.
현재 결과·결정·직접 플레이 목록은 [TASKS](TASKS.md)를 따릅니다.

## 실행

각 런처를 더블클릭합니다. 타이틀이 나오면 Z로 이어하기를 선택합니다.
저장은 모드별 독립 슬롯입니다. 새 게임 런처는 해당 슬롯을 초기화합니다.

- [Play-V4](Launchers/current/Play-V4.cmd): 기본 새 게임, A01 시작.
- [Play-V4-Mechanics](Launchers/current/Play-V4-Mechanics.cmd): 장치 시험 코스.
- [Play-V4-AfterDash](Launchers/current/Play-V4-AfterDash.cmd): 대시 획득 후 자연 저장 재개.
- [Play-V4-AfterWall](Launchers/current/Play-V4-AfterWall.cmd): 벽 점프 획득 후 자연 저장 재개.
- [Play-V4-BeforeBoss](Launchers/current/Play-V4-BeforeBoss.cmd): C04 개발 fixture, 기본 보스.
- [Play-V4-AfterEnding](Launchers/current/Play-V4-AfterEnding.cmd): 엔딩 후 A01 개발 fixture.
- [Boss-Base](Launchers/current/Boss-Base.cmd): 현재 기본 보스 비교.
- [Boss-B2](Launchers/current/Boss-B2.cmd): B2 후보와 AV1 비교.
- [Boss-B2-fast](Launchers/current/Boss-B2-fast.cmd): 이전 기본 B2-fast와 AV1 비교.
- [Boss-B2-slow](Launchers/current/Boss-B2-slow.cmd): 느린 B2 후보와 AV1 비교.
- [Boss-B3](Launchers/current/Boss-B3.cmd): B3 8패턴(기본과 같은 값의 후보 Asset), C04에서 오른쪽 문으로 진입.
- [Boss-B3-P2](Launchers/current/Boss-B3-P2.cmd): B3 HP14 개발 시작, 첫 공격 후 2페이즈.

## 조작

이동 ←/→ 또는 A/D, 점프 Z 또는 Space, 공격 X 또는 J,
대시 C 또는 K/Left Shift. 공중에서 점프 키를 놓고 다시 누르면 더블 점프.
위+X는 위 공격, 공중 아래+X는 아래 공격·포고, 공중 아래+C는 Slam.
Esc는 Pause, Z/Enter는 확인, X/Esc는 취소, M은 개발 지도(기본 작게, 크게→끔→작게 순환).
V4 Development 지도는 방문한 방만 표시하며 게임은 계속됩니다. `Play-V4.cmd -NoMap`으로 끕니다.
패드: 스틱 이동, A 점프·확인, X 공격, B 대시·취소, Menu Pause.

## 폴더와 개발

Unity 6000.3.24f1으로 프로젝트 루트를 열어 개발합니다.

- Assets/Scenes: 배치 원본. 반복 오브젝트는 Assets/Prefabs.
- Assets/Scripts, Assets/Editor: 게임 코드·에디터 작업.
- Assets/ScriptableObjects: 기본 튜닝. 후보는 Assets/BossCandidates·Assets/Candidates.
- Launchers/current: 현재 실행기. Builds: 최신·직전 빌드.
- Batches: 승인된 배치 지시문. Tools: 현재 실행 도구.
- Validation: 요약·결과. Validation/_raw: 원본 증거, Git 제외. Archive: 이전 이력.

수치는 Inspector의 설정 Asset에서 조정합니다. Play 중 바꾼 값은 기록한 뒤
Stop 후 원본 Asset에 반영·저장하고 재빌드합니다. [V4 안내](LEVEL_V4.md).
작업은 승인 Batch 범위 안에서 Task별 로컬 커밋하며 push는 사용자가 합니다.
회귀: Tools/Run-Regression.ps1 -Tag 새태그 -Quiet (기본 13개).
종료 시 기본 회귀와 Tools/Batch-Card.ps1 -Base 시작커밋 -Allowed 허용범위로 기록합니다.

[이전 README 전체 이력](Archive/README-history-2026-10.md)
