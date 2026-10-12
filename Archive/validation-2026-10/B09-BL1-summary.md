# B09-BL1 — 사람 CSV 요약

상태 DONE. Tools/B09PlaytestSummary.py는 원본 CSV를 보존하며 방별 room_exit 활성시간 합계와 3초 창의 연속6전환(3왕복)을 요약한다.
실제 20261011-201602.csv의 A02↔A03 전환 루프 의심 검출.
원문: `_raw/B09/BL1-human-summary.md`.
마지막 방에서 기록 없이 종료한 체류 시간은 합계에 포함하지 않으며 요약에 명시한다.
실행: python Tools/B09PlaytestSummary.py Playtest --output <새 Markdown 경로>.
