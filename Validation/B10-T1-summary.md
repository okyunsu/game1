# B10 T1 — 실제 입력 탐색 봇

상태: DONE. B10-release-r11, 실제 exe, 60fps. 기본/V2/V3 Scene·PlayerTuning·제품 저장 경로 변경 없음. 검증 코드와 실행 도구는 명시적 `-verify-b10-search` 옵션에서만 활성화된다.

## 실제 합격

- MechanicsTest 9개: pad, seal, lantern, spike, enemy, steam, lift, crumble, wall. 각 성공 입력을 JSON으로 저장한 뒤 읽어서 2회 연속 재생 PASS.
- 이전 B09 PASS 중 10개: A01/main, A02/floor, A03/street, A06/floor, B01/street, B01/back, C04/cross, C04/back, C03/floor, C03/barracks. 각 구간과 전체 경로를 각각 2회 연속 재생 PASS.
- `Validation/_raw/B10/T1-mechanics-r11.txt`, `T1-r11-<방>-<경로>.txt`, 입력은 `inputs/`. 원본 보존, 커밋 제외.

원문 핵심:

```text
MEASURE B10 mode=mechanics PASS=9 BLOCKED-GEOMETRY=0 budget-deferred=0
PASS mechanics device lift replay=2 input=Validation/_raw/B10/inputs\MechanicsTest_lift_1.json-2305094409.json
PASS route A01/main replay=2 segments=1
PASS route B01/back replay=2 segments=4
PASS route C03/barracks replay=2 segments=2
```

## 구현과 범위

도약 위치 0.25u, 달리기 0/1/2u, 유지 .10~.40초(.05 간격), 공중 지속/해제/역방향, 태그별 DJ·dash·wall·pogo·slam·steam·lift 축. 확장 모드는 위치 .1u/유지 .02초. 초기 위치 준비 후 모든 중간 구간은 실제 키 입력으로만 연결한다. 최종 목표는 지정 발판의 실제 접지, 포고 이벤트, 장치 상태 또는 정확한 목적 방이다. 후보 전체를 소진한 경우에만 BLOCKED-GEOMETRY를 기록한다. 40분 제한으로 중단한 항목은 미판정 상태로 남겨 재방문한다.

Release 렌더 입력 재생의 OS 스케줄 편차를 줄이기 위해 opt-in 도구만 `Time.captureDeltaTime=1/fps`를 쓴다. Input System은 Dynamic Update 그대로다. 장치 해제는 레버 공격, 하강 탑승, 하부 레버 공격, 상승까지 실제 입력으로 확인했다.

초기 실패 원문(보존):

```text
MEASURE mechanics lift attempts=1 success=False pos=(61.43, 1.82) peak=8.620002 unlocked=False lowVisited=False
```

공중 초기 준비 뒤 대기하는 동안 레버 높이를 지나쳤다. lift 준비 대기 시간을 줄인 검증 경로에서 실제 해제와 왕복 탑승을 재현했다. 제품 장치 값 변경 없음.

## 미실시·후속

T2 전체 70경로, T4 왕복, T5 고립, T6 자연 완주, T7 보스 연결, T9 연속 코스, T10 FPS 재생은 별도 Task다. 이 T1 합격은 그 Task들의 합격을 의미하지 않는다. 게임패드 입력은 미실시.
