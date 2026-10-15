# Batch 15 결과 — 월드 지도

**종료 회귀: Quiet 13/13 + 기본 13/13. 최종 12런처+NoMap·지도4모드 Error0. Release 표시/검증6타입0.**
유형별 Task: **feature 7 / verification 6 / doc 1 / cleanup 0 = 14**. 상태: DONE 10 / REVIEW 4 / BLOCKED 0.

<!-- CARD START -->
```text
BATCH CARD 299a4f4f14c2e92a08d30cbce70fcae07aa48ed9..HEAD (194fcaf)
변경 파일: 31 (+1368 / -34 줄, 바이너리 0)
  Scripts: 16
  Scenes: 0
  Prefabs: 0
  Editor: 5
  Validation: 2
  문서: 3
  기타: 5
허용 범위: Assets/Scripts, Assets/Editor, Assets/Maps, Assets/Maps.meta, Assets/Scenes/_tmp-gen-check, Launchers/current/Start-V4.ps1, Tools/Check-RoomRules.py, README.md, TASKS.md, Validation/B15-summary.md, Validation/B15-room-rules-v4-baseline.md, Builds, Batches/BATCH-15.md
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 28 / REVIEW 4 / DOING 0 / BLOCKED 1 / PARTIAL 8
결정 대기(## 2. 결정 대기 표의 D- 행): 0
열린 버그(## 4. 열린 버그 표 행): 4
직접 플레이 확인 대기(## 3. 목록 항목): 5
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 13/13 (Validation/_raw/regression/B15-final-default/result.txt)
```
<!-- CARD END -->

시작: 2026-10-15 23:33:54 KST. Base: `299a4f4f14c2e92a08d30cbce70fcae07aa48ed9`.
종료: 2026-10-16 01:02:48 KST.
B14 이후 기존 문서 커밋 2개는 보존. 지시문은 `56f3055`로 먼저 커밋. 이번 배치만 합쳐 `Batch 15: world map`, 태그 `B15`. push 없음.
카드는 합치기 직전 커밋 기준이며 최종 커밋은 카드·종료 결과·시각 기록을 포함한다. 카드의 열린 버그 수는 단순 행 수로 CLOSED 1행도 포함한다(배치 지시대로 상태만 변경).

## 직접 플레이 확인

[TASKS 인계](../TASKS.md)의 기존 대기 순서 유지. 추가 약10분: [Play-V4](../Launchers/current/Play-V4.cmd) 새 게임 → M3상태 → 방 전환 지도 변화 → 큰 지도 가림 정도 → `Play-V4.cmd -NoMap`.
T2/T3/T5/BL4의 감각·시인성 수락은 사용자 판단. 기존 AfterDash 확인에서 D 열림색·CP도 함께 볼 수 있다.

## Task별 결과

모든 14 Task의 Quiet 회귀는 각각13/13. 아래 커밋은 합치기 전 로컬 커밋이며 최종 인계 커밋은 `B15`.

| Task | 유형 | 상태 | 사전 커밋 | 주요 파일 | 실제 결과 |
|---|---|---|---|---|---|
| T1 | feature | DONE | 281eb9d | MapLayoutBuilder, WorldMapLayout, Assets/Maps | 21방·32문(엘리베이터1) 좌표 보존, 재생성 SHA 동일, v6 생성·삭제 |
| T2 | feature | REVIEW | 42a7a7b | V4WorldMap, V4MiniMap | 방문1→2→3·현재 방·플레이어 점·M3상태·시간/입력맵 유지 exe PASS |
| T3 | feature | REVIEW | e68820c / 8d32ac8 | V4WorldMap, WorldMapSource, B09LaunchVerifier | 문·미방문 ?·21방/31문 all·no-map/비V4 오브젝트0, 가장자리 여백 보정 |
| T4 | verification | DONE | e2e91ef | B15MapVerifier | 실제 exe 10초·M3회 Error0, 촬영 KNOWN-LIMIT(1회) |
| T5 | feature | REVIEW | a524854 | Start-V4.ps1, B10StageVerifier | Development 전달·NoMap·3런처10초 Error0 |
| T6 | doc | DONE | bb4c578 | TASKS, README | D-B3 근거 정리·D-V5-PILOT 기록·사용법2줄·인계 |
| T7 | verification | DONE | 09ad039 | MapLayoutBuilder, TASKS | 본 Task 종료 기본13/13·Quiet13/13·12런처·Release 표시/검증 제외 |
| BL1 | feature | DONE | 59bc757 | B09Authoring, V4SceneMap, B15AuthoringCheck | 생성 인자화·방 목록 TextAsset, 21씬 SHA 동일·임시21방 요소 동일·저장/Continue |
| BL2 | feature | DONE | 3a42bf2 | PlaytestLogger, V4WorldMap, B15LogVerifier | CSV9열 불변·실제 입력3방/2문·지도3변경/2열기·큰 지도 시간 |
| BL3 | verification | DONE | 36fb172 | Check-RoomRules.py, 방 기준선 | 21방·37756u²·105요소·64문 표식·원본 불변·판정 없음 |
| BL4 | feature | REVIEW | e7a12b8 | V4WorldMap, WorldMapSource, B15MapVerifier | 저장 능력 문 열림색·CP, 능력0/Dash·Continue·off/비V4 PASS |
| BL5a | verification | DONE | 34d8edb | B04BossVerifier | 추적60/30/120 회피 피해0·회복 반격·Pause·사망복귀·10회Error0 |
| BL5b | verification | DONE | c89603c | B04BossVerifier | 2페이즈30/120 전환1·추적4·도약3·회복 측정·사망 초기화 |
| BL5c | verification | DONE | 0086b09 | B04BossVerifier | 8패턴60fps 3바퀴, 매 바퀴 탄/낙하물/파동0·Error0 |

