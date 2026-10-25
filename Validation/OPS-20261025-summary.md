# OPS-20261025 종료 카드·검증 요약

아래 카드는 Task 커밋 `0d5db36` 시점이다. 이후 종료 기록은 허용된 TASKS·이 요약·Batch의 검증 대체 설명에 한정되며 최종 git diff/status를 별도 확인한다. 카드의 “마지막 REGRESSION 13/13”은 **과거 B20 기록**이다. 이번에는 게임 파일 변경이 없어 Unity/Run-Regression을 실행하지 않았고 운영 스크립트 모의8경로·문서/보호 검사를 수행했다. 상태 합계는 TASKS의 역사적 미종결 추적 표까지 포함한다. 이번 Current Tasks는 DONE3다.

```text
BATCH CARD bf9693a..HEAD (0d5db36)
변경 파일: 13 (+794 / -169 줄, 바이너리 0)
  Scripts: 0
  Scenes: 0
  Prefabs: 0
  Editor: 0
  Validation: 1
  문서: 10
  기타: 2
허용 범위: AGENTS.md, CLAUDE.md, .agents/skills/metro-handoff/SKILL.md, .claude/skills/metro-handoff/SKILL.md, TASKS.md, Archive/TASKS-B12-B19.md, Archive/TASKS-B10-operations-20261025.md, Archive/TASKS-B20.md, Batches/BATCH_TEMPLATE.md, Batches/BATCH-OPS-20261025.md, Tools/Preflight.ps1, Tools/protected-files.txt, Validation/OPS-20261025-summary.md, Validation/_raw/OPS-20261025/**
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 3 / REVIEW 11 / DOING 0 / BLOCKED 2 / PARTIAL 5
결정 대기(## 2. 결정 대기 표의 D- 행): 2
열린 버그(## 4. 열린 버그 표 행): 6
직접 플레이 확인 대기(## 3. 목록 항목): 7
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 13/13 (Validation/_raw/regression/B20-closing-default/result.txt)
```

# OPS-20261025 — 운영 개선 검증

Base B20 `bf9693a`, 기존 미푸시 커밋 2개 보존. 이번 게임 변경 없음. Task별 커밋 뒤 카드 확인 후 종료 기록만 추가했다.

| Task | 상태 | 결과 |
| --- | --- | --- |
| OPS-T1 | DONE | TASKS 원문 정규화 비교 일치·과거 미종결18행 유지·7문서 링크불량0·두 skill frontmatter/원본 경로 확인. 원문: `_raw/OPS-20261025/T1-docs.txt` |
| OPS-T2 | DONE | AGENTS·Batch 템플릿·보호 목록 정리. 숫자 없는 문서 Task 허용, 위험도별 검증, 위반 변경만 제거, Queue 승인 범위 합산, V5 FAIL 원문 판정 명시 |
| OPS-T3 | DONE | Preflight 구문오류0·모의8경로 PASS·보호387 SHA변경0·문서 보관/링크 검사 PASS. 실제 Unity 미실시 |

미실시: Unity 기동·컴파일·게임 회귀·플레이. 이번 작업은 게임 소스·에셋·설계를 바꾸지 않는다.

