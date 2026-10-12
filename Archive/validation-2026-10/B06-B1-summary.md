# B06 B1 — 벽 점프와 WallJumpTest

REVIEW / 2026-10-10. 기술 구현·핵심 검사·지정 회귀 완료, 사용자 손맛 판단 대기.

표시된 ClimbableWall에만 벽 미끄러짐/점프. 기본 hasWallJump=false. 접지→벽→추가 점프 소비, 부착 시 보유 공중 능력 회복, 입력 잠금, 기존 점프 컷 공유, Pause/전환/리스폰의 기존 입력 초기화 연결. WallJumpPickup/GrantWallJump/버전1 저장 필드/HUD 획득 문구 추가. 미끄러짐 공격은 Motor의 벽 반대 Facing을 사용하도록 PlayerAttack 방향 선택 한 줄 수정(요구 동작에 필요한 최소 변경).

PlayerTuning.asset에는 신규5필드만 추가: slide2.5u/s, horizontal8u/s, velocity12u/s, inputLock0.15s, coyote0.08s. 기존 값 diff0. 테스트 씬은 안전 바닥·3/4/5u 높이12 굴뚝·건너기/대시/안전 포고·일반벽/게이트 대조·더블 점프 오브젝트. 저장 없음, 메뉴·BuildSettings 미추가. `-walljumptest`, Afterglow/Open Wall Jump Test.

실제 검사: 30/60/120fps 모두 4u 굴뚝 벽 점프만으로 오름, slide vy=-2.5, 일반벽/게이트 및 능력 미보유 slide0, 입력 잠금·중복 점프0·Pause/입력 clear PASS/Error0. 기본13/13·새 빌드 RouteAll·PogoTest PASS. 신규 필드 없는 기존 저장을 사용하는 기존 저장 회귀 PASS. 전체 경로와 P03는 이번 실행 PASS이며 기존 간헐 실패 해결로 단정하지 않는다.

첫 자동 입력 검사는 다음 원문으로 실패했다. 게임 수치를 변경하지 않고 jump 발동 확인까지 키를 유지하도록 도구1회 보정 후 통과.

`FAIL System.Exception: chimney timeout position=(22.37, 1.82) wall=False jumps=3`

핵심 수치 원문:

`PASS 4u chimney 12u climbed with wall jumps only y=13.97894`

`MEASURE chimney4 jumps=5`

증거: `Validation/_raw/B06/B1-wall-30-a1.txt`(실패), `B1-wall-30-a2.txt`, `B1-wall-60-a1.txt`, `B1-wall-120-a1.txt`, `REG-B06-B1-summary.txt` 및 각 회귀 원본. git diff --check PASS, 기본 Scene/Prefab/Packages/ProjectSettings diff0.

미실시: 실제 패드 손맛·3/5u 사람 비교·벽 점프 획득 후 디스크 종료/Continue의 별도 신규 능력 직접 검사는 후속 C1/F1 기술 확인과 인계에서 구분한다. 기존 회귀는 신규 wallJump=true 저장 검사를 대체하지 않는다.
