이번 실행은 사용자 승인 Batch 22(보스1 + draft4.1 반영)만 수행한다. AGENTS.md를 따른다. Design/v5의 JSON·문서는 읽기만 한다.

(개정 2026-10-28 23시: 처음 판은 원격 기준이라 B21 재개 결과를 몰랐다. 위험 지형 복귀 구현·도랑 측정 Task를 빼고, B21 결과를 이어받도록 바꿨다.)

## Batch 22
- 시작: `Tools/Preflight.ps1 -Batch 22`. 실패하면 needs-unity=yes Task를 시작하지 않고 BLOCKED 기록 후 종료.
- Base: 현재 HEAD(B21 재개 + 10/28 문서 커밋들). 시작 시 해시 기록. 이전 미푸시 커밋은 합치지 않는다.
- 읽을 범위: AGENTS.md, TASKS.md 0·1·2절, GAME_DESIGN.md 12-2·13절, `Design/v5/WORLD_V5.md` 13·14절, `Design/proposals-1028/04_BOSS1_CONTRACT.md`(고정 계약), `01_DECISIONS.md`, `Validation/B21-resume-summary.md`·`B21-hazard-return.md`.
- 예산: AI 실행 기준 넉넉히(밤샘 1회분). Task 안전 상한 14개. 종료 정리 시간 확보.
- 재시도: 같은 접근 1회(총 2회). 두 번째 실패면 되돌리고 실패 기록만 커밋.
- 트랙
  - A 보스1: T1~T4 — 합격: BossTest_B1에서 계약 C-2~C-5 동작, 능력 없이 처치
  - B 방 반영: T5~T7 — 합격: draft4.1 V5 6방 재생성, HazardReturn V5 기본, 실측 표
  - C 합치기: T8~T9 — 합격: A03→A04 보스→A05 대시→A03 고리 자동 경로 + 사용자 안내
- 사전 결정

| 결정 | 답 | 날짜 | 출처 |
| --- | --- | --- | --- |
| D-V5-DRAFT4 | draft4.1 채택 | 2026-10-28 | 사용자 |
| D-HAZARD-RETURN | V5 기본 켜짐, V4 기존 유지 | 2026-10-28 | 사용자 |
| D-BOSS4 | 보스 4, 보스1 A04 싸우고 얻음 | 2026-10-28 | 사용자 |
| D-BOSS1-LINK | 처치 → 바닥 덮개 제거 → A05 | 2026-10-28 | Claude 추천, 사용자 변경 없음 |
| D-HUB-ENEMY | A03 적 0 + CP-A03 | 2026-10-28 | Claude 추천, 사용자 변경 없음 |
| D-A03-PRACTICE-HAZARD | 이번에 넣지 않음 | 2026-10-28 | 사용자 |

## Task