T1 도구 한계: skill-creator quick_validate.py는 `ModuleNotFoundError: No module named 'yaml'`로 두 진입 파일 모두 실행 실패. 패키지 추가 없이 이번의 단순 name/description frontmatter와 원본 참조를 직접 검사로 대체했다. Claude Code 실제 /skills 표시 및 새 Codex 세션 자동 발견은 미실시. Codex 저장소 skill 위치는 [공식 문서](https://learn.chatgpt.com/docs/customization/overview)로 확인했다.

Ruling: 운영 작업 식별자는 OPS-20261025 / 게임 Batch21과 승인을 혼동하지 않기 위함 / 다음 게임 배치 번호에 영향 없음.

T2 확인: 기본 보호 패턴은 Design·PRD·GAME_DESIGN·Tuning asset/meta·Player prefab/meta·ProjectSettings이며, B20의 당시387개 스냅샷과 구분했다. 보호 파일이 필요하다는 이유만으로 안전한 Task 결과를 전부 되돌리지 않는다. Ready Queue는 승인된 제품 일감이 없어 0건 유지. P8·PRD 변경·README 정리는 제외했다.

## T3 실제 검증과 한계

원문: `Validation/_raw/OPS-20261025/T3-preflight-tests-final.txt`。

```text
PASS PowerShell parse errors=0
PASS skip-unity exit=0 status=SKIP_UNITY
PASS mock-success exit=0 status=PASS
PASS mock-license-198 exit=198 status=LICENSE_BLOCKED
PASS mock-other-error exit=1 status=UNITY_ERROR
PASS mock-timeout exit=124 status=TIMEOUT
PASS missing-executable exit=1 status=ERROR
PASS sandbox-localappdata exit=1 status=ERROR
PASS mock-project-open exit=3 status=PROJECT_OPEN
MEASURE actualUnityRuns=0; mocked process exits only; existing Editor was not stopped
```

- 실제 -SkipUnity 실행: Git·LOCALAPPDATA·Hub 정보 기록, Unity 미실행. 샌드박스 LOCALAPPDATA 리디렉션을 확인하고 Unity 경로에서는 사전 차단하도록 구현했다. Hub 프로세스0은 단독 실패 조건이 아니다.
- 매번 고유 preflight 폴더·preflight.txt·result.json·Unity 로그를 보존하며 기존 결과를 덮어쓰지 않는다. exit198은 그대로 반환하고 다른 Unity 오류·timeout과 구분한다. timeout은 자신이 시작한 프로세스만 종료한다.
- 검증 도구 첫 실행은 리디렉션된 stdout의 한글 경로가 잘못 해석되어 결과 JSON 조회에 실패했다. 제품 결과 파일은 정상 보존돼 있었으며, 도구만 새 evidence 폴더 탐색으로 바꿔 재검증했다. 최초 실패·재실행 폴더는 _raw에 보존했다.
- 원문 `closing-protection.txt`: `MEASURE protectedFiles=387 MEASURE protectedSHAChanges=0` (PowerShell 출력 한 줄). 문서7개 링크불량0·과거 TASKS 전체 원문 비교 일치·미종결18행 추적 유지.
- 미실시: 실제 Unity 라이선스·컴파일·13회귀·V5 물리·실제 Editor 프로젝트 잠금·실제 timeout 종료. 모의 PASS는 이 항목들의 통과를 뜻하지 않는다. 게임 파일을 바꾸지 않아 게임 회귀를 실행하지 않았다.
- 미실시: Claude Code /skills 및 새 Codex 세션의 자동 선택. 두 SKILL.md의 frontmatter·원본 경로만 확인했다.

Ruling: 실제 Unity 기동 대신 모의 프로세스로 종료 코드별 분기를 검증 / 라이선스 환경을 다시 조사하거나 게임 실행 없이 운영 스크립트의 제어 흐름 확인 / 실제 Unity 연동은 다음 needs-unity=yes Batch 시작 시 별도 확인 필요.

## 모의 실패 원문 (의도한 주입, 실제 Unity 실패 아님)

```text
FAIL same-project Unity already open; startup NOT RUN; non-Unity tasks may continue
FAIL preflight LOCALAPPDATA missing, relative, nonexistent, or sandbox redirected; Unity NOT RUN; non-Unity tasks may continue after checking their dependencies
FAIL preflight Unity executable missing: C:\Users\gosu3\OneDrive\문서\ChatGPT\game1\Validation\_raw\OPS-20261025\tests-20261025-233205-458978c0\not-present.exe; non-Unity tasks may continue after checking their dependencies
FAIL Unity startup timeout; only spawned preflight process stopped; non-Unity tasks may continue
MEASURE unityExitCode=7
FAIL Unity startup exit=7; cause not classified as licensing; non-Unity tasks may continue
MEASURE unityExitCode=198
FAIL Unity startup exit=198; Unity tasks BLOCKED(external dependency); do not retry unchanged environment; non-Unity tasks may continue
MEASURE unityExitCode=0
```

## 최종 인계

- 적용: P1~P7·P9·P10 및 검토 보완. P8·게임/설계·기본 수치 수정 없음.
- 커밋: 지시문 f780a27, T1 a0ee60a, T2 db7c23e, T3 0d5db36, 종료 기록은 git log -1 참조. 기존 B20 두 커밋 보존. push·squash·태그 변경 없음.
- 실제 검증: 기본 검사기 대신 두 frontmatter 직접 확인, 원문 Archive 완전성·미종결18행·문서 링크·PowerShell 구문·모의8경로·보호387 SHA0·카드 범위밖0/깨진링크0/없는런처0.
- 문제/한계: 새 skill의 앱 자동 발견과 실제 Unity 연동은 미실시. 다음 Unity Batch 시작 시 Preflight 실제 실행으로 확인한다. 기존 게임 결정2건과 버그6건은 변경하지 않았다.
- Ready Queue: 승인 일감0건. 시간이 남아도 새 제품 작업을 만들지 않고 종료했다.
