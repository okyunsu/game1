# B18 T2 — draft2 실제 Scene 검사

Player.prefab BoxCollider2D 0.7×1.6을 직접 읽었다. 지지면 양끝 ±몸 반폭을 포함한 0.25u 샘플과 확장 높이2.2u를 검사한다. 내부에 덮인 바닥은 제외하며, 각 후보는 양의 실제 콜라이더 겹침이다. 한 방향 통과 발판은 별도 MEASURE이며 현재 Scene에는 없다. 접촉 경계0.003u와 양의 침투를 구분한다. 적·순찰 끝점은 프리팹 몸 크기와 Scene Transform을 사용한다.

|방|서 있는 자리 샘플|끼임 후보|적 박힘/지지 이상|순찰 끝 이상|
|---|---:|---:|---:|---:|
|A01|318|8|0|0|
|A02|297|14|1|0|
|A03|445|13|0|0|
|A04|326|12|0|0|
|A05|209|12|0|0|
|A06|333|17|0|0|

T2 검출 합격: T1과 같은 A02 블록 끝·발판, 블록 안 적이 모두 FAIL. 검사 CLI의 exit1은 검출 결과이며 컴파일 실패가 아니다.

실제 원문: [T2-before-final.txt](_raw/B18/T2-before-final.txt).

