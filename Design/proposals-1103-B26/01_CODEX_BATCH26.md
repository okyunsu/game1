이번 실행은 사용자 승인 Batch 26만 수행한다. AGENTS.md를 따른다. Ready Queue는 승인·기준·범위가 갖춰진 행만 진행한다.

## Batch 26
- 목표 상태: (1) D-REVIEW-CLEANUP 목록 21행이 처리돼 있고, TASKS 0절에 REVIEW 수가 세 칸으로 적혀 있다: 기존 QA 대상 6 / 목록 밖 기존 행(ID 포함) / 이번 Batch 신규. (2) draft6.1·아트 v1.1이 12방에 적용되고 정적·Scene FAIL0. (3) 전체 AB 고리 자동 경로가 완주하거나, 처음 막힌 단계와 원인(도구/입력/지형)이 원문과 함께 기록돼 있다. (4) Development 빌드 오류0·전체 회귀13/13.
- 시간 예산: 10시간(밤샘 1회), 종료 정리 40분 확보. Task 안전 상한 30개.
- 사용자 사전 점검: Unity Hub에 로그인한 상태로 실행을 유지했는지 확인(B25 첫 실행이 exit198로 25개 중 20개 BLOCKED였음).
- 시작 전: 작업 트리의 Claude 미커밋 변경 4개(`Design/proposals-1103-B26/*`, `Design/v5/WORLD_V5.md`, `Design/v5/rooms_v5_AB.json`, `Validation/D61-room-rules-v5-AB.md`)를 내용 변경 없이 `docs: V5 draft6.1 and B26 instructions (Claude)`로 커밋한다. **이 커밋은 사용자가 이 지시문으로 명시 승인한다.** 커밋 전 `Design/proposals-1103-B26/01_CODEX_BATCH26.md` 첫 줄 아래에 "목표 상태: (1) D-REVIEW-CLEANUP 목록 21행"이 있는지 확인하고, 없으면(수정 전 파일) 커밋하지 말고 이 지시문을 받은 메시지 원문을 그 파일에 덮어쓴 뒤 커밋한다(사용자 명시 승인, 이번 한정 Design 예외). 그 외 미커밋 변경이 있으면 그 파일만 건드리지 않고 보고한 뒤 계속한다.
- 시작: 첫 단계 `Tools/Preflight.ps1 -Batch 26`. 실패하면 Unity 필요 Task와 의존 경로만 BLOCKED, 아래 "Unity 없이 가능한 작업"과 그 Queue는 계속한다.
- Unity 없이 가능한 작업: B26-T0(REVIEW 정리·DECISIONS 기록), B26-T3(아트 파일 SHA 확인·스테이징·Import 설정 manifest), B26-T12(안내 문서), Q4(Claude용 요약).
- 자율 처리 단계: AGENTS.md "자율 처리 단계" 절(A/B/C). draft6.1 방 좌표는 **Claude 설계 원본을 사용자가 이 지시문으로 승인**한 것이므로 Scene 재생성은 A. 설계 JSON 수정은 C(하지 않음).
- 재시도: 최초 실패 후 같은 접근 1회(총2회), 접근 최대3개.
- 중단 규칙: **명령 하나가 거부되거나 실패하면 그 항목만 건너뛰고 사유를 기록한 뒤 다음 항목을 진행한다.** Batch 전체를 멈추지 않는다.
- 트랙(서로 독립):
  - A 정리: T0 — 합격: 목표 (1), DECISIONS 기록
  - B 설계 반영: T1~T2 — 합격: 정적·Scene FAIL0, 실측 표
  - C 아트: T3~T7 — 합격: 충돌체 불변, 스크린샷 24장
  - D 자동 경로: T8~T10 — 합격: 목표 (3)
- 사전 결정:

| 결정 | 답 | 날짜 | 출처·사용자 승인 근거 | 막히면 대체 |
| --- | --- | --- | --- | --- |
| D-REVIEW-CLEANUP | REVIEW 정리: 폐기 10(B15-T5, B16b-T2, B16b-T5, B16b-BL1, B17-T2, B17-T5, B18-T3, B18-T6, B21-T5, B22-T8 — V5/B25로 대체), 수락 3(B15-T2, B15-T3, B15-BL4 — 지도 기능, 자동 검증으로 충분), 미채택 종료 2(B21-T6, B21-BL2), QA 1회로 남김 6(B23-T1 보스1 등장, B22-T5 도랑 복귀, B23-T3 일어나기, B24-T11 보스2, B25-T14 그래픽, **B20-T4 캐릭터 크기 비교 D-BODY**) | 2026-11-03 | 사용자(Claude 앱 대화) | 목록의 ID가 REVIEW가 아니면 바꾸지 않고 현재 상태 보고. 목록 밖 REVIEW 행은 그대로 두고 ID 보고 |
| D-CORNER-FIX | 미채택(머리 충돌은 발판 간격 1.8 규칙·draft4.2~6.1로 해결). 후보 코드는 기본 꺼짐 그대로 둠 | 2026-11-03 | 사용자 | — |
| D-DRAFT6-1 | WORLD_V5 19절(B02 등불·위 통로 끊기, B06 등불×3, B05 조각 이동) | 2026-11-03 | 사용자(B26 내용 유지 승인), 설계 Claude | — |
| D-ART-1-1 | `02_ART_V1_1.md`(아트 원본은 f1a6f5a에 커밋됨) | 2026-11-03 | 사용자, 그림 Claude | — |
| D-B26-EXTRA | Backlog BL3~BL5(표시층), QA 안내에 신규 REVIEW 포함, T9 선행 T8, 방 이름 출처 | 2026-11-03 | 사용자 | — |

## Task

| Task | 유형 | needs-unity | 선행 | 내용 | 합격 기준 |
| --- | --- | --- | --- | --- | --- |
| B26-T0 | doc | no | 없음 | 사전 결정 5건을 DECISIONS.md에 기록. TASKS의 REVIEW 행을 D-REVIEW-CLEANUP대로 상태 변경(폐기=DEFERRED+사유 "V5로 대체", 수락=DONE, 미채택=DEFERRED+사유, QA 6=REVIEW 유지). 0절에 REVIEW 수 세 칸 | 목록 21행 처리, 변경 행 표, 목록 밖 REVIEW ID 목록 |
| B26-T1 | verification | yes | 없음 | draft6.1로 12방 재생성, 정적·Scene 검사, 요소 수 | 정적 FAIL0, Scene FAIL0 |
| B26-T2 | verification | yes | T1 | 실측: B02 바닥 등불2→섬→바닥 5/5(A04 레시피), B02 위 통로→끊긴 곳 하강→바닥 5/5(E3 피격 수 기록), B06 등불×3→끝 바닥·조각 5/5, B05 굴뚝→왼쪽 기둥 꼭대기 조각 3/3 | 각 표, 머리 접촉0 |
| B26-T3 | tool | no | 없음 | `Design/art_v1/`의 새 타일 10개(E1~E4·boss1·boss2·pickup_dash·pickup_walljump·checkpoint·lever)와 새 bg_A/bg_B(320×360)를 `Validation/_raw/26/art-staging/`에 복사·SHA 확인, Import 설정(16 PPU·Point·무압축)을 manifest로 기록. **Assets에는 넣지 않음**(.meta 규칙) | SHA 일치, manifest |
| B26-T4 | feature | yes | T1, T3 | 첫 단계: 스테이징 파일을 Unity에서 `Assets/Art/Original/v1/`로 Import, Unity가 만든 .meta와 함께 커밋. 이어서 적 종류별·보스·획득물·체크포인트·레버 스프라이트 적용(충돌체 불변) | .meta 포함 커밋, Scene 검사 원문 T1과 동일 |
| B26-T5 | feature | yes | T1, T3 | 배경 아래 기준 정렬·위쪽 `sky1` 단색(이음새 없음), 증기 기둥 불투명도 0.25 + 위로 오르는 입자 | 스크린샷, Scene 검사 원문 T1과 동일 |
| B26-T6 | verification | yes | T4 | B25 스크린샷의 정체불명 갈색 작은 사각형(약 0.4×0.8u, A01·A03·B02 등 플레이어 근처) 정체 확인 → 디버그·가이드 표시면 기본 꺼짐(B), 게임 요소면 알맞은 스프라이트 | 원인 기록, 스크린샷 |
| B26-T7 | verification | yes | T4, T5 | 12방 스크린샷 24장 + contact sheet + 목업 비교 | `Validation/_raw/26/art/` |
| B26-T8 | verification | yes | T1 | B25 Q5 방별 경로 실패 6건(A03 건물 앞, A05 바닥 접근, A06 출구 높이, B02, B04 인트로 뒤 벽 접근, B06) 재시도. **먼저 원인을 도구·입력·지형 중 하나로 분류**한다. 도구·입력이면 수정(A), 지형이면 위치·원문만 기록(C) | PASS 수, 남은 실패 원인 분류 |
| B26-T9 | verification | yes | T1, T8 | 전체 AB 고리(T8에서 고친 입력 사용): A03 CP → A04 → B1 → A05 대시 → A03 연습 → 선반 → B01 → B02 바닥 → B03 → B04 보스2 → B03 굴뚝 → B06 → B03 → B02 위 통로 → B01 탑 → B05 → A02 굴뚝, 저장/Continue | 완주 또는 처음 막힌 단계 원문, 구간별 시간 |
| B26-T10 | feature | yes | 없음 | 지도 PNG 저장을 화면 캡처 대신 RenderTexture로(B25 T6 캡처 실패) | 지도 PNG 2장(방문 전/후) |
| B26-T11 | verification | yes | T1~T10 중 끝난 것 | Development 빌드, 전체 회귀, 런처 smoke(Loop·Graybox·B1·B2·V4), B3 cycle/phase | 오류0, 13/13, Error0 |
| B26-T12 | doc | no | T11 | 사용자 QA 1회 안내: Play-V5-Loop 30~40분 + BodySize 비교, 기존 QA 6개와 **이번 Batch 신규 REVIEW 각각** 확인할 것 한 줄씩, 기록 칸(전체 시간·보스 시도·막힌 곳·머리 박은 곳·그래픽 헷갈린 곳·캐릭터 크기 선택) | README·TASKS 3·9절, 신규 REVIEW 누락0 |

