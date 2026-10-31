# 메트로 1

막힌 길을 기억하고 이동 능력을 얻어 돌아오는 Windows 2D 탐색 액션 게임.

## 현재 상태

Sprint 3, Batch24 V5 draft5 건물 거리·대시 감실·적 배치·조각 성장·보스2 시험장까지 구현했습니다. V4 21방, 대시·더블 점프·벽 점프·Slam,
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

Batch24 검수는 마지막에 **한 번, 약20~30분** 진행합니다. 기존 수락 대기 항목의 순서는 [TASKS 3절](TASKS.md#3-직접-플레이-확인)에 보존했습니다.

1. [Play-V5-Loop](Launchers/current/Play-V5-Loop.cmd): A03·능력 없음·기본 몸1.6으로 시작합니다. 건물 거리(상자→M건물→굴뚝→W옥상)를 둘러본 뒤 CP-A03→동쪽 승강기→A04 보스1→A05 대시→도랑→A03 건물/옥상→승강기→서쪽 대시 연습3단계/선반을 확인합니다. 레버는 X, 탑승은 승강기 중앙입니다. 이어서 A02를 거쳐 A01 높은 대시 감실의 조각을 얻고 A03으로 돌아옵니다. 고리 약15~20분.
2. [Play-BossTest-B2](Launchers/current/Play-BossTest-B2.cmd): 중앙 획득대에서 벽 점프를 얻고 좌우로3u 멀어지면 잠금·1.8초 등장 후 전투가 시작됩니다. 양쪽 벽에 붙어 Z를 다시 눌러 높은 충격파를 피하고 돌진·낙하물 틈에 공격합니다. HP26·피해1·한 페이즈입니다. 사망하면 입구 체크포인트로 돌아오며 벽 점프는 유지됩니다. 재도전은 중앙 통과로 시작합니다. 약5~10분.

조각은 ID별로 저장되며4개마다 최대 HP가1 증가하고 즉시 모두 회복합니다. HUD의 `조각 n/4`는 다음 성장까지의 수입니다. 같은 조각은 이어하기·방 재진입 시 다시 나오지 않습니다. Loop/B1 런처는 매번 독립 fixture 슬롯을 초기화합니다. B2 런처는 독립 슬롯을 유지하며, 처음부터 다시 보려면 Esc→제목으로→새 게임을 선택합니다. 제목 화면의 이어하기로 현재 슬롯을 재개합니다.

질문: **A03이 쉬는 거리로 보이나요? 보스 등장·위험 지형 복귀의 딜레이가 자연스러운가요? 승강기·서쪽 연습이 자연스러운가요? 조각을 찾아 돌아갈 이유가 생겼나요? B2에서 벽 점프를 배워 써야 한다고 느꼈나요?**

기록: 고리 시간____ / B1·B2 시도 횟수____ / 막힌 곳____ / 머리 박은 곳____ / 쉬는 거리로 보였는지____.

기술 확인과 한계: B1 실제 처치13.017초, B2 벽 점프7회·포고26회 처치24.767초/HP2, 조각4개 최대HP4→5·저장/이어하기/새 게임 초기화는 통과했습니다. 새 연속 고리는 Editor·최종 exe 모두 A03 반환 승강기에서 재탑승이 안 되어 전체 완주·그 뒤 A01 왕복/Continue가 남아 있습니다. A02 조각길은 늦은 코요테 이륙으로 머리0/5이나 보통 이륙에서는5/5 접촉합니다. B2 바닥 일반 점프는10/10 피격되지만 중앙 획득대 위 일반 점프로는 회피할 수 있어 설계 검토 대기입니다. 수동으로도 반환 탑승이 막히면 위치를 기록하고 B2로 넘어가 주세요. [이번 결과·원문](Validation/B24-summary.md).

보스1만 확인하려면 [Play-BossTest-B1](Launchers/current/Play-BossTest-B1.cmd)을 실행합니다. 사망 시 실제 CP-A03으로 돌아갑니다. 몸 비교는 고리 cmd 뒤 `-BodyHeight 1.3`을 붙이며 기본 채택은 별도입니다.

조정 위치: B1/B2는 각각 `Assets/BossCandidates/B1/BossTuning.asset`, `Assets/BossCandidates/B2/BossTuning.asset`(HP·피해·파동 높이/방향·예고·패턴). B2 배치·획득대·벽·체크포인트는 `Assets/Scenes/Test/BossTest_B2.unity`, BossEncounter의 `introDuration`·`acquisitionX`·`acquisitionDistance`입니다. 복귀 딜레이는 `Assets/Candidates/B21HazardReturn.asset`의 `fadeSeconds`0.15초·`getUpTime`0.45초입니다. Play 중 바꾼 값은 종료 전 기록하고 Edit 모드에서 원본을 저장한 뒤 재빌드합니다.

V5 층 A draft5: [새 시작](Launchers/current/Play-V5.cmd) / [대시 보유 A03](Launchers/current/Play-V5-AfterDash.cmd) / [더블 점프 보유](Launchers/current/Play-V5-DJ.cmd). `-NoMap` 지원. 현재 V4/V5 런처는 모두 `Builds/B24-delivery`를 사용하고 직전 B23 빌드는 비교용으로 보존합니다.

Batch19 검사 정리: 기존 후보69는 머리 위0·닫힌 문6·측면43·구덩이 가장자리20입니다. 실제 Scene은 FAIL0/WARN0이며 닫힌 문·측면·구덩이는 MEASURE로 표시합니다. draft2 머리 위4건은 계속 FAIL로 검출합니다. 정적 후보22는 A02 WallJump 조건과 A06 유일 입구 DJ 조건을 반영하면0입니다. 무능력 계산도 별도로 남기며 실제 자연 완주와 구분합니다. [후보](Validation/B19-candidates.md), [Scene 검사](Validation/B19-clearance.md), [정적 검사](Validation/B19-room-rules-v5-A.md), [규칙 근거](Validation/B19-rules-ledger.md).

Scene 검사: Unity Editor의 `-executeMethod V5Batch18.Clearance -verify-b18-clearance -verify-output 새파일` 또는 Development exe의 `-verify-b18-clearance`(독립 `-save-path` 필수). JSON 검사: `Tools/Check-RoomRules.py Design/v5/rooms_v5_A.json --v5 --self-test --output 새파일`. 검사 결과와 실제 플레이 불가능 판정은 구분합니다.

최종 Development 빌드의 측정 플래그: `-verify-b18-hop`, 일반 경로 `-verify-b16-route -b18-route`, DJ 경로 `-verify-b16-route -b18-dj-route -give=dj`. 모두 V5 인자와 별도의 `-save-path`, 새 `-verify-output`을 사용합니다. [점프·포고 표](Validation/B18-hop-limits.md), [일반 경로](Validation/B18-route.md), [DJ 경로](Validation/B18-dj-route.md). 일반 경로는5구간 통과했고, DJ는 A01 접근120초 미완주 뒤 A06 도착 fixture에서3구간 통과했습니다.

Batch19 DJ 재측정: 목표 좌표는 draft3와 일치합니다. 자동 입력이 platform2 옆면 충돌 후 바닥으로 떨어져 x24.947에서 점프를 반복하는 타이밍 문제를 기록했습니다. 실제 DJ 상승4.884u로 마지막 발판11.6→문 y15의 높이는 충분하나 자연 A01→A06 완주는 미확인입니다. [프레임 측정](Validation/B19-dj-route.md). B19 당시에는 Scene·게임 수치·배포 빌드를 유지하고 측정 소스만 추가했습니다. 현재 B24 배포 exe에도 이 측정 코드와 크기 비교 도구가 들어 있습니다.

Windows 실행: Unity와 Unity를 호출하는 검증 스크립트는 정상 사용자 LOCALAPPDATA를 사용하는 샌드박스 밖 실행으로 검증했습니다. 격리 경로의 라이선스 실패와 게임 실패를 구분합니다. 코드 편집·JSON 분석은 샌드박스 안에서, Git 기록은 별도 사용자 승인 예외로 수행했습니다.

Batch20 크기 비교: [BodySizeTest](Launchers/current/Play-BodySize.cmd)에서 **F6**으로 높이 1.6 → 1.3 → 1.1 → 1.6u를 순환합니다. 현재 높이와 공격 사거리는 화면 오른쪽 위에 표시됩니다. 방향키 이동, Z 점프, X 공격, 공중에서 아래+X 포고는 같습니다. 숫자가 표시된 막대6개, 작은 계단5개, 등불, 서 있는 E1을 비교합니다. 먼저 같은 막대 아래에서 세 크기를 비교한 뒤 계단·포고·공격을 확인하세요(약5분).

실제 방 비교는 기존 [Play-V5](Launchers/current/Play-V5.cmd)의 1.6u를 기준으로 [1.3u](Launchers/current/Play-V5-Body13.cmd), [1.1u](Launchers/current/Play-V5-Body11.cmd)를 차례로 실행합니다(각 A01 시작, 약5분). 후보 런처는 크기별 별도 fixture 저장 슬롯을 사용하고 `-NoMap`도 지원합니다. 기본 Play-V5에는 크기 덮어쓰기 컴포넌트를 붙이지 않습니다.

비교는 플레이어 전체의 균일 축소이며 전환 순간 발 위치를 유지합니다. 공격 판정도 같은 비율로 줄고, 이동·점프·중력·카메라 설정은 유지합니다. 도구는 Editor/Development 전용이며 Release에는 포함하지 않습니다. 크기 선택은 런타임 비교라 종료하면 저장되지 않습니다. 기본 크기 채택은 사용자가 D-BODY에서 고른 뒤 별도 배치로 반영합니다. 배치는 새 [BodySizeTest Scene](Assets/Scenes/Test/BodySizeTest.unity)이 원본입니다. Play 중 바꾼 배치를 유지하려면 변경을 기록한 뒤 Edit 모드에서 저장하세요. 기본 PlayerTuning·Prefab은 이번 배치에서 바꾸지 않습니다.
Batch20 결과: [전체 요약](Validation/B20-summary.md). A03→A05 우회와 대시 없는 일반 점프 도랑 통과를 재현했습니다. 정적 검사 새 FAIL2는 이 실제 버그를 검출한 기대 결과이며 수정 fixture는 FAIL0입니다. draft4 작성과 기본 크기 선택은 TASKS의 결정 대기를 따릅니다.

Batch21에서는 A03 쪽 A05 지름길이 벽 파괴 전에는 닫힙니다. A04에서 A05로 들어가 벽의 오른쪽에서 왼쪽으로 공격하면 열린 뒤 저장·이어하기에도 양방향 통과가 유지됩니다. 방의 좌표·기본 몸 크기·이동 수치는 같습니다.

후보 비교는 기존 크기 비교 다음에 진행합니다. 후보마다 독립 저장 슬롯을 쓰며 기본 Play-V5에는 두 동작이 모두 꺼져 있습니다.

- [위험 지형 복귀](Launchers/current/Play-V5-HazardReturn.cmd): A05 도랑에 닿으면 피해1 뒤 마지막 안전 바닥으로 돌아오는 동작을 비교합니다. A05 이동을 포함해 약5~10분.
- [머리 모서리 보정](Launchers/current/Play-V5-CornerFix.cmd): A01/A02 발판 모서리에서 상승이 끊기는지 비교합니다. 허용 겹침0.25u이며 넓은 천장은 그대로 막힙니다. 약3~5분.
- [두 후보 동시](Launchers/current/Play-V5-Candidates.cmd): 머리 보정과 A05 도랑 복귀를 함께 확인합니다. 약5~10분.

위 비교 런처는 A01부터 시작합니다. B22에서 HazardReturn은 V5 기본으로 통합되어 해당 인자를 추가해도 같은 동작입니다. 크기와 함께 비교하려면 런처 인자 `-BodyHeight 1.3` 또는 `-BodyHeight 1.1`을 붙일 수 있습니다. 후보 값은 `Assets/Candidates/B21HazardReturn.asset`·`B21CornerFix.asset`에서만 조정합니다. Play 중 변경은 기록한 뒤 Edit 모드에서 명시 저장하고 Development 빌드를 다시 만듭니다. 후보 빌드에는 기존 빌드 진입점에 `-b21-candidates`를 함께 전달합니다. B22부터 위험 지형 복귀는 V5 기본 켜짐이며 V4는 기존 동작입니다. 모서리 보정 기본 채택은 D-CORNER-FIX 결정 후 별도 반영합니다.

Backlog 대시 비교: [머리+대시 모서리](Launchers/current/Play-V5-DashCornerFix.cmd)는 발끝이 낮은 모서리에 걸릴 때 위로 올리는 후보도 켭니다. 같은 B21CornerFix Asset의 별도 토글은 기본 꺼짐이며, 허용 발끝 겹침 높이는0.25u입니다. 기존 머리 후보와 두 후보 동시 런처에는 이 추가 동작이 꺼져 있습니다. 선택 비교 후 D-CORNER-FIX에서 머리만/머리+대시/미채택을 고릅니다.
