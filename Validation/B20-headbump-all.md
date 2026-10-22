# B20 Backlog 1 — 머리 후보 전체 실제 접촉

상태 DONE(측정). h1.6의 b/c 후보36면 모두 실제 Development exe에서 풀 점프했다. T3에서 남았던25면과, 측정 격리 조건을 통일하기 위한 기존11면 재측정을 포함한다. **일치33/36=91.67%, 불일치3면.** 정적 면 목록·크기별 분류는 [T3](B20-headbump.md)에 보존했다. 게임 배치·수치·규칙 변경 없음.

최초11표본의 불일치2(A01 solid2, A02 solid0)는 원본 자동 플레이 CSV에서 측정 중 문 전환이 있었음을 확인했다. 따라서 그 두 결과를 단순 모서리 불일치로 해석한 초기 가설은 철회한다. 검증 실행에서만 RoomExit를 비활성화하고 실제 측정 방 ID를 기록했으며, 피해 넉백과 적도 분리했다. 솔리드/발판/닫힌 문 콜라이더, PlayerMotor·튜닝·카메라는 그대로다. 재측정에서는 그 두 면 모두 같은 방에서 정상 머리 접촉했다.

최초 프로토콜의 전환 원문([자동 CSV](_raw/playtest/20261023-001944.csv)):

```text
"2026-10-22T15:19:45.8668573Z","room_exit","A01",46.654999,1.015000,"",1.038379,0.000000,"exit=A01-A02;to=A02;room_active_seconds=0.990208"
"2026-10-22T15:19:47.0659700Z","room_exit","A02",-0.345000,1.015000,"",2.237122,0.000000,"exit=A01-A02;to=A01;room_active_seconds=1.193793"
```

|방|후보 면|일치|불일치|일치율|
|---|---:|---:|---:|---:|
|A01|3|3|0|100.00%|
|A02|10|8|2|80.00%|
|A03|12|11|1|91.67%|
|A04|3|3|0|100.00%|
|A05|1|1|0|100.00%|
|A06|7|7|0|100.00%|
|합계|36|33|3|91.67%|

## 불일치 목록

세 곳 모두 실제 방 ID 일치·접지·이륙을 확인했으나 머리 접촉 없이 약2.52u 상승했다. 원인은 확정하지 않았다. 정적 투영은 아주 작은 가장자리 겹침도 후보로 세며 실제 접촉/모서리 분리와 다를 수 있으나, 이 가설을 검증한 것은 아니다. 새 설계 FAIL이나 진행 불가로 판정하지 않는다.

```text
MEASURE head-contact room=A02 actualRoom=A02 face=solid4 x=-0.345 top=21.000 clearance=3.000 category=c setupGrounded=True launched=True contact=False beforeFrame=-1 headFrame=-1 vyBefore=0.0000 vyAfter=0.0000 maxFeet=23.5350 predictionMatch=False
MEASURE head-contact room=A02 actualRoom=A02 face=solid11 x=1.000 top=21.000 clearance=3.000 category=c setupGrounded=True launched=True contact=False beforeFrame=-1 headFrame=-1 vyBefore=0.0000 vyAfter=0.0000 maxFeet=23.5350 predictionMatch=False
MEASURE head-contact room=A03 actualRoom=A03 face=solid5 x=-0.345 top=15.000 clearance=3.000 category=c setupGrounded=True launched=True contact=False beforeFrame=-1 headFrame=-1 vyBefore=0.0000 vyAfter=0.0000 maxFeet=17.5350 predictionMatch=False
```

## 실제 접촉 원문

