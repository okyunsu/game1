이번 실행은 사용자 승인 Batch 25(층 B 6방 + 그래픽 1차 + 수정 + 다듬기 Queue)만 수행한다. AGENTS.md를 따른다. Design/의 JSON·문서는 읽기만 한다. **이번 Batch는 밤샘 2회 분량이며, 필수 단계가 끝나거나 막혀도 아래 Ready Queue를 위에서부터 계속 진행한다.**

## Batch 25
- 시작 전: 작업 트리의 Claude 미커밋 변경(`Design/v5/*`, `Design/proposals-1101-B25/*`, `Validation/D60-room-rules-v5-AB.md`)을 내용 변경 없이 `docs: V5 draft6 floor B and B25 instructions (Claude)`로 먼저 커밋한다. **이 커밋은 사용자가 이 지시문으로 명시 승인한다.** 그 외 미커밋 변경이 있으면 멈추고 보고한다.
- 시작: `Tools/Preflight.ps1 -Batch 25`. 실패하면 Unity Task BLOCKED, 문서·도구 Task와 Queue의 no-unity 행만 진행.
- 설계 원본 전환: 이번부터 V5 생성 입력은 `Design/v5/rooms_v5_AB.json` + `world_v5_AB.json`(층 A 6방 + 층 B 6방 = 12방). `*_A.json`은 보존용.
- 읽을 범위: AGENTS.md, TASKS.md 0·1·2·3절, `Design/v5/WORLD_V5.md` 16~18절, `Design/proposals-1101-B25/*`, `Design/proposals-1101/03_BOSS2_SPEC.md`, `Validation/B24-summary.md`, `Validation/D60-room-rules-v5-AB.md`.
- 예산: AI 실행 기준 밤샘 2회분. Task 상한 32개. 단계 경계에서만 예산 판단.
- 진행 규칙: 한 단계가 실패·BLOCKED여도 의존하지 않는 다음 단계·Queue는 계속. 단계마다 Quiet 1회, 전체 회귀·빌드·런처는 P5.
- 재시도: 같은 접근 1회(총 2회). 실패면 되돌리고 기록, 다음 Task.
- 기준 미달(도달·머리·보스 회피)이면 좌표·수치를 고치지 말고 원문·위치를 기록해 Claude 결정 대기로.

| 결정 | 답 | 날짜 | 출처 |
| --- | --- | --- | --- |
| D-FLOOR-B6 | 층 B 6방 draft6(WORLD_V5 18절) | 2026-11-01 | 사용자 |
| D-ART-1 | 그래픽 1차 적용(02_GRAPHICS) | 2026-11-01 | 사용자 |
| D-B24-B2-GATE | 전투 시작 시 획득대 바닥으로 내려감 | 2026-11-01 | Claude 추천(B24 결정 대기 해소안) |
| D-B23-GEOMETRY | A02 조각 길 발판 삭제·조각 선반 이동(벽 점프 재방문) | 2026-11-01 | Claude 추천 |
| D-A03-B01 | 서쪽 벽 대시 문 → 선반 끝 천장 구멍 | 2026-11-01 | Claude(world 겹침 정정) |

## P1 — 수정
| Task | unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B25-T0 | no | 결정 표를 TASKS 2-1, B24 결정 대기 2건(D-B24-B2-GATE·D-B23-GEOMETRY) 처리 | 표 |
| B25-T1 | yes | **승강기 재탑승 버그**(B24 T3/T6: 반환 호출 뒤 승강기만 올라감). 원인 찾아 제품 코드 수정, V4 승강기 무변화 | A03 승강기 왕복 3회·다른 층에서 호출 후 탑승 PASS, V4 B01·C01 회귀 |
| B25-T2 | yes | 보스 획득대 `sink_on_fight`: 전투 시작 시 받침이 바닥 높이로 내려감(B2) | 받침 위 일반 점프 회피 0/10, 기존 바닥 10/10 피격 유지 |

## P2 — 층 B 생성
| Task | unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B25-T3 | no | 검사기: **증기**를 아래→top 상승 연결로 도달성에 반영(승강기처럼) | B02 위 통로·B03 선반·B04 문 도달 WARN 소멸, self-test |
| B25-T4 | yes | 생성기: AB 파일 입력, 천장/바닥 구멍 문(A03-B01, A02-B05, B05-C01 stub), B04 `boss`(B24 아레나 코드 재사용, 입구 오른쪽, 획득대 sink), `cover` 뒤 조각, 새 CP-B01 | fixture·생성 로그 |
| B25-T5 | yes | 12방 생성(A 6 재생성 포함) + 정적·Scene 검사 + 요소 수 | 정적 FAIL0, Scene FAIL0, 요소 일치 |
| B25-T6 | yes | 지도 UI에 층 B 6방·문 표시 | 지도 스크린샷, 방문/미방문 |

