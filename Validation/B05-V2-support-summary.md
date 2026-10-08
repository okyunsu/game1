# B05 V2 확장 지원 — DONE

B/C 후보 작성·입력 기록/재생을 기존 검증 틀에서 확장했다. B05Build는 존재하는 V2 Scene ID만 별도 프로필에 넣고, 빌드 사본 Menu의 V2SceneMap과 RoomExit/OptionalRoomEntry 목적지만 등록한다. 현재 A01~C06 18개. 기존 BuildSettings/ProjectSettings/RoomSession 변경0. 저장 변경은 기존 B05-I의 PendingScene 매핑 추가 1곳뿐이다.

실제 Unity 최종 V2 빌드 `Validation/_raw/B05/V2-build.txt`: `Succeeded errors=0`. 기본13/13+RouteAll은 `_raw/B05/base13/`에서 실제 PASS, V2 제외 기본 프로필도 유지한다. 새로운 도구는 명시된 -verify-b05-* 플래그로만 켜진다. 기존 R01~R17 입력 변경0.

프레임 관찰에서 실제 사망·잘못된 방을 확인하도록 보강했다. 이전 FixedUpdate 관찰은 TimeScale0 사망 구간을 놓쳐 B06/C06의 A01 복귀 뒤 계속 입력하는 실패를 만들었고, 이를 PASS로 인정하지 않았다. 수정 후 실제 B06/C06 발견 기록은 올바른 해당 방에서 PASS, World a5는 보스의 실제 사망을 FAIL로 잡았다. 도구 보강을 게임 규칙 변경으로 취급하지 않는다.

컴파일 도구 초기 오류 원문(해소):
```
error CS1061: 'PlayerDash' does not contain a definition for 'CanAirDash'
```
실제 속성 AirUsed로 1회 보정. 후보 스킨·지형 작성 스크립트는 원본 Scene을 새 경로로 저장한 뒤에만 편집한다. 감각·사람 시간·Gate 수락은 별도 REVIEW이며 이 지원 Task의 컴파일 결과와 구분한다.
