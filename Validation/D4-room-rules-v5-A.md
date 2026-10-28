# D4 — rooms_v5_A draft4 정적 검사 (2026-10-28)

대상: `Design/v5/rooms_v5_A.json`(rooms-v5-A-draft4) + `Design/v5/world_v5_A.json`. 실행: Claude Code(방 세션). **Unity 미실행**, 정적 기하 검사만.

```text
python Tools/Check-RoomRules.py Design/v5/rooms_v5_A.json --v5 --self-test --world Design/v5/world_v5_A.json --output <새 파일>   # exit 0
```

## 요약 (실제 수행·미실시 구분)
| 항목 | 결과 |
|---|---|
| 데이터 방 FAIL | **0** (A01~A06) |
| 데이터 방 WARN | **16** — 모두 A03: reachability 12(선반 x1·P1 x23.3·P2 x30.8 × 시작 4곳), height 3, ascent 1 |
| 기준(draft3.1, 같은 검사기) | FAIL 0 / WARN 0 (이번 실행 직전 같은 명령으로 측정) |
| self-test | PASS(원문 아래). 아래 `FAIL synthetic…`·`FAIL B21-fixture…` 줄은 검사기가 의도적으로 만든 불량 예제를 잡은 것이며 데이터 FAIL이 아님 |
| boss·hazard_return 필드 | 검사기가 읽지 않음(무시). 이 필드가 원인인 실패는 없음. 계약 값은 별도 스크립트로 직접 대조 |
| trench-escape MEASURE | `--max-step-evidence` 미지정 → **미실시** |

## 해석
- A03 WARN 16은 의도된 대시 게이트다. 대시 연습 선반·P1·P2는 일반 점프·포고 어느 쪽으로도 닿지 않게 만들었고(그래서 reachability 'absent in pogo envelope'), 검사기는 대시 게이트를 몰라 WARN으로 낸다. height 3건은 같은 세 발판의 오름(4~6u)이고, ascent 1건은 A03에서 적·등불이 없어진 결과다. 규칙 버전이 같지만 배치가 의도적으로 바뀌었으므로 개수 비교로 회귀 판정하지 않는다.
- 사양 05의 A03 3단계 "아래가 hazard"는 `covered-trench` FAIL(바닥 위에 뜬 hazard를 덮인 도랑으로 판정)이 나서 넣지 않았다(첫 실행에서 `FAIL A03 covered-trench: solid=1 x=23..36 zone=26.6..30.8` 확인 후 제거).
- 직접 대조(스크립트): 아레나 x24~49·y1~11 안에 발판·고체·hazard·봉인·적·등불 0, 잠금벽 자리 x23~24 비어 있음, 바닥 상면 y1이 x24~44 연속, `boss` 필드 값이 04 계약 C-6과 동일, A05 breakable 0.
- 미검증: 능력 없이 A03 서쪽 선반 도달 불가는 정적 포고 포락선 기준일 뿐 실제 입력 증명이 아니다. 대시로 5.5u/4.5u 간격이 실제로 건너지는지(대시 약 2.9u + 점프)도 Unity 측정 전이다. 덮개·잠금벽·hazard 복귀 동작은 생성기·런타임 미구현이라 검증하지 않았다.

## 검사기 원문 출력
# V5 층 A 방 규칙 검사

원본: `Design/v5/rooms_v5_A.json` (읽기 전용)
SHA256: `f1f5a5428fc5423fd72d9f0df18a43aef595e012ea443c0490120224a5ab57e5`

