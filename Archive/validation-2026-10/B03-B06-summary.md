# B03-B06 — REVIEW (Batch03 진입/필수경로 기술 검증)

이 방은 Dash·포고로 두 연속 간격을 건너고 같은 입구로 돌아오는 선택 도전이다. Batch02 raw B06 내부를 그대로 복원; 발견 분모9 유지, Double Jump/진행 보상/게이트 우회 없음. B03 기존 오브젝트 이동·삭제0, 새 청록색 문만 (7,2)에 추가. 문 위 `↑ 선택 대시 도전` 안내. 겹친 상태에서 위 방향을 눌러 진입하며 평상시 좌우·점프·대시에 반응하지 않는다.

접근1 첫 Editor 검사는 B06이 보호된 Editor Build Settings에 미등록이라 진입 실패(`B06-entry.txt`). ProjectSettings 변경 없이 exe에서 확인. 첫 구현은 disabled RoomExit 콜백도 실행되어 R09→B06 간섭(`RouteAll-main-a2.txt`); 새 입구의 RoomExit만 제거하고 OptionalRoomEntry가 목적지/입력 처리를 맡게 수정. 기존 출구는 그대로. 재설계 후 RouteAll 최종 R01~R17 전부 PASS/텔레포트0/Error0 (`RouteAll-final.txt`). 런처 첫 인자 오류로 다른 자연경로 도구 실행 실패한 `RouteAll-main.txt`도 별도 보존.

최종 실제 기본exe (`B06-final-entry.txt`):
```
MEASURE discovery stop x=32.293 y=3.003 hp=3 tick=241
MEASURE return stop x=3.500 y=1.813 hp=3 tick=505
MEASURE B06 entrance-discovery-return seconds=10.217
```
지정 입력 Up 진입→발견→B03 복귀/Dash true·Double Jump false/Error0 PASS. `B06-final-lifecycle.txt`: 실제 입력 입구3회, 낙사·치명피격 fixture 각각 CP-B01/최대HP 복귀, 이전 적 손상 후 재입장 E3 초기화, 시계1 PASS/Error0. placement는 낙사 검사 setup만이며 왕복/입구에는 사용 안 함.

Scene 원본 B03의 새 OptionalRoomEntry 문 위치·안내 문자열, B06 내부 블록/PogoTarget/E3 Reset을 Edit 모드에서 조정한다. Play 변경은 Stop 후 Scene에 반영·저장. 사람1~3분 시간/체감은 미확인; 10.217s 자동 이동은 숙련 입력 참고값이다. 사용자 B06 5분 확인 대기. 기존 A06/C05 PARTIAL 때문에 PRD/GAME_DESIGN Target18 문구는 미변경.
