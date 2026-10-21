# 메트로 1

막힌 길을 기억하고 이동 능력을 얻어 돌아오는 Windows 2D 탐색 액션 게임.

## 현재 상태

Sprint 3, Batch 18 draft3까지 구현. V4 21방, 대시·더블 점프·벽 점프·Slam,
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

V5 층 A draft3 파일럿: [새 시작](Launchers/current/Play-V5.cmd) / [대시 보유 A03](Launchers/current/Play-V5-AfterDash.cmd) / [더블 점프 보유](Launchers/current/Play-V5-DJ.cmd).
세 런처 모두 `-NoMap` 지원·V5 전용 fixture 슬롯 사용. [검증·직접 플레이 안내](TASKS.md#9-latest-handoff--batch19).

Batch18: A02 블록·발판 간격과 블록 속 적을 draft3 재생성으로 수정했고 작은 발판12개를 넣었습니다. 최신 V5는 `Builds/B18-delivery`, 기존 V4 런처는 보존본 `Builds/B12-delivery`를 사용합니다.
Batch19 검사 정리: 기존 후보69는 머리 위0·닫힌 문6·측면43·구덩이 가장자리20입니다. 실제 Scene은 FAIL0/WARN0이며 닫힌 문·측면·구덩이는 MEASURE로 표시합니다. draft2 머리 위4건은 계속 FAIL로 검출합니다. 정적 후보22는 A02 WallJump 조건과 A06 유일 입구 DJ 조건을 반영하면0입니다. 무능력 계산도 별도로 남기며 실제 자연 완주와 구분합니다. [후보](Validation/B19-candidates.md), [Scene 검사](Validation/B19-clearance.md), [정적 검사](Validation/B19-room-rules-v5-A.md), [규칙 근거](Validation/B19-rules-ledger.md).

Scene 검사: Unity Editor의 `-executeMethod V5Batch18.Clearance -verify-b18-clearance -verify-output 새파일` 또는 Development exe의 `-verify-b18-clearance`(독립 `-save-path` 필수). JSON 검사: `Tools/Check-RoomRules.py Design/v5/rooms_v5_A.json --v5 --self-test --output 새파일`. 검사 결과와 실제 플레이 불가능 판정은 구분합니다.

최종 Development 빌드의 측정 플래그: `-verify-b18-hop`, 일반 경로 `-verify-b16-route -b18-route`, DJ 경로 `-verify-b16-route -b18-dj-route -give=dj`. 모두 V5 인자와 별도의 `-save-path`, 새 `-verify-output`을 사용합니다. [점프·포고 표](Validation/B18-hop-limits.md), [일반 경로](Validation/B18-route.md), [DJ 경로](Validation/B18-dj-route.md). 일반 경로는5구간 통과했고, DJ는 A01 접근120초 미완주 뒤 A06 도착 fixture에서3구간 통과했습니다.

Batch19 DJ 재측정: 목표 좌표는 draft3와 일치합니다. 자동 입력이 platform2 옆면 충돌 후 바닥으로 떨어져 x24.947에서 점프를 반복하는 타이밍 문제를 기록했습니다. 실제 DJ 상승4.884u로 마지막 발판11.6→문 y15의 높이는 충분하나 자연 A01→A06 완주는 미확인입니다. [프레임 측정](Validation/B19-dj-route.md). Scene·게임 수치·배포 빌드는 유지합니다. B19의 새 프레임·높이 측정 코드는 저장된 Editor 소스에만 있으며 기존 B18 배포 exe에는 포함하지 않았습니다.

Windows 실행: Unity와 Unity를 호출하는 검증 스크립트는 정상 사용자 LOCALAPPDATA를 사용하는 샌드박스 밖 실행으로 검증했습니다. 격리 경로의 라이선스 실패와 게임 실패를 구분합니다. 코드 편집·JSON 분석은 샌드박스 안에서, Git 기록은 별도 사용자 승인 예외로 수행했습니다.
