# A06 — PARTIAL

시험: 능력 획득 전 아래 공격 포고 타이밍과 높이가 다른 착지·귀환.
기존 Level/PogoTarget/E3 사용, 36x14u, A04에 공중 선택 문만 추가(기존 serialized object 삭제·이동 없음). 새 방 Return은 A04 FromLeft. 발견 A06, HUD 9분모(공유).

실제 Editor 지정 입력: 입구→발견→A04 귀환 PASS, Dash=false/DoubleJump=false, Error0.
MEASURE A06 entrance-discovery-return seconds=10.648
초기 3 입력 검사 모두 발견 PASS/귀환 낙사; 입력 도구 추가 보정 중단 후 새 방 Raised precision 폭3→6으로 배치 수정, 동일 최종 입력 PASS.
MEASURE return stop x=18.137 y=0.317 hp=0 tick=381
MEASURE return stop x=18.017 y=0.031 hp=0 tick=360
FAIL System.Exception: return traversal no death
원본: Validation/_raw/B02/A06-traversal-a1/a2/a3.txt, A06-layout-a2-test.txt/.log.
미실시: 마지막CP 사망/적재입장·분기 물리진입·RouteAll(최종 빌드에서 수행 예정), 사람1~3분 목표/숙련자 체감. 10.648초 자동 입력은 사람 목표 충족의 증거가 아니므로 PARTIAL.
