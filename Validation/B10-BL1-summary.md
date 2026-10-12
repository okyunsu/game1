# B10-BL1 V4 경로 회귀 등록

상태 DONE: `Validation/Run-Regression.ps1 -Tag V4`에 현재 Release 선택·저장 입력 manifest·60fps 재생을 등록하고 1회 실행했다. 당시 manifest69개, 결과63 PASS/6 FAIL. 후속 T2에서 C04/dj를 확보해 현재 manifest는70개지만 등록 실행의 69개 결과를 그대로 보존한다. 전체 회귀 PASS 주장 없음.

실행 원본 `_raw/B10/V4-20261012-161107.txt`, 빌드 B10-release-r61. T2의 두 번 성공 증거와 반복 안정성 실패는 구분한다.

```text
FAIL fps-replay A01/main fps=60 replay=2 input=Validation/_raw/B10/inputs\A01_main-whole-0403416529-eab681a3.json
FAIL fps-replay B04/low fps=60 replay=2 input=Validation/_raw/B10/inputs\B04_low-whole-0410087642-86ac73eb.json
FAIL fps-replay B06/steam fps=60 replay=2 input=Validation/_raw/B10/inputs\B06_steam-whole-0422401702-51b386e4.json
FAIL fps-replay C01/lift fps=60 replay=2 input=Validation/_raw/B10/inputs\C01_lift-whole-0405327301-3cbdcc2e.json
FAIL fps-replay C02/mid fps=60 replay=2 input=Validation/_raw/B10/inputs\C02_mid-whole-0422278766-c35ac8e3.json
FAIL fps-replay Z01/elevator fps=60 replay=2 input=Validation/_raw/B10/inputs\Z01_elevator-whole-0425094339-c475f2c5.json
MEASURE observed active render-fps=60.0389425190342 frames=56367
MEASURE B10 mode=replay PASS=63 BLOCKED-GEOMETRY=0 budget-deferred=6
Error/Exception/Assert=0
```

미실시: 이번 등록 실행에서 C04/dj(당시 입력 미확보), 자연 전체 입력.
