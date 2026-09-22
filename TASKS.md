# Development Plan / Execution Queue

현재: 2026-09-16 개발 착수. 운영 기준 v0.2, Target 18방 유지. 사용자가 N1(S1-01~05)과 Unity 설치를 승인했다. 2026-09-17 N1 기술 구현·Unity 검증 완료. S1-01/02 DONE, S1-03/04/05 REVIEW. 사용자 이동감 수락 전이며 S1-06 이후는 미착수다.

## 운영 기준

분류: AUTO=독립 수행, REVIEW=구현 후 사용자 Unity 검토, DECISION=사용자 결정 전 변경 구현 금지.

상태: TODO(계획), READY(착수 승인·의존성 충족), IN_PROGRESS, IMPLEMENTED(구현·Unity 기술 검증 완료), REVIEW(사용자 검토 등록), DONE(최종 완료), BLOCKED, DEFERRED. Dependency의 `:IMPLEMENTED`는 IMPLEMENTED·REVIEW·DONE에서 충족하고, `:DONE`은 DONE에서만 충족한다. 기술 검증 무효화 시 후속 경로도 재검증하며 세부 기준은 AGENTS를 따른다. `없음`은 의존성만 없다는 뜻이며 자동 개발 승인은 아니다.

개별 DoD에 AGENTS의 공통 완료 조건을 추가 적용한다. 기술적 완료 조건만 먼저 충족하면 IMPLEMENTED로 기록하고, 사용자 감각·설계 수락 조건은 REVIEW→DONE에서 확인한다. 기술 검증 완료를 사용자 수락으로 대체하지 않는다. 구현 Task는 Unity 실행·회귀 확인·Editor 조정 가능성·상태 갱신·인계를 포함한다. DECISION과 기록 Task는 실행 자체 대신 판단·증거·반영을 검증하며 Unity 실행 조건은 비적용이다.

사용자 일일 예산: 인계 확인 10분, 실제 플레이 30~45분, Unity 수치·배치 조정 15~25분, 결정·증거·다음 큐 승인 5~20분(총 60~100분). 전체 플레이테스트 날은 다른 검토를 줄여 120분까지 배정한다. 외부 테스트는 사용자가 관찰 가능한 시간에 1회씩 예약한다.

에이전트 작업은 Task당 대체로 1~4시간, 검토는 5~20분으로 잡는다. 전체 플레이테스트·주간 검토는 별도 시간 예산이다. 매일 실제 완료량과 남은 검토 시간을 확인하며 검토 대기 작업을 무제한 쌓지 않는다.

### 승인된 실행 묶음과 야간 운영

2026-09-16 사용자 승인: N1 전체(S1-01 → S1-02 → S1-03 → S1-04 → S1-05). Unity 미설치 확인 후 설치부터 진행하도록 추가 승인됨. S1-06 이후는 이번 실행 범위 밖이다. 개발 지시 시 작업 범위를 묶음으로 기록하고 같은 범위의 매 Task마다 재승인을 요청하지 않는다. 묶음 승인도 Dependency와 Sprint Gate를 면제하지 않는다.

승인된 N1: S1-01 → S1-02 → S1-03 → S1-04 → S1-05. 각 작업의 기술 검증 후 다음 의존성을 평가한다. 마지막 세 작업은 사용자 부재 중 IMPLEMENTED/REVIEW 상태로 연속 진행 가능하다. 종료 시 이동감 검토 묶음 1개로 인계하고 S1-06/07은 기다린다. 기술 검증 실패·규칙 변경 필요 시 해당 경로를 중단한다.

후보 N2: 이동감 세 작업 모두 DONE 후 S1-06, S1-07 → S1-08 → S1-09 → S1-10. Agent Queue는 이 중 지금 실행 가능한 최대 5개만 표시하며 완료할 때마다 갱신한다. 카메라·Slice 검토를 남기고 S1-G에서 사용자 판단을 기다린다.

구현 연속 실행은 기능적 통합을 위한 것이다. 최종 감각·진행 방향에 대한 승인은 DONE 의존성 및 주간 Gate에 유지한다. 모든 Gate는 해당 Sprint의 활성 REVIEW Task가 DONE인지 확인하며, 승인된 DEFERRED는 제외 사유를 대조한다.

## Development Schedule

### Sprint 1 — Foundation / Vertical Slice

Week 1. 목표: 핵심 이동과 짧은 플레이 구간 완성. Unity Project Setup, Player Movement, Camera, Basic Tilemap, Room Structure, Checkpoint, 첫 Vertical Slice를 포함한다.

Day 1 환경·입력, Day 2~3 이동, Day 4 카메라·방, Day 5 체크포인트·구간, Day 6 사용자 검토, Day 7 수정 여유·Gate. 이동 파라미터를 직접 바꾸며 재미를 판단할 수 있어야 한다.

