# 개발 작업과 기록

## 0. 지금

- 갱신: 2026-10-26, Codex (Batch21 진행)
- Git 기준: 시작 계약 커밋/Base `7d40918`. 현재 HEAD는 `git log -1`, origin/main은 로컬 참조이며 fetch하지 않음.
- push 대기: Base 시점9커밋(이전8+시작 계약1). 현재 수 확인: `git rev-list --count origin/main..HEAD`. push·squash 없음.
- 진행 중: 승인 Batch21. Preflight exit198, 독립 T3 검사기 보정 완료·종료 검사 진행.
- 사용자 차례: 기존 V4/V5 수락 대기 순서 유지, D-BODY 크기 비교, 로컬 커밋 push
- Claude 차례: B20 측정 근거로 draft4 제안(D-V5-DRAFT4), 사용자 채택은 별도
- Codex 차례: T3 로컬 커밋 후 Batch 종료 기록. Ready Queue 0건.
- 막힌 것: Unity 라이선스 exit198. T1/2/4/5/6/8 및 의존 T7/9·Backlog1~3 BLOCKED. 환경 변경 전 재실행하지 않음.
- 최신 게임 결과: [B20 요약](Validation/B20-summary.md). 기본 크기·규칙·Scene 유지, Sprint Gate 미실시.

## 1. 현재 상태 / Current Sprint

- Sprint3: V5 층 A 파일럿. B21 정적 잠금 계약 보정 완료, 실제 잠금·후보·새 측정은 Unity exit198로 차단. 크기와 draft4 결정 대기2건.
- B19 검사 판정 정리와 B20 결과는 [B19](Validation/B19-summary.md)·[B20](Validation/B20-summary.md) 참조.
- 운영 개선은 종료했다. 게임 [Batch21](Batches/BATCH-21.md)은 사용자 2026-10-26 실행 지시로 승인됐다.

## 1-0. Current Tasks (Batch21)

Base `7d40918`(Claude 설계 계약·지시문 시작 커밋). 이전8커밋 보존. [T3 검증](Validation/B21-room-rules.md).

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| B21-T1 | BLOCKED | needs-unity=yes: Preflight exit198. 런타임/생성기 수정 미착수 |
| B21-T2 | BLOCKED | T1·Unity 의존. Scene 재생성·실제 exe 5항목 미실시 |
| B21-T3 | DONE | 잠긴 우회 MEASURE, 무잠금/틀린 키/다른 벽 키/봉인 FAIL 검출. 전체 정적 FAIL0/WARN0·기존 fixture 유지 |
| B21-T4 | BLOCKED | needs-unity=yes. B21Arena·가로 간격 신규 실측 미실시 |
| B21-T5 | BLOCKED | needs-unity=yes. 위험 지형 복귀 후보·도랑 폭별 측정 미착수 |
| B21-T6 | BLOCKED | needs-unity=yes. 모서리 보정 후보 미착수·PlayerMotor 변경0 |
| B21-T7 | BLOCKED | needs-unity=no이나 T4 선행 의존. 36면 부족분 새 표 미작성 |
| B21-T8 | BLOCKED | T1~T6·Unity 의존. 새 빌드·8런처·회귀·Scene 검사 미실시 |
| B21-T9 | BLOCKED | T8 의존. 후보 전달용 결정/README/플레이 안내 미작성. 의무 종료 인계는 별도 수행 |
| B21-BL1 | BLOCKED | T7 의존. 1.3/1.1 머리 여유 표 미작성 |
| B21-BL2 | BLOCKED | T6·Unity 의존. 대시 모서리 보정 미착수 |
| B21-BL3 | BLOCKED | T5·Unity 의존. A04 후보 포고 시도 미실시 |

### 이전 OPS-20261025 결과

Base `bf9693a`(B20). 기존 미푸시 2커밋 보존, push·squash·게임 태그 변경 없음.

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| OPS-T1 | DONE | TASKS 원문 완전 보관·과거 미종결18행 유지·관련7문서 링크0불량·skill frontmatter/참조 확인. 기본 검사기 PyYAML 부재로 직접 구조 검사 대체 |
| OPS-T2 | DONE | 완료·재시도·시간/상한·보호·Queue·증거 규칙 통일, V5 대상/판정·템플릿 명시. 게임 변경0 |
| OPS-T3 | DONE | Preflight 구문오류0·모의8경로 PASS·실제 Unity 미실시·보호387 SHA변경0. 카드 범위 밖0·깨진 링크0·없는 런처0 |

