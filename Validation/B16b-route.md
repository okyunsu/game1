# B16b BL2 — 실제 입력 경로 측정

측정 완료, 최종 Quiet 진행 중. `-verify-b16-route`는 Development 전용이며 실제 InputSystem 키보드 이동·Z 점프·아래+X 공격을 사용한다. 구간별 상한120초. 구간 안에 위치 직접 변경/능력 부여/HP 복원은0이다. 앞 구간이 목표 방에 도착하지 못하면 다음 방의 기존 도착 spawn으로 명시적 fixture 전환한 뒤 새 구간을 측정한다. 이 재개를 자연 완주로 합치지 않는다.

고정 waypoint는 원본 발판·등불·문·사당 주변을 선택한 검증 입력이다. 점프 누름0.42초, 재입력 간격0.65초, 공격 간격0.34초. 최초 가로 도착 허용0.65u, 도구 보정 후0.25u. 게임 수치·Scene·Design·Tuning 변경0. 적 접근 시 점프와 공격을 시도하며, 공중 아래 공격으로 적/등불 포고를 시도한다. 별도 경로 탐색기나 최적 입력 탐색은 만들지 않았다.

| 구간 | 기점 | 목표 |
|---|---|---|
| A01 | 실제 새 시작 | A02 바닥 문 |
| A02 | A01-A02 | A03 바닥 문 |
| A03 | A02-A03 | 거리→동쪽 발판/등불→A04 |
| A04 | A03-A04 | 위 통로→바닥 구멍→A05 |
| A05 | A04-A05 낙하 | 대시 사당 접촉 |

런너 첫 줄 PASS는 측정 도구가 마지막까지 실행됐다는 뜻이다. 자연 경로 통과 여부는 각 `PASS route`/`BLOCKED route` 줄로만 판단한다. BLOCKED는 이 고정 입력의 한도이며 사람이 통과할 수 없는 지형이라는 증거가 아니다. 실제 물리 문 전환은 별도 T3에서 검증했다.

원본 `_raw/B16b/BL2-route.txt/log`, `BL2-route-save.json`, `BL2-build.txt/log`, `BL2-compile.log`. 결과와 Quiet는 완료 후 추가한다.

최초 측정: A01→A02→A03 자연 연결(5.60s/4.02s), A03 첫 낮은 발판 앞에서120초, A04 위 발판 접근에서120초, A05 낙하 도착 fixture에서1.45초에Dash 수집. Error/Exception/Assert=0, Quiet13/13. 원문은 최종 결과에 함께 보존한다.

검사 도구 보정1회: A03 경로에 첫 낮은 발판의 접근/상단 waypoint(14.5,1 →18.5,2.9 →20.5,2.9)를 추가하고, 다음 점프를 너무 일찍 시작하던 waypoint 가로 허용을0.65→0.25u로 좁혔다. 진행 로그를 원본 log에도 기록한다. 공간/물리/능력은 변경0. 재측정 후 추가 도구 보정은 하지 않는다.

## 실제 결과

최종 자연 연결은 A01→A02→A03→A04. A03은8.12초·누적 포고3회로 동쪽 위 문을 통과했다. A04는 위 발판 접근(step3)에서 떨어진 뒤 x15.63 부근 아래쪽에서 반복 점프하며120초를 소진했다. 최대 높이5.42, HP4. 이 입력은 앞 발판으로 되돌아가 재시도하는 탐색을 하지 않는다. 실제 지형 불가로 확정하지 않고 직접 플레이/설계 검토 대상으로 남긴다. A05는 A04-A05 도착 fixture에서 Dash 수집을 확인했다. 자연 전체 경로→Dash 완주는 미확인.

각 줄의 pogos는 같은 PlayerAttack 인스턴스의 누적 카운터다. A05의4는 새 포고4회라는 뜻이 아니다. Error/Exception/Assert=0. 이전 측정 원본도 보존한다.

### BL2-route 원문

```text
PASS route A01 goal=A02 elapsed=5.60 position=(46.59, 1.02) step=0 maxX=46.59 maxY=3.54 hp=4 pogos=0
PASS route A02 goal=A03 elapsed=4.02 position=(26.61, 1.02) step=0 maxX=26.61 maxY=3.54 hp=4 pogos=0
BLOCKED route A03 goal=A04 elapsed=120.06 position=(15.64, 1.02) step=0 maxX=15.64 maxY=1.02 hp=4 pogos=0 reason=120s input budget exhausted
MEASURE route independent-fixture room=A04 spawn=A03-A04 after previous segment did not arrive
BLOCKED route A04 goal=A05 elapsed=120.05 position=(15.60, 2.60) step=3 maxX=15.60 maxY=5.20 hp=4 pogos=1 reason=120s input budget exhausted
MEASURE route independent-fixture room=A05 spawn=A04-A05 after previous segment did not arrive
PASS route A05 goal=Dash elapsed=1.45 position=(37.91, 3.38) step=1 maxX=46.00 maxY=7.25 hp=4 pogos=1
Error/Exception/Assert=0
```

### BL2-r1-route 원문

```text
PASS route A01 goal=A02 elapsed=5.59 position=(46.59, 1.02) step=0 maxX=46.59 maxY=3.54 hp=4 pogos=0
PASS route A02 goal=A03 elapsed=4.00 position=(26.61, 1.02) step=0 maxX=26.61 maxY=3.54 hp=4 pogos=0
PASS route A03 goal=A04 elapsed=8.12 position=(48.52, 11.02) step=13 maxX=48.52 maxY=13.60 hp=4 pogos=3
BLOCKED route A04 goal=A05 elapsed=120.06 position=(15.63, 1.80) step=3 maxX=15.63 maxY=5.42 hp=4 pogos=4 reason=120s input budget exhausted
MEASURE route independent-fixture room=A05 spawn=A04-A05 after previous segment did not arrive
PASS route A05 goal=Dash elapsed=1.48 position=(37.67, 3.38) step=1 maxX=46.00 maxY=7.25 hp=4 pogos=4
Error/Exception/Assert=0
```

최종 원본 `_raw/B16b/BL2-r1-route.txt/log`, `BL2-r1-route-save.json`, `BL2-r1-compile.log`, `BL2-r1-build.txt/log`. 관찰CSV `_raw/playtest/20261016-152039.csv`(최초), 최종CSV 경로는 실행 log와 _raw/playtest에 보존한다.
최종 상태 DONE(측정 수행 완료). B16b-BL2-r1 Quiet13/13. A04 구간만 BLOCKED, 검증 입력 보정1회 소진. 제품/설계 변경 없음.
