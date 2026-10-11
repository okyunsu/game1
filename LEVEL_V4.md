# Batch09 V4 데이터 월드

원본: `Design/rooms_v2.json`, 월드 배치 참조 `Design/world_v7.json`. 21방, 로컬 원점=방 왼쪽 아래,1단위=1u, 사각형=[x,y,w,h]. 기본/V2/V3 Scene과 기존 PlayerTuning은 보존했다. 데이터 이동·검사기 상수 수정0.

Unity 메뉴 `Afterglow > Build V4 From Design`으로 `Assets/Scenes/V4/<ID>_V4.unity`를 생성한다. 원본 변경 후 재생성하며 Scene을 직접 고쳐 원본으로 쓰지 않는다. 최종 재생성21개 SHA256 차이0. 내부 fileID/저장 순서를 고정하고 외부 guid 참조는 유지한다. 방별 카메라 경계는 size와 같다. `Assets/Prefabs/V4/Player.prefab`은 좌표 y가 발 높이가 되도록 collider offset과 Sprite pivot만 파생했다.

`Afterglow > Open Mechanics Test`는 `Assets/Scenes/Test/MechanicsTest.unity`를 연다. 실행 빌드는 `Builds/B09-delivery/`, 실행기는 같은 폴더 또는 `Launchers/B09/`에 있다.

| 요소 | 생성 결과 |
|---|---|
| solid/platform/climb | 단단한 collider/기존 방식/ClimbableWall |
| seal/cover/breakable | Slam 전용 SealFloor/기존 공격 파괴 |
| pad/pogo/hazard | SlamPad/등불 PogoTarget·가시 DamageBlock+PogoTarget/피해 구역 |
| steam/lifts/crumble | 주기 상승 기둥/상단 초기 레버 승강기/접촉 후 낙하·복귀 발판 |
| E4/E3←→/BOSS | HP2 제자리 벌레/기존 E3+방향/기존 보스+V4 Slam 보상 |
| marks | 사당 능력·CP·SignPost·DiscoveryReward·엔딩·S02 표지 |
| doors/spawns | 연결 ID 대응, 낙하 좌표 그대로, 일방향 역표시 비활성 |

CP 표지 아래 Ground 표면을 저장 도착 지점으로 파생한다. 연결 spawn 원본 좌표는 바꾸지 않았다. 미매핑 문구는 SignPost로 보존하며 목록은 `Validation/_raw/B09/T5-elements-*.txt`의 sign-only 행이다. S02에는 표지와 도착 저장 flag만 둔다.

Slam: 공중 아래+C(대시). 지상·대시 중 불가, 수평0/하강24, 착지 범위1.6/피해1/회복.15. 일반 아래 공격은 아래+X. 개발 F8은 Slam 부여, M은21방 지도, 시험 코스 F는 후보1→2→기본. 엔딩 제출은 능력을 유지해 A01에서 재개하도록 연결했다.

조정 위치: `Assets/ScriptableObjects/MechanicsTuning.asset`의 단위·Tooltip. 포고는 기존 `CombatTuning.asset`의 pogoVelocity. 장치 위치·영역은 데이터 원본을 변경해 재생성한다. Play 중 Asset 값을 바꿨다면 종료 후 해당 Asset Inspector에서 확인·저장한다. F 후보는 복사본이므로 종료 시 사라지며, 원본 Asset에 수동으로 옮겨야 반영된다. 기본 후보 채택은 사용자 판단이다.

검증: 21방 기동·장치 단위·4저장·3실행기 PASS. 자연 완주는 A03에서 미완주, 전 연결77/108 판정 PASS, 최소 능력 고립0은 미증명. 거리2.5u 기준16개 위반은 원본 이동 권한 결정 대기. 상세 결과와 원문 링크는 `TASKS.md`와 `Validation/B09-T*-summary.md`. 검증 실패를 수락 완료로 처리하지 않았다.
