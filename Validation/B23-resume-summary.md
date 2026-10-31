# Batch 23 — 등장·복귀 연출과 draft4.2

| 종료 검사 | 이번 실행 결과 |
| --- | --- |
| Batch-Card | [Task 커밋 후 카드](_raw/23/final-batch-card-resume.txt): `667543d..539fa2b` 30파일, 범위 밖0·깨진 링크0·없는 빌드/런처0. 이후 종료 기록 TASKS·본 요약만 추가 |
| 보호/작업 트리 | [보호99파일 SHA 변경0](_raw/23/protected-final.txt). 기존 Claude 문서 커밋 이후 Design·V4 Scene·기본 튜닝·ProjectSettings 변경0. 종료 후 별도 git status 확인 |
| 빌드/회귀 | [최종 빌드](_raw/23/build8-final/result.txt) Development 오류0/경고0. [현재 소스 기본 회귀](_raw/regression/B23-final/result.txt) **13/13**. 이후 제품 코드 변경0(검증 입력만 보완) |

2026-10-31, 사용자 “다시해줘 틀어놨어”로 재개. [지시문](../Design/proposals-1031/01_CODEX_BATCH23.md). 이전 [exit198 차단 기록](B23-summary.md)은 보존했다. 재개 Preflight는 [hubLicensingClient=True·exit0](_raw/23/preflight-20261031-150230-286-1d8e5318/preflight.txt). 선행 Claude 문서8파일은 내용 변경 없이 `667543d`로 커밋했다. 기존 차단2커밋을 보존했으며 fetch/push 없음.

## 새로 플레이 가능한 범위

[Play-V5-Loop](../Launchers/current/Play-V5-Loop.cmd), 빌드 `Builds/B23-delivery/Afterglow.exe`. B1 등장1.8초(잠금·흔들림·낙하·착지 표시·정지), 이동만 허용·양쪽 무피해. HP22. V5 안전 바닥 복귀에 어두워짐·일어나기0.45초를 추가했다. draft4.2 V5 6방을 Unity API로 재생성했으며 A03은 low 시작 승강기와 서쪽 방향 대시 연습이다. 좌표·Design 원본·몸 크기·모서리 기본값은 변경하지 않았다.

## 검증 결과와 남은 기준

| Task | 상태·실제 결과 | 근거 |
| --- | --- | --- |
| T0 | DONE — 전달 출처·사용자 방향·Claude 추천 구분. B22-T5는 딜레이 추가 요구와 함께 REVIEW 유지 | TASKS 2-1/10절, `8d4d3ab` |
| T1 | REVIEW — 시작→첫 패턴1.800s, 양쪽 피해0, 이동만 허용. B3 8패턴·페이즈·사망 초기화·Slam/엔딩 PASS | [B1](_raw/23/T1-T2-boss1/result.txt), [B3 Cycle](_raw/23/T1-B3Cycle-fixture/B3Cycle.txt), [Phase](_raw/23/T1-B3Phase-fixture/B3Phase.txt), [Ending](_raw/23/T1-B3Ending-fixture/B3Ending.txt), `3c0416f` |
| T2 | DONE — HP14→22만 변경. 무능력 실제 처치12.867s·HP3·포고1·3패턴·Error0 | 같은 B1 실행, `1bbbbf0` |
| T3 | REVIEW — 일어나기0.450s·총복귀0.650s·잠금 중 피해0·HP0 사망/체크포인트 복원 PASS | [원문](_raw/23/T3-hazard1/result.txt), `5b1d291` |
| T4 | DONE — low 잠금 없음, 실제 X 레버 상→하→상 탑승·V4 B01/C01 기존 위 시작/잠금/상단 해제 PASS | [원문](_raw/23/T4-lift-centered/result.txt), `b370eb1` |
| T5 | PARTIAL — 승강기 lo↔hi 양방향 fixture·접근 간격 경계/블록 제외 self-test PASS. 본문 규칙의 첫 고리19→0. 전체 FAIL3(A06), WARN24 | [정적](_raw/23/T5-static-final.md), [이전 비교](_raw/23/T5-comparison.txt), `d65bedc` |
| T6 | PARTIAL — 6 Scene 재생성·요소 수 일치. Scene FAIL4(A02 overhead), 정적 FAIL3(A06). 요구 FAIL0 미달 | [생성](_raw/23/T6-generate/result.txt), [요소](_raw/23/T6-counts/result.txt), [Scene](_raw/23/T6-clearance/result.txt), `0027aa3` |
| T7 | PARTIAL — A03 각 무능력0/5·대시5/5. A01/A02 접근 머리0. A04 두 번째 접근1/5 접촉으로 전체 머리0 미달. 무너짐3개·가시복귀2방·높은 조각 PASS | [연습30회](_raw/23/T7-practice2/result.txt), [필드](_raw/23/T7-field2/result.txt), `cf37877` |
| T8 | BLOCKED — 자동 경로 재시도 종료. A04 일반 적 구간 피해와 무너짐 착지에서 미완주. 새 고리 완료·그 뒤 Continue 미실시 | [고리2](_raw/23/T8-loop2/result.txt), [고리3](_raw/23/T8-loop3-combat/result.txt), [고리4](_raw/23/T8-loop4-facing/result.txt) |
| T9 | PARTIAL — 질문2개·기록4칸·README/인계·B23 런처 갱신. V5 Loop·V4 BeforeBoss smoke Error0. 전체 고리 미완주 한계 명시 | [고리 런처](_raw/23/final-loop-launch.txt), [V4 런처](_raw/23/final-v4-launch.txt) |
| BL1 | DONE — A06 실제 검출 목록 아래 기록, 수정0 | [본문 규칙 비교](_raw/23/T5-comparison.txt), [참고 스크립트](_raw/23/BL1-reference-d42.txt) |

