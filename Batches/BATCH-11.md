이번 실행은 Batch 11만 수행한다. 무인 실행이다. AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다.
push는 하지 않는다(아침에 사용자가 한다).

## 시작 전 알아둘 것 (2026-10-12 저장소 정리)
- 루트 정리: 배치 지시문은 Batches/, 지난 설계 문서는 Archive/docs/. TASKS.md는 재구성됐고 원문은 Archive/TASKS-before-2026-10-12.md.
- Validation 대부분과 일회용 Editor 스크립트 34개는 Archive/validation-2026-10/, Archive/editor-oneoff/로 옮겼다. Archive의 .cs는 컴파일되지 않는다.
  트랙 C 확인에 B04 때 보스 검사 스크립트가 필요하면 Assets/Editor로 복사해 쓸 수 있다(git mv 복원). 이것은 검사 도구 보정 1회로 센다.
- 회귀는 Tools/Run-Regression.ps1(기본 13), 숫자 카드는 Tools/Batch-Card.ps1. 예전 배치별 Run-*.ps1은 쓰지 않는다.
- 런처는 Launchers/current만 쓴다. Builds/에는 B09-delivery, B10-delivery, B10-release-r78만 남아 있다(옛 빌드 삭제됨).
- PlaytestLogger: 자동 실행(batchmode 또는 -verify-* 인자)의 csv는 Validation/_raw/playtest/로 간다. 자동 실행에는 -verify- 인자를 붙인다.
- AGENTS.md Housekeeping, 결정 대기 상한 8개, 배치 종료 squash 규칙이 새로 들어갔다. 시작 전에 해당 절을 읽는다.

## Batch 11 — 보스 후보 체험·B3 후보·조용한 회귀
- Base 커밋: 시작 시 HEAD를 기록한다(B10 태그 이후 사용자 결정 반영 커밋 포함).
- 예산: 6시간 또는 Task 9개 중 먼저 도달하는 쪽. 트랙 순서 A → C → B → Backlog.
- 트랙은 서로 독립이다. 한 트랙이 실패해도 그 트랙만 되돌리고 다음 트랙으로 간다.
- 실패 규칙: 같은 문제 1회 재시도, 두 번째 실패면 그 Task 변경을 되돌리고 실패 기록만 커밋한다.
- 검사 도구 규칙: 확인용 도구 보정은 Task당 1회까지. 넘으면 "미검증"으로 기록하고 다음으로 간다.
- 일이 일찍 끝나면 그대로 종료한다. 시간을 채우려고 범위를 늘리지 않는다.

## 허용 범위
- 수정 가능: Assets/Scripts(보스·후보 선택·fixture 관련), Assets/BossCandidates/B3/(새 폴더), Assets/Editor(보관 스크립트 복원 시에만), Launchers/current, Tools/Run-Regression.ps1, Builds(Housekeeping 규칙), Validation(배치 요약 1개 + _raw), TASKS.md, README.md(링크만)
- 수정 금지: Assets/ScriptableObjects의 기본 Tuning 값, Assets/BossCandidates/B2*, AV1 후보 Asset 값, 씬 지형, GAME_DESIGN.md, PRD.md, AGENTS.md

## 사전 결정
| 결정 | 답 | 막히면 대체 |
| --- | --- | --- |
| 후보 선택 방식 | 기존 방식이 있으면 재사용, 없으면 실행 인자 -boss=base\|b2\|b2fast\|b2slow\|b3 | 인자 없으면 기존 기본형과 동일해야 함 |
| AV1 켜기 | 기존 방식 재사용, 없으면 -av1 인자 (코드 3파일 이내) | 3파일 초과면 만들지 않고 "불가" 기록 |
| 보스 직전 시작 | 개발용 fixture로 CP-C04, 대시·벽 점프·더블 점프 보유 | 기존 fixture 우선 재사용 |
| B3 수치 애매 | 아래 B3 사양 값 그대로, 임의 조정 금지 | 피할 수 없음이 확인되면 그 패턴만 끄고 기록 |

## 트랙 A — 기존 후보 체험 (먼저)
- A1 후보 선택 인자: 시작 로그에 BossTuning 이름과 extendedPatterns 1줄. 인자 없음은 기존과 동일.
- A2 AV1 켜기: -av1 시 시작 로그 1줄.
- A3 런처: Launchers/current에 Boss-Base.cmd, Boss-B2.cmd, Boss-B2-fast.cmd, Boss-B2-slow.cmd. 모두 CP-C04 시작, -av1 켬(A2 불가면 끔).
- A4 Play-V4-BeforeBoss.cmd, Play-V4-AfterEnding.cmd는 자연 저장이 없어 실행 불가다. fixture 저장으로 실행 가능하게 바꾼다. 안 되면 TASKS의 "실행 불가" 표시만 유지.
- 합격 기준: 각 런처가 CP-C04(또는 해당 단계)에서 시작, 로그에 맞는 후보, 보스방 진입 후 10초 Error 0.

