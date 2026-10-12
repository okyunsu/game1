# B10-T3 — 생성기 수정과 재개 데이터 적용

상태 DONE. covered floor 문 trigger가 덮개/봉인 바닥 착지에서 먼저 발동하는 생성기 배치 오차(c)를 .1u inset으로 수정했다(47b20ac). 재개 지시의 Claude 수정은 662cc0c, 현재 V4 Scene 재생성은 24ceeda에 보존했다. `@lift`는 상층 레버 공격→탑승→도착 대기로 검사했다.

검사기는 재개 적용 시 21방 PASS(`_raw/B10/resume-check-1300.txt`). 에이전트가 검사기 상수를 바꾸거나 이번 재개 중 지형을 추가 이동한 내역은 없다. T2는 현재 설계 70/70 저장 입력 전체 재생2회 PASS이며, C04도 입력 탐색으로 통과해 데이터 이동 예외를 쓰지 않았다.

| 방 | Scene 추가/삭제 줄 | 재판정 |
|---|---|---|
| A01 |363/224| PASS |
| A02 |8/8| PASS |
| A05 |8/8| PASS |
| A06 |2/2| PASS |
| B01 |0/0; 경로 데이터만 변경| PASS |
| B02 |6/6| PASS |
| B03 |322/350| PASS |
| C01 |0/0; 경로 데이터만 변경| PASS |
| C02 |6/6| PASS |
| Z01 |540/401| PASS |

그 외 A03/A04/B04/B05/B06/C03/C04/C05/C06/S01/S02의 Scene 차이는 모두0. 원문: [_raw/B10/resume-scene-differences-r40.txt](_raw/B10/resume-scene-differences-r40.txt).

수정 전 실제 핵심 수치: B02 전체4098 후보 실패, `closest=.01548716`, 위치 `(18.00,1.01)`에서 착지 즉시 전환. 원문 `_raw/B10/T3-B02-full-grid-r26.txt`. 그때의 `travel=84.5`는 전환 후 다른 방 좌표가 섞인 값이므로 방 안 이동 근거로 쓰지 않았다. 수정 후 측정은 현재 방 좌표만 집계한다.

원본 데이터 수정 이유·요소 이름은 [Claude 내역](../Design/CHANGES_CLAUDE_B10.md), 생성기 전후는 [CHANGES_B10](../Design/CHANGES_B10.md). 자연 완주·모든 입구 탈출·프레임 안정성은 별도 Task이며 이 결과로 승격하지 않는다.
