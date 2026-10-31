# D42 — rooms_v5_A draft4.2 정적 검사 (2026-10-31)

대상: `Design/v5/rooms_v5_A.json`(rooms-v5-A-draft4.2, SHA256 `ddd08c5b032e4e654051756b760ca20d203f6dbc96ff93d08a40da002e381188`) + `world_v5_A.json`(변경 없음). 실행: Claude(앱 세션), `Tools/Check-RoomRules.py`(e085ed9 사본). **Unity 미실행.**

| 항목 | draft4.1 (D41) | draft4.2 |
|---|---|---|
| FAIL | 0 | **0** |
| WARN | 16 (A03) | **30** (A03) |
| self-test | PASS | PASS |
| A01 / A02 도달(무능력) | 11 / 9 | 11 / 9 (같음) |
| 접근 머리 충돌(간격<1.8, 오름≥1.5, 떠 있는 발판) | A01~A05 20건 | **0건** |

A03 WARN 30의 내역: (1) 검사기가 레버 승강기를 모름 → 위층 발판·A04 문이 거리에서 도달 불가로 나옴. **B23에서 검사기에 승강기(lo↔hi) 연결 추가 후 재판정.** (2) 대시 연습 Pa·Pb·선반은 의도한 대시 게이트. (3) 위 마을(상면9)·A02-A03U 문은 거리에서 직접 못 감(의도, A02 고리로 감). (4) height·ascent는 승강기 구간.

중간에 한 번 FAIL이 있었다: A02 굴뚝 아래 조각 길 발판을 x11.6으로 옮기자 굴뚝 벽(x10~11, y11~)과 가로 0.6·머리 여유 0.5로 `edge-headroom` FAIL. x12~13.4(폭1.4)로 고쳐 해소.

접근 머리 충돌 검사: `Design/proposals-1031/approach_check.py`(Claude 스크립트). B23에서 검사기 규칙으로 옮긴다.

## 검사기 원문 출력
# V5 층 A 방 규칙 검사

원본: `Design/v5/rooms_v5_A.json` (읽기 전용)
SHA256: `ddd08c5b032e4e654051756b760ca20d203f6dbc96ff93d08a40da002e381188`

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
| A01 | 768 | 7 | 9.115 | 3 | 0 | 0 | PASS |
| A02 | 1008 | 8 | 7.937 | 5 | 0 | 0 | PASS |
| A03 | 1000 | 1 | 1.000 | 5 | 0 | 30 | WARN |
| A04 | 600 | 6 | 10.000 | 2 | 0 | 0 | PASS |
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
MEASURE A02 arrival: start=A02-B05 fall=True seed=[7] noAbilityOrdinary=9 noAbilityPogo=9 capabilities=WallJump ordinary=14 pogo=14
MEASURE A02 arrival: start=A02-A06 fall=False seed=[2] noAbilityOrdinary=10 noAbilityPogo=10 capabilities=WallJump ordinary=14 pogo=14
WARN A03 height: platform=0 nearest lower rise=8 (static candidate)
WARN A03 height: platform=1 nearest lower rise=6 (static candidate)
WARN A03 height: platform=2 nearest lower rise=12 (static candidate)
WARN A03 height: platform=3 nearest lower rise=12 (static candidate)
WARN A03 height: platform=4 nearest lower rise=12 (static candidate)
WARN A03 ascent: >=4u ascent with platforms only; branch requires human review
MEASURE A03 arrival: start=A02-A03 fall=False seed=[0] noAbilityOrdinary=1 noAbilityPogo=1 capabilities=none ordinary=1 pogo=1
WARN A03 reachability: start=A02-A03 surface=1 x=1 top=9 absent in pogo envelope
WARN A03 reachability: start=A02-A03 surface=2 x=1 top=15 absent in pogo envelope
WARN A03 reachability: start=A02-A03 surface=3 x=26.5 top=13 absent in pogo envelope
WARN A03 reachability: start=A02-A03 surface=4 x=35 top=13 absent in pogo envelope
WARN A03 reachability: start=A02-A03 surface=5 x=46.1 top=13 absent in pogo envelope
WARN A03 reachability: start=A02-A03 door=A02-A03U absent in pogo envelope
WARN A03 reachability: start=A02-A03 door=A03-A04 absent in pogo envelope
MEASURE A03 arrival: start=A02-A03U fall=False seed=[1] noAbilityOrdinary=2 noAbilityPogo=2 capabilities=none ordinary=2 pogo=2
WARN A03 reachability: start=A02-A03U surface=2 x=1 top=15 absent in pogo envelope
WARN A03 reachability: start=A02-A03U surface=3 x=26.5 top=13 absent in pogo envelope
WARN A03 reachability: start=A02-A03U surface=4 x=35 top=13 absent in pogo envelope
WARN A03 reachability: start=A02-A03U surface=5 x=46.1 top=13 absent in pogo envelope
WARN A03 reachability: start=A02-A03U door=A03-A04 absent in pogo envelope
MEASURE A03 arrival: start=A05-A03 fall=False seed=[0] noAbilityOrdinary=1 noAbilityPogo=1 capabilities=none ordinary=1 pogo=1
WARN A03 reachability: start=A05-A03 surface=1 x=1 top=9 absent in pogo envelope
WARN A03 reachability: start=A05-A03 surface=2 x=1 top=15 absent in pogo envelope
WARN A03 reachability: start=A05-A03 surface=3 x=26.5 top=13 absent in pogo envelope
WARN A03 reachability: start=A05-A03 surface=4 x=35 top=13 absent in pogo envelope
WARN A03 reachability: start=A05-A03 surface=5 x=46.1 top=13 absent in pogo envelope
WARN A03 reachability: start=A05-A03 door=A02-A03U absent in pogo envelope
WARN A03 reachability: start=A05-A03 door=A03-A04 absent in pogo envelope
MEASURE A03 arrival: start=A03-A04 fall=False seed=[5] noAbilityOrdinary=2 noAbilityPogo=2 capabilities=none ordinary=2 pogo=2
WARN A03 reachability: start=A03-A04 surface=1 x=1 top=9 absent in pogo envelope
WARN A03 reachability: start=A03-A04 surface=2 x=1 top=15 absent in pogo envelope
WARN A03 reachability: start=A03-A04 surface=3 x=26.5 top=13 absent in pogo envelope
WARN A03 reachability: start=A03-A04 surface=4 x=35 top=13 absent in pogo envelope
WARN A03 reachability: start=A03-A04 door=A02-A03U absent in pogo envelope
MEASURE A04 arrival: start=A03-A04 fall=False seed=[0] noAbilityOrdinary=8 noAbilityPogo=10 capabilities=none ordinary=8 pogo=10
MEASURE A05 arrival: start=A04-A05 fall=True seed=[4] noAbilityOrdinary=6 noAbilityPogo=6 capabilities=none ordinary=6 pogo=6
MEASURE A05 arrival: start=A05-A03 fall=False seed=[2] noAbilityOrdinary=6 noAbilityPogo=6 capabilities=none ordinary=6 pogo=6
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
