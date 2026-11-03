# B26-T2 draft6.1 실제 입력 측정

| 구간 | 결과 | 머리 | 피격 | 근거 |
| --- | --- | --- | --- | --- |
| B02 바닥→섬→바닥 | 5/5 | 0/5 | 0/5 | T2-B02-floor-r2 |
| B02 위 통로→조각→끊긴 곳 하강→바닥 | 5/5 | 0/5 | E3 0/5 | T2-B02-upper-r1 |
| B06 3회 포고쌍→끝 바닥·조각 | 5/5 | 0/5 | 끝 바닥 각1 | T2-B06-r1 |
| B05 굴뚝→왼쪽 기둥 꼭대기 조각 | 3/3 | 0/3 | 0/3 | T2-B05-r4 |

모든 원문 `Validation/_raw/26/<근거>/result.txt`, actual=exe·Error0. 매 trial 초기 진입/HP복원/능력 fixture이며 이후 경로에는 위치·HP 쓰기0. 연속 자연 수집/전체 AB 완주와 구분한다. B02 바닥은 증기 영향 밖 x13.5 시작, B05는 굴뚝 벽접촉 x12.365/y7.246에서 벽점프 소유 시작. B05 매 trial7벽점프,20.268~20.274u에서 서쪽 이탈→19.015u착지. A04 레시피는 등불쌍 중 첫 등불1포고 후 섬 착지, B06은 총3포고. 양쪽 등불 각각을 때렸다는 주장은 하지 않는다.

원문 핵심:

```
MEASURE B26 section=B02-floor trial=1/5 seconds=2.984 hits=0 head=False pos=(23.086, 1.015)
MEASURE B26 section=B02-floor trial=2/5 seconds=3.017 hits=0 head=False pos=(22.998, 1.015)
MEASURE B26 section=B02-floor trial=3/5 seconds=3.000 hits=0 head=False pos=(22.974, 1.015)
MEASURE B26 section=B02-floor trial=4/5 seconds=2.984 hits=0 head=False pos=(23.086, 1.015)
MEASURE B26 section=B02-floor trial=5/5 seconds=3.017 hits=0 head=False pos=(22.998, 1.015)
```

```
MEASURE B26 section=B02-upper trial=1/5 seconds=9.067 hits=0 head=False pos=(24.126, 1.015)
MEASURE B26 section=B02-upper trial=2/5 seconds=7.251 hits=0 head=False pos=(24.121, 1.015)
MEASURE B26 section=B02-upper trial=3/5 seconds=9.617 hits=0 head=False pos=(24.105, 1.015)
MEASURE B26 section=B02-upper trial=4/5 seconds=7.250 hits=0 head=False pos=(24.121, 1.015)
MEASURE B26 section=B02-upper trial=5/5 seconds=9.552 hits=0 head=False pos=(24.121, 1.015)
```

```
MEASURE B26 section=B06 trial=1/5 seconds=7.517 hits=1 head=False pos=(31.067, 1.063)
MEASURE B26 section=B06 trial=2/5 seconds=7.450 hits=1 head=False pos=(31.150, 1.015)
MEASURE B26 section=B06 trial=3/5 seconds=7.518 hits=1 head=False pos=(31.067, 1.063)
MEASURE B26 section=B06 trial=4/5 seconds=7.451 hits=1 head=False pos=(31.150, 1.015)
MEASURE B26 section=B06 trial=5/5 seconds=7.451 hits=1 head=False pos=(31.150, 1.015)
```

```
MEASURE B26 section=B05 trial=1/3 seconds=5.051 hits=0 head=False pos=(9.878, 19.015)
MEASURE B26 section=B05 trial=2/3 seconds=5.050 hits=0 head=False pos=(9.878, 19.015)
MEASURE B26 section=B05 trial=3/3 seconds=5.050 hits=0 head=False pos=(9.998, 19.015)
```

최초 실패 원문은 T2-B02-floor-r1, T2-B05-r1/r2/r3에 보존. B05는3접근·마지막 동일접근1재시도 안에서 종료. 설계/보호/능력 수치 변경0.
