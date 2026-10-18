# B18 BL4 — 실제 입력 경로 측정

측정만 수행. 구간당120초, 실제 가상 키보드 입력과 PlayerMotor를 사용했다. 방 안 위치 보정0. 선행 구간 미도착 시에만 작성된 도착 spawn으로 독립 fixture를 시작한다. BLOCKED 경로 행은 입력 경로의 측정 한계이며 게임 지형의 물리적 불가능 증명이 아니다. 튜닝·Scene·Design 변경0.

|결과|방|목표|시간s|마지막 위치|접촉|
|---|---|---|---:|---|---|
|BLOCKED|A01|A06|120.05|(24.95, 4.39)||
|PASS|A06|Shard|9.58|(3.03, 7.68)||
|PASS|A06|A02|7.78|(46.58, 5.69)|solid11|
|PASS|A02|Shard|0.05|(3.50, 21.01)|solid11|

모든 구간별 sample과 마지막 행에 실제 접촉 콜라이더 이름을 남겼다. 접촉 없음은 빈 문자열이며, 해당 프레임이 공중/자유 이동 상태라는 뜻이다. 원문: [BL4-route.txt](_raw/B18/BL4-route.txt).

```text
PASS B16V5Verifier actual=exe
PASS isolated save path
PASS V5 six-room layout
PASS launcher start room
PASS fixture dash
PASS fixture double jump
PASS map switch
PASS existing schema checkpoint saved
MEASURE route limit=120s/segment input=virtual-keyboard teleportWithinSegment=0 abilities=DJ
PASS DJ owned fixture
MEASURE route recipe=draft3-DJ contacts=actual-player-collider eachSegmentLimit=120
MEASURE route natural-continuation room=A01
MEASURE route begin room=A01 spawn=start position=(14.00, 1.01) hp=4 dash=False dj=True
MEASURE route contacts room=A01 position=(14.00, 1.01) colliders=solid1
MEASURE route sample room=A01 elapsed=0.0 step=0 position=(14.00, 1.01) grounded=True hp=4 pogos=0
MEASURE route waypoint room=A01 step=1 position=(5.64, 3.07)
MEASURE route waypoint room=A01 step=2 position=(9.31, 4.81)
MEASURE route waypoint room=A01 step=3 position=(15.86, 9.33)
MEASURE route contacts room=A01 position=(24.95, 1.90) colliders=
MEASURE route sample room=A01 elapsed=10.0 step=3 position=(24.95, 1.90) grounded=False hp=4 pogos=1
MEASURE route contacts room=A01 position=(24.95, 3.08) colliders=
MEASURE route sample room=A01 elapsed=20.0 step=3 position=(24.95, 3.08) grounded=False hp=4 pogos=1
MEASURE route contacts room=A01 position=(24.95, 3.30) colliders=
MEASURE route sample room=A01 elapsed=30.0 step=3 position=(24.95, 3.30) grounded=False hp=4 pogos=2
MEASURE route contacts room=A01 position=(24.95, 3.52) colliders=
MEASURE route sample room=A01 elapsed=40.0 step=3 position=(24.95, 3.52) grounded=False hp=4 pogos=3
MEASURE route contacts room=A01 position=(24.95, 2.60) colliders=
MEASURE route sample room=A01 elapsed=50.0 step=3 position=(24.95, 2.60) grounded=False hp=4 pogos=3
MEASURE route contacts room=A01 position=(24.95, 1.01) colliders=solid1
MEASURE route sample room=A01 elapsed=60.0 step=3 position=(24.95, 1.01) grounded=False hp=4 pogos=4
MEASURE route contacts room=A01 position=(24.95, 4.46) colliders=
MEASURE route sample room=A01 elapsed=70.0 step=3 position=(24.95, 4.46) grounded=False hp=4 pogos=4
MEASURE route contacts room=A01 position=(24.95, 3.46) colliders=
MEASURE route sample room=A01 elapsed=80.0 step=3 position=(24.95, 3.46) grounded=False hp=4 pogos=4
MEASURE route contacts room=A01 position=(24.95, 3.49) colliders=
MEASURE route sample room=A01 elapsed=90.0 step=3 position=(24.95, 3.49) grounded=False hp=4 pogos=4
MEASURE route contacts room=A01 position=(24.95, 2.60) colliders=
MEASURE route sample room=A01 elapsed=100.0 step=3 position=(24.95, 2.60) grounded=False hp=4 pogos=4
MEASURE route contacts room=A01 position=(24.95, 3.31) colliders=
MEASURE route sample room=A01 elapsed=110.0 step=3 position=(24.95, 3.31) grounded=False hp=4 pogos=4
BLOCKED route A01 goal=A06 elapsed=120.05 position=(24.95, 4.39) step=3 maxX=24.95 maxY=10.60 hp=4 pogos=4 contacts= reason=120s input budget exhausted
MEASURE route independent-fixture room=A06 spawn=A01-A06 after previous segment did not arrive
PASS route arrival fixture A06
MEASURE route begin room=A06 spawn=A01-A06 position=(36.85, 1.01) hp=4 dash=False dj=True
MEASURE route contacts room=A06 position=(36.85, 1.01) colliders=solid10
MEASURE route sample room=A06 elapsed=0.0 step=0 position=(36.85, 1.01) grounded=True hp=4 pogos=4
MEASURE route waypoint room=A06 step=1 position=(38.26, 3.49)
MEASURE route waypoint room=A06 step=2 position=(42.33, 7.41)
MEASURE route waypoint room=A06 step=3 position=(32.13, 6.31)
MEASURE route waypoint room=A06 step=4 position=(25.65, 5.68)
MEASURE route waypoint room=A06 step=5 position=(17.83, 3.79)
MEASURE route waypoint room=A06 step=6 position=(12.15, 3.97)
MEASURE route waypoint room=A06 step=7 position=(8.19, 8.12)
PASS route A06 goal=Shard elapsed=9.58 position=(3.03, 7.68) step=7 maxX=42.48 maxY=9.01 hp=2 pogos=7 contacts=
MEASURE route natural-continuation room=A06
MEASURE route begin room=A06 spawn=A01-A06 position=(3.03, 7.01) hp=2 dash=False dj=True
MEASURE route contacts room=A06 position=(3.03, 7.01) colliders=platform0
MEASURE route sample room=A06 elapsed=0.0 step=0 position=(3.03, 7.01) grounded=True hp=2 pogos=7
MEASURE route waypoint room=A06 step=1 position=(7.77, 6.45)
MEASURE route waypoint room=A06 step=2 position=(25.28, 4.55)
MEASURE route waypoint room=A06 step=3 position=(31.76, 4.80)
MEASURE route waypoint room=A06 step=4 position=(38.24, 6.40)
MEASURE route waypoint room=A06 step=5 position=(42.26, 7.22)
MEASURE route waypoint room=A06 step=6 position=(45.26, 8.35)
PASS route A06 goal=A02 elapsed=7.78 position=(46.58, 5.69) step=6 maxX=46.58 maxY=9.53 hp=1 pogos=10 contacts=solid11
MEASURE route natural-continuation room=A02
MEASURE route begin room=A02 spawn=A02-A06 position=(3.50, 21.01) hp=1 dash=False dj=True
PASS route A02 goal=Shard elapsed=0.05 position=(3.50, 21.01) step=0 maxX=3.50 maxY=21.01 hp=1 pogos=10 contacts=solid11
Error/Exception/Assert=0
```

BL4 DONE(측정), 3구간 PASS/1구간 BLOCKED. A01은 x24.95에서 목표 발 높이8.8까지 이어가지 못한 입력 recipe 한계다. 120초 끝 위치(24.95,4.39), 최종 접촉 없음이므로 물리 콜라이더가 가뒀다고 판정하지 않는다. 이후 A06은 작성된 도착 spawn 독립 fixture였고 왼쪽 조각9.58초, 일방 A02 이동7.78초, 감실 조각0.05초를 통과했다. 자연 DJ 전체 완주가 아니다. A06 왼쪽 구간 포고 카운터는4→7(3회 증가)이지만 개별 타격 대상 ID는 기록하지 않아 가시3개 각각의 포고 성공으로 확정하지 않는다. 통과행 contacts는 종료 시 도착방일 수 있고, sample은 그 행의 현재 방이다. 전체 Error0·Quiet 실제 REGRESSION 13/13. [회귀](_raw/regression/B18-BL4/result.txt).