## 1-1. 이전 Batch 미종결 추적

B20-T4 REVIEW: [크기 비교](Validation/B20-bodysize.md), D-BODY 사용자 수락 대기. 이전 B12~B19의 기록상 미종결 행은 아래에 유지한다. 후속 Batch에서 해소·대체된 역사적 상태도 있으므로 현재 버그/차단 수에 합산하지 않는다. 원문은 [B12~B19](Archive/TASKS-B12-B19.md), 전체는 [B20 종료 스냅샷](Archive/TASKS-B20.md). 이번 정리는 상태를 재판정하지 않는다.

| Task | 상태 | 당시 결과·남은 확인 |
| --- | --- | --- |
| B18-T3 | REVIEW | draft3 Scene6·작은 발판12·적15·구덩이6·요소 일치·문14/stub/일방/저장 PASS·Quiet13/13 |
| B18-T4 | PARTIAL | A02 두 원인 해소·적0/순찰끝0, 전체 후보76→69. 모두 측면 겹침, overhead-only0. 후보0 미충족·Quiet13/13 |
| B18-T5 | PARTIAL | 추가 규칙FAIL0/WARN0·위반JSON4개 검출. 기존 도달성20/단차2 FAIL22 보존, 전체FAIL0 미충족·Quiet13/13 |
| B18-T6 | REVIEW | Development 빌드·V5+NoMap6모드 각10초Error0. B18/B12 보존·V4 런처 변경0·Quiet13/13 |
| B17-T2 | REVIEW | draft2 6방·요소 일치·구덩이6·도랑 바닥 수정 반영·보호 SHA0변경, Quiet13/13 |
| B17-T5 | REVIEW | B17 Development·V5 3런처10초Error0·최신+직전2개, Quiet13/13 |
| B17-T7 | BLOCKED | 앞선 Quiet13/13·18런처Error0·카드0문제는 유효. 종료 기본 회귀는 Editor 라이선스 exit198로 실행 전 중단 |
| B16b-T2 | REVIEW | 수정본 Scene6개·요소 일치·A02조각2/A04·A06각1·Quiet13/13 |
| B16b-T5 | REVIEW | Development 전달·15런처+NoMap3 PASS·Release 제외 검증 |
| B16b-BL1 | REVIEW | 지도6방/12문·stub×4/화살표1·V4지도PASS·Quiet13/13 |
| B16-T1 | BLOCKED | 생성기 경로 문자열 컴파일 오류 1회 보정 후 재실패, 제품 코드 전체 원복. Quiet13/13 |
| B16-T4 | PARTIAL | 정적 방 규칙·가짜 문 안전/겹침/높이 위반 검출. 실제 gate·물리 도달성 미검증 |
| B16-T7 | PARTIAL | Quiet13/13·기본13/13, 기존12런처 Error0. 카드 범위밖/깨진링크/없는런처0. V5 전달 기준 미충족 |
| B15-T2 | REVIEW | 방문방·위치·M3상태, 실제 exe·시간·입력맵 유지, Quiet13/13 |
| B15-T3 | REVIEW | 문 약자·종류·미방문 ?, map-all/no-map/비V4 검사, Quiet13/13 |
| B15-T5 | REVIEW | 전달 Development·NoMap·3런처 10초 Error0, Quiet13/13 |
| B15-BL4 | REVIEW | 보유 능력 문 열림색·CP, 저장/Continue·no-map/비V4·Quiet13/13 |
| B12-T3 | PARTIAL | 추적 연타·정지 피격·P2 회복 X·Quiet13/13; 회피 지형 간섭, Pause/복귀/10회/fps 일부 미검증 |

## 2. 결정 대기

| ID | 질문·선택지 | AI 추천·이유 | 풀리는 항목 |
|---|---|---|---|
| D-BODY | [BodySizeTest](Launchers/current/Play-BodySize.cmd)와 V5에서 비교 후 높이1.6 유지 / 1.3 / 1.1 중 선택 | 1.3을 비교 우선 후보로 추천. 키·폭이 약19% 줄고 기본보다 공격 사거리가19% 줄어드는 절충. 채택은 직접 플레이 후 판단 | 기본 크기 반영 다음 배치·B20-T4 REVIEW. [측정](Validation/B20-bodysize.md) |
| D-V5-DRAFT4 | Claude가 B20 결과로 draft4 작성 → 다음 배치 Scene·필요 코드 반영 / 현재 draft3 유지·보류 | draft4 작성 추천. 실제 shortcut 우회와 도랑 무능력 통과 근거가 있고 문-벽 저장 키 연결 새 코드도 필요 | A05 지름길·도랑 설계 수정, 머리 여유 배치 검토. [T1](Validation/B20-shortcut.md)·[T2](Validation/B20-trench.md)·[T3](Validation/B20-headbump.md) |

