# B18 T4 — draft3 실제 Scene 검사

Player.prefab BoxCollider2D 0.7×1.6을 직접 읽었다. 지지면 양끝 ±몸 반폭을 포함한 0.25u 샘플과 확장 높이2.2u를 검사한다. 내부에 덮인 바닥은 제외하며, 각 후보는 양의 실제 콜라이더 겹침이다. 한 방향 통과 발판은 별도 MEASURE이며 현재 Scene에는 없다. 접촉 경계0.003u와 양의 침투를 구분한다. 적·순찰 끝점은 프리팹 몸 크기와 Scene Transform을 사용한다.

|방|서 있는 자리 샘플|끼임 후보|적 박힘/지지 이상|순찰 끝 이상|
|---|---:|---:|---:|---:|
|A01|320|7|0|0|
|A02|304|10|0|0|
|A03|509|13|0|0|
|A04|339|12|0|0|
|A05|209|12|0|0|
|A06|330|15|0|0|

남는 후보는 아래 좌표를 Claude 검토에 돌린다. 데이터·규칙을 바꾸거나 FAIL을 숨기지 않았다. 후보는 좁은 머리 여유뿐 아니라 방 외벽·계단 측면·닫힌 문·구덩이 가장자리와 몸을 겹친 자리도 포함한다. 실제 플레이 진행 불가와 동일하지 않다.

실제 원문: [T4-after.txt](_raw/B18/T4-after.txt).

