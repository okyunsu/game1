이번 실행은 Batch 12만 수행한다. 무인 장시간 실행이다. AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다.
push는 하지 않는다(아침에 사용자가 한다).

## 시작 전 알아둘 것
- B11 결과: 보스 후보 선택 인자(-boss=base|b2|b2fast|b2slow)와 -av1, Boss-*.cmd 런처, Run-Regression.ps1 -Quiet가 이미 있다. Validation/B11-summary.md를 읽는다.
- B11의 B3는 "새로 추가한 BossTuning 필드가 false로 로드됨 → import assert" 로 실패·복원됐다. 원문: Validation/_raw/B11/C-b3.txt, C-build2.log.
- 회귀는 항상 Tools/Run-Regression.ps1 -Quiet 로 돌린다(창이 거의 뜨지 않음). 배치 마지막 1회만 기본 방식도 함께 돌린다.

## 사용자 결정 (2026-10-13)
- 보스 플레이 결과: 패턴이 보이고, 피할 수 있고, 반격 가능하다. B2-fast를 현재 보스로 채택한다.
- 다만 패턴 수가 부족하다. 새 패턴을 추가한다.

## Batch 12 — B2-fast 채택, B3 확장 보스(패턴 추가, 2페이즈)
- Base 커밋: 시작 시 HEAD를 기록한다(B11 이후).
- 예산: 7시간. Task 수 상한 없음. **아래 큐를 위에서부터 끝까지 진행한다.**
- **큐 규칙:** 한 Task가 BLOCKED되면 그 Task 변경만 되돌리고 바로 다음 Task로 간다. 의존 Task(표의 "선행")는 선행이 BLOCKED면 SKIPPED로 적고 넘어간다. 큐가 남아 있는데 멈추지 않는다. 종료는 (1) 큐 소진, (2) 예산 소진, (3) 컴파일이 되돌려도 복구되지 않는 상태 중 하나일 때만 한다.
- 재시도: Task당 같은 문제 1회. 예외로 T2(B3 import 문제)는 2회까지.
- 검사 도구 보정: Task당 1회. 넘으면 그 항목만 "미검증"으로 기록하고 Task는 계속한다.
- 새 검사 도구를 따로 만들지 않는다. B04BossVerifier 등 기존 보스 검사를 재사용·확장한다.

## 허용 범위
- 수정 가능: Assets/Scripts(보스 관련, BossCandidateSelection), Assets/Editor(보스 검사·빌드·Asset 생성 스크립트), Assets/BossCandidates/B3/(새 폴더), Assets/ScriptableObjects/BossTuning.asset(T1에서만), Assets/Prefabs(보스 탄·낙하물 프리팹 새로 추가만), Launchers/current, Builds(Housekeeping), Validation(B12-summary.md + _raw), TASKS.md, GAME_DESIGN.md 13절(T9에서만)
- 수정 금지: Assets/BossCandidates/B2, B2-fast, B2-slow, 씬 지형(C06 아레나 크기 포함), 플레이어 Tuning, AV1 Asset, PRD.md, AGENTS.md

## 공통 설계 기준 (B3 새 패턴 전부)
- 플레이어 기준: 점프 높이 약 2.4u, 더블 점프 포함 수평 약 4.3u, 대시 있음. C06 아레나 x 1~19.
- 모든 공격은 예고 0.4초 이상, 예고는 기존처럼 바닥/공중 경고 표시.
- 패턴이 끝나면 회복 1.0초 이상(2페이즈도 0.8초 이상).
- 각 패턴 시작 시 플레이어 쪽을 본다.
- 피해 1, B3 HP 28(B2-fast와 같음). 그 밖의 기존 4패턴 값은 B2-fast 그대로.
- 패턴마다 개별 on/off 값을 둔다(하나가 실패해도 나머지는 켤 수 있게).

## Task 큐

