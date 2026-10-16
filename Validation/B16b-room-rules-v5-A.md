# V5 층 A 방 규칙 검사

원본: `Design/v5/rooms_v5_A.json` (읽기 전용)
SHA256: `d5653a66cec9873bacd36757651d776e8cc23b73070fb7e176e5c92e04555473`

정적 기하 검사 후보 결과. FAIL도 설계 확정 판정이 아니며 Unity 물리·적 포고·공중 조작·벽 가림·능력 문 실제 동작은 미검증이다.
계산: WORLD_V5 3절 점프2.5u·일반 단차2u·포고2u, 기존 CombatTuning downAttackHeight=1.3u. 포고 상단=y+.5, 포고 뒤 상승 여유=2+타격높이. 등불·가시 둘 다 포함, 가로 간격3u. 일반 경로와 포고 경로를 별도로 계산한다.
모든 spawns를 각각 시작점으로 검사한다. fall은 x 아래 첫 고체/발판/온전한 봉인·덮개·부서지는 벽 상단을 착지점으로 삼는다. 벽 가림·충돌·적 이동·입력 타이밍을 풀지 않는 낙관적 경로 후보이며 실제 문 gate 차단은 T3에서 검사한다.
밀도 요소=아이템 marks(조각/사당)+기존 11종. 높이20 초과는 아래·위 절반에 아이템 각1개를 검사한다.

| 방 | 면적 u² | 요소 | /1000u² | 문 | FAIL | WARN | 판정 |
|---|---:|---:|---:|---:|---:|---:|---|
| A01 | 768 | 5 | 6.510 | 3 | 0 | 0 | PASS |
| A02 | 1008 | 8 | 7.937 | 5 | 9 | 3 | FAIL |
| A03 | 1000 | 9 | 9.000 | 5 | 0 | 0 | PASS |
| A04 | 600 | 10 | 16.667 | 2 | 1 | 0 | FAIL |
| A05 | 500 | 5 | 10.000 | 3 | 0 | 0 | PASS |
| A06 | 672 | 6 | 8.929 | 2 | 0 | 1 | WARN |

```text
MEASURE A01 arrival: start=A01-A02 fall=False seed=[1] ordinary=6 pogo=9
MEASURE A01 pogo-route: start=A01-A02 surface=5 x=25 top=11.6
MEASURE A01 arrival: start=A01-A06 fall=True seed=[5] ordinary=7 pogo=9
MEASURE A01 arrival: start=A01-Z01 fall=False seed=[1] ordinary=6 pogo=9
MEASURE A01 pogo-route: start=A01-Z01 surface=5 x=25 top=11.6
MEASURE A01 arrival: start=start fall=False seed=[1] ordinary=6 pogo=9
MEASURE A01 pogo-route: start=start surface=5 x=25 top=11.6
WARN A02 headroom: platform=0 clearance=1.5
WARN A02 headroom: platform=2 clearance=1
WARN A02 headroom: platform=3 clearance=1.4
MEASURE A02 arrival: start=A01-A02 fall=False seed=[0] ordinary=3 pogo=7
FAIL A02 reachability: start=A01-A02 surface=1 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A01-A02 surface=2 x=1 top=35 absent in pogo envelope
MEASURE A02 pogo-route: start=A01-A02 surface=5 x=13 top=9
MEASURE A02 pogo-route: start=A01-A02 surface=6 x=13.5 top=10.5
MEASURE A02 pogo-route: start=A01-A02 surface=7 x=11.5 top=12.4
MEASURE A02 arrival: start=A02-A03 fall=False seed=[0] ordinary=3 pogo=7
FAIL A02 reachability: start=A02-A03 surface=1 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-A03 surface=2 x=1 top=35 absent in pogo envelope
MEASURE A02 pogo-route: start=A02-A03 surface=5 x=13 top=9
MEASURE A02 pogo-route: start=A02-A03 surface=6 x=13.5 top=10.5
MEASURE A02 pogo-route: start=A02-A03 surface=7 x=11.5 top=12.4
MEASURE A02 arrival: start=A02-A03U fall=False seed=[5] ordinary=6 pogo=7
FAIL A02 reachability: start=A02-A03U surface=1 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-A03U surface=2 x=1 top=35 absent in pogo envelope
MEASURE A02 arrival: start=A02-B05 fall=True seed=[7] ordinary=6 pogo=7
FAIL A02 reachability: start=A02-B05 surface=1 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-B05 surface=2 x=1 top=35 absent in pogo envelope
MEASURE A02 arrival: start=A02-A06 fall=False seed=[1] ordinary=7 pogo=8
FAIL A02 reachability: start=A02-A06 surface=2 x=1 top=35 absent in pogo envelope
MEASURE A03 arrival: start=A02-A03 fall=False seed=[0] ordinary=6 pogo=14
MEASURE A03 pogo-route: start=A02-A03 surface=1 x=1 top=9
MEASURE A03 pogo-route: start=A02-A03 surface=4 x=1 top=15
MEASURE A03 pogo-route: start=A02-A03 surface=8 x=43 top=11
MEASURE A03 arrival: start=A02-A03U fall=False seed=[1] ordinary=7 pogo=14
MEASURE A03 pogo-route: start=A02-A03U surface=4 x=1 top=15
MEASURE A03 pogo-route: start=A02-A03U surface=8 x=43 top=11
MEASURE A03 arrival: start=A05-A03 fall=False seed=[0] ordinary=6 pogo=14
MEASURE A03 pogo-route: start=A05-A03 surface=1 x=1 top=9
MEASURE A03 pogo-route: start=A05-A03 surface=4 x=1 top=15
MEASURE A03 pogo-route: start=A05-A03 surface=8 x=43 top=11
MEASURE A03 arrival: start=A03-A04 fall=False seed=[8] ordinary=7 pogo=14
MEASURE A03 pogo-route: start=A03-A04 surface=1 x=1 top=9
MEASURE A03 pogo-route: start=A03-A04 surface=4 x=1 top=15
MEASURE A04 arrival: start=A03-A04 fall=False seed=[0] ordinary=6 pogo=11
FAIL A04 reachability: start=A03-A04 surface=1 x=48 top=1 absent in pogo envelope
MEASURE A05 arrival: start=A04-A05 fall=True seed=[1, 6] ordinary=8 pogo=8
MEASURE A05 arrival: start=A05-A03 fall=False seed=[0, 4] ordinary=8 pogo=8
WARN A06 headroom: platform=1 clearance=1.5
MEASURE A06 arrival: start=A01-A06 fall=False seed=[3] ordinary=10 pogo=13
```

PASS self-test synthetic door-safety / overlap / height detected; intact seal traversal retained

회귀 실행 도구 보정 1회: 제한된 셸로 시작한 `B16b-T4`는 Unity 라이선스 초기화에서 중단했다. 제품 테스트에 진입하지 못한 원문은 `_raw/regression/B16b-T4/S1-02.log`에 보존한다. 기존 정상 실행 환경에서 새 태그 `B16b-T4-r1`로 재실행한다.

```text
[Licensing::Module] Error: Timed-out after 60.01s, waiting for Licensing to initialize
Crash!!!
```
최종 Quiet: B16b-T4-r1 REGRESSION 13/13. 정적 후보를 기록한 T4는 DONE.
