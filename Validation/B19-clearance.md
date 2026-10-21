# B19 T2 — Scene 검사 판정 정리

상태 DONE. Unity 컴파일 exit0, draft3 Scene6개 검사 exit0, draft2 임시 생성 Scene 및0.5u 틈 fixture 검사 exit0, Quiet 회귀 실제13/13.

머리 위 minY>발 높이+접촉 오차0.003인 비일방 콜라이더만 clearance FAIL로 유지했다. closed-door / side / pit-edge는 MEASURE이며 방별 요약에 개수를 표시한다. 양옆 틈은 몸 폭0.7+검사 여유0.2보다 좁고 세로로 몸 높이 이상 겹치는 경우 WARN이다. 기존 적 박힘·지지면·순찰 끝점 FAIL은 유지했다. 게임 값이 아닌 검사 분류이며 Scene·Design·수치 에셋 수정0.

|검사|FAIL|WARN|MEASURE 기존 clearance 후보|결과|
|---|---:|---:|---:|---|
|draft3 Scene6|0|0|69(문6/측면43/구덩이20)|PASS|
|draft2 임시 Scene6|5(머리 위4/적 박힘1)|0|72(문6/측면46/구덩이20)|기존 버그 검출 PASS|
|폭0.5 fixture|0|1|측면4|WARN 분류 PASS|

일시 Scene은 기존 생성기를 Assets/Editor/B19TempScenes에만 실행해 만들고 검증 후 삭제했다. 기존 Scene 파일을 덮어쓰지 않았다. 기존 B18 runtime 진입 플래그와 Editor 진입점은 동일한 Scan 함수를 사용한다. 새 helper V5Batch19.ClearanceFixtures는 명시 opt-in이다.

실제 실패·핵심 수치 원문:

```text
MEASURE A01 summary samples=320 clearance=0 enemy=0 patrol=0 overhead=0 closedDoor=0 side=5 pitEdge=2 sideGap=0
MEASURE A02 summary samples=304 clearance=0 enemy=0 patrol=0 overhead=0 closedDoor=3 side=5 pitEdge=2 sideGap=0
MEASURE A03 summary samples=509 clearance=0 enemy=0 patrol=0 overhead=0 closedDoor=3 side=6 pitEdge=4 sideGap=0
MEASURE A04 summary samples=339 clearance=0 enemy=0 patrol=0 overhead=0 closedDoor=0 side=6 pitEdge=6 sideGap=0
MEASURE A05 summary samples=209 clearance=0 enemy=0 patrol=0 overhead=0 closedDoor=0 side=8 pitEdge=4 sideGap=0
MEASURE A06 summary samples=330 clearance=0 enemy=0 patrol=0 overhead=0 closedDoor=0 side=13 pitEdge=2 sideGap=0
FAIL A01 clearance surface=solid7 x=7.345 feet=3.000 blocker=platform0 oneWay=False minY=4.300 category=overhead
FAIL A02 clearance surface=solid13 x=7.845 feet=3.000 blocker=platform0 oneWay=False minY=4.500 category=overhead
FAIL A02 enemy-spawn prefab=E1 x=19.000 feet=9.010 blockers=solid14 support=True
FAIL A06 clearance surface=solid6 x=27.845 feet=3.000 blocker=platform2 oneWay=False minY=3.400 category=overhead
FAIL A06 clearance surface=solid7 x=39.845 feet=3.000 blocker=platform3 oneWay=False minY=4.500 category=overhead
PASS draft2 overhead=4 expectedPairs=4 sideGap=0 enemyInsideSolid=1
WARN fixture side-gap left=solid1 right=solid2 width=0.500 required=0.900
PASS horizontal-gap fixture width=0.500 WARN=1 FAIL=0
MEASURE temporaryScenesDeleted=True
```

[Scene 원문](_raw/B19/T2-draft3.txt), [fixture 원문](_raw/B19/T2-fixtures.txt), [회귀](_raw/regression/B19-T2/result.txt). 미실시: 새 배포 빌드·사람 감각 수락(이번 검사 코드 변경의 합격 조건이 아님).