| 순서 | Task | 선행 | 내용 | 합격 기준 |
|---|---|---|---|---|
| 1 | T1 B2-fast 채택 | - | 기본 BossTuning.asset 값을 B2-fast와 같게 만든다(extendedPatterns 포함). B2-fast 원본은 그대로. 변경 전 값은 Validation/B12-summary.md에 표로 남긴다. Play-V4·BeforeBoss 런처가 인자 없이 B2-fast 동작이 되는지 확인 | 인자 없이 시작 로그 extendedPatterns=True, 값 비교표 일치, 회귀 13/13 |
| 2 | T2 B3 import 원인 해결 | - | B11 실패 재현 후 원인을 고친다. 방법: (1) 새 필드는 C# 코드에만 추가하고 .asset YAML을 손으로 편집하지 않는다. (2) 컴파일이 끝난 **다음 별도 Unity 실행**에서 Editor 메서드가 B2-fast를 복사해 B3 Asset을 만들고 SerializedObject로 값 설정 → SaveAssets. (3) 세 번째 실행에서 B3 값을 읽어 로그로 확인한 뒤 빌드 | 빌드 로그에 B3 새 필드 값이 지정값 그대로 출력, import assert 0. 원인 1~2줄 기록 |
| 3 | T3 추적 연타 | T2 | 0.5초 예고 → 플레이어 쪽으로 3u/s 이동하며 근접 3회(간격 0.35초, 각 범위 2.5u, 각 타격 0.25초 전 예고). 최대 이동 4u, 벽 앞 정지. 회피: 뒤로 대시 또는 점프+더블 점프로 넘기 | 아래 "패턴 합격 기준" |
| 4 | T4 연속 도약 | T2 | 1회차 기존 도약(예고 0.7초, 착지점=예고 시작 시 플레이어 x, 충격파 없음). 착지 0.3초 뒤 2회차 예고 0.5초, 착지점=그때 플레이어 x, 착지 시 양방향 충격파(기존 값). 회복 1.5초 | 패턴 합격 기준 |
| 5 | T5 백스텝 탄 | T2 | 0.6초 예고 → 뒤로 3u 백스텝(벽 앞 정지) → 플레이어 쪽으로 탄 3발, 간격 0.45초, 높이 순서 낮음(중심 0.5u)·높음(중심 2.8u)·낮음, 탄속 7u/s, 탄 크기 0.6u. 낮은 탄=점프로 회피, 높은 탄=서 있으면 안 맞음. 탄은 벽에서 사라짐. 프리팹은 새로 추가 | 패턴 합격 기준 + 높은 탄은 서 있을 때 피해 0 |
| 6 | T6 낙하물 | T2 | 0.8초 예고(보스가 바닥 내리침) → 플레이어 x 기준 -3u/0/+3u 세 곳에 1.0초 낙하 예고(폭 1.5u) → 천장에서 낙하. 아레나 밖 지점은 생략. 세 곳 사이 1.5u 안전 지대 | 패턴 합격 기준 + 안전 지대에 서 있으면 피해 0 |
| 7 | T7 2페이즈 | T3~T6 중 2개 이상 DONE | HP 50% 이하에서 0.6초 멈춤 + 경고 표시 후 2페이즈. 회복 시간 ×0.8(최소 0.8초), 추적 연타 4회, 연속 도약 3회(3회차 예고 0.5초) | 페이즈 전환 1회만, 전환 중 피격 무시 없음(보스 HP는 계속 깎임), 사망 복귀 시 1페이즈로 초기화 |
| 8 | T8 패턴 순서·런처 | T2 | 순환: 근접 → 추적 연타 → 도약 → 백스텝 탄 → 충격파 → 연속 도약 → 낙하물 → 돌진. 꺼진 패턴은 건너뜀. Launchers/current/Boss-B3.cmd(CP-C04, -av1, -boss=b3) | 런처로 시작, 로그에 B3·켜진 패턴 목록, 1바퀴 Error 0 |
| 9 | T9 문서 | T8 | GAME_DESIGN 13절에 "B2-fast 채택(10/13)"과 B3 패턴 표(이름·예고·회피·수치)를 **추가**한다. 기존 문장은 지우지 않는다. B3는 후보로 표시 | 문서 링크 깨짐 0 |
| 10 | T10 체험 런처 정리 | T8 | Boss-B3-P2.cmd: B3를 HP 14(2페이즈 직전)로 시작하는 개발용 런처 | 시작 로그에 HP 14, 첫 피격으로 2페이즈 전환 |

### 패턴 합격 기준 (T3~T6 각각, 60fps)
1. 가만히 서 있으면 피격된다.
2. 의도한 회피 입력이면 피해 0이다.
3. 회피 후 회복 구간에 반격하면 보스 HP가 줄어든다.
4. Pause 중 진행이 멈추고 재개 후 이어진다.
5. 사망 → CP-C04 복귀 후 보스 HP·패턴·탄·낙하물이 초기화된다.
6. 해당 패턴만 켠 상태로 10회 연속 Error 0.
30fps/120fps는 "의도한 회피 = 피해 0" 한 항목만 확인한다. 실패하면 그 fps만 기록하고 Task는 PARTIAL.

## 배치 공통 합격 기준
- Task마다 Tools/Run-Regression.ps1 -Quiet 13/13. 마지막에 기본 방식 13/13도 1회.
- 마지막에 Tools/Batch-Card.ps1 -Base <시작 HEAD> -Allowed <위 허용 범위>: 허용 범위 밖 0, TASKS 깨진 링크 0, 없는 빌드·런처 0.
- Builds/에는 최신 1개 + 직전 1개만(런처가 가리키는 빌드 유지).

## TASKS 갱신
- D-BOSS-PICK을 "결정 기록: B2-fast 채택(10/13)"으로 옮긴다.
- 새 결정 대기 D-B3: "Boss-B3 / Boss-B3-P2 각 5~10분 플레이 → B3 채택·패턴별 끄기·수치 조정". 결정 대기 8개 이하 유지.

## 종료
- Task별 로컬 커밋. 끝나면 origin/main..HEAD가 이번 배치 커밋뿐인지 확인 후 git reset --soft <시작 HEAD>로 하나로 합친다.
- 커밋 첫 줄 "Batch 12: adopt B2-fast, B3 boss with new patterns and phase 2". 본문에 Task 표(DONE/PARTIAL/BLOCKED/SKIPPED), 숫자 카드, 미검증 목록. 태그 B12. push 안 함.
- Validation/B12-summary.md: 맨 위 숫자 카드, 다음 "사람이 할 일"(런처별 몇 분, 무엇을 볼지), 다음 Task 표, 실패 원문.
- 실행 시작·종료 시각을 요약 맨 위에 적는다.
- 보고 10줄 이내.
