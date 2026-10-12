# B10-T2 — 재개 설계 70경로 전수 판정

상태 DONE: 모든 경로에서 성공한 저장 입력의 전체 경로 2회 연속 재생 증거를 확보했다. 변경된 10개 방의 이전 PASS는 사용하지 않았다. 중간 waypoint 배치 없이 경로 첫 준비 위치만 배치했다. 자연 완주·최소 능력·프레임 안정성과 구분한다.

| 방 | PASS / 전체 |
|---|---|
| A01 | 5 / 5 |
| A02 | 3 / 3 |
| A03 | 3 / 3 |
| A04 | 3 / 3 |
| A05 | 2 / 2 |
| A06 | 3 / 3 |
| B01 | 5 / 5 |
| B02 | 4 / 4 |
| B03 | 2 / 2 |
| B04 | 3 / 3 |
| B05 | 2 / 2 |
| B06 | 3 / 3 |
| C01 | 3 / 3 |
| C02 | 4 / 4 |
| C03 | 5 / 5 |
| C05 | 3 / 3 |
| C04 | 4 / 4 |
| C06 | 1 / 1 |
| S01 | 1 / 1 |
| S02 | 1 / 1 |
| Z01 | 10 / 10 |

전체 70/70. 실제 막힘으로 확정한 T2 경로 0. C04의 32분할 탐색 중 성공 입력을 발견했으므로 데이터 이동·폭 변경 없이 끝났다.

마지막 원문:
```
PASS route C04/dj replay=2 segments=5 whole-input=Validation/_raw/B10/inputs\C04_dj-whole-0810076038-2a928505.json
MEASURE observed active render-fps=60.0356978258343 frames=112044
MEASURE B10 mode=routes PASS=1 BLOCKED-GEOMETRY=0 budget-deferred=0
Error/Exception/Assert=0
```

입력 목록: [_raw/B10/resume-current-r68.tsv](_raw/B10/resume-current-r68.tsv). C04 증거: [_raw/B10/T2-resume-r64-C04-grid-p0.txt](_raw/B10/T2-resume-r64-C04-grid-p0.txt). 나머지 경로 원문은 `T2-resume*.txt`; r54~r58 빌드 결과는 Scene 원본 오류로 제외했다. 기존 로그와 입력 파일은 삭제하지 않았다.

추가 안정성 한계: Run-Regression -Tag V4의 별도 동일60fps 전체 재생은 63/69 PASS,6 FAIL였다. T2의 두 번 통과 증거를 전체 반복 안정성으로 확대하지 않는다. 30/120fps와 새 C04 입력은 T10 표로 기록한다.
