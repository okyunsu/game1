# B17 BL1 — draft2 실제 입력 경로 측정

측정 합격: 통과 구간·막힌 좌표·원문 기록 완료. 자연 전체 대시 완주는 미확인. A01→A02→A03은 자연 연결, A03 동쪽 포고/A04 상부 진입은 각120초 입력 한도로 BLOCKED. 이후 A04/A05는 작성된 도착 spawn을 이용한 독립 fixture이며 자연 연결로 세지 않는다.

|구간|결과|시간s|마지막 위치|비고|
|---|---|---:|---|---|
|A01 시작→A02|PASS|6.23|(46.52,1.01)|자연, HP4→3|
|A02 바닥→A03|PASS|5.25|(26.58,1.73)|자연, HP3→2|
|A03 동쪽 위→A04|BLOCKED|120.05|(40.24,2.78)|L2/L3 포고 입력 놓침 후보, step8, maxY10.03, HP1|
|A04 위 통로→구멍→A05|BLOCKED|120.05|(19.35,0.58)|독립 도착, 위 발판→Lk 접근 뒤 구덩이 바닥에 남음, maxY7.51|
|A05 낙하→대시|PASS|2.10|(37.43,3.33)|독립 도착, 자연 pickup 입력|

`-verify-b16-route -b17-route`로 가상 키보드 입력. 방 안 teleport0, 능력 없이 시작, 튜닝/Scene 수정0. 새 블록·구덩이 때문에 주기 점프를 사용했고 A03 높아진 출구를13u로 입력했다. A03/A04의 waypoints는 작성된 블록·발판·등불 좌표 주변을 입력 목표로 택했다. 측정용 입력 좌표이며 기본 게임 동작이나 설계 수치를 바꾸지 않는다. 이전 Batch16b의 입력은 기본 flag 동작으로 그대로 남겼다.

