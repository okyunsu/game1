# B06 D1 — V2 유도 가이드

REVIEW / 2026-10-10. 기술 PASS, 길찾기/문구 톤 판단 대기.

Resources/GuideObjectives: 현재 V2 목표4단계. Resources/GuideGraph: V2 빌드 Scene inventory의 RoomExit와 OptionalRoomEntry에서 생성, 능력 게이트 무시 BFS. 에디터 Afterglow/Rebuild V2 Guide Graph로 재생성. GuidePresentation: 위 중앙 한 줄 및 개발 전용 F1 방/단계/목표/다음 출구 방향. RoomExit.mainPath는 사용자 별도 승인된 최소 필드 추가. 필수 연결 양방향28개에만 새 GuideLantern 자식 프리팹(선택 분기 제외).

실제 Release·Release -no-guide·Development exe 3검사 PASS/Error0. 새 게임→Dash→Double Jump→보스 처치 문구4단계, 18방 fixture·필수 출구28/등불28, no-guide 등불/문구/F1 모두0. Release에서 F1 무반응, Development에서 F1 표시. 생성 그래프 A01→C06의 다음 출구 A02 PASS. 빌드 errors0/warnings0. V2 Scene diff는18개 모두 삭제0줄(기존 배치·연결 값 변경0).

첫 Release 검사는 C06에 없는 Entry spawn을 사용해 방 진입 실패를 놓쳤다. 실제 FromLeft로 도구1회 보정 및 진입 ID assertion 추가 후 전체 통과. 원문:

`FAIL System.Exception: required route directions=28, optional exits unmarked`

증거 `Validation/_raw/B06/D1-release-guide-a1.txt`, `D1-release-noguide-a1.txt`(실패), `D1-release-guide-a2.txt`, `D1-release-noguide-a2.txt`, `D1-development-guide-a1.txt`, `D1-authoring.txt` 및 빌드 로그.

미실시: 사람의 길찾기 시간·HUD 겹침/실제 글꼴 가독성 수락. V3 목표표/연결 재배선은 Batch07. Pause 목표 토글은 Task 예산에 따른 미착수 Backlog다.
