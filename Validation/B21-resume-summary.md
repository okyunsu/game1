# Batch 21 재개 결과

| 종료 검사 | 이번 실행 결과 |
| --- | --- |
| Preflight | [PASS](_raw/21/preflight-20261026-210600-366-3eaa46bc/preflight.txt), Hub licensing client=True·Unity exit0 |
| 최종 Development 빌드 | [오류0·경고0](_raw/21/resume-20261026-210738/postdoor-build.txt), `Builds/B21-delivery/Afterglow.exe` |
| 최종 소스 회귀 | [Quiet13/13](_raw/regression/B21-T1-physical/result.txt)·[종료 기본13/13](_raw/regression/B21-postdoor-default/result.txt) |
| 실제 런처 | [최종10개 모두 PASS](_raw/21/resume-20261026-210738/postdoor-launchers-retry1/summary.txt), 각10초·Error0. 필수8개+대시 후보+BodySizeTest |
| V5 정적 | [실제 데이터 FAIL0/WARN0·self-test PASS](_raw/21/resume-20261026-210738/T8-static.md). 의도한 위반 fixture의 FAIL은 원문 보존 |
| V5 Scene | [6방 검사](_raw/21/resume-20261026-210738/T8-clearance.txt)·[B19 동일 규칙 비교](_raw/21/resume-20261026-210738/T8-scene-protection-docs.txt): FAIL0/WARN0, 기존69후보 유지 |
| 보호 파일 | [현재 정책88개 SHA 변경0](_raw/21/resume-20261026-210738/final-protection.txt). Design·기본 Asset·Player.prefab·금지 Scene/코드 변경0 |
| Batch-Card | [최종 카드](_raw/21/resume-20261026-210738/final-batch-card.txt): 범위 밖0·TASKS 깨진 링크0·없는 빌드/런처0. 별도 작업 트리 검사는 [종료 원문](_raw/21/resume-20261026-210738/final-worktree.txt) |

사용자 2026-10-26 지시로 T1부터 재개했다. **T3는 기존 완료 `736c6aa`를 유지했고 구현·개별 작업을 반복하지 않았다.** T8의 필수 종료 정적 검사는 수행했다. 최초 라이선스 exit198와 당시 차단 상태는 [이전 종료 원문](B21-summary.md)에 그대로 남겼다. 재개 Base는 `544ea5e`, 최초 계약 Base는 `7d40918`이다.

Claude 설계 변경: `7d40918`의 WORLD_V5.md·rooms_v5_A.json 지름길 잠금/벽 공격 방향 계약. 좌표·수치 변경 없음. 이번 실행에서 Design 원본 수정0이다.

## 플레이 가능해진 범위와 Task

| Task | 상태 | 결과·근거 |
| --- | --- | --- |
| T1 | DONE | 저장 키와 문 AND 조건·벽 공격 방향 연결. 잠김 고체/해제 trigger 복원. A03 몸 오른끝48.9850에서 문 x49 앞 정지. 기존 gate·봉인 유지·저장 스키마 변경0. [구현/규칙](B21-lock.md) |
| T2 | DONE | V5 6방 재생성·기하/요소 유지, 실제5항목·14방향·stub4·일방 역방향2·저장/Continue PASS. [실제 지름길](B21-shortcut.md) |
| T3 | DONE 유지 | 이전 결과 보존. [검사기](B21-room-rules.md) |
| T4 | DONE | B21Arena·4레시피 각3회·0.05u 이분 탐색. [가로 한계](B21-gaps.md) |
| T5 | REVIEW | 위험 지형 후보 기본 꺼짐. A05 통과0/5·동쪽 복귀5/5·피해1, HP0 기존 사망/체크포인트 유지. [도랑 폭24회](B21-hazard-return.md) |
| T6 | REVIEW | 머리 보정0.25u 후보 기본 꺼짐. 같은36면 접촉33→10, 벽점프/포고/대시 각10·몸1.3/1.1 실제 보정. [전후36행](B21-corner.md) |
| T7 | DONE | 36면 필요 여유4.22u 대비 부족0.12~1.82u·오름·천장 표. [기본 몸 표](B21-headroom-table.md) |
| T8 | DONE | Development·런처·회귀·정적/Scene·보호/범위 확인. 실패와 재시도는 아래 보존 |
| T9 | DONE | README 실행 안내·TASKS 0/2/3/9절. 결정4/8, 과거 미종결과 플레이 순서 유지 |
| BL1 | DONE | 몸1.3/1.1 동일36면·원자료 후보 각32면·최대 부족1.52/1.32u. [작은 몸 표](B21-headroom-small.md) |
| BL2 | REVIEW | 대시 발끝 별도 토글 기본 꺼짐·0.2u 턱10/10·매회 실제 보정1회. [꺼짐 동일성/켜짐](B21-dash-corner.md) |
| BL3 | DONE | 후보(a)만·능력 없음·실제 적 제외 A04 등불2개→섬 보상5/5·HP4→4·복귀0. [위치/레시피](B21-a04-island.md) |

