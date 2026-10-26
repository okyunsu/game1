# B21 T5 — 위험 지형 복귀 후보

REVIEW: A05·도랑 폭 표·기본 꺼짐·회귀와 A04 섬 접지 확인 완료. 초기 PARTIAL 원인과 실패 원문은 아래에 보존했다. 기본 채택은 사용자 판단이다. Claude 설계 변경 `7d40918`은 지름길 계약뿐이며 이번 Design·기본 수치·Player.prefab·RoomSession·KillZone 수정0.

[후보 Asset](../Assets/Candidates/B21HazardReturn.asset): 접지0.1초·위험 가장자리 몸 여백0.5u·복귀 전 정지0.12초. 움직이는/무너지는 바닥은 안전 위치로 기록하지 않는다. 피해가 실제 적용되고 HP>0일 때만 복귀한다. 설정은 Development 빌드의 임시 Menu 구성에서 참조하며 원본 Scene은 변경하지 않는다. `-hazard-return`과 V5/측정 arena가 함께 있어야 Player에 컴포넌트를 붙인다. 기본 인자에서는 컴포넌트0. [런처](../Launchers/current/Play-V5-HazardReturn.cmd).

## 실제 결과

[꺼짐](_raw/21/resume-20261026-210738/T5-off-retry1.txt), [켜짐](_raw/21/resume-20261026-210738/T5-on-airborne3.txt), [올바른 트리거 폭 측정](_raw/21/resume-20261026-210738/T5-arena-fixture2.txt). 이번 빌드 오류0/경고0, [Quiet](_raw/regression/B21-T5-resume/result.txt) 13/13. Quiet 이후 변경은 빌드 구성 주입·검증 fixture이며 기본 피해 로직은 그대로다. 최종 소스 회귀는 T8에서 실행한다.

| 항목 | 결과 |
| --- | --- |
| 꺼짐, B20 A05 일반 점프 | 통과1/1·HP4→3·복귀0 |
| 켜짐, A05 동쪽에서 일반 점프 | 통과0/5·각 HP4→3·동쪽 바닥 복귀5/5 |
| A04 등불 포고 | 재확인 섬(29.8056,1.8150)·grounded=True·포고3·복귀2·PASS |

| 도랑 폭 u | 능력 없음 성공/3 | 대시 보유(정점 대시) 성공/3 | 실패 시 |
| ---: | ---: | ---: | --- |
| 4.5 | 3/3 | 3/3 | 없음 |
| 5.0 | 0/3 | 3/3 | 피해1·동쪽 복귀 |
| 5.5 | 0/3 | 3/3 | 피해1·동쪽 복귀 |
| 6.0 | 0/3 | 3/3 | 피해1·동쪽 복귀 |

폭 표는 B21Arena의 상대 높이0/-0.6, 도랑 DamageBlock 상단-0.3에서 측정했다. 이동 레시피는 동쪽에서 달리기→점프, 대시 보유 시 정점 부근 대시. 능력 없음4.5u는 피해 없이 건너므로 이 후보가 모든 도랑을 막는다는 뜻이 아니다. 실제 A05 폭6u에서만 통과0/5를 확인했다. 지형 변경0.

```text
MEASURE A05-return enabled=False trial=1 crossed=True hp=4->3 returns=0 end=(13.2216, 2.9230)
MEASURE A05-return enabled=True trial=1 crossed=False hp=4->3 returns=1 end=(13.4712, 1.0150)
PASS A05 no crossing and eastern return 5
MEASURE trench-width width=5.0 dashOwned=False trial=1 crossed=False hp=4->3 returns=1 end=(5.7976, 0.0150)
PASS failed crossing touched damaging trench and returned
MEASURE trench-width width=6.0 dashOwned=True trial=3 crossed=True hp=4->4 returns=0 end=(-1.4200, 0.0150)
Error/Exception/Assert=0
```

## 실패와 한계

원문은 삭제하지 않았다. 초기 빌드는 후보 구성 누락으로 실패했다. 명시 `-b21-candidates`·CleanBuildCache 후 실제 설정 주입과 컴포넌트 생성 확인. 다음 실패는 공중 배치 후 대기/이전 접지 상태에 따른 포고 입력 준비 문제다.

```text
FAIL System.Exception: hazard candidate opt-in True
FAIL System.Exception: arena candidate enabled
FAIL System.Exception: A04 pogo at 24 feet=(18.86, 1.02)
FAIL System.Exception: A04 pogo at 24 feet=(16.83, 1.02)
MEASURE A04-pogo end=(29.8368, 1.8150) pogos=3 returns=2 enemiesDisabled=True
FAIL System.Exception: A04 two-lantern pogo reaches island without hazard return
```

마지막 FAIL에는 Batch에 없는 **복귀0회** 추가 조건이 있었다. 그 조건은 제거하고 실제 섬 접지 도달만 확인하도록 검사기를 바로잡았다. 섬 도달 수치는 그대로 보존하며 수정된 판정은 새 빌드 [T6 중 독립 재확인](_raw/21/resume-20261026-210738/T6-pogo.txt)에서 PASS였다. A04 통과에 복귀2회가 있었음을 유지한다. 최초 폭 표 `T5-arena-retry1`은 런타임 fixture의 isTrigger 누락으로 피해0이었으므로 유효 증거에서 제외했다. 위 표는 수정 후 `fixture2`만 사용한다. 게임 DamageBlock/후보 로직 실패와 구분한다.

Ruling: 복귀 정지0.12초 / 피해가 보인 뒤 짧게 원위치로 돌아오도록 비교 / 사용자 체감 채택은 보류. 후보 Asset 값을 바꾸면 재빌드하며 Play 중 변경은 종료 전에 기록 후 Edit 모드에서 명시 저장한다. B21 전달 빌드가 검증된 뒤 기존 B20 빌드 정리와 모든 런처 확인은 T8에서 한다.

HP0 조건은 [이번 실제 사망 확인](_raw/21/resume-20261026-210738/T6-final-fatal.txt)에서 후보 복귀0·기존 사망/체크포인트 HP 회복 PASS로 확인했다.

측정 라벨 정정: T5/T6-pogo 원문의 enemiesDisabled=True는 HP 복원 뒤 적 재생성 때문에 실제 제외를 보장하지 못했다. 원문의 실제 접지 도달·복귀2회는 유지한다. [BL3 재확인](B21-a04-island.md)은 매번 HP 복원 뒤 실제 비활성 검사·이동 능력 없음으로 5/5 보상 수집, 피해0·복귀0·포고2였다.
