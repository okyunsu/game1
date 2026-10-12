# B10-T10 프레임별 저장 경로 재생

상태 PARTIAL: T2 70경로 전부 30/120fps 재생 2회 결과 표 완료. 자연 완주 입력은 T6 미완료로 미실시. r59 69개에 r68 C04 전체4개 결과를 경로 키별로 반영해 중복 없이70개.

30fps43 PASS/27 FAIL,120fps59 PASS/11 FAIL. 실패는 해당 저장 입력의 프레임 간 재생 차이이며 통행 불가로 해석하지 않음.

| 경로 | 30fps | 120fps |
|---|---|---|
| A01/dj | PASS | PASS |
| A01/main | FAIL | FAIL |
| A01/slam | PASS | PASS |
| A01/slam_pad | PASS | PASS |
| A01/upper | PASS | PASS |
| A02/floor | PASS | PASS |
| A02/secret | PASS | PASS |
| A02/wall | FAIL | FAIL |
| A03/high | PASS | FAIL |
| A03/roofs | PASS | PASS |
| A03/street | PASS | PASS |
| A04/high | FAIL | PASS |
| A04/low | FAIL | FAIL |
| A04/slam_pad | PASS | PASS |
| A05/main | FAIL | FAIL |
| A05/secret | FAIL | PASS |
| A06/floor | PASS | PASS |
| A06/spikes | FAIL | PASS |
| A06/up | PASS | PASS |
| B01/back | PASS | PASS |
| B01/balcony | PASS | PASS |
| B01/lift | PASS | PASS |
| B01/slam_pad | FAIL | PASS |
| B01/street | PASS | PASS |
| B02/high | FAIL | PASS |
| B02/low | FAIL | PASS |
| B02/pogo_enemy | FAIL | PASS |
| B02/steam | PASS | PASS |
| B03/climb | PASS | FAIL |
| B03/top | PASS | PASS |
| B04/low | FAIL | PASS |
| B04/practice | PASS | PASS |
| B04/wj | PASS | PASS |
| B05/climb | FAIL | PASS |
| B05/cross | PASS | PASS |
| B06/low | PASS | PASS |
| B06/steam | FAIL | FAIL |
| B06/up | PASS | PASS |
| C01/high | PASS | FAIL |
| C01/lift | FAIL | FAIL |
| C01/low | PASS | PASS |
| C02/down | FAIL | FAIL |
| C02/low | FAIL | PASS |
| C02/mid | PASS | PASS |
| C02/up | FAIL | PASS |
| C03/barracks | PASS | PASS |
| C03/c05 | PASS | PASS |
| C03/chimney | FAIL | PASS |
| C03/floor | PASS | PASS |
| C03/gj | PASS | PASS |
| C04/back | PASS | PASS |
| C04/cross | PASS | PASS |
| C04/dj | PASS | PASS |
| C04/wj | PASS | PASS |
| C05/down | PASS | PASS |
| C05/up | FAIL | PASS |
| C05/up_dj | PASS | PASS |
| C06/exit | PASS | PASS |
| S01/walk | PASS | PASS |
| S02/walk | PASS | PASS |
| Z01/back | FAIL | PASS |
| Z01/dj | FAIL | PASS |
| Z01/elevator | FAIL | FAIL |
| Z01/gallery | FAIL | PASS |
| Z01/in | FAIL | PASS |
| Z01/lanterns | FAIL | PASS |
| Z01/lower | FAIL | PASS |
| Z01/steam | PASS | PASS |
| Z01/up_A01 | PASS | PASS |
| Z01/up_A05 | PASS | PASS |

원인 범위: 입력은 render Dynamic Update에서 읽고 물리는 기존20ms 간격을 유지한다. 60fps 입력을 시간 기준으로 resample하면 프레임 경계의 점프/대시·벽 접촉·공격 처리와 실제 도착 위치가 달라진다. 예: A02/wall platform0에30/120 모두 y27.38~27.61에서 입력 종료, A03/high120은 대시 후 y14.89. 같은60fps 회귀에서도 일부 실패가 있어 fps만 단일 원인으로 확정하지 않는다. 핵심 Input System/PlayerTuning 수정0.

