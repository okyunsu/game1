# Batch24 — draft5·조각 성장·벽 감시자

종료 카드(T14 커밋 `cdbfa3f` 기준, 마지막 요약 파일을 더하기 전). 최종 커밋 포함 카드는 `_raw/24/batch-card-final.txt`에 보존한다. 표의 TASKS 상태는 과거 유지 표를 포함한 전체 집계다.

```text
BATCH CARD 88f458e..HEAD (cdbfa3f)
변경 파일: 54 (+6043 / -1825 줄, 바이너리 0)
  Scripts: 32
  Scenes: 8
  Prefabs: 0
  Editor: 5
  Validation: 0
  문서: 3
  기타: 6
허용 범위: Assets/Scripts/*, Assets/BossCandidates/B2/*, Assets/Scenes/Test/*, Assets/Scenes/V5/*, Launchers/current/*, Tools/*, Validation/*, TASKS.md, README.md, PRD.md
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 47 / REVIEW 20 / DOING 0 / BLOCKED 5 / PARTIAL 13
결정 대기(## 2. 결정 대기 표의 D- 행): 5
열린 버그(## 4. 열린 버그 표 행): 5
직접 플레이 확인 대기(## 3. 목록 항목): 13
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 13/13 (Validation/_raw/regression/B24-final/result.txt)
```

종료 카드와 보호 검사는 아래에 기록했다. Base `88f458e`(승인된 Claude 설계 선행 커밋), 로컬 origin/main은 fetch하지 않은 참조다. 기존 B23 16커밋을 보존하고 push하지 않았다.

## 결과

새 플레이 범위: A03 건물 거리, A01 대시 감실과 조각, A01 E4/A02·A05 E2, A06 접근 수정, 영구 조각 수집·4개마다 최대HP+1, 벽 점프 획득 후 싸우는 B2 시험장. 사용자 직접 지시로 P3를 승인했으며 지시문의 보류 표기보다 우선한다. 방 좌표·이동 수치·B1/B3 기본값은 변경하지 않았다.

| Task | 상태·커밋 | 변경·직접 검증 |
| --- | --- | --- |
| T0 | DONE `eab5ab9` | TASKS 전달 피드백·명시 P3 승인 |
| T1 | DONE `c75ecb0` | V5 6 Scene 재생성. 정적 FAIL0/WARN27(주로 A03 건물), Scene FAIL0. E4/E2·보상 요소 일치 |
| T2 | DONE `815107f` | A03 4·A02 5·A04 2·A06 3구간, 각 머리0/5·착지5/5. A02 조각 P3는 코요테 이륙 레시피 조건부, 보통 이륙은 머리5/5 실패 |
| T3 | PARTIAL `fa6d76c` | 실제 고리 A04 하부 포고→B1→대시→도랑→A03 건물/W옥상까지. 반환 승강기 재탑승 실패, 연습/그 뒤 Continue 잔여 |
| T4 | DONE `c7cdca7` | 감실 무능력0/5·대시5/5·실제 조각 수집 |
| T5 | DONE `6814ed8` | E4 포고1·피해0, A02 E2 점프5/5·A05 E2 점프대시5/5·피해0 |
| T6 | BLOCKED `42f31fe` | T3 후반 선행 실패로 A01 왕복 미진입. 반환 승강기 입력2회 실패·실험 제거, 결과 보존 |
| T7 | DONE `5669bf8` | PRD 지정3문장만 정확 패치 |
| T8 | DONE `371946c` | 실제4조각 maxHP4→5/HP3→5·안내1.500s·ID/bonus 즉시저장·중복거절·Continue·재진입숨김·새게임·구version1 호환 Error0 |
| T9 | DONE `b1d8a98` | 보스별 양방향·첫예고·패턴순서 선택 필드, 기본값은 기존 유지. B1/B3 cycle·phase·사망 PASS |
| T10 | DONE `8a0c6dd` | 새 B2 Scene·개발 런처·동일방 체크포인트 재시작. 실제 획득→3u 이탈 잠금·인트로1.800~1.817s. 비활성 RoomExit의 트리거를 명시 차단 |
| T11 | REVIEW `118eb66` | 후보HP26/피해1·패턴2,3,7만 관측·페이즈전환0·파동 collider2.800u/양방향·첫예고1.000~1.017s |
| T12 | PARTIAL `878f734` | 바닥 무벽점프10/10 피격·벽점프 실입력 처치24.767s HP2·벽점프7/포고26, 사망복귀·획득유지·처치ID/재진입없음 PASS/Error0. 획득대 위 일반점프 회피 예외가 있어 설계 조건은 미충족 |
| T13 | DONE `60fbcef` | B2 포함 최종 Development 오류0/경고0·전체13/13·런처4개 Error0·기존 B3 처치→엔딩 PASS |
| T14 | DONE `cdbfa3f` | README·TASKS3/9 검수20~30분·고리→B2·질문/기록·슬롯·값조정·한계 안내 |

