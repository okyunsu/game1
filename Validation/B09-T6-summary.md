# B09-T6 — 전환 전수 검사

상태 BLOCKED: 데이터의 거리 조건을 충족하려면 사용자 결정이 필요하다. 코드·검사는 유지하며 독립 Task를 진행했다.

21방/63 directed exit 표지, destination spawn ID 유일성과 가로 도착 벽 방향 위반0. 설계 일방향 역표시는 비활성으로 분류했다. 실제 Scene 16종 요소 수 일치, 적 prefab 참조 누락0.

원문: `MEASURE scenes=21 directed exits=63 violations=16`
15개 낙하 도착은 출구 중점과 약2.2u, Z01→S02는2u로 승인 기준2.5u 미달. 데이터 오류로 통행이 막혔다는 실측은 없으므로 승인된 1u 이동 예외를 적용하지 않았다. 목록: `_raw/B09/T5-6-audit-r6.txt` FAIL LINK 원문16줄.

V4Room의 도착 억제(이동1u 또는 .4초)는 V4에만 연결했다. T7 실제 방향키2초 유지에서는 기록된 모든 hold의 추가 전환0. 요청한 별도3초 유지 검사는 미실시로 남기며 T6 전체 PASS를 주장하지 않는다.

후속 고립 보조 검사에서 Unity가 disabled RoomExit에도 trigger 메시지를 보내는 문제를 발견했다. V4ExitRule에서 enabled와 Boss 완료 flag를 직접 확인하도록 수정. Release 실제 물리 겹침으로 보스 전 상단 문/S02 비활성 역문이 전환0, Boss 완료 상태 허용 PASS/Error0 (`_raw/B09/T6-gates.txt`). 기본/V2/V3 동작은 바꾸지 않았다.

원문 실패·수치 보존:
```
FAIL LINK A01 -> Z01 A01-Z01 distance=2.200001
FAIL LINK A04 -> A05 A04-A05 distance=2.2
FAIL LINK A05 -> Z01 A05-Z01 distance=2.200001
FAIL LINK A06 -> A01 A01-A06 distance=2.2
FAIL LINK B01 -> A03 A03-B01 distance=2.200001
FAIL LINK B01 -> A03 B01-A03pit distance=2.200001
FAIL LINK B02 -> A04 B02-A04 distance=2.2
FAIL LINK B04 -> B02 B04-B02 distance=2.2
FAIL LINK B05 -> A02 A02-B05 distance=2.200001
FAIL LINK B06 -> A06 A06-B06pit distance=2.200001
FAIL LINK C01 -> B05 B05-C01 distance=2.200001
FAIL LINK C05 -> C03 C03-C05 distance=2.200001
FAIL LINK C04 -> C03 C03-C04 distance=2.200001
FAIL LINK C06 -> C04 C04-C06 distance=2.2
FAIL LINK S01 -> C06 C06-S01 distance=2.2
FAIL LINK Z01 -> S02 Z01-S02 distance=2
MEASURE scenes=21 directed exits=63 violations=16
```