[원본](_raw/B17/BL1-route.txt), [실행 로그](_raw/B17/BL1-route.log), [빌드](_raw/B17/BL1-build.txt).

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
MEASURE route recipe=draft2 grounded-periodic-jump=True new-block-and-pit-waypoints=True raised-A03-exit=True tuningChanges=0
MEASURE route natural-continuation room=A01
MEASURE route begin room=A01 spawn=start position=(14.00, 1.01) hp=4 dash=False dj=False
MEASURE route sample room=A01 elapsed=0.0 step=0 position=(14.00, 1.01) grounded=True hp=4 pogos=0
PASS route A01 goal=A02 elapsed=6.23 position=(46.52, 1.01) step=0 maxX=46.52 maxY=5.00 hp=3 pogos=1
MEASURE route natural-continuation room=A02
MEASURE route begin room=A02 spawn=A01-A02 position=(3.50, 1.01) hp=3 dash=False dj=False
MEASURE route sample room=A02 elapsed=0.0 step=0 position=(3.50, 1.01) grounded=True hp=3 pogos=1
PASS route A02 goal=A03 elapsed=5.25 position=(26.58, 1.73) step=0 maxX=26.58 maxY=9.00 hp=2 pogos=3
MEASURE route natural-continuation room=A03
MEASURE route begin room=A03 spawn=A02-A03 position=(3.50, 1.01) hp=2 dash=False dj=False
MEASURE route sample room=A03 elapsed=0.0 step=0 position=(3.50, 1.01) grounded=True hp=2 pogos=3
MEASURE route waypoint room=A03 step=1 position=(14.28, 1.70)
MEASURE route waypoint room=A03 step=2 position=(18.36, 3.07)
MEASURE route waypoint room=A03 step=3 position=(22.32, 4.93)
MEASURE route waypoint room=A03 step=4 position=(25.36, 3.07)
MEASURE route waypoint room=A03 step=5 position=(30.25, 7.28)
MEASURE route waypoint room=A03 step=6 position=(32.17, 9.36)
MEASURE route waypoint room=A03 step=7 position=(36.25, 10.03)
MEASURE route waypoint room=A03 step=8 position=(37.81, 8.75)
MEASURE route sample room=A03 elapsed=10.0 step=8 position=(40.24, 3.10) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=20.0 step=8 position=(40.24, 2.41) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=30.0 step=8 position=(40.24, 3.10) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=40.0 step=8 position=(40.24, 2.41) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=50.0 step=8 position=(40.24, 3.10) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=60.0 step=8 position=(40.24, 2.20) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=70.0 step=8 position=(40.24, 3.20) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=80.0 step=8 position=(40.24, 2.20) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=90.0 step=8 position=(40.24, 3.20) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=100.0 step=8 position=(40.24, 2.20) grounded=False hp=1 pogos=6
MEASURE route sample room=A03 elapsed=110.0 step=8 position=(40.24, 3.20) grounded=False hp=1 pogos=6
BLOCKED route A03 goal=A04 elapsed=120.05 position=(40.24, 2.78) step=8 maxX=40.24 maxY=10.03 hp=1 pogos=6 reason=120s input budget exhausted
MEASURE route independent-fixture room=A04 spawn=A03-A04 after previous segment did not arrive
PASS route arrival fixture A04
MEASURE route begin room=A04 spawn=A03-A04 position=(3.50, 1.01) hp=1 dash=False dj=False
MEASURE route sample room=A04 elapsed=0.0 step=0 position=(3.50, 1.01) grounded=False hp=1 pogos=6
MEASURE route waypoint room=A04 step=1 position=(6.33, 3.44)
MEASURE route waypoint room=A04 step=2 position=(9.00, 3.38)
MEASURE route waypoint room=A04 step=3 position=(14.28, 5.70)
MEASURE route waypoint room=A04 step=4 position=(16.32, 7.51)
MEASURE route sample room=A04 elapsed=10.0 step=4 position=(19.35, 2.90) grounded=False hp=1 pogos=6
MEASURE route sample room=A04 elapsed=20.0 step=4 position=(19.35, 0.41) grounded=False hp=1 pogos=6
MEASURE route sample room=A04 elapsed=30.0 step=4 position=(19.35, 2.90) grounded=False hp=1 pogos=6
MEASURE route sample room=A04 elapsed=40.0 step=4 position=(19.35, 0.41) grounded=False hp=1 pogos=6
MEASURE route sample room=A04 elapsed=50.0 step=4 position=(19.35, 2.90) grounded=False hp=1 pogos=6
MEASURE route sample room=A04 elapsed=60.0 step=4 position=(19.35, 0.41) grounded=False hp=1 pogos=6
MEASURE route sample room=A04 elapsed=70.0 step=4 position=(19.35, 2.90) grounded=False hp=1 pogos=6
MEASURE route sample room=A04 elapsed=80.0 step=4 position=(19.35, 0.41) grounded=True hp=1 pogos=6
MEASURE route sample room=A04 elapsed=90.0 step=4 position=(19.35, 2.92) grounded=False hp=1 pogos=6
MEASURE route sample room=A04 elapsed=100.0 step=4 position=(19.35, 0.41) grounded=True hp=1 pogos=6
MEASURE route sample room=A04 elapsed=110.0 step=4 position=(19.35, 2.92) grounded=False hp=1 pogos=6
BLOCKED route A04 goal=A05 elapsed=120.05 position=(19.35, 0.58) step=4 maxX=19.35 maxY=7.51 hp=1 pogos=6 reason=120s input budget exhausted
MEASURE route independent-fixture room=A05 spawn=A04-A05 after previous segment did not arrive
PASS route arrival fixture A05
MEASURE route begin room=A05 spawn=A04-A05 position=(46.50, 9.28) hp=1 dash=False dj=False
MEASURE route sample room=A05 elapsed=0.0 step=0 position=(46.50, 9.28) grounded=False hp=1 pogos=6
MEASURE route waypoint room=A05 step=1 position=(40.69, 3.46)
PASS route A05 goal=Dash elapsed=2.10 position=(37.43, 3.33) step=1 maxX=46.50 maxY=9.28 hp=1 pogos=6
MEASURE route measurement complete; BLOCKED rows are controller limits, not proof of impossible geometry
Error/Exception/Assert=0
```

## 재시도 경로 중단 — 외부 라이선스

초회에서 A04의 주기 점프가 너무 일찍 시작한 것으로 보이는 입력 후보가 있어, A03/A04는 기존 목표 높이·적 반응 점프만 쓰는1회 재시도를 준비했다. Unity가 새 빌드 전에 exit198을 반환했고, 라이선스 연결의 일시 오류인지 같은 빌드를1회 더 실행해도 같았다. 재시도 exe/경로 측정은 실행하지 않았다. 아직 컴파일하지 못한 입력 변경은 초회 빌드에서 성공한 상태로 복원했다. 따라서 새 실패 제품 코드/Scene은 남지 않았다. 원본 두 로그를 보존한다.

```text
[Licensing::Module] Error: 'com.unity.editor.headless' was not found.
No valid Unity Editor license found. Please activate your license.
Exiting without the bug reporter. Application will terminate with return code 198
```

원본: [첫 거부](_raw/B17/BL1-r1-build.log), [라이선스 재시도](_raw/B17/BL1-r1-build-license-retry.log). 포고가 물리적으로 불가능하다는 판정은 하지 않는다. 사용자 라이선스 복구 전 입력 개선 재시도 경로만 BLOCKED. Task 종료 Quiet 결과는 실제 실행 후 덧붙인다.

## Task 종료 Quiet — 실행 불가

게임 검사가 실행되기 전에 모든 Editor가 라이선스 exit198로 종료됐다. 아래0/13은 제품 회귀13건 실패 판정이 아니라 검증 도구 실행 불가다. 이전 T1~T7 Quiet는 실제13/13이었다.

```text
S1-02 FAIL | S1-02 FAIL exit=198 evidence=S1-02-night-S2-B17-BL1.txt mode=headless
S1-04 FAIL | S1-04 FAIL exit=198 evidence=S1-04-night-S2-B17-BL1.txt mode=headless
S1-07 FAIL | S1-07 FAIL exit=198 evidence=S1-07-night-S2-B17-BL1.txt mode=headless
S1-08 FAIL | S1-08 FAIL exit=198 evidence=S1-08-night-S2-B17-BL1.txt mode=headless
Camera FAIL | Camera FAIL exit=198 evidence=S1-06-lead0-night-S2-B17-BL1.txt mode=graphics-640x360
T1 FAIL | T1 FAIL exit=198 evidence=Verification-night-T1-B17-BL1.txt mode=headless
T2 FAIL | T2 FAIL exit=198 evidence=Verification-night-T2-B17-BL1.txt mode=headless
T3 FAIL | T3 FAIL exit=198 evidence=Verification-night-T3-B17-BL1.txt mode=headless
T4 FAIL | T4 FAIL exit=198 evidence=Verification-T4-night-S2-B17-BL1.txt mode=headless
S2 FAIL | S2 FAIL exit=198 evidence=S2-01-integration-night-S2-B17-BL1.txt mode=headless
F2R FAIL | F2R FAIL exit=198 evidence=F2R-feedback-night-S2-B17-BL1.txt mode=headless
LOGR FAIL | LOGR FAIL exit=198 evidence=LOGR-playtest-night-S2-B17-BL1.txt mode=headless
Boss FAIL | Boss FAIL exit=198 evidence=boss-recovery-night-S2-B17-BL1.txt mode=headless
REGRESSION 0/13
```