## 구현·데이터 계약

- 지도 좌표 원본은 Design/world_v7.json. Editor 메뉴 `Afterglow > Build Map Layout From Design` → [TextAsset](../Assets/Maps/world_v7.json). 런타임 선택은 기본 world_v7, 인자 `-map-layout=<이름>`. 좌표 계산·변경 없음. v6 산출물은 확인 후 삭제.
- 빌드 씬 복사본에 TextAsset 참조를 주입하고 로드 후 데이터 연결 오브젝트를 제거한다. 원본 Scene·Prefab 변경0. Release에는 표시·검증 타입이 없으며 레이아웃 데이터는 V4 방 목록에 사용한다.
- 기본 작게240×150, 우상단·현재 방 중심. M: 작게→크게→끔→작게. 크게는 화면 가로/세로70%, 방문 방 전체 자동 스케일·반투명. 현재 방 노랑 테두리·플레이어 초록 점. 미방문 방은 숨기고 문 끝에 ?. 엘리베이터1개는 데이터에 남기고 그리지 않는다.
- T3 종료 검토: 문 경계 여백8~10px보다 ?의 최대22px가 커 잘릴 수 있어 내부 여백24px(스케일 계산에서48px 차감)로 보정. 외부 패널 크기·방 중심·월드 좌표는 동일. 최종 exe에서 M/모드 검사, 종료 Quiet/기본 회귀로 재확인.
- 저장 ProgressData의 기존 능력 플래그만 읽어 보유 문은 밝은 초록(0.35,1,0.5). 조회 간격0.25초. CP-방ID는 현재5종 모두 지도 방과 대응(실제 CP 표식 A03/B01/C01, 시작A01, 기존 개발 fixture C04). 새 저장 필드 없음.
- Scene 생성: `B09Authoring.Generate(dataPath, outputFolder, worldPath)` 또는 기존 메뉴. CLI `-room-design`, `-room-output`, `-map-design`. 기본값 rooms_v2 / Assets/Scenes/V4 / world_v7. 임시 출력은 Resources를 덮어쓰지 않으며 기존 파일 줄바꿈을 보존한다.
- CSV 기존9열 이름·순서 유지. 기존 진입 UTC·사망 방을 재사용하고 V4 퇴장·문 ID·체류를 추가. 이전 위치와 실제 문을 대조하며 직접 전환은 unobserved, 사망 복귀는 respawn으로 표시. 지도 열기는 큰 지도 진입과 Off→Small을 각각1회로 센다(초기 Small 제외).
- 빌드는 B13/B14의 B12BossAssets.Build + `-b11-candidates` 방식. [런처](../Launchers/current/Start-V4.ps1)는 기존 Builds/B12-delivery 경로 유지. Builds는 최신 B12-delivery + 직전 B13-delivery 두 개이며 임시 Release는 확인 후 정리.

### 문 gate 대응

| gate | 약자 | 잠김 색 | 원본 문 ID |
|---|---|---|---|
| Dash | D | 청록 | A03-B01 |
| WallJump | W | 노랑 | A02-B05, A02-A06, A02-B06, C03-C05 |
| DJ | J | 보라 | A01-A06, C03-C04 |
| Slam | S | 주황 | A01-Z01, A05-Z01 |
| Boss | B | 빨강 | C06-S01 |