## 트랙 C — 새 후보 B3 (기본값 채택 아님)
- B2 Asset을 Assets/BossCandidates/B3/로 복사해 시작한다. HP 28, 피해 1, B2의 기존 4패턴 값은 그대로 둔다.
- 패턴 순환(고정 순서): 근접 → 추적 연타 → 도약 → 충격파 → 연속 도약 → 돌진. 각 패턴 시작 시 플레이어 쪽을 본다.
- 새 패턴 1 "추적 연타": 0.5초 예고 후 플레이어 쪽으로 3u/s로 이동하며 근접 3회(간격 0.35초, 각 범위 2.5u, 각 타격 0.25초 전 짧은 예고 표시). 최대 이동 4u, 벽 앞에서 정지. 끝나면 회복 1.2초. 의도한 회피: 뒤로 대시, 또는 점프+더블 점프로 넘어가기.
- 새 패턴 2 "연속 도약": 1회차는 기존 도약(예고 0.7초, 착지점=예고 시작 시 플레이어 x, 착지 충격파 없음). 착지 0.3초 뒤 2회차 예고 0.5초, 착지점=그때의 플레이어 x, 착지 시 양방향 충격파(기존 값). 끝나면 회복 1.5초. 의도한 회피: 착지 예고 밖으로 이동, 충격파는 점프.
- 패턴 사이 회복은 최소 1.0초를 유지한다.
- 런처 Launchers/current/Boss-B3.cmd (CP-C04 시작, -av1).
- C 합격 기준 (60fps, 새 패턴 각각):
  1. 가만히 서 있으면 피격된다(위협이 있다).
  2. 의도한 회피 입력이면 피해 0이다.
  3. 6패턴 1바퀴 동안 Error 0, Pause·사망 복귀(CP-C04) 후 HP와 패턴이 초기화된다.
- 확인은 기존 보스 검사 방식을 재사용한다. 새 검사 도구를 따로 만들지 않는다.

## 트랙 B — 회귀 조용한 모드
- Tools/Run-Regression.ps1에 -Quiet 옵션을 추가한다. 게임 exe는 작은 창(640×360, 창 모드)으로 띄우고, 화면이 필요 없는 검사만 -batchmode -nographics로 돌린다.
- 검사별로 기존 방식과 Quiet 결과가 같을 때만 그 검사를 Quiet 대상으로 표시한다. 다르면 그 검사는 기존 방식을 유지한다.
- 기본값(옵션 없음)은 지금과 완전히 같다.
- 합격 기준: 기존 방식 13/13, -Quiet 13/13, 검사별 결과 비교표를 배치 요약에 넣는다.

## Backlog (남는 시간에만)
1. README.md의 지워진 빌드 링크를 Launchers/current 또는 "삭제됨"으로 바꾼다(링크만). 합격 기준: README 깨진 링크 0.

## 배치 공통 합격 기준
- 각 트랙 끝에 Tools/Run-Regression.ps1 13/13 (트랙 B 이후에는 기존 방식과 -Quiet 모두)
- 마지막에 Tools/Batch-Card.ps1 -Base <시작 HEAD> -Allowed <위 허용 범위>: 허용 범위 밖 0, TASKS 깨진 링크 0, 없는 빌드·런처 0
- Builds/에는 최신 1개 + 직전 1개만 남긴다(런처가 가리키는 빌드는 유지).

## TASKS 갱신
- "단계 보류"의 D-COMBAT 중 보스 선택 부분을 "결정 대기 D-BOSS-PICK"으로 옮긴다: Boss-Base / B2 / B2-fast / B2-slow / B3 중 선택, 각 3~5분, AV1 켜짐 여부 표시, 런처 링크.
- 히트스톱·Q6 등 나머지 전투 손맛 항목은 "단계 보류"에 그대로 둔다.
- 결정 대기 전체가 8개를 넘지 않게 한다.

## 종료
- Task별 로컬 커밋으로 진행한다. 끝나면 origin/main..HEAD가 이번 배치 커밋뿐인지 확인한 뒤 git reset --soft <시작 HEAD>로 하나로 합친다.
- 커밋 메시지 첫 줄은 "Batch 11: boss candidates, B3 patterns, quiet regression". 본문에 Task별 상태 표, 숫자 카드, 미검증 목록을 넣는다. 태그 B11을 붙인다. push하지 않는다.
- Validation/B11-summary.md에 결과를 남긴다. 맨 위는 숫자 카드, 그다음은 사람이 할 일(런처별 몇 분, 무엇을 볼지)이다.
- 보고 10줄 이내: 트랙별 PASS·FAIL·미검증, 런처 목록, B3 합격 기준 결과, Quiet 대상 검사 수, REGRESSION, 숫자 카드.
