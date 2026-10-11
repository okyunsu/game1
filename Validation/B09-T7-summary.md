# B09-T7 — 전 연결 왕복

상태 PARTIAL. 입력은 Release/60fps에서 실행했다. 각 연결의 문 앞 준비 위치만 배치한 뒤 키 입력으로 진입했다. 도착 후 방향키2초, 중립1초, 역문까지 실제 입력으로 이동했다. 동일 검증 실패 경로를 반복하지 않고 결과를 보존한다.

원문: `MEASURE mode=connections PASS=77 FAIL=31`
108개 hold/return 판정 중77 PASS(71.3%). 기록된 hold 모두 extra=0. 다만 위/아래 방향키만 유지해 정지한 경우 이동3u를 만족하지 못했고, 일부 역문 접근은 자동 입력 한도로 미완주했다. 원문 FAIL/ONEWAY/방향별 수치는 `_raw/B09/T-connections-r5.txt`에 모두 남겼다.

전체 왕복 PASS를 주장하지 않는다. 일방향 역표시는 설계대로 제외. 최소 거리16개는 T6 결정 대기이며, 세로 문에서 방향키만으로3u 이동할 조건은 설계 판단 필요. 나머지 역문 미완주는 수동 확인 필요; 데이터 오류/제품 갇힘으로 확정하지 않았다.
Error/Exception/Assert=0. r3/r4 참조 오류로 중단한 실행은 폐기하고 T5 참조 수정 후 r5 결과만 채택했다.

원문 실패·수치 보존:
```
FAIL B09TraverseVerifier actual=exe
FAIL return A02 -> A01 A01-A02 classification=automatic-input-limit/manual
FAIL hold A01 -> A06 A01-A06 extra=0 move=0.003931999
FAIL return Z01 -> A01 A01-Z01 classification=automatic-input-limit/manual
FAIL hold A02 -> B05 A02-B05 extra=0 move=0.003931999
FAIL return A05 -> A03 A05-A03 classification=automatic-input-limit/manual
FAIL hold A03 -> B01 A03-B01 extra=0 move=0.003931999
FAIL return A03 -> A04 A03-A04 classification=automatic-input-limit/manual
FAIL connection A05 -> A03 A05-A03 entry pos=(2.37, 1.02) classification=automatic-input-limit/manual
FAIL return Z01 -> A05 A05-Z01 classification=automatic-input-limit/manual
FAIL return A01 -> A06 A01-A06 classification=automatic-input-limit/manual
FAIL return A02 -> A06 A02-A06 classification=automatic-input-limit/manual
FAIL hold B01 -> A03 A03-B01 extra=0 move=2.731005
FAIL return B03 -> B04 B03-B04low classification=automatic-input-limit/manual
FAIL connection B04 -> B02 B04-B02 entry pos=(6.00, 2.02) classification=automatic-input-limit/manual
FAIL return A02 -> B05 A02-B05 classification=automatic-input-limit/manual
FAIL hold B05 -> C01 B05-C01 extra=0 move=0.003931999
FAIL return A02 -> B06 A02-B06 classification=automatic-input-limit/manual
FAIL hold C01 -> B05 B05-C01 extra=0 move=1.031006
FAIL return C03 -> C02 C02-C03high classification=automatic-input-limit/manual
FAIL return C02 -> C03 C02-C03low classification=automatic-input-limit/manual
FAIL hold C03 -> C05 C03-C05 extra=0 move=0.006143808
FAIL hold C03 -> C04 C03-C04 extra=0 move=0.003931999
FAIL hold C04 -> C03 C03-C04 extra=0 move=2.067004
FAIL hold C04 -> C05 C05-C04 extra=0 move=2.459014
FAIL hold C04 -> C06 C04-C06 extra=0 move=0.003931999
FAIL return C03 -> C05 C03-C05 classification=automatic-input-limit/manual
FAIL return C04 -> C06 C04-C06 classification=automatic-input-limit/manual
FAIL hold C06 -> S01 C06-S01 extra=0 move=0.003931999
FAIL hold Z01 -> A01 A01-Z01 extra=0 move=0.006143808
FAIL hold Z01 -> A05 A05-Z01 extra=0 move=0.003931999
FAIL hold Z01 -> S02 Z01-S02 extra=0 move=0.006143808
MEASURE mode=connections PASS=77 FAIL=31
FAIL System.Exception: Incomplete actual input checks; see per-item classifications
```
