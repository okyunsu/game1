# B02 마무리 빌드·회귀 — PARTIAL (B2 의존 BLOCKED)

최종 인계: Builds/B02-delivery/Afterglow-B02-delivery.exe. Windows x64 / Unity6000.3.24f1. 17플레이방(A06/C05 포함, B06 제외)+Menu/Ending/PogoTest 총20씬. Build Succeeded errors=0 warnings=0. 실제 exe 기존 입력 라이브러리 R01~R17 PASS/Error0, 보스→Ending→Menu까지 실제 가상 입력. 기본13 기존 검사1회13/13 PASS. 전체 경로 입력 라이브러리 수정 없음.

신규 문이 필수 경로를 가로채는 회귀를 확인하여 추가 문만 수정. A06/C05는 x2,y5.6에 유지. B06 문3개 위치 실패로 분기·Scene을 되돌림; B03은 START_COMMIT과 byte/diff 동일. B06 원본 Scene/.meta는 Validation/_raw/B02/blocked-B06.unity(.meta)에 보존.
보호 Player core/GameState/KillZone/기본 모든 tuning·BossTuning/기존Prefab/나머지Scene/Packages/ProjectSettings/PRD/GAME_DESIGN 변경 없음. AGENTS는 승인 두 줄만.

실제 exe A06/C05 지정 분기 진입2회 각각, 낙사/치명피격→CP-A03/CP-C04 최대HP·timeScale1, E3 재입장 초기화 PASS/Error0. B06은 당시 후보 진입/사망 CP-B01 PASS지만 경로 회귀 때문에 최종 적용하지 않음.
Editor 왕복 발견: A06 10.648s / C05 9.733s, 능력 제한 PASS. 사람1~3분·체감은 미확인, 방PARTIAL 유지. 후보B2 미구현/빌드없음(걷기/위치/도약높이 결정 대기). 따라서 요청된 기본/후보 두 빌드 중 기본만 생성, 마무리 Task PARTIAL.

## 실패와 핵심 수치 원문

FAIL System.Exception: R04 fixed-tick replay wrong authored exit destination A06
FAIL System.Exception: R10 fixed-tick replay wrong authored exit destination B06
FAIL System.Exception: R04 fixed-tick replay recording exhausted room=A05 pos=(9.83, 3.21)
FAIL System.Exception: R01 fixed-tick replay recording exhausted room=A01 pos=(34.15, 3.87)
MEASURE continuous start deviation R03=0.029u; no placement performed

앞의 두 줄은 실제 분기 회귀(수정/복원 완료). 뒤 두 줄은 같은 입력 소진 재현으로 원인 미확정, 실패를 삭제/성공으로 변경하지 않음. 마지막 인계 빌드의 전체1회 PASS와 구분한다. 첫 빌드의 사용하지 않는 검사 필드 경고1개는 제거, 최종0개.

원본: Validation/_raw/B02/B02-delivery-build.txt/.log, RouteAll-delivery.txt/.log, REG-B02-final-summary.txt 및13개 원본, A06/C05-lifecycle-a2.txt/.log. 이전 B02-main/B02-main-final/B02-main-r3 빌드는 회귀 조사 이력이며 인계용 아님. 실패원본 RouteAll.txt, RouteAll-final.txt, RouteAll-r3/retry/retry2.txt/.log 보존.
미실시: 보스확장30/60/120fps·후보엔딩/B2 exe, 물리게임패드·자연플레이시간·체감. Sprint Gate 수락 없음.
