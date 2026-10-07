# C05 — PARTIAL

시험: 더블 점프 높이 확보 → 포고·공중 대시 연결 → 높은 전망 착지와 귀환.
기존 Level/PogoTarget/E3, 36x14u, C03에는 선택 문만 추가. Return C03 FromLeft, 발견C05.
첫 입력 도달실패; 출발 점프 x6으로 도구1회보정에도 마지막 착지실패.
MEASURE discovery stop x=34.635 y=0.259 hp=0 tick=286
마지막 전망 top6.5→4.5u로 새 방 배치수정: 발견PASS/입구귀환낙사.
MEASURE return stop x=8.365 y=0.051 hp=0 tick=433
FAIL System.Exception: return traversal no death
입구 착지 폭8→10u로 세번째 배치 접근, 같은 최종 지정 입력 왕복PASS/Error0.
MEASURE C05 entrance-discovery-return seconds=9.733
MEASURE return stop x=3.471 y=1.814 hp=4 tick=481
원본: Validation/_raw/B02/C05-traversal-a1/a2.txt, C05-layout-a2/a3-test.txt/.log.
미실시: 사망 체크포인트/적재입장·물리분기·최종RouteAll(통합에서 수행 예정), 사람1~3분·체감. 자동9.733초는 사람시간 목표 충족으로 인정하지 않음。
