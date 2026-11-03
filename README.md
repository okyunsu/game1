# 메트로 1

막힌 길을 기억하고 이동 능력을 얻어 돌아오는 Windows 2D 탐색 액션 게임.

## 현재 상태

현재 Batch 진행·구현 결과·남은 작업·사용자 결정은 [TASKS](TASKS.md#0-지금)를 따릅니다. 직접 플레이 확인 순서는 [TASKS 3절](TASKS.md#3-직접-플레이-확인)에 있습니다.

## 실행

현재 런처는 `Builds/B26-delivery`를 사용하며, 직전 비교 빌드는 `Builds/B25-delivery`입니다.

각 런처를 더블클릭합니다. 타이틀이 나오면 Z로 이어하기를 선택합니다.
저장은 모드별 독립 슬롯입니다. 새 게임 런처는 해당 슬롯을 초기화합니다.

- [Play-V4](Launchers/current/Play-V4.cmd): 기본 새 게임, A01 시작.
- [Play-V4-Mechanics](Launchers/current/Play-V4-Mechanics.cmd): 장치 시험 코스.
- [Play-V4-AfterDash](Launchers/current/Play-V4-AfterDash.cmd): 대시 획득 후 자연 저장 재개.
- [Play-V4-AfterWall](Launchers/current/Play-V4-AfterWall.cmd): 벽 점프 획득 후 자연 저장 재개.
- [Play-V4-BeforeBoss](Launchers/current/Play-V4-BeforeBoss.cmd): C04 개발 fixture, 기본 보스.
- [Play-V4-AfterEnding](Launchers/current/Play-V4-AfterEnding.cmd): 엔딩 후 A01 개발 fixture.
- [Boss-Base](Launchers/current/Boss-Base.cmd): 현재 기본 보스 비교.
- [Play-BossTest-B2](Launchers/current/Play-BossTest-B2.cmd): 벽 점프를 얻고 싸우는 보스2 시험장. Boss-B2도 같은 시험장을 엽니다.
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

사람 플레이 방별 체류 시간 CSV: 제목 화면에서 **새 게임**으로 시작한 V4/V5 세션은 기존 PlaytestLogger가 `Playtest/<날짜-시간>.csv`를 기록합니다. 게임을 종료하거나 제목으로 돌아간 뒤 아래 명령으로 방별 합계를 저장합니다. 일반 Continue·fixture는 기록이 없을 수 있습니다. B26 Play-V5-Loop는 실행부터 자동 기록하고 종료 시 방별 CSV를 만듭니다(30초 smoke로 검증). 제목/Continue로 기록을 재시작하면 새 suffix 원본을 만들며 덮어쓰지 않습니다.

```powershell
& 'C:/Users/gosu3/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe' -B Tools/Export-RoomTimes.py Playtest/20261022-233114.csv
```

입력 경로는 자신의 세션으로 바꿉니다. 기본 출력은 `Playtest/<입력파일명>-room-times.csv`이며 열은 `source,room,visits,completed_visits,incomplete_visits,active_seconds`입니다. 같은 방 재방문은 합산하고 일시정지는 제외합니다. 종료 전 마지막 방도 포함합니다. 비정상 종료는 마지막 기록까지의 하한, 출구 기록 누락은 다음 입장까지의 추정으로 `incomplete_visits`에 표시합니다. 기존 파일은 덮어쓰지 않으므로 재출력은 `--output Playtest/다른이름.csv`를 사용합니다. 원본 로그와 요약 CSV는 Git 제외입니다. 이 도구는 과거 로그에도 쓸 수 있으며 새 게임 플레이 측정과 구분합니다.

B26 검수는 [Play-V5-Loop](Launchers/current/Play-V5-Loop.cmd) **1회30~40분** + [BodySizeTest](Launchers/current/Play-BodySize.cmd) **약5분**입니다. 현재 Builds/B26-delivery의 draft6.1·아트v1.1을 실행합니다. [TASKS 3절](TASKS.md#3-직접-플레이-확인)에 기존 QA6개와 이번 신규 REVIEW 각각의 확인 항목을 적었습니다. 크기 비교 뒤 A03 CP→A04 B1→A05 대시→A03 연습/선반→B01→B02→B03→B04 보스2→벽 점프 재방문(B06·B02위통로·B01탑·B05·A02)을 진행합니다.

기존 QA: B20-T4 크기 / B22-T5 도랑복귀 / B23-T1 보스1등장 / B23-T3 일어나기 / B24-T11 보스2 / B25-T14 그래픽. 신규 REVIEW9개: B26-T4 종류별 스프라이트 / T5 배경·증기 / BL1 조각HUD / BL3 사망표시 / BL4 음량·동시소리 / BL5 대기반짝임 / Q1 플레이어반전·탄성·잔상 / Q2 흰피격 / Q3 방이름. 각 확인 항목은 TASKS3에 있습니다.

[Graybox](Launchers/current/Play-V5-Graybox.cmd)는 같은 충돌체·별도 슬롯입니다. [B1 단독](Launchers/current/Play-BossTest-B1.cmd)·[B2 단독](Launchers/current/Play-BossTest-B2.cmd)은 필요한 경우 선택합니다. M으로 지도를 전환하며 방문 전후를 봅니다. B2는 중앙 벽점프 획득→3u이탈→잠금/1.8초 등장, 전투 중 받침 하강, 사망 뒤 능력유지/재도전을 확인합니다.

기록: 전체 시간____ / 보스 시도 횟수____ / 막힌 방·좌표·입력____ / 머리 박은 곳____ / 그래픽이 헷갈린 곳____ / 캐릭터 크기 선택(1.6/1.3/1.1)____ / 소리·예고 느낌____.

조정 위치: Assets/Art/Original/v1/ArtSettings.asset의 종류별 스프라이트·패럴랙스0.3, V5 Scene의 B26SteamVisual opacity0.25/riseSpeed1.4, Assets/Audio/B25/Feedback.asset의 이벤트볼륨·입자·보스연출·적예고. B1/B2 전투 튜닝 값은 기존 Asset을 유지합니다. Play 중 변경은 메모→Stop→원본 Asset/Scene 저장→재빌드합니다.

자동 측정은 [B26 현재 작업](TASKS.md#1-26-current-tasks-batch26), 독립 진입 반복은 [경로 측정](Validation/B26-T2-routes.md), 남은 지형/입력 한계는 [원인 장부](Validation/B26-T8-route-ledger.md)를 따릅니다. 모서리 후보는 미채택 종료(기본 꺼짐)입니다. 조각은 ID별 저장·4개마다 최대HP+1/전회복이며 Loop/B1 독립 fixture 슬롯, B2 유지 슬롯입니다.

V5 AB draft6.1: [새 시작](Launchers/current/Play-V5.cmd) / [대시 보유](Launchers/current/Play-V5-AfterDash.cmd) / [더블 점프](Launchers/current/Play-V5-DJ.cmd), -NoMap 지원.

방별 실제 입력 회귀 묶음은 새 Development 빌드 후 `Tools/Run-RoomRoutes.ps1 -Tag 고유이름`으로 실행합니다. 12방을 독립 진입 fixture에서 시작하고 적을 유지한 채 주요 출구까지 입력합니다. B06은 선택 도전 뒤 입구로 돌아오는 경로입니다. 통과/실패와 각 원문은 `Validation/_raw/26/<태그>/routes.csv`·방별 result/game.log에 보존하며, 하나가 실패해도 나머지 방을 검사합니다. 전체 자연 완주와 구분합니다. 현재 A03 서쪽 진입 뒤 거리의4u 건물은 설계 판단(D-B26-A03) 대기입니다.

지난 Batch의 검사 결과와 분류는 [TASKS 보관 색인](TASKS.md#5-보관된-항목-색인)을 따릅니다.

Scene 검사: Unity Editor의 `-executeMethod V5Batch18.Clearance -verify-b18-clearance -verify-output 새파일` 또는 Development exe의 `-verify-b18-clearance`(독립 `-save-path` 필수). JSON 검사: `Tools/Check-RoomRules.py Design/v5/rooms_v5_A.json --v5 --self-test --output 새파일`. 검사 결과와 실제 플레이 불가능 판정은 구분합니다.

최종 Development 빌드의 측정 플래그: `-verify-b18-hop`, 일반 경로 `-verify-b16-route -b18-route`, DJ 경로 `-verify-b16-route -b18-dj-route -give=dj`. 모두 V5 인자와 별도의 `-save-path`, 새 `-verify-output`을 사용합니다. [점프·포고 표](Validation/B18-hop-limits.md), [일반 경로](Validation/B18-route.md), [DJ 경로](Validation/B18-dj-route.md). 진행 상태와 미완주 경로는 [TASKS 미해결 목록](TASKS.md#1-1-미해결-목록)을 따릅니다.

DJ 경로의 검증 결과·잔여 한계는 [TASKS 미해결 목록](TASKS.md#1-1-미해결-목록)을 따릅니다.

Windows 실행: Unity와 Unity를 호출하는 검증 스크립트는 정상 사용자 LOCALAPPDATA를 사용하는 샌드박스 밖 실행으로 검증했습니다. 격리 경로의 라이선스 실패와 게임 실패를 구분합니다. 코드 편집·JSON 분석은 샌드박스 안에서, Git 기록은 별도 사용자 승인 예외로 수행했습니다.

Batch20 크기 비교: [BodySizeTest](Launchers/current/Play-BodySize.cmd)에서 **F6**으로 높이 1.6 → 1.3 → 1.1 → 1.6u를 순환합니다. 현재 높이와 공격 사거리는 화면 오른쪽 위에 표시됩니다. 방향키 이동, Z 점프, X 공격, 공중에서 아래+X 포고는 같습니다. 숫자가 표시된 막대6개, 작은 계단5개, 등불, 서 있는 E1을 비교합니다. 먼저 같은 막대 아래에서 세 크기를 비교한 뒤 계단·포고·공격을 확인하세요(약5분).

실제 방 비교는 기존 [Play-V5](Launchers/current/Play-V5.cmd)의 1.6u를 기준으로 [1.3u](Launchers/current/Play-V5-Body13.cmd), [1.1u](Launchers/current/Play-V5-Body11.cmd)를 차례로 실행합니다(각 A01 시작, 약5분). 후보 런처는 크기별 별도 fixture 저장 슬롯을 사용하고 `-NoMap`도 지원합니다. 기본 Play-V5에는 크기 덮어쓰기 컴포넌트를 붙이지 않습니다.

비교는 플레이어 전체의 균일 축소이며 전환 순간 발 위치를 유지합니다. 공격 판정도 같은 비율로 줄고, 이동·점프·중력·카메라 설정은 유지합니다. 도구는 Editor/Development 전용이며 Release에는 포함하지 않습니다. 크기 선택은 런타임 비교라 종료하면 저장되지 않습니다. 기본 크기 채택은 사용자가 D-BODY에서 고른 뒤 별도 배치로 반영합니다. 배치는 새 [BodySizeTest Scene](Assets/Scenes/Test/BodySizeTest.unity)이 원본입니다. Play 중 바꾼 배치를 유지하려면 변경을 기록한 뒤 Edit 모드에서 저장하세요. 기본 PlayerTuning·Prefab은 이번 배치에서 바꾸지 않습니다.
크기 선택과 설계 판단 대기는 [TASKS 결정 대기](TASKS.md#2-결정-대기)를 따릅니다.

이전 Batch 구현 상태와 이후 변경은 [TASKS 현재 작업·미해결 목록](TASKS.md#1-1-미해결-목록)을 따릅니다.

후보 비교는 기존 크기 비교 다음에 진행합니다. 후보마다 독립 저장 슬롯을 쓰며 기본 Play-V5에는 두 동작이 모두 꺼져 있습니다.

- [위험 지형 복귀](Launchers/current/Play-V5-HazardReturn.cmd): A05 도랑에 닿으면 피해1 뒤 마지막 안전 바닥으로 돌아오는 동작을 비교합니다. A05 이동을 포함해 약5~10분.
- [머리 모서리 보정](Launchers/current/Play-V5-CornerFix.cmd): A01/A02 발판 모서리에서 상승이 끊기는지 비교합니다. 허용 겹침0.25u이며 넓은 천장은 그대로 막힙니다. 약3~5분.
- [두 후보 동시](Launchers/current/Play-V5-Candidates.cmd): 머리 보정과 A05 도랑 복귀를 함께 확인합니다. 약5~10분.

위 비교 런처는 A01부터 시작합니다. B22에서 HazardReturn은 V5 기본으로 통합되어 해당 인자를 추가해도 같은 동작입니다. 크기와 함께 비교하려면 런처 인자 `-BodyHeight 1.3` 또는 `-BodyHeight 1.1`을 붙일 수 있습니다. 후보 값은 `Assets/Candidates/B21HazardReturn.asset`·`B21CornerFix.asset`에서만 조정합니다. Play 중 변경은 기록한 뒤 Edit 모드에서 명시 저장하고 Development 빌드를 다시 만듭니다. 후보 빌드에는 기존 빌드 진입점에 `-b21-candidates`를 함께 전달합니다. B22부터 위험 지형 복귀는 V5 기본 켜짐이며 V4는 기존 동작입니다. 모서리 보정 기본 채택은 D-CORNER-FIX 결정 후 별도 반영합니다.

Backlog 대시 비교: [머리+대시 모서리](Launchers/current/Play-V5-DashCornerFix.cmd)는 발끝이 낮은 모서리에 걸릴 때 위로 올리는 후보도 켭니다. 같은 B21CornerFix Asset의 별도 토글은 기본 꺼짐이며, 허용 발끝 겹침 높이는0.25u입니다. 기존 머리 후보와 두 후보 동시 런처에는 이 추가 동작이 꺼져 있습니다. 선택 비교 후 D-CORNER-FIX에서 머리만/머리+대시/미채택을 고릅니다.