| 필드 오름(시작 상면→도착 상면) | 머리 접촉 | 착지 |
| --- | --- | --- |
| A01 x4/3→x8.8/4.8 | 0/5 | 5/5 |
| A01 x8.8/4.8→x14.6/6.8 | 0/5 | 5/5 |
| A01 x14.6/6.8→x24.4/8.8 | 0/5 | 5/5 |
| A01 x24.4/8.8→x28.2/10.5 | 0/5 | 5/5 |
| A02 x4.5/3→x8.8/5 | 0/5 | 5/5 |
| A02 x8.8/5→x12.4/7 | 0/5 | 5/5 |
| A02 x12.4/7→x16/9 | 0/5 | 5/5 |
| A04 x5/3→x9.3/5 | 0/5 | 5/5 |
| A04 x9.3/5→x13.5/7 | **1/5** | 4/5 |

선택한 일반 오름 접근의 실제 입력 측정이며 A02 위쪽 벽 점프 경로나 Scene 전체 머리 여유의 통과를 뜻하지 않는다. A04 접촉 상대는 다음 발판 `platform2`, 위치(15.264,4.565). 첫 측정의 무너지는 발판 주기 혼입을 피하려 두 번째 측정은 출발/도착 발판의 복구에3.8초를 기다렸으나1건이 남았다. 좌표는 수정하지 않았다.

## 설계 기준 미달과 판단

- A02 platform3 상면10.5에 대해 climb0 하단11. Scene 검사가 overhead4곳을 검출했다. Codex 좌표 변경 금지이므로 데이터 수정이 필요하다.
- A04 일반 오름의 머리 접촉0 기준은 미달이다. 다음 발판 `platform2`에1/5 접촉했다. 위치/조작 경로를 Claude가 검토해야 한다.
- T5 본문은 떠 있는 발판 오름≥1.5, 참고 스크립트는 낮은 오름도 포함한다. draft4.1 첫 고리는 본문19건, 참고20건(A01 오름1.1 추가). 개수를 맞추려고 규칙을 변경하지 않았다.
- A06은 수정 금지/다음 draft 대상이나 전체 FAIL0 기준에는 포함된다. 본문3건을 숨기지 않고 FAIL로 남겼다. 승강기 지원으로 A03→A04 문 도달 WARN은 사라졌다. 남은24WARN은 위 마을/대시 연습의 정적 후보 등이며 실제 진행 불가로 단정하지 않는다.

| A06 출발→떠 있는 목표 | 간격u | 오름u | 본문/참고 |
| --- | ---: | ---: | --- |
| solid5→platform1 | 1.0 | 2.0 | FAIL / APPROACH |
| platform1→platform0 | 1.0 | 2.0 | FAIL / APPROACH |
| solid7→platform3 | 1.4 | 2.0 | FAIL / APPROACH |
| solid6→platform2 | 1.0 | 0.9 | 본문 대상 밖 / APPROACH |
| solid7→platform2 | 1.5 | 0.9 | 본문 대상 밖 / APPROACH |

## 실패 원문과 미실시

```text
Assets\Scripts\Verification\B22Probe.B23.cs(42,57): error CS7036: There is no argument given that corresponds to the required formal parameter 'key' of 'B09Json.A(Dictionary<string, object>, string)'
FAIL System.Exception: Timeout room ready
ArgumentException: The scene is invalid.
FAIL System.Exception: Timeout lift endpoint
MEASURE A03 lift up=True surface=13.000 feet=3.198 unlocked=True
FAIL System.Exception: player rides lift to endpoint
MEASURE A04 approach from=(9.3,5) to=(13.5,7) headContacts=1/5 landed=4/5
FAIL System.Exception: all field approach head contacts0
MEASURE B1 fight seconds=6.400 defeated=False playerHP=0 pogos=0 patterns=1,2 phaseTransitions=0 body=1.60
FAIL System.Exception: actual inputs defeat B1 without dash/wall/double jump
MEASURE loop generalEnemy defeated=False playerHP=2 player=(8.93, 3.07) enemy=(10.65, 5.52, 0.00)
MEASURE loop jump=A04 crumble landed=False end=(14.0842, 1.0150) hp=2
FAIL System.Exception: physical jump A04 crumble
```

