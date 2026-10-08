# B05-I V2 빌드·Scene 매핑 — DONE

`B05Build.Run -b05-v2`가 V2 프로필. 저장된 기본 Scene·ProjectSettings·EditorBuildSettings는 수정하지 않고 build copy의 A지역 목적지 6개만 V2 Scene 경로로 매핑한다. 기존 방 18개와 V2 A 6개·SkinTest를 함께 넣어 A03→기존 B01 및 B01→A03_V2가 가능하다.

ProgressSave의 기존 PendingScene 선택식 결과에 `V2SceneMap.Resolve`를 한 번 적용했다. V2 Menu bootstrap이 등록한 A01~A06의 Scene 경로만 바뀌고 CP-A01/A03의 Spawn ID·저장 형식·기본 Scene 선택은 유지한다. RoomSession 변경0. 기본 빌드는 매핑 목록이 비어 기존 동작이다.

실제 V2 exe New Game→A01_V2 PASS, CP-A03 죽음/왕복/타이틀→Continue→A03_V2 PASS/Error0 (`A03-save-a1.txt`). 기존 B01→A03_V2 물리 문 왕복 PASS (`A03-life-a1.txt`). 각 후보 build Succeeded/errors0, 원문 `_raw/B05/*-build.txt`.

기본 회귀 **13/13 + 기존 RouteAll PASS** (`REG-B05base-summary.txt`와 각 원문은 `_raw/B05/base13/`). S1-02/04/07/08·Camera·T1/T2/T3/T4·S2·F2R·LOGR·Boss. 실제 수행 결과이며 V2 경로 결과와 구분한다. 이 이후 추가한 검증 도구는 명시 CLI 없으면 생성되지 않는다.

별도 저장은 QA 실행기의 `-save-path`로 분리한다. V2 exe를 실행기 없이 직접 열면 기존 기본 슬롯을 공유하므로 QA 실행기를 사용한다. 저장 로직·기본 파일명을 임의로 바꾸지 않았다.
