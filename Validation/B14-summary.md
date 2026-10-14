# Batch 14 결과

시작: 2026-10-14 21:4x KST, 시작 HEAD 2de8d26 (= origin/main, 태그 B12 존재, 트리 깨끗, Unity 미실행 확인).
종료: 2026-10-15 01:0x KST. 실행: Claude Code. push 안 함.

<!-- CARD START -->
```text
BATCH CARD 2de8d26..HEAD (7c9090e)
변경 파일: 75 (+400 / -33 줄, 바이너리 0)
  Scripts: 70
  Scenes: 0
  Prefabs: 0
  Editor: 1
  Validation: 1
  문서: 2
  기타: 1
허용 범위: Assets/Scripts, Assets/Editor, Assets/ScriptableObjects/BossTuning.asset, README.md, TASKS.md, Validation/B14-summary.md
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 18 / REVIEW 0 / DOING 0 / BLOCKED 1 / PARTIAL 8
결정 대기(## 2. 결정 대기 표의 D- 행): 0
열린 버그(## 4. 열린 버그 표 행): 4
직접 플레이 확인 대기(## 3. 목록 항목): 5
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 13/13 (Validation/_raw/regression/B14-final-default/result.txt)
```
카드의 HEAD는 합치기 전 T4 커밋(7c9090e)이며, 합친 커밋의 파일 내용은 카드 삽입 외 동일.
<!-- CARD END -->

## 사람이 할 일
- [Play-V4-BeforeBoss](../Launchers/current/Play-V4-BeforeBoss.cmd) **10분**: 인자 없는 기본 보스가 이제 B3다. 이어하기 → C04 오른쪽 문 → C06. 8패턴 예고·회피, HP 절반에서 2페이즈 전환을 본다. 처치 후 Slam → 천장 → S01 엔딩까지 이어서 확인.
- [Boss-B2-fast](../Launchers/current/Boss-B2-fast.cmd) **2분**: 이전 4패턴 보스가 그대로 나오는지만 확인.
- [Play-V4](../Launchers/current/Play-V4.cmd): 기존 D-NEXT(새 게임→엔딩 전체 플레이)는 그대로 대기. 이번에 바뀐 플레이 내용은 보스 하나뿐이다.
- 결정 대기 0. Task 3의 "못 잡는 고장" 16개를 보고 다음 배치에 측정 검사를 넣을지 정하면 된다.

## Task 표
| Task | 상태 | 커밋(합치기 전) | 결과 | 원본 |
|---|---|---|---|---|
| 0 | DONE | 없음(저장소 밖) | Codex 금지 규칙 3줄 추가, 기존 내용 유지 | `C:\Users\gosu3\.codex\rules\default.rules` |
| 1 | DONE | dfe908b | 기본 보스 = B3. Quiet 13/13. B12 B3 검사 13종 PASS 줄 수 전부 ≥ B12. 인자 없이 C06_V4 진입 로그로 B3 확인. 처치→엔딩 PASS | `_raw/B14/T1-*`, `_raw/regression/B14-T1-quiet` |
| 2 | DONE | cc8b988 | Verification 69파일 `#if UNITY_EDITOR \|\| DEVELOPMENT_BUILD`. Release 빌드 errors0, 검증 타입 0/73. 개발 빌드 런처5 PASS. Quiet 13/13 | `_raw/B14/T2-*`, `_raw/regression/B14-T2-quiet` |
| 3 | DONE(측정) | 없음(측정만) | 23항목 47회 실행, 건너뜀 0. 잡음 7 / 못 잡음 16 | `_raw/B14/T3-sweep.txt`, `_raw/regression/B14-T3-*` |
| 4 | DONE | 합친 커밋 | 기본 회귀 13/13, 카드, B14 하나로 합치고 태그 | `_raw/regression/B14-final-default` |

