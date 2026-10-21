# B19 T4 — DJ 자연 경로 원인 측정

상태 DONE(측정). 원인 분류: **입력 타이밍**. 목표 좌표 낡음 아님. 전체 경로가 물리적으로 불가능하다는 근거 없음. 검증 입력의 목표·타이밍과 Scene·Design·게임 수치는 바꾸지 않았다.

기존 recipe 그대로 실제 Development exe에서 A01→A06만120초 측정했다. 렌더 프레임7200개마다 위치·실제 collider 발 높이·vx/vy·접지·DJ 잔여/횟수·입력·다음 목표·접촉 콜라이더를 남겼다. 위치 보정은 시작/별도 높이 fixture에서만 사용했고 자연 구간 안에는0이다. 이전 B18 끝 위치4.39와 이번5.62의 위상 차이는 있지만 같은 x24.947·step3 정지가 재현됐다.

|분류|측정/판단|
|---|---|
|자동 입력 목표가 낡음|아님. 모든 waypoint가 draft3 solid7/platform0~4 상단과 일치한다. 새 작은 발판 platform2=(23.8..25.8, top8.8), platform3=(27.2..29.2, top10.5)도 포함한다. draft2에서는 이 구간이 등불이었으나 현재 목표 위치는 새 발판에 맞는다.|
|입력 타이밍|주원인. 3.8334초, (15.863,9.331), vy7.4·공중에서 platform1 목표를 완료하고 바로 x24.8/top8.8을 향한다. 4.4166초 x19.3431/발8.123에서 DJ를 사용한 뒤 5.1167초 x23.435/발7.667에 platform2 옆면 접촉(vx0, vy-15). 상단8.8에 착지하지 못하고 5.5000초 바닥1.015로 낙하한다. 이후 목표 x 차이가0.2보다 작아 수평 입력이 없어지고, 같은 발판 아래에서 점프를 반복한다.|
|물리적으로 못 닿음|정지한 바닥1→platform2 top8.8은 상승7.8u로 단독 DJ4.884u보다 높다. 그러나 원래 앞 발판 top6.8→8.8은 오름2u/간격1.3u다. 실패 위치에서 수직 반복하는 입력이 경로를 복구하지 못한 것이며 지형 전체 불가능의 증명이 아니다.|
|마지막 발판→천장 문 y15|기존 PlayerTuning 12/12/중력30의 연속식4.8u, 실제 키 입력 별도 fixture의 DJ 상승4.8840u. 마지막 발판 발11.6에 더하면 정점16.4840. 문의 아래 경계15보다 높다. 이 높이 측정은 A01 자연 경로 완주나 실제 문 통과를 대신하지 않는다.|

자동 좌표가 낡지 않았으므로 좌표 갱신 재실행 조건은 해당하지 않는다. 타이밍/착지 확인/복구 입력 개선은 이번 범위에 없어 수행하지 않았다. 물리 불가능으로 판정하지 않아 Design 수정도 요청하지 않는다. 다음 배치에서 입력 recipe 개선을 승인하면 이어갈 수 있다.

실제 핵심 원문(필드 순서: room,frame,elapsed,step,x,bodyY,feet,vx,vy,grounded,djAvailable,djCount,input,targetX,targetY,contacts):

```text
MEASURE traceFrames=7200 duration=119.9880 targetStallX=24.9470 finalFeet=5.6230
MEASURE lateStep3 feetMin=1.0150 feetMax=6.6850 contacts={'none': 6267, 'solid1': 330, 'platform2': 3}
FRAME A01,241,3.8334,3,15.8630,9.3310,9.3310,6.0000,7.4000,False,True,1,RightArrow,24.8000,8.8000,
FRAME A01,276,4.4166,3,19.3431,8.1230,8.1230,6.0000,12.0000,False,False,2,RightArrow|Z,24.8000,8.8000,
FRAME A01,318,5.1167,3,23.4350,7.6670,7.6670,0.0000,-15.0000,False,False,2,RightArrow,24.8000,8.8000,platform2
FRAME A01,323,5.1999,3,23.4638,6.2990,6.2990,1.4400,-18.0000,False,False,2,RightArrow,24.8000,8.8000,
FRAME A01,341,5.5000,3,24.9470,1.0150,1.0150,0.6000,0.0000,False,False,2,,24.8000,8.8000,solid1
FRAME A01,342,5.5166,3,24.9470,1.0150,1.0150,0.0000,0.0000,True,True,2,Z,24.8000,8.8000,solid1
FRAME A01,383,6.2000,3,24.9470,2.6530,2.6530,0.0000,12.0000,False,False,3,Z,24.8000,8.8000,
FRAME A01,4283,71.2021,3,24.9470,6.6850,6.6850,0.0000,0.0000,False,False,47,Z,24.8000,8.8000,platform2
BLOCKED route A01 goal=A06 elapsed=120.05 position=(24.95, 5.62) step=3 maxX=24.95 maxY=10.32 hp=4 pogos=3 contacts= reason=120s input budget exhausted
MEASURE DJ-height actualRise=4.8840 analyticEnvelope=4.8000 jumpVelocity=12.0000 doubleJumpVelocity=12.0000 gravity=30.0000 finalPlatformFeet=11.6 apexFromFinalPlatform=16.4840 doorMinY=15
Error/Exception/Assert=0
```

후반 step3의 공중 접촉 없음6267프레임·바닥 접촉330·platform2 아래 접촉3, 발 높이 범위1.015..6.685u. 빈 contacts를 끼임 증거로 삼지 않았다. 프레임은 물리 고정 틱과 달라 같은 물리 상태가 여러 렌더 프레임에 기록될 수 있다.

검증 도구 첫 접근은 Editor에서 아래처럼 실패했다. 프로젝트 Build Profile은 수정하지 않았고, 실제 V5 Scene을 포함하는 임시 Development 빌드로 접근을 바꿨다. 빌드 Succeeded errors=0 warnings=0, 자연 경로 측정과 별도 높이 측정의 실제 exe exit0/Error0. 이 FAIL은 제품 경로 실패가 아니라 Editor 테스트 초기화 실패다.

```text
FAIL B16V5Verifier actual=Editor
FAIL System.Exception: Timeout room ready
Scene 'Assets/Scenes/V5/A01_V5.unity' couldn't be loaded because it has not been added to the active build profile or shared scene list or the AssetBundle has not been loaded.
Error/Exception/Assert=1
```

[전체 프레임](_raw/B19/T4-exe.txt), [핵심 사건](_raw/B19/T4-events.txt), [높이 원문](_raw/B19/T4-height.txt), [draft2/draft3 좌표 비교](_raw/B19/T4-geometry.txt), [Editor 실패](_raw/B19/T4-trace.txt), [빌드](_raw/B19/T4-build.txt). Quiet 실제 REGRESSION 13/13: [원문](_raw/regression/B19-T4/result.txt). 임시 빌드는 종료 시 정리하며 사용자 런처는 기존 B18 빌드를 유지한다.