| ID | Task | 분류 | Status | Dependency | Agent Work | Human Review | Definition of Done |
| --- | --- | --- | --- | --- | --- | --- | --- |
| S1-01 | Unity Project Setup | AUTO | DONE | 없음 | 설치된 Unity 6 확인, 정확한 버전·호환 패키지 고정, PRD 폴더·Boot/Menu 시작 경로 구성, README 기록 | 버전·열기 방법 확인 5분 | 빈 프로젝트 Play·Windows 기본 빌드 성공, 불필요한 패키지 없음 |
| S1-02 | Input System | AUTO | DONE | S1-01:DONE | 키보드·패드 Action, Gameplay/UI 맵, 입력 전환·연결 해제 Pause 처리 | 두 장치 입력 확인 5분 | 명세 버튼 일치, UI 입력이 게임에 새지 않음 |
| S1-03 | 좌우 이동·기본 점프·낙하 | REVIEW | REVIEW | S1-02:DONE | Rigidbody2D 이동·접지·공중 제어, PlayerTuning·Prefab, 평지·발판만 있는 기술 검증 공간(본격 Room 배치 제외) | 가감속·방향 전환·공중 조작 15분 | 벽 접지 없음, Inspector 변경 적용, 기본 조작 수락 |
| S1-04 | Coyote Time / Jump Buffer | REVIEW | REVIEW | S1-03:IMPLEMENTED | 경계 시간 입력 보정, 단일 소비, 전환 시 초기화 | 0.08/0.12초 이탈·0.10/0.15초 착지 비교 10분 | 허용 경계 안팎 기대대로 작동, 중복 점프 없음 |
| S1-05 | Variable Jump Height | REVIEW | REVIEW | S1-04:IMPLEMENTED | Jump Cut·하강 배율·최대 낙하속도, 튜닝 항목 | 짧게/길게 누른 높이 비교 10분 | 높이 차이 명확, 30/60/120fps 동작 확인 |
| S1-06 | Camera | REVIEW | TODO | S1-03:DONE, S1-04:DONE, S1-05:DONE | Cinemachine 추적·방 경계·착지 시야 | 급반전·낙하 시야 확인 10분 | 떨림·시야 밖 필수 착지 없음, 설정 위치 안내 |
| S1-07 | Basic Tilemap / Room Structure | AUTO | TODO | S1-01:DONE, S1-03:DONE, S1-04:DONE, S1-05:DONE | Ground/Hazard/출입구 규약, Room Scene·Spawn ID, 전환 관리자, A01~A04 그레이박스 | 출입구·Tilemap 편집 확인 5분 | 4방 양방향 연결, 플레이어 중복·벽 끼임 없음; 플레이 통합은 S1-09 |
| S1-08 | Checkpoint / 최소 사망 복귀 | AUTO | TODO | S1-05:IMPLEMENTED, S1-07:DONE | CP-A01/A03, 런타임 체크포인트, Kill Zone 사망·최대 HP 복귀 | Spawn 위치·반복 사망 5분 | 능력 없는 초기 상태에서 안전 복귀; 디스크 저장은 S2-06 |
| S1-09 | 첫 Vertical Slice 통합 | REVIEW | TODO | S1-06:IMPLEMENTED, S1-08:DONE | A01~A04 이동 구간 5~10분, A03 닫힌 게이트 외형, 안내 배치 | 이동만으로 15분 플레이·조정 | 시작~A04 왕복·사망 복귀 가능, 공격·능력 미구현임을 표시 |
| S1-10 | Slice 검증·실행 안내 | AUTO | TODO | S1-09:IMPLEMENTED | Windows Slice 빌드, 경계 입력·충돌 회귀, README 재현 경로 | 빌드 실행 확인 5분 | 빌드에서 구간 재현, 명백한 런타임 오류 없음, 결과 인계 |
| S1-G | Week 1 Scope Gate | DECISION | TODO | S1-06:DONE, S1-09:DONE, S1-10:DONE | 이동·Slice 결과와 문제·잔여 일정을 정리 | 이동감 수락 또는 수정 우선순위 결정 15분 | 사용자 판단 기록; 통과 전 S2 콘텐츠 착수 금지 |

Gate 실패 시 확대하지 않고 같은 이동·Slice Task를 재개한다. 통과 결정이 있어야 S1-G를 DONE으로 둔다.

### Sprint 2 — Core Game Loop

Week 2. 목표: Dash 획득 → 기존 Gate 재방문·해금 → 전투·사망·복귀의 핵심 루프 완성. 적은 우선 E1 한 종류만 만든다.

Day 8~9 Dash·Gate, Day 10~11 전투·적·피격, Day 12 저장·루프, Day 13 검토, Day 14 수정 여유·Gate.

