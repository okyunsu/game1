# B10 T9 — MechanicsTest 연속 경로

상태: DONE. 실제 Release `B10-release-r15`, 60fps. Scene의 작성된 시작 지점부터 끝 x97까지 실제 입력으로 이동한 전체 경로를 저장하고, 파일을 읽어 2회 연속 재생 PASS. 중간 위치 배치 없음. 시험 Scene 자체가 제공하는 능력 사용, 에이전트가 능력을 추가 부여하지 않음.

원본: `_raw/B10/T9-course-r15.txt`, `.live.txt`, `.log`. 전체 입력: `_raw/B10/inputs/MechanicsTest_continuous-2316130616.json`.

```text
MEASURE continuous course segment=11 pos=(77.11, 2.81)
MEASURE continuous course segment=13 pos=(86.49, 15.82)
MEASURE continuous course segment=16 pos=(97.07, 1.81)
PASS continuous course saved replay repetition=1 input=Validation/_raw/B10/inputs\MechanicsTest_continuous-2316130616.json
PASS continuous course saved replay repetition=2 input=Validation/_raw/B10/inputs\MechanicsTest_continuous-2316130616.json
MEASURE B10 mode=course PASS=1 BLOCKED-GEOMETRY=0 budget-deferred=0
```

16개 구간: 바닥 이동, pad/seal 영역 통행, 포고/증기/승강기 영역 통행, 실제 무너짐 대기, 벽 점프와 DJ로 왼쪽 벽 정상 도달, 오른쪽 벽 정상, 끝 바닥. 장치별 공격·Slam·포고·승강기 왕복 작동은 T1에서 별도로 2회 재생 확인했으며, 이 연속 경로가 모든 장치의 작동을 하나씩 수행했다고 주장하지 않는다.

초기 실패 원문:

```text
FAIL continuous course segment=11 pos=(77.11, 3.21) peak=3.835
FAIL continuous course segment=14 pos=(85.64, 4.26) peak=15.67499
FAIL continuous course saved replay repetition=1
```

무너지는 발판은 고정 높이 접지 대신 실제 Falling 이벤트로 확인했고, 벽 정상은 실제 정상 x/높이와 접지로 연결했다. 증기는 전역 Time.time 주기이므로 초기 위치를 바꾸지 않고 시작점에서 새 On 주기를 기다린 뒤 전체 저장 입력을 재생했다. 제품 장치 수치와 Scene 변경0. 실패 원본/후보 입력 삭제0.

미실시: 이 연속 입력의 30/120fps 재생, 게임패드, 모든 장치 작동을 연결한 별도 연속 코스. T10 및 남은 승인 검증에서 이어간다.
