# B21 Backlog2 — 대시 발끝 모서리 후보

REVIEW: 기본 꺼짐. [같은 후보 Asset](../Assets/Candidates/B21CornerFix.asset)에 `dashCornerEnabled=false`·`maxDashStep=0.25u`를 추가했다. [비교 런처](../Launchers/current/Play-V5-DashCornerFix.cmd)는 머리 보정과 대시 보정을 함께 켠다. 기본/기존 머리 후보에는 대시 보정이 꺼져 있다. PlayerMotor는 수정하지 않았고 PlayerDash의 벽 충돌 처리에서 명시 후보일 때만 호출한다. Design·지형·기본 수치·저장 형식 변경0.

발끝이 닿은 구조물의 윗면이 발보다0.25u 이내일 때만, 머리/몸이 들어갈 현재·다음 위치가 비어 있음을 확인하고 위로 옮긴다. 보정 이동은 걸린 높이+기존 두 접촉 여백+추가0.01u다. 큰 벽·문 봉인·천장 겹침은 원래 대시 중단으로 돌아간다.

꺼짐은 [새 실측](_raw/21/resume-20261026-210738/BL2-off/metrics.txt)·[동일성 확인](_raw/21/resume-20261026-210738/BL2-off/identity-proof.txt)에서 T6의 풀 점프·짧은 점프·턱·지상/공중 대시 모든 개별 값과 같았다. 최대값2.52002/1.00800/2.51250·대시2.87988, 짧은 점프의 두 번째0.84003도 같은 값으로 보존했다. [빌드](_raw/21/resume-20261026-210738/BL2-build.txt) 오류0/경고0. [Quiet](_raw/regression/B21-BL2/result.txt)13/13은 이번 실행 결과를 확인한다.

켜짐은 B21Arena 실행에서 높이0.2u 턱에 지상 대시10회, **10/10 통과·매회 보정1회·실제 상승0.21494u**, Error0이었다([전체 원문](_raw/21/resume-20261026-210738/BL2-on.txt)). 자연 V5 모든 미세 단차의 통과를 주장하지 않는다.

```text
PASS BL2 OFF all jump/ledge/dash measured values match T6 OFF exactly, including individual early-cut short sample
PASS B20 reported maxima full=2.52002 short=1.00800 ledge=2.51250; dash all6=2.87988
MEASURE dash-corner trial=1 corrected=1 lift=0.21494 crossed=True feet=(3002.9850, 1000.2140)
MEASURE dash-corner trial=10 corrected=1 lift=0.21494 crossed=True feet=(3002.9850, 1000.2140)
Error/Exception/Assert=0
```

Ruling: 발끝 겹침 허용0.25u·시험 턱0.2u / 작은 모서리만 비교하고 기존 머리 후보와 같은 규모로 둠 / 후보에서만 지름길이 쉬워질 수 있어 기본 채택은 사용자 판단. 토글은 기본false, 인자 `-dash-corner-fix`가 있을 때만 일시적으로 켤 수 있으며 기본 Asset을 런타임에 수정하지 않는다.