## Task 0 — default.rules 최종 내용(원문)
```text
prefix_rule(pattern=["C:\\Users\\gosu3\\.cache\\codex-runtimes\\codex-primary-runtime\\dependencies\\native\\powershell\\pwsh.exe", "-Command", "& 'C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe' -batchmode -nographics -projectPath . -executeMethod B05Rooms.Run -logFile Validation/_raw/B05/rooms-editor.log"], decision="allow")
prefix_rule(pattern=["Stop-Process", "-Id", "16540"], decision="allow")
prefix_rule(pattern=["C:\\Users\\gosu3\\.cache\\codex-runtimes\\codex-primary-runtime\\dependencies\\native\\powershell\\pwsh.exe", "-Command", "& ./Validation/Run-Regression.ps1 -Tag B05base -SkipPogo"], decision="allow")
prefix_rule(pattern=["C:\\Users\\gosu3\\.cache\\codex-runtimes\\codex-primary-runtime\\dependencies\\native\\powershell\\pwsh.exe", "-Command", "& ./Tools/Build-B10Release.ps1 -Tag B10-release-r60"], decision="allow")
prefix_rule(pattern=["C:\\Users\\gosu3\\.cache\\codex-runtimes\\codex-primary-runtime\\dependencies\\native\\powershell\\pwsh.exe", "-Command", "$taskPlan=Get-Content Validation/_raw/B13/A2-moves.txt | Where-Object {$_ -ne 'B10-geometry-report.md'}\nforeach($taskName in $taskPlan){$taskSource='Validation/'+$taskName;$taskDest='Archive/validation-2026-10/'+$taskName;if(Test-Path -LiteralPath $taskDest){throw \"Preserve existing $taskDest\"};$taskTracked=@(git ls-files -- $taskSource);if($taskTracked.Count){git mv -- $taskSource $taskDest;if($LASTEXITCODE){throw \"git mv $taskSource\"}}else{Move-Item -LiteralPath $taskSource -Destination $taskDest}}\nNew-Item -ItemType Directory -Force Validation/Routes/V2 | Out-Null\n'Historical inputs moved to Archive/validation-2026-10/Routes/V2; directory preserved for LEVEL_V2 link (outside B13 scope).' | Set-Content Validation/Routes/V2/moved-result.txt"], decision="allow")
prefix_rule(pattern=["git", "push"], decision="forbidden", justification="push는 사용자가 직접 한다")
prefix_rule(pattern=["git", "reset", "--hard"], decision="forbidden")
prefix_rule(pattern=["git", "clean"], decision="forbidden")
```

## Task 1 — D-B3 적용
- 기본값이 정해지는 곳: `BossCandidateSelection.Awake`에서 `-boss=` 인자가 없으면 id=`base` → 씬 `BossEncounter.tuning`을 쓴다. C06_V4·C06 씬은 `Assets/ScriptableObjects/BossTuning.asset`을 참조한다. 그래서 B12 T1(B2-fast 채택)과 같은 방식으로 **기본 Asset 값만 B3 값으로 교체**했다(씬·런처 변경 0). 이전 값 백업: `_raw/B14/T1-default-before.asset`.
- B2-fast는 `Assets/BossCandidates/B2-fast` 그대로. `-boss=b2fast` 실행 로그: `BOSS candidate=b2fast BossTuning=BossTuning extendedPatterns=True b3Patterns=False patterns=B2-fast cycle`.
- 회복값은 확인만(변경 0): 기존4 recovery 0.8, chase 1.2, comboLeap 1.5, backshot 1.2, falling 1.2 — 결정과 같음. 참고: B3 Asset의 damage는 1(B2-fast는 2). 사용자가 플레이한 B3 값을 그대로 채택했다.
- 같이 바꾼 것: `B12BossAssets.Build`의 "기본 = B2-fast" 확인을 "기본 = B3"로, `b3Patterns` Tooltip 문구, README 현재 상태 2줄·런처 설명 2줄, TASKS(D-B3 결정 기록 10/14, rec10 취소, D-COMBAT/AV2 DEFERRED).
- 새 검사: `B04BossVerifier -b14-defeat`(인자 없는 기본 보스 → HP1 fixture → 실제 X로 처치 → Slam → S01 정상 → 엔딩 저장). HP 설정·S01 진입은 fixture이며 자연 승리 검사가 아니다.
- 인자 없는 C06_V4 진입 로그(모든 실행 공통): `BOSS candidate=base BossTuning=BossTuning extendedPatterns=True b3Patterns=True patterns=melee,chase,leap,backshot,wave,comboLeap,falling,charge`
- 빌드: `Builds/B12-delivery`에 같은 이름으로 다시 빌드(런처 경로 유지). slots 폴더 보존(백업 `_raw/B14/T1-slots-backup`).