| ID | Task | 분류 | Status | Dependency | Agent Work | Human Review | Definition of Done |
| --- | --- | --- | --- | --- | --- | --- | --- |
| S2-01 | Dash 획득·실행 | REVIEW | TODO | S1-G:DONE | A05 능력 접촉, 대시 상태·쿨다운·공중 횟수, HUD 상태 | 거리·벽 충돌·연타 15분 | 일반 벽 통과·무한 체공 없음, 획득 전 사용 불가 |
| S2-02 | Ability Gate G-D | REVIEW | TODO | S2-01:IMPLEMENTED | A03↔B01 격자, A04↔A05 연결, 안전 연습·재방문 표식 | 획득 전후 Gate 경험 10분 | 능력과 대시 상태 모두 필요, 걷기·점프·넉백 우회 불가 |
| S2-03 | 기본 공격 1종 | REVIEW | TODO | S1-G:DONE | 지상·공중 공격 판정, 우선순위·취소, CombatTuning | 공격 거리·공중 제어 10분 | 1회당 대상별 피해 1번, 추가 공격 종류 없음 |
| S2-04 | E1 순찰형 | AUTO | TODO | S2-03:IMPLEMENTED | EnemyTuning·Prefab, 순찰·벽/낭떠러지 처리·피격·제거, A04 배치 | 위치·타수 확인 5분 | 한 패턴·HP 2, 배치 수정 가능, 재입장 초기화 |
| S2-05 | Health / Damage / Death 확정 | AUTO | TODO | S2-01:IMPLEMENTED, S2-04:DONE, S1-08:DONE | 접촉 피해·무적·넉백, 상태 취소, 적 초기화·능력 유지 | 연속 피격·죽음·복귀 10분 | 동시 피해 중복 없음, 대시 중 사망도 중력 복원, 3초 내 복귀 목표 확인 |
| S2-06 | 단일 슬롯 Save / Continue | AUTO | TODO | S2-02:IMPLEMENTED, S2-05:DONE | 진행 스키마·안전 파일 교체, 능력/CP 저장, Menu New/Continue/Quit, 덮어쓰기 확인 | 종료·재개와 초기화 확인 10분 | 획득 직후 종료/사망 보존, 손상·쓰기 실패 안내, README 저장 위치 기록 |
| S2-07 | 첫 능력 루프 통합 | REVIEW | TODO | S2-06:DONE | A01~A05·B01, G-J 관찰 외형·CP-B01, Pause·기본 안내 통합 | 무힌트 Dash 획득→B01 도달 20분 | 게이트 기억·복귀 기록, 복귀 90초 목표 측정, 소프트락 없음 |
| S2-08 | 핵심 루프 회귀 검증 | AUTO | TODO | S2-07:IMPLEMENTED | 키보드·패드 입력, 저장·게이트·리스폰 조합 검증 및 빌드 | 실패 건 재현 검수 5분 | 수행·미수행 검증 구분, 진행 차단 오류 해결 |
| S2-G | Week 2 Scope Gate | DECISION | TODO | S2-01:DONE, S2-02:DONE, S2-03:DONE, S2-07:DONE, S2-08:DONE | 핵심 루프 재미·안정성·남은 일정 요약 | 확장 가능 여부·15방 또는 Minimum 11방 전환 판단 15분 | 통과 기록 또는 수정 목록; 통과 전 S3 확장 금지 |

Gate 실패 시 월드 확장 대신 루프를 수정한다. 축소는 TASKS의 Scope Decision 기록과 PRD·GAME_DESIGN 변경을 동반한다.

### Sprint 3 — Content Complete

Week 3. 목표: Double Jump, 추가 지역, 적 Target 3종 / Minimum 2종, 보스, 전체 월드, 엔딩을 연결하여 처음부터 끝까지 플레이 가능하게 한다.

Day 15~16 Double Jump·B, Day 17~18 C·적·보스, Day 19 통합·엔딩, Day 20 완주, Day 21 수정 여유·Gate. 외부 신규 참가자는 Target 2명 / Minimum 1명 기준으로 이 주에 Week 4 시간을 확보한다.

| ID | Task | 분류 | Status | Dependency | Agent Work | Human Review | Definition of Done |
| --- | --- | --- | --- | --- | --- | --- | --- |
| S3-01 | Double Jump | REVIEW | TODO | S2-G:DONE | 추가 점프·소비 순서·저장·HUD, B04 획득 지점 | 낙하·Coyote·대시 조합 15분 | 3번째 점프 불가, 획득 후 종료/복귀 유지 |
| S3-02 | B02~B05 / G-J | REVIEW | TODO | S3-01:IMPLEMENTED | B 필수 경로, 높은 턱·안전 연습, 재방문 표식 | G-J 전후·B04→B01 동선 20분 | 실제 점프 높이로 게이트 검증, 재방문 120초 목표 측정 |
| S3-03 | E2 돌진형 | AUTO | TODO | S3-02:IMPLEMENTED | E2 Prefab·예고·돌진·회복, B02 배치 | 예고·회피 공간 5분 | 벽에서 정지, 예고 이후 방향 고정, 수치 조정 가능 |
| S3-04 | E3 고정 사격형 | AUTO | TODO | S3-02:IMPLEMENTED | E3·탄 Prefab, 예고·감지·벽 차단·정리, B03 배치 | 화면 안 예고 확인 5분 | 탄 벽 충돌·방 전환 정리, 감지 밖 공격 없음 |
| S3-05 | C01~C04 / 전체 필수 연결 | REVIEW | TODO | S3-01:DONE, S3-02:DONE, S3-03:DONE, S3-04:DONE | 탑 그레이박스·CP-C01/C04·조합 도전·경계 전환 | 순차 난이도·안전 착지 20분 | B05부터 C04 왕복, 새 능력 없이 진행 불가, 기존 능력으로 통과 |
| S3-06 | C06 / 최종 보스 | REVIEW | TODO | S3-05:IMPLEMENTED | 단일 페이즈·패턴 2개·BossTuning·출구 잠금·재도전 | 예고·공격 기회·재도전 길 20분 | 승리·사망 모두 종료, 15초 이내 재진입 목표 확인 |
| S3-07 | 엔딩·처치 저장 | AUTO | TODO | S3-06:IMPLEMENTED | 보스 처치·엔딩 플래그, Ending 화면·타이틀 버튼, 동시 사망 처리 | 처치 직후 종료·Continue 확인 5분 | 첫 처치→엔딩, 이후 Continue→엔딩, 새 게임 초기화 |
| S3-08 | 선택 방 A06/B06/C05 | REVIEW | TODO | S3-07:DONE | 확정 범위가 18방일 때만 3개 소규모 이동 도전 배치 | 분기 규모·필수 경로 혼동 10분 | 진행 보상·새 시스템 없음, 각각 1~3분 왕복; 15방 또는 Minimum 결정 시 DEFERRED |
| S3-09 | Content Complete 완주 검사 | AUTO | TODO | S3-07:DONE | 적용 범위의 필수 연결·적(Target 3/Minimum 2종)·2능력·저장·엔딩 통합 빌드, 선택 방 상태 대조 | 처음~끝 실제 플레이, Target 예산 60~120분 / Minimum 실제 시간 기록 | 그레이박스여도 엔딩 도달, 적용 방 수와 표 일치, 결과 기록 |
| S3-G | Week 3 Scope Gate | DECISION | TODO | S3-05:DONE, S3-06:DONE, S3-09:DONE | 완주 증거·오류·S3-08 상태·Week 4 여력 정리 | 콘텐츠 고정·축소 결정 15분 | S3-08은 DONE 또는 승인된 DEFERRED, 완주 가능 상태 확정 |

