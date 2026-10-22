# B20 T2 — 도랑·일반 점프·대시 실측

상태 DONE(측정). 실제 Development exe, PlayerMotor·PlayerTuning·Scene 변경 없음. 대시·벽 점프·더블 점프를 끈 실제 키 입력을 사용했다. 도랑 각 시도 시작과 별도 평지 측정 arena 설정만 위치를 초기화했다. 사용자 저장은 사용하지 않았다.

```text
MEASURE A05-trench mode=walk-only crossed=False hp=4->0 minFeet=0.4150 position=(11.6350, 0.4150) died=True seconds=2.853
MEASURE A05-trench mode=ordinary-jumps crossed=True hp=4->3 minFeet=0.4150 position=(13.0896, 2.9230) died=False seconds=1.433
MEASURE hazard room=A05 hp=4->3 start=(9.4000, 0.8160) end=(9.9840, 0.4150) xRange=9.4000..9.9840 respawn=False input=none
MEASURE hazard room=A04 hp=4->3 start=(23.6000, 0.8160) end=(23.0160, 0.4150) xRange=23.0160..23.6000 respawn=False input=none
```

A05 x6..12는 **능력 없이 일반 점프를 반복하면 통과 가능**(피해1). 걷기만 한 이번 recipe는 오른쪽 0.6u 턱에서 막혀 반복 피해로 사망했다. 따라서 이번 결과는 “걷기만으로도 통과”를 입증하지 않는다. 두 방 모두 hazard에서 피해1과 넉백이 있고 **도랑 입구로 복귀하는 동작은 없음**. `Assets/Scripts/DamageBlock.cs` Apply는 PlayerHealth.TakeDamage만 부른다. `PlayerHealth.cs` TakeDamage는 일반 넉백·무적·HP0 사망만 처리한다. `RoomSession.cs` Respawn은 사망 시 체크포인트 복귀이며 hazard별 입구 복귀가 아니다. A04 노트의 가정과 구현이 다르다.

|측정(각3회)|1회|2회|3회|최소|최대|평균|
|---|---:|---:|---:|---:|---:|---:|
|일반 점프 최대 성공 턱 높이 u|2.51250|2.51250|2.51250|2.51250|2.51250|2.51250|
|그 다음 실패 턱 높이 u|2.52500|2.52500|2.52500|2.52500|2.52500|2.52500|
|지상 대시 활성 구간 거리 u|2.87988|2.87988|2.87988|2.87988|2.87988|2.87988|
|공중 대시 활성 구간 거리 u|2.87988|2.87988|2.87988|2.87988|2.87988|2.87988|
|지상 대시 활성 중 높이 변화 u|0|0|0|0|0|0|
|공중 대시 활성 중 높이 변화 u|0|0|0|0|0|0|
|풀 점프 발 상승 u|2.52002|2.52002|2.52002|2.52002|2.52002|2.52002|
|짧은 점프 발 상승 u|1.00800|1.00800|1.00800|1.00800|1.00800|1.00800|

턱은 평지에서 2u 앞에서 오른쪽+점프 유지, 높이 [2,2.8]를 이분 탐색했다. 분해능0.0125u, 이번 입력에서 최대 성공/최소 실패 경계이며 모든 인간 입력의 절대 한계가 아니다. 측정 arena의 큰 좌표에 따른 부동소수 오차가 있어 표시값을 과도하게 정밀한 설계 한계로 쓰지 않는다.

짧은 점프는 이륙을 관측한 첫 렌더 프레임에 키를 놓는 recipe(약0.0333초)다. 풀 점프는 착지까지 유지했다. 프리팹 몸 크기0.7×1.6, 기존 PlayerTuning의 jumpVelocity12/gravity30/cutMultiplier0.5를 읽는다. 연속식 풀 점프2.4u, 즉시 잘랐을 때0.6u. 실제 짧은 점프는 누른 시간과 이산 적분 때문에 즉시 잘랐을 때와 다르다. T3 판정에는 실제 최대값 풀2.52002/짧음1.00800을 쓴다.

최초 관측은 입력 요청부터 대시 종료 직후까지의 창이라 공중 높이 -0.01801..0.04199u가 포함됐다. 이를 대시 자체의 높이 변화로 오인하지 않도록 Motor 다음 FixedUpdate에서 실제 활성 시작/종료 좌표를 따로 관측했다. 활성8틱 동안 거리2.87988u/높이0이며, 요청 창 수치는 원문에 별도 필드로 남겼다. 계산 거리 dashSpeed18×duration0.16=2.88u와 일치한다.

실제 컴파일·빌드 `Succeeded errors=0 warnings=0`, exe `Error/Exception/Assert=0`. [최종 원문](_raw/B20/T2-fixedticks.txt), [첫 요청 창 측정](_raw/B20/T2-metrics.txt), [T3/T5용 실제 수치](_raw/B20/T2-metrics.json), [Quiet 최종](_raw/regression/B20-T2-final/result.txt). 도랑의 피해 없는 자연 완주나 사용자 체감 수락은 확인하지 않았다.