일반 문=흰 짧은 선, down=아래 화살표, shortcut=점선. 원본 전체 대응은 `_raw/B15/map-all.txt`의 GATE 줄.

## 문제·재시도·미실시

1. 첫 제한 환경 Unity는 라이선스 초기화 뒤 종료. 일반 로컬 환경에서 정상 실행. Python도 WindowsApps 별칭이 제한 환경에서 실행되지 않아 일반 로컬 환경을 사용했다.
2. T4 초안 ScreenCapture 참조 컴파일 실패 → 호출 제거로 도구1회 보정. 화면 촬영은 KNOWN-LIMIT, 1회 시도 후 재시도 없음. 영상·스크린샷 시각 QA는 미실시이며 사용자 시인성 판단을 기다린다.
3. 첫 T1 Quiet는 위 컴파일 때문에2/13, 도구 보정 뒤 T1 재실행13/13. 최초 실패 줄:
```text
S1-07 FAIL | S1-07 FAIL exit=1 evidence=S1-07-night-S2-B15-T1.txt mode=headless
S1-08 FAIL | S1-08 FAIL exit=1 evidence=S1-08-night-S2-B15-T1.txt mode=headless
Camera FAIL | Camera FAIL exit=1 evidence=S1-06-lead0-night-S2-B15-T1.txt mode=graphics-640x360
T1 FAIL | T1 FAIL exit=1 evidence=Verification-night-T1-B15-T1.txt mode=headless
T2 FAIL | T2 FAIL exit=1 evidence=Verification-night-T2-B15-T1.txt mode=headless
T3 FAIL | T3 FAIL exit=1 evidence=Verification-night-T3-B15-T1.txt mode=headless
T4 FAIL | T4 FAIL exit=1 evidence=Verification-T4-night-S2-B15-T1.txt mode=headless
S2 FAIL | S2 FAIL exit=1 evidence=S2-01-integration-night-S2-B15-T1.txt mode=headless
F2R FAIL | F2R FAIL exit=1 evidence=F2R-feedback-night-S2-B15-T1.txt mode=headless
LOGR FAIL | LOGR FAIL exit=1 evidence=LOGR-playtest-night-S2-B15-T1.txt mode=headless
Boss FAIL | Boss FAIL exit=1 evidence=boss-recovery-night-S2-B15-T1.txt mode=headless
REGRESSION 2/13
Assets\Scripts\Verification\B15MapVerifier.cs(22,71): error CS0103: The name 'ScreenCapture' does not exist in the current context
```
4. T5 첫 Boss-B3 검사: 빌드 호출의 -b11-candidates 누락으로 CP-C04 fixture가 주입되지 않음. 기존 인자를 복구해1회 재빌드 후 통과. 보스·씬 수정0. 원문:
```text
FAIL B10StageVerifier actual=exe
FAIL System.Exception: Timeout room ready
ArgumentException: The scene is invalid.
```
5. T7 런처 검사 인자 파싱 오류1회 보정. 첫 T7-* (r1 없는 결과)는 합격 근거에서 제외. T7-*-r1 및 Final-*만 사용한다.
6. BL1 경로 이스케이프 컴파일 오류1회 보정. 첫 재생성 SHA 차이는 21씬·GuideGraph 모두 CRLF/LF만 달랐음. 즉시 원본 복원, 기존 줄바꿈 보존으로 해당 원인1회 보정해 전부 동일. 원문:
```text
FAIL BL1 generation; default SHA equality; temp output removed
FAIL Default regeneration SHA differs; originals restored
```
7. BL3 표 구분선의 여분1열을1회 보정. 첫 산출물은 `_raw/B15/BL3-baseline-first-format.md`에 보존. 최종21열·21행·원본SHA 불변.
8. B12-CHASE-VERIFY는 중앙 fixture로 60/30/120 회피·반격·Pause·복귀·10회 Error0 확인 후 CLOSED. TASKS 4절 열린 버그 표는 기존 설명·근거를 유지하고 상태만 추가. 10회 반복에는 피격이 있으나 Error0이며 회피 피해0은 별도 회피 구간에서 측정.

## 실제 핵심 수치 원문

### 방 생성·CSV

```text
T1-generation.txt
PASS rooms=21 doors=32 elevator=1 coordinates=unchanged deterministic=True v6=True deleted=True
BL2-runtime.txt
MEASURE CSV=C:\Users\gosu3\OneDrive\문서\ChatGPT\game1\Validation\_raw\playtest\20261016-001835.csv entries=3 exits=2 mapOpens=2 changes=3 largeSeconds=0.414833
Error/Exception/Assert=0
```

### 추적 회피

