# V5 층 A 방 규칙 검사

원본: `Design/v5/rooms_v5_A.json` (읽기 전용)
SHA256: `88a9613af81c195abbff981c6c55faa398ea7651e239da4da473ec7aa3d11c00`

정적 기하 검사 후보 결과. FAIL도 설계 확정 판정이 아니며 Unity 물리·적 포고·공중 조작·벽 가림·능력 문 실제 동작은 미검증이다.
계산: WORLD_V5 3절 점프2.5u·일반 단차2u·포고2u, 기존 CombatTuning downAttackHeight=1.3u. 포고 상단=y+.5, 포고 뒤 상승 여유=2+타격높이. 등불·가시 둘 다 포함, 가로 간격3u. 일반 경로와 포고 경로를 별도로 계산한다.
모든 spawns를 각각 시작점으로 검사한다. fall은 x 아래 첫 고체/발판/온전한 봉인·덮개·부서지는 벽 상단을 착지점으로 삼는다. 벽 가림·충돌·적 이동·입력 타이밍을 풀지 않는 낙관적 경로 후보이며 실제 문 gate 차단은 T3에서 검사한다.
B19: noAbilityOrdinary/noAbilityPogo는 능력 없는 계산이며 capabilities 결과와 구분한다. climb 방은 WallJump 보유를 가정한 벽 전체 길이 경로다(첫 도착 소유 주장 아님). 유효 입구가 하나이고 gate가 있으면 그 능력을 요구한다. DJ 높이는 PlayerTuning의 (jumpVelocity²+doubleJumpVelocity²)/(2×gravity)=4.8u를 읽기 전용으로 계산한다. 연속 벽 접촉에는 프리팹 몸 높이를 포함한다. 가로3u·충돌·입력 제한은 여전히 검사기 한계다.
밀도 요소=아이템 marks(조각/사당)+기존 11종. 높이20 초과는 아래·위 절반에 아이템 각1개를 검사한다.

draft2 추가 규칙: 몸체 (0.7, 1.6)u를 프리팹에서 읽음. 머리 여유=키+.6u, 방 높이≥12, 도랑 위 바닥 금지는 관찰 버그 근거 FAIL. 구덩이 순찰 교차·폭>3은 WARN. 맞닿은 고체의 내부 면은 서 있는 면에서 제외한다.

B19 근거 없는 정적 후보는 WARN: door-safety, overlap, ability-gate, height, arrival, reachability, pit-patrol, pit-width. 후보 출력은 유지하며 실제 플레이 불가능으로 확정하지 않는다.

draft3 추가 규칙: 가장자리 가로0.9u 이내 구조물 머리 여유2.2u, 적 솔리드 침투·서 있는 면·순찰 전구간 지지 FAIL. 오름4u 이상에서 작은 발판 없는 포고 구간·등불3연속은 WARN(기하 후보).

B20: shortcut 반대편 진입의 closed_until_wall_broken 저장 키 표시와 도착 spawn의 장벽 우회는 실제 A03→A05 통과 근거 FAIL. 표시는 검사 계약이며 런타임 잠금 구현을 보장하지 않는다. 도랑·구덩이 탈출은 --max-step-evidence의 실측치와 비교한 MEASURE(피해·머리 충돌 제외).

| 방 | 면적 u² | 요소 | /1000u² | 문 | FAIL | WARN | 판정 |
|---|---:|---:|---:|---:|---:|---:|---|
| A01 | 768 | 3 | 3.906 | 3 | 0 | 0 | PASS |
| A02 | 1008 | 7 | 6.944 | 5 | 0 | 0 | PASS |
| A03 | 1000 | 5 | 5.000 | 5 | 1 | 0 | FAIL |
| A04 | 600 | 9 | 15.000 | 2 | 0 | 0 | PASS |
| A05 | 600 | 5 | 8.333 | 3 | 1 | 0 | FAIL |
| A06 | 672 | 8 | 11.905 | 2 | 0 | 0 | PASS |