S3-09는 선택 방을 기다리지 않고 필수 완주를 우선 확인한다. S3-G에서 최종 방 수를 확정한다. 완주 실패 시 신규 콘텐츠를 중단하고 진행 차단 수정에 집중한다.

### Sprint 4 — Playtest / Polish / Portfolio Evidence

Week 4. 목표: 완성도를 높이고 설계 개선의 근거를 확보한다. 새로운 핵심 시스템은 추가하지 않는다.

Day 22~24 관찰 테스트 Target 3회 / Minimum 2회·원인 분석, Day 25~26 동선·난이도·이동감·최소 연출, Day 27 빌드·회귀·근거 정리, Day 28 여유·최종 검수. 참가자 플레이는 하루 한 세션으로 관찰 시간을 분산한다.

| ID | Task | 분류 | Status | Dependency | Agent Work | Human Review | Definition of Done |
| --- | --- | --- | --- | --- | --- | --- | --- |
| S4-01 | Full Playtest 준비 | AUTO | TODO | S3-G:DONE | 고정 빌드·검사표·관찰 항목·초기 수치 기록, 저장 초기화 안내 | 테스트 순서·참가자 일정 10분 | 동일 빌드·장치·조건 기록 가능, 미실시 결과 없음 |
| S4-02 | Full Playtest (적용 범위별) | REVIEW | TODO | S4-01:DONE | 개발자 1회·신규 외부 Target 2명 / Minimum 1명 각 1회 기록을 정리, 원인 후보 분류 | 직접 플레이·관찰, Target 세션별 60~120분 / Minimum 실제 시간 | 23절 지표 누락 여부 확인, 실제 시간·오류·동선 확보; 미확보는 BLOCKED |
| S4-03 | 개선 우선순위 결정 | DECISION | TODO | S4-02:DONE | 시간·반복 사망·길찾기·이동감 근거와 조정안 제시 | 무엇을 바꿀지 수치·배치 단위로 결정 15분 | 승인 변경 목록·성공 기준·영향 문서 기록; 새 기능 없음 |
| S4-04 | 이동감 / 레벨 동선 / 난이도 개선 | REVIEW | TODO | S4-03:DONE | 승인된 수치·배치·표식 수정, 같은 구간 비교·게이트 재검증 | 변경 전후 같은 구간 플레이 20분 | 승인 가설 검증·유지/되돌림 기록, 진행 회귀 없음 |
| S4-05 | UI 정리 / Audio·VFX 최소 완성 | REVIEW | TODO | S3-G:DONE | 기존 HUD·메뉴 가독성, 명세 효과음·점멸·능력·승리 피드백, 출처 기록 | 무음·720p·1080p·패드 메뉴 15분 | 정보 누락·잘림 없음, 기존 규칙 변화 없음, 리소스 이용 조건 확인 |
| S4-06 | 버그 수정·회귀 | AUTO | TODO | S4-04:IMPLEMENTED, S4-05:IMPLEMENTED | Bugs의 출시 차단·주요 오류 수정, 재현별 재검증 | 주요 재현 건 검수 10분 | 진행 불가·크래시·저장 손실·명백한 런타임 오류 0, 잔여 경미 오류 명시 |
| S4-07 | 최종 Windows Build Test | REVIEW | TODO | S4-04:DONE, S4-05:DONE, S4-06:DONE | 릴리스 후보 빌드·버전 고정, 키보드/패드 각각 완주, 저장 오류·전환 검사, README 실행 안내 | 별도 실행 파일·저장 경로 검수 15분 | 양 장치 완주 증거·체크리스트, 배포 빌드에 디버그 우회 없음 |
| S4-08 | Portfolio Evidence | AUTO | TODO | S4-04:DONE, S4-07:DONE | 이동 1개·동선/난이도 1개 이상 사례, 전후 수치·영상 위치·측정 한계·역할 정리 | 설명이 실제 결정과 일치하는지 10분 | 의도→구현→문제→수정→결과가 근거로 연결, 가짜 개선 수치 없음 |
| S4-G | Week 4 Gate / 최종 완료 검수 | DECISION | TODO | S4-07:DONE, S4-08:DONE | PRD DoD 대조, 미충족·잔여 오류·최종 범위 제시 | 최종 수락 또는 미완 항목 판단 20분 | 모든 필수 DoD 충족·문서 일치; 미충족은 승인 없이 완료 처리 금지 |

## Gate 실패·버그 대응

Gate 실패는 해당 Gate를 BLOCKED로 두고 기존 관련 Task를 재개한다. 이미 승인된 동작의 명백한 버그는 즉시 고칠 수 있다. 작업 ID는 `BUG-001`부터 부여하고 아래 Bugs에 분류·Status·Dependency·Agent Work·Human Review·DoD를 포함해 작은 Task로 기록한다. 새 규칙이 필요한 수정은 DECISION으로 분리한다. 검증 후 Gate를 다시 검토한다.

