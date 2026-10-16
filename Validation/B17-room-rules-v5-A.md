# V5 층 A 방 규칙 검사

원본: `Design/v5/rooms_v5_A.json` (읽기 전용)
SHA256: `9c5306c73ecab345ed4ad10b12ddb3671076ac7c9b792d47f7ddc941d8c3d4a2`

정적 기하 검사 후보 결과. FAIL도 설계 확정 판정이 아니며 Unity 물리·적 포고·공중 조작·벽 가림·능력 문 실제 동작은 미검증이다.
계산: WORLD_V5 3절 점프2.5u·일반 단차2u·포고2u, 기존 CombatTuning downAttackHeight=1.3u. 포고 상단=y+.5, 포고 뒤 상승 여유=2+타격높이. 등불·가시 둘 다 포함, 가로 간격3u. 일반 경로와 포고 경로를 별도로 계산한다.
모든 spawns를 각각 시작점으로 검사한다. fall은 x 아래 첫 고체/발판/온전한 봉인·덮개·부서지는 벽 상단을 착지점으로 삼는다. 벽 가림·충돌·적 이동·입력 타이밍을 풀지 않는 낙관적 경로 후보이며 실제 문 gate 차단은 T3에서 검사한다.
밀도 요소=아이템 marks(조각/사당)+기존 11종. 높이20 초과는 아래·위 절반에 아이템 각1개를 검사한다.

draft2 추가 규칙: 몸체 (0.7, 1.6)u를 프리팹에서 읽음. 머리 여유=키+.6u, 방 높이≥12, 도랑 위 바닥 금지, 바닥 구덩이 순찰 교차 금지·폭≤3. 맞닿은 고체의 내부 면은 서 있는 면에서 제외한다.

| 방 | 면적 u² | 요소 | /1000u² | 문 | FAIL | WARN | 판정 |
|---|---:|---:|---:|---:|---:|---:|---|
| A01 | 768 | 5 | 6.510 | 3 | 0 | 0 | PASS |
| A02 | 1008 | 8 | 7.937 | 5 | 9 | 0 | FAIL |
| A03 | 1000 | 10 | 10.000 | 5 | 0 | 0 | PASS |
| A04 | 600 | 10 | 16.667 | 2 | 0 | 0 | PASS |
| A05 | 600 | 5 | 8.333 | 3 | 0 | 0 | PASS |
| A06 | 672 | 8 | 11.905 | 2 | 13 | 0 | FAIL |

