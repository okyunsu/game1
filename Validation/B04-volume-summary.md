# B04 Backlog 1 — AV1 volume audit

상태 DONE. 생성 PCM WAV 원본 peak와 profile gain을 곱한 단일 음원 peak dBFS를 계산했다. Pitch ±.04에 따른 재생 길이 변화는 포함하지 않은 원본 길이이며, 동시 믹스·실제 출력 장치 청감은 사용자 QA에서 확인한다.

| Event | Seconds | PCM peak | Gain | Peak dBFS |
|---|---:|---:|---:|---:|
| jump | 0.130 | 0.4391 | 0.30 | -17.61 |
| dash | 0.130 | 0.4499 | 0.30 | -17.39 |
| swing | 0.130 | 0.4502 | 0.30 | -17.39 |
| hit | 0.130 | 0.4515 | 0.30 | -17.36 |
| hurt | 0.130 | 0.4558 | 0.30 | -17.28 |
| death | 0.320 | 0.4535 | 0.30 | -17.33 |
| checkpoint | 0.260 | 0.4542 | 0.30 | -17.31 |
| ability | 0.260 | 0.4542 | 0.30 | -17.31 |
| bossTell | 0.260 | 0.4541 | 0.34 | -16.23 |
| victory | 0.260 | 0.4543 | 0.30 | -17.31 |

최대–최소 편차 1.38dB, 6dB 초과 outlier 없음. gain 변경0(보스 .34, 나머지 .30 유지). 길이 .130/.260/.320s로 짧은 이벤트 신호를 유지한다. 기본 audio/tuning 변경0. 원본 WAV Assets/Audio/AV1/, raw table Validation/_raw/B04/audio-peak.csv.
