```text
BATCH CARD — Base 0c45a1a..최종 B11 (squash는 파일 내용 불변)
변경 파일: 15 (+331 / -37 줄, 바이너리 0)
  Scripts: 3
  Scenes: 0
  Prefabs: 0
  Editor: 1
  Validation: 1
  문서: 2
  기타: 8
허용 범위: Assets/Scripts, Assets/BossCandidates/B3, Assets/Editor, Launchers/current, Tools/Run-Regression.ps1, Builds, Validation, TASKS.md, README.md
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 15 / REVIEW 0 / DOING 0 / BLOCKED 2 / PARTIAL 7
결정 대기(## 2. 결정 대기 표의 D- 행): 1
열린 버그(## 4. 열린 버그 표 행): 4
직접 플레이 확인 대기(## 3. 목록 항목): 6
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 13/13 (Validation/_raw/regression/B11-final-quiet/result.txt)
```

# Batch 11 — 2026-10-12~13

## 사람이 할 일

- 기존 TASKS 직접 플레이 목록을 먼저 확인한다. 보스 선택은 아래 4개를 각 **3~5분** 비교한다. 모두 **AV1 켜짐**, 타이틀에서 Z 이어하기 → CP-C04, 대시·벽 점프·더블 점프 보유.
- [Base](../Launchers/current/Boss-Base.cmd): 기본2패턴 비교 기준. [B2](../Launchers/current/Boss-B2.cmd): 기존4패턴. [B2-fast](../Launchers/current/Boss-B2-fast.cmd), [B2-slow](../Launchers/current/Boss-B2-slow.cmd): 예고·속도 비교. 예고 가독성, 뒤로 대시/점프 회피, 회복 공격 기회를 본다.
- **D-BOSS-PICK**: 네 후보 중 선택 또는 보류. AI 추천은 Base를 기준으로 위협과 회피 예고가 명확한 후보를 고르는 것. 기본값 채택은 사용자만 결정한다. 히트스톱·Q6 등은 기존 단계 보류 유지.
- [BeforeBoss](../Launchers/current/Play-V4-BeforeBoss.cmd) **3분**: AV1 끔, CP-C04와 능력3개, C06 진입. [AfterEnding](../Launchers/current/Play-V4-AfterEnding.cmd) **3분**: A01과 엔딩·보스 처치·Slam/능력 복원. 둘 다 **개발 fixture 저장**이며 자연 플레이 저장이 아니다.
- B3는 1회 재시도 후 실패 복원되어 실행/선택할 수 없다. B3의 위협·의도한 회피·6패턴/Pause/사망 복귀는 합격 주장 없음.

## 결과와 파일

| Task | 상태 | 작업 커밋 (최종 B11로 합침) | 주요 파일·실제 결과 |
|---|---|---|---|
| A1 | DONE | a13ee36, 3ce50c0, d0335b2 | BossCandidateSelection.cs, Verification/Editor/B03Build.cs. base/b2/b2fast/b2slow 선택과 이름·extendedPatterns 로그, 인자없음 base 유지. 누락 후보는 base 복귀 |
| A2 | DONE | a13ee36 | 같은2파일. 기존 AV1 Profile 참조를 runtime에 연결; -av1 시작 로그1줄. AV1 기본 Asset 미변경 |
| A3 | DONE | 0e07a4f, 3ce50c0, d0335b2 | Launchers/current/Boss-*.cmd, Start-V4.ps1. 후보4개 CP-C04·능력3개 복원, 정확한 후보 로그, fixture arena 진입 후10초 Error0 |
| A4 | DONE | 0e07a4f, 3ce50c0 | BeforeBoss.json/AfterEnding.json과 기존 StageVerifier. BeforeBoss CP-C04, AfterEnding A01+보스/엔딩/Slam 복원. 기존 자연 저장2개 유지 |
| C | BLOCKED / FAIL 복원 | da3c8a6 | B3 Asset/구현/런처 변경만 되돌림. 첫 실행 신규 필드 false, 재시도 ForceUpdate 뒤 import assert 실패. 범위·기본규칙 확대 없이 다음 트랙 진행 |
| B | DONE | 616b252 | Tools/Run-Regression.ps1 -Quiet. 기본13/13·Quiet13/13, 아래 검사별 비교 일치. 기본 옵션 실행 방법 유지 |
| BL1 | DONE | 701359b | README.md 삭제된 빌드 링크/경로만 삭제됨 표시. 깨진 Markdown 링크0. Builds에는 B10-B11-delivery와 B10-delivery만 유지 |

