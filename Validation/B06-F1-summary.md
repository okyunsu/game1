# B06 F1 — Development 전달 빌드

DONE / 2026-10-10. `Builds/B06-delivery/Afterglow.exe`, 40 Scene(기본18+V2 18+Menu/Ending+PogoTest/WallJumpTest), Development, errors0/warnings0. 기존 BuildSettings/제품명/저장 위치 유지. 빌드 스크립트가 실행기3개도 생성한다.

실제 실행기3개 직접 기동 PASS/Error0. Guide/NoGuide 새 게임→A01_V2, 목표/등불 모드 일치. WallJumpTest 능력2종 보유·저장 없음·부서지는 벽2개·적/낙사 없음. 가이드와 NoGuide는 서로 다른 전용 QA 파일을 사용한다. 실행기 자동 생성 전후 SHA256 동일.

전달 전 테스트 굴뚝의 왼쪽 하단에 2u 입구를 열었다(벽 하단3u, 지면1u). 바닥에서 직접 들어가게 하는 후보 Scene 배치이며 정상13u/등반12u는 유지. 최종 exe 재검사: 벽 점프 30/60/120fps 전부 PASS, 프리셋3종60fps 전부 PASS/Error0. 이 재검사는 테스트 배치 변경에 연결된 범위만 수행했다.

최종 보호 검사 원문:

`MEASURE protected violations=0`

`MEASURE existing validation changes=0`

`MEASURE Packages/ProjectSettings changes=0`

`MEASURE V2 files with deleted lines=0`

`MEASURE PlayerTuning removed prior lines=0`

증거 `Validation/_raw/B06/B06-delivery-build.txt`, `F1-launch-{guide,noguide,wall}-a1.txt`, `F1-wall-{30,60,120}-a1.txt`, `F1-presets-60-a1.txt`, `F1-protected-diff.txt`, 실행기 hash 및 raw 로그.

미실시: 사람 손맛/패드 실물·HUD/표지판/두 벽 시각 수락, 자연 V2 전체 재완주. 지정 기본13/RouteAll/Pogo 회귀는 B1에서 PASS, 저장/기믹은 C1, 가이드 Release/Development/no-guide는 D1, 표지판은 E1에서 직접 확인했다. 이전 간헐 입력 이슈 해결이나 Sprint Gate 통과를 뜻하지 않는다.

Task8개 예산 소진. 엔딩 후 CP-C01·Pause 목표 토글·굴뚝 측정 HUD Backlog 및 V3 재배선/레벨/엔딩은 미착수. push 없음.
