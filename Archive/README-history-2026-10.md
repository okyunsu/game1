# 메트로 1

## Batch09 V4 확인 빌드 (2026-10-12)

`삭제됨` Development 한 개. 같은 폴더 또는 `Archive/launchers/B09/`의 `Play-V4.cmd`는 새 게임, `Play-V4-Mechanics.cmd`는 장치 시험 코스, `Play-V4-AfterBoss.cmd`는 전체 능력+Slam 상태의 A01 QA로 시작한다. 각 모드는 전달 폴더 `slots/`의 독립 저장을 사용한다. 새 게임/AfterBoss 실행은 해당 슬롯을 초기화한다. 기존 실행기는 유지했다.

공중 아래+C=내려찍기, 아래+X=기존 아래 공격. 개발 F8=Slam 부여, M=21방 지도, 시험 코스 F=장치 후보1/2/기본 순환. 조정 값은 `MechanicsTuning.asset`, 위치는 설계 데이터→생성기. [V4 생성·장치 안내](../LEVEL_V4.md).

21방 기동·장치 단위·4저장·3실행기는 PASS. 경로29/70 PASS, 전체 왕복77/108 판정 PASS이며 자연 완주는 A03에서 미완주다. 거리2.5u 기준16개 위반과 최소 능력 고립 미증명이 남아 V4 전체 수락은 대기한다. 기본 회귀13개 PASS, RouteAll/Pogo·현재 V2 W22 입력은 FAIL. [현재 결과·사용자 확인](../TASKS.md).

## Batch 06 시스템 확인 빌드 (2026-10-10)

`삭제됨`는 V2 전체 Development 후보다. 아래 실행기를 사용한다(기본 저장 슬롯과 분리).

- 벽 점프 테스트 (삭제됨): 저장 없음. 표시된 청록색 벽 쪽 입력으로 미끄러짐, Z/패드 A로 벽 점프. 숫자 1/2/3으로 기본·끈적·빠름 프리셋 비교. 세 굴뚝은 왼쪽 하단 입구에서 진입한다. 포고 표적·대시 연계·일반벽 대조와 부서지는 벽2종도 있다.
- V2 가이드 켬 (삭제됨): 목표 한 줄·필수 연결 등불, F1 개발 길잡이. `progress-v2-guide.json` 사용.
- V2 가이드 끔 (삭제됨): `-no-guide`로 세 가이드 모두 끔, `progress-v2-noguide.json` 사용. 표지판은 가이드와 별도다.

현재 V2 능력 위치와 연결은 그대로다. V3 재배선·C05 더블 점프·S01·엔딩 변경은 아직 적용하지 않았다. 타이틀 표시만 메트로 1이며 Product/Company·exe·저장 위치는 Afterglow 유지.

조정 위치: 영구 벽 점프 값은 `Assets/ScriptableObjects/PlayerTuning.asset`의 Wall jump 그룹, 비교 후보3개는 `Assets/Scenes/Test/WallJumpTest.unity`의 WallJumpTestCourse/Presets(런타임 복사본만 변경). 벽의 횟수/방향/ID는 `Assets/Prefabs/B06/BreakableWall_*.prefab` 및 Scene 인스턴스. 표지판 문구/거리·등불 위치는 V2 Scene, 목표는 `Assets/Resources/GuideObjectives.asset`. 출구 연결 변경 후 `Afterglow > Rebuild V2 Guide Graph`로 그래프를 갱신한다. Play 중 값을 기록하고 Stop 후 원본 Asset/Scene에 반영·저장한 뒤 다시 빌드한다. 기본값 채택은 사용자 판단이다.

재빌드: Unity 배치 `-executeMethod B06Build.Run -b05-v2 -b06-build B06-delivery -n9-build B06-delivery`. BuildSettings 변경 없음. 세 실행기와 같은 폴더의 Data/DLL을 함께 보존한다. 전체 맵은 없고 현재 V2 단계표만 사용한다.

## Batch 05 V2 확인 빌드 (2026-10-08)