T2의 Inspector/Scene 접근은 Editor 전용 `-b24-editor`에서 AssetDatabase Scene 로드를 사용한다. ProjectSettings를 바꾸지 않고, 빌드는 기존 SceneManager 경로를 사용한다. B2 시험은 실제 V4 Player prefab을 사용하며 벽점프 소유를 실제 픽업으로 저장한다.

주요 변경 파일: T1 `Assets/Scenes/V5/*.unity`, T2/T3/T4/T5/T6 `Assets/Scripts/Verification/B22Probe*.cs`·`RoomSession.cs`(Editor Scene 로드), T8 `ProgressSave.cs`·`DiscoveryReward.cs`·`PlayerHealth.cs`·`GameHUD.cs`·`UITuning.cs`, T9 `BossTuning.cs`·`BossEncounter*.cs`, T10 `B24BossArena.cs`·`Dev/B24BossTestLaunch.cs`·`RoomExit.cs`·`Editor/B24BossAuthoring.cs`·`Assets/Scenes/Test/BossTest_B2.unity`, T11 `Assets/BossCandidates/B2/BossTuning.asset`, T12 `Verification/B22Probe.B24Boss.cs`, T13 `Editor/B22Authoring.cs`·`Launchers/current/*`. T7 PRD·T0/T14 TASKS/README.

## 원문 증거·실패와 한계

- 정적 [T1-static](_raw/24/T1-static.md), [Scene](_raw/24/T1-clearance/result.txt), [요소](_raw/24/T1-counts/result.txt). 정적 self-test PASS, 도랑 max-step 신규 측정 JSON은 제공하지 않았으며 정적 도달 후보는 실제 완주 증명이 아니다.
- [머리 실측](_raw/24/T2-head2-editor/result.txt)·[조각 코요테](_raw/24/T2-shard-coyote/result.txt). 일반 이륙의5/5 접촉을 코요테 입력의0/5로 숨기지 않는다. A02 P3 접근 설계 판단은 TASKS D-B23-GEOMETRY에 남겼다.
- [고리 실패](_raw/24/T6-revisit2-reboard/result.txt): 반환 승강기 `surface=13`인데 player feet는 약1.015. 처음 승강기는 탑승 성공하며 반환 호출 후 플레이어를 못 태우는 상태다. 제품 승강기 코드는 이번에 바꾸지 않았다. [최종 exe 고리](_raw/24/P5-loop-exe/result.txt)도 `MEASURE A03 lift up=True surface=13.000 feet=1.015 unlocked=True`, `FAIL System.Exception: player rides lift to endpoint`로 같은 지점 실패. B1 처치12.985s/HP3 및 A03 W옥상까지 PASS이며 전체 고리 시간·그 뒤 연습/선반/Continue·A01 왕복은 미실시다. 재시도 종료.
- [감실](_raw/24/T4-vault1/result.txt)·[적](_raw/24/T5-enemies1/result.txt)·[성장/호환](_raw/24/T8-fragments1/result.txt). 구 저장은 기존 version1이며 누락 조각 필드는 빈 배열/보너스0으로 읽는다. 최대HP 보너스는 수집 ID수/4에서 산출해 중복 성장하지 않는다.
- [B1](_raw/24/T9-B1-editor/result.txt), [B3 cycle](_raw/24/T9-B3-cycle/result.txt), [B3 phase](_raw/24/T9-B3-phase/result.txt).
- B2 최초 실패: [T10-T11-T12-patterns](_raw/24/T10-T11-T12-patterns/result.txt) 비활성 RoomExit에도 Unity 트리거가 전달되어 다른방으로 전환. `enabled` 검사로 수정했다. [두 번째](_raw/24/T10-T11-T12-patterns2-lock/result.txt) 높이1.000은 생성 프레임의 오래된 bounds 측정이었다. 고정 물리 프레임 뒤 실제2.800 확인. [T12-patterns3](_raw/24/T12-patterns3-physics/result.txt)의 미부착 Slam NullReference는 시험 fixture 문제, [T12-patterns4](_raw/24/T12-patterns4-noSlamFixture/result.txt)의 교대5/10은 지연삭제 전 이전 파동을 측정한 도구 오류였다. 제품 높이·좌표를 변경하지 않고 새 파동 생명주기를 기다려 분리했다.
- [최종 B2 패턴/10회](_raw/24/T12-patterns5-wave-lifecycle/result.txt):
  - `MEASURE B2 first tell=1.017 height=2.800 directions=-1,1`
  - `MEASURE B2 observed=2,3,7 phaseTransitions=0`
  - `MEASURE B2 floor trial=10 noWallJump=True jumped=True hit=True damage=1 peakFeet=3.523`
  - `MEASURE B2 pedestal counterexample noWallJump=True jumped=True hit=False damage=0 peakFeet=4.535`
  - `MEASURE B2 pedestal ordinary jump bypass=True; floor ten-hit evidence is not universal ability-gate proof`
