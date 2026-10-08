# B05-A06 — REVIEW

기술 구현·해당 검증 완료, 방 크기·밀도·감각·사람 시간은 사용자 수락 대기. 새 Scene Assets/Scenes/V2/A06_V2.unity, 원본 Scene 수정0.

폭 60u, 카메라 경계 방 폭×18u. 3~5비트, 필수 넓은 착지·안전 바닥, 선택 1개. 선택 top3.2→4.8→6.8u, 착지 폭2→1.3→1.3u. 높이·입구·발견 위치와 이유는 LEVEL_V2.md. 이동·전투 tuning 변경0, 기존 연결·출입구·Spawn ID 유지.

실제 exe 검증:
- 필수 입구→출구 고정 입력 재생 PASS/Error0: Validation/_raw/B05/A06-main-final-replay.txt
- 선택 끝 발견 접촉·착지 고정 입력 재생 PASS/Error0: Validation/_raw/B05/A06-optional-final-replay.txt
- KillZone 낙사 / 원본 Health API 치명 피격→CP·HP 복귀, 물리 문 양방향 왕복 PASS/Error0: Validation/_raw/B05/A06-life-a1.txt
- 적이 있는 A04는 재입장 새 EnemyReset 인스턴스 PASS. 적이 없는 방은 해당 없음.
- A03의 G-D 미보유 차단은 gate-final.txt, Dash 보유 통과는 continuous-A-delivery-replay.txt. 다른 방에는 능력 게이트 없음.

V2 입력 원본은 raw에 보존하고 재사용용 소스는 Validation/Routes/V2/V2-A06-main.json 및 V2-A06-optional.json. 기존 R01~R17 입력 수정0. 필수/선택 검사는 독립 Scene 시작 fixture를 사용하며 실제 구간 이동은 입력으로 수행. A 전체 연속 8구간은 시작 이후 배치·능력 지급 없이 별도 PASS.

미실시: 사용자 자연 플레이·체감 수락, CP→주요 도전 30~90초 및 예상 방 활동시간 사람 측정, 물리 게임패드. 자동 입력 시간을 사람 시간으로 기록하지 않음.

실패 이력 / 원문 핵심 줄(최종 해결 또는 새 입력 보정, raw 삭제0):

