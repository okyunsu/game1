P3: 보류
(사용자: 위 줄을 `P3: 승인`으로 바꾸면 조각→최대 HP 단계를 실행한다.)

이번 실행은 사용자 승인 Batch 24(확장, 5단계)만 수행한다. 기존 Batch 4~5개 분량을 한 번에 진행하고 사람 검수는 마지막 1회로 모은다. AGENTS.md를 따른다. Design/의 JSON·문서는 읽기만 한다.

## Batch 24 (확장)
- 시작 전: 작업 트리에 Claude가 넣은 미커밋 변경(`Design/v5/*`, `Design/proposals-1101/*`, `Validation/D43-room-rules-v5-A.md`, `Validation/D50-room-rules-v5-A.md`)이 있으면 내용을 바꾸지 말고 `docs: V5 draft5 and B24 instructions (Claude)`로 먼저 커밋한다. **이 커밋은 사용자가 이 지시문으로 명시 승인한다.** 그 외 미커밋 변경이 있으면 멈추고 보고한다.
- 시작: `Tools/Preflight.ps1 -Batch 24`. 실패하면 needs-unity=yes Task를 시작하지 않고 BLOCKED 기록 후 종료.
- 읽을 범위: AGENTS.md, TASKS.md 0·1·2·3절, `Design/v5/WORLD_V5.md` 16·17절, `Design/proposals-1101/*`, GAME_DESIGN.md 12-2·13절, `Validation/D50-room-rules-v5-A.md`, `Validation/B23-resume-summary.md`.
- 예산: AI 실행 기준 밤샘 2회분까지. Task 안전 상한 24개. **단계 경계에서만** 예산 판단: 다음 단계 전체를 끝낼 시간이 없으면 시작하지 않고 종료 정리.
- 진행 규칙: 한 단계의 Task가 실패·BLOCKED여도 **의존하지 않는 다음 단계는 계속**한다. 단계마다 종료 시 기본 회귀(Quiet) 1회. 전체 회귀·빌드·런처는 P5에서 1회.
- 재시도: 같은 접근 1회(총 2회). 두 번째 실패면 되돌리고 실패 기록만 커밋, 다음 Task로.
- 검증 입력(자동 경로)은 제품 수치·좌표를 바꾸지 않고 입력 쪽에서 해결한다.

| 결정 | 답 | 날짜 | 출처 |
| --- | --- | --- | --- |
| D-HUB-STREET | 허브 건물 거리(draft4.3) | 2026-11-01 | 사용자 |
| D-BIG-BATCH | 큰 묶음, 사람 검수 마지막 1회 | 2026-11-01 | 사용자 |
| D-DRAFT5 | A01 대시 감실·E4/E2 배치·A06 정리 | 2026-11-01 | Claude 추천(사용자 범위 확대 요청) |
| D-BOSS2 | 03_BOSS2_SPEC 시험 아레나 | 2026-11-01 | 원칙은 사용자(D-BOSS4), 수치 Claude 추천 |
| D-FRAGMENT-HP | 맨 위 줄이 `승인`일 때만 | — | 사용자 |

## P1 — draft5 반영과 머리 충돌 확인
| Task | needs-unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B24-T0 | no | 00_README 플레이 기록·결정을 TASKS 2-1·10절에 반영 | 표 행 추가 |
| B24-T1 | yes | draft5로 V5 6방 재생성. 정적·Scene 검사 | 정적 FAIL0, Scene FAIL0(B23 A02 overhead 포함), 요소 수 일치(A01 E4 1·감실 발판, A02·A05 E2) |
| B24-T2 | yes | 머리 충돌 실측: A03 상자→M→굴뚝→옥상 각 5회, 처마 밑 점프 5회, A02 오름 3구간·조각 길 2구간, A04 위 길 2구간, A06 수정 2구간 | 머리 접촉 0, 착지 5/5 |
| B24-T3 | yes | 고리 자동 경로 복구(B23 T8 실패: A04 일반 적·무너지는 발판). A03 CP → 승강기 → A04 → B1 → A05 대시 → 도랑 → A03 → 건물 → 옥상 → 승강기 → 연습 → 선반, 저장/Continue | 단계별 PASS, 고리·보스전 시간 MEASURE |