```text
BL5a-60.txt
MEASURE B12 pattern=4 fps=60 dodge=False hitEvents=1 hp=3 DJ=0 peak=1.810
MEASURE B12 pattern=4 fps=60 dodge=True hitEvents=0 hp=4 DJ=0 peak=1.810
MEASURE B12 isolated pattern=4 repeat=1
MEASURE B12 isolated pattern=4 repeat=2
MEASURE B12 isolated pattern=4 repeat=3
MEASURE B12 isolated pattern=4 repeat=4
MEASURE B12 isolated pattern=4 repeat=5
MEASURE B12 isolated pattern=4 repeat=6
MEASURE B12 isolated pattern=4 repeat=7
MEASURE B12 isolated pattern=4 repeat=8
MEASURE B12 isolated pattern=4 repeat=9
MEASURE B12 isolated pattern=4 repeat=10
Error/Exception/Assert=0
BL5a-30.txt
MEASURE B12 pattern=4 fps=30 dodge=True hitEvents=0 hp=4 DJ=0 peak=1.810
Error/Exception/Assert=0
BL5a-120.txt
MEASURE B12 pattern=4 fps=120 dodge=True hitEvents=0 hp=4 DJ=0 peak=1.792
Error/Exception/Assert=0
```

### 2페이즈 회복

```text
BL5b-30.txt
MEASURE phase two chase recovery=0.966668700396825
MEASURE phase two combo recovery=1.20000229591837
Error/Exception/Assert=0
BL5b-120.txt
MEASURE phase two chase recovery=0.966683000283447
MEASURE phase two combo recovery=1.20001969954649
Error/Exception/Assert=0
```

### 8패턴 3바퀴

```text
BL5c.txt
MEASURE B15 soak cycle=1 projectiles=0 falling=0 waves=0
MEASURE B15 soak cycle=2 projectiles=0 falling=0 waves=0
MEASURE B15 soak cycle=3 projectiles=0 falling=0 waves=0
Error/Exception/Assert=0
```

## BL1 임시 생성 요소 비교

기본 V4 21씬 + 기존 Resources2개 SHA256 동일. 같은 데이터를 임시 출력에 생성해 GameObject·전체 컴포넌트 수와 모든 컴포넌트 타입별 수를 비교했다. 임시 폴더는 삭제했다. 상세 SHA는 `_raw/B15/BL1-generation-r1.txt`.

| Room | V4 objects | Temp objects | V4 components | Temp components | all type counts equal |
|---|---:|---:|---:|---:|---|
| A01 | 33 | 33 | 103 | 103 | True |
| A02 | 40 | 40 | 131 | 131 | True |
| A03 | 45 | 45 | 140 | 140 | True |
| A04 | 36 | 36 | 116 | 116 | True |
| A05 | 31 | 31 | 101 | 101 | True |
| A06 | 36 | 36 | 118 | 118 | True |
| B01 | 37 | 37 | 121 | 121 | True |
| B02 | 38 | 38 | 121 | 121 | True |
| B03 | 44 | 44 | 138 | 138 | True |
| B04 | 30 | 30 | 101 | 101 | True |
| B05 | 34 | 34 | 113 | 113 | True |
| B06 | 33 | 33 | 106 | 106 | True |
| C01 | 31 | 31 | 100 | 100 | True |
| C02 | 44 | 44 | 137 | 137 | True |
| C03 | 39 | 39 | 123 | 123 | True |
| C05 | 26 | 26 | 83 | 83 | True |
| C04 | 28 | 28 | 90 | 90 | True |
| C06 | 23 | 23 | 73 | 73 | True |
| S01 | 16 | 16 | 51 | 51 | True |
| Z01 | 52 | 52 | 169 | 169 | True |
| S02 | 12 | 12 | 38 | 38 | True |

## 증거·다음 단계

- 회귀: `Validation/_raw/regression/B15-<Task>/result.txt`. T1만 `B15-T1-retry`가 최종. 본 Task 종료 기본은 B15-main-default, 종료 보정/최종은 B15-final-T3-quiet 및 B15-final-default.
- 실제 exe 원본은 `Validation/_raw/B15/`, CSV는 `Validation/_raw/playtest/`. 원본은 삭제하지 않고 Git 제외. [방 기준선](B15-room-rules-v4-baseline.md)은 판정 없는 측정 표다.
- Batch Backlog 전부 소진. TASKS8절 Approved Backlog도 추가 작업 없음. 다음 후보는 Claude가 작성할 V5 층A 설계·좌표 수신 후 파일럿. 이번에 V5 방 데이터·Scene 제작과 Sprint Gate는 진행하지 않았다.