원본 FAIL/MEASURE:
```text
T10-resume-r59-30.txt
FAIL fps-replay A01/main fps=30 replay=2 input=Validation/_raw/B10/inputs\A01_main-whole-0403416529-eab681a3.json
FAIL fps-replay A02/wall fps=30 replay=2 input=Validation/_raw/B10/inputs\A02_wall-whole-0446146370-3b70d776.json
FAIL fps-replay A04/high fps=30 replay=2 input=Validation/_raw/B10/inputs\A04_high-whole-0409285696-6b02f9b2.json
FAIL fps-replay A04/low fps=30 replay=2 input=Validation/_raw/B10/inputs\A04_low-whole-0407350693-336b7885.json
FAIL fps-replay A05/main fps=30 replay=2 input=Validation/_raw/B10/inputs\A05_main-whole-0404289103-1c2e1088.json
FAIL fps-replay A05/secret fps=30 replay=2 input=Validation/_raw/B10/inputs\A05_secret-whole-0405139953-04def911.json
FAIL fps-replay A06/spikes fps=30 replay=2 input=Validation/_raw/B10/inputs\A06_spikes-whole-0359551042-f6b9dedb.json
FAIL fps-replay B01/slam_pad fps=30 replay=2 input=Validation/_raw/B10/inputs\B01_slam_pad-whole-0406486315-a1cc171a.json
FAIL fps-replay B02/high fps=30 replay=2 input=Validation/_raw/B10/inputs\B02_high-whole-0413335178-9f162c03.json
FAIL fps-replay B02/low fps=30 replay=2 input=Validation/_raw/B10/inputs\B02_low-whole-0409394368-7b4054ac.json
FAIL fps-replay B02/pogo_enemy fps=30 replay=2 input=Validation/_raw/B10/inputs\B02_pogo_enemy-whole-0410231929-09a71e3d.json
FAIL fps-replay B04/low fps=30 replay=2 input=Validation/_raw/B10/inputs\B04_low-whole-0410087642-86ac73eb.json
FAIL fps-replay B05/climb fps=30 replay=2 input=Validation/_raw/B10/inputs\B05_climb-whole-0427338895-10a830f9.json
FAIL fps-replay B06/steam fps=30 replay=2 input=Validation/_raw/B10/inputs\B06_steam-whole-0422401702-51b386e4.json
FAIL fps-replay C01/lift fps=30 replay=2 input=Validation/_raw/B10/inputs\C01_lift-whole-0405327301-3cbdcc2e.json
FAIL fps-replay C02/down fps=30 replay=2 input=Validation/_raw/B10/inputs\C02_down-whole-0423152909-dd6c5920.json
FAIL fps-replay C02/low fps=30 replay=2 input=Validation/_raw/B10/inputs\C02_low-whole-0448063395-b6c8130e.json
FAIL fps-replay C02/up fps=30 replay=2 input=Validation/_raw/B10/inputs\C02_up-whole-0424282816-b24f1dd5.json
FAIL fps-replay C03/chimney fps=30 replay=2 input=Validation/_raw/B10/inputs\C03_chimney-whole-0403534234-a73ebdd8.json
FAIL fps-replay C05/up fps=30 replay=2 input=Validation/_raw/B10/inputs\C05_up-whole-0403101622-558633c0.json
FAIL fps-replay Z01/back fps=30 replay=2 input=Validation/_raw/B10/inputs\Z01_back-whole-0403183863-43c281a6.json
FAIL fps-replay Z01/dj fps=30 replay=2 input=Validation/_raw/B10/inputs\Z01_dj-whole-0422350167-04a33a61.json
FAIL fps-replay Z01/elevator fps=30 replay=2 input=Validation/_raw/B10/inputs\Z01_elevator-whole-0425094339-c475f2c5.json
FAIL fps-replay Z01/gallery fps=30 replay=2 input=Validation/_raw/B10/inputs\Z01_gallery-whole-0424416500-5fb6bc45.json
FAIL fps-replay Z01/in fps=30 replay=2 input=Validation/_raw/B10/inputs\Z01_in-whole-0402582273-5ffa12de.json
FAIL fps-replay Z01/lanterns fps=30 replay=2 input=Validation/_raw/B10/inputs\Z01_lanterns-whole-0422314636-dd4ae33a.json
FAIL fps-replay Z01/lower fps=30 replay=2 input=Validation/_raw/B10/inputs\Z01_lower-whole-0424540312-41bb9f98.json
MEASURE observed active render-fps=30.0051928430751 frames=24486
MEASURE B10 mode=replay PASS=42 BLOCKED-GEOMETRY=0 budget-deferred=27
Error/Exception/Assert=0
T10-resume-r59-120.txt
FAIL fps-replay A01/main fps=120 replay=2 input=Validation/_raw/B10/inputs\A01_main-whole-0403416529-eab681a3.json
FAIL fps-replay A02/wall fps=120 replay=2 input=Validation/_raw/B10/inputs\A02_wall-whole-0446146370-3b70d776.json
FAIL fps-replay A03/high fps=120 replay=2 input=Validation/_raw/B10/inputs\A03_high-whole-0406557160-f9e7a28d.json
FAIL fps-replay A04/low fps=120 replay=2 input=Validation/_raw/B10/inputs\A04_low-whole-0407350693-336b7885.json
FAIL fps-replay A05/main fps=120 replay=2 input=Validation/_raw/B10/inputs\A05_main-whole-0404289103-1c2e1088.json
FAIL fps-replay B03/climb fps=120 replay=2 input=Validation/_raw/B10/inputs\B03_climb-whole-0407328786-053e3ff0.json
FAIL fps-replay B06/steam fps=120 replay=2 input=Validation/_raw/B10/inputs\B06_steam-whole-0422401702-51b386e4.json
FAIL fps-replay C01/high fps=120 replay=2 input=Validation/_raw/B10/inputs\C01_high-whole-0403494751-6bae0997.json
FAIL fps-replay C01/lift fps=120 replay=2 input=Validation/_raw/B10/inputs\C01_lift-whole-0405327301-3cbdcc2e.json
FAIL fps-replay C02/down fps=120 replay=2 input=Validation/_raw/B10/inputs\C02_down-whole-0423152909-dd6c5920.json
FAIL fps-replay Z01/elevator fps=120 replay=2 input=Validation/_raw/B10/inputs\Z01_elevator-whole-0425094339-c475f2c5.json
MEASURE observed active render-fps=120.034870262117 frames=102067
MEASURE B10 mode=replay PASS=58 BLOCKED-GEOMETRY=0 budget-deferred=11
Error/Exception/Assert=0
T10-resume-r68-C04-30.txt
MEASURE observed active render-fps=30.0324122255167 frames=882
MEASURE B10 mode=replay PASS=4 BLOCKED-GEOMETRY=0 budget-deferred=0
Error/Exception/Assert=0
T10-resume-r68-C04-120.txt
MEASURE observed active render-fps=120.100505525797 frames=3455
MEASURE B10 mode=replay PASS=4 BLOCKED-GEOMETRY=0 budget-deferred=0
Error/Exception/Assert=0
```

미실시: 자연 전체 입력 30/120fps. 원본 입력/로그 `_raw/B10/`.
