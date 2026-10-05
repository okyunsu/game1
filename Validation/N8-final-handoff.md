# N8 최종 인계 — 2026-10-06

총 소요: 요청 원문 저장 이후 약 81.3분(00:28~01:49 KST; 초기 문서 검토 별도).
- 0부 DONE: 요청/기존 변경 보존, N7 실제120분 원본 검토 PASS. 메모리+0.574%, 오브젝트 종료29→33(+13.8%); 전체CSV23~61로 첨부29~49 정정.
- 1부 PARTIAL: 입력/ATK1 위 공격 기술 PASS·REVIEW. 실제exe30/60/120, ↑/W·패드·기존좌우·표시/판정 확인. ATK2 포고는 보호된 즉시 사망 연결부로 BLOCKED, ATK3/4 의존 미실시.
- 2부 BLOCKED: PogoTest/S5-hard 미생성. 포고 최대 타수 미측정으로 보스HP28 후보 유지(24/32 확정하지 않음).
- 3부 DONE: R01~R17 개별·연속입력 기록, 최종 새 빌드에서 순차60fps3/3·30·120·가상패드60fps 모두 새게임→보스→엔딩→타이틀 PASS·저장 플래그 확인·Error0. RouteAll60 회귀 추가.
- 4부 DONE: 기존 N7 A02 진단 exe 일반창, 4해상도×창/전체화면8유효PNG, 사망후약0.12s·분산>0·Error0.
새 Decisions Needed: N8-POGO-ORDER — KillZone 또는 RoomSession에 포고 우선 처리 연결부를 좁게 허용할지. 현재 규칙/판정 크기 변경으로 우회하지 않음.

## 실행과 사용자 확인
- 최종 실행 후보: `Builds/N7-hard-only/Afterglow-N7-hard-only.exe` 유지. 이 기존 바이너리에는 N8 위 공격이 없다.
- 위 공격 확인: Unity `Assets/Scenes/Menu.unity` Play 또는 `Builds/REG-route-N8-route-gate-final/Afterglow-REG-route-N8-route-gate-final.exe`를 검증 플래그 없이 실행. 후자는 검사 전용 진단 빌드이며 S5-hard/출시 빌드가 아니다.
- PogoTest 씬·Afterglow > Open Pogo Test 메뉴·pogoVelocity/pogoRefreshesAir 필드 미생성. 포고 손맛10분 검토는 연결부 해결 뒤 진행한다.
- 현재 수정 위치: `Assets/ScriptableObjects/CombatTuning.asset`의 Directional attack 폭/높이(u), `Assets/Input/PlayerControls.inputactions` Gameplay Move ↑/↓·W/S. Play 종료 후 원하는 값을 Asset에 다시 입력·저장. 기존 Scene/Prefab·Motor/Dash/InputReader·보호 파일·Packages/ProjectSettings 불변.
- 사용자 판단: N7 Hard 완주 난이도와 위 공격 손맛, LEVEL_PROPOSALS.md의6개 후보 선택. 감각/설계 REVIEW와 Sprint3 Gate는 자동 검사로 수락 처리하지 않았다.

## 검증·문제와 다음 작업
- 기본13/13: REG-N8-final-a2-summary.txt. 추가 현재소스 새빌드 RouteAll60: REG-N8-route-gate-final-summary.txt1/1. 서로 실행된14개 검사이며 한 번의14개 명령을 실행했다고 기록하지 않는다.
- 최종 경로: N8-sequential-all-60s1/s2/s3,30,120,pad60.txt. 최종 입력N8-route-final-library.json. all은 텔레포트·능력 지급·HP/보스 상태 쓰기 없이 실제 종료 상태를 연결한다. 새 회귀는 현재 소스를 빌드해 검사하므로 오래된 exe로 새 튜닝을 판정하지 않는다.
- 자동 활동시간 최단은 이번 최종6회 중119.028361초(약1.98분), 사람 시간 아님·전역 최단 경로 주장 아님. 인간 목표60~120분과 직접 비교해 수락할 수 없다. 상세 N8-route-summary.txt / N8-route-timing.json / 연결 Playtest CSV.
- 이전 실패 보존: 라이선스 샌드박스 미진입, F2R 비공개API 검사오류, 경로 전환/입력 길이/중복 증거 이름, Editor R16 재생 한계, 중단된 중복 FPS 검사. 보정 후 최종 성공과 구분했다. 사망 캡처의 death-only IgnoreFocus를 사용했으므로 실제 focus 상실/복귀는 미검증.
- 단계 로컬 커밋, push 없음. N7-hard-only/S4-pre3 보존. 요청된 독립 작업 종료, 자동 검사 프로세스 없음. 다음 작업은 N8-POGO-ORDER 결정 뒤 포고 체인 재개이며 승인 밖 레벨 변경/다음 Sprint 확대는 하지 않는다.

상세: N8-final-checks.txt, N8-ATK1.txt, N8-ATK2.txt, N8-protected-files.txt, N8-death-capture.txt, N8-soak-review.txt.
