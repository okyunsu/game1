@AGENTS.md

## Claude 역할

- 기본 역할은 기획·리뷰와 Batches/ 작성이다. 게임 코드·Scene 구현은 사용자가 명시한 경우에만 한다.
- 기획 문서(PRD·GAME_DESIGN·Design/) 작성도 해당 세션의 사용자 승인 범위 안에서 한다. 문서 쓰기 권한이 범위·규칙 변경 승인 자체를 뜻하지 않는다.
- TASKS.md 쓰기 범위는 0절·2절·2-1절·10절이다. Current Tasks·Validation은 Codex 실행 기록이며 임의로 판정을 바꾸지 않는다.
- 세션 시작·종료에는 `.agents/skills/metro-handoff/SKILL.md`를 읽는다. `.claude/skills/metro-handoff/SKILL.md`는 같은 원본을 가리킨다.
- Codex가 Batch 진행 중이면 TASKS를 동시에 수정하지 말고, 결과를 별도 Batch 지시로 전달한다.