B12 대비 PASS 줄 수(`_raw/B14/T1-pass-compare.txt`, 기본 보스·`-boss` 인자 없음, 형식 PASS/FAIL):
```text
B12 vs B14-T1 (default boss, no -boss arg) PASS line counts
T3-60 B12=8/2 B14=8/2 ge=True
T4-60 B12=25/0 B14=27/0 ge=True
T4-30 B12=7/0 B14=7/0 ge=True
T4-120 B12=7/0 B14=7/0 ge=True
T5-60 B12=36/0 B14=36/0 ge=True
T5-30 B12=8/0 B14=8/0 ge=True
T5-120 B12=8/0 B14=8/0 ge=True
T6-60 B12=30/0 B14=30/0 ge=True
T6-30 B12=7/0 B14=7/0 ge=True
T6-120 B12=7/0 B14=7/0 ge=True
T7-phase B12=17/0 B14=17/0 ge=True
T8-cycle B12=9/0 B14=9/0 ge=True
T10-p2 B12=9/0 B14=9/0 ge=True
```
T3는 B12와 같은 PARTIAL(추적 회피 지형 간섭, 열린 버그 B12-CHASE-VERIFY 그대로).

## Task 2 — 검증 코드 배포판 분리
C1 게임 코드 → Verification 호출 지점(파일·줄):

| 게임 코드 | 줄 | 쓰는 Verification 타입 | 처리 |
|---|---|---|---|
| B09LegacyMode.cs | 5 | N5Args | N5Args는 감싸지 않음 |
| BossCandidateSelection.cs | 10, 11 | N5Args | 〃 |
| BossEncounter.cs | 36 | N5Args | 〃 |
| GuidePresentation.cs | 7 | N5Args | 〃 |
| PogoTestCourse.cs | 18, 34 | N5Args | 〃 |
| V3SceneMap.cs | 8 | N5Args | 〃 |
| V4Launch.cs | 6, 7 | N5Args | 〃 |
| V4SceneMap.cs | 4 | N5Args | 〃 |
| WallJumpTestCourse.cs | 32 | N5Args | 〃 |

- 게임 코드가 쓰는 것은 런처 인자 해석기 `N5Args`뿐이다. `N5Runner.cs` 안에서 `N5Runner` 클래스만 감싸고 `N5Args`는 밖에 두었다. 나머지 68파일은 파일 전체를 감쌌다. 게임 코드 쪽 변경은 0.
- 씬 직렬화 참조: `V3RoomRoutes`가 V3 씬(A01_V3 등)에 붙어 있다. Release에서는 이 컴포넌트가 빠진다(V3 씬을 열 때 missing script 경고 가능). Release 빌드 자체는 warnings=0.
- Release 확인 방법: 빌드된 `Assembly-CSharp.dll`에서 Verification 타입 이름 73개를 문자열로 검색해 개발 빌드와 비교. 결과 `types=73 in_release=0 in_dev=72`, `N5Args release=1 dev=1`. dev에서도 0인 1개는 원래부터 `#if UNITY_EDITOR`였던 B06BreakVerifier. DLL 크기 release 246,784 / dev 751,104 bytes. 근거 `_raw/B14/T2-release-types.txt`.
- C4 Release: `B10Build.Run -b10-build B10-B14release` → `Succeeded errors=0 warnings=0`. `-v4 -v4-new-game`로 15초 실행: 저장 `visitedRooms:[A01]`, `checkpointId CP-A01`, 로그 Exception/Error 0줄. 확인 후 `Builds/B10-B14release` 삭제.
- C3 개발 빌드(Development) 런처 smoke: Play-V4, Boss-B2-fast, Boss-B3, Play-V4-BeforeBoss, Play-V4-AfterEnding 모두 첫 줄 PASS, `Error/Exception/Assert=0`. 기존 시작 검사이며 10초 연속 측정은 아니다.

## Task 3 — 회귀 테스트의 고장 탐지 범위(측정만)
제한값: `[Range]` 23개(Inspector 슬라이더 제한), 그중 11개는 `OnValidate`에서 `Mathf.Clamp`도 한다. `respawnDelay`는 `[Min(0)]`. 각 항목을 범위 안의 끝값 하나로 바꾸고 기본 13 Quiet 회귀를 돌린 뒤 원래 값으로 되돌렸다(47회, 재시도·건너뜀 0). 실행 후 `git diff Assets/ScriptableObjects/PlayerTuning.asset` 없음.

