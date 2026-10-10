# B07 T24 — PARTIAL
A02 최초 계단과 재방문 굴뚝 두 구간을 승인된 QA 전환으로 건너뛰었다. 능력 부여/그 외 위치 이동 없이 실제 획득과 입력으로 나머지22단계 경로의20구간→보스→S01→엔딩→타이틀을 검사했다. T24-rest-final60 PASS/Error0. 이는 끊김 없는 전체 완주 합격이 아니다.
PARTIAL authorized QA skip A02 main at 1; known failed route is not retried
PARTIAL authorized QA skip A02 revisit at 12; known failed route is not retried
MEASURE continuous 21 S01 main seconds=20.9842674 ticks=945 stage=22 pos=(0.00, 0.00) HP=0 dash=False wall=False DJ=False
PASS world reaches summit ending
PASS full world ending save
Error/Exception/Assert=0
CP-A03/B01/C01/C04 저장·Continue/legacy는 T24-checkpoint-continue.txt PASS. 엔딩 후 HP0는 제거된 플레이어에 대한 측정 생략 표시이며 사망이 아니다.
검증기 초기 두 실행은 엔딩에서 제거되는 Player/Health 접근 NullReferenceException으로 FAIL; 원본 보존. 종료 상태에서 접근을 생략한 최종 실행만 통과 근거다. 게임 엔딩 자체는 기존 방 고정 입력도 PASS.
