# B16b T2 — 수정본 기준 V5 Scene 6개

상태: T2 REVIEW(직접 플레이 판단 대기). 요소·보호 SHA 검사·수정본 Quiet REGRESSION 13/13 통과. 입력 커밋 `62cc8c1`.

수정 전 영구 V5 Scene은 없었다. 이전 임시 출력은 이미 삭제했고, 수정 전 측정은 `_raw/B16b/T4-before-input-update.md`, `T4-after-seal-model.md`, T1b/T1c 과거 로그로만 보존하며 현재 설계의 합격 근거에서 제외한다. 새 데이터로 임시6방·patrol·V4 재생성 SHA를 다시 검사한 뒤 Assets/Scenes/V5에 처음 저장했다.

입력 확인: A02 조각2개, A05-A03 도착 x3.5/dir1/door_x0.5, A04 50x12와 world rect[126,9,176,21], world 문 ID A02-A06. WORLD_V5의 겹침형 크기 기준 변경도 읽어 검사기에 반영했다. 에이전트의 Design 수정0.

모든 셀은 데이터 배열/marks와 실제 Scene 컴포넌트·오브젝트 수가 일치한 값이다.

| Room | solid | platform | enemies | pogo | hazard | doors | seal | breakable | pad | steam | lifts | crumble | shards | dash |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| A01 | 6 | 4 | 2 | 2 | 0 | 3 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| A02 | 12 | 5 | 2 | 1 | 0 | 5 | 0 | 0 | 0 | 0 | 0 | 0 | 2 | 0 |
| A03 | 7 | 8 | 4 | 5 | 0 | 5 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| A04 | 7 | 3 | 3 | 5 | 1 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 |
| A05 | 11 | 0 | 1 | 0 | 1 | 3 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 1 |
| A06 | 8 | 6 | 1 | 3 | 1 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 |

stub4개는 disabled RoomExit와 닫힌 충돌 영역으로 생성했다. 추가로 A02 일방 받는 쪽과 A05 낙하 역방향도 닫힌다. Scene/저장 ID는 V5로 분리하고, V4는 기존 경로를 재사용한다. V5 선택은 개발 빌드에서만 -v5로 활성화한다.

fixture는 기존 저장 필드를 사용한다. A03에는 Entry와 같은 위치의 CP-A03 Spawn 별칭을 추가했다(체크포인트 장치 추가 아님). 임의 다른 시작방에는 CP-A01 별칭을 둔다. 공식3런처는 A01/A03만 사용한다. 임의 시작방 fixture의 재실행 Continue는 기존 CP 매핑을 따른다.

기존 V3 안내 규칙은 바꾸지 않았다. 대시 이후 안내 목표가 층 A 밖의 닫힌 문을 가리킬 수 있다. 조정 위치는 Scene 원본의 지형/marks 산출 오브젝트와 Patrol left/right Transform이다. Play 중 변경은 기록한 뒤 Stop 후 Scene에 반영·저장하고 재빌드한다.

원본: `_raw/B16b/T2-updated-generation.log`, `T1c-updated-generation.txt`, `T2-scenes-updated.log`, `T2-counts.md`.

T3 실제 exe 검증에서 14문 전환·스폰·능력 차단·아이템 획득·저장/Continue가 통과했다. Scene 기술 검증은 완료했고 방 문법·손맛 판단을 기다린다.
