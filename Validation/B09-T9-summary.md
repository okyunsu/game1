# B09-T9 — 고립 보조 검사

상태 PARTIAL. 최소 능력의 입구별 다른 출구 도달/갇힘0 합격은 미증명이다.

Release/60fps에서21방 첫 route를 태그에 적힌 능력만 부여해 검사했다. 원문 `MEASURE mode=isolation PASS=17 FAIL=4`, Error0 (`_raw/B09/T-isolation-r8.txt`). 이 도구는 연결 ID별 도착이 아니라 경로 첫 노드 준비 위치에서 시작한다. 따라서17 PASS를 최소 진입 능력·다른 문 탈출 PASS로 승격하지 않는다. S02 단일 노드도 이동 증거가 아니다.

특히 C06/exit PASS는 disabled 출구 trigger 우회로 보스 없이 통과한 결과라 철회했다. T6에서 V4 enabled/Boss guard를 고치고 실제 물리 겹침 전환0 PASS. 이후 C06 보조 경로 결과는 유효하지 않으며 올바른 최소 상태 검사도 미실시.

원문 수동 확인 항목:
```
MANUAL route B02/low waypoints=2/5 pos=(60.00, 14.30) classification=automatic-input-limit; product/data fault not established
MANUAL route B03/climb waypoints=4/12 pos=(17.05, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route B04/low waypoints=1/2 pos=(17.67, 3.88) classification=automatic-input-limit; product/data fault not established
MANUAL route C05/up waypoints=1/2 pos=(19.03, 1.01) classification=automatic-input-limit; product/data fault not established
MEASURE mode=isolation PASS=17 FAIL=4
FAIL System.Exception: Incomplete actual input checks; see per-item classifications
```

제한된 경로 도구 보정·재실행 뒤에도 입구별 최소 상태를 증명할 검증 계획이 부족하다. 새 자동 계획 구축으로 확대하지 않고 미검증과 영향(고립 가능성을 배제 못함)을 인계한다. 데이터 이동0, 실제 갇힘으로 확정한 방0; 이것은 갇힘0 PASS가 아니다. 이전 r5 엔딩 도구 NullReference 원본은 보존하고 채택하지 않았다.
