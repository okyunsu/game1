# B21 T6 — 머리 모서리 보정 후보

REVIEW: 기술 구현·측정 완료, 기본 채택은 사용자 판단. 종료 기본 회귀는 [T8 최종 소스](_raw/regression/B21-T8-default/result.txt)에서13/13이다. Claude 설계 변경 `7d40918`은 지름길 계약뿐이며 기존 지형·PlayerTuning·Player.prefab 변경0. PlayerMotor 변경은 T6의 Development/Editor 조건부 호출3줄뿐이고 후보가 꺼지면 위치·속도 쓰기를 하지 않는다.

[후보 Asset](../Assets/Candidates/B21CornerFix.asset)의 허용 겹침0.25u(승인 탐색 범위0.15~0.35), 추가 여백0.01u. 기존 Physics2D.defaultContactOffset 두 면의 여백을 더해 실제 분리 여백은 현재0.03u다. 상승 중 다음 물리 틱의 머리 충돌을 감지해 작은 겹침만 더 가까운 바깥쪽으로 옮기며 상승 속도는 유지한다. 현재/다음 위치가 다른 고체와 겹치면 적용하지 않는다. `-corner-fix`가 있는 V5 Development 실행에서만 붙는다. [단독](../Launchers/current/Play-V5-CornerFix.cmd)·[두 후보 동시](../Launchers/current/Play-V5-Candidates.cmd).

## 꺼짐 — 기존 수치 재현

B20Metrics.JumpData의 **같은 최대값 집계 정의**로 검증했다. [이번 실제 원문](_raw/21/resume-20261026-210738/T6-off/metrics.txt)·[JSON](_raw/21/resume-20261026-210738/T6-off/T2-metrics.json)·[수치 확인](_raw/21/resume-20261026-210738/T6-off/identity-proof.txt). 각 대표값은 B20 대비 ±0.001u 안이다.

| 지표 | 이번 대표값 u | B20 u |
| --- | ---: | ---: |
| 풀 점프 최대 상승 | 2.52002 | 2.52002 |
| 짧은 점프 최대 상승 | 1.00800 | 1.00800 |
| 성공 턱 | 2.51250 | 2.51250 |
| 지상/공중 대시 활성 이동 | 2.87988(각3회 모두) | 2.87988 |

**개별 짧은 점프는 [1.00800, 0.84003, 1.00800]이었다.** 3회가 모두 같은 값이었다고 주장하지 않는다. 원래 B20 도구는 이륙을 본 렌더 프레임에 키를 해제하고 최대값을 JSON에 남긴다. 이번 해제 간격은 약0.033초이나 한 표본은 더 일찍 잘렸다. 입력/고정 틱 위상 차이라는 해석이며 게임 코드를 바꿔 수치를 맞추지 않았다. 단일한 인간 조작 한계나 모든 해제 타이밍 동일성을 증명하지 않는다.

```text
MEASURE jump kind=short trial=2 rise=0.84003 releaseAfter=0.03339
PASS corner OFF B20 JumpData full=2.52002 expected=2.52002 tolerance=.001
PASS corner OFF B20 JumpData shortJump=1.00800 expected=1.00800 tolerance=.001
PASS corner OFF B20 JumpData maxStep=2.51250 expected=2.51250 tolerance=.001
PASS corner OFF ground/air dash all6 activeDx=2.87988 tolerance=.001
```

## 켜짐 — 36면 접촉 전후

B20 원본의 같은36면·같은 격리 프로토콜(적/출구/피해 trigger 비활성화)로 실제 exe를 실행했다. 초기 후보는33/36 그대로였으며 물리 접촉 여백을 반영한 최종 후보는 **10/36**, 23면 감소다. 정적 면 전체가 진행 가능해졌다는 뜻은 아니다. 큰 겹침·넓은 천장은 보정하지 않으므로 접촉이 남는다.

| 방 | B20 실제 접촉 | 최종 후보 |
| --- | ---: | ---: |
| A01 | 3/3 | 1/3 |
| A02 | 8/10 | 0/10 |
| A03 | 11/12 | 4/12 |
| A04 | 3/3 | 2/3 |
| A05 | 1/1 | 0/1 |
| A06 | 7/7 | 3/7 |
| 합계 | 33/36 | 10/36 |

[B20 원문](_raw/B20/BL1-head-all.txt)·[이번 최종36면 원문](_raw/21/resume-20261026-210738/T6-final-head.txt). 면별 전후:

| 방 | 면 | B20 접촉 | 후보 접촉 |
| --- | --- | --- | --- |
| A01 | solid1 | 예 | 예 |
| A01 | solid2 | 예 | 아니오 |
| A01 | platform4 | 예 | 아니오 |
| A02 | solid0 | 예 | 아니오 |
| A02 | solid1 | 예 | 아니오 |
| A02 | solid4 | 아니오 | 아니오 |
| A02 | solid6 | 예 | 아니오 |
| A02 | solid8 | 예 | 아니오 |
| A02 | solid11 | 아니오 | 아니오 |
| A02 | solid14 | 예 | 아니오 |
| A02 | platform1 | 예 | 아니오 |
| A02 | platform2 | 예 | 아니오 |
| A02 | platform4 | 예 | 아니오 |
| A03 | solid0 | 예 | 아니오 |
| A03 | solid1 | 예 | 아니오 |
| A03 | solid2 | 예 | 아니오 |
| A03 | solid4 | 예 | 아니오 |
| A03 | solid5 | 아니오 | 아니오 |
| A03 | solid7 | 예 | 아니오 |
| A03 | solid9 | 예 | 아니오 |
| A03 | platform0 | 예 | 예 |
| A03 | platform1 | 예 | 예 |
| A03 | platform2 | 예 | 예 |
| A03 | platform4 | 예 | 예 |
| A03 | platform9 | 예 | 아니오 |
| A04 | solid5 | 예 | 아니오 |
| A04 | platform1 | 예 | 예 |
| A04 | platform2 | 예 | 예 |
| A05 | solid6 | 예 | 아니오 |
| A06 | solid2 | 예 | 아니오 |
| A06 | solid4 | 예 | 예 |
| A06 | solid5 | 예 | 예 |
| A06 | solid8 | 예 | 아니오 |
| A06 | solid9 | 예 | 아니오 |
| A06 | solid10 | 예 | 예 |
| A06 | platform4 | 예 | 아니오 |

## 연결 동작·크기·기존 사망

[최종 동작 원문](_raw/21/resume-20261026-210738/T6-final-movement.txt): 실제 작은 머리 모서리에서 보정과 상승 속도 유지를 확인했고, 벽 점프10/10·포고10/10·대시10/10, Error/Exception/Assert0. [몸1.3](_raw/21/resume-20261026-210738/T6-final-body13.txt)·[몸1.1](_raw/21/resume-20261026-210738/T6-final-body11.txt)은 두 후보를 동시에 켜 실제 보정1회씩·요청 몸 크기·Error0을 확인했다. [위험 지형 HP0](_raw/21/resume-20261026-210738/T6-final-fatal.txt)는 기존 사망/체크포인트 회복을 확인했다. T5의 [A04 섬 접지](_raw/21/resume-20261026-210738/T6-pogo.txt)도 PASS(복귀2회 포함).

[빌드](_raw/21/resume-20261026-210738/T6-build-final.txt) 오류0/경고0. [Quiet](_raw/regression/B21-T6-resume/result.txt) 13/13은 기본 꺼짐 경로 확인이며 그 뒤 후보 여백/검증 fixture만 보완했다. 최종 소스 기본 회귀는 T8에서13/13으로 확인했다.

## 실패·보완 원문

```text
FAIL System.Exception: wall jump actual 1
FAIL System.Exception: Timeout actual small corner correction
B21 corner candidate overlap=0.04992676 shift=-0.05992676 clearNow=True clearNext=False
B21 corner candidate overlap=0.04992676 shift=-0.07992676 clearNow=True clearNext=True
FAIL System.Exception: Timeout actual pogo 3
MEASURE corner movement wallJump=10/10 pogo=10/10 dash=10/10
MEASURE corner compatibility height=1.299988 corrections=1
MEASURE corner compatibility height=1.099976 corrections=1
Error/Exception/Assert=0
```

[첫 동작 실패](_raw/21/resume-20261026-210738/T6-movement.txt)·[분리 여백 검사](_raw/21/resume-20261026-210738/T6-movement-trace1.log)·[여백 보완 뒤 포고 fixture 실패](_raw/21/resume-20261026-210738/T6-movement-margin2.txt). 초기 분리 거리0.01u는 물리 query 여백 때문에 거절돼 실제 보정0이었다. 두 접촉 면의 기존 여백을 포함해 해결했다. 벽 점프 fixture는 과거 접지의 코요테 시간을 기다리고, 포고 fixture는 공격 범위 안(등불 윗면+1.1u)에서 시작하도록 보완했다. 게임의 벽 점프·포고 수치를 바꾸지 않았다.

Ruling: 허용 겹침0.25u / 승인 범위 중간값으로 작은 겹침만 비교 / 사용자 채택 전 기본 꺼짐. 분리 여백은 기존 물리 접촉 여백2개+후보 추가0.01u / 실제 query 비겹침 확인 / 보정량은 겹침 폭보다0.03u 더 크다. 꺼짐 수치 판정은 기존 B20 JumpData 최대값 정의 / 같은 지표 비교 / 개별 짧은 점프 편차를 숨기지 않음.

최종 문 물리 잠금 보완 후에도 [같은36면 재측정](_raw/21/resume-20261026-210738/postdoor-head.txt)·[전후 비교](_raw/21/resume-20261026-210738/postdoor-head-proof.txt)는 방별3→1/8→0/11→4/3→2/1→0/7→3, 합33→10과 Error0이었다. 이 검사는 기존 B20과 같이 적·출구·DamageBlock을 제외한 머리 접촉 프로토콜이다. 새 잠긴 출구의 물리 Collider도 제외해 출구 없는 비교 조건을 유지했다. 자연 경로 통과는 별도의 지름길 exe 검사로 확인했다.