Scope Cut은 장식/연출 밀도 감소 → 선택 방 제거(18→15) → Minimum 11방·적 2종 순으로 검토한다. 위험이 크면 중간 단계 없이 Minimum으로 전환할 수 있다. 매일 잔여 구현·검토 시간을 대조하고 늦어도 S2-G에서 전망을 판단한다. Gate 실패 또는 남은 일정 초과 전망이면 S-CUT을 제시한다. 승인 전에는 자동 삭제하지 않는다.

### S-CUT — Minimum Release 전환 결정 및 작업 재설정

- ID: S-CUT
- Task: PRD Minimum Release 전환 여부 결정 및 실행 명세 반영
- 분류: DECISION
- Status: TODO (위험 발생 시에만 제시; 현재 Target 유지)
- Dependency: 없음. Sprint Gate 완료를 기다리지 않고 일정 위험 발생 시 판단 가능.
- Agent Work: 남은 구현·통합·검토 비용과 11방/적 2종 대비안을 제시하고, 결정 후 아래 Task 매핑·의존성·DoD·적용 Scope·설계 문서를 갱신한다. 이 Task 자체는 게임 구현을 하지 않는다.
- Human Review: Target 유지 / 15방 축소 / Minimum 전환 결정, 제거 대상·잔여 위험 확인(15분).
- Definition of Done: 사용자 결정·일시·근거 기록, 영향 문서·Task 수정 완료. Minimum이면 GAME_DESIGN 8절의 대체 그래프가 활성 원본이고 S3-04/S3-08의 제외와 S3-05 의존성 수정이 일치함. 15방만 선택하면 S3-08을 DEFERRED로 두고 선택 분기 제거를 관련 배치 Task에 반영한다. 실제 Scene 수정은 아래 기존 Task를 재개하여 검증. Target 유지 결정 뒤 새 위험이 발생하면 새 판단 기록과 함께 이 Task를 재개한다. 이 Task는 미사용이어도 제품 완료를 막지 않음.

| 영향 Task | Minimum 적용 시 Agent Work / DoD 대체 | Dependency·상태 처리 |
| --- | --- | --- |
| S1-07, S1-09 | A01/A03/A05 중심으로 학습 통합, 이동 Slice 3방. A05 능력 실물은 S2-01에서 추가 | 전환이 Week 1 이전이면 4방 DoD를 3방으로 변경하고 PRD Week 1 Gate도 동시 갱신. 이미 통과한 Gate 증거는 보존 |
| S2-01, S2-02, S2-04, S2-07 | A03↔A05 직접 연결, E1 소개를 A05 획득 전 공간으로 이동, A02/A04 삭제 참조 정리 | 이미 구현한 경우 배치·연결 검증을 재개, REVIEW는 재수락 필요 |
| S3-02 | B01↔B02↔B04, B03 제거, B05와 G-J 유지 | S3-01:IMPLEMENTED 유지; 방·표식·귀환 검증 변경 |
| S3-03 | E2를 B02에 유지 | S3-02:IMPLEMENTED 유지 |
| S3-04 | E3 전용 기능·배치 제외 | DEFERRED. S3-05에서 이 의존성 제거; DEFERRED를 DONE으로 간주하지 않음 |
| S3-05 | C01↔C02↔C04 연결, C03 제거, 종합 이동을 C02에 통합, 이미 존재하는 E3 전용 배치·참조 정리 | 대체 Dependency: S3-01:DONE, S3-02:DONE, S3-03:DONE, S-CUT:DONE |
| S3-06, S3-07 | 1보스·2패턴·엔딩·저장 유지, 기존 연출 단순화 | 기술 의존성 유지, C04↔C06 재검증 |
| S3-08 | A06/B06/C05 선택 방 제외 | DEFERRED, S3-G에서 제외 승인 확인 |
| S3-09, S3-G | 11방·E1/E2·두 게이트·엔딩으로 완주 기준 변경 | 제거 방 참조 없음, 모든 양방향 연결·새 게임·Continue 재검증 |
| S4-02 | 개발자 1회+신규 외부 1명 1회, 실제 시간 기록 | 요구 인원 미확보는 BLOCKED, 표본 한계 기록 |
| S4-05 | 기존 최소 정보·예고·피격·획득·승리 표현 재사용 | 추가 장식 밀도 제외, 판독성 유지 |
| S4-07, S4-08, S4-G | 양 입력 완주·저장 안정성·개선 2사례 유지 | Minimum 완료 조건으로 대조; 테스트를 생략해 DONE 처리하지 않음 |

전환 이후 이미 완료한 구현 Task는 영향 부분만 재개한다. 재개한 선행 작업을 참조하는 후속 작업은 재검증이 끝나기 전 의존성이 충족되지 않은 것으로 처리한다. 기존 결과·기록은 보존한다. 늦은 전환 자체의 수정·회귀 비용도 예상 잔여량에 포함한다.

## 기록 양식

이 절은 실제 데이터가 생길 때 채운다. 아래 항목은 빈 양식이며 완료·테스트 증거가 아니다. 별도 문서 파일을 늘리지 않고 이 파일에서 관리한다. 구현 이후 영상·빌드·스크린샷은 실제 저장 위치를 링크한다.

### Playtest Records

`Test ID / 일시 / 빌드·수치 버전 / 참가자 코드·숙련도 / 입력 장치 / 전체 활동 시간 / A·B·C 시간 / 방별 사망·반복 사망 / 길 잃음 위치 / 능력 후 첫 행동 / 게이트 기억·복귀 / 보스 재시도 / 방향 오판 / 관찰 메모 / 증거 위치`