## P3 — 층 B 실측
| Task | unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B25-T7 | yes | 첫 통과(대시만): A03 선반 → 받침 점프 → B01(탑 아래 통로) → 지붕 → B02 바닥 길(등불 2·E2·산성 대시·E1) → B03 증기 2회 → B04 문 | 구간별 5회 성공, 머리 접촉0, 산성 복귀 |
| B25-T8 | yes | B02 위 길(증기 → 위 통로 → 조각, E3 사격 회피), B06 건너기(등불·가시 포고·무너지는 발판) | 각 5회, 조각 수집 |
| B25-T9 | yes | B04 보스2 방 안: 획득 → 3 이탈 → 잠금·연출 → 처치(벽 점프), 사망 → CP-B01 → 재진입 재시작 | B24 T12 기준 동일 |
| B25-T10 | yes | 벽 점프 재방문: B01 탑 굴뚝 → 받침 조각 → B05, B05 굴뚝 ↔ A02 굴뚝, B05 부서지는 벽 조각, B03 동쪽 굴뚝 → B06, A02 조각 선반 | 각 경로 3회 성공 |
| B25-T11 | yes | 조각 합계: 첫 통과 4개(A01 선반·A04 섬·A01 감실·B02 위 통로)로 최대 HP +1 | 최대 HP 5 MEASURE |
| B25-T12 | yes | 확장 고리 자동 경로: A03 CP → 승강기 → A04 → B1 → A05 → A03 → 연습 → 선반 → B01 → B02 → B03 → B04 보스2 → B03 → B06 → B03 → B02 → B01, 저장/Continue | 단계별 PASS, 전체 시간 MEASURE |

## P4 — 그래픽 1차: 직접 제작 아트 (02_GRAPHICS, `Design/art_v1/`)
| Task | unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B25-T13 | no | `Design/art_v1/`의 직접 제작 타일·배경을 `Assets/Art/Original/v1/`로 복사(원본은 Design에 그대로). Import: 16 PPU, Point 필터, 압축 없음. RESOURCES.md에 "직접 제작(Claude 생성 스크립트 `Design/art_v1/make_art.py`), 외부 라이선스 없음" 등록. 외부 에셋 다운로드 금지 | import 설정 기록 |
| B25-T14 | yes | 생성기에 표시 층(02_GRAPHICS 매핑표): solid=층별 벽(윗면은 _top), platform=층별 발판, crumble·climb·acid·spike·door·lantern·fragment 스프라이트, 층별 패럴랙스 배경(bg_A/bg_B, 0.3배 이동). 플레이어·적은 player/enemy 스프라이트, 충돌체 불변 | 충돌체 변경0(정적·Scene 결과 동일) |
| B25-T15 | yes | 12방 적용 + `Play-V5-Graybox.cmd` 비교 런처 | 런처 Error0 |
| B25-T16 | yes | 방마다 스크린샷 2장 `Validation/_raw/25/art/` + `Design/art_v1/mockups`와 나란히 비교 1장 | 24장+비교 |

## P5 — 통합
| Task | unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| B25-T17 | yes | Development 빌드, 전체 회귀, 런처 smoke(Loop·B1·B2·Graybox·V4) | 오류0, 13/13, Error0 |
| B25-T18 | no | 사용자 안내(00_README 검수 절), TASKS 3·9절 | 문서 |

## Ready Queue (P5 뒤, 또는 앞 단계가 막혔을 때 위에서부터. 각 행 독립)
| ID | unity | 내용 | 합격 기준 |
| --- | --- | --- | --- |
| Q1 | yes | 타격·피격·적 처치 효과음과 짧은 입자(기존 AttackFeedback 확장), 볼륨 값 Asset | 이벤트별 재생 MEASURE, Error0 |
| Q2 | yes | 조각 획득·문 잠김/열림·승강기·증기 효과음 | 이벤트별 재생 |
| Q3 | yes | 보스 등장 카메라 살짝 당기기(줌 0.9, 1.8s 연출 안), 보스 처치 정지 프레임 0.1s | B1·B2 연출 MEASURE, B3 무변화 |
| Q4 | yes | 적 예고 색(공격 직전 0.3s 밝아짐) E1·E2·E3 | 예고 MEASURE |
| Q5 | yes | 방별 자동 경로 회귀 묶음(12방 각 진입→주요 출구) `Tools/Run-RoomRoutes` | 12/12 PASS 또는 실패 원문 |
| Q6 | no | 플레이 시간 측정 도구: 방별 체류 시간 CSV를 Playtest/에 저장(사람 플레이용) | CSV 형식·README |

- 허용 범위: 수정 가능 `Assets/Scripts/*`, `Assets/Art/Original/*`(직접 제작 아트 import), 표시 층 스크립트, `Assets/Audio/*`, `Assets/BossCandidates/B2/*`, `Assets/Scenes/Test/*`, `Assets/Scenes/V5/*`(재생성), `Launchers/current/*`, `Tools/*`, `Validation/*`(새 파일), TASKS.md, README.md, RESOURCES.md
- 수정 금지: `Design/*`, `Assets/Scenes/V4/*`, `PlayerTuning.asset`, 기본 `BossTuning.asset`(B3), B1 Asset 값, PRD.md, GAME_DESIGN.md
- push하지 않는다. 태그 B25.

## 중단 조건
- B1·B2·B3 회귀 FAIL 2회 → 해당 변경 되돌리고 다음으로, 결정 대기 기록.
- Unity exit198 → Unity Task BLOCKED, no-unity 행만 계속.
