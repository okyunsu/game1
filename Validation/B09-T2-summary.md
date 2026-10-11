# B09-T2 — 내려찍기

상태 REVIEW (입력 손맛 사용자 수락 대기). Release 실제 입력 검증 PASS, Error/Exception/Assert=0.
공중 아래+C 발동, 땅/대시 중 불가, 수평0·하강24u/s, Seal 일반 아래 공격 불변·Slam 파괴, 더블 점프/등불 Pogo 기존 입력 PASS.
첫 FAIL은 대시 cooldown/공중 사용 상태 준비 오류였고 바닥에서 .5초 대기로 도구만 보정했다.
원문: `_raw/B09/T2-3-slam-a1.txt`, `_raw/B09/T2-3-slam-a2.txt`.
Seal은 IDamageable/BreakableWall/PogoTarget을 구현하지 않아 일반 공격·포고·대시 파괴 경로가 없다. 별도 Seal 대시 충돌·포고 충돌 시나리오는 미실시.
수치: Assets/ScriptableObjects/MechanicsTuning.asset. PlayerTuning 값 변경 없음.
개발 F8 능력 부여. hasSlam 저장/보스 보상은 V4 통합 Task에서 연결·검증한다.