```text
MEASURE A01 arrival: start=A01-A02 fall=False seed=[2] noAbilityOrdinary=11 noAbilityPogo=11 capabilities=none ordinary=11 pogo=11
MEASURE A01 arrival: start=A01-A06 fall=True seed=[9] noAbilityOrdinary=11 noAbilityPogo=11 capabilities=none ordinary=11 pogo=11
MEASURE A01 arrival: start=A01-Z01 fall=False seed=[1] noAbilityOrdinary=11 noAbilityPogo=11 capabilities=none ordinary=11 pogo=11
MEASURE A01 arrival: start=start fall=False seed=[1] noAbilityOrdinary=11 noAbilityPogo=11 capabilities=none ordinary=11 pogo=11
MEASURE A01 trench-escape: pit=8 x=38..40.5 depth=0.6 measuredMaxStep=2.5125 noAbilityJumpEscape=possible; damage/headroom not solved
MEASURE A02 arrival: start=A01-A02 fall=False seed=[0] noAbilityOrdinary=9 noAbilityPogo=9 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 arrival: start=A02-A03 fall=False seed=[1] noAbilityOrdinary=9 noAbilityPogo=9 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 arrival: start=A02-A03U fall=False seed=[8] noAbilityOrdinary=9 noAbilityPogo=9 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 arrival: start=A02-B05 fall=True seed=[9] noAbilityOrdinary=9 noAbilityPogo=9 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 arrival: start=A02-A06 fall=False seed=[2] noAbilityOrdinary=10 noAbilityPogo=10 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 trench-escape: pit=14 x=11..13.5 depth=0.6 measuredMaxStep=2.5125 noAbilityJumpEscape=possible; damage/headroom not solved
MEASURE A03 arrival: start=A02-A03 fall=False seed=[0] noAbilityOrdinary=16 noAbilityPogo=18 capabilities=none ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A02-A03 surface=11 x=1 top=15
MEASURE A03 arrival: start=A02-A03U fall=False seed=[7] noAbilityOrdinary=16 noAbilityPogo=18 capabilities=none ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A02-A03U surface=11 x=1 top=15
MEASURE A03 arrival: start=A05-A03 fall=False seed=[2] noAbilityOrdinary=16 noAbilityPogo=18 capabilities=none ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A05-A03 surface=11 x=1 top=15
MEASURE A03 arrival: start=A03-A04 fall=False seed=[16] noAbilityOrdinary=16 noAbilityPogo=18 capabilities=none ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A03-A04 surface=11 x=1 top=15
FAIL A03 shortcut-closed: door=A05-A03 from=A03 to=A05 missing valid closed_until_wall_broken key
MEASURE A03 trench-escape: pit=11 x=20.5..23 depth=0.6 measuredMaxStep=2.5125 noAbilityJumpEscape=possible; damage/headroom not solved
MEASURE A03 trench-escape: pit=12 x=36..38.5 depth=0.6 measuredMaxStep=2.5125 noAbilityJumpEscape=possible; damage/headroom not solved
MEASURE A04 arrival: start=A03-A04 fall=False seed=[2] noAbilityOrdinary=10 noAbilityPogo=14 capabilities=none ordinary=10 pogo=14
MEASURE A04 trench-escape: trench=0 x=22..40 depth=0.6 measuredMaxStep=2.5125 noAbilityJumpEscape=possible; damage/headroom not solved
MEASURE A04 trench-escape: pit=10 x=17.5..20 depth=0.6 measuredMaxStep=2.5125 noAbilityJumpEscape=possible; damage/headroom not solved
MEASURE A05 arrival: start=A04-A05 fall=True seed=[5] noAbilityOrdinary=8 noAbilityPogo=8 capabilities=none ordinary=8 pogo=8
MEASURE A05 arrival: start=A05-A03 fall=False seed=[2] noAbilityOrdinary=8 noAbilityPogo=8 capabilities=none ordinary=8 pogo=8
FAIL A05 shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0
MEASURE A05 trench-escape: trench=0 x=6..12 depth=0.6 measuredMaxStep=2.5125 noAbilityJumpEscape=possible; damage/headroom not solved
MEASURE A05 trench-escape: pit=10 x=32.5..35 depth=0.6 measuredMaxStep=2.5125 noAbilityJumpEscape=possible; damage/headroom not solved
MEASURE A06 arrival: start=A01-A06 fall=False seed=[5] noAbilityOrdinary=1 noAbilityPogo=1 capabilities=DJ ordinary=11 pogo=14
MEASURE A06 trench-escape: trench=0 x=9..22 depth=0.6 measuredMaxStep=2.5125 noAbilityJumpEscape=possible; damage/headroom not solved
```

PASS self-test synthetic door-safety / overlap / height WARN detected; intact seal traversal retained
FAIL synthetic headroom: surface=0 overhead=3 clearance=1.4 required=2.2 playerHeight=1.6
FAIL synthetic room-height: height=10 minimum=12 cameraHeight=11
FAIL synthetic covered-trench: solid=2 x=9.5..20 zone=12..16
WARN synthetic pit-patrol: enemy=0 patrol=[4, 12] pit=7..9.5 feet=1
FAIL synthetic covered-trench: solid=2 x=9.5..20 zone=7..10.5
WARN synthetic pit-width: x=7 width=3.5 maximum=3
FAIL synthetic edge-headroom: surface=1 overhead=2 gap=0.5 clearance=1.5 required=2.2
FAIL synthetic enemy-inside-solid: enemy=0 x=6 feet=1 solid=[1]
FAIL synthetic enemy-support: enemy=0 x=8 feet=1 no standing surface
FAIL synthetic patrol-support: enemy=0 patrol=[3, 8] feet=1 leaves standing surface
WARN synthetic pogo-chain: consecutive lanterns=0,1,2
WARN synthetic pogo-only-climb: fromTop=1 toTop=7.5 rise=6.5 no small platform in local climb
PASS spike-only rise>=4 climb without small platform detected; lantern chain remains lantern-only
PASS B20 fixed shortcut fixture FAIL=0; missing lock/bad key/bypass/seal each detected; a/b order independent
PASS draft2 five synthetic rules detected; draft3 four JSON fixtures detected; clean fixture PASS