재개 실행11 Task와 기존 T3 유지1건이다. 필수 Task·Backlog 소진, Ready Queue0으로 종료한다. REVIEW는 기술 작업 완료 뒤 사용자 감각·채택 판단 대기다. 기본 크기·후보 채택·Sprint Gate를 승인한 것으로 처리하지 않았다.

## 후보 꺼짐/켜짐과 draft4용 수치

꺼짐 실측의 기존 B20 최대값: 풀 점프2.52002·짧은 점프1.00800·턱2.51250·지상/공중 대시2.87988u를 재현했다. **짧은 점프 개별3회는 `[1.00800, 0.84003, 1.00800]`**이다. BL2 추가 후도 개별 값이 같았다. 모든 짧은 입력 타이밍이 동일했다는 뜻은 아니다. 기존 도구의 최대값 정의를 비교했고 입력/물리 틱 위상 해석은 추론으로 남겼다([원문과 한계](B21-corner.md)).

| 항목 | 꺼짐 | 켜짐 |
| --- | --- | --- |
| A05 일반 점프 | 통과1/1·HP4→3·복귀0 | 통과0/5·매회 HP4→3·동쪽 복귀5/5 |
| 머리 접촉, 같은36면 | B20 접촉33 | 최종 접촉10, 방별1/0/4/2/0/3 |
| 대시 발끝0.2u 턱 | 별도 토글 기본false | 10/10·실제 상승0.21494u |
| A04 섬 구간 | 이번5회 비교의 대상 아님 | 포고2회로 섬 보상5/5·피해/복귀0 |

머리36면은 적·출구·위험 트리거를 제외한 기존 통제 프로토콜이며 자연 전체 경로 완주 증거가 아니다. 물리 잠금 보완 후에도 [같은36면 재확인](_raw/21/resume-20261026-210738/postdoor-head-proof.txt)33→10이다.

| 가로 이동 레시피 | 최소/최대/평균 u (각3회) |
| --- | --- |
| 달리기 점프 | 4.55 / 4.60 / 4.5833 |
| 점프 정점 부근 공중 대시 | 7.35 / 7.40 / 7.3833 |
| 지상 대시 직후 점프 | 4.30 / 4.30 / 4.30 |
| 대시만 | 3.05 / 3.05 / 3.05 |

B18 달리기4.75u 대비 이번 차이는0.15~0.20u로 ±0.25u 안이다. 지상 대시 후 점프는 기존 대시 종료 수평속도0 처리 후 재가속 레시피다. 대시만3.05u는 틈 통과 한계이므로 활성 대시 이동2.87988u와 구분한다.

| Arena 도랑 폭 u | 능력 없음 성공/3 | 정점 대시 성공/3 |
| ---: | ---: | ---: |
| 4.5 | 3/3 | 3/3 |
| 5.0 | 0/3 | 3/3 |
| 5.5 | 0/3 | 3/3 |
| 6.0 | 0/3 | 3/3 |

실패 시 피해1·동쪽 안전 바닥 복귀였다. 이 후보가 모든 도랑을 막지는 않는다. 36면 여유 표의 필요4.22u는1.6+2.52+0.1이며, 측정한 아래 면에서 위 구조물 윗면까지 직접 오르면36개 모두2u 초과다. 천장 자체3·다른 천장 겹침14·외곽17로, 다른 경로의 도달 가능성을 확정하지 않는다.

## 실패 원문·검증 한계

- 초기 후보 빌드 구성 누락, fixture의 trigger 누락·입력 준비, A04에 승인 밖 복귀0 조건을 추가했던 검증 실패는 [T5](B21-hazard-return.md)에 그대로 보존했다. 수정한 별도 원문만 통과 근거로 쓴다. 최종 머리 보정 분리 여백·벽점프/포고 fixture 수정 원인은 [T6](B21-corner.md)에 있다.
- 이전 T5/T6-pogo의 `enemiesDisabled=True`는 HP 복원 후 실제 상태를 보장하지 못한 라벨이었다. 접지/복귀 실측은 유지하고, BL3에서 HP 복원 뒤 실제 적 비활성을 매번 확인해5/5를 새로 측정했다.
- 종료 중 [기본 V5 최초 실행](_raw/21/resume-20261026-210738/closing-launchers/Play-V5.txt)은 아래처럼 실패했다. 기존 basename의 Smoke 슬롯 json/tmp를 [별도 보존](_raw/21/resume-20261026-210738/closing-failed-slot/)했고, [새 슬롯 재시도](_raw/21/resume-20261026-210738/closing-launchers/Play-V5-retry1.txt)는 PASS였다. 원인은 확정하지 않았고 저장 코드를 바꾸지 않았다.

```text
FAIL B20LaunchProbe actual=exe
FAIL System.Exception: Timeout room ready
Error/Exception/Assert=0
```