- Base: `0c45a1ac7776a8b38a0669e1ce614cb054210bd5`. 시작 작업 트리 깨끗함. 실행 중 Task별 로컬 커밋, 종료 시 이번 배치 커밋만 squash하고 B11 태그. push 안 함.
- 최종 빌드: `Builds/B10-B11-delivery/Afterglow.exe`, Development, CleanBuildCache. [빌드 원본](./_raw/B11/final-build.txt): `Succeeded errors=0 warnings=0`.
- 기본 Tuning·B2/fast/slow·AV1 Asset·씬 지형·PRD/GAME_DESIGN 미변경. 런처 별도 slots를 사용하고 실행 시 해당 fixture snapshot을 다시 복사한다.
- C04_V4에는 CP-C04 저장 Spawn이 없어서 `-boss-fixture`에서만 기존 Entry 위치에 runtime Spawn을 만든다. 기본 실행에는 적용되지 않는다.
- 후보 빌드 참조가 script-only 씬 캐시에 누락된 실행은 합격 근거에서 제외. CleanBuildCache로 재생성한 A-final 및 launcher-final만 근거로 사용한다.
- 검사 도구: A StageVerifier의 출구 중앙 placement는 V4ExitRule을 통과하지 못해 기존 RequestTransition fixture로 보정1회. **자연 출구 이동 PASS와 구분**한다. C 보스 검사 fixture/새필드 import 보정1회 뒤 중단.
- 조정 위치: `Assets/BossCandidates/B2`, `B2-fast`, `B2-slow`의 `BossTuning.asset`, AV1은 `Assets/Candidates/AV1/PresentationProfile.asset`. Play 종료 후 원본 Asset에 원하는 값을 명시 저장하고 재빌드한다.

## 런처 최종 실제 실행

공통 실행점 Start-V4.ps1 -Smoke를 각 런처와 같은 Mode/Boss/AV1 인자로 실행했다. 기존 StageVerifier로 저장·능력·room 복원을 검사하고 BeforeBoss는 arena10초를 추가했다. 로그의 이전 이름 `natural`은 이 검사에서는 fixture snapshot을 의미하며 자연 진행을 증명하지 않는다.

| 실행 | 결과·로그 |
|---|---|
| Boss-Base | [최종 원본](./_raw/B11/launcher-final-base.txt), [로그](./_raw/B11/launcher-final-base.log) |
| Boss-B2 | [최종 원본](./_raw/B11/launcher-final-b2.txt), [로그](./_raw/B11/launcher-final-b2.log) |
| Boss-B2-fast | [최종 원본](./_raw/B11/launcher-final-b2fast.txt), [로그](./_raw/B11/launcher-final-b2fast.log) |
| Boss-B2-slow | [최종 원본](./_raw/B11/launcher-final-b2slow.txt), [로그](./_raw/B11/launcher-final-b2slow.log) |
| BeforeBoss (AV1 끔, boss 인자없음) | [최종 원본](./_raw/B11/launcher-final-before.txt) |
| AfterEnding | [최종 원본](./_raw/B11/launcher-final-ending.txt) |

실제 원본 핵심 수치 (위 `natural` 표기는 fixture임):

