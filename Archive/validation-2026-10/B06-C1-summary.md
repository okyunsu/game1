# B06 C1 — 부서지는 벽

DONE / 2026-10-10. 신규 BreakableWall과 Shortcut/Secret 프리팹2종, WallJumpTest 바닥2개. hitsToBreak3·방향·wallId Inspector. 기존 HitFeedback 사용. PlayerAttack에서 공격당 벽1회 전달하고 포고 판정에서 제외한다. 금지 쪽에서 숏컷 표시를 숨기며 비밀 벽은 금 간 선을 표시한다.

ProgressData에 brokenWalls 배열만 추가, version1 유지. NewGame 초기화·Continue 복원·SaveCurrent 기록. 방 재입장/사망은 영구 세트를 유지, 저장 없는 새 Editor 세션에서는 세트 초기화.

실제 Unity60fps: 금지 방향 실제 공격 피해0, 허용 실제 공격3회 각각1히트/세 번째 개방, 비밀 반대쪽 명중, 아래 공격 포고0. 격리 디스크에 wallJump=true/개방 ID 기록·사망 유지·방 재입장 시 프리팹 재생성 비활성·타이틀/Continue 능력/개방 복원·새 게임 초기화 PASS/Error0. 기존 필드만 있는 version1 JSON의 missing field=false/빈 배열 및 Continue PASS. 증거 `Validation/_raw/B06/C1-break-a1.txt`, `C1-authoring.txt`.

재입장 검사는 기본 방에 runtime fixture를 생성한 검사다. 기본 Scene에 벽을 추가하지 않았다. WallJumpTest는 계속 저장 비사용이며 영구 저장 검사는 별도 QA 파일만 사용. 사람의 두 벽 시각 구분은 전달 빌드에서 확인 대기.