```text
MEASURE player prefab size=(0.70, 1.60) step=0.25 extraHeight=0.6 edgeExtension=0.35
FAIL A01 clearance surface=solid0 x=-0.095 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A01 clearance surface=solid0 x=1.155 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A01 clearance surface=solid0 x=3.905 feet=1.000 blocker=solid7 oneWay=False minY=1.000
FAIL A01 clearance surface=solid5 x=-0.095 feet=15.000 blocker=solid3 oneWay=False minY=15.000
FAIL A01 clearance surface=solid6 x=48.155 feet=15.000 blocker=solid4 oneWay=False minY=15.000
FAIL A01 clearance surface=solid8 x=38.155 feet=0.400 blocker=solid1 oneWay=False minY=0.000
FAIL A01 clearance surface=solid8 x=40.405 feet=0.400 blocker=solid2 oneWay=False minY=0.000
MEASURE A01 enemy-spawn x=18.000 feet=6.810 blockers=none support=True
MEASURE A01 patrol-end x=15.500 feet=6.810 blockers=none support=True
MEASURE A01 patrol-end x=21.500 feet=6.810 blockers=none support=True
MEASURE A01 enemy-spawn x=31.000 feet=1.010 blockers=none support=True
MEASURE A01 patrol-end x=26.000 feet=1.010 blockers=none support=True
MEASURE A01 patrol-end x=36.000 feet=1.010 blockers=none support=True
MEASURE A01 summary samples=320 clearance=7 enemy=0 patrol=0
FAIL A02 clearance surface=solid0 x=4.905 feet=1.000 blocker=solid13 oneWay=False minY=1.000
FAIL A02 clearance surface=solid0 x=7.655 feet=1.000 blocker=solid13 oneWay=False minY=1.000
FAIL A02 clearance surface=solid4 x=-0.095 feet=21.000 blocker=closed:A02-A06 oneWay=False minY=21.000
FAIL A02 clearance surface=solid4 x=1.155 feet=21.000 blocker=closed:A02-A06 oneWay=False minY=21.000
FAIL A02 clearance surface=solid5 x=-0.095 feet=35.000 blocker=solid2 oneWay=False minY=35.000
FAIL A02 clearance surface=solid7 x=28.155 feet=35.000 blocker=solid3 oneWay=False minY=35.000
FAIL A02 clearance surface=solid11 x=1.155 feet=21.000 blocker=closed:A02-A06 oneWay=False minY=21.000
FAIL A02 clearance surface=solid11 x=9.905 feet=21.000 blocker=solid8 oneWay=False minY=11.000
FAIL A02 clearance surface=solid14 x=11.155 feet=0.400 blocker=solid0 oneWay=False minY=0.000
FAIL A02 clearance surface=solid14 x=13.405 feet=0.400 blocker=solid1 oneWay=False minY=0.000
MEASURE A02 enemy-spawn x=21.000 feet=9.010 blockers=none support=True
MEASURE A02 patrol-end x=17.500 feet=9.010 blockers=none support=True
MEASURE A02 patrol-end x=24.500 feet=9.010 blockers=none support=True
MEASURE A02 enemy-spawn x=20.000 feet=1.010 blockers=none support=True
MEASURE A02 patrol-end x=15.500 feet=1.010 blockers=none support=True
MEASURE A02 patrol-end x=24.500 feet=1.010 blockers=none support=True
MEASURE A02 summary samples=304 clearance=10 enemy=0 patrol=0
FAIL A03 clearance surface=solid0 x=16.905 feet=1.000 blocker=solid9 oneWay=False minY=1.000
FAIL A03 clearance surface=solid0 x=19.655 feet=1.000 blocker=solid9 oneWay=False minY=1.000
FAIL A03 clearance surface=solid1 x=23.905 feet=1.000 blocker=solid10 oneWay=False minY=1.000
FAIL A03 clearance surface=solid1 x=26.655 feet=1.000 blocker=solid10 oneWay=False minY=1.000
FAIL A03 clearance surface=solid5 x=-0.095 feet=15.000 blocker=closed:A03-B01 oneWay=False minY=15.000
FAIL A03 clearance surface=solid5 x=1.155 feet=15.000 blocker=closed:A03-B01 oneWay=False minY=15.000
FAIL A03 clearance surface=solid6 x=-0.095 feet=19.000 blocker=solid3 oneWay=False minY=19.000
FAIL A03 clearance surface=solid8 x=50.155 feet=19.000 blocker=solid3 oneWay=False minY=19.000
FAIL A03 clearance surface=solid11 x=20.655 feet=0.400 blocker=solid0 oneWay=False minY=0.000
FAIL A03 clearance surface=solid11 x=22.905 feet=0.400 blocker=solid1 oneWay=False minY=0.000
FAIL A03 clearance surface=solid12 x=36.155 feet=0.400 blocker=solid1 oneWay=False minY=0.000
FAIL A03 clearance surface=solid12 x=38.405 feet=0.400 blocker=solid2 oneWay=False minY=0.000
FAIL A03 clearance surface=platform4 x=1.155 feet=15.000 blocker=closed:A03-B01 oneWay=False minY=15.000
MEASURE A03 enemy-spawn x=7.000 feet=9.010 blockers=none support=True
MEASURE A03 patrol-end x=4.000 feet=9.010 blockers=none support=True
MEASURE A03 patrol-end x=11.000 feet=9.010 blockers=none support=True
MEASURE A03 enemy-spawn x=13.500 feet=5.010 blockers=none support=True
MEASURE A03 patrol-end x=12.000 feet=5.010 blockers=none support=True
MEASURE A03 patrol-end x=15.500 feet=5.010 blockers=none support=True
MEASURE A03 enemy-spawn x=32.000 feet=1.010 blockers=none support=True
MEASURE A03 patrol-end x=28.500 feet=1.010 blockers=none support=True
MEASURE A03 patrol-end x=35.000 feet=1.010 blockers=none support=True
MEASURE A03 enemy-spawn x=30.500 feet=5.010 blockers=none support=True
MEASURE A03 patrol-end x=28.800 feet=5.010 blockers=none support=True
MEASURE A03 patrol-end x=32.200 feet=5.010 blockers=none support=True
MEASURE A03 summary samples=509 clearance=13 enemy=0 patrol=0
FAIL A04 clearance surface=solid1 x=-0.095 feet=11.000 blocker=solid0 oneWay=False minY=11.000
FAIL A04 clearance surface=solid2 x=50.155 feet=11.000 blocker=solid0 oneWay=False minY=11.000
FAIL A04 clearance surface=solid5 x=7.905 feet=1.000 blocker=solid3 oneWay=False minY=1.000
FAIL A04 clearance surface=solid5 x=10.655 feet=1.000 blocker=solid3 oneWay=False minY=1.000
FAIL A04 clearance surface=solid7 x=22.155 feet=0.400 blocker=solid6 oneWay=False minY=0.000
FAIL A04 clearance surface=solid7 x=29.405 feet=0.400 blocker=solid4 oneWay=False minY=0.400
FAIL A04 clearance surface=solid7 x=32.655 feet=0.400 blocker=solid4 oneWay=False minY=0.400
FAIL A04 clearance surface=solid7 x=39.905 feet=0.400 blocker=solid8 oneWay=False minY=0.000
FAIL A04 clearance surface=solid9 x=48.905 feet=1.000 blocker=solid2 oneWay=False minY=1.000
FAIL A04 clearance surface=solid9 x=50.155 feet=1.000 blocker=solid2 oneWay=False minY=1.000
FAIL A04 clearance surface=solid10 x=17.655 feet=0.400 blocker=solid5 oneWay=False minY=0.000
FAIL A04 clearance surface=solid10 x=19.905 feet=0.400 blocker=solid6 oneWay=False minY=0.000
MEASURE A04 enemy-spawn x=15.000 feet=5.010 blockers=none support=True
MEASURE A04 patrol-end x=13.800 feet=5.010 blockers=none support=True
MEASURE A04 patrol-end x=16.200 feet=5.010 blockers=none support=True
MEASURE A04 enemy-spawn x=14.000 feet=1.010 blockers=none support=True
MEASURE A04 patrol-end x=11.500 feet=1.010 blockers=none support=True
MEASURE A04 patrol-end x=16.500 feet=1.010 blockers=none support=True
MEASURE A04 enemy-spawn x=31.000 feet=6.910 blockers=none support=True
MEASURE A04 patrol-end x=25.000 feet=6.910 blockers=none support=True
MEASURE A04 patrol-end x=40.000 feet=6.910 blockers=none support=True
MEASURE A04 summary samples=339 clearance=12 enemy=0 patrol=0
FAIL A05 clearance surface=solid2 x=-0.095 feet=11.000 blocker=solid0 oneWay=False minY=11.000
FAIL A05 clearance surface=solid3 x=50.155 feet=11.000 blocker=solid1 oneWay=False minY=11.000
FAIL A05 clearance surface=solid4 x=6.155 feet=0.400 blocker=solid6 oneWay=False minY=0.000
FAIL A05 clearance surface=solid4 x=11.905 feet=0.400 blocker=solid7 oneWay=False minY=0.000
FAIL A05 clearance surface=solid6 x=0.905 feet=1.000 blocker=breakable0 oneWay=False minY=1.000
FAIL A05 clearance surface=solid6 x=2.155 feet=1.000 blocker=breakable0 oneWay=False minY=1.000
FAIL A05 clearance surface=solid9 x=35.905 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A05 clearance surface=solid9 x=39.155 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A05 clearance surface=solid9 x=48.905 feet=1.000 blocker=solid3 oneWay=False minY=1.000
FAIL A05 clearance surface=solid9 x=50.155 feet=1.000 blocker=solid3 oneWay=False minY=1.000
FAIL A05 clearance surface=solid10 x=32.655 feet=0.400 blocker=solid8 oneWay=False minY=0.000
FAIL A05 clearance surface=solid10 x=34.905 feet=0.400 blocker=solid9 oneWay=False minY=0.000
MEASURE A05 enemy-spawn x=18.000 feet=1.010 blockers=none support=True
MEASURE A05 patrol-end x=14.000 feet=1.010 blockers=none support=True
MEASURE A05 patrol-end x=23.000 feet=1.010 blockers=none support=True
MEASURE A05 summary samples=209 clearance=12 enemy=0 patrol=0
FAIL A06 clearance surface=solid1 x=-0.095 feet=13.000 blocker=solid0 oneWay=False minY=13.000
FAIL A06 clearance surface=solid3 x=48.155 feet=13.000 blocker=solid0 oneWay=False minY=13.000
FAIL A06 clearance surface=solid4 x=9.155 feet=0.400 blocker=solid8 oneWay=False minY=0.000
FAIL A06 clearance surface=solid4 x=21.905 feet=0.400 blocker=solid9 oneWay=False minY=0.000
FAIL A06 clearance surface=solid8 x=-0.095 feet=1.000 blocker=solid1 oneWay=False minY=1.000
FAIL A06 clearance surface=solid8 x=1.155 feet=1.000 blocker=solid1 oneWay=False minY=1.000
FAIL A06 clearance surface=solid8 x=2.905 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A06 clearance surface=solid8 x=5.655 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A06 clearance surface=solid9 x=24.405 feet=1.000 blocker=solid6 oneWay=False minY=1.000
FAIL A06 clearance surface=solid9 x=27.155 feet=1.000 blocker=solid6 oneWay=False minY=1.000
FAIL A06 clearance surface=solid10 x=37.405 feet=1.000 blocker=solid7 oneWay=False minY=1.000
FAIL A06 clearance surface=solid10 x=39.655 feet=1.000 blocker=solid7 oneWay=False minY=1.000
FAIL A06 clearance surface=solid10 x=46.905 feet=1.000 blocker=solid2 oneWay=False minY=1.000
FAIL A06 clearance surface=solid10 x=48.155 feet=1.000 blocker=solid2 oneWay=False minY=1.000
FAIL A06 clearance surface=platform0 x=1.155 feet=7.000 blocker=solid1 oneWay=False minY=1.000
MEASURE A06 enemy-spawn x=42.400 feet=5.010 blockers=none support=True
MEASURE A06 patrol-end x=41.500 feet=5.010 blockers=none support=True
MEASURE A06 patrol-end x=43.500 feet=5.010 blockers=none support=True
MEASURE A06 enemy-spawn x=8.300 feet=5.010 blockers=none support=True
MEASURE A06 patrol-end x=7.000 feet=5.010 blockers=none support=True
MEASURE A06 patrol-end x=9.600 feet=5.010 blockers=none support=True
MEASURE A06 enemy-spawn x=45.000 feet=1.010 blockers=none support=True
MEASURE A06 patrol-end x=41.000 feet=1.010 blockers=none support=True
MEASURE A06 patrol-end x=46.000 feet=1.010 blockers=none support=True
MEASURE A06 summary samples=330 clearance=15 enemy=0 patrol=0
```

