# B07 BL5 — PARTIAL
실제 숫자 -verify-fps30/120 인자로 각각 전체 필수 경로의 A02 두 구간만 승인 QA 전환으로 건너뛰었다. 두 실행 모두 그 외 actual input 경로20구간·능력획득·보스·S01·엔딩→타이틀 PASS/Error0. A02 포함 무중단 완주는 미충족이다. 60fps는 T24에 기록.
원본 BL5-confirmed30.txt, BL5-world120.txt. 초기 BL5-world30은 helper의 잘못된 fps 전달로 유효한30fps 근거가 아니며 보존했다.
Validation/_raw/B07/BL5-confirmed30.txt
PARTIAL authorized QA skip A02 main at 1; known failed route is not retried
PARTIAL authorized QA skip A02 revisit at 12; known failed route is not retried
MEASURE continuous 21 S01 main seconds=20.500277 ticks=918 stage=22 pos=(0.00, 0.00) HP=0 dash=False wall=False DJ=False
PASS world reaches summit ending
PASS full world ending save
Error/Exception/Assert=0
Validation/_raw/B07/BL5-world120.txt
PARTIAL authorized QA skip A02 main at 1; known failed route is not retried
PARTIAL authorized QA skip A02 revisit at 12; known failed route is not retried
MEASURE continuous 21 S01 main seconds=20.4019825 ticks=918 stage=22 pos=(0.00, 0.00) HP=0 dash=False wall=False DJ=False
PASS world reaches summit ending
PASS full world ending save
Error/Exception/Assert=0