- Base 커밋: 시작 시 HEAD(DOC-01 `b845145` 이후).
- 허용 범위: 수정 가능 `Assets/Scripts/*`, `Assets/Art/Original/*`, `Assets/Audio/*`, `Assets/Scenes/Test/*`, `Assets/Scenes/V5/*`(재생성), `Launchers/current/*`, `Tools/*`, `Validation/*`(새 파일), TASKS.md, README.md, RESOURCES.md, DECISIONS.md(T0 기록만) / 수정 금지 `Design/*`(시작 전 커밋 예외만), `Assets/Scenes/V4/*`, protected-files, B1·B2 Asset 값, PRD.md, GAME_DESIGN.md
- Backlog(위에서부터, 모두 승인: 사용자 2026-11-03, 허용 범위: Batch와 동일, 충돌체·판정 시점 불변):
  1. B26-BL1·feature·yes·선행 T4·HUD 조각 아이콘(`fragment`)+`n/4` — 합격: 스크린샷
  2. B26-BL2·feature·yes·선행 없음·Play-V5-Loop 실행 시 방별 체류 시간 기록 자동(B25 Q6 도구 연결) → `Playtest/` CSV — 합격: 30초 smoke에서 CSV 생성
  3. B26-BL3·feature·yes·선행 T4·적 E1~E4 사망 연출(입자 + 0.3s 사라짐, 사망 판정 시점 불변) — 합격: 4종 MEASURE
  4. B26-BL4·feature·yes·선행 없음·효과음 볼륨 정리(이벤트별 볼륨 표, 동시 재생 클리핑0) — 합격: 볼륨 표·클리핑 측정
  5. B26-BL5·feature·yes·선행 T4·획득물·조각·체크포인트 대기 반짝임(1.5s 주기) — 합격: 스크린샷 2장
- 종료 조건: 목표 상태 도달 또는 시간 예산 소진. 남은 시간은 Backlog → Ready Queue.

## Ready Queue

| ID | needs-unity | 승인 | 선행 | 허용 범위 | 내용 | 합격 기준 |
| --- | --- | --- | --- | --- | --- | --- |
| Q1 | yes | 사용자 2026-11-03 | T4 | Batch와 동일, 충돌체 불변 | 플레이어 표현: 진행 방향 좌우 반전, 점프·착지 늘였다 줄이기(0.1s), 대시 잔상 3장 | 이벤트 MEASURE, 충돌체 불변 |
| Q2 | yes | 사용자 2026-11-03 | T4 | Batch와 동일, 충돌체 불변 | 적·보스 피격 흰색 번쩍임 0.06s | MEASURE |
| Q3 | yes | 사용자 2026-11-03 | T1 | Batch와 동일 | 방 입장 시 방 이름 1.2s 표시. 표시는 "방 ID + `Design/v5/rooms_v5_AB.json` 각 방 `title`의 "—" 앞부분"(예: B02 + "압연 공장" → "B02 압연 공장"), title이 없으면 방 ID만 | 12방 확인 |
| Q4 | no | 사용자 2026-11-03 | 없음 | `Validation/` 새 파일만 | 이번 Batch 실패·REVIEW·Decisions Needed를 Claude용 한 장 `Validation/B26-for-claude.md`(방·좌표·원문 링크) | 문서 |

- push하지 않는다. 태그 B26.