현재 기록: 없음(미실시).

### Design Decision / Before–After

`Decision ID / 관련 Task·방·시스템 / 기획 의도 / 초기 값·구현 / Test ID·문제 근거 / 수정 가설 / 승인자·일시 / 변경 값·파일 / 동일 구간 재검증 / 실제 결과·한계 / 유지·되돌림 / 반영 문서 / 증거 위치`

현재 기록: 없음. 이 문서의 v0.2 설계값은 검증 가설이며 플레이테스트를 통과했다는 의미가 아니다.

### Resource Register

`리소스 / 출처 URL 또는 직접 제작 / 라이선스·확인일 / 사용 위치 / 표기 의무 / 실제 표기 위치`

2026-09-17: Assets/Art/Block.png — 직접 생성한 2×2 흰색 기하 도형용 텍스처. 플레이어·검증 발판에 색을 입혀 재사용. 외부 아트·오디오 없음, 외부 리소스 표기 의무 없음.

### Latest Handoff

2026-09-22 Git 인계 준비: 사용자가 현재 상태의 Git 연결을 요청했다. 사용자 지정 원격 `origin`을 `https://github.com/okyunsu/game1.git`으로 연결했고 기존 원격 ref가 없음을 확인했다. 사용자가 지정한 작성자 정보를 저장소 로컬 설정에 적용하여 N1 스냅샷을 `main`의 초기 커밋으로 기록·push하는 작업을 진행한다. 소스·Unity 설정·문서·Validation을 보존하고 Library/Temp/Builds/Logs/UserSettings 제외를 확인했다. 초기 커밋 메시지: `chore: snapshot Sprint 1 N1 movement prototype`. 원격 반영 완료 여부는 push 결과와 로컬/원격 HEAD 일치로 확인한다. Unity 재실행은 하지 않았으며 아래 기존 검증 기록과 Sprint 상태를 유지한다.

2026-09-17 / N1 / Unity 6000.3.24f1 (4e7b9b5b6244), Input System 1.20.0. 설계 기준 v0.2 유지.

#### Completed

- S1-01 **DONE**: Hub 3.21.3·Editor 설치, 프로젝트·폴더·Boot/Menu 기초 구성, 빈 씬 Play 및 Windows x64 Mono 빌드 성공.
- S1-02 **DONE**: Gameplay/UI 분리, 키보드·패드 입력, Pause·장치 해제·재개 시 입력 초기화. Unity 가상 장치 기술 검증 16개 통과. 물리 패드는 미검수.
- S1-03 / S1-04 / S1-05 **REVIEW**: 각 단계 구현·Unity 검증을 마친 뒤 다음 IMPLEMENTED 의존 경로를 실행했다. 최종적으로 **Player Movement Review** 1묶음에 등록. 사용자 수락은 아직 없음.
- N1 실행 종료. S1-06/07·S1-08 이후·Sprint 2는 미착수. Prototype Value 변경·후보 채택 없음.

#### Changed Files

- `ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`, `Packages/packages-lock.json`: Editor·실제 설치 패키지 고정. Cinemachine 3.1.7은 문서상 호환 확인만 했으며 미설치.
- `Assets/Scripts/PlayerInputReader.cs`, `Assets/Input/PlayerControls.inputactions`: 입력·맵 전환·최소 Pause. Attack/Dash는 미구현이며 향후 이름만 README에 예약.
- `Assets/Scripts/PlayerMotor.cs`, `Assets/Scripts/PlayerTuning.cs`, `Assets/ScriptableObjects/PlayerTuning.asset`: Rigidbody2D 속도 제어·바닥 접촉 법선·시간 보정·상승당 1회 Jump Cut·하강 제한.
- `Assets/Prefabs/Player.prefab`, `Assets/Prefabs/PlayerFrictionless.physicsMaterial2D`, `Assets/Art/Block.png`: 0.7×1.6u 플레이어·무마찰 접촉·최소 도형 표시.
- `Assets/Scenes/MovementTest.unity`: 평지·발판·벽만 있는 기술 검증 공간, 고정 직교 카메라. 추적 카메라나 Room/Tilemap 시스템 구현 아님.
- `Assets/Scenes/Boot.unity`, `Assets/Scenes/Menu.unity`: S1-01 초기 구성용. 현재 검토는 MovementTest에서 시작한다.
- `Assets/Editor/ProjectSetup.cs`, `InputSetup.cs`, `MovementSetup.cs`, `N1Verification.cs`, `N1Build.cs`: 생성·실제 Unity Play Mode 검증·빌드 도구. Editor 폴더이므로 Windows 플레이어에 검증용 가상 장치·임시 지형이 포함되지 않는다.
- `.gitignore`, `README.md`, `TASKS.md`, `Validation/*.txt`: 생성 캐시 제외, 실행·검토 안내, 실제 결과. PRD/GAME_DESIGN/AGENTS 내용은 수정하지 않음.

#### Validation

검증 장비: 이 Windows 11 PC, Editor 6000.3.24f1, 고정 물리 간격 0.02초. 아래는 실제 실행 기록이며 사용자 감각 수락과 구분한다.

