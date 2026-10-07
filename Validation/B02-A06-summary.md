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

## 통합 확인 보강

최종 exe B02-main-final에서 왼쪽 출구 위 선택 문(x2,y5.6) 지정 점프·왼쪽 입력 진입2회, 낙사·치명 피격 후 마지막 CP/최대HP 복귀와 E3 재입장 초기화 PASS/Error0. 원본 Validation/_raw/B02/A06-lifecycle-a2.txt/.log(같은 basename). 첫 문(x6)은 RouteAll R04를 가로채어 이동한 것으로 최초 실패를 보존한다. 사망·재입장 미검증 문구는 최초 Editor 왕복 시점 기록이며 이번 통합으로 해소.
남은 조건은 사람1~3분 도전 시간·감각 수락. PARTIAL 유지.

최종 적용: B02-delivery, B06 제외 상태. 최종 기본13/13 및 RouteAll R01~R17 PASS/Error0 [통합](B02-final-summary.md). 기본 게임17방 후보, 인간 시간/체감만 남아 PARTIAL.