## 남은 후보 좌표표

|방|서는 면|x|발 높이|부딪힌 콜라이더|
|---|---|---:|---:|---|
|A01|solid0|-0.095|1.000|solid5|
|A01|solid0|1.155|1.000|solid5|
|A01|solid0|3.905|1.000|solid7|
|A01|solid5|-0.095|15.000|solid3|
|A01|solid6|48.155|15.000|solid4|
|A01|solid8|38.155|0.400|solid1|
|A01|solid8|40.405|0.400|solid2|
|A02|solid0|4.905|1.000|solid13|
|A02|solid0|7.655|1.000|solid13|
|A02|solid4|-0.095|21.000|closed:A02-A06|
|A02|solid4|1.155|21.000|closed:A02-A06|
|A02|solid5|-0.095|35.000|solid2|
|A02|solid7|28.155|35.000|solid3|
|A02|solid11|1.155|21.000|closed:A02-A06|
|A02|solid11|9.905|21.000|solid8|
|A02|solid14|11.155|0.400|solid0|
|A02|solid14|13.405|0.400|solid1|
|A03|solid0|16.905|1.000|solid9|
|A03|solid0|19.655|1.000|solid9|
|A03|solid1|23.905|1.000|solid10|
|A03|solid1|26.655|1.000|solid10|
|A03|solid5|-0.095|15.000|closed:A03-B01|
|A03|solid5|1.155|15.000|closed:A03-B01|
|A03|solid6|-0.095|19.000|solid3|
|A03|solid8|50.155|19.000|solid3|
|A03|solid11|20.655|0.400|solid0|
|A03|solid11|22.905|0.400|solid1|
|A03|solid12|36.155|0.400|solid1|
|A03|solid12|38.405|0.400|solid2|
|A03|platform4|1.155|15.000|closed:A03-B01|
|A04|solid1|-0.095|11.000|solid0|
|A04|solid2|50.155|11.000|solid0|
|A04|solid5|7.905|1.000|solid3|
|A04|solid5|10.655|1.000|solid3|
|A04|solid7|22.155|0.400|solid6|
|A04|solid7|29.405|0.400|solid4|
|A04|solid7|32.655|0.400|solid4|
|A04|solid7|39.905|0.400|solid8|
|A04|solid9|48.905|1.000|solid2|
|A04|solid9|50.155|1.000|solid2|
|A04|solid10|17.655|0.400|solid5|
|A04|solid10|19.905|0.400|solid6|
|A05|solid2|-0.095|11.000|solid0|
|A05|solid3|50.155|11.000|solid1|
|A05|solid4|6.155|0.400|solid6|
|A05|solid4|11.905|0.400|solid7|
|A05|solid6|0.905|1.000|breakable0|
|A05|solid6|2.155|1.000|breakable0|
|A05|solid9|35.905|1.000|solid5|
|A05|solid9|39.155|1.000|solid5|
|A05|solid9|48.905|1.000|solid3|
|A05|solid9|50.155|1.000|solid3|
|A05|solid10|32.655|0.400|solid8|
|A05|solid10|34.905|0.400|solid9|
|A06|solid1|-0.095|13.000|solid0|
|A06|solid3|48.155|13.000|solid0|
|A06|solid4|9.155|0.400|solid8|
|A06|solid4|21.905|0.400|solid9|
|A06|solid8|-0.095|1.000|solid1|
|A06|solid8|1.155|1.000|solid1|
|A06|solid8|2.905|1.000|solid5|
|A06|solid8|5.655|1.000|solid5|
|A06|solid9|24.405|1.000|solid6|
|A06|solid9|27.155|1.000|solid6|
|A06|solid10|37.405|1.000|solid7|
|A06|solid10|39.655|1.000|solid7|
|A06|solid10|46.905|1.000|solid2|
|A06|solid10|48.155|1.000|solid2|
|A06|platform0|1.155|7.000|solid1|

T4 PARTIAL: 적 박힘0·순찰 끝 이상0, 기존 A02 solid13/platform0 후보와 solid14 적 박힘 해소. 전체 끼임 후보는76→69라 후보0 기준 미충족. 남은69건은 검사상 몸이 계단·벽·구덩이 가장자리·닫힌 문 측면과 겹치는 자리이며 데이터 수정은 하지 않는다. 사용자/Claude 검토 자료로 돌린다.

Quiet 실제 REGRESSION 13/13. [원문](_raw/regression/B18-T4/result.txt).