| 검증 | 기대 / 실제 결과 | 증거 |
| --- | --- | --- |
| 빈 씬 Play·Windows 기본 빌드 | Play 진입→2초 실행→Edit 복귀; 빌드 Succeeded, 오류/경고 0 | `Validation/S1-01-play.txt`, `S1-01-build.txt` |
| 입력 및 마지막 장치 표시 수정 회귀 | D/방향키·스틱/D-pad·Space/A, 홀드 중복 없음, Esc/Menu Pause, A/B/Enter 재개, UI 입력 유출 없음, 패드 해제 Pause·키보드 복구. 16개 통과 | `Validation/S1-02-results.txt` |
| S1-03 단계 검증 | 벽 접지·벽 점프 없음, 좌우 반전·공중 제어·착지·Inspector 적용, 입력 포함 27개 통과 | `Validation/S1-03-results.txt` |
| S1-04 단계 검증 | Coyote 경계·착지 Buffer·중복 소비 검사 통과 | `Validation/S1-04-results.txt` |
| 최종 이동 회귀 | 입력·기본 이동·보정·가변 점프·하강·Inspector 변경, 총 96개 assertion 통과 | `Validation/S1-05-results.txt` |
| 최종 Coyote | 실제 0.0800초 후 입력 허용, 0.1234초 후 입력 거부 | 위 최종 결과의 MEASURE |
| 최종 Buffer | 착지 점프 0.0999초 전 입력 허용; 약 0.147초 전 입력 만료; 홀드 입력 다음 착지 재사용 없음 | 위 최종 결과의 MEASURE |
| FPS 조건 | 평균 렌더 30.0 / 60.0 / 120.0fps 확인. 각 조건에서 이동·벽 접촉·중복 점프·높이 차이·낙하 검사 통과 | 위 최종 결과 |
| 점프 높이 | 긴 점프 모두 2.520u. 짧은 점프 30/60/120fps에서 각각 1.944/1.596/1.596u | 입력 예약은 프레임 단위여서 짧은 홀드의 실제 길이가 동일하지 않음. FPS 간 동일 입력 궤적 일치 증거로 해석하지 않는다 |
| 하강·설정 | 하강 가속도 45u/s², 최대 속도 18u/s. Inspector 상한 16으로 임시 변경 시 다음 물리 틱 적용 | 각 FPS 검사; 임시 변경은 복제한 설정에서 수행해 원본 초기값 보존 |
| Windows N1 빌드 | Succeeded, 오류/경고 0, Development Build=false | `Validation/N1-build.txt`, `Builds/N1/Afterglow.exe` |
| 빌드 실행 | Afterglow 창 생성, Player.log의 렌더러·입력 초기화 확인 | `%USERPROFILE%/AppData/LocalLow/Afterglow/Afterglow/Player.log`; 화면 직접 확인은 아래 미실시 참고 |

원시 로그는 `Logs/S1-01-final.log`, `Logs/S1-02-final.log`, `Logs/S1-03-verify.log`, `Logs/S1-04-final.log`, `Logs/S1-05-final.log`, `Logs/N1-build.log`. 최종 입력·이동·빌드 로그에서 C# 컴파일 오류·Exception 없음. 검사 중 Console Error/Exception을 수집해 실패 처리한다. 초기 실패 기록도 `Logs/*attempt.txt`에 보존했으며 최종 결과로 덮어 숨기지 않았다.

**미실시:** 물리 게임패드 실제 조작·탈착, 사용자의 이동감/재미 수락, 빌드 화면 직접 검수·수동 플레이, 해상도별 화면 검수. computer-use 화면 캡처 단계가 사용자의 물리 Escape 입력으로 중단되어 추가 화면 조작을 하지 않았다. 카메라·방 전환·사망/리스폰 실제 연동은 이번 범위 밖이며 검증하지 않았다. 입력/모터 초기화 훅은 제공하되 미구현 시스템 연동을 통과했다고 주장하지 않는다.

#### Issues

- ENV-001 (해결): Unity Editor/Hub 미설치 → 사용자 설치 승인 후 Hub 3.21.3, Editor 6000.3.24f1 설치. 별도로 존재하는 6000.6.0f1을 이 프로젝트에서 사용하지 않음.
- TEST-001 (해결): 첫 빈 프로젝트 자동화가 검색 인덱스 초기화 전에 Play 진입해 `UnityEditor.Search.SearchDatabase` 예외 발생. 시작 콜백을 기다린 뒤 재검증하여 미재현.
- TEST-002 (해결): 배치 모드 Game View 포커스로 가상 키 입력 미전달. 검증 중에만 입력 포커스 정책을 조정하고 종료 시 복원.
- TEST-003 (해결): 자동 입력 1프레임 지연·물리 틱 이전 판정·검증용 순간 위치 변경의 이전 접지 캐시 때문에 초기 경계/120fps 테스트 실패. 실제 입력 시각 기록, 물리 틱 대기, 공중 시험의 초기 상태 분리 후 재검증 통과. 이를 게임 규칙 변경으로 우회하지 않음.
- 환경 메시지: 배치 로그의 access-token 갱신 메시지 뒤 Unity Personal entitlement가 확인되고 빌드/Play 성공. 종료 시 Mono debugger listen/abort 메시지는 있으나 플레이 중 게임 C# 예외는 재현되지 않음.
- 검토 한계: 짧은 점프 입력은 FPS별 실제 홀드 길이가 같지 않다. 화면·실제 패드 검수와 감각 수락이 남아 있으며 게임 전체 무오류/완주를 주장하지 않는다.

#### Decisions Needed

Player Movement Review에서 S1-03/04/05 각각 수락 또는 수정 의견 필요. 기본 수치 채택 여부는 사용자 판단이며 현재는 v0.2 Prototype Value 그대로다. 새로운 설계 결정이나 범위 확대 요청 없음.