결정 대기2건. 기본 크기와 draft3는 아직 바꾸지 않았다. D-V5-CLEARANCE와 WORLD의 A06 적 표기 정리는 B19에서 해소한 상태를 유지한다. [기존 근거 장부](Validation/B19-rules-ledger.md), [B20 추가](Validation/B20-rules-ledger-add.md).

## 2-1. 결정 기록

| ID | 결정 | 날짜 | 근거 |
|---|---|---|---|
| D-B21-EXEC | Batch21 실행 승인. 시작 3파일 커밋·draft3.1 계약 구현·기본 꺼짐 후보2종·T6 한정 PlayerMotor 예외. 기본 크기 채택은 제외 | 2026-10-26 | 사용자: “Batches/BATCH-21.md 읽고 진행해줘”. [승인 범위·사전 결정](Batches/BATCH-21.md). Claude 설계·제안을 사용자 감각 수락으로 바꾸지 않음 |
| D-OPS-20261025 | P1~P7·P9·P10을 검토 보완과 함께 적용. P8·게임 수정 제외 | 2026-10-25 | 사용자: “그렇게 해서 진행해줄래/”. 직전 검토의 적용 순서·보완을 승인. [범위](Batches/BATCH-OPS-20261025.md) |
| D-PLAY-20261022 | 점프 불쾌 원인 ① 캐릭터 크기 ③ 머리 충돌. draft4는 B20 측정 후 작성하며 기본 크기 채택은 별도 | 2026-10-22 | B20 사전 지시가 전달한 사용자 판단을 대리 기록. 채팅 전문·직접 발언 인용은 없음. [B20 목적·지시](Batches/BATCH-20.md), [플레이](Playtest/20261022-233114.csv) |
| D-V5-PILOT | 방 구조 V5 재설계의 첫 파일럿으로 층 A(A01~A06)만 새 방 문법(방 안 열린 두 갈래·겹침·보이지만 못 가는 곳, 방 크기 축소, 능력 없이 도는 고리 2개)으로 만든다. A06은 더블 점프 전용(A02 벽 점프 문과 B06 구멍 진입은 닫음). 플레이한 뒤 수정한다. 설계 문서와 방 데이터는 Claude 쪽에서 작성한다. | 2026-10-15 | 사용자 결정. V4는 선형이고 방이 크며 비어 있다고 판단함(부서지는 벽 21방 중 1곳, 허브·엔딩 방을 뺀 저밀도 방 존재). 21방 전체 제작 전에 방 문법을 6방으로 먼저 확인한다. 설계 데이터: `Design/v5/`. Batch 16(생성 BLOCKED) → Batch 16b에서 이어함. 사용자 수정본62cc8c1로 V5 Scene6개·전달 빌드를 생성했다([B16b](Validation/B16b-summary.md)). |
| D-B3 | B3(8패턴 + HP50% 2페이즈)를 기본 보스로 채택, 패턴 전부 켬. 기존4패턴 회복 0.8s, 새 패턴 회복 1.2/1.5s 유지. 예고 보임·회피 가능·2페이즈 확인, 사운드는 AV1로 당분간 충분. Boss-B3-rec10(회복1.0 비교) 취소. B2-fast는 [Boss-B2-fast](Launchers/current/Boss-B2-fast.cmd)로 계속 선택 | 2026-10-14 | 사용자 직접 플레이 후 결정. 진행 속도가 나쁘지 않았고, 마지막 보스라 난이도가 더 높아도 된다고 판단했다. (적용: Batch14 T1) |
| D-COMBAT/AV2 | AV2(전투 임팩트·사운드 강화 후보) DEFERRED | 2026-10-14 | D-B3 플레이에서 사운드는 AV1로 당분간 충분하다고 판단해 AV2를 보류. 10/12 D-COMBAT(임팩트·사운드 구현 후 재판단)과 이어진다. (사용자 결정) |
| D-BOSS-PICK | B2-fast 채택(10/13). 패턴이 보이고 회피·반격 가능, 신규 패턴은 B3 후보에서 추가 | 2026-10-13 | 패턴이 보이고 회피·반격이 가능했다. 패턴 수 부족은 B3 후보로 보완하기로 했다. (Batch12 사용자 결정) |
| D-MOVE | 수락. 현재 기본값 채택(점프 수치, 벽 점프 기본 프리셋1, B10 장치 기본값, V2 가이드 문구 현행). B06 벽 점프·가이드 REVIEW → DONE | 2026-10-12 | 사용자 직접 플레이에서 이동이 자연스러움 |
| D-LOOK | 방 흐름·방 전환 수락(그레이박스 단계). B05-V2 방 흐름 REVIEW → DONE. 아트 부분은 2-2 단계 보류 | 2026-10-12 | 그래픽이 없는 현 단계에서 방 흐름·전환이 자연스러웠다. 아트는 아트 단계로 보류. (사용자 결정) |
| D-COMBAT | 보류. 임팩트(히트 이펙트)·소리 구현 후 히트스톱·AV1·Q6·F2R 후보와 함께 다시 판단(2-2) | 2026-10-12 | 임팩트(히트 이펙트)와 사운드 구현이 우선이라 구현 후 다시 판단한다. (사용자 결정) |
| D-RULE-T5 | 막다른 방(A01, A05, A06, C06, S01 등 원문 기재)은 들어온 문 귀환·자연 획득·엔딩 후 재개를 탈출로 인정. BL4는 범위 제외(원문 없음) | 2026-10-12 | 사용자 결정; [T5](Validation/B10-T5-summary.md) |
| D-RULE-DIST | 낙하 도착 약2.2u 15곳과 Z01→S02 2u 예외 승인. B10 Claude 지형 수정 수락 | 2026-10-12 | 사용자 직접 플레이에서 방 전환이 자연스러움; [B09-T6](Validation/B09-T6-summary.md), [T12 보고서](Archive/validation-2026-10/B10-geometry-report.md) |
| D-NEXT | 사용자가 새 게임부터 엔딩까지 직접 플레이한 뒤 다음 배치 결정. 자동 경로 탐색 배치는 하지 않음. B07 A02·C03 후보는 이 플레이 결과까지 유지 | 2026-10-12 | 직접 플레이한 뒤 수정할 부분을 정하기 위해. (사용자 결정) |
| D-BOSS | 보류. 사용자 플레이에서 보스 이상이 보이면 다시 연다 | 2026-10-12 | [B10-T7](Validation/B10-T7-summary.md) 실제 보스→엔딩 자동 검사 2회 PASS |