```text
MEASURE natural Continue checkpoint=CP-C04 room=C04
MEASURE BossTuning=BossTuning extendedPatterns=False
MEASURE BossTuning=BossTuning extendedPatterns=True
MEASURE natural Continue checkpoint=CP-A01 room=A01
Error/Exception/Assert=0
```

A 초기 실패 원문도 보존했다. 씬 캐시/fixture를 보정한 후 최종 실행만 PASS 근거로 사용:

```text
FAIL System.Exception: Timeout room ready
Error/Exception/Assert=1
FAIL System.Exception: Timeout door C06
Error/Exception/Assert=0
```

## Quiet 검사별 비교

기본 B11-C와 Quiet B11-B-Quiet에서 같은 method·판정 조건을 실행했다. PASS 확인 줄 수는 첫 요약줄을 제외했으며 실제 타이밍 측정값이 완전히 같다는 뜻이 아니다. 13개는 모두 Unity Editor 검사여서 이 기본 회귀에는 별도 게임 exe 실행이 없다. Quiet의 그래픽 실행은640×360 창모드이며 렌더링 PNG가 필요한 Camera는 -nographics를 사용하지 않는다.

| 검사 | 기존 | Quiet | PASS 확인줄 기존/Quiet | 방식 |
|---|---|---|---|---|
| S1-02 | PASS | PASS | 19/19 | headless 채택 |
| S1-04 | PASS | PASS | 63/63 | headless 채택 |
| S1-07 | PASS | PASS | 71/71 | headless 채택 |
| S1-08 | PASS | PASS | 70/70 | headless 채택 |
| Camera | PASS | PASS | 76/76 | 그래픽 640×360 유지 |
| T1 | PASS | PASS | 36/36 | headless 채택 |
| T2 | PASS | PASS | 45/45 | headless 채택 |
| T3 | PASS | PASS | 13/13 | headless 채택 |
| T4 | PASS | PASS | 17/17 | headless 채택 |
| S2 | PASS | PASS | 43/43 | headless 채택 |
| F2R | PASS | PASS | 48/48 | headless 채택 |
| LOGR | PASS | PASS | 10/10 | headless 채택 |
| Boss | PASS | PASS | 3/3 | headless 채택 |

- A 종료: [REGRESSION 13/13](./_raw/regression/B11-A/result.txt).
- C 복원 종료: [REGRESSION 13/13](./_raw/regression/B11-C/result.txt).
- B Quiet 비교: [REGRESSION 13/13](./_raw/regression/B11-B-Quiet/result.txt).
- 최종 기본: [REGRESSION 13/13](./_raw/regression/B11-final-default/result.txt). 최종 Quiet: [REGRESSION 13/13](./_raw/regression/B11-final-quiet/result.txt). 최종 비교도13개 모두 PASS 확인줄 수 일치.

## C 실패 원문·미검증

[B3 첫 실제 exe 검사](./_raw/B11/C-b3.txt), [재시도 import 로그](./_raw/B11/C-build2.log):

```text
FAIL B04BossVerifier actual=exe
FAIL System.Exception: Timeout B3 pattern resolution
Error/Exception/Assert=0
BuildFailedException: B3 six-pattern asset must be imported before building
```

원본 HIT 줄은 기존 패턴에서 발생했으므로 B3의 위협 PASS로 세지 않는다. B3 신규 boolean이 false로 로드되는 원인은 미확정이다. 1회 재시도 한도에 따라 모든 C 변경을 복원했다. B3 새 패턴 각각의 60fps 위협/피해0 회피, 6패턴1바퀴, B3 Pause/사망 복귀는 **미검증**. 원본 로그는 삭제하지 않았다.

기존 수락 대기·자연 전체 플레이·패드 체감은 이번 기술검사의 합격 범위 밖이며 TASKS 목록 유지. 다음 Batch는 D-BOSS-PICK 및 기존 전체 플레이 결과 반영, B3 import 원인 해결부터 별도 승인.


