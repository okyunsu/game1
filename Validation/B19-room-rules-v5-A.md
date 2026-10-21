# B19 T3 — 능력 조건을 구분한 정적 검사

상태 DONE. Python 컴파일·기존 위반 fixture4개 재검출. Scene·Design·PlayerTuning 변경0. 아래 도달성은 조건부 낙관적 기하 포락선이며 자연 플레이 가능성 증명이 아니다.

|방|이전 FAIL|능력 조건 적용 후 FAIL|사라진 FAIL|조건|
|---|---:|---:|---:|---|
|A01|0|0|0|없음|
|A02|9|0|9|WallJump|
|A03|0|0|0|없음|
|A04|0|0|0|없음|
|A05|0|0|0|없음|
|A06|13|0|13|DJ|
|합계|22|0|22|A02 벽 경로9 / A06 DJ 높이·도달성13|

잔여 FAIL: 0. 새 FAIL: 0. 무능력 계산을 삭제하거나 전부 능력 보유로 바꾸지 않았다. A02는 WallJump 가정 없이9개 후보(WARN)가 그대로 남는다. A06은 유일 입구 A01의 DJ gate와 실제 arrival ID를 확인한 경우에만 DJ를 적용하며 무능력 계산은13개 후보(WARN)가 그대로 남는다. A02 WallJump는 climb 면을 오르는 조건부 결과이며 A02 첫 방문에 소유한다고 주장하지 않는다.

DJ 높이: 기존 PlayerTuning jumpVelocity=12, doubleJumpVelocity=12, gravity=30에서 4.8u. 하드코딩0. 실제 고정 틱·점프 해제·벽 가림·가로3u 포락선의 한계는 남는다. 초기 발만으로 벽 접촉을 계산한 결과 A02 상단5개가 남았고, 기존 몸 높이1.6u를 포함한 벽 측면 접촉으로 보정했다. 중간 원문은 보존했다.

|기존 FAIL 원문|변경 원인 분류|
|---|---|
|`FAIL A02 reachability: start=A01-A02 surface=2 x=1 top=21 absent in pogo envelope`|검사기 한계: climb 면과 몸 높이를 포함한 벽 접촉 누락|
|`FAIL A02 reachability: start=A01-A02 surface=3 x=1 top=35 absent in pogo envelope`|검사기 한계: climb 면과 몸 높이를 포함한 벽 접촉 누락|
|`FAIL A02 reachability: start=A02-A03 surface=2 x=1 top=21 absent in pogo envelope`|검사기 한계: climb 면과 몸 높이를 포함한 벽 접촉 누락|
|`FAIL A02 reachability: start=A02-A03 surface=3 x=1 top=35 absent in pogo envelope`|검사기 한계: climb 면과 몸 높이를 포함한 벽 접촉 누락|
|`FAIL A02 reachability: start=A02-A03U surface=2 x=1 top=21 absent in pogo envelope`|검사기 한계: climb 면과 몸 높이를 포함한 벽 접촉 누락|
|`FAIL A02 reachability: start=A02-A03U surface=3 x=1 top=35 absent in pogo envelope`|검사기 한계: climb 면과 몸 높이를 포함한 벽 접촉 누락|
|`FAIL A02 reachability: start=A02-B05 surface=2 x=1 top=21 absent in pogo envelope`|검사기 한계: climb 면과 몸 높이를 포함한 벽 접촉 누락|
|`FAIL A02 reachability: start=A02-B05 surface=3 x=1 top=35 absent in pogo envelope`|검사기 한계: climb 면과 몸 높이를 포함한 벽 접촉 누락|
|`FAIL A02 reachability: start=A02-A06 surface=3 x=1 top=35 absent in pogo envelope`|검사기 한계: climb 면과 몸 높이를 포함한 벽 접촉 누락|
|`FAIL A06 height: platform=3 nearest lower rise=4 (static candidate)`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 height: platform=4 nearest lower rise=4 (static candidate)`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=0 x=9 top=0.4 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=1 x=3 top=3 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=2 x=24.5 top=3 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=3 x=0 top=1 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=4 x=22 top=1 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=6 x=1 top=7 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=7 x=6.5 top=5 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=8 x=28 top=3.9 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=9 x=40.9 top=5 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 surface=10 x=44.5 top=5 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|
|`FAIL A06 reachability: start=A01-A06 door=A02-A06 absent in pogo envelope`|검사기 한계: 유일 입구 DJ 보유 조건 및 4.8u 높이 누락|

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
MEASURE A03 arrival: start=A02-A03 fall=False seed=[0] noAbilityOrdinary=16 noAbilityPogo=18 capabilities=none ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A02-A03 surface=11 x=1 top=15
MEASURE A03 arrival: start=A02-A03U fall=False seed=[7] noAbilityOrdinary=16 noAbilityPogo=18 capabilities=none ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A02-A03U surface=11 x=1 top=15
MEASURE A03 arrival: start=A05-A03 fall=False seed=[2] noAbilityOrdinary=16 noAbilityPogo=18 capabilities=none ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A05-A03 surface=11 x=1 top=15
MEASURE A03 arrival: start=A03-A04 fall=False seed=[16] noAbilityOrdinary=16 noAbilityPogo=18 capabilities=none ordinary=16 pogo=18
MEASURE A03 pogo-route: start=A03-A04 surface=11 x=1 top=15
MEASURE A04 arrival: start=A03-A04 fall=False seed=[2] noAbilityOrdinary=10 noAbilityPogo=14 capabilities=none ordinary=10 pogo=14
MEASURE A05 arrival: start=A04-A05 fall=True seed=[5] noAbilityOrdinary=8 noAbilityPogo=8 capabilities=none ordinary=8 pogo=8
MEASURE A05 arrival: start=A05-A03 fall=False seed=[2] noAbilityOrdinary=8 noAbilityPogo=8 capabilities=none ordinary=8 pogo=8
MEASURE A06 arrival: start=A01-A06 fall=False seed=[5] noAbilityOrdinary=1 noAbilityPogo=1 capabilities=DJ ordinary=11 pogo=14
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
PASS draft2 five synthetic rules detected; draft3 four JSON fixtures detected; clean fixture PASS
```

[이전 원문](_raw/B19/T3-before.json), [초기 중간 결과](_raw/B19/T3-after-initial.md), [능력 계산 원문](_raw/B19/T3-after.md). Quiet 실제 REGRESSION 13/13: [원문](_raw/regression/B19-T3/result.txt). 근거 없는 기존 정적 규칙8종은 WARN으로 낮췄다(door-safety/overlap/ability-gate/height/arrival/reachability/pit-patrol/pit-width). 후보는 숨기지 않으며 실제 버그가 연결된 머리 여유·방 높이·도랑 덮임·적 배치 FAIL을 유지한다. [최종 정적·fixture 원문](_raw/B19/T3-final.md).
