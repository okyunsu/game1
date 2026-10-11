# B09-T5 — 데이터 → V4

상태 DONE. 21 Scene 생성, 16종 실제 Scene 컴포넌트 전수 카운트 일치(21×16 종류, 원본 spawn만 집계하고 Entry/CP 보조 spawn 제외). 다른 요소도 최종 audit에서 직접 대조, `_raw/B09/T5-elements-*.txt`에 종류별 수와 표지 매핑 기록.
외부 Asset 참조를 보존하고 내부 Unity fileID/저장 순서를 안정화한다. 초기 ID 방식이 외부 prefab 참조까지 바꾸는 오류는 r3에서 확인해 검사 중단, 정규식 외부 guid 제외 후 r4 수정. r3 입력 검증은 폐기, 원본 로그 보존.
21방 동일 파일 해시는 canonical1/canonical2에서 차이0 확인. 외부 참조 보존 수정 뒤 최종 재생성 비교는 T12 인계에 추가 기록.
spawn 좌표는 변경0. 검사기가 y를 발 높이로 사용하는 것을 확인해 V4 전용 Player Prefab collider offset .8u와 발 원점 Sprite를 생성했다. 기본 Prefab과 Tuning 수정0. MechanicsTest는 기존 중심 원점 Player를 사용한다.
지역별 월드 rect는 기록하되 각 방 Scene은 명시된 방 로컬 원점으로 생성한다. 카메라 경계=방 크기.
일방향 역표시는 destination에 같은 ID arrival이 없으면 비활성. Z01 elevator→S02는 Seal 해제 후 트리거, S02 역표시는 비활성.
미매핑 표지는 삭제 없이 SignPost로 보존하고 원문 목록에 sign-only로 기록. CP 포함 문자열도 Checkpoint로 매핑. S01 엔딩, C06 기존 Boss를 복사 연결.
데이터/room_check.py/기본·V2·V3 Scene 변경0. 승인된 1u 데이터 수정은 사용하지 않았다.

최종: r6 생성 결과를 다시 생성해 21 Scene SHA256 차이0 (`_raw/B09/T5-final-before.json`, `T5-final-diff.json`). Release r5 21방 적 Prefab/실제 생성 참조 PASS/Error0 (`T-scenes-r5.txt`). E3←/→는 기존 E3와 방향으로, BOSS는 데이터 위치의 기존 보스로 연결했다. CP는 표지 아래 가장 가까운 Ground 표면을 재개 지점으로 사용한다(연결 spawn 데이터는 그대로). 표지·획득물 높이는 발 원점 플레이어와 겹치도록 collider 중심만 파생한다. 비활성 일방향 역표시는 안내 그래프에서도 제외한다.

최종 실제 전체 요소 수 표: `_raw/B09/T5-6-audit-all-elements.txt`, FAIL COUNT=0.
