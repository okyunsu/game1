# B06b T3 — 등반 측정 HUD

DONE. 테스트 Scene 원본에 각3/4/5u 굴뚝·한쪽12u Zone 지정. 바닥에서 상승 출발→정상 시간·벽 점프 횟수, 최근3회 오른쪽 아래 표시/Logs/walljump-runs.txt 기록. 구간 이탈·바닥 추락 시 시도 초기화.

Unity60fps 자동4u 등반 PASS/Error0. 표시 및 로그 동일, 실제 등반 관찰 시간 차이0.0399999866s <=0.05, 점프5회 일치.
`MEASURE course=chimney4 preset=0 seconds=3.09999890164399 jumps=5`
`MEASURE HUD seconds=3.05999891500488 display=4u chimney 3.06s / 5 wall jumps`
[원본](./_raw/B06b/T3-metrics-a1.txt). 미실시: 사람 HUD 가독성.