## 2-2. 단계 보류

결정 대기 상한(8개)에 세지 않는다.

| 항목 | 내용 | 다시 열 조건 | 근거 링크 |
|---|---|---|---|
| D-COMBAT | 전투 손맛: B01 히트스톱 0.05s, B04-AV1 사운드·형태·명도, Q6 A/B/C 후보, F2R 표시 | 임팩트(히트 이펙트)·사운드 구현 후 | [B01 결과](Validation/B02-C-summary.md), [B04](Validation/B04-final-summary.md), [Q6](Validation/Q6-feedback-candidates.txt), [F2R](Validation/F2R-summary.txt) |
| D-LOOK 아트 | CC0 픽셀 스타일 유지/교체, V2 기본 교체 시점 | 아트 단계 시작 시 | [LEVEL_V2](LEVEL_V2.md) |

## 3. 직접 플레이 확인

- [Play-V4-Mechanics](Launchers/current/Play-V4-Mechanics.cmd): 6장치·E4·Slam 손맛, 10분.
- [Play-V4](Launchers/current/Play-V4.cmd): A01부터 V4 경로·되돌아가기, 25~30분(자동 자연 진행은 C02까지).
- [Play-V4-AfterDash](Launchers/current/Play-V4-AfterDash.cmd): 이어하기→CP-A03 대시 복원, 5분.
- [Play-V4-AfterWall](Launchers/current/Play-V4-AfterWall.cmd): 이어하기→CP-B01 대시+벽 점프 복원, 5분.
- [Play-V4-BeforeBoss](Launchers/current/Play-V4-BeforeBoss.cmd): 개발 fixture CP-C04, 능력3개, 3분. [Play-V4-AfterEnding](Launchers/current/Play-V4-AfterEnding.cmd): 개발 fixture A01, 엔딩 후 능력/Slam 복원, 3분. 자연 저장 검증과 구분한다.

