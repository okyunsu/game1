# B07 R-A04 — REVIEW
마지막 순서의 승인 접근1개: B02/C02 성공 패턴의 11u 도움닫기를 A04 비밀에 적용(x60→56). 기존 필수 지형/비밀 지형 변경0. 접근에서 이전 점프를 놓도록 WalkUnder를 사용한다.
초기 author 도구가 구조체 복사본만 수정하여 실제 Scene에 적용되지 않은 두 실행은 FAIL, 원본 보존. 배열 원소에 기록한 마지막 보정에서 발견 PASS/Error0; 기본 필수 경로도 PASS. 전달 빌드의 같은 고정 입력 재생 PASS.
MEASURE route seconds=45.0558635 ticks=2247 stage=5 pos=(61.04, 1.81) pogos=0 wallJumps=0 hp=2 doubleJump=True
FAIL System.Exception: A04 secret actual input record
MEASURE route seconds=45.0705851 ticks=2248 stage=5 pos=(61.04, 1.81) pogos=0 wallJumps=0 hp=2 doubleJump=True
FAIL System.Exception: A04 secret actual input record
MEASURE route seconds=13.2013548 ticks=643 stage=8 pos=(71.73, 5.81) pogos=0 wallJumps=0 hp=2 doubleJump=True
PASS A04 secret actual input record
MEASURE route seconds=13.1799456 ticks=643 stage=0 pos=(71.61, 5.81) pogos=0 wallJumps=0 hp=2 doubleJump=True
PASS A04 secret actual input replay
미실시: 사용자 비밀 발견/체감 수락. 원본 _raw/B07/A04-secret-resume-secret*.txt.