- [첫 전투 실패](_raw/24/T12-fight1/result.txt): `MEASURE B2 fight seconds=24.033 defeated=False HP=0 wallJumps=8 pogos=24`, `FAIL System.Exception: actual wall-jump input defeats B2`. [두 번째 입력 전투](_raw/24/T12-fight2-chargeInput/result.txt): `MEASURE B2 fight seconds=24.767 defeated=True bossHP=0 HP=2 wallJumps=7 pogos=26`. 사망 fixture 뒤 실제 복귀 HP4로 시작, 전투 동안 HP·pose·보스·소유권 쓰기0. 패턴 관측용 HP100 fixture와 승리 증거는 별도 실행이다.

도구 실패 원문과 모든 실패 로그는 삭제·덮어쓰기하지 않았다. 시도별 새 폴더를 사용했다. Error0은 Unity Error/Exception/Assert 집계이며, 검증기의 `FAIL System.Exception` 판정과 구분한다. Release·실제 패드·사람 감각·Sprint Gate는 미실시다.

## 회귀·배포

- P1/P2/P3/P4 Quiet 각각13/13: `_raw/regression/B24-P1`~`B24-P4/result.txt`.
- [최종 기본 전체13/13](_raw/regression/B24-final/result.txt). 새 source에서13개 메서드를 실행, 과거 카드 REGRESSION 행을 통과 증거로 재사용하지 않음.
- [Development 빌드](_raw/24/P5-build/result.txt): `Succeeded errors=0 warnings=0`. 이번 빌드1회, B2 포함 Scene 목록 기록. `Builds/B24-delivery/Afterglow.exe`.
- smoke [Loop](_raw/24/smoke-loop.txt)·[B1](_raw/24/smoke-boss1.txt)·[B2](_raw/24/smoke-boss2.txt)·[V4](_raw/24/smoke-v4.txt), 모두 exit0/Error0. V4 실제 Continue CP-C04→기존 보스 아레나 확인.
- [기존 B3 최종 exe](_raw/24/P5-B3-ending/B3Ending.txt): 회복 상태 HP1 fixture에서 실제 마지막 X→처치/Slam→S01/엔딩 저장 PASS/Error0. 자연 B3 완전 전투 증거로 바꾸지 않는다.
- 빌드 정리: 최신 B24/직전 B23만 보존. B22의13개 저장 슬롯은 `_raw/24/B22-retired-slots/`, 파일 목록은 `B22-retired-manifest.txt`에 보존한 뒤 검증한 절대 경로의 이전 빌드를 삭제했다. `_tmp`는 비어 있음.
- Preflight `_raw/24/preflight-20261101-001940-843-e4f5a850` PASS, hubLicensingClient=True/Unity exit0. 원본 Unity 로그의 라이선스 갱신 경고/문구도 보존했으며, 정상 실행 exit0·직접 결과 파일과 구분했다.

## 사전 결정·Ruling

- P3: 2026-11-01 사용자 직접 승인. PRD71/83/111 세 문장만 변경. PRD77의 과거 세션한정 기록은 보존하며 새 사용자 승인이 이번 영구 조각/HP의 근거다.
- Scene/수치 수정 없이 입력으로 검증: A04는 적을 끄지 않고 승인된 하부 포고 길을 선택. T2 조각길은 늦은 이륙의 유효성과 일반 입력의 실패를 함께 기록. 잘못된 경우 비용은 입력 시험 재수행이며 기본 기하에는 영향이 없다.
- B2 획득대 상면2u에서 일반점프 peak4.535가 파동 상단3.8u를 넘는다. 획득대 비활성/예외 허용 선택은 사용자 설계 결정이므로 자동 수정하지 않았다. D-B24-B2-GATE.
- 기존 B23 미푸시 커밋이 있으므로 soft reset/squash 조건이 성립하지 않아 Task별 커밋을 보존한다. push 없음, 종료 태그 B24.

## 종료 범위 확인

- T0~T14 총15 Task(상한24), DONE11/REVIEW1/PARTIAL2/BLOCKED1. T3/T6 미완료를 전체 완료로 바꾸지 않았다. Backlog 없음·Ready Queue0.
- 보호 스냅샷103개 중102개 동일, PRD만 승인3문장 예외. `git diff 88f458e`에서 Design/V4/B1/기본 튜닝/ProjectSettings/GAME_DESIGN 변경0. `.unity`·`.asset`·`.meta`는 Unity API/생성 결과만 사용했다.
- Batch 전체 diff-check는 Unity 직렬화 YAML/메타의 빈 값 뒤 공백과 검증 C# 2파일의 EOF 빈 줄 서식 경고를 보고했다(`_raw/24/batch-diff-check.txt`). 제품/컴파일 실패가 아니며 Unity 생성 파일을 텍스트로 정리하지 않았다. 미커밋 diff-check는0. 별도 최종 status/diff, README/TASKS/이 요약의 링크 검사 누락0. TASKS 결정5·버그5·과거 수락 순서 보존. 원본 로그는 raw에 남기고 커밋하지 않았다.
- 로컬 origin/main 대비 종료33커밋(이전 B23 16 + 이번 Claude 선행/Task15/종료1), Batch2개. fetch/push/soft reset 없음. 최종 HEAD는 `git log -1`, 태그 B24로 확인한다.