- [BodySizeTest](Launchers/current/Play-BodySize.cmd), `Assets/Scenes/Test/BodySizeTest.unity`: 기존 대기 목록 다음에 F6으로1.6/1.3/1.1, 같은 막대·계단·포고·E1을 비교, 약5분.
- [Play-V5 기본1.6](Launchers/current/Play-V5.cmd) → [1.3](Launchers/current/Play-V5-Body13.cmd) → [1.1](Launchers/current/Play-V5-Body11.cmd): 각 A01 시작, 화면 점유·머리 걸림·공격 사거리 비교 후 D-BODY 답변, 약5분. 기본 채택은 별도 배치.

조정 값 위치와 Play 종료 후 반영은 [LEVEL_V4](LEVEL_V4.md). 기본 보스(B3, D-B3 10/14)는 [기본 BossTuning](Assets/ScriptableObjects/BossTuning.asset)에서 조정하고(Boss-B3 런처는 [후보 Asset](Assets/BossCandidates/B3/BossTuning.asset)을 쓴다) Play 종료 후 원하는 값을 명시 저장·재빌드한다.

## 4. 열린 버그

| ID | 증상 | 근거 |
|---|---|---|
| B07-V2-BOSS-REPLAY / B09-REGRESSION | 새 빌드 V2 고정 입력이 W22 보스에서 실제 사망(tick285, 진입편차.082u), 이전 B06b 빌드는 PASS. 원인 미확정 | [B07-T25](Validation/B07-T25-summary.md), [B09-T11](Validation/B09-T11-summary.md) |
| B07-A02-TRAVERSE | A02 필수 계단·재방문 굴뚝: 자동 입력 미완주, 사람 확인 필요 | [B07-R-A02](Validation/B07-R-A02-summary.md) |
| B07-C03-OPTIONAL | C03 첫 선택 포고 도전: 자동 입력 미완주, 사람 확인 필요 | [B07-R-C03](Validation/B07-R-C03-summary.md) |
| V5-A05-SHORTCUT | A03에서 A05로 x3.5에 도착해 온전한 벽 x1..2 안쪽으로 우회. 벽도 Side.Any로 생성됨 | [사용자 CSV 9~10행](Playtest/20261022-233114.csv), [B20 T1](Validation/B20-shortcut.md) |
| V5-A05-TRENCH | 대시 없이 일반 점프로 도랑 통과(피해1). 걷기만은 이번 측정에서 실패·사망. A04/A05의 도랑 입구 복귀 동작 없음 | [사용자 CSV](Playtest/20261022-233114.csv), [B20 T2](Validation/B20-trench.md) |
| V5-HEADBUMP | 사용자 점프 머리 걸림 피드백. h1.6 머리 충돌 후보36면·격리 실측33/36 접촉, 모서리 수평 보정 없음. 불일치3과 초기 문 전환2도 보존 | [플레이 CSV](Playtest/20261022-233114.csv), [B20 T3](Validation/B20-headbump.md), [전체 실측](Validation/B20-headbump-all.md) |

B16-GENERATOR·B12-CHASE-VERIFY·B18-CLEARANCE·B18-STATIC의 해소 상태는 유지한다. 이전 실패 기록은 기존 Validation에 보존했다. 기존 제품 버그3건에 B20의 V5 관찰3건을 추가해 열린 버그6건, 결정 대기2건이다. 정적 후보와 실제 진행 불가 판정은 구분한다.

Experimental-F1: 닫힘. F1 구성(히트스톱·표시)이 B01 F1-R(히트스톱, F2R 30/60/120fps 검사)과 F2R(표시 REVIEW)로 재구현됨. [B01 인계](Archive/B01-latest-handoff-20261007.md), [F2R](Validation/F2R-summary.txt).

## 5. 보관된 항목 색인

상세는 [원문](Archive/TASKS-before-2026-10-12.md)과 [이전 이력](Archive/TASKS_ARCHIVE.md).