정적 기하 검사 후보 결과. FAIL도 설계 확정 판정이 아니며 Unity 물리·적 포고·공중 조작·벽 가림·능력 문 실제 동작은 미검증이다.
계산: WORLD_V5 3절 점프2.5u·일반 단차2u·포고2u, 기존 CombatTuning downAttackHeight=1.3u. 포고 상단=y+.5, 포고 뒤 상승 여유=2+타격높이. 등불·가시 둘 다 포함, 가로 간격3u. 일반 경로와 포고 경로를 별도로 계산한다.
모든 spawns를 각각 시작점으로 검사한다. fall은 x 아래 첫 고체/발판/온전한 봉인·덮개·부서지는 벽 상단을 착지점으로 삼는다. 벽 가림·충돌·적 이동·입력 타이밍을 풀지 않는 낙관적 경로 후보이며 실제 문 gate 차단은 T3에서 검사한다.
B19: noAbilityOrdinary/noAbilityPogo는 능력 없는 계산이며 capabilities 결과와 구분한다. climb 방은 WallJump 보유를 가정한 벽 전체 길이 경로다(첫 도착 소유 주장 아님). 유효 입구가 하나이고 gate가 있으면 그 능력을 요구한다. DJ 높이는 PlayerTuning의 (jumpVelocity²+doubleJumpVelocity²)/(2×gravity)=4.8u를 읽기 전용으로 계산한다. 연속 벽 접촉에는 프리팹 몸 높이를 포함한다. 가로3u·충돌·입력 제한은 여전히 검사기 한계다.
밀도 요소=아이템 marks(조각/사당)+기존 11종. 높이20 초과는 아래·위 절반에 아이템 각1개를 검사한다.

draft2 추가 규칙: 몸체 (0.7, 1.6)u를 프리팹에서 읽음. 머리 여유=키+.6u, 방 높이≥12, 도랑 위 바닥 금지는 관찰 버그 근거 FAIL. 구덩이 순찰 교차·폭>3은 WARN. 맞닿은 고체의 내부 면은 서 있는 면에서 제외한다.

B19 근거 없는 정적 후보는 WARN: door-safety, overlap, ability-gate, height, arrival, reachability, pit-patrol, pit-width. 후보 출력은 유지하며 실제 플레이 불가능으로 확정하지 않는다.

draft3 추가 규칙: 가장자리 가로0.9u 이내 구조물 머리 여유2.2u, 적 솔리드 침투·서 있는 면·순찰 전구간 지지 FAIL. 오름4u 이상에서 작은 발판 없는 포고 구간·등불3연속은 WARN(기하 후보).

B21: shortcut 반대편 진입의 closed_until_wall_broken 저장 키가 실제 우회된 breakable과 일치하면 spawn-bypass는 MEASURE(잠금으로 해소). 누락·잘못된 키·봉인 우회는 FAIL을 유지한다. 표시는 검사 계약이며 런타임 잠금 구현을 보장하지 않는다. 도랑·구덩이 탈출은 --max-step-evidence의 실측치와 비교한 MEASURE(피해·머리 충돌 제외). 실측 JSON 미지정 시 trench-escape MEASURE는 미실시.

| 방 | 면적 u² | 요소 | /1000u² | 문 | FAIL | WARN | 판정 |
|---|---:|---:|---:|---:|---:|---:|---|
| A01 | 768 | 3 | 3.906 | 3 | 0 | 0 | PASS |
| A02 | 1008 | 7 | 6.944 | 5 | 0 | 0 | PASS |
| A03 | 1000 | 0 | 0.000 | 5 | 0 | 16 | WARN |
| A04 | 600 | 5 | 8.333 | 2 | 0 | 0 | PASS |
| A05 | 600 | 4 | 6.667 | 3 | 0 | 0 | PASS |
| A06 | 672 | 8 | 11.905 | 2 | 0 | 0 | PASS |

