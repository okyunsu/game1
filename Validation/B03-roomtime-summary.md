# B03-ROOMTIME — DONE (측정 산출물)

A06/C05 Scene 및 기존 입구 배치 변경0. 실제 exe, 첫2회 점프 없이 오른쪽으로 이동해 의도적으로 낙사→실제 CP 복귀, 3번째 지정 점프/포고/Dash(소유 시)/DJ(소유 시)로 발견→원래 방 복귀. fixture 전환은 각 시도 시작 전 부모 방의 기존 FromLeft만 사용, 방 내부·입구·귀환에 위치 쓰기0. A06 Dash/DJ false, C05 둘 다 true. 새 게임 규칙/수치 변경0.

| 방 | 첫 실패1/2(s) | 성공 왕복(s) | 두 실패+진입+성공 전체(s) | 사람 목표60~180s와 차이 |
|---|---|---|---|---|
| A06 | 1.733 / 1.733 | 10.634 | 16.401 | 최소보다43.599s, 최대보다163.599s 짧음 |
| C05 | 2.167 / 2.133 | 9.750 | 16.351 | 최소보다43.649s, 최대보다163.649s 짧음 |

전체 시간은 Continue 이후 부모 방 fixture 전환·입구 입력·CP 복귀를 포함, 게임 프로세스 기동·사람 탐색/학습 시간은 제외. 의도적 낙사는 방 초반에서 빠르게 발생했으므로 실제 어려운 구간에서 실패하는 사람의 재시도 시간과 다를 수 있다. 이 측정만으로 사람1~3분을 PASS 또는 FAIL로 판정하지 않는다. 기존 A06/C05는 시간·체감 PARTIAL 유지, 플레이 후 유지/의미 있는 도전 추가 여부 결정.

원문 (`_raw/B03/A06-time.txt`, `C05-time.txt`), 양쪽 actual exe PASS/Error0:
```
MEASURE A06 intentionalFail=1 seconds=1.733
MEASURE A06 intentionalFail=2 seconds=1.733
MEASURE A06 entrance-discovery-return seconds=10.634
MEASURE A06 two-failures-plus-roundtrip totalSeconds=16.401
MEASURE C05 intentionalFail=1 seconds=2.167
MEASURE C05 intentionalFail=2 seconds=2.133
MEASURE C05 entrance-discovery-return seconds=9.750
MEASURE C05 two-failures-plus-roundtrip totalSeconds=16.351
```
미실시: 사람 자연 탐색/도전 수락. Play 배치 수정은 이번 Task에서 하지 않음; 조정할 때는 각 Scene의 Edit 모드 원본을 사용하고 Play에서 바꾼 값은 Stop 후 반영·저장한다.
