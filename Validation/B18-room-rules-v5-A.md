# V5 층 A 방 규칙 검사

원본: `Design/v5/rooms_v5_A.json` (읽기 전용)
SHA256: `88a9613af81c195abbff981c6c55faa398ea7651e239da4da473ec7aa3d11c00`

정적 기하 검사 후보 결과. FAIL도 설계 확정 판정이 아니며 Unity 물리·적 포고·공중 조작·벽 가림·능력 문 실제 동작은 미검증이다.
계산: WORLD_V5 3절 점프2.5u·일반 단차2u·포고2u, 기존 CombatTuning downAttackHeight=1.3u. 포고 상단=y+.5, 포고 뒤 상승 여유=2+타격높이. 등불·가시 둘 다 포함, 가로 간격3u. 일반 경로와 포고 경로를 별도로 계산한다.
모든 spawns를 각각 시작점으로 검사한다. fall은 x 아래 첫 고체/발판/온전한 봉인·덮개·부서지는 벽 상단을 착지점으로 삼는다. 벽 가림·충돌·적 이동·입력 타이밍을 풀지 않는 낙관적 경로 후보이며 실제 문 gate 차단은 T3에서 검사한다.
밀도 요소=아이템 marks(조각/사당)+기존 11종. 높이20 초과는 아래·위 절반에 아이템 각1개를 검사한다.

draft2 추가 규칙: 몸체 (0.7, 1.6)u를 프리팹에서 읽음. 머리 여유=키+.6u, 방 높이≥12, 도랑 위 바닥 금지, 바닥 구덩이 순찰 교차 금지·폭≤3. 맞닿은 고체의 내부 면은 서 있는 면에서 제외한다.

| 방 | 면적 u² | 요소 | /1000u² | 문 | FAIL | WARN | 판정 |
|---|---:|---:|---:|---:|---:|---:|---|
| A01 | 768 | 3 | 3.906 | 3 | 0 | 0 | PASS |
| A02 | 1008 | 7 | 6.944 | 5 | 9 | 0 | FAIL |
| A03 | 1000 | 5 | 5.000 | 5 | 0 | 0 | PASS |
| A04 | 600 | 9 | 15.000 | 2 | 0 | 0 | PASS |
| A05 | 600 | 5 | 8.333 | 3 | 0 | 0 | PASS |
| A06 | 672 | 8 | 11.905 | 2 | 13 | 0 | FAIL |

```text
MEASURE A01 arrival: start=A01-A02 fall=False seed=[2] ordinary=11 pogo=11
MEASURE A01 arrival: start=A01-A06 fall=True seed=[9] ordinary=11 pogo=11
MEASURE A01 arrival: start=A01-Z01 fall=False seed=[1] ordinary=11 pogo=11
MEASURE A01 arrival: start=start fall=False seed=[1] ordinary=11 pogo=11
MEASURE A02 arrival: start=A01-A02 fall=False seed=[0] ordinary=9 pogo=9
FAIL A02 reachability: start=A01-A02 surface=2 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A01-A02 surface=3 x=1 top=35 absent in pogo envelope
MEASURE A02 arrival: start=A02-A03 fall=False seed=[1] ordinary=9 pogo=9
FAIL A02 reachability: start=A02-A03 surface=2 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-A03 surface=3 x=1 top=35 absent in pogo envelope
MEASURE A02 arrival: start=A02-A03U fall=False seed=[8] ordinary=9 pogo=9
FAIL A02 reachability: start=A02-A03U surface=2 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-A03U surface=3 x=1 top=35 absent in pogo envelope
MEASURE A02 arrival: start=A02-B05 fall=True seed=[9] ordinary=9 pogo=9
FAIL A02 reachability: start=A02-B05 surface=2 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-B05 surface=3 x=1 top=35 absent in pogo envelope
MEASURE A02 arrival: start=A02-A06 fall=False seed=[2] ordinary=10 pogo=10
FAIL A02 reachability: start=A02-A06 surface=3 x=1 top=35 absent in pogo envelope
MEASURE A03 arrival: start=A02-A03 fall=False seed=[0] ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A02-A03 surface=11 x=1 top=15
MEASURE A03 arrival: start=A02-A03U fall=False seed=[7] ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A02-A03U surface=11 x=1 top=15
MEASURE A03 arrival: start=A05-A03 fall=False seed=[2] ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A05-A03 surface=11 x=1 top=15
MEASURE A03 arrival: start=A03-A04 fall=False seed=[16] ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A03-A04 surface=11 x=1 top=15
MEASURE A04 arrival: start=A03-A04 fall=False seed=[2] ordinary=10 pogo=14
MEASURE A05 arrival: start=A04-A05 fall=True seed=[5] ordinary=8 pogo=8
MEASURE A05 arrival: start=A05-A03 fall=False seed=[2] ordinary=8 pogo=8
FAIL A06 height: platform=3 nearest lower rise=4 (static candidate)
FAIL A06 height: platform=4 nearest lower rise=4 (static candidate)
MEASURE A06 arrival: start=A01-A06 fall=False seed=[5] ordinary=1 pogo=1
FAIL A06 reachability: start=A01-A06 surface=0 x=9 top=0.4 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=1 x=3 top=3 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=2 x=24.5 top=3 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=3 x=0 top=1 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=4 x=22 top=1 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=6 x=1 top=7 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=7 x=6.5 top=5 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=8 x=28 top=3.9 absent in pogo envelope
FAIL A06 reachability: start=A01-A06 surface=9 x=40.9 top=5 absent in pogo envelope
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
FAIL synthetic edge-headroom: surface=1 overhead=2 gap=0.5 clearance=1.5 required=2.2
FAIL synthetic enemy-inside-solid: enemy=0 x=6 feet=1 solid=[1]
FAIL synthetic enemy-support: enemy=0 x=8 feet=1 no standing surface
FAIL synthetic patrol-support: enemy=0 patrol=[3, 8] feet=1 leaves standing surface
WARN synthetic pogo-chain: consecutive lanterns=0,1,2
WARN synthetic pogo-only-climb: fromTop=1 toTop=7.5 rise=6.5 no small platform in local climb
PASS draft2 five synthetic rules detected; draft3 four JSON fixtures detected; clean fixture PASS

## B18 추가 규칙 결과

|방|draft3 추가 FAIL|추가 WARN|
|---|---:|---:|
|A01|0|0|
|A02|0|0|
|A03|0|0|
|A04|0|0|
|A05|0|0|
|A06|0|0|

추가 규칙 FAIL0/WARN0. 별도 JSON fixture4개를 실제 저장·재독해 가로0.5/머리1.5, 블록 안 적, 공중 적·순찰 바닥 이탈, 등불3연속을 모두 검출했다. 깨끗한 fixture PASS. 적 몸은 프리팹에서 읽으며 순찰은 같은 높이의 이어진 면 전체를 검사한다.
T5 PARTIAL: 신규 규칙 기준은 통과했지만 기존 도달성20·단차2의 정적 FAIL22가 그대로 남아 전체 FAIL0 기준은 충족하지 않는다. 기존 규칙을 완화하지 않았으며 실제 불가능 판정은 아니다. Design 읽기 전용 유지.

최종 코드 자체검사 재실행 PASS. [최종 원문](_raw/B18/T5-rules-final.md). Quiet 실제 REGRESSION 13/13. [원문](_raw/regression/B18-T5/result.txt).
