이번 실행은 Batch 13만 수행한다. 무인 장시간 실행이다(사용자 퇴근 전까지). AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다.
push는 하지 않는다(저녁에 사용자가 B12와 함께 push한다).

## 시작 조건
- HEAD는 로컬 B12 커밋(태그 B12)이어야 한다. origin/main보다 B12 커밋 1개 앞선 상태가 정상이다.
- 미추적 파일이 Batches/BATCH-13.md 하나뿐이면 먼저 그것만 커밋한다("docs: add Batch 13 instructions"). 그 외 변경이 있으면 시작하지 않고 멈춘다.
- 시작 시각과 시작 HEAD를 기록한다.

## 시간 규칙
- **18:00 KST 이후에는 새 Task를 시작하지 않는다.** 진행 중인 Task만 끝내고 종료 처리(회귀·카드·squash·요약)를 18:30까지 마친다.
- 18:00 전에 큐가 끝나면 그대로 종료한다. 시간을 채우려고 범위를 늘리지 않는다.
- 큐 규칙: Task가 BLOCKED되면 그 Task 변경만 되돌리고 다음 Task로 간다. 선행이 BLOCKED면 SKIPPED. 큐가 남아 있으면 멈추지 않는다.
- 재시도는 Task당 같은 문제 1회. 검사 도구 보정은 Task당 1회.
- 회귀는 Tools/Run-Regression.ps1 -Quiet. 배치 마지막에만 기본 방식도 1회.

## 이 배치의 원칙 (사용자 승인 없이 할 수 있는 일만)
- 기본값(Tuning, 기본 보스, 기본 AV) 채택·변경 금지. 새 것은 모두 "후보"로, 실행 인자로만 켠다.
- GAME_DESIGN.md·PRD.md·씬 지형 수정 금지. AGENTS.md는 Track A5의 "이력 이동"만 허용.
- 외부 에셋 다운로드 금지. 소리는 코드로 합성하거나 기존 AV1 클립을 재사용한다.

## 허용 범위
README.md, AGENTS.md(A5만), TASKS.md, Tools/, Validation/(B13-summary.md + _raw, A2 정리), Archive/, Batches/BATCH-13.md, Assets/Scripts(보스·연출·Verification), Assets/Editor, Assets/BossCandidates/B3*/(새 폴더만), Assets/Candidates/AV2/(새 폴더), Assets/Prefabs(새 연출 프리팹 추가만), Launchers/current, Builds(Housekeeping)
수정 금지: Assets/ScriptableObjects 기본 Tuning 값, Assets/BossCandidates/B2*, Assets/BossCandidates/B3/BossTuning.asset 기존 값, Assets/Candidates/AV1 값, Assets/Scenes, GAME_DESIGN.md, PRD.md, Packages, ProjectSettings(A·C의 스크립트 정의 심볼 추가 제외)

## Track A — 저장소 정리 (게임 동작 변화 없음, 먼저)
| Task | 내용 | 합격 기준 |
|---|---|---|
| A1 | 실제 회귀 실행기 Validation/Run-Regression.ps1을 Tools/로 git mv하고 Tools/Run-Regression.ps1 하나로 합친다. 검사 내용·판정은 바꾸지 않는다. 기존 경로를 부르는 곳은 새 경로로 | 기본 13/13, -Quiet 13/13, 검사별 PASS 줄 수가 B12 최종과 같음 |
| A2 | Validation/에 남은 스크립트(Run-N5-TA/TF.ps1, Prepare-N5-Replay.ps1)와 요약이 아닌 로그·json·이미지 폴더를 Archive/validation-2026-10/로 git mv. TASKS·README가 링크하는 파일은 남기거나 링크를 새 경로로 | Validation/에 *-summary.md, 결과 txt, _raw만 남음. .md 깨진 링크 수 증가 0 |
| A3 | Tools/의 B10 전용 스크립트(Build-B10Harness, Build-B10Release, Make-B10Manifest, Run-B10Check, Run-B10Partitions, Run-B10Queue, Run-B10Search, S3Run, S3Runtime, B09PlaytestSummary)를 Archive/tools-b10/로 git mv. 런처·회귀·Batch-Card가 참조하면 그 파일은 남긴다 | 런처 Play-V4·Boss-B2·Boss-B3 시작 로그 정상, 회귀 13/13 |
| A4 | README.md를 현재 상태 중심으로 다시 쓴다(80줄 이내): 게임 한 줄 소개, 현재 상태(B12까지), 실행 방법(Launchers/current 각 런처 1줄), 조작키, 폴더 구조, 개발 방식(Batch·회귀·숫자 카드) 요약. 옛 빌드 이력 전문은 Archive/README-history-2026-10.md로 옮기고 링크 1줄 | README 80줄 이하, 깨진 링크 0, 언급한 런처가 모두 존재 |
| A5 | AGENTS.md에서 과거 사건 서술·배치별 예시·이력성 문장만 Archive/AGENTS-history-2026-10.md로 옮긴다. **규칙 문장은 한 줄도 바꾸거나 지우지 않는다.** 옮긴 줄 목록을 요약에 남긴다 | 규칙 문장 diff 0(옮긴 줄은 이력성 근거와 함께 표로), AGENTS 180줄 이하 목표(못 미치면 그대로 기록) |