- 물리 잠금 보완 후 최초 런처 검사10건은 검증 호출에서 `Evidence`에 절대 경로를 넘겨 Windows PowerShell의 `Join-Path` 뒤 `GetFullPath`가 실패했다([호출 stderr](_raw/21/resume-20261026-210738/postdoor-launchers/postdoor-Play-V5-stderr.txt)). Unity exe 실행 전 도구 호출 실패다. 기존 상대 경로 계약으로 고친 [새 실행10건](_raw/21/resume-20261026-210738/postdoor-launchers-retry1/summary.txt)은 모두 PASS/Error0이었다.
- 자연 전체6방 완주·모든 입력 타이밍·사용자 체감 수락·Release 빌드·Sprint Gate는 수행하지 않았다. 정적 도달 후보를 실제 완주로 바꾸어 기록하지 않는다.

## 실행 대상과 보존

Unity `C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe`, Python `C:/Users/gosu3/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe`. Unity와 호출 스크립트/검증 exe는 사용자 지시대로 샌드박스 밖에서 실행했다. 최종 빌드 `V5Batch20.Build -b21-candidates`, Scene 생성은 `V5Batch21.Generate`의 Unity API로만 수행했다.

정적 명령은 `Tools/Check-RoomRules.py Design/v5/rooms_v5_A.json --v5 --self-test --world Design/v5/world_v5_A.json --max-step-evidence Validation/_raw/21/resume-20261026-210738/T6-off/T2-metrics.json --self-test-output-dir Validation/_raw/21/resume-20261026-210738/T8-rule-fixtures --output Validation/_raw/21/resume-20261026-210738/T8-static.md`. 이번 턱 실측을 전달했고 trench-escape MEASURE9개를 기록했다. Scene 검사는 Unity `-executeMethod V5Batch18.Clearance -verify-b18-clearance`, 대상 `Assets/Scenes/V5/*.unity`·비교 B19 동일 규칙이다. 기존69후보(닫힌 문6·측면43·구덩이20)를 보존했다. 이후 문 런타임 수정은 Scene 원본/정적 자료를 바꾸지 않았다.

빌드는 B21-delivery와 기존 V4 B12-delivery를 보존했다. 지운 B20 빌드의 슬롯12개는 [SHA 검증 뒤 보존](_raw/21/resume-20261026-210738/T8-slot-manifest.json)했고 [정리 기록](_raw/21/resume-20261026-210738/T8-housekeeping.txt)을 남겼다. 원본 로그/JSON/PNG는 `_raw`에 남기고 커밋하지 않는다.

## Ruling과 사용자 확인

- 문은 기존 회색·기존 Collider 재사용 / 잠금 물리 경계와 해제 복원 / 저장 키가 있는 문에만 영향.
- 후보 복귀 정지0.12s·머리 허용0.25u·추가 여백0.01u·대시 허용0.25u / 승인된 후보 탐색의 비교값 / 기본 채택 전에는 비교 런처에서만 영향.
- 간격 탐색0.5~14u·출발x-3.5·점프x-0.35, A04 포고 입력 높이+0.4/+0.7u / 정해진 레시피로 재현 / 다른 입력의 경계와 성공률은 달라질 수 있음.
- 여유 표는 같은36면·직접 대응하는 아래/위 면 기준 / 원자료에 없는 경로를 추정하지 않음 / 전체 경로 설계는 Claude draft4와 사용자 판단.
- 이전 TASKS 표는 이번 허용 범위에 Archive가 없어 현 위치에 보존 / 범위 밖 이동 방지 / 문서 길이만 남음.
- 최초 Base 이후 이력과 T3는 이미 공개돼 재개 Base `544ea5e` 이후의 이번 미공개 커밋만 합친다. 기존 원격 `B21` 태그는 과거 차단 종료를 가리켜 보존하고 최종 태그는 `B21-resume`으로 구분한다 / 공개 이력 변경 금지 준수 / 비교 시 두 태그를 구분해야 함. push하지 않는다.

기존 V4/V5·몸 크기 확인 순서 뒤, [복귀 후보](../Launchers/current/Play-V5-HazardReturn.cmd) A05/A04 5~10분 → [머리 후보](../Launchers/current/Play-V5-CornerFix.cmd) A01/A02 3~5분 → [동시 후보](../Launchers/current/Play-V5-Candidates.cmd)5~10분 → [대시 후보](../Launchers/current/Play-V5-DashCornerFix.cmd)3~5분을 비교한다. A01에서 시작한다. 결정 대기는 D-BODY·D-V5-DRAFT4·D-HAZARD-RETURN·D-CORNER-FIX 총4건이며 추천/선택지는 [TASKS](../TASKS.md#2-결정-대기)에 있다.

조정값은 `Assets/Candidates/B21HazardReturn.asset`과 `B21CornerFix.asset`에만 있다. Play 중 바꾼 값은 종료 전 기록하고 Edit 모드에서 명시 저장한 뒤 Development 빌드를 다시 만든다. Scene 배치·기본 몸·기본 수치 Asset은 이번에 바꾸지 않았다.
