# B20 T1 — A05 지름길 실측

상태 DONE(측정). 실제 Development exe, 기본 draft3 Scene·수치·Player 프리팹 변경 없음. 사용자 로그 [CSV](../Playtest/20261022-233114.csv) 9~10행의 A03→A05 통과를 재현했다. 저장은 전용 `Validation/_raw/B20/T1-retry-save.json`만 사용했다.

```text
MEASURE door arrival room=A05 feet=(3.5000, 1.0089)
MEASURE shortcut first-entry x=3.5000 wallMin=1.0000 wallMax=2.0000 wallPresent=True collider=True broken=False side=inside breakableFrom=Any
PASS reproduced A03 to A05 inside intact wall
PASS actual inward-side attack broke wall
MEASURE shortcut reentry broken=True active=False saveKey=V5-A05-breakable0
PASS wall state survives room roundtrip
PASS Continue restores persistent wall key
MEASURE shortcut after-continue broken=True active=False x=3.5000
PASS wall remains open after save Continue and reentry
Error/Exception/Assert=0
```

처음 도착할 때 벽은 x1..2에 온전히 존재하지만 플레이어는 x3.5, 이미 벽 안쪽이다. A05 안쪽 x2.8에서 실제 왼쪽 공격으로 벽을 부순 뒤 A03 왕복, 타이틀→이어하기→A03→A05에서 벽이 계속 비활성임을 확인했다. 첫 문 접근 fixture는 진입 후 0.4초 억제 시간을 기다리지 않아 타임아웃이었다. 측정 대기를 0.5초로 수정한 한 번의 재시도에서 통과했다. 제품 문 로직은 수정하지 않았다.

|파일·함수·필드|현재 처리|
|---|---|
|`Design/v5/world_v5_A.json`, doors[A05-A03].type|`shortcut`은 지도 데이터 표기. 런타임 잠금 키가 아님|
|`Assets/Editor/B09Authoring.cs`, Generate 문 생성|rooms JSON의 gate·stub·oneway·down_only·도착 spawn 유무를 읽음. world의 문 type은 잠금으로 읽지 않음|
|같은 Generate, breakable 생성|wallId=`V5-A05-breakable0`, hitsToBreak=1. breakableFrom 지정이 없어 `Side.Any`. 안쪽 전용 의도가 현재 Scene으로 전달되지 않음|
|`Assets/Scripts/RoomExit.cs`, OnTriggerEnter2D|V4ExitRule.Allows 후 RequestTransition. 부서지는 벽 상태 직접 확인 없음|
|`Assets/Scripts/V4ExitRule.cs`, Allows, gate/elevatorSeal|능력 gate(DJ/WallJump/Dash/Slam/Boss), SealFloor.Broken만 지원. BreakableWall.wallId 연결 필드 없음|
|`Assets/Scripts/BreakableWall.cs`, ReceiveAttack/Start, wallId/breakableFrom|공격 방향과 영구 벽 키 지원. 문과 연동하지 않음|
|`Assets/Scripts/ProgressSave.cs`, MarkWallBroken/IsWallBroken, ProgressData.brokenWalls|파괴 저장·복원 수단 존재. 문별 닫힘 플래그는 없음|
|`Assets/Scripts/SealFloor.cs`, Broken/Break|봉인 바닥 전용. 기존 문 elevatorSeal은 이것만 참조|

결론: **새 코드 필요 — 문을 BreakableWall.wallId의 영구 파괴 상태에 연결하는 조건과 생성기 데이터 매핑이 필요하다.** 저장 스키마의 brokenWalls는 재사용할 수 있다. 문을 단순 영구 일방 통행으로 바꾸는 것은 파괴 후 양방향 지름길 요구를 만족하지 않는다. spawn x3.5와 breakableFrom=Any도 Claude draft4 및 다음 Scene 반영 때 함께 검토해야 한다. 이번 배치는 수정하지 않는다.

컴파일·빌드 `Succeeded errors=0 warnings=0`. Task Quiet 회귀 결과는 [원문](_raw/regression/B20-T1/result.txt). 실제 측정 [원문](_raw/B20/T1-shortcut-retry.txt), 첫 fixture 실패 [원문](_raw/B20/T1-shortcut.txt). 전체 자연 경로 완주나 사용자 체감 수락은 주장하지 않는다.
