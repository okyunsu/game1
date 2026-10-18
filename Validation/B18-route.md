# B18 BL1 — 실제 입력 경로 측정

측정만 수행. 구간당120초, 실제 가상 키보드 입력과 PlayerMotor를 사용했다. 방 안 위치 보정0. 선행 구간 미도착 시에만 작성된 도착 spawn으로 독립 fixture를 시작한다. BLOCKED 경로 행은 입력 경로의 측정 한계이며 게임 지형의 물리적 불가능 증명이 아니다. 튜닝·Scene·Design 변경0.

|결과|방|목표|시간s|마지막 위치|접촉|
|---|---|---|---:|---|---|
|PASS|A01|A02|5.97|(46.57, 2.09)|solid0|
|PASS|A02|A03|4.87|(26.60, 9.01)|platform0|
|PASS|A03|A04|8.70|(48.56, 15.53)|solid5|
|PASS|A04|A05|8.08|(46.11, 1.29)||
|PASS|A05|Dash|1.88|(37.64, 3.38)||

모든 구간별 sample과 마지막 행에 실제 접촉 콜라이더 이름을 남겼다. 접촉 없음은 빈 문자열이며, 해당 프레임이 공중/자유 이동 상태라는 뜻이다. 원문: [BL1-route.txt](_raw/B18/BL1-route.txt).

```text
PASS B16V5Verifier actual=exe
PASS isolated save path
PASS V5 six-room layout
PASS launcher start room
PASS fixture dash
PASS fixture double jump
PASS map switch
PASS existing schema checkpoint saved
MEASURE route limit=120s/segment input=virtual-keyboard teleportWithinSegment=0 abilities=none
MEASURE route recipe=draft3 contacts=actual-player-collider tuningChanges=0
MEASURE route natural-continuation room=A01
MEASURE route begin room=A01 spawn=start position=(14.00, 1.01) hp=4 dash=False dj=False
MEASURE route contacts room=A01 position=(14.00, 1.01) colliders=solid1
MEASURE route sample room=A01 elapsed=0.0 step=0 position=(14.00, 1.01) grounded=True hp=4 pogos=0
PASS route A01 goal=A02 elapsed=5.97 position=(46.57, 2.09) step=0 maxX=46.57 maxY=4.04 hp=3 pogos=1 contacts=solid0
MEASURE route natural-continuation room=A02
MEASURE route begin room=A02 spawn=A01-A02 position=(3.50, 1.01) hp=3 dash=False dj=False
MEASURE route contacts room=A02 position=(3.50, 1.01) colliders=solid0
MEASURE route sample room=A02 elapsed=0.0 step=0 position=(3.50, 1.01) grounded=True hp=3 pogos=1
PASS route A02 goal=A03 elapsed=4.87 position=(26.60, 9.01) step=0 maxX=26.60 maxY=11.53 hp=2 pogos=2 contacts=platform0
MEASURE route natural-continuation room=A03
MEASURE route begin room=A03 spawn=A02-A03 position=(3.50, 9.01) hp=2 dash=False dj=False
MEASURE route contacts room=A03 position=(3.50, 9.01) colliders=platform0
MEASURE route sample room=A03 elapsed=0.0 step=0 position=(3.50, 9.01) grounded=True hp=2 pogos=2
MEASURE route waypoint room=A03 step=1 position=(18.08, 13.35)
MEASURE route waypoint room=A03 step=2 position=(24.76, 3.49)
MEASURE route waypoint room=A03 step=3 position=(28.96, 6.79)
MEASURE route waypoint room=A03 step=4 position=(32.56, 5.01)
MEASURE route waypoint room=A03 step=5 position=(35.20, 7.53)
MEASURE route waypoint room=A03 step=6 position=(38.56, 9.49)
MEASURE route waypoint room=A03 step=7 position=(41.76, 11.49)
MEASURE route waypoint room=A03 step=8 position=(44.96, 13.49)
PASS route A03 goal=A04 elapsed=8.70 position=(48.56, 15.53) step=8 maxX=48.56 maxY=15.53 hp=2 pogos=5 contacts=solid5
MEASURE route natural-continuation room=A04
MEASURE route begin room=A04 spawn=A03-A04 position=(3.50, 1.01) hp=2 dash=False dj=False
MEASURE route contacts room=A04 position=(3.50, 1.01) colliders=solid5
MEASURE route sample room=A04 elapsed=0.0 step=0 position=(3.50, 1.01) grounded=True hp=2 pogos=5
MEASURE route waypoint room=A04 step=1 position=(9.00, 3.38)
MEASURE route waypoint room=A04 step=2 position=(14.28, 5.90)
MEASURE route waypoint room=A04 step=3 position=(16.56, 8.80)
MEASURE route waypoint room=A04 step=4 position=(18.84, 7.49)
MEASURE route waypoint room=A04 step=5 position=(21.84, 9.33)
MEASURE route waypoint room=A04 step=6 position=(41.28, 8.20)
MEASURE route waypoint room=A04 step=7 position=(42.84, 7.99)
PASS route A04 goal=A05 elapsed=8.08 position=(46.11, 1.29) step=7 maxX=46.11 maxY=9.39 hp=2 pogos=7 contacts=
MEASURE route natural-continuation room=A05
MEASURE route begin room=A05 spawn=A04-A05 position=(46.50, 8.65) hp=2 dash=False dj=False
MEASURE route contacts room=A05 position=(46.50, 8.65) colliders=
MEASURE route sample room=A05 elapsed=0.0 step=0 position=(46.50, 8.65) grounded=False hp=2 pogos=7
MEASURE route waypoint room=A05 step=1 position=(40.69, 3.44)
PASS route A05 goal=Dash elapsed=1.88 position=(37.64, 3.38) step=1 maxX=46.50 maxY=8.65 hp=2 pogos=7 contacts=
Error/Exception/Assert=0
```

BL1 측정 완료: 다섯 구간 모두 실제 입력 PASS, 자연 연속으로 대시 수집까지 도달. 적 포고 사용(최종7회), HP4→2. A02→A03은 위쪽 문으로 도착했다(발9.01). 통과행 position은 출발방의 마지막 위치이고 contacts는 종료 시점의 도착방일 수 있다. 막힌 행은 이번 실행에 없으며, sample 접촉은 그 행의 현재 방이다. 사람의 난이도·체감은 미수락.

BL1 DONE(측정). 컴파일 오류0. Quiet 실제 REGRESSION 13/13. [회귀](_raw/regression/B18-BL1/result.txt).