```text
MEASURE A01 arrival: start=A01-A02 fall=False seed=[2] ordinary=8 pogo=11
MEASURE A01 pogo-route: start=A01-A02 surface=7 x=25 top=11.6
MEASURE A01 arrival: start=A01-A06 fall=True seed=[7] ordinary=9 pogo=11
MEASURE A01 arrival: start=A01-Z01 fall=False seed=[1] ordinary=8 pogo=11
MEASURE A01 pogo-route: start=A01-Z01 surface=7 x=25 top=11.6
MEASURE A01 arrival: start=start fall=False seed=[1] ordinary=8 pogo=11
MEASURE A01 pogo-route: start=start surface=7 x=25 top=11.6
MEASURE A02 arrival: start=A01-A02 fall=False seed=[0] ordinary=5 pogo=9
FAIL A02 reachability: start=A01-A02 surface=2 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A01-A02 surface=3 x=1 top=35 absent in pogo envelope
MEASURE A02 pogo-route: start=A01-A02 surface=5 x=18 top=10.8
MEASURE A02 pogo-route: start=A01-A02 surface=8 x=13 top=9
MEASURE A02 pogo-route: start=A01-A02 surface=9 x=11.5 top=12.4
MEASURE A02 arrival: start=A02-A03 fall=False seed=[1] ordinary=5 pogo=9
FAIL A02 reachability: start=A02-A03 surface=2 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-A03 surface=3 x=1 top=35 absent in pogo envelope
MEASURE A02 pogo-route: start=A02-A03 surface=5 x=18 top=10.8
MEASURE A02 pogo-route: start=A02-A03 surface=8 x=13 top=9
MEASURE A02 pogo-route: start=A02-A03 surface=9 x=11.5 top=12.4
MEASURE A02 arrival: start=A02-A03U fall=False seed=[8] ordinary=8 pogo=9
FAIL A02 reachability: start=A02-A03U surface=2 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-A03U surface=3 x=1 top=35 absent in pogo envelope
MEASURE A02 arrival: start=A02-B05 fall=True seed=[9] ordinary=8 pogo=9
FAIL A02 reachability: start=A02-B05 surface=2 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-B05 surface=3 x=1 top=35 absent in pogo envelope
MEASURE A02 arrival: start=A02-A06 fall=False seed=[2] ordinary=9 pogo=10
FAIL A02 reachability: start=A02-A06 surface=3 x=1 top=35 absent in pogo envelope
MEASURE A03 arrival: start=A02-A03 fall=False seed=[0] ordinary=9 pogo=18
MEASURE A03 pogo-route: start=A02-A03 surface=7 x=1 top=9
MEASURE A03 pogo-route: start=A02-A03 surface=9 x=1 top=15
MEASURE A03 pogo-route: start=A02-A03 surface=11 x=43 top=13
MEASURE A03 arrival: start=A02-A03U fall=False seed=[7] ordinary=10 pogo=18
MEASURE A03 pogo-route: start=A02-A03U surface=9 x=1 top=15
MEASURE A03 pogo-route: start=A02-A03U surface=11 x=43 top=13
MEASURE A03 arrival: start=A05-A03 fall=False seed=[2] ordinary=9 pogo=18
MEASURE A03 pogo-route: start=A05-A03 surface=7 x=1 top=9
MEASURE A03 pogo-route: start=A05-A03 surface=9 x=1 top=15
MEASURE A03 pogo-route: start=A05-A03 surface=11 x=43 top=13
MEASURE A03 arrival: start=A03-A04 fall=False seed=[11] ordinary=10 pogo=18
MEASURE A03 pogo-route: start=A03-A04 surface=7 x=1 top=9
MEASURE A03 pogo-route: start=A03-A04 surface=9 x=1 top=15
MEASURE A04 arrival: start=A03-A04 fall=False seed=[2] ordinary=8 pogo=14
MEASURE A04 pogo-route: start=A03-A04 surface=8 x=21 top=6.9
MEASURE A05 arrival: start=A04-A05 fall=True seed=[5] ordinary=8 pogo=8
MEASURE A05 arrival: start=A05-A03 fall=False seed=[2] ordinary=8 pogo=8
FAIL A06 height: platform=3 nearest lower rise=4 (static candidate)
FAIL A06 height: platform=4 nearest lower rise=4 (static candidate)
MEASURE A06 arrival: start=A01-A06 fall=False seed=[5] ordinary=1 pogo=1
FAIL A06 reachability: start=A01-A06 surface=0 x=9 top=0.4 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=1 x=3 top=3 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=2 x=25 top=3 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=3 x=0 top=1 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=4 x=22 top=1 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=6 x=1 top=7 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=7 x=6.5 top=5 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=8 x=28 top=3.9 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=9 x=40 top=5 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=10 x=44.5 top=5 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 door=A02-A06 absent in pogo envelope
```

PASS self-test synthetic door-safety / overlap / height detected; intact seal traversal retained
FAIL synthetic headroom: surface=0 overhead=3 clearance=1.4 required=2.2 playerHeight=1.6
FAIL synthetic room-height: height=10 minimum=12 cameraHeight=11
FAIL synthetic covered-trench: solid=2 x=9.5..20 zone=12..16
FAIL synthetic pit-patrol: enemy=0 patrol=[4, 12] pit=7..9.5 feet=1
FAIL synthetic covered-trench: solid=2 x=9.5..20 zone=7..10.5
FAIL synthetic pit-width: x=7 width=3.5 maximum=3
PASS draft2 five synthetic rules detected; clean fixture PASS

## T4 합격 범위

추가한 draft2 규칙 위반0. 기존 검사 후보 FAIL22(도달성20·일반 단차2)는 삭제하거나 판정을 완화하지 않았다. 설계 데이터는 변경하지 않았으며 실제 물리 도달성을 확정하는 결과가 아니다.

|방|추가 규칙 FAIL|머리 여유 최솟값u|구덩이|
|---|---:|---:|---:|
|A01|0|3|1|
|A02|0|2.9|1|
|A03|0|3|2|
|A04|0|3|1|
|A05|0|3|1|
|A06|0|2.4|0|

의도적인 위반 JSON5개를 _raw에 저장·다시 읽어 검사했다. 간격1.4/높이10/도랑 위 바닥/구덩이 순찰/폭3.5 각각 FAIL 검출, 깨끗한 fixture는 PASS. 프리팹 BoxCollider2D 0.7×1.6 읽기 성공, fallback 미사용. 위험 도랑은 구덩이 폭 규칙에서 제외하고, 순찰은 바닥 높이의 적만 비교한다(상부 통로의 순찰은 바닥 구덩이와 독립). 실제 점프/복귀는 [T3](B17-probe.md)에서 별도 측정한다.

상태 DONE. Quiet 결과는 종료 후 추가한다.

Quiet B17-T4: REGRESSION 13/13.
