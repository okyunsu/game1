# 벽 점프 레벨 메트릭 — Batch06b

2026-10-10 / Unity Editor60fps / 기존 PlayerTuning 기준1 및 Short3.5u/s·0.06s 후보2, rising=false. 기준값과 후보 원본 Asset을 변경하지 않고 복사본·격리12u 벽/안전바닥/발판에서 측정했다. Batch07 치수 참고이며 사용자 손맛 수락 전 기본 레벨을 확정하지 않는다.

긴 킥: 벽 반대 입력으로 점프 후 계속 멀어짐, 출발 높이까지 돌아오는 동안의 중심 최대 수평거리. 짧은 킥: 벽 쪽 입력 유지, 같은 벽에 다시 붙을 때까지의 최대 이격/상승과 재부착 높이 이득. 두 킥 모두 버튼 홀드·점프컷 없음, 더블 점프 없음. 각 값 링크는 원본 MEASURE 줄이다.

| 측정 | 기준1 | 후보2 |
|---|---|---|
| 긴 킥 최대 수평 | [4.891u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:4) | [4.891u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:73) |
| 긴 킥 최고 상승 | [2.470u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:4) | [2.470u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:73) |
| 짧은 킥 최대 이격 | [1.560u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:7) | [0.314u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:76) |
| 짧은 킥 최고 상승 / 재부착 이득 | [2.470 / 2.078u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:7) | [2.470 / 2.428u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:76) |
| 벽 점프→대시 최대 검증 틈 | [8.05u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:70) | [8.05u](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:139) |

대시 틈은 출발/착지 발판 같은 높이, 폭3u 착지 발판에 실제 접지 확인. 발동 지연0~0.8s를0.05s 간격으로 비교했고 최대 표본0.55s에서8.299u 공중 이동,0.22u 여유를 빼고0.05u 단위로 내린8.05u에 실제 착지했다. 연속 모든 타이밍의 수학적 상한이나 사용자 안정 성공 거리를 뜻하지 않는다.

| 굴뚝 안쪽 폭 / 높이12u | 기준1 같은 벽 반복 | 후보2 같은 벽 반복 | 양쪽 왕복 자동 입력 기준1 / 후보2 |
|---|---|---|---|
| 3u | [완등5회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:9) | [완등5회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:78) | [12s 미완등9회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:8) / [12s 미완등4회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:77) |
| 4u | [완등5회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:11) | [완등5회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:80) | [완등5회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:10) / [완등6회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:79) |
| 5u | [완등5회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:13) | [완등5회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:82) | [완등9회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:12) / [12s 미완등7회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:81) |
| 6u | [완등5회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:15) | [완등5회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:84) | [12s 미완등2회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:14) / [12s 미완등1회](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:83) |

같은 벽을 반복하면3/4/5/6u 모두 완등했다. 폭만 넓혀 벽 점프 능력 획득 후 등반을 막을 수는 없다. 양쪽 왕복 미완등은 이 고정 입력의12초 관찰 결과이며 플레이 불가능 판정이 아니다. 레벨 배치·표식·착지 발판은 Batch07에서 이 조건을 확인하고 확정한다.

## 원본 MEASURE

전체 원본: [BL1-metrics-a2.txt](C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Validation/_raw/B06b/BL1-metrics-a2.txt:4). 첫 측정(a1)은 대시 표본 끝0.4s가 최대라 표본 확대 후 a2를 최종 표로 사용했다. 모두 Error/Exception/Assert=0.

