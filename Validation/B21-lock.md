# B21 T1 — 문과 벽 저장 키 연결

Claude 설계 변경: 시작 계약 `7d40918`, `Design/v5/WORLD_V5.md`·`rooms_v5_A.json`의 draft3.1 잠금/공격 방향 표시. 좌표·수치 변경 없음. 이번 재개 기준 HEAD `544ea5e`, 사용자 2026-10-26 Hub 실행·T1 재개·T3 반복 금지 지시.

생성기는 문 `closed_until_wall_broken`을 V4ExitRule.closedUntilWallBroken으로, `breakable_side` 배열을 같은 인덱스의 BreakableWall.Side로 매핑한다. 기존 gate·elevatorSeal과 AND 조건으로 작동한다. 벽 잠금이 없는 기존 V4 문은 같은 판정을 유지한다. 잠긴 문은 기존 닫힌 표식의 회색을 사용하며 파괴 후 원래 색으로 돌아간다. ProgressSave·저장 스키마·기본 Asset·V4 Scene 변경 없음.

이번 실행: [Preflight](_raw/21/preflight-20261026-210600-366-3eaa46bc/preflight.txt) `hubLicensingClient=True`, Unity exit0. [Development 빌드](_raw/21/resume-20261026-210738/T1-build.txt) `Succeeded errors=0 warnings=0`. 실제 이번 소스 exe [규칙 검증](_raw/21/resume-20261026-210738/T1-rule.txt):

```text
PASS B21RuleProbe actual=exe
PASS absent wall lock preserves existing V4 exit
PASS intact persistent wall locks exit
PASS locked door reuses existing gray closed marker
PASS brokenWalls key opens exit without schema change
PASS wall unlock does not bypass ability gate
PASS ability and wall conditions both required
PASS wall unlock does not bypass elevator seal
PASS blank lock restores legacy condition
Error/Exception/Assert=0
```

기존 V4 Scene을 재생성하지 않고 기존 [V4 실행기](../Launchers/current/Start-V4.ps1)의 NewGame Smoke를 10초 실행해 exit0/Error0을 확인했다([원문](_raw/21/resume-20261026-210738/T1-V4-launch.txt)). 이 런처는 보존된 B12 빌드다. 현재 소스 조건 검증은 별도의 B21 Development exe에서 수행했으므로 구분한다.

Quiet 회귀는 [이번 결과](_raw/regression/B21-T1-resume/result.txt)를 참조한다. 실제 V5 Scene 재생성과 5항목·14문 방향 검증은 T2에서 한다. T3 검사기는 재수행하지 않았다.

종료 점검에서 전환 거부만으로는 문 밖으로 걷는 것을 막지 못함을 발견해 보완했다. 잠긴 동안 기존 문 BoxCollider를 고체 Ground 레이어로 전환하고, 파괴 후 원래 trigger·레이어·색으로 복원한다. 키가 없는 기존 문은 상태를 쓰지 않는다. 새 Collider·Scene·저장 필드 추가0.

[최종 빌드](_raw/21/resume-20261026-210738/postdoor-build.txt) 오류0/경고0, [최종 규칙 exe](_raw/21/resume-20261026-210738/postdoor-rule.txt)는 기존 조건 전체와 `PASS locked doorway is a physical solid`, `PASS blank lock restores original trigger`, Error0을 확인했다. [실제 A03 접근](_raw/21/resume-20261026-210738/postdoor-shortcut.txt)은 몸 오른끝48.9850으로 문 x49 앞에서 멈췄다. [보완 Quiet](_raw/regression/B21-T1-physical/result.txt)와 [종료 기본 회귀](_raw/regression/B21-postdoor-default/result.txt)는 최종 소스를 대상으로 한다.

Ruling: 기존 닫힌 회색 표시와 같은 문 Collider 재사용 / 잠금의 물리 경계를 추가 구조물 없이 유지 / 해당 저장 키가 지정된 문에만 적용하며 해제 시 원상 복원.