## P2 — 층 A 내용 확인
| Task | needs-unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B24-T4 | yes | A01 대시 감실: 무능력 0/5, 대시 5/5, 조각 수집 | 표 |
| B24-T5 | yes | 적 배치: A01 E4(가시 구덩이 위, 포고 튕김 1회), A02 E2(돌진 → 점프로 넘김 5회), A05 E2(대시로 피함 5회). 적이 문 안전 구역·구덩이·hazard 규칙 위반 없음 | 표, 피해·사망 원문 |
| B24-T6 | no | 고리 자동 경로에 A01 대시 감실 왕복 추가(대시 재방문), 고리 시간 MEASURE | T3 경로 + 재방문 PASS |

## P3 — 조각 → 최대 HP (맨 위 줄이 `P3: 승인`일 때만, 아니면 SKIPPED 기록)
| Task | needs-unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B24-T7 | no | 02_FRAGMENT_HP의 PRD 패치 3곳을 문장 그대로 적용(수정 전 문장 불일치 시 적용하지 않고 보고) | diff 3곳 |
| B24-T8 | yes | 조각 ID 저장·HUD `조각 n/4`·4개마다 최대 HP +1·즉시 회복·안내 1.5초. 구 저장 로드 호환 | 저장/로드/새 게임 초기화 PASS, 4번째 조각에서 최대 HP 5(기본4+1) MEASURE |

## P4 — 보스2 시험 아레나 (03_BOSS2_SPEC)
| Task | needs-unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B24-T9 | yes | 충격파 높이 2.8·양방향을 보스별 Asset 값으로(B1·B3 기존 값 유지) | B1·B3 회귀 무변화 |
| B24-T10 | yes | 벽 점프 획득대 + "획득 후 획득대에서 3 멀어짐" 트리거 → 잠금 → 등장 연출(B23 틀) | 트리거·잠금·연출 1.8s MEASURE |
| B24-T11 | yes | `Assets/BossCandidates/B2/BossTuning.asset`(HP26·피해1·높은 충격파→돌진→낙하물·첫 사이클 예고1.0s) + `BossTest_B2` 씬(양쪽 ClimbableWall) | 패턴 3종만 관측, 1페이즈 |
| B24-T12 | yes | 검증: ① 벽 점프 끈 상태 높은 충격파 10회 피격(못 피함) ② 벽 점프로 고정 입력 처치 ③ 사망→복귀·벽 점프 유지·재진입 재시작 ④ 처치 저장(`B2-B04`)·재진입 시 없음 | 각 MEASURE, Error0 |

## P5 — 통합
| Task | needs-unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B24-T13 | yes | Development 빌드, 전체 기본 회귀, 런처 smoke(Play-V5-Loop, Play-BossTest-B1, **Play-BossTest-B2**, V4 기존) | 오류0, 회귀13/13, 런처 Error0 |
| B24-T14 | no | 사용자 안내: 검수 1회(약 20~30분) 순서·질문·기록 칸(00_README 검수 절) | README·TASKS 3·9절 |

- 허용 범위: 수정 가능 `Assets/Scripts/*`, `Assets/BossCandidates/B2/*`, `Assets/Scenes/Test/*`, `Assets/Scenes/V5/*`(재생성), `Launchers/current/*`, `Tools/*`, `Validation/*`(새 파일), TASKS.md, README.md, PRD.md(**P3 승인 시 T7 패치 3곳만**)
- 수정 금지: `Design/*`, `Assets/Scenes/V4/*`, `PlayerTuning.asset`, 기본 `BossTuning.asset`(B3), `Assets/BossCandidates/B1/*` 값, GAME_DESIGN.md
- 기준 미달(머리 접촉·도달·보스 회피)이면 좌표·수치를 고치지 말고 원문과 위치를 기록해 Claude 결정 대기로.
- push하지 않는다. 태그 B24.

## 중단 조건
- B1·B3 회귀 FAIL 2회 → 해당 단계 변경 되돌리고 다음 단계로, 결정 대기 기록.
- Unity 라이선스 exit198 → Unity Task BLOCKED, 문서·도구 Task만 계속.
