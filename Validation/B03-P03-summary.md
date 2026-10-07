# B03-P03 — DONE (진단/1회 입력 후보 비교, 간헐 실패는 미해결)

게임코드 수정0, 원본 `Validation/N9-pogo-library.json` 수정0. tracked game Scripts(Verification 제외)·Scenes·Prefabs·ScriptableObjects·Input·Packages·ProjectSettings SHA256 before/after 전부 일치 (`_raw/B03/P03-game-hashes-before/after.txt`). 새 opt-in diagnostic만 추가, actual exe B03-B2-final의 PogoTest를 60fps/1280×720/D3D11로 실행. 보스 후보 매핑은 PogoTest/플레이어 튜닝에 영향 없음. BeginSection(2)는 전용 코스가 제공하는 독립 시작 fixture, 이후 실제 원본 고정 틱 입력만 재생.

| 회차 | 원본87틱 | 중립5틱 추가 후보92틱 |
|---|---|---|
| 1 | PASS 1.815s | PASS 1.815s |
| 2 | PASS 1.815s | FAIL 1.915s (기록 소진) |
| 3 | PASS 1.815s | PASS 1.816s |
| 4 | PASS 1.815s | PASS 1.815s |
| 5 | PASS 1.815s | PASS 1.815s |
| 6 | PASS 1.815s | PASS 1.815s |
| 7 | PASS 1.815s | PASS 1.815s |
| 8 | PASS 1.815s | PASS 1.815s |
| 9 | PASS 1.815s | PASS 1.815s |
| 10 | PASS 1.815s | PASS 1.815s |

보정 전100%(10/10), 후90%(9/10). 작은 표본이고 실행 시점이 달라 **보정이 실패를 유발/개선했다고 인과 확정하지 않음**. 후보는 안정화 해결안으로 채택하지 않는다. 실패 순간 입력·위치·속도는 `P03-after-2.txt/.log`; 성공 포함 각 실행은 `P03-before/after-1~10.txt/.log`, 매 물리틱 TRACE 기록. 기존 Batch02 전체코스4/5는 독립 P03 표본과 조건이 달라 같은 분모로 합산하지 않음.

가설1: 입력이 실제 적용되는 물리틱이 1틱 달라져 점프/포고/대시 높이가 변하고, **고정 길이 기록이 착지/Update 목표 판정 직전에 끝나 false failure를 만들 수 있다**. 근거: 같은 held34 입력 tick15에서 성공 y2.055/vy12/groundFalse, 실패 y1.815/vy0/groundTrue; 포고 tick52는 둘 다 수행됐지만 y2.203 vs2.485, 대시 높이6.307 vs6.589. 실패에서는 땅에 닿기 직전 기록이 소진되고 `.log`에 이후 `PogoTest P03 goal`이 남는다. 기존 B02/Pogo-3.log도 FAIL 검사 뒤 goal 로그가 존재한다. 엔진/게임의 정확한 입력 소비 원인까지 확정한 것은 아님.

핵심 원문:
```
TRACE tick=15 held=34 axis=1 x=55.63199 y=2.05500 vx=6.00000 vy=12.00000 ground=False DJ=True dash=False pogos=0
TRACE tick=15 held=34 axis=1 x=55.63199 y=1.81500 vx=6.00000 vy=0.00000 ground=True DJ=True dash=False pogos=0
FAIL-FRAME tick=92 held=0 axis=0 x=64.82394 y=6.40900 vx=0.00000 vy=-3.60000 ground=False DJ=False dash=False pogos=1
FAIL-FRAME tick=93 held=0 axis=0 x=64.82394 y=6.31900 vx=0.00000 vy=-4.50000 ground=False DJ=False dash=False pogos=1
MEASURE P03 outcome=FAIL seconds=1.915 reason=recording exhausted
FAIL System.Exception: P03 original recording goal
```

허용된 입력 보정1회만 사용: 새 Verification/Data/B03-pogo-library.json에서 P01/P02·P03 기존87틱은 그대로, 마지막에 axis0/held0 중립5틱(.1s)을 추가해 판정 여유 가설을 시험. 1회 후보에도 여유가 부족한 실패가 있어 추가 입력 수정 없이 종료. 게임 경로/기본 라이브러리 불변, 원본 자료 삭제0. Task 완료는 진단 산출물 기준이며 P03 안정성 이슈 해소가 아니다.
