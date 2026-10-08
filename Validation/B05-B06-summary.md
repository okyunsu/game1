# B05 B06 — REVIEW

60u 선택 Dash·포고. 기존 lethal PogoTarget x12/y2.5, x17/y3.5. 실제 포고 2회·Dash 입력을 거쳐 발견 탑으로 간다. Double Jump 불필요.

새 Scene: Assets/Scenes/V2/B06_V2.unity. 기존 방·원본 Prefab 수정0. 설계는 LEVEL_V2.md, 지정 입력은 Validation/Routes/V2/V2-B06-{main,optional}.json.

## 실제 검증

- 필수 입구→선택한 출구 가상 입력 기록 및 고정 재생 PASS: Validation/_raw/B05/B06-main-replay-delivery.txt
- 발견 접촉+발판 착지 고정 재생 PASS: Validation/_raw/B05/B06-optional-replay-delivery.txt
- 실제 KillZone 접촉 사망 / Health API 치명피격 / 체크포인트·HP 복귀 / 물리문 양방향 접촉 fixture PASS: Validation/_raw/B05/B06-life-a1.txt
- 적 방은 재입장 Live instance 교체, CP 방은 타이틀·Continue Scene 매핑도 실제 PASS. 처치·사망 fixture와 자연 플레이 검증을 구분한다.
- 각 최종 검사 Error/Exception/Assert=0.

핵심 측정 원문:
`

MEASURE optional B06 stage=0 pos=(31.18, 7.61) ticks=237 found=True room=B06 pogos=2
PASS B06 actual pogo chain; no Double Jump
Error/Exception/Assert=0 ```

## 실패·미실시

초기 높이 가설 2회 보정 후 원인을 재진단: lethal 포고 표적을 일반 점프로 지나가다 사망. 실제 Down+X·Dash 입력 접근으로 변경하여 PASS. FixedUpdate-only 관찰이 사망 중 TimeScale0를 놓친 문제는 프레임 관찰로 따로 보정했다.

```
B06-optional-a1:
FAIL B05Optional actual=exe
MEASURE optional B06 stage=1 pos=(28.61, 5.86) ticks=2217
FAIL System.Exception: B06 optional discovery contact + shelf landing recording
B06-optional-a2:
FAIL B05Optional actual=exe
MEASURE optional B06 stage=1 pos=(28.61, 7.08) ticks=2218
FAIL System.Exception: B06 optional discovery contact + shelf landing recording
B06-optional-a3:
FAIL B05Optional actual=exe
MEASURE optional B06 stage=53 pos=(31.39, 9.89) ticks=2217
FAIL System.Exception: B06 optional discovery contact + shelf landing recording
B06-optional-a4:
FAIL B05Optional actual=exe
MEASURE optional B06 stage=3 pos=(31.57, 7.61) ticks=2217 found=False room=A01
FAIL System.Exception: B06 optional discovery contact + shelf landing recording
```

사람 난이도·자연 이동 시간·CP→주요 도전 30~90초·지역 감각·비승 난이도는 미실측/사용자 REVIEW. 예상 시간은 계획치이며 자동 입력 시간을 사람 시간으로 사용하지 않았다. Sprint Gate·기본값 채택0.

