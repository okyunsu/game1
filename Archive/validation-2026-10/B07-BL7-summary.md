# B07 BL7 — DONE
GuidePresentation의 실행 인자·GUIStyle·Stage 캐시는 T23 커밋에서 완료했다. SignPost GUIStyle 캐시, WallJumpPickup의 HUD 없는 획득 문구, WallJumpTestMetrics 이전/현재 높이 교차 보간을 추가했다. 픽업 안내 3초는 해당 컴포넌트 Inspector 한 곳에서 조절한다.
실제 기본13/13 PASS: _raw/B07/Regression/REG-B07BL7-summary.txt. 실제 30fps 실행 인자를 확인한 BL7-confirmed30 PASS/Error0.
MEASURE course=chimney4 preset=0 seconds=3.33999881660998 jumps=5
PASS HUD/log elapsed difference=0.0420881244430018
MEASURE HUD seconds=3.29791069216698 display=4u chimney 3.30s / 5 wall jumps
초기 잘못된 검증기 인자/숫자 fps 전달 결과는 원본에 보존하고 30fps 통과 근거로 쓰지 않는다. 전체 회귀는 T25에 따로 기록한다.