```text
MEASURE head-contact room=A01 actualRoom=A01 face=solid1 x=9.655 top=1.000 clearance=3.300 category=c setupGrounded=True launched=True contact=True beforeFrame=35 headFrame=36 vyBefore=7.8000 vyAfter=0.0000 maxFeet=2.6850 predictionMatch=True
MEASURE head-contact room=A01 actualRoom=A01 face=solid2 x=46.655 top=1.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=84 headFrame=85 vyBefore=9.0000 vyAfter=0.0000 maxFeet=2.3850 predictionMatch=True
MEASURE head-contact room=A01 actualRoom=A01 face=platform4 x=30.155 top=11.600 clearance=3.400 category=c setupGrounded=True launched=True contact=True beforeFrame=133 headFrame=134 vyBefore=7.2000 vyAfter=0.0000 maxFeet=13.3850 predictionMatch=True
MEASURE head-contact room=A02 actualRoom=A02 face=solid0 x=-0.345 top=1.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=186 headFrame=187 vyBefore=9.0000 vyAfter=0.0000 maxFeet=2.3850 predictionMatch=True
MEASURE head-contact room=A02 actualRoom=A02 face=solid1 x=26.655 top=1.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=232 headFrame=233 vyBefore=9.0000 vyAfter=0.0000 maxFeet=2.3850 predictionMatch=True
MEASURE head-contact room=A02 actualRoom=A02 face=solid6 x=26.655 top=9.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=389 headFrame=390 vyBefore=9.0000 vyAfter=0.0000 maxFeet=10.3850 predictionMatch=True
MEASURE head-contact room=A02 actualRoom=A02 face=solid8 x=9.655 top=22.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=435 headFrame=436 vyBefore=9.0000 vyAfter=0.0000 maxFeet=23.3850 predictionMatch=True
MEASURE head-contact room=A02 actualRoom=A02 face=solid14 x=11.000 top=0.400 clearance=4.100 category=c setupGrounded=True launched=True contact=True beforeFrame=562 headFrame=563 vyBefore=2.4000 vyAfter=0.0000 maxFeet=2.8850 predictionMatch=True
MEASURE head-contact room=A02 actualRoom=A02 face=platform1 x=12.755 top=7.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=615 headFrame=616 vyBefore=9.0000 vyAfter=0.0000 maxFeet=8.3850 predictionMatch=True
MEASURE head-contact room=A02 actualRoom=A02 face=platform2 x=15.905 top=9.000 clearance=2.900 category=c setupGrounded=True launched=True contact=True beforeFrame=660 headFrame=661 vyBefore=9.0000 vyAfter=0.0000 maxFeet=10.2850 predictionMatch=True
MEASURE head-contact room=A02 actualRoom=A02 face=platform4 x=15.855 top=12.400 clearance=2.600 category=b setupGrounded=True launched=True contact=True beforeFrame=704 headFrame=705 vyBefore=10.2000 vyAfter=0.0000 maxFeet=13.3850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=solid0 x=-0.345 top=1.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=752 headFrame=753 vyBefore=9.0000 vyAfter=0.0000 maxFeet=2.3850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=solid1 x=27.655 top=1.000 clearance=3.500 category=c setupGrounded=True launched=True contact=True beforeFrame=803 headFrame=804 vyBefore=6.6000 vyAfter=0.0000 maxFeet=2.8850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=solid2 x=48.655 top=1.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=852 headFrame=853 vyBefore=9.0000 vyAfter=0.0000 maxFeet=2.3850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=solid4 x=-0.345 top=9.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=897 headFrame=898 vyBefore=9.0000 vyAfter=0.0000 maxFeet=10.3850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=solid7 x=48.655 top=13.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=1055 headFrame=1056 vyBefore=9.0000 vyAfter=0.0000 maxFeet=14.3850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=solid9 x=16.655 top=3.000 clearance=3.500 category=c setupGrounded=True launched=True contact=True beforeFrame=1105 headFrame=1106 vyBefore=6.6000 vyAfter=0.0000 maxFeet=4.8850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=platform0 x=0.655 top=9.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=1153 headFrame=1154 vyBefore=9.0000 vyAfter=0.0000 maxFeet=10.3850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=platform1 x=10.655 top=5.000 clearance=3.500 category=c setupGrounded=True launched=True contact=True beforeFrame=1205 headFrame=1206 vyBefore=6.6000 vyAfter=0.0000 maxFeet=6.8850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=platform2 x=16.655 top=7.000 clearance=3.500 category=c setupGrounded=True launched=True contact=True beforeFrame=1259 headFrame=1260 vyBefore=6.6000 vyAfter=0.0000 maxFeet=8.8850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=platform4 x=1.000 top=15.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=1319 headFrame=1320 vyBefore=3.6000 vyAfter=0.0000 maxFeet=17.3850 predictionMatch=True
MEASURE head-contact room=A03 actualRoom=A03 face=platform9 x=48.855 top=13.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=1369 headFrame=1370 vyBefore=9.0000 vyAfter=0.0000 maxFeet=14.3850 predictionMatch=True
MEASURE head-contact room=A04 actualRoom=A04 face=solid5 x=-0.345 top=1.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=1420 headFrame=1421 vyBefore=9.0000 vyAfter=0.0000 maxFeet=2.3850 predictionMatch=True
MEASURE head-contact room=A04 actualRoom=A04 face=platform1 x=17.855 top=6.900 clearance=4.100 category=c setupGrounded=True launched=True contact=True beforeFrame=1480 headFrame=1481 vyBefore=2.4000 vyAfter=0.0000 maxFeet=9.3850 predictionMatch=True
MEASURE head-contact room=A04 actualRoom=A04 face=platform2 x=20.655 top=6.900 clearance=4.100 category=c setupGrounded=True launched=True contact=True beforeFrame=1546 headFrame=1547 vyBefore=2.4000 vyAfter=0.0000 maxFeet=9.3850 predictionMatch=True
MEASURE head-contact room=A05 actualRoom=A05 face=solid6 x=-0.345 top=1.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=1603 headFrame=1604 vyBefore=9.0000 vyAfter=0.0000 maxFeet=2.3850 predictionMatch=True
MEASURE head-contact room=A06 actualRoom=A06 face=solid2 x=46.655 top=5.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=1654 headFrame=1655 vyBefore=9.0000 vyAfter=0.0000 maxFeet=6.3850 predictionMatch=True
MEASURE head-contact room=A06 actualRoom=A06 face=solid4 x=9.000 top=0.400 clearance=4.100 category=c setupGrounded=True launched=True contact=True beforeFrame=1714 headFrame=1715 vyBefore=2.4000 vyAfter=0.0000 maxFeet=2.8850 predictionMatch=True
MEASURE head-contact room=A06 actualRoom=A06 face=solid5 x=2.655 top=3.000 clearance=3.500 category=c setupGrounded=True launched=True contact=True beforeFrame=1772 headFrame=1773 vyBefore=6.6000 vyAfter=0.0000 maxFeet=4.8850 predictionMatch=True
MEASURE head-contact room=A06 actualRoom=A06 face=solid8 x=6.155 top=1.000 clearance=3.500 category=c setupGrounded=True launched=True contact=True beforeFrame=1826 headFrame=1827 vyBefore=6.6000 vyAfter=0.0000 maxFeet=2.8850 predictionMatch=True
MEASURE head-contact room=A06 actualRoom=A06 face=solid9 x=27.655 top=1.000 clearance=2.400 category=b setupGrounded=True launched=True contact=True beforeFrame=1871 headFrame=1872 vyBefore=10.8000 vyAfter=0.0000 maxFeet=1.7850 predictionMatch=True
MEASURE head-contact room=A06 actualRoom=A06 face=solid10 x=33.655 top=1.000 clearance=2.400 category=b setupGrounded=True launched=True contact=True beforeFrame=1910 headFrame=1911 vyBefore=10.8000 vyAfter=0.0000 maxFeet=1.7850 predictionMatch=True
MEASURE head-contact room=A06 actualRoom=A06 face=platform4 x=46.655 top=5.000 clearance=3.000 category=c setupGrounded=True launched=True contact=True beforeFrame=1953 headFrame=1954 vyBefore=9.0000 vyAfter=0.0000 maxFeet=6.3850 predictionMatch=True
Error/Exception/Assert=0
```

모든36면의 전체 프레임과 속도 변화는 [원문](_raw/B20/BL1-head-all.txt)에 있다. actualRoom은36/36 일치, setupGrounded/launched도36/36이다. 컴파일·빌드 errors=0 warnings=0. [빌드](_raw/B20/BL1-build.txt), [Quiet](_raw/regression/B20-BL1/result.txt). Release 제외 조건 안의 검증 코드만 바뀌었고 제품 크기 비교 코드는 T4 그대로다.

이 항목으로 실행 Task는7+Backlog1=8개다. Backlog2(작은 발판 h1.3/1.1 추가 표)는 8Task 상한 때문에 시작하지 않았다. 남은 시간을 이유로 승인된 개수 상한을 넘기지 않는다.