build1은 검증 코드 배열 형변환 오류였고 수정 후build2 errors0/warnings0이었다. B3 첫3검사는 후보 주입 없는 빌드의 CP-C04 누락으로 전투 전에 실패했다. [build3](_raw/23/build3-candidates/result.txt)부터 B22 기존 인자 `-b21-candidates -b17-draft2-map -b11-candidates`를 반영한 뒤 모두 통과했다. B3 전투 회귀 실패로 바꾸어 기록하지 않았다. 승강기 검증 첫 시도는 레버 사거리 부족, 이후 오른쪽 끝에 남아 위층의 이웃 발판 아래와 충돌하는 경로였다. 원문은 T4-lift1/2/3와 T8-loop1에 보존했다.

기존 D42/WORLD 문서의 FAIL0은 이번 B23 실행 증거가 아니다. max-step 실측 JSON이 없어 trench-escape MEASURE 미실시. Release·실제 패드·사람의 첫 승리 시간/감각 수락·Sprint Gate 미실시.

Ruling: 접근 규칙은 정상 점프 높이 안(오름1.5~2.6, 떠 있는 목표 하단이 출발 상면보다 위, 간격1.8 미만)을 대상으로 참고 스크립트의 상한을 사용했다. 낮은 오름을 섞어20건으로 맞추지 않았다. 틀렸을 때 비용은 검사 규칙 재승인·재측정이며 기본값·좌표 변경은 없다.


## 고리·탑승 한계와 인계

승강기는 레버를 치기 위해 오른쪽 끝에 간 뒤 중앙x44.8로 이동하면 상면1↔13 실제 탑승을 통과했다. 끝에 계속 남는 입력은 위층 이웃 발판 아래에 머리가 닿아 탑승을 놓쳤다. 탑승 안내에 중앙 이동을 적었고 자연스러운지는 사용자 확인이 필요하다. V4 Scene 원본 수정0, 기본 위 시작/잠금 동작은 실제 B01/C01에서 확인했다.

T8 첫 경로는 같은 승강기 끝 탑승 실패, 둘째는 A04 일반 적에게 HP4→1을 잃고 보스에게 사망, 셋째는 일반 적 처치를 불필요한 검사 조건으로 강제하고 공격 방향 유지가 잘못돼 중단, 마지막은 반격 방향을 고쳤으나 적 접촉으로 플랫폼에서 밀려난 뒤 무너지는 발판 착지에 실패했다. 시작 CP fixture 이후 위치·HP·튜닝·적 상태·획득 능력을 써서 통과시키지 않았다. 입력 경로 보완의 비용이 커졌고 재시도 한도에 도달해 자동 경로 작업을 종료했다. 게임 전체 진행 불가로 단정하지 않지만 새 연속 고리 통과 증거는 없다. 기술적인 개별 검증 결과와 구분한다.

사용자 확인은10~15분 예상, 질문: ① 보스 등장이 나타나는 느낌인가? ② A03 위층 연습·승강기가 자연스러운가? 기록: 고리 시간____ / 보스 시도 횟수____ / 막힌 곳____ / 머리 박은 곳____. B1 단독 테스트는3~5분. 기존 직접 플레이 수락 순서는 TASKS3절에 보존했다.

조정: B1 후보 BossTuning의 HP22/피해1/돌진7.5u/s, Scene BossEncounter introDuration1.8s/introDropHeight/introShake, B21HazardReturn.asset fadeSeconds0.15s/getUpTime0.45s. Play 종료 전 기록→Edit 모드에서 명시 저장→재빌드. 기존 복귀3값·몸1.6·모서리 꺼짐은 유지한다.

빌드 정리: B23 최신·B22 직전만 보존. B21 슬롯20개는 [보존 SHA](_raw/23/retired-B21-slots-manifest.json)·raw/23/retired-B21-slots에 복사 후 오래된 빌드만 삭제했다([원문](_raw/23/housekeeping.txt)). 원본 검증 로그 삭제0. Ready Queue0, 실행 Task11개(T0~T9·BL1). 시작 이전 커밋3개를 보존하여 squash하지 않았다. push 없음.

Ruling: Archive는 허용 범위 밖이라 과거 표를 옮기지 않고 현재 이전 Task 표·B22 요약으로 연결했다. 이번 종료 요약은 재개 결과이며 기존 B23 시작 차단 요약은 덮어쓰지 않았다.

T8 보완 입력이 연속 실패하여 Batch 재시도 규칙대로 해당 일반 적 전투 입력 보완만 제거하고 T7 시점 검증 소스로 복원했다. 게임 구현·다른 Task 결과는 보존했다. 실험 입력의 실패 원문과 당시 소스 커밋 `665753e`는 남겼다. 최종 배포는 복원된 소스로 build8-final에서 다시 빌드했다.