| Task | 유형 | needs-unity | 선행 | 내용 | 합격 기준 |
| --- | --- | --- | --- | --- | --- |
| B22-T0 | doc | no | 없음 | `01_DECISIONS.md`를 TASKS 2-1·2절·B21 처리 표에 반영 | 표 행 추가, D-V5-DRAFT4·D-HAZARD-RETURN 결정 대기에서 이동 |
| B22-T1 | feature | yes | 없음 | 보스 처치 저장을 보스 ID 단위로 확장. 기존 `bossDefeated`는 최종 보스(C06) 의미로 호환 유지. `EndingFlow`·`DefeatedBossSuppression`은 최종 보스에만 반응 | 기존 C06 엔딩·CompletionBuildVerification 회귀 PASS, 새 ID 저장/로드 PASS, 구 저장 파일 로드 PASS |
| B22-T2 | feature | yes | T1 | `BossEncounter`가 아레나 범위·잠금벽·트리거·처치 이벤트(덮개 제거)·보스 ID를 인스턴스 값으로 받게 일반화. B3 기본 동작 변경 없음 | B3 런처 회귀 PASS(패턴·잠금·엔딩) |
| B22-T3 | feature | yes | T2 | 새 Asset `Assets/BossCandidates/B1/BossTuning.asset`: HP14, 피해1, 1페이즈, 도약·충격파·돌진만 on, 돌진 속도 7/8u/s 후보. 보스 윗면 PogoTarget | 패턴 3종만 발생, 2페이즈 전환 0회, 포고 튕김 MEASURE |
| B22-T4 | verification | yes | T3 | `BossTest_B1` 씬(계약 C-1: 폭25×높이10, 잠금벽, 덮개, CP). 대시·벽 점프·더블 점프 끈 상태 자동 처치, 사망·재시도, 처치 후 재진입 | 능력 없이 처치 MEASURE, 사망→CP 복귀·HP 초기화, 처치 후 재진입 시 보스·잠금벽·덮개 없음, 런처 `Play-BossTest-B1.cmd` |
| B22-T5 | feature | yes | 없음 | HazardReturn(B21 T5 후보)을 **V5 씬에서 인자 없이 기본 켜짐**. V4·인자 없는 V4 런처는 기존 동작 | V4 hazard 회귀 무변화, V5 기본 실행에서 컴포넌트 존재 |
| B22-T6 | feature | yes | T2 | 생성기가 `rooms_v5_A.json` A04 `boss` 필드(arena·gate_wall·trigger·spawn·floor_cover·checkpoint)와 A03 `CP-A03`을 읽어 배치. `hazard_return` 필드는 없음(draft4.1 삭제) | 필드 fixture 단위 테스트 PASS |
| B22-T7 | verification | yes | T5,T6 | draft4.1로 V5 6방 재생성 + 실측: ① A04 무능력 직행으로 섬 5회 실패·왼쪽 바닥 복귀, 등불 2개로 섬 보상 5/5(BL3 레시피 방식) ② A05 무능력 도랑 10회 통과0·들어온 쪽 복귀, 대시 10회 통과10 ③ A03 연습 간격 5.5/4.5/4.5 각각 무능력 5회 실패·대시 5회 성공 ④ 지름길 잠금 코드가 데이터 없을 때 비활성 | 각 표, 정적 검사 FAIL0, Scene 검사 FAIL0 |
| B22-T8 | verification | yes | T4,T7 | A04 아레나에 보스1 배치 후 고리 자동 경로: A03 → A04 아레나 진입·잠금 → 처치 → 덮개 제거 → A05 낙하 → 대시 획득 → 도랑 대시 → A03 → 대시 연습 → 대시 문 선반. 저장/Continue 1회 | 고리 MEASURE 소요 시간, 단계별 PASS, Error0 |
| B22-T9 | doc | no | T8 | 사용자 플레이 안내: 런처 `Play-V5-Loop.cmd`(A03 시작), 고리 1회 10~15분, 플레이 전 질문 2개(05 마지막 절), 기록 칸(고리 시간·보스 시도 횟수·막힌 곳) | README·TASKS 3·9절 갱신 |

- 허용 범위: 수정 가능 `Assets/Scripts/*`, `Assets/BossCandidates/B1/*`, `Assets/Scenes/Test/*`, `Assets/Scenes/V5/*`(T7 재생성만), `Launchers/current/*`, `Tools/*`, `Validation/*`(새 파일), TASKS.md, README.md
- 수정 금지: `Design/v5/*`, `Design/proposals-1028/*`, `Assets/Scenes/V4/*`, `PlayerTuning.asset`, 기본 `BossTuning.asset`(B3), `Assets/Candidates/B21HazardReturn.asset` 값, PRD.md, GAME_DESIGN.md
- Backlog
  1. B22-BL1(verification, yes, T8): 고리 자동 경로를 몸 1.3으로 1회(D-BODY 참고용) — 합격: 단계별 PASS/FAIL 표만, 기본값 변경 없음
- 종료 조건: 전부 완료·BLOCKED 또는 예산 소진.

## 하지 않을 것
- 방 좌표 변경(실측 결과가 기준에 못 미치면 수치와 원인만 기록하고 Claude 결정 대기로 올림).
- B21 지름길 잠금 코드 삭제, 모서리 보정 기본 켜기.
- 보스2~4, 최종 보스 패턴 변경, 새 적 유형·새 패턴.
- push.

## 중단 조건
- 계약(04)과 코드 구조가 맞지 않아 계약 값을 바꿔야 할 때 → 멈추고 TASKS 2절 결정 대기로 올림.
- B3 회귀 FAIL 2회 반복 → T2 되돌리고 B1을 별도 컴포넌트로 만드는 우회안을 결정 대기로.
- T7 실측이 기준 미달 → 해당 방만 FAIL 기록, 다른 Task 계속.