## Track B — B3 후보 후속 (기술 보완, 채택 아님)
| Task | 내용 | 합격 기준 |
|---|---|---|
| B1 | T3 추적 연타 회피 검증 보완: B12에서 뒤로 대시가 fixture의 낮은 지형(x6.37)에 막혔다. 지형은 바꾸지 않는다. 검사 시작 위치를 아레나 중앙 쪽으로 옮기거나 "점프+더블 점프로 넘기" 회피로 확인 | 60fps 의도한 회피 피해0, 회복 반격, Pause, 사망 복귀, 10회 Error0. 30/120fps 회피0 |
| B2 | 회복 비교 후보: B3를 복사해 Assets/BossCandidates/B3-rec10/(기존4패턴 회복 1.0초, 나머지 동일) 생성. B12 T2의 "컴파일→별도 실행 생성→검증→빌드" 절차를 따른다. 런처 Boss-B3-rec10.cmd | 시작 로그에 B3-rec10과 회복값, 1바퀴 Error0 |
| B3 | B3 2페이즈 30/120fps 확인: 페이즈 전환·추적4회·도약3회·회복 시간 | 30/120 각각 전환1회·Error0, 회복 측정값 기록 |
| B4 | B3 8패턴 연속 3바퀴 soak(60fps, 플레이어 무적 fixture 허용) | Error0, 탄·낙하물·파동 누적 0(바퀴 끝마다 남은 오브젝트 수 기록) |

## Track E — 전투 임팩트·사운드 후보 AV2 (D-COMBAT 재판단용, 기본값 아님)
- 실행 인자 -av2로만 켠다. 인자 없으면 지금과 완전히 같다. AV1 Profile 구조를 재사용하고 Assets/Candidates/AV2/에 새 Profile을 만든다(B12 T2 절차).
| Task | 내용 | 합격 기준 |
|---|---|---|
| E1 | 플레이어 공격 명중: 명중 지점 스파크(0.1초, 프리팹), 피격 대상 흰색 번쩍임 0.06초, 명중음(코드 합성 또는 AV1 재사용) | -av2에서 명중 1회당 이벤트 각1, 빗나감 0. 30/60/120 |
| E2 | 화면 흔들림: 명중 0.05u·0.08초, 플레이어 피격 0.12u·0.12초, 보스 착지·낙하물 0.15u·0.15초. 각 크기 Profile 값으로 조정 가능, 0이면 꺼짐 | 흔들림 후 카메라가 원위치로 복귀(오차 0.01u 이하), 방 전환 중에는 흔들림 없음 |
| E3 | 보스 연출: 보스 피격 번쩍임·피격음, 각 패턴 예고 시작 시 예고음(패턴별 높낮이 다르게), 2페이즈 전환 경고음, 보스 처치 시 0.3초 슬로모션(Time.timeScale 0.3, Pause와 충돌 없음) | 패턴 8종 예고음 각1, 처치 슬로모션 후 timeScale 1 복귀, Pause 중 처치 없음 |
| E4 | 플레이어 피격: 피격음, 짧은 빨간 테두리(0.15초), 무적 중 깜빡임은 기존 유지 | 피격 1회당 각1, 무적 중 추가 재생 0 |
| E5 | 런처: 기존 Boss-B2-fast(=기본)·Boss-B3·Play-V4에 대응하는 -av2 런처 Boss-B3-AV2.cmd, Play-V4-AV2.cmd | 시작 로그에 AV2 활성 1줄, 10초 Error0 |

