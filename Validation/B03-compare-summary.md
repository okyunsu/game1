# B03-COMPARE — PARTIAL (각3회 처치 측정 완료 / 돌진 회피율 미관찰)

60fps Unity Editor, CP-C04 Continue→실제 이동·점프·공격/포고 입력→처치/Ending/타이틀. 전투 중 HP/보스 상태/위치 쓰기0. 기본은 기존 좌측 회복 공격, B2는 머리 위 추적 포고 입력으로 **전략이 다르므로 인간 난이도의 직접 비교가 아님**. 처치 시간은 전투 시작부터 보스HP0, hitstop 포함 게임시계와 별도 실시간을 기록.

| 보스 | 3회 게임시간(s) | 3회 실시간(s) | 평균 게임/실시간(s) | 피격3회/평균 | 근접 회피 | 도약 회피 | 충격파 회피 | 돌진 회피 |
|---|---|---|---|---|---|---|---|---|
| 기본 | 22.000 / 21.983 / 21.000 | 23.803 / 23.787 / 22.805 | 21.661 / 23.465 | 0/0/0, 평균0 | 23/23 (100%) | 해당 없음 | 21/21 (100%) | 해당 없음 |
| B2 | 20.483 / 20.350 / 20.417 | 22.287 / 22.154 / 22.220 | 20.417 / 22.220 | 0/0/0, 평균0 | 3/3 (100%) | 3/3 (100%) | 3/3 (100%) | N/A — 3회 모두 예고 중 처치 |

패턴 회피율은 Tell 시작~Recovery 종료까지의 피격0 비율이다. 실제 attack resolution(Recovery) 없이 처치된 예고는 분모에서 제외. 모든 지정 dodge 수단/30·120fps를 검증하는 값이 아니며 걷기 접촉도 별도이다. 돌진 실행 회피율이 없어 PARTIAL로 유지, B03-A 중단 경로의 대체 검사로 해석하지 않는다.

원문 (raw `compare-basic-1/2/3.txt`, `compare-B2-1-a2/2/3.txt`):
```
MEASURE candidate=basic killGameSeconds=22.000 killRealSeconds=23.803 hits=0
MEASURE candidate=basic killGameSeconds=21.983 killRealSeconds=23.787 hits=0
MEASURE candidate=basic killGameSeconds=21.000 killRealSeconds=22.805 hits=0
MEASURE candidate=B2 killGameSeconds=20.483 killRealSeconds=22.287 hits=0
MEASURE candidate=B2 killGameSeconds=20.350 killRealSeconds=22.154 hits=0
MEASURE candidate=B2 killGameSeconds=20.417 killRealSeconds=22.220 hits=0
```
최초 B2 pilot(`compare-B2-1.txt/.log`) 실패:
```
FAIL System.Exception: input fight death HP=26 hits=3
```
실제 피해2회 후 MarkDead의 Hit 이벤트까지 카운트3; 유효 처치 표본의 평균에서 제외하되 숨기지 않음. 재측정 `1-a2`는 이전 입력으로 PASS. 줄바꿈 때문에 문자열 보정이 적용되지 않은 사실을 코드 확인 후 수정했고, 표본2/3에서는 첫 점프를 거리6u 안에서 시작하고 더블 점프를 .23s 뒤 입력하는 보정이 적용됐다. 비교용 verifier만 변경, 게임 코드 변경0. Error0 전 표본. 사람 체감 수락 대기.
