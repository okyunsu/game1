# B09-T6 — 전환 전수 검사

상태 BLOCKED: 데이터의 거리 조건을 충족하려면 사용자 결정이 필요하다. 코드·검사는 유지하며 독립 Task를 진행했다.

21방/63 directed exit 표지, destination spawn ID 유일성과 가로 도착 벽 방향 위반0. 설계 일방향 역표시는 비활성으로 분류했다. 실제 Scene 8종 요소 수 일치, 적 prefab 참조 누락0.

원문: `MEASURE scenes=21 directed exits=63 violations=16`
15개 낙하 도착은 출구 중점과 약2.2u, Z01→S02는2u로 승인 기준2.5u 미달. 데이터 오류로 통행이 막혔다는 실측은 없으므로 승인된 1u 이동 예외를 적용하지 않았다. 목록: `_raw/B09/T5-6-audit-r6.txt` FAIL LINK 원문16줄.

V4Room의 도착 억제(이동1u 또는 .4초)는 V4에만 연결했다. T7 실제 방향키2초 유지에서는 기록된 모든 hold의 추가 전환0. 요청한 별도3초 유지 검사는 미실시로 남기며 T6 전체 PASS를 주장하지 않는다.