## Track C — 검증 코드를 배포판에서 분리 (가장 위험, 마지막)
- 목표: Release(비개발) 빌드에서 Assets/Scripts/Verification 코드가 빠지게 한다. 개발 빌드·에디터·런처·회귀는 지금과 같아야 한다.
- 방법: asmdef 분리는 하지 않는다(게임 코드가 Assembly-CSharp라 참조 방향 문제). 대신 Verification 파일과, 게임 코드에서 Verification을 부르는 지점을 `#if UNITY_EDITOR || DEVELOPMENT_BUILD`로 감싼다. 런처용 빌드는 계속 Development로 만든다.
| Task | 내용 | 합격 기준 |
|---|---|---|
| C1 | 게임 코드 → Verification 호출 지점 목록 작성(파일·줄). 변경 없음 | 목록을 요약에 표로 |
| C2 | 감싸기 적용 | 에디터 컴파일 Error0, 기본·Quiet 회귀 13/13 |
| C3 | Development 빌드로 런처 확인 | Play-V4, Boss-B2-fast 기본, Boss-B3, BeforeBoss, AfterEnding 시작·10초 Error0 |
| C4 | Release 빌드 1회(Builds/에 남기지 않고 확인 후 삭제) | errors0, 새 게임 시작→A01 진입 Error0, 빌드 로그에서 Verification 타입 0 |
- **C2~C4 중 하나라도 실패하면 Track C 전체를 되돌린다.** 사용자가 저녁에 런처로 플레이해야 하므로 런처가 깨진 상태로 끝내면 안 된다.

## Backlog (18:00 전에 큐가 끝났을 때만, 위에서부터)
1. E1~E4 30/120fps 전부 재확인(빠진 fps만)
2. B3 각 패턴 예고 표시 색·모양이 서로 구분되는지 스크린샷 대신 예고 오브젝트 색값·크기 표로 기록
3. TASKS.md "보관된 항목 색인"의 링크가 A2 이동 후에도 모두 열리는지 재확인

## 배치 공통 합격 기준
- Task마다 -Quiet 13/13. 마지막에 기본 13/13 1회.
- Tools/Batch-Card.ps1 -Base <시작 HEAD> -Allowed <위 허용 범위>: 허용 범위 밖 0, TASKS 깨진 링크 0, 없는 빌드·런처 0.
- Builds/에는 최신 1개 + 직전 1개만(런처가 가리키는 빌드 유지).
- 종료 시 모든 런처(Launchers/current/*.cmd)가 시작 로그 정상.

## TASKS 갱신
- 결정 대기 D-B3를 갱신: "Boss-B3 / Boss-B3-rec10 / Boss-B3-P2 플레이 → 패턴별 유지·끄기, 회복 0.8 vs 1.0, 추적 연타 회피 가능 여부"(B1 결과 반영).
- "단계 보류" D-COMBAT을 결정 대기로 옮긴다: "Play-V4-AV2 / Boss-B3-AV2 플레이 → AV2 채택·항목별 끄기·크기 조정"(E가 하나라도 DONE일 때만).
- 결정 대기 8개 이하 유지.

## 종료
- Task별 로컬 커밋. 끝나면 git reset --soft <시작 HEAD>로 하나로 합친다(BATCH-13 지시문 커밋은 시작 HEAD 이전이므로 포함되지 않아도 됨).
- 커밋 첫 줄 "Batch 13: repo cleanup, B3 follow-ups, AV2 impact candidates, release split". 본문에 Task 표(DONE/PARTIAL/BLOCKED/SKIPPED), 숫자 카드, 미검증 목록. 태그 B13. push 안 함.
- Validation/B13-summary.md: 맨 위에 시작·종료 시각과 숫자 카드, 다음 "사람이 할 일"(런처별 몇 분, 무엇을 볼지, 결정할 것), Task 표, 실패 원문.
- 보고 10줄 이내.
