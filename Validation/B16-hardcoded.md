# B16 ID·좌표 특수 처리 점검

Base: 6860a5f. B09Authoring / V4Room / V4Launch / ProgressSave / GuidePresentation 검색.

| 위치 | 기존 처리 | 검토한 V5 처리(구현 원복) |
|---|---|---|
| B09Authoring.OutputPath/Shell | V4 출력·표시·발 원점 Player 프리팹 | V5 입력일 때 출력명·표시만 V5, 기존 Player 재사용 |
| B09Authoring.Mark | marks 글과 x/y로 사당·조각·CP·표지판 | 같은 계약. A02 감실 marks에 '조각' 글이 없어 아이템은 생성하지 않음(설계 본문과 데이터 불일치) |
| B09Authoring.Generate | A01 start → CP-A01 | V5도 start 유지. 개발 A03 fixture에는 Entry 좌표 CP-A03 별칭 추가(실제 CP 장치 아님) |
| B09Authoring.Generate | seal/cover/breakable/pogo/lift는 데이터 좌표 | V5도 동일, 저장 ID 접두어만 V5로 분리 |
| B09Authoring.Generate | Z01 엘리베이터, S02 비활성, C06 보스 | V5에 해당 방 없음, 실행되지 않음 |
| B09Authoring.CoveredFloorTrigger | y=0,h=1 + cover/seal 겹침이면 트리거 상단 .1u inset | V5 동일, 원본 영역·표시 보존 |
| V4Room.Arrive | S02 방문, .4초/1u 도착 문 재진입 억제 | 같은 코드 재사용, V5에 S02 없음 |
| ProgressSave.PendingScene | CP-A01/A03 등 기존 저장 형식 | 기존 형식 유지, V5 경로 선택만 변경 |
| GuidePresentation | 기존 V3 능력별 안내·V4 graph | 규칙 변경 금지에 따라 유지. V5 경로와 안내 목적지 불일치 가능, 플레이 인계에 명시 |

T1 컴파일 두 번째 실패로 생성기·실행 모드 변경 전부 원복. 위 표는 구현 완료 주장이 아니라 재개 시 참고할 검토 기록이다. 기본 V4 재생성 SHA 검사는 실행되지 못했다.
