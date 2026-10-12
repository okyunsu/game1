# B06 B2 — 테스트 전용 프리셋

REVIEW / 2026-10-10. 기술 PASS, 프리셋 손맛 선택 대기.

WallJumpTestCourse에 Inspector 프리셋3개와 런타임 PlayerTuning 복사본. Editor/Development에서 숫자키1/2/3으로 전환, HUD에 현재 이름 표시. 원본 Asset 값을 수정하지 않는다.

실제 Unity60fps: 숫자키 전환3/3 및 4u·12u 굴뚝 벽 점프만 완등3/3 PASS/Error0. `MEASURE chimney4 jumps=5`, `MEASURE chimney4 jumps=5`, `MEASURE chimney4 jumps=4`. 값 원문:

`MEASURE preset=1 기본 values=2.5/8/12/0.15`

`MEASURE preset=2 끈적 values=1.5/7/12/0.2`

`MEASURE preset=3 빠름 values=3.5/9.5/12.5/0.12`

증거 `Validation/_raw/B06/B2-presets-60-a1.txt`. PlayerTuning.asset 작업 전후 diff0. 실제 패드·사람 감각 미실시. 기본 프리셋은 후보 비교용이며 기본 게임에 채택하지 않았다.