전체 후보: `삭제됨` (실제 exe `Afterglow-V2-delivery.exe`). A만 V2·나머지 기존 혼합: `삭제됨`. 각 실행기는 해당 폴더의 별도 QA 저장 슬롯을 사용한다. 기본·AV1·B2 빌드는 보존했다. [1쪽 QA](../Archive/docs/V2_QA.md), [방별 설계](../LEVEL_V2.md), [현재 인계](../TASKS.md#latest-handoff).

CC0 Kenney Pixel Platformer 1.2를 새 V2 Scene·Variant에 적용했다. 18개 방의 지정 필수/발견 입력, 새 게임→두 능력→보스 처치→엔딩→타이틀 23구간 연속 재생, 저장/이어하기 PASS/Error0. 사람 시간·밀도·난이도·스타일은 REVIEW이며 기본 교체·Sprint Gate는 사용자 판단이다.

V2 빌드는 Editor `B05Build.Run`에 `-b05-v2 -b05-build <새이름>`을 전달한다. Scene 목록은 이 별도 빌드 스크립트에서만 구성하며 기존 BuildSettings/ProjectSettings는 바꾸지 않는다. 새 빌드 폴더의 exe에 `-save-path <별도 QA 파일>`을 주어 실행한다. 조정 위치·Play 종료 후 반영 방법은 QA 안내를 따른다.

## Batch 04 확인 빌드 (2026-10-08)

기본: `삭제됨`. AV1 후보: `삭제됨`. 같은 폴더 Data/DLL 유지. 두 빌드 모두18플레이방·기존2패턴 보스/기본 튜닝 유지. 기본13/13 및 각 최종exe R01~R17 연속 RouteAll→타이틀 PASS/Error0. 기본 AV1 소리/시각추가0, 후보는 직접 생성 효과음10종·5종 기하형/시각 애니메이션/명도 개선. 감각·기본 채택은 REVIEW. [최종 검증](../Validation/B04-final-summary.md), [55분 QA](../Validation/B04-QA-summary.md), [9방 사진](../Validation/B04-shots-summary.md).

각 최종 폴더의 `QA-A01.cmd`/`QA-C06.cmd`/`QA-A06.cmd`/`QA-B06.cmd`/`QA-C05.cmd`를 더블클릭하면 그 폴더의 QA 전용 슬롯만 초기화한다. Menu Z 이어하기. AV1 공유 조정은 `Assets/Candidates/AV1/PresentationProfile.asset` Inspector의 clip/volume/pitch, bodyColors/hazardOpacity, formRatios와 animation 값 한 곳에서 한다. Play 중 값을 기록하고 종료 후 원하는 값인지 확인/반영·저장·AV1 재빌드. 기본 Scene에 후보 활성화를 추가하지 않는다.

B2/fast/slow는 아래 B03 후보 exe를 유지한다. B04 추가 승인 검증은 B2 돌진30/60/120fps·fast/slow60fps 지정DJ 회피0/실패1 및 도약/돌진 Pause·재개·CP-C04/HP/패턴 초기화 PASS. 기존 BOSS-2 전체는 최종 도약회피 전체fps·동시HP0 등 잔여 미검증으로 PARTIAL이며 자연 전투·난이도 수락은 별도다. [돌진 증거](../Validation/B04-C-summary.md). 아래 Batch03 이후 설명은 당시 이력이다.
## Batch 03 확인 빌드 (2026-10-08)

기본: `삭제됨`. 후보 B2: `삭제됨`. 같은 폴더의 Data/DLL을 함께 유지한다. 18플레이방+Menu/Ending/옵트인PogoTest=21씬. 기본13/13·실제exe RouteAll R01~R17 PASS/Error0; B2 실제 확장 적용·처치→Ending→타이틀 PASS/Error0. B2 패턴별30/60/120fps·Pause/사망 검증은 중단/미완료로 PARTIAL이다. [최신 인계](../TASKS.md#latest-handoff).

B03 왼쪽에서 x7의 청록색 `↑ 선택 대시 도전` 입구 앞에서 ↑/스틱 위를 누르면 B06으로 간다. Dash·포고만으로 끝의 발견에 접촉한 뒤 같은 입구로 B03에 돌아온다. 일반 필수 이동 입력은 선택 입구를 활성화하지 않는다. A06/C05 입구는 이전과 같고 사람 시간·체감 PARTIAL 유지. 발견은 세션 한정/저장 안 함, 분모9·실제 최대8/9(A-1 제외). 기본값 채택·Sprint Gate는 사용자 판단이다.

B2-fast: `삭제됨`, B2-slow: `삭제됨`. 두 후보 실제 수치 assertion·처치/Ending/타이틀 PASS/Error0. 각 후보 폴더의 `QA-C06.cmd`를 더블클릭하면 전용 QA 슬롯을 초기화해 CP-C04부터 비교할 수 있다. 기본 폴더에는 `QA-A06.cmd`/`QA-B06.cmd`/`QA-C05.cmd`도 있다.

후보 수치 원본은 `Assets/BossCandidates/B2/BossTuning.asset` 및 B2-fast/slow의 같은 Asset, 문 배치는 B03 Scene의 새 OptionalRoomEntry, B06 내부는 B06 Scene이다. Inspector Tooltip에 단위를 표시했다. Play에서 조정한 값은 Stop 후 해당 Asset/Scene에 원하는 값을 다시 반영·저장하고 재빌드해야 exe에 적용된다. 기본 보스 Asset은 그대로 보존한다. 아래 Batch02 설명은 당시 이력이다.
## Batch 02 확인용 기본 빌드 (2026-10-07)

실행: `삭제됨`. 같은 폴더의 Data/DLL을 함께 유지한다. 17플레이방+Menu/Ending/옵트인PogoTest(20씬), 기존 보스2패턴·기본 튜닝 유지. 기본13/13·최종exe RouteAll R01~R17 PASS, 런타임Error0. 새 선택 방 A06/C05는 시간·체감 조건이 남은 PARTIAL 후보이며 Sprint Gate 수락은 아니다. 이전 `B02-main`/`B02-main-final`/`B02-main-r3`은 B06 분기 회귀가 있어 확인용으로 쓰지 않는다.

A04/C03 왼쪽 기본 출구 위 선택 문(x2,y5.6)에 높은 점프 후 왼쪽 이동으로 닿으면 A06/C05로 간다. B06은 필수 귀환 경로 간섭으로 방과 추가 문을 되돌렸고 실행 범위에서 제외했다. 발견에 접촉하고 같은 입구로 돌아온다. [방별 시험·시간/사용자 QA](../Validation/B02-QA-summary.md), [현재 결과](../TASKS.md#latest-handoff). 배치는 각 새 Scene에서 수정한다. Play 중 Scene 변경은 Stop 후 Scene에 다시 반영·저장한다. 발견은 저장되지 않고 기존 A-1·이번 B06 제외로 실제 최대7/9다.

후보 보스 B2는 걷기 속도·두 위치·도약 높이 결정 대기이므로 아직 실행 파일이 없다. 히트스톱 Pause·사망·0초 기술검사는 PASS이며 0.05s 체감 수락 대기. 조정 원본 FeedbackTuning.asset은 Play 종료 후 저장하고 다시 빌드한다.

## Batch 01 확인용 빌드 (2026-10-07)

`삭제됨`를 실행한다. 같은 폴더의 Data/DLL을 함께 보존한다. A04 전투·C06 보스에서 명중 히트스톱과 기존 베기·번쩍임·HP 칸을 약5분 확인한다. 히트스톱과 Pause/사망이 겹치는 상황은 미검증이므로 [현재 인계](../TASKS.md#latest-handoff)의 상태·경로 결과를 먼저 확인한다. 저장은 아래 기존 단일 슬롯 안내를 따른다. 조정 위치는 `Assets/ScriptableObjects/FeedbackTuning.asset`의 Hit stop / hitStopSeconds(실시간 초, 기본0.05, 0이면 없음). Play 종료 후 원하는 값을 명시 저장하고 재빌드해야 exe에 반영된다.

막힌 길을 기억하고 새 이동 능력으로 돌아와 정상에 도달하는 작은 탐색 게임.

- 장르: 2D 액션 플랫폼 / Metroidvania-lite
- 플랫폼: Windows PC, 키보드·게임패드
- Unity: **6000.3.24f1 (4e7b9b5b6244)** / C#
- 패키지: Input System **1.20.0** 사용. Cinemachine **3.1.7** 사용.

## 현재 상태

2026-10-04 night4 큐 검토·실행 종료. Q6 A/B/C 후보3개 실제 수치 적용·exe·회귀PASS, 기본값은 미변경. Q1 빈 저장/미지원 버전 예외 버그로 S2-04 BLOCKED. Q2~Q5 전체 요구 검증은 미완료, Q7 캡처 오류/검정 이미지로 유효 스크린샷 없음. 종료 회귀는 각각13/13PASS이지만 추가 저장 오류와 미검증 항목은 남아 있다. 최신 결과는 TASKS Latest Handoff를 따른다. 아래 S4-pre2 설명은 기존 기준 빌드 안내다.

2026-10-04 Sprint 3 / Target 15방. TM 검사 정비와 U1 통합 HUD·첫 조작 안내·사망 표시를 더한 S4-pre2 빌드다. TM/U0 DONE, U1·F2R·LOGR REVIEW, S3-06~10 REVIEW이며 Sprint 3 Gate는 사용자 판단 대기다. U1 상태검사와 회귀는 PASS지만 요청 스크린샷 6장은 촬영 도구 중단으로 미실시다. 상태와 증거의 원본은 [TASKS](../TASKS.md)다.

## Unity에서 실행

1. Unity Hub의 Add로 이 저장소 루트 폴더를 등록한다.
2. 지정한 Unity Editor 버전으로 연다.
3. `Assets/Scenes/A01.unity`를 열고 Play한다. 메뉴 `Afterglow > Open Slice`도 가능하다.
4. Play를 누르고 Game 뷰에 포커스를 둔 뒤 조작한다.

`MovementTest.unity`는 이동 회귀 검사용이다(`Afterglow > Open Movement Test`). Slice는 A01에서 시작하며 밝은 문을 통과해 A04까지 이동한다. 접촉한 CP-A01/A03에서 사망 후 복귀한다.

레벨 배치는 Assets/Prefabs/Level 프리팹, 크기는 Rect Tool 또는 Scale로 조절한다. 지형은 Simple Sprite·1×1u 콜라이더 기준이다. Scene Play 변경은 Stop 후 다시 반영·저장한다.

## 로컬 빌드 실행

night4 피드백 비교용: `삭제됨`(현재), `삭제됨`(강하게), `삭제됨`(약하게). 각2분간 타이틀 왼쪽 아래 후보명과 A01 공격/A04 적 명중·HP칸·처치 효과를 비교한다. 같은 폴더의 Data/DLL을 유지한다. `Q6-*-attempt1`은 참조 보정 전 증거이며 비교용이 아니다. [후보 표](../Validation/Q6-feedback-candidates.txt).

후보 수정 위치는 `Assets/Resources/Q6/Feedback-A/B/C.asset`이다. 원본 `Assets/ScriptableObjects/FeedbackTuning.asset` 기본값은 변경하지 않았다. Play 종료 후 원하는 값/이전 값을 명시 저장하고 후보를 다시 빌드해야 exe에 반영된다. 원본 기본값 적용은 사용자 결정 후 별도 수행한다. Q7 캡처 스크립트는 `-capture` 없으면 비활성이고 현재 촬영 결과는 검정 이미지라 참고 화면으로 사용할 수 없다.

S4-pre2에서 확인할 것: `삭제됨`에서 왼쪽 위 HP칸·대시/더블 점프 아이콘과 키 표기가 잘 보이고 이동 공간을 가리지 않는지 확인한다.
새 게임 A01의 첫 안내가 시간 경과/첫 이동으로 사라지고 Continue에는 안 나오며, 사망 중 검정 표시가 복귀 직후 걷히는지 확인한다.
수정은 `Assets/Resources/UITuning.asset`(HUD 위치·크기/HP칸/안내 시간/글자/사망 음영); Play 종료 후 원하는 값·이전 값을 명시 저장. `Validation/U1-shots/`에는 촬영 중단 안내만 있으며 스크린샷 6장은 미실시다.

S4-pre에서 확인할 것: `삭제됨`에서 공격 표시·적 HP칸/흰 번쩍임·처치 효과·보스 상단 HP 막대의 가시성을 확인한다.
새 게임을 시작하면 CSV를 자동 기록하며 사망 원인은 `unrecorded`, 활동 시간에서 Pause를 제외한다. 이어하기는 새 기록 세션을 시작하지 않는다.
CSV 실제 폴더: `C:\Users\gosu3\AppData\LocalLow\Afterglow\Afterglow\Playtest` (타 PC에서는 해당 사용자의 LocalLow/Afterglow/Afterglow/Playtest).

현재 통합 빌드: `삭제됨` (Windows x64, Unity 6000.3.24f1). 같은 폴더의 Afterglow-S4-pre2_Data와 DLL을 함께 보존한다. Menu·15플레이방·Ending 포함, 테스트 씬 제외. Builds는 Git 제외이므로 새 checkout에는 exe가 없다. 기존 `삭제됨`와 `삭제됨`는 보존했다.

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
| 위 공격 — N8 Unity 소스 | ↑(보조 W)+X | 스틱 위+X |
| 대시 | C (보조 K / Left Shift) | B |
| 메뉴 이동 / 확인 / 취소 | ↑/↓ / Z·Enter / X·Esc | D-pad / A / B |

아래 문단은 N8 당시 인계 이력입니다. 현재 N9 실행·조작 안내는 문서 끝의 N9 절과 TASKS Latest Handoff를 따르세요. N8 위 공격은 Unity 소스의 기술 REVIEW 후보이며 30/60/120fps에서 검증했다. 아래 공격·포고는 동일 프레임 KillZone 사망 우선 처리 연결부의 허용 판단이 필요해 BLOCKED다. 최종 실행 후보는 `삭제됨`를 유지하며 이 기존 exe에는 N8 위 공격이 없다. `삭제됨`은 현재 소스의 검사 전용 진단 빌드로 S5-hard가 아니다. 포고 미구현이므로 PogoTest 씬과 pogoVelocity/pogoRefreshesAir Inspector 필드는 아직 없다. 위 판정 폭·높이는 `Assets/ScriptableObjects/CombatTuning.asset`의 Directional attack에서 수정하며 Play 종료 후 Asset에 원하는 값을 저장한다.

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
수락/수정 의견은 [TASKS의 Latest Handoff](../TASKS.md#latest-handoff)를 따른다. S3-09/10 v2는 실제 보스·엔딩과 필수 회귀 PASS/REVIEW다. F1 효과 이벤트 검사는 PASS였으나 T2 회귀 실패로 전체 복원했다. 선택 방·아트·사운드·흔들림·K2는 포함하지 않았다.

## N5 검증 도구 (2026-10-04)

작업 폴더에서 PowerShell로 실행한다. 일반 플레이는 `삭제됨`; 아래 플래그를 주지 않으면 새 검증 도구는 오브젝트를 만들지 않는다. 기존 저장 대신 별도 `-save-path`를 사용하고, 기존 증거 파일에는 덮어쓰지 않는다.

사용자 플레이 녹화와 같은 시작 슬롯 재생:

```powershell
& .\Builds\S4-pre3\Afterglow-S4-pre3.exe -record Logs/user-play.json -save-path Logs/user-play-save.json
& .\Validation\Prepare-N5-Replay.ps1 -Recording Logs/user-play.json -SavePath Logs/user-replay-save.json
& .\Builds\S4-pre3\Afterglow-S4-pre3.exe -replay Logs/user-play.json -save-path Logs/user-replay-save.json -verify-fps 60 -verify-output Validation/user-replay.txt
```

`-record`는 사용자가 Menu에서 시작한 플레이의 Gameplay 축/점프/대시/공격 held·pressed·released를 물리 틱마다 관찰한다. 엔딩 도달 또는 정상 종료 시 JSON을 저장한다(강제 프로세스 종료는 저장하지 못한다). 시작 씬·체크포인트·스폰·능력, 방 입장 틱, 최종 위치가 포함된다. `-replay`는 새 격리 슬롯에서 같은 메타데이터를 확인한 뒤 가상 키보드로 재생한다. 자동 검증용 녹화에는 `-verify-record-route`를 추가한다. 검증된 범위는 A01→A02 짧은 지정 경로이며, 사용자의 임의 전체 경로·Pause·사망이 섞인 재생은 별도 확인이 필요하다. JSON의 방 입장 `tick × fixedDeltaTime(기본 .02s)`으로 구간 시간을 계산한다.

| 플래그/명령 | 용도·출력 |
|---|---|
| `-verify-save-failure readonly/folder -verify-marker Logs/test -verify-output Validation/test.txt` | 호스트 준비 마커와 격리 권한을 쓰는 저장 실패 검사. `Run-N5-TA.ps1` 참고; 이동 분기 KNOWN-LIMIT |
| `-verify-save-failure ending -save-path <ending-fixture>` | 새 프로세스 Ending Continue/타이틀/재Continue; N5-TA-ending.txt |
| `-verify-boss-retry -save-path <CP-C04-fixture>` | 3회 물리 사망·복귀·재진입 후 가상 입력 처치·엔딩; N5-TB-boss-retry.txt |
| Editor `-executeMethod N5Editor.Run -verify-fps-matrix -save-path <CP-A01-both-fixture>` | 30/60/120/144fps 구간 행렬; N5-TD-fps-matrix.txt |
| `-verify-soak 60 -save-path <CP-A01-both-fixture> -verify-csv Validation/soak.csv` | 실제 시간 안정성 러너. 현재 A04 KillZone 선택 오류로 KNOWN-LIMIT; 60분 PASS 아님 |
| `-verify-display start/hit/abilities/death/B04/boss/ending -verify-shot <PNG> -verify-output <TXT>` | 일반 창 EOF 캡처·픽셀 분산·화면/GUI/뷰포트 기록. `Run-N5-TF.ps1`의 씬별 시작 빌드 사용. 기존 Q7 `-capture`와 충돌을 피하는 별도 플래그 |
| `-verify-route-legacy -save-path <new-slot> -verify-output <TXT>` | 이전 N5 자연 경로 러너; bare `-verify-route`도 호환 유지 |
| `-verify-route R01` (R01~R17) | 구간 시작점만 설정하고 고정틱 가상 입력을 기록. `-route-prefix`, `-route-library`, `-verify-output`, `-save-path`에 새 경로 지정 |
| `-verify-route all -route-library <JSON> -verify-fps 60` | 저장된 성공 구간을 새 게임부터 실제 종료 상태로 재생. 텔레포트·능력 지급 없음. `-route-gamepad`로 가상 패드 사용 |
| `-verify-route train -route-continuous -route-attempt-limit 1` | 새 게임부터 연속 기록; 첫 구간 실패 시 중단. 결과 원본은 TASKS의 N8 경로 요약을 확인 |

모든 수치·Scene·Prefab 기본값은 유지했다. 구간 시작/출구/KillZone 배치는 승인된 T-D/T-E 검증 설정에만 사용한다. 실제 결과와 미검증 항목은 [N5 인계](../Validation/N5-final-handoff.md)를 따른다.

## 문서

- [AGENTS](../AGENTS.md): Batch 승인·작업·Git·검증·인계 절차
- [PRD](../PRD.md): 제품 범위·Target / Minimum·완료 조건
- [GAME_DESIGN](../GAME_DESIGN.md): 게임 규칙·레벨·튜닝 가설
- [TASKS](../TASKS.md): Current Sprint / Current Tasks / Bugs / Decisions Needed / Latest Handoff
- [README](../README.md): 실행 방법·조작·수정 위치
- [BATCH_TEMPLATE](../Batches/BATCH_TEMPLATE.md): 예산·트랙·사전 결정·허용 범위·Backlog·종료 조건
- [Archive](../Archive/TASKS_ARCHIVE.md): 완료 Task·해결 기록·과거 인계·관찰 및 설계 결정 원문
- [LEVEL_PROPOSALS](../Archive/docs/LEVEL_PROPOSALS.md), [PROCESS_METRICS](../PROCESS_METRICS.md): 현재 링크된 배치·운영 자료로 위치 유지

## S3-complete v2 (2026-10-03)

실행: `삭제됨`. [빌드 결과](../Validation/S3-complete-v2-build.txt), [실제 exe 결과](../Validation/S3-complete-v2-runtime.txt). 15방/17씬, Error/Exception/Assert0. 최초 최종 빌드의 검사 전용 ScreenCapture 컴파일 오류는 호출 제거 1회 보정 후 성공했다.

C06 배치와 전투 시작선·예고 영역은 `Assets/Scenes/C06.unity`, 공통 보스 값은 `Assets/ScriptableObjects/BossTuning.asset`, 반복 충격파는 `Assets/Prefabs/BossWave.prefab`에서 수정한다. 빛 시간(2s)은 C06 보스의 EndingFlow / Light Duration, 엔딩 문구·버튼은 `Assets/Scripts/EndingMenu.cs`다. Scene의 Play 변경은 Stop 후 다시 입력·저장한다. 설정 Asset은 Stop 후 원하는 값 또는 이전 값으로 명시적으로 저장한다. 기존 Player/Combat/E1/E2/E3 값은 변경하지 않았다.

F1 실패 실험: `삭제됨`, Menu+14방이며 보스/엔딩은 없다. 공격 베기 .10s / 흰색 번쩍임 .08s / 실시간 히트스톱 .05s가 들어 있지만 필수 T2 검증에 실패하여 기본 프로젝트에서는 모두 복원했다. 실험 빌드는 사용자 판단용이며 성공 빌드로 간주하지 않는다. 폴더 전체를 보존한다.


N8 자동 경로의 최종 입력은 `Validation/N8-route-final-library.json`이며 순차60fps3회·30/120fps·가상 패드 완주와 저장을 확인했다. 기본13회귀에 현재소스 새빌드 `RouteAll-60fps`를 더했다. `Tools/Run-Regression.ps1 -Tag <새태그>`는14개, `-OnlyRoute`는 새빌드 경로 검사만 실행한다. 자동 활동시간 약2분은 사람 플레이시간이 아니다. [최종 인계](../Validation/N8-final-handoff.md).


## N9 방향 공격·포고

| 동작 | 키보드 | 패드 |
| --- | --- | --- |
| 위 공격 | ↑+X (W+X) | 스틱 위+X |
| 아래 공격·튕기기 | 공중 ↓+X (S+X) | 공중 스틱 아래+X |

지상 아래+X는 좌우 공격입니다. 포고는 획득한 더블 점프·대시 사용권만 회복합니다. 몸이 가시에 먼저 닿으면 사망합니다. 설정은 CombatTuning.asset의 Down attack / pogo, 보스 HP는 BossTuning.asset maxHP입니다. Play를 종료한 뒤 Asset에 원하는 값을 다시 입력하고 저장하세요.

본편은 `삭제됨`, PogoTest는 같은 폴더의 `PogoTest 실행.bat`로 실행합니다. 에디터 Afterglow > Open Pogo Test 또는 exe의 `-pogotest` 인자로도 엽니다. Build Settings와 타이틀 메뉴에는 추가하지 않았습니다. 테스트 과정은 저장을 쓰지 않습니다.

## N9 후보 비교와 검증 범위

후보 실행 파일은 `삭제됨`, `삭제됨`, `삭제됨`입니다. H1은 현재 Hard(보스 HP28), H2는 승인된 빠른 적·보스(HP32), H3는 H2 전투와 플레이어 HP5/무적1.0s입니다. 원본 설정을 바꾸지 않고 `Assets/N9Candidates/H2`, `H3`의 Asset·Prefab 복사본을 빌드 씬에 연결했습니다. 각 후보를 10분 비교한 뒤 하나를 선택합니다.

포고 값은 `Assets/ScriptableObjects/CombatTuning.asset`의 `pogoVelocity`, `pogoRefreshesAir`, `downAttackWidth`, `downAttackHeight`, 보스 HP는 `BossTuning.asset`의 `maxHP`입니다. PogoTest 배치·안내 위치는 `Assets/Scenes/Test/PogoTest.unity`, 반복 가시 표적은 `Assets/Prefabs/PogoTarget.prefab`입니다. Scene은 Play 종료 후 다시 입력·저장하고, Asset은 원하는 최종 값을 명시적으로 입력·저장합니다. 후보 값을 기본값으로 적용하려면 사용자 선택이 필요합니다.

`Tools/Run-Regression.ps1 -Tag <새이름>`은 기본13 + RouteAll60 + PogoTest60, 총15개를 검사합니다. `-SkipPogo`는 이전14개, `-OnlyRoute`는 경로2개만 실행합니다. 실패를 PASS로 바꾸지 않습니다. 기존 FixedUpdate 관찰의 사망 누락을 바로잡은 뒤 R05 실제 사망이 확인되어 연속 완주 경로는 BLOCKED입니다. Editor 고정 Pogo 입력 재생도 별도 BLOCKED이며, 실제 exe Pogo3구간·체크포인트 복귀·능력 회복 PASS와 구분합니다. 상세 결과와 무효화된 과거 결과는 `Validation/N9-route-recheck.txt`, 최종 상태는 TASKS Latest Handoff를 확인합니다.


## Batch06b 벽 점프 조절·엔딩 후 이어하기 (2026-10-10)

Development 실행기:
- `삭제됨`: 저장 없는 안전 테스트. 1 기준, 2 짧은 킥, 3 짧은 킥+상승 부착. 벽 쪽 입력으로 점프하면 짧은 킥, 중립·반대 입력은 긴 킥. x114 한쪽12u 벽 추가.
- 테스트 전용 `7/8` Short speed ±0.5u/s, `9/0` Short lock ±0.01s, `6` 상승 부착 토글. 화면 전체값과 `Logs/walljump-tuning-last.txt` 일치. 원하는 값을 그대로 남겨 전달한다. Play 복사본이며 원본 Asset에는 자동 반영되지 않는다.
- 오른쪽 아래 각 굴뚝/한쪽 벽의 최근3회 시간·벽 점프 횟수, `Logs/walljump-runs.txt` 기록.
- `Play-V2-Guide.cmd` / `Play-V2-NoGuide.cmd`: 기존 독립 QA 슬롯. 현재 가이드 유지, Pause 목표 토글 취소.
- `QA-AfterEnding.cmd`: 실행할 때마다 별도 `qa-afterending.json` 슬롯을 엔딩 도달/능력3종으로 다시 만든다. 이어하기→C01_V2/CP-C01, C06 보스 재등장 없음. 일반 QA 슬롯과 분리된다.

영구 튜닝은 `Assets/ScriptableObjects/PlayerTuning.asset` Wall jump. 기존 프리셋1 값은 채택했다. Short3.5/.06과 상승 부착은 후보로만 제공하며, 기본 Asset 반영은 사용자 결정 이후 Stop 상태에서 편집·저장·재빌드한다. Scene 배치가 원본이며 V3 레벨은 이번에 바꾸지 않았다.

## Batch07 V3 후보 실행 (2026-10-11)

`삭제됨`와 `Play-V3-NoGuide.cmd`로 시작합니다. QA-V3-A03/B01/C01/C04.cmd는 각각 대시 / 대시·벽점프 / 전 능력 / 보스 직전 CP를 별도 슬롯에 준비하며 타이틀에서 Continue를 선택합니다. 슬롯은 `삭제됨`입니다. 기본 저장과 분리됩니다. `Play-WallJumpTest.cmd`도 함께 있습니다. 실행기 원본은 `Archive/launchers/B07/`입니다.

A02 계단·굴뚝과 C03 첫 선택 도전은 PARTIAL입니다. QA 우회 검증은 실행기 기본 플레이에 적용되지 않습니다. 30/60/120fps는 A02 두 구간을 건너뛴 나머지 경로만 엔딩까지 확인했습니다. V2 고정 입력 보스 사망은 회귀 의심으로 남았습니다. 자세한 상태·결정·직접 플레이 순서는 TASKS Latest Handoff에 있습니다.

조정할 배치·영역은 V3 Scene, 반복 오브젝트는 기존 Prefab이 원본입니다. 이동 수치는 기존 PlayerTuning Asset 한 곳에서 조절합니다. Scene 값을 Play에서 바꾸면 Stop 후 같은 값을 다시 입력·저장하고 재빌드합니다. Short 후보는 런타임 복사본이며 기본값으로 채택하지 않았습니다. WallJumpPickup 획득 안내 길이는 해당 컴포넌트 Inspector의 announcementSeconds(초)입니다. V3 방 시간·출구·사망 위치는 `Playtest/` CSV에 기록됩니다.