```text
MEASURE A01 arrival: start=A01-A02 fall=False seed=[2] noAbilityOrdinary=11 noAbilityPogo=11 capabilities=none ordinary=11 pogo=11
MEASURE A01 arrival: start=A01-A06 fall=True seed=[9] noAbilityOrdinary=11 noAbilityPogo=11 capabilities=none ordinary=11 pogo=11
MEASURE A01 arrival: start=A01-Z01 fall=False seed=[1] noAbilityOrdinary=11 noAbilityPogo=11 capabilities=none ordinary=11 pogo=11
MEASURE A01 arrival: start=start fall=False seed=[1] noAbilityOrdinary=11 noAbilityPogo=11 capabilities=none ordinary=11 pogo=11
MEASURE A02 arrival: start=A01-A02 fall=False seed=[0] noAbilityOrdinary=9 noAbilityPogo=9 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 arrival: start=A02-A03 fall=False seed=[1] noAbilityOrdinary=9 noAbilityPogo=9 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 arrival: start=A02-A03U fall=False seed=[8] noAbilityOrdinary=9 noAbilityPogo=9 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 arrival: start=A02-B05 fall=True seed=[9] noAbilityOrdinary=9 noAbilityPogo=9 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 arrival: start=A02-A06 fall=False seed=[2] noAbilityOrdinary=10 noAbilityPogo=10 capabilities=WallJump ordinary=14 pogo=14
WARN A03 height: platform=9 nearest lower rise=4 (static candidate)
WARN A03 height: platform=10 nearest lower rise=6 (static candidate)
WARN A03 height: platform=11 nearest lower rise=6 (static candidate)
WARN A03 ascent: >=4u ascent with platforms only; branch requires human review
MEASURE A03 arrival: start=A02-A03 fall=False seed=[0] noAbilityOrdinary=16 noAbilityPogo=16 capabilities=none ordinary=16 pogo=16
WARN A03 reachability: start=A02-A03 surface=16 x=1 top=15 absent in pogo envelope
WARN A03 reachability: start=A02-A03 surface=17 x=23.3 top=11 absent in pogo envelope
WARN A03 reachability: start=A02-A03 surface=18 x=30.8 top=13 absent in pogo envelope
MEASURE A03 arrival: start=A02-A03U fall=False seed=[7] noAbilityOrdinary=16 noAbilityPogo=16 capabilities=none ordinary=16 pogo=16
WARN A03 reachability: start=A02-A03U surface=16 x=1 top=15 absent in pogo envelope
WARN A03 reachability: start=A02-A03U surface=17 x=23.3 top=11 absent in pogo envelope
WARN A03 reachability: start=A02-A03U surface=18 x=30.8 top=13 absent in pogo envelope
MEASURE A03 arrival: start=A05-A03 fall=False seed=[2] noAbilityOrdinary=16 noAbilityPogo=16 capabilities=none ordinary=16 pogo=16
WARN A03 reachability: start=A05-A03 surface=16 x=1 top=15 absent in pogo envelope
WARN A03 reachability: start=A05-A03 surface=17 x=23.3 top=11 absent in pogo envelope
WARN A03 reachability: start=A05-A03 surface=18 x=30.8 top=13 absent in pogo envelope
MEASURE A03 arrival: start=A03-A04 fall=False seed=[15] noAbilityOrdinary=16 noAbilityPogo=16 capabilities=none ordinary=16 pogo=16
WARN A03 reachability: start=A03-A04 surface=16 x=1 top=15 absent in pogo envelope
WARN A03 reachability: start=A03-A04 surface=17 x=23.3 top=11 absent in pogo envelope
WARN A03 reachability: start=A03-A04 surface=18 x=30.8 top=13 absent in pogo envelope
MEASURE A04 arrival: start=A03-A04 fall=False seed=[0] noAbilityOrdinary=8 noAbilityPogo=10 capabilities=none ordinary=8 pogo=10
MEASURE A05 arrival: start=A04-A05 fall=True seed=[5] noAbilityOrdinary=8 noAbilityPogo=8 capabilities=none ordinary=8 pogo=8
MEASURE A05 arrival: start=A05-A03 fall=False seed=[2] noAbilityOrdinary=8 noAbilityPogo=8 capabilities=none ordinary=8 pogo=8
MEASURE A06 arrival: start=A01-A06 fall=False seed=[5] noAbilityOrdinary=1 noAbilityPogo=1 capabilities=DJ ordinary=11 pogo=14
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
PASS B20 fixed shortcut fixture FAIL=0; missing lock/bad key/seal detected; valid locked bypass is MEASURE; a/b order independent
FAIL B21-fixture-unlocked shortcut-closed: door=A05-A03 from=A03 to=A05 missing valid closed_until_wall_broken key
FAIL B21-fixture-unlocked shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0
FAIL B21-fixture-bad-key shortcut-closed: door=A05-A03 from=A03 to=A05 missing valid closed_until_wall_broken key
FAIL B21-fixture-bad-key shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0
FAIL B21-fixture-unrelated-key shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0
MEASURE B21-fixture-locked shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0; 잠금으로 해소 key=V5-A05-breakable0; runtime lock NOT VERIFIED
FAIL B21-fixture-seal shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past seal0
PASS B21 locked spawn bypass MEASURE; unlocked/bad-key/unrelated-key/seal bypass FAIL; a/b order independent
PASS draft2 five synthetic rules detected; draft3 four JSON fixtures detected; clean fixture PASS
