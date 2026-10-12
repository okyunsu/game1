# Batch 10 생성기 수정 내역

데이터 원본 이동·폭 변경은 아직 0건이다. `rooms_def.py`, `room_check.py`, 기본/V2/V3 Scene, PlayerTuning은 변경하지 않았다.

| 방 | 요소 | 전 | 후 | 이유 | 근거 |
|---|---|---|---|---|---|
| B02 | door:B02-A04 / cover[0] | 문 BoxCollider size.y=1, offset.y=0; top=1 | size.y=.9, offset.y=-.05; top=.9 | (c) 덮개 top=1에 겹친 트리거가 착지 접촉 여유에서 발동 | `_raw/B10/T3-B02-full-grid-r26.txt`: 정상60fps 20분할, 4098/4098 후보 실패; closest=.01548716, (18.00,1.01) |
| A01 | door:A01-Z01 / seal[0] | 동일 top=1 | 동일 top=.9 | 같은 생성기 배치 규칙의 봉인 바닥도 표면 아래에서만 문 발동 | `_raw/B10/T3-A01-slam-r28.txt`: 수정 후 전체 저장 입력2회 PASS; 원래 r26 탐색 원본 보존 |
| A05 | door:A05-Z01 / seal[0] | 동일 top=1 | 동일 top=.9 | 같은 생성기 배치 규칙; 원래 r27 main은 봉인 착지 전 Z01 전환 | `_raw/B10/T2-r27b-g1-A05-main.live.txt`; 수정 후 경로 연결 재검증 진행 |

문 시각 Rect와 JSON, 연결·gate·도착 좌표는 그대로다. `B09Authoring.CoveredFloorTrigger`가 덮개/봉인 upper face와 바닥 문의 upper face가 일치할 때만 .1u 아래로 inset한다. 실제 Scene 차이는 A01/A05/B02의 Collider offset/size 각2줄뿐이다. `T3-room-check-r28.txt`: 기존 검사기 21방 PASS, 상수 수정0.

원래 r26 travel=84.5는 방 전환 후 위치를 섞은 측정이어서 방 안 최대 이동거리 근거로 쓰지 않는다. r28 이후 측정은 현재 방 좌표만 집계한다. 이 보정은 검사 도구에만 적용했다.
