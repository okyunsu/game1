이번 실행은 사용자 승인 Batch 23(보스·복귀 연출 + draft4.2 반영)만 수행한다. AGENTS.md를 따른다. Design/v5의 JSON·문서는 읽기만 한다.

## Batch 23
- 시작 전: 작업 트리에 Claude가 넣은 미커밋 변경(`Design/v5/*`, `Design/proposals-1031/*`, `GAME_DESIGN.md`, `Validation/D42-room-rules-v5-A.md`)이 있으면 내용을 바꾸지 말고 `docs: V5 draft4.2 and B23 instructions (Claude)`로 먼저 커밋한다. 그 외 미커밋 변경이 있으면 멈추고 보고한다.
- 시작: `Tools/Preflight.ps1 -Batch 23`. 실패하면 needs-unity=yes Task를 시작하지 않고 BLOCKED 기록 후 종료.
- Base: 시작 시 HEAD 기록(origin/main `e085ed9` 이후).
- 읽을 범위: AGENTS.md, TASKS.md 0·1·2·3절, GAME_DESIGN.md 12-2·13절, `Design/v5/WORLD_V5.md` 15절, `Design/proposals-1031/00_README.md`, `Validation/D42-room-rules-v5-A.md`, `Validation/B22-summary.md`.
- 예산: AI 실행 기준 넉넉히(밤샘 1회분). Task 안전 상한 14개. 종료 정리 시간 확보.
- 재시도: 같은 접근 1회(총 2회). 두 번째 실패면 되돌리고 실패 기록만 커밋.
- 트랙
  - A 연출·수치: T1~T3
  - B 방 반영: T4~T7
  - C 고리: T8~T9
- 사전 결정: `00_README.md` 결정 표(D-HUB-CLEAR·D-BOSS-INTRO·D-BOSS1-HP·D-RETURN-GETUP·D-CLIMB-GAP·D-HUB-LIFT)를 T0에서 TASKS 2-1에 옮긴다. [사용자]와 [Claude 추천] 구분 유지.

## Task

