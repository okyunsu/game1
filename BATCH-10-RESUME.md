이번 실행은 Batch 10 재개만 수행한다. BATCH-10.md의 규칙·사전 결정·예산(남은 시간)을 그대로 따른다.

## 재개 전 추가 작업 (먼저)

1. Claude가 설계 데이터를 고쳤다: `Design/CHANGES_CLAUDE_B10.md` 참고. `Design/rooms_def.py`, `build_rooms.py`, `room_check.py`, `rooms_v2.json`이 바뀌었다. 먼저 `python Design/rooms_def.py` 후 `python Design/room_check.py Design/rooms_v2.json <임시>`로 21방 PASS 확인 → 이 변경을 커밋(`B10 resume apply Claude design fixes`).
2. 생성기(`Afterglow > Build V4 From Design`)로 V4 Scene 재생성. B10에서 고친 바닥 문 inset 규칙은 유지. 재생성 차이 표(바뀐 방: A01 A02 A05 A06 B01 B02 B03 C01 C02 Z01, 그 외 방 차이 0이어야 함).
3. **바뀐 10개 방의 경로는 이전 PASS도 무효**로 보고 T2를 다시 판정한다. 보류였던 10건(A01/dj, A02/wall, A05/main, A06/spikes, B01/lift, B03/climb, B05/climb, C01/lift, Z01/up_A01, S01/walk 결과 없음)을 먼저.
4. `@lift`는 "직전 발판 근처 레버 공격 → 탑승 → 도착 대기"로 해석.

## 그다음

TASKS의 "B10 진행" 줄 기준으로 T2 → T3 → T4 → T5 → T6 → T10 → T12 → T13 → Backlog → 반복 작업 순서로 계속한다. 종료 조건은 BATCH-10.md와 같다(PARTIAL 기록 후 종료 금지).
