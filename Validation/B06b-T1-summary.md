# B06b T1 — 짧은 킥·상승 부착

REVIEW. 기존 값 유지, Short8u/s·0.15s / rising false 신규 필드만 Asset에 추가. 벽 쪽 입력0.5 이상은 Short, 중립·반대는 Long. 상승 속도 유지, 하강 시에만 slide limit. 사용자 후보 수락 대기.

Unity Editor 기존 B06 벽 검사30/60/120fps 모두 PASS/Error0. 새 검사30/60/120fps PASS/Error0: 한쪽12u 완등, short3.5/0.06 최대 벽 이격0.2989807u, Long 거리 기준과 차이<=0.05u, rising false 부착0/true 발생, 홀드당 벽 점프1회.

원문:
`MEASURE single12 fps=120 rising=False jumps=5 distance=0.2989807 summit=13.93286`
`MEASURE single12 fps=120 rising=True jumps=6 distance=0.2989807 summit=14.01886`
`MEASURE long kick distance=1.560028 rising=True`

지정 Run-Regression -Tag B06b-T1 1회: 기본13/13 PASS. RouteAll R01~R10 PASS 후 기존 B04-REPLAY-EDGE와 같은 R11 시작편차0.110u/입력 소진, Pogo P01/P02 PASS 후 기존 P03 간헐 경계 FAIL. 새 회귀로 확정하지 않았으며 전체 경로 PASS로 기록하지 않는다.
`FAIL System.Exception: R11 fixed-tick replay recording exhausted room=B01 pos=(33.31, 5.41)`
`FAIL System.Exception: P03 input goal pogos=1`

첫60fps 짧은 킥 도구: `FAIL System.Exception: single wall climb within budget`. 해제 입력이 물리 틱 전에 재눌림 → .04s 물리 대기1회 보정; 게임 수치 변경 없음.

[원본](./_raw/B06b/T1-short-120-a2.txt), [기본30](./_raw/B06b/T1-base-30.txt), 기본60/120 및 short30/60-a2 동일 폴더. [회귀](./_raw/B06b/REG-B06b-T1-summary.txt). 미실시: 실제 패드·사람 손맛. 기존 Validation 보존.