| ID | 한 줄 요약 | 링크 |
|---|---|---|
| B10 종료 노트 | 70경로 성공 입력2회, 재생 안정성 PASS와 구분, r54~r58 결과 제외 | [원문](Archive/TASKS-before-2026-10-12.md) |
| B10-RESUME | Claude 수정 적용, 10방 이전 PASS 무효·T2-resume만 근거 | [원문](Archive/TASKS-before-2026-10-12.md) |
| B10-FAST-CLOCK | 무제한 렌더링 r19·r17 이전 T5 결과는 판정 제외 | [원문](Archive/TASKS-before-2026-10-12.md) |
| B09-INPUT-COVERAGE | 전 연결31 미통과·70경로41 미완주(당시), 추가 합격 주장 없음 | [B09-T8](Validation/B09-T8-summary.md) |
| B04-R11 | R11 시작편차0.110u 입력 소진, 제품 회귀 미확정 | [B06b-T1](Validation/B06b-T1-summary.md) |
| B03-P03 | P03 goal pogos=1 간헐 FAIL/Error0, 해결 주장 없음 | [B03-P03](Validation/B03-P03-summary.md) |
| B03-BOSS-VALIDATION | 최종 도약 회피 전체fps·동시HP0 미검증, 감각 수락 대기 | [B04-C](Validation/B04-C-summary.md) |
| N9-ROUTE | R05 사망으로 이전 완주 판정 철회, 추가 탐색 BLOCKED | [N9 재확인](Validation/N9-route-recheck.txt) |
| D0 | 기준 상태 T2/T4/S2 실패 재현, 회귀 판정 제외 | [D0](Validation/D0-summary.txt) |
| F2-T2 | F2 넉백 잠금 검사 재실패로 전체 복원 | [T2 재실패](Validation/Verification-night-T2-F2-rerun.txt) |
| ROOM-001 | 배치 모드 Tile 저장 0셀, 대안 B 블록 지형으로 우회, OPEN | [H5](Validation/ROOM-001-h5.txt) |
| RESET-001 | Sliced+autoTiling 콜라이더 불일치, Simple 방식 사용 | [원문](Archive/TASKS-before-2026-10-12.md) |
| N5 / UI-CAP-001 | 검증 도구 한계(T-A/D/E/F/G), 캡처 모듈 참조 불가 | [N5](Validation/N5-final-handoff.md) |
| S3T01-POGO | P03 고정 입력 반복5회 중 4PASS/1FAIL, REVIEW | [pogo5](Validation/B02-pogo5-summary.md) |
| S3T01-LEVEL | A-1 지정 입력 실패로 제외, 재적용은 별도 판단 | [A-1](Validation/S3T01-A-1-shelf-a3.txt) |
| Q-night4 | Q1 저장 수정 완료(S2-04 REVIEW), Q2~Q5 미완료, Q7 촬영 실패 | [원문](Archive/TASKS-before-2026-10-12.md) |
| LOG-001 | 사망 원인 정확 기록 미구현, 관찰 이벤트 허용 여부 판단 필요 | [LOG](Validation/LOG-playtest.txt) |
| CAM-002 / K2 | DEFERRED, 아트 적용 후 상대 거리로 재확인 | [원문](Archive/TASKS-before-2026-10-12.md) |
| Experimental-F1 | 닫힘(4번 근거) | [B01 인계](Archive/B01-latest-handoff-20261007.md) |
| Approved Backlog | Batch09 세 항목 처리 완료, 이전 승인 Backlog 소진 | [원문](Archive/TASKS-before-2026-10-12.md) |


| B12~B19 Task 표 | 과거 상태 보존, 미종결은 1-1에서 추적 | [이력](Archive/TASKS-B12-B19.md) |
| B10 결과·이전 후보 | 현재 계획과 분리해 보관 | [이력](Archive/TASKS-B10-operations-20261025.md) |
| B20 전체 상태·인계 | 운영 정리 전 원문과 Task 결과 | [스냅샷](Archive/TASKS-B20.md) |

## 8. Ready Queue

사용자가 승인했고 사용자 플레이 결과에 의존하지 않는 Task만 둔다. Batch Task·Backlog 종료 후 남은 시간과 Task 상한 안에서 위부터 진행한다. 각 행의 허용 범위는 실행 Batch 종료 검사의 허용 목록에 합친다. 비어 있으면 종료하며 임의로 일감을 추가하지 않는다.

| ID | 내용 | 합격 기준 | 허용 범위 | needs-unity | 선행 | 승인일·출처 |
| --- | --- | --- | --- | --- | --- | --- |

현재 0건. 다음 Batch 후보는 승인 Queue가 아니다.

