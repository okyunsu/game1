이번 실행은 사용자 승인 Batch 22(보스1 + 위험 지형 복귀)만 수행한다. AGENTS.md를 따른다. V5 방 데이터와 V5 씬은 건드리지 않는다.

## Batch 22
- 선행: Unity 라이선스 정상(Preflight exit 0). 실패하면 needs-unity=yes Task를 시작하지 않고 BLOCKED 기록 후 종료.
- 읽을 범위: AGENTS.md, TASKS.md 0·1·2절, GAME_DESIGN.md 12·13절, `Design/proposals-1028/04_BOSS1_CONTRACT.md`(고정 계약), `03_GAME_DESIGN_PATCH.md`의 12-1·13절.
- 예산: AI 실행 기준 넉넉히(밤샘 1회분). Task 안전 상한 12개. 종료 정리 시간 확보.
- 재시도: 같은 접근 1회(총 2회). 두 번째 실패면 되돌리고 실패 기록만 커밋.
- 트랙(독립):
  - A: T1~T4 보스1 — 합격: BossTest 씬에서 계약 C-2~C-5 동작, 능력 없이 처치 가능
  - B: T5~T6 위험 지형 복귀 — 합격: 도랑 폭 후보별 능력 없음 통과 0/10, 대시 통과 10/10
- 사전 결정

| 결정 | 답 | 날짜 | 출처 |
| --- | --- | --- | --- |
| D-BOSS4 | 보스 4, 보스1 A04 싸우고 얻음 | 2026-10-28 | 사용자 |
| D-A05-TRENCH | 도랑 게이트 + hazard 복귀, 부서지는 벽 삭제 | 2026-10-28 | 사용자 |
| D-SPLIT | 방 데이터는 Claude Code 담당. 이 Batch는 V5 데이터·씬 수정 금지 | 2026-10-28 | 사용자 |
| D-BOSS1-LINK | 처치 → 바닥 덮개 제거 → A05 | 미확정 | Claude 추천. 사용자가 바꾸면 계약 C-3만 교체 |

## Task

| Task | 유형 | needs-unity | 선행 | 내용 | 합격 기준 |
| --- | --- | --- | --- | --- | --- |
| B22-T0 | doc | no | 없음 | 01_DECISIONS.md를 TASKS 2-1·2·Batch21 표에 반영 | 표 행 추가, B21-T1/T2 SUPERSEDED, T5 흡수 표시 |
| B22-T1 | feature | yes | 없음 | 보스 처치 저장을 보스 ID 단위로 확장(`bossDefeated` 단일 bool 호환 유지 = 최종 보스). `DefeatedBossSuppression`·`EndingFlow`가 최종 보스에만 반응하도록 | 기존 C06 엔딩 회귀 PASS, 새 ID 저장/로드 PASS, 구 저장 파일 로드 PASS |
| B22-T2 | feature | yes | T1 | `BossEncounter`를 아레나 범위·잠금벽·트리거·처치 이벤트(덮개 제거)를 인스턴스 값으로 받게 일반화. B3 기본 동작 변경 없음 | B3 런처 회귀 PASS(패턴·잠금·엔딩) |
| B22-T3 | feature | yes | T2 | 새 Asset `Assets/BossCandidates/B1/BossTuning.asset`: HP14, 피해1, 1페이즈, 도약·충격파·돌진만 on, 돌진 속도 후보 7/8u/s. 보스 윗면 PogoTarget | 패턴 3종만 발생, 2페이즈 전환 0회, 포고 튕김 MEASURE |
| B22-T4 | verification | yes | T3 | `BossTest_B1` 씬: 계약 C-1 크기 아레나(폭25×높이10), 잠금벽, 덮개, CP. 대시·벽 점프·더블 점프 끈 상태로 자동 처치 시도, 사망·재시도, 처치 후 재진입 | 능력 없이 처치 성공 MEASURE, 사망→CP 복귀·HP 초기화, 처치 후 재진입 시 보스·잠금벽·덮개 없음, 런처 `Play-BossTest-B1.cmd` |
| B22-T5 | feature | yes | 없음 | hazard 복귀(GAME_DESIGN 12-1): `hazard_return` 있으면 피해1 → 약 0.3초 정지 → 지정 지점 이동, 없으면 기존 동작. HP0이면 기존 사망 | V4·V5 기존 hazard 회귀 무변화, 복귀 MEASURE(위치·HP) |
| B22-T6 | verification | yes | T5 | 테스트 아레나(`TrenchTest` 씬)에서 도랑 폭 5.0/5.5/6.0/6.5/7.0u별 측정: 능력 없음(달리기 점프) 10회, 대시 보유(달리기 점프+공중 대시) 10회 | 표: 폭별 통과율. 추천 폭 = 능력 없음 0/10 이고 대시 10/10 인 가장 좁은 폭 +0.25u 여유 |
| B22-T7 | doc | no | T4,T6 | 사용자 플레이 안내: 런처 2개, 각 3~5분, 확인할 것(보스1 손맛·읽힘, 도랑 폭 체감) | README·TASKS 3절 갱신 |

- Base: `544ea5e` 또는 현재 HEAD(시작 시 기록).
- 허용 범위: 수정 가능 `Assets/Scripts/*`, `Assets/BossCandidates/B1/*`, `Assets/Scenes/Test/*`, `Launchers/current/*`, `Tools/*`(생성기 필드 읽기만: `boss`, `hazard_return`), `Validation/*`(새 파일), TASKS.md, README.md
- 수정 금지: `Design/v5/*`, `Assets/Scenes/V5/*`, `Assets/Scenes/V4/*`, `PlayerTuning.asset`, 기본 `BossTuning.asset`(B3), PRD.md, GAME_DESIGN.md
- Backlog
  1. B22-BL1(feature, yes, T2): 생성기가 `rooms_v5_A.json`의 `boss`·`hazard_return` 필드를 읽어 씬에 배치하는 코드만 추가(씬 재생성은 하지 않음) — 합격: 필드가 있는 fixture JSON으로 생성기 단위 테스트 PASS
- 종료 조건: 전부 완료·BLOCKED 또는 예산 소진.

## 하지 않을 것
- V5 씬 재생성, 방 좌표 변경(방 세션 담당).
- 보스2~4, 최종 보스 패턴 변경.
- 새 적 유형·새 패턴 추가(B1은 기존 패턴만).
- push.

## 중단 조건
- 계약(04)과 코드 구조가 맞지 않아 계약 값을 바꿔야 할 때 → 멈추고 차이를 TASKS 2절 결정 대기로 올림.
- B3 회귀 FAIL이 2회 반복 → 일반화(T2) 되돌리고 B1을 별도 컴포넌트로 만드는 우회안을 결정 대기로 올림.