| Task | 유형 | needs-unity | 선행 | 내용 | 합격 기준 |
| --- | --- | --- | --- | --- | --- |
| B23-T0 | doc | no | 없음 | 00_README의 플레이 기록을 TASKS 10절(Playtest), 결정을 2-1에 반영. B22-T5는 사용자 피드백(딜레이 추가)과 함께 처리 표기 | 표 행 추가, 근거 링크 |
| B23-T1 | feature | yes | 없음 | 보스 등장 연출: 잠금벽 닫힘 → 카메라 흔들림 → 보스 위에서 낙하·착지(착지 충격 표시) → 정지 → 첫 패턴. 총 시간 BossTuning(또는 Encounter) 값 `introDuration` 기본 1.8s. 연출 중 보스·플레이어 무피해, 플레이어 이동만 허용. B1에 적용, B3는 값 0이면 기존과 동일 | B1 연출 MEASURE(시작→첫 패턴 1.8±0.05s, 연출 중 피격0), B3 기존 회귀 PASS |
| B23-T2 | tuning | yes | 없음 | B1 Asset HP 14 → 22 | 무능력 자동 처치 PASS(시간·남은 HP 기록) |
| B23-T3 | feature | yes | 없음 | 위험 지형 복귀 일어나기: 피해 → 화면 짧게 어두워짐(약 0.15s) → 안전 바닥으로 이동 → 밝아짐 → 입력 잠금·무적 `getUpTime` 0.45s. 값은 `B21HazardReturn.asset`에 새 필드로(기존 접지0.1·여백0.5·정지0.12 유지) | A05 도랑 복귀 MEASURE(잠금 시간, 무적 중 피해0), HP0이면 기존 사망 |
| B23-T4 | feature | yes | 없음 | 레버 승강기 `start:"low"` 필드: 처음부터 아래(lo)에 있고 잠금 없이 아래·위 레버 모두 호출 가능. 필드 없으면 기존 동작(위에서 시작, 위 레버로 해제) | V4 B01·C01 승강기 기존 동작 무변화, 새 필드 fixture PASS |
| B23-T5 | tool | no | 없음 | `Tools/Check-RoomRules.py`: (1) 승강기를 lo↔hi 상면 연결로 도달성에 반영 (2) 접근 머리 충돌 규칙 추가: 떠 있는 발판으로 오름≥1.5, 가로 간격<1.8이면 FAIL(블록 고체는 제외). 참고 구현 `Design/proposals-1031/approach_check.py` | draft4.2 FAIL0, A03 승강기 관련 도달 WARN 소멸, draft4.1 데이터로 돌리면 접근 규칙이 20건 검출(self-test) |
| B23-T6 | verification | yes | T4,T5 | draft4.2로 V5 6방 재생성. 정적·Scene 검사 | 정적 FAIL0, Scene FAIL0, 요소 수 일치(A01 무너짐2·가시1, A02 가시1, A03 승강기1·적0, A04 무너짐1) |
| B23-T7 | verification | yes | T6 | 실측: ① 접근 머리 충돌 — A01·A02·A04 오르는 구간 각각 달리며 점프 5회, 머리 접촉 0 ② A03 연습 3단계 무능력 각 0/5·대시 각 5/5 ③ 승강기 거리↔위층 왕복·레버 양쪽 ④ 무너지는 발판(A01 2·A04 1) 밟고 떨어짐·복구 ⑤ 가시 구덩이(A01·A02) 빠지면 마지막 안전 바닥 복귀 ⑥ A01 높은 선반 조각 수집 | 각 표, 실패는 원문 보존 |
| B23-T8 | verification | yes | T1~T3,T7 | 고리 자동 경로(A03 CP → 승강기 → A04 → B1(새 연출·HP22) → A05 대시 → 도랑 → A03 → 승강기 → 위층 연습 → 선반) + 저장/Continue | 단계별 PASS, 고리 시간·보스전 시간 MEASURE, Error0 |
| B23-T9 | doc | no | T8 | 사용자 안내: Play-V5-Loop 갱신, 고리 1회 10~15분, 플레이 전 질문 2개(① 보스 등장이 "나타나는" 느낌인가 ② A03 위층 연습과 승강기가 자연스러운가), 기록 칸(고리 시간·보스 시도 횟수·막힌 곳·머리 박은 곳) | README·TASKS 3·9절 |

- 허용 범위: 수정 가능 `Assets/Scripts/*`, `Assets/BossCandidates/B1/*`, `Assets/Candidates/B21HazardReturn.asset`(새 필드 추가만, 기존 값 유지), `Assets/Scenes/Test/*`, `Assets/Scenes/V5/*`(T6 재생성만), `Launchers/current/*`, `Tools/*`, `Validation/*`(새 파일), TASKS.md, README.md
- 수정 금지: `Design/v5/*`, `Design/proposals-*/*`, `Assets/Scenes/V4/*`, `PlayerTuning.asset`, 기본 `BossTuning.asset`(B3 값), PRD.md, GAME_DESIGN.md
- Backlog
  1. B23-BL1(verification, no, T5): 새 접근 규칙으로 A06 검사 결과만 기록(수정 없음) — 합격: A06 검출 목록 표
- 종료 조건: 전부 완료·BLOCKED 또는 예산 소진.

## 하지 않을 것
- 방 좌표 변경(기준 미달이면 수치·원인 기록 후 Claude 결정 대기).
- 모서리 보정 기본 켜기(D-CORNER-FIX 대기), 몸 크기 변경(D-BODY 대기).
- 보스2~4, 새 적 유형·새 패턴. push.

## 중단 조건
- B3 회귀 FAIL 2회 → T1의 B3 경로를 되돌리고 B1 전용으로 한정, 결정 대기로 기록.
- 승강기 `start:"low"`가 V4 승강기 동작을 바꾸면 → 되돌리고 결정 대기.