```text
MEASURE preset=1 long horizontal=4.891083 peakHeight=2.47
MEASURE preset=1 short horizontal=1.560028 peakHeight=2.47 reattachGain=2.078001
MEASURE preset=1 chimneyWidth=3 strategy=alternating climbed=False jumps=9 peak=8.89491 observationLimit=12s
MEASURE preset=1 chimneyWidth=3 strategy=sameWall climbed=True jumps=5 peak=13.94491 observationLimit=12s
MEASURE preset=1 chimneyWidth=4 strategy=alternating climbed=True jumps=5 peak=13.86091 observationLimit=12s
MEASURE preset=1 chimneyWidth=4 strategy=sameWall climbed=True jumps=5 peak=13.94491 observationLimit=12s
MEASURE preset=1 chimneyWidth=5 strategy=alternating climbed=True jumps=9 peak=13.93292 observationLimit=12s
MEASURE preset=1 chimneyWidth=5 strategy=sameWall climbed=True jumps=5 peak=13.88686 observationLimit=12s
MEASURE preset=1 chimneyWidth=6 strategy=alternating climbed=False jumps=2 peak=6.51491 observationLimit=12s
MEASURE preset=1 chimneyWidth=6 strategy=sameWall climbed=True jumps=5 peak=13.94486 observationLimit=12s
MEASURE preset=1 dashDelay=0 equalHeightAirRange=3.847992
MEASURE preset=1 dashDelay=0.05 equalHeightAirRange=4.807983
MEASURE preset=1 dashDelay=0.1 equalHeightAirRange=5.247986
MEASURE preset=1 dashDelay=0.15 equalHeightAirRange=5.89917
MEASURE preset=1 dashDelay=0.2 equalHeightAirRange=6.37915
MEASURE preset=1 dashDelay=0.25 equalHeightAirRange=6.739136
MEASURE preset=1 dashDelay=0.3 equalHeightAirRange=7.099121
MEASURE preset=1 dashDelay=0.35 equalHeightAirRange=7.459106
MEASURE preset=1 dashDelay=0.4 equalHeightAirRange=7.699097
MEASURE preset=1 dashDelay=0.45 equalHeightAirRange=7.819092
MEASURE preset=1 dashDelay=0.5 equalHeightAirRange=8.179077
MEASURE preset=1 dashDelay=0.55 equalHeightAirRange=8.299072
MEASURE preset=1 dashDelay=0.6 equalHeightAirRange=8.299072
MEASURE preset=1 dashDelay=0.65 equalHeightAirRange=8.299072
MEASURE preset=1 dashDelay=0.7 equalHeightAirRange=4.891083
MEASURE preset=1 dashDelay=0.75 equalHeightAirRange=4.891083
MEASURE preset=1 dashDelay=0.8 equalHeightAirRange=4.891083
MEASURE preset=1 wallJumpDash validatedGap=8.05 delay=0.55 resolution=0.05 clearance=0.22
MEASURE preset=2 long horizontal=4.891083 peakHeight=2.47
MEASURE preset=2 short horizontal=0.3140259 peakHeight=2.47 reattachGain=2.428
MEASURE preset=2 chimneyWidth=3 strategy=alternating climbed=False jumps=4 peak=8.94486 observationLimit=12s
MEASURE preset=2 chimneyWidth=3 strategy=sameWall climbed=True jumps=5 peak=13.94891 observationLimit=12s
MEASURE preset=2 chimneyWidth=4 strategy=alternating climbed=True jumps=6 peak=13.93291 observationLimit=12s
MEASURE preset=2 chimneyWidth=4 strategy=sameWall climbed=True jumps=5 peak=13.99891 observationLimit=12s
MEASURE preset=2 chimneyWidth=5 strategy=alternating climbed=False jumps=7 peak=6.614942 observationLimit=12s
MEASURE preset=2 chimneyWidth=5 strategy=sameWall climbed=True jumps=5 peak=13.94894 observationLimit=12s
MEASURE preset=2 chimneyWidth=6 strategy=alternating climbed=False jumps=1 peak=6.564909 observationLimit=12s
MEASURE preset=2 chimneyWidth=6 strategy=sameWall climbed=True jumps=5 peak=13.94891 observationLimit=12s
MEASURE preset=2 dashDelay=0 equalHeightAirRange=3.447998
MEASURE preset=2 dashDelay=0.05 equalHeightAirRange=4.807983
MEASURE preset=2 dashDelay=0.1 equalHeightAirRange=5.527985
MEASURE preset=2 dashDelay=0.15 equalHeightAirRange=5.779175
MEASURE preset=2 dashDelay=0.2 equalHeightAirRange=6.37915
MEASURE preset=2 dashDelay=0.25 equalHeightAirRange=6.739136
MEASURE preset=2 dashDelay=0.3 equalHeightAirRange=7.099121
MEASURE preset=2 dashDelay=0.35 equalHeightAirRange=7.459106
MEASURE preset=2 dashDelay=0.4 equalHeightAirRange=7.699097
MEASURE preset=2 dashDelay=0.45 equalHeightAirRange=7.819092
MEASURE preset=2 dashDelay=0.5 equalHeightAirRange=8.179077
MEASURE preset=2 dashDelay=0.55 equalHeightAirRange=8.299072
MEASURE preset=2 dashDelay=0.6 equalHeightAirRange=8.299072
MEASURE preset=2 dashDelay=0.65 equalHeightAirRange=8.299072
MEASURE preset=2 dashDelay=0.7 equalHeightAirRange=4.891083
MEASURE preset=2 dashDelay=0.75 equalHeightAirRange=4.891083
MEASURE preset=2 dashDelay=0.8 equalHeightAirRange=4.891083
MEASURE preset=2 wallJumpDash validatedGap=8.05 delay=0.55 resolution=0.05 clearance=0.22
```