| 항목(기본값) | 범위 | OnValidate | 하한 결과 | 상한 결과 | 판정 |
|---|---|---|---|---|---|
| moveSpeed (6) | 5~7 | O | S1-04 FAIL | S1-04 FAIL | 잡음 |
| acceleration (80) | 60~100 | O | 13/13 | 13/13 | **못 잡음** |
| deceleration (100) | 80~120 | O | 13/13 | 13/13 | **못 잡음** |
| airControl (0.9) | 0.8~1 | O | 13/13 | 13/13 | **못 잡음** |
| jumpVelocity (12) | 11~13 | O | 13/13 | 13/13 | **못 잡음** |
| doubleJumpVelocity (12) | 11~13 | X | 13/13 | 13/13 | **못 잡음** |
| gravity (30) | 26~34 | O | 13/13 | 13/13 | **못 잡음** |
| fallGravityMultiplier (1.5) | 1.2~1.8 | O | 13/13 | 13/13 | **못 잡음** |
| maxFallSpeed (18) | 16~22 | O | 13/13 | 13/13 | **못 잡음** |
| jumpCutMultiplier (0.5) | 0.4~0.7 | O | 13/13 | 13/13 | **못 잡음** |
| coyoteTime (0.1) | 0.06~0.14 | O | S1-04 FAIL | S1-04 FAIL | 잡음 |
| jumpBuffer (0.12) | 0.08~0.16 | O | S1-04 FAIL | S1-04 FAIL | 잡음 |
| dashSpeed (18) | 16~20 | X | T1 FAIL | 13/13 | 잡음(하한만) |
| dashDuration (0.16) | 0.14~0.2 | X | T1 FAIL | T1 FAIL | 잡음 |
| dashCooldown (0.45) | 0.35~0.6 | X | 13/13 | T1 FAIL | 잡음(상한만) |
| wallSlideMaxFallSpeed (2.5) | 1~5 | X | 13/13 | 13/13 | **못 잡음** |
| wallJumpHorizontalSpeed (8) | 6~11 | X | 13/13 | 13/13 | **못 잡음** |
| wallJumpVelocity (12) | 10~13 | X | 13/13 | 13/13 | **못 잡음** |
| wallJumpInputLock (0.15) | 0.08~0.25 | X | 13/13 | 13/13 | **못 잡음** |
| wallCoyoteTime (0.08) | 0.04~0.12 | X | 13/13 | 13/13 | **못 잡음** |
| wallJumpShortHorizontalSpeed (8) | 0~11 | X | 13/13 | 13/13 | **못 잡음** |
| wallJumpShortInputLock (0.15) | 0~0.25 | X | 13/13 | 13/13 | **못 잡음** |
| respawnDelay (0.6) | ≥0 | X | S1-08, T2 FAIL | (상한 없음) | 잡음 |

- 합계: 잡음 7 (moveSpeed, coyoteTime, jumpBuffer, dashSpeed, dashDuration, dashCooldown, respawnDelay), 못 잡음 16. dashSpeed 상한·dashCooldown 하한은 한쪽 방향만 잡는다.
- 잡는 이유도 주의: S1-04는 6 u/s, 코요테 0.08/0.12 탐침처럼 **기본값을 그대로 박은** 검사다(`Assets/Editor/N1Verification.cs` 253행 등). 그래서 디자이너가 값을 정상 조정해도 FAIL이 난다(실제 고장과 구분 못 함).
- 제한값 자체 검사(4번): 기본 13 회귀와 Assets/Editor에 "범위 밖 값 → 경계값 고정"을 확인하는 검사는 없다(코드 검색 결과). 코드 읽기 기준: OnValidate 클램프는 11개만 있고, 나머지 12개(doubleJumpVelocity, dash 3개, wall 8개)는 `[Range]`뿐이라 Inspector 슬라이더 밖(텍스트 편집·스크립트 대입)에서는 제한되지 않는다. 실행 측정은 하지 않았다.

제안(테스트 추가는 하지 않음). 기대치는 "설정 Asset 값에서 계산"으로 두어 정상 조정에는 PASS, 동작 고장에는 FAIL이 나게 한다. 허용 오차는 60fps 물리 1 tick(0.02s) 기준:

| 항목 | 측정 | 허용 오차 |
|---|---|---|
| acceleration / deceleration | 정지→최고속(최고속→정지) 도달 시간 = speed/accel | ±1 tick |
| airControl | 공중 방향 전환 시간 = 2·speed/(accel·airControl) | ±1 tick |
| jumpVelocity / doubleJumpVelocity | 이륙 직후 vy, 정점 높이 v²/(2g) | vy ±0.05 u/s, 높이 ±2% |
| gravity / fallGravityMultiplier | 상승·하강 구간 vy 변화율 | ±2% |
| maxFallSpeed | 긴 낙하의 최종 vy | ±0.05 u/s |
| jumpCutMultiplier | 버튼 뗀 순간 vy 비율 | ±0.01 |
| wallSlideMaxFallSpeed | 벽 미끄럼 최종 vy | ±0.05 u/s |
| wallJumpHorizontalSpeed / Velocity / Short* | 벽 점프 직후 (vx, vy) | ±0.05 u/s |
| wallJumpInputLock / ShortInputLock | 반대 입력이 먹히기 시작한 시각 | ±1 tick |
| wallCoyoteTime | 벽 이탈 후 (값−1tick)에 점프 됨, (값+1tick)에 안 됨 | 경계 ±1 tick |
| dashSpeed 상한·dashCooldown 하한 | 대시 거리 = speed×duration, 재대시 허용 시각 = cooldown | 거리 ±speed×1tick, 시각 ±1 tick |
| 이미 잡는 7개(S1-04·T1) | 박힌 기본값 대신 Asset 값 기준으로 기대치 계산 | 현재 오차 유지 |
| 클램프 | OnValidate 대상: 범위 밖 값 대입 후 OnValidate → 경계값. Range만 있는 12개는 클램프 정책 결정 필요 | 정확히 경계값 |

## 실제 FAIL / MEASURE 원문
T1 T3-60(B12와 같은 PARTIAL):
```text
FAIL B04BossVerifier actual=exe
MEASURE B12 pattern=4 fps=60 dodge=False hitEvents=1 hp=3 DJ=0 peak=1.792
MEASURE B12 pattern=4 fps=60 dodge=True hitEvents=1 hp=3 DJ=0 peak=1.810
FAIL System.Exception: stand hit / intended dodge zero
```
T1 처치→엔딩 첫 시도(검사 도구 실수: 처치 후에도 "회복 중"인지 확인함. 실제로는 HP0 처치 성공):
```text
FAIL B04BossVerifier actual=exe
MEASURE default boss tuning=BossTuning HP=28 patterns=melee,chase,leap,backshot,wave,comboLeap,falling,charge
FAIL System.Exception: X damage occurred inside recovery
```
두 번째 시도: 180초 TIMEOUT, 증거 파일 없음. 원인은 엔딩 씬에서 Player가 없어진 뒤 `B04BossVerifier.Cleanup`의 NullReferenceException(`_raw/B14/T1-defeat2.log`). 같은 실행의 저장 파일은 `bossDefeated:true, hasSlam:true, endingReached:true, visitedRooms C04,C06,S01`. Cleanup에 null 확인을 넣은 세 번째 실행이 PASS(`_raw/B14/T1-defeat3.txt`). 검사 도구 실패이며 제품 실패가 아니라서 AGENTS 규칙대로 따로 취급했다. 지시문의 "1회 재시도"를 엄격히 보면 도구 수정 2회였다는 점을 그대로 적는다.

T1 처치→엔딩 최종:
```text
PASS B04BossVerifier actual=exe
MEASURE default boss tuning=BossTuning HP=28 patterns=melee,chase,leap,backshot,wave,comboLeap,falling,charge
PASS actual final X defeats B3; boss flag and Slam
PASS boss defeat -> ending saved
Error/Exception/Assert=0
```
B2-fast 선택 확인용 실행(의도한 음성 대조라 FAIL이 정상): `FAIL System.Exception: default boss is B3 without -boss`. `-boss=b2fast` 로그는 Task 1 참고.

T3 회귀 FAIL 줄은 위 표에 모두 있다(원문 `_raw/B14/T3-sweep.txt`).

## 미실시
- B3 자연(fixture 없는) 승리와 사람 손 회피는 자동화하지 않았다. 처치→엔딩은 HP1 fixture와 S01 진입 fixture를 썼다.
- T3 추적 회피(B12-CHASE-VERIFY)는 그대로 남아 있다.
- Release 빌드는 새 게임 A01 진입까지만 확인했다(전체 경로 아님). 런처 smoke는 기존 시작 검사이며 10초 연속 측정은 아니다.
- Task 3 클램프 동작은 코드 읽기만 했고 실행 측정은 하지 않았다.
- RouteAll·Pogo 경로 회귀는 기본 13 범위 밖이라 실행하지 않았다.
