# B06 — BLOCKED / 실행 범위 복원

시험: 대시 거리 조절과 포고 후 대시 회복을 활용한 왕복(더블 점프 미보유).
기존 Level/PogoTarget/E3, 36x14u, B03의 기존 오브젝트는 그대로이며 선택 문1개만 추가. Return은 B03 FromLeft. 발견 B06.
Editor 지정 입력 입구→발견→B03 PASS, Dash=true/DoubleJump=false, Error0.
MEASURE B06 entrance-discovery-return seconds=10.216
MEASURE return stop x=3.471 y=1.814 hp=3 tick=505
원본 Validation/_raw/B02/B06-traversal-a1.txt/.log.
미실시: 사망 체크포인트/적재입장·분기진입·최종RouteAll(통합에서 수행 예정), 인간1~3분/숙련자 도전감. 자동10.216초는 목표 시간 충족으로 인정하지 않음.

## 통합 확인 보강

최종 exe B02-main-final에서 왼쪽 출구 위 선택 문(x2,y5.6) 지정 점프·왼쪽 입력 진입2회, 낙사·치명 피격 후 마지막 CP/최대HP 복귀와 E3 재입장 초기화 PASS/Error0. 원본 Validation/_raw/B02/B06-lifecycle-a2.txt/.log(같은 basename). 첫 문(x6)은 RouteAll R04를 가로채어 이동한 것으로 최초 실패를 보존한다. 사망·재입장 미검증 문구는 최초 Editor 왕복 시점 기록이며 이번 통합으로 해소.
남은 조건은 사람1~3분 도전 시간·감각 수락. PARTIAL 유지.

## B06 분기 최종 위치

(x2,y5.6) 문은 R10 귀환을 가로채어 마지막 위치(x8,y5.9)로 이동. 실제 exe B02-main-r3에서 Dash=true/DJ=false로 문 아래에서 점프 입력 진입2회·낙사/피격 복귀·E3 초기화 PASS/Error0. 원본 Validation/_raw/B02/B06-lifecycle-r3.txt/.log. 문 위치와 기본 경로 전체 결과는 최종 통합 요약을 따른다.

## 최종 판정 — BLOCKED

세 위치(x6,y4.4 → x2,y5.6 → x8,y5.9)의 입구가 필수 경로 입력에 간섭. 특히 마지막 두 위치 모두 R10 귀환을 가로챔. 재시도 한도에 따라 B03의 추가 문을 제거하고 B06 Scene/.meta를 Validation/_raw/B02/blocked-B06.unity(.meta)에 이동해 원본 보존, 빌드 제외. 기존 B03 객체는 원상 복원. 왕복·사망 검사는 당시 후보 구현 이력이며 최종 적용 성공이 아님.
FAIL System.Exception: R10 fixed-tick replay wrong authored exit destination B06
원본 Validation/_raw/B02/RouteAll-final.txt, RouteAll-r3-retry2.txt/.log. 필요 판단: B06보류(추천) / B03에 선택 입구용 착지 공간 추가 허용 / 진입 방식 재설계 허용. 어느 선택도 이번 Batch에서 임의 확장하지 않음.