## 9. Latest Handoff — OPS-20261025

### 결정 질문지

기존 D-BODY(1.6 유지/1.3/1.1)·D-V5-DRAFT4(draft4 제안 작성/현행 보류)는 2절 그대로다. 추천은 1.3 우선 비교, B20 측정 근거의 draft4 제안 작성. 이번 운영 개선은 두 결정을 대신하지 않는다.

### 직접 플레이 확인

기존 3절 V4 수락 순서를 유지한다. 이어 B17/B18 미수락 항목: A02 블록·가장자리 점프·선반 적, A05 낙하/카메라, A01~A04 작은 발판12개, A03/A04 등불, DJ 런처의 A06 포고·조각·일방 문을 약10~15분 확인한다. 기존 [B18 인계](Validation/B18-summary.md)의 수락 대기는 그대로다.

1. 새 [BodySizeTest](Launchers/current/Play-BodySize.cmd), `Assets/Scenes/Test/BodySizeTest.unity`: F6으로1.6→1.3→1.1, 막대6개·계단5개·등불 포고·E1, 약5분.
2. [V5 기본1.6](Launchers/current/Play-V5.cmd) → [1.3](Launchers/current/Play-V5-Body13.cmd) → [1.1](Launchers/current/Play-V5-Body11.cmd): A01의 화면 점유·점프·사거리, 약5분. 실제 방에서도 F6 전환 가능(비교 런처에서만).

배치는 Scene이 원본, 반복 오브젝트는 Prefab이다. 비교 높이는 Dev 컴포넌트의 후보3개만 사용하고, 공격 크기는 런타임 전용 CombatTuning 복사본에서 같은 비율로 축소한다. 기본 PlayerTuning·Prefab은 그대로다. Play 변경은 종료 시 되돌아가므로 유지할 배치·값은 기록 후 Edit 모드에서 명시 저장한다. 기본 크기 적용은 D-BODY 후 별도 배치다.

### Task별 결과

세 Task DONE, 게임 변경·이전 수락 상태 변경 없음. [전체 검증](Validation/OPS-20261025-summary.md). 기본 검사기 PyYAML 부재는 단순 frontmatter/참조 직접 검사로 대체했다.

| Task | 상태 | 커밋 | 주요 파일·실제 검증·남은 한계 |
| --- | --- | --- | --- |
| OPS-T1 | DONE | `a0ee60a` | TASKS·Archive·CLAUDE·인계 skill. 과거 원문 완전 보관, 미종결18행 추적, 7문서 링크0. 실제 앱 skill 선택은 미실시 |
| OPS-T2 | DONE | `db7c23e` | AGENTS·Batch 템플릿·보호 목록. 완료/중단/Queue/시간·V5 명령 정리. Queue0, P8 제외 |
| OPS-T3 | DONE | `0d5db36` | Tools/Preflight.ps1. 구문오류0·모의8경로 PASS·보호387 SHA변경0·카드 범위/링크/런처0. 실제 Unity·게임 회귀 미실시 |

기존 B20 미푸시2커밋을 보존해 squash·게임 태그 변경·push를 하지 않았다. 최종 문서 확인은 종료 기록 커밋에 포함한다. 카드 상태 합계는 1-1의 역사적 미종결 행을 포함하므로 이번 Task 상태와 구분한다.

### 다음 Batch 후보

Claude draft4 제안과 사용자 판단 후 A05 문-벽 저장 키·도랑/머리 여유 반영을 별도 승인한다. D-BODY 선택 후 기본 크기 반영도 별도 승인이다. 기존 D-NEXT 전체 플레이·B07 미완주 확인은 유지한다. 층 B/C 확대·Sprint Gate는 승인되지 않았다. P8 문서 정리는 이번 범위 밖이다.

## 10. Playtest·Before-After

| 날짜 | 방·대상 | 사용자 피드백 / 변경 전→후 | 근거 |
| --- | --- | --- | --- |
| 2026-10-22 | A03→A05·A05 도랑·점프 | 지름길 우회·무능력 도랑 통과, 점프 불쾌 원인 ①크기 ③머리 충돌. B20은 측정·비교 도구 추가까지; 기본 설계 수정·개선 수락은 아직 없음 | [사용자 로그](Playtest/20261022-233114.csv), [전달 지시](Batches/BATCH-20.md), [측정 결과](Validation/B20-summary.md) |
