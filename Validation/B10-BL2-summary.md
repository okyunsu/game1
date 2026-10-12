# B10-BL2 장치 후보 비교

상태 DONE: 현재 설계의 장치 관련 19경로를 기본/후보1/후보2에서 각각 저장 입력 2회로 비교했다. 성공한 입력을 새 수치에서 재생한 안정성 비교이며, 실패가 새 수치에서 통행 불가라는 증명은 아니다. 기본 수치/Asset 변경0. 감각 및 기본 채택은 사용자 판단 대기.

후보1: 증기1.5초 켜짐/1.5초 꺼짐, 무너짐0.4초, 승강기4u/s. 후보2: 증기2.5/2.5초, 무너짐0.8초, 승강기4u/s. 기존 MechanicsTestPresets의 런타임 복사본만 사용.

| 경로 | 기본 | 후보1 | 후보2 |
|---|---|---|---|
| A01/slam | PASS | PASS | PASS |
| A01/slam_pad | PASS | PASS | PASS |
| A04/low | PASS | PASS | PASS |
| A04/slam_pad | PASS | PASS | PASS |
| A05/main | FAIL | FAIL | FAIL |
| A06/spikes | PASS | PASS | PASS |
| B01/lift | PASS | FAIL | FAIL |
| B01/slam_pad | PASS | PASS | PASS |
| B02/pogo_enemy | PASS | FAIL | PASS |
| B02/steam | PASS | FAIL | FAIL |
| B03/top | PASS | PASS | PASS |
| B06/steam | PASS | FAIL | FAIL |
| C01/lift | FAIL | FAIL | FAIL |
| C02/mid | PASS | PASS | PASS |
| C02/up | PASS | PASS | PASS |
| Z01/elevator | PASS | FAIL | FAIL |
| Z01/lanterns | PASS | PASS | PASS |
| Z01/steam | PASS | FAIL | FAIL |
| C04/dj | PASS | PASS | PASS |

실제 결과: 기본 17/19, 후보1 11/19, 후보2 12/19 PASS. 세 실행 모두 Error/Exception/Assert=0. 기본 유지 추천: 후보가 재생 안정성을 개선하지 않았고 실제 손맛 수락이 없다.

원본 핵심 FAIL·MEASURE:
```text
BL2-resume-r71-baseline.txt
FAIL fps-replay A05/main fps=60 replay=2 input=Validation/_raw/B10/inputs\A05_main-whole-0404289103-1c2e1088.json
FAIL fps-replay C01/lift fps=60 replay=2 input=Validation/_raw/B10/inputs\C01_lift-whole-0405327301-3cbdcc2e.json
MEASURE observed active render-fps=60.0228519541418 frames=14776
MEASURE B10 mode=replay PASS=17 BLOCKED-GEOMETRY=0 budget-deferred=2
BL2-resume-r66-candidate1.txt
FAIL fps-replay A05/main fps=60 replay=2 input=Validation/_raw/B10/inputs\A05_main-whole-0404289103-1c2e1088.json
FAIL fps-replay B01/lift fps=60 replay=2 input=Validation/_raw/B10/inputs\B01_lift-whole-0403526511-313df93b.json
FAIL fps-replay B02/pogo_enemy fps=60 replay=2 input=Validation/_raw/B10/inputs\B02_pogo_enemy-whole-0410231929-09a71e3d.json
FAIL fps-replay B02/steam fps=60 replay=2 input=Validation/_raw/B10/inputs\B02_steam-whole-0410493360-f6b9a483.json
FAIL fps-replay B06/steam fps=60 replay=2 input=Validation/_raw/B10/inputs\B06_steam-whole-0422401702-51b386e4.json
FAIL fps-replay C01/lift fps=60 replay=2 input=Validation/_raw/B10/inputs\C01_lift-whole-0405327301-3cbdcc2e.json
FAIL fps-replay Z01/elevator fps=60 replay=2 input=Validation/_raw/B10/inputs\Z01_elevator-whole-0425094339-c475f2c5.json
FAIL fps-replay Z01/steam fps=60 replay=2 input=Validation/_raw/B10/inputs\Z01_steam-whole-0422444473-735f132e.json
MEASURE observed active render-fps=60.0267329557891 frames=13673
MEASURE B10 mode=replay PASS=10 BLOCKED-GEOMETRY=0 budget-deferred=8
BL2-resume-r66-candidate2.txt
FAIL fps-replay A05/main fps=60 replay=2 input=Validation/_raw/B10/inputs\A05_main-whole-0404289103-1c2e1088.json
FAIL fps-replay B01/lift fps=60 replay=2 input=Validation/_raw/B10/inputs\B01_lift-whole-0403526511-313df93b.json
FAIL fps-replay B02/steam fps=60 replay=2 input=Validation/_raw/B10/inputs\B02_steam-whole-0410493360-f6b9a483.json
FAIL fps-replay B06/steam fps=60 replay=2 input=Validation/_raw/B10/inputs\B06_steam-whole-0422401702-51b386e4.json
FAIL fps-replay C01/lift fps=60 replay=2 input=Validation/_raw/B10/inputs\C01_lift-whole-0405327301-3cbdcc2e.json
FAIL fps-replay Z01/elevator fps=60 replay=2 input=Validation/_raw/B10/inputs\Z01_elevator-whole-0425094339-c475f2c5.json
FAIL fps-replay Z01/steam fps=60 replay=2 input=Validation/_raw/B10/inputs\Z01_steam-whole-0422444473-735f132e.json
MEASURE observed active render-fps=60.0274228235389 frames=13559
MEASURE B10 mode=replay PASS=11 BLOCKED-GEOMETRY=0 budget-deferred=7
BL2-resume-r70-C04-candidate1.txt
MEASURE observed active render-fps=60.0767410146895 frames=924
MEASURE B10 mode=replay PASS=1 BLOCKED-GEOMETRY=0 budget-deferred=0
BL2-resume-r70-C04-candidate2.txt
MEASURE observed active render-fps=60.0709730745451 frames=924
MEASURE B10 mode=replay PASS=1 BLOCKED-GEOMETRY=0 budget-deferred=0
```

미실시: 후보별 새 입력 전수 탐색·사람 손맛 수락. 원본: `_raw/B10/BL2-resume-*` 및 입력19개 TSV.