```text
MEASURE player prefab size=(0.70, 1.60) step=0.25 extraHeight=0.6 edgeExtension=0.35
FAIL A01 clearance surface=solid0 x=-0.095 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A01 clearance surface=solid0 x=1.155 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A01 clearance surface=solid0 x=3.905 feet=1.000 blocker=solid7 oneWay=False minY=1.000
FAIL A01 clearance surface=solid5 x=-0.095 feet=15.000 blocker=solid3 oneWay=False minY=15.000
FAIL A01 clearance surface=solid6 x=48.155 feet=15.000 blocker=solid4 oneWay=False minY=15.000
FAIL A01 clearance surface=solid7 x=7.345 feet=3.000 blocker=platform0 oneWay=False minY=4.300
FAIL A01 clearance surface=solid8 x=38.155 feet=0.400 blocker=solid1 oneWay=False minY=0.000
FAIL A01 clearance surface=solid8 x=40.405 feet=0.400 blocker=solid2 oneWay=False minY=0.000
MEASURE A01 enemy-spawn x=17.000 feet=6.810 blockers=none support=True
MEASURE A01 patrol-end x=14.500 feet=6.810 blockers=none support=True
MEASURE A01 patrol-end x=21.000 feet=6.810 blockers=none support=True
MEASURE A01 enemy-spawn x=31.000 feet=1.010 blockers=none support=True
MEASURE A01 patrol-end x=26.000 feet=1.010 blockers=none support=True
MEASURE A01 patrol-end x=36.000 feet=1.010 blockers=none support=True
MEASURE A01 summary samples=318 clearance=8 enemy=0 patrol=0
FAIL A02 clearance surface=solid0 x=4.905 feet=1.000 blocker=solid13 oneWay=False minY=1.000
FAIL A02 clearance surface=solid0 x=7.655 feet=1.000 blocker=solid13 oneWay=False minY=1.000
FAIL A02 clearance surface=solid4 x=-0.095 feet=21.000 blocker=closed:A02-A06 oneWay=False minY=21.000
FAIL A02 clearance surface=solid4 x=1.155 feet=21.000 blocker=closed:A02-A06 oneWay=False minY=21.000
FAIL A02 clearance surface=solid5 x=-0.095 feet=35.000 blocker=solid2 oneWay=False minY=35.000
FAIL A02 clearance surface=solid7 x=28.155 feet=35.000 blocker=solid3 oneWay=False minY=35.000
FAIL A02 clearance surface=solid11 x=1.155 feet=21.000 blocker=closed:A02-A06 oneWay=False minY=21.000
FAIL A02 clearance surface=solid11 x=9.905 feet=21.000 blocker=solid8 oneWay=False minY=11.000
FAIL A02 clearance surface=solid13 x=7.845 feet=3.000 blocker=platform0 oneWay=False minY=4.500
FAIL A02 clearance surface=solid15 x=11.155 feet=0.400 blocker=solid0 oneWay=False minY=0.000
FAIL A02 clearance surface=solid15 x=13.405 feet=0.400 blocker=solid1 oneWay=False minY=0.000
FAIL A02 clearance surface=platform1 x=17.905 feet=9.000 blocker=solid14 oneWay=False minY=9.000
FAIL A02 clearance surface=platform1 x=20.155 feet=9.000 blocker=solid14 oneWay=False minY=9.000
FAIL A02 clearance surface=platform2 x=12.155 feet=12.400 blocker=climb0 oneWay=False minY=11.000
FAIL A02 enemy-spawn prefab=E1 x=19.000 feet=9.010 blockers=solid14 support=True
MEASURE A02 patrol-end x=15.000 feet=9.010 blockers=none support=True
MEASURE A02 patrol-end x=22.000 feet=9.010 blockers=none support=True
MEASURE A02 enemy-spawn x=20.000 feet=1.010 blockers=none support=True
MEASURE A02 patrol-end x=15.500 feet=1.010 blockers=none support=True
MEASURE A02 patrol-end x=24.500 feet=1.010 blockers=none support=True
MEASURE A02 summary samples=297 clearance=14 enemy=1 patrol=0
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
FAIL A03 clearance surface=platform2 x=1.155 feet=15.000 blocker=closed:A03-B01 oneWay=False minY=15.000
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
MEASURE A03 summary samples=445 clearance=13 enemy=0 patrol=0
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
MEASURE A04 summary samples=326 clearance=12 enemy=0 patrol=0
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
FAIL A06 clearance surface=solid6 x=27.845 feet=3.000 blocker=platform2 oneWay=False minY=3.400
FAIL A06 clearance surface=solid7 x=39.845 feet=3.000 blocker=platform3 oneWay=False minY=4.500
FAIL A06 clearance surface=solid8 x=-0.095 feet=1.000 blocker=solid1 oneWay=False minY=1.000
FAIL A06 clearance surface=solid8 x=1.155 feet=1.000 blocker=solid1 oneWay=False minY=1.000
FAIL A06 clearance surface=solid8 x=2.905 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A06 clearance surface=solid8 x=5.655 feet=1.000 blocker=solid5 oneWay=False minY=1.000
FAIL A06 clearance surface=solid9 x=24.905 feet=1.000 blocker=solid6 oneWay=False minY=1.000
FAIL A06 clearance surface=solid9 x=27.655 feet=1.000 blocker=solid6 oneWay=False minY=1.000
FAIL A06 clearance surface=solid10 x=37.405 feet=1.000 blocker=solid7 oneWay=False minY=1.000
FAIL A06 clearance surface=solid10 x=39.655 feet=1.000 blocker=solid7 oneWay=False minY=1.000
FAIL A06 clearance surface=solid10 x=46.905 feet=1.000 blocker=solid2 oneWay=False minY=1.000
FAIL A06 clearance surface=solid10 x=48.155 feet=1.000 blocker=solid2 oneWay=False minY=1.000
FAIL A06 clearance surface=platform0 x=1.155 feet=7.000 blocker=solid1 oneWay=False minY=1.000
MEASURE A06 enemy-spawn x=42.000 feet=5.010 blockers=none support=True
MEASURE A06 patrol-end x=40.800 feet=5.010 blockers=none support=True
MEASURE A06 patrol-end x=43.200 feet=5.010 blockers=none support=True
MEASURE A06 enemy-spawn x=8.300 feet=5.010 blockers=none support=True
MEASURE A06 patrol-end x=7.000 feet=5.010 blockers=none support=True
MEASURE A06 patrol-end x=9.600 feet=5.010 blockers=none support=True
MEASURE A06 enemy-spawn x=45.000 feet=1.010 blockers=none support=True
MEASURE A06 patrol-end x=41.000 feet=1.010 blockers=none support=True
MEASURE A06 patrol-end x=46.000 feet=1.010 blockers=none support=True
MEASURE A06 summary samples=333 clearance=17 enemy=0 patrol=0
```

T2 DONE. 컴파일 오류0. Quiet 실제 REGRESSION 13/13. [회귀 원문](_raw/regression/B18-T2/result.txt).