#### 사용자가 다음에 직접 확인할 항목

아래 Human Review Queue의 Scene·Inspector·테스트 순서를 사용한다. 먼저 Unity에서 MovementTest를 열어 키보드와 실제 패드로 조작하고, 3개 Task를 개별 수락/반려한다.

#### Recommended Next Task

우선순위 1: Player Movement Review(약 15~20분). 반려 항목이 생기면 관련 Task를 재개하고 영향 경로를 재검증한다. 수락 전 S1-06/07은 의존성 미충족이며 이번 실행은 종료한다. 모두 DONE 이후 다음 실행 범위를 확인하여 Camera/Room 작업을 진행할 수 있다. N2는 여전히 미승인 후보다.

## Current Work Queue

### Current Sprint

Sprint 1 — 착수일 2026-09-16. 2026-09-17 N1 기술 구현 종료, 사용자 이동감 검토 대기.

### Current Task

N1 구현 작업 없음. S1-01/02 DONE; S1-03/04/05 REVIEW. 다음 사용자 작업은 Player Movement Review.

### Agent Queue

비어 있음(0/5). 승인된 N1 기술 작업 완료. S1-06 이후 자동 시작 금지.

### Human Review Queue

**1/3 묶음 — Player Movement Review (S1-03/04/05, 각 REVIEW)**

- 열 Scene: `Assets/Scenes/MovementTest.unity` (메뉴 `Afterglow > Open Movement Test`도 가능).
- GameObject: `Player`, `Floor`, `Low Platform`, `Middle Platform`, `High Platform`, `Left Wall`, `Right Wall`.
- Prefab: `Assets/Prefabs/Player.prefab`. `PlayerMotor`의 Tuning/Ground Layers와 Rigidbody2D/BoxCollider2D 확인. 주요 수치 원본은 아래 Asset 1개.
- ScriptableObject: `Assets/ScriptableObjects/PlayerTuning.asset`.
- Inspector: Move Speed 6, Acceleration 80, Deceleration 100, Air Control 0.9, Jump Velocity 12, Gravity 30, Fall Gravity Multiplier 1.5, Max Fall Speed 18, Jump Cut Multiplier 0.5, Coyote Time 0.10, Jump Buffer 0.12. 단위·Tooltip·범위 제공.

| 순서 | 조작 / 확인 | 관련 Task |
| --- | --- | --- |
| 1 | Play 후 평지에서 A/D 또는 ←/→, 빠르게 반전·손 떼기. 패드 스틱/D-pad도 확인. 즉시 반응·제동·공중 방향 제어를 판단 | S1-03 |
| 2 | Space/A로 낮은 발판에 올라 끝에서 걸어 떨어진 직후 점프. 늦게 누르면 점프하지 않는지 비교. 벽에 붙은 공중 상태에서는 점프 금지 | S1-03/04 |
| 3 | 발판에서 내려오며 착지 직전에 Space/A. 착지 시 1회 점프, 계속 홀드해도 다음 착지에서 자동 반복하지 않아야 함. 충분히 일찍 누르면 만료 | S1-04 |
| 4 | 짧게 누르기/끝까지 홀드를 반복. 높이 차이·낙하 속도·착지 제어감을 판단. 최대 속도 수치 검증은 자동 결과 참고 | S1-05 |
| 5 | Esc/Menu Pause → Enter/A 또는 Esc/B 재개. 재개 버튼이 점프가 되지 않는지, 실제 패드 분리 시 Pause·키보드 복구 확인 | S1-02 회귀·물리 장치 미검수 보완 |
| 6 | 원본값을 먼저 적고 PlayerTuning의 Move Speed/Jump Cut 등 한 값씩 권장 범위 안에서 변경·비교. 각 Task 수락/반려와 원하는 값 기록 | S1-03/04/05 |

Play 중 Asset 변경은 남을 수 있으므로 이전 값을 기록한다. **Stop 후** Inspector에서 원하는 값/이전 값으로 명시적으로 반영하고 저장한다. Scene 배치의 Play Mode 변경은 되돌아가므로 Stop 후 확정 배치를 재입력한다. 사용자 수락 전 후보값을 새 기본값으로 확정하지 않는다. 현재 기본 수치는 모두 문서의 초기값이다.

### Decisions Needed

Player Movement Review의 S1-03/04/05 개별 수락·반려만 필요. S1-06 이후 승인 여부는 이번 N1 완료와 별개다.

### Bugs

현재 N1 자동 검증 범위에서 재현 중인 게임 런타임 오류는 없음. 미검수 범위는 Latest Handoff에 명시했다. 위 ENV/TEST 항목은 도구·환경·재현 절차 이슈이며 제품 기능 버그와 구분한다. 향후 실제 게임 오류는 `BUG-001`부터 기록한다.

등록 양식: `ID / Task / 분류 / Status / Dependency / 심각도 / 빌드 / 재현 단계 / 기대·실제 / Agent Work / Human Review / DoD / 재검증 결과`.

심각도: Blocker=크래시·진행 불가·저장 손실, Major=핵심 입력·전투·카메라·게이트 오류, Minor=진행 가능한 표시·연출 문제.

### Backlog

비어 있음. 위 계획 밖의 새 기능을 암묵적으로 승인하지 않는다. 관찰된 문제만 근거와 함께 등록한다.

### Deferred

현재 없음. 향후 승인된 Scope Cut 항목은 결정 ID·이유·영향을 기록한다. PRD의 Out of Scope는 향후 구현 예약 목록이 아니다.




