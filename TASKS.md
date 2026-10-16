# 개발 작업과 기록

원문 전체: [TASKS-before-2026-10-12](Archive/TASKS-before-2026-10-12.md). 이 파일은 원문을 옮겨 요약했으며 새 판단을 추가하지 않았다.

## 1. 현재 상태

- **Sprint3 / Batch16b: V5 층 A 파일럿 REVIEW — 플레이 가능**. 사용자 수정본 `62cc8c1`을 별도 저장한 뒤 Scene6개를 생성했다. 문14방향·순찰13개·조각4개·저장 복원과 런처15개+V5 NoMap3를 실제 exe에서 확인했다. [결과](Validation/B16b-summary.md). 직접 플레이·방 문법 수락 대기, Sprint Gate 미실시.
- Batch16의 T1 BLOCKED·의존 Task/Backlog SKIPPED를 Batch16b에서 이어받았다. [당시 결과](Validation/B16-summary.md)는 보존한다.
- Sprint3 / Batch15 종료(14 Task: DONE10 / REVIEW4, V5 방 데이터·Scene 제작 제외). **사용자 전체 플레이 대기**: 새 게임부터 엔딩까지 직접 플레이한 뒤 다음 배치를 정한다(D-NEXT). Sprint Gate 미실시.
- Batch15: 시작 HEAD `299a4f4`, 최종 인계 태그 `B15`, push 안 함. 모든 Task Quiet13/13·종료 기본13/13, 최종12런처+NoMap·지도4모드 Error0. [B15 결과](Validation/B15-summary.md).
- Batch14 시작: 2de8d26(B13 A5). 커밋을 하나로 합쳐 태그 B14. push 안 함. [B14 결과](Validation/B14-summary.md): 기본 보스 B3(D-B3), 검증 코드 Release 제외, 회귀 탐지 범위 측정(못 잡는 PlayerTuning 항목 16개).
- 회귀: Batch14 Task별 Quiet13/13, 최종 기본13/13. Batch12 결과는 아래 1-1.

## 1-0a. Current Tasks (Batch16b)

| Task | 상태 | 현재 결과 |
|---|---|---|
| B16b-T1a | DONE | 기존 CLI stub 예외 기록·보호 SHA0변경·Quiet13/13 |
| B16b-T1b | DONE | stub/일방 문·수정별 컴파일0·V4 재생성 SHA동일·Quiet13/13 |
| B16b-T1c | DONE | 기존 Prefab 변경0·순찰13개/끝점26·V4 SHA동일·Quiet13/13 |
| B16b-T2 | REVIEW | 수정본 Scene6개·요소 일치·A02조각2/A04·A06각1·Quiet13/13 |
| B16b-T3 | DONE | 실제exe14문·도착·stub4·DJ·저장복원·Error0·Quiet13/13 |
| B16b-T4 | DONE | 포고/모든도착/위아래아이템 검사·자체검사PASS·데이터FAIL10/WARN4 기록 |
| B16b-T5 | REVIEW | Development 전달·15런처+NoMap3 PASS·Release 제외 검증 |
| B16b-T6 | DONE | 상태·실행법·인계 갱신·링크0불량·Quiet13/13 |
| B16b-T7 | DONE | Quiet13/13·기본13/13·카드 범위/링크/없는런처0 |
| B16b-BL1 | REVIEW | 지도6방/12문·stub×4/화살표1·V4지도PASS·Quiet13/13 |
| B16b-BL2 | DONE | 자연A01→A04·A04구간120초BLOCKED·독립A05대시1.50초·Error0·Quiet13/13 |
| B16b-BL3 | DONE | 면적8260/4548·적7/13·장치15/22·문22/20·합계일치·Quiet13/13 |

## 1-0. Current Tasks (Batch16)

| Task | 상태 | 결과 |
|---|---|---|
| B16-T1 | BLOCKED | 생성기 경로 문자열 컴파일 오류 1회 보정 후 재실패, 제품 코드 전체 원복. Quiet13/13 |
| B16-T2/T3/T5 | SKIPPED | T1 의존. V5 Scene·문 exe 검사·전달 빌드·런처 미생성 |
| B16-T4 | PARTIAL | 정적 방 규칙·가짜 문 안전/겹침/높이 위반 검출. 실제 gate·물리 도달성 미검증 |
| B16-T6 | DONE | 실제 실패 상태·데이터 불일치 인계, V5 플레이 링크 생성 안 함 |
| B16-T7 | PARTIAL | Quiet13/13·기본13/13, 기존12런처 Error0. 카드 범위밖/깨진링크/없는런처0. V5 전달 기준 미충족 |
| B16-BL1/BL2 | SKIPPED | V5 지도/Scene/exe 의존. 실제 경로 측정 없음 |
| B16-BL3 | DONE | V4/V5 6방 비교: 면적8260/4848, 적7/12, 문표식22/20, 데이터·행 합계 일치 |

## 1-1. Current Tasks (Batch15)

| Task | 상태 | 결과 |
|---|---|---|
| B15-T1 | DONE | 지도 TextAsset 21방·32문, 좌표·재생성·v6 검증, Quiet13/13 |
| B15-T2 | REVIEW | 방문방·위치·M3상태, 실제 exe·시간·입력맵 유지, Quiet13/13 |
| B15-T3 | REVIEW | 문 약자·종류·미방문 ?, map-all/no-map/비V4 검사, Quiet13/13 |
| B15-T4 | DONE | 실제 exe 10초·M3회 Error0, 촬영 KNOWN-LIMIT, Quiet13/13 |
| B15-T5 | REVIEW | 전달 Development·NoMap·3런처 10초 Error0, Quiet13/13 |
| B15-T6 | DONE | 결정 기록·지도 사용법·인계 갱신, Quiet13/13 |
| B15-T7 | DONE | 최종12런처+NoMap·지도4모드Error0, Release 표시/검증6타입0, Quiet13/13·기본13/13 |
| B15-BL1 | DONE | 생성 인자화·방 목록 TextAsset화, 21씬 SHA동일·임시 요소수 동일, V4 저장/Continue·Quiet13/13 |
| B15-BL2 | DONE | V4 방·문·체류·지도 사용 로그, 기존9열 불변·실제 입력3방2문·Quiet13/13 |
| B15-BL3 | DONE | 방 기준선21방·37756u²·판정 없음·원본 불변·Quiet13/13 |
| B15-BL4 | REVIEW | 보유 능력 문 열림색·CP, 저장/Continue·no-map/비V4·Quiet13/13 |
| B15-BL5a | DONE | 추적 회피60/30/120 피해0·반격·Pause·사망복귀·10회Error0·Quiet13/13 |
| B15-BL5b | DONE | 2페이즈30/120 전환1·추적4·도약3·회복측정·Error0·Quiet13/13 |
| B15-BL5c | DONE | 8패턴60fps3바퀴·바퀴별 탄/낙하물/파동0·Error0·Quiet13/13 |

## 1-2. Current Tasks (Batch12)

| Task | 상태 | 결과 |
|---|---|---|
| B12-T1 | DONE | B2-fast 기본 채택; 필드 일치·인자 없는 런처2개·Quiet13/13 |
| B12-T2 | DONE | 분리 생성·저장·재로드·빌드 성공; 새 필드 유지·assert0·Quiet13/13 |
| B12-T3 | PARTIAL | 추적 연타·정지 피격·P2 회복 X·Quiet13/13; 회피 지형 간섭, Pause/복귀/10회/fps 일부 미검증 |
| B12-T4 | DONE | 연속 도약 60fps 6항목·30/120 회피0·Quiet13/13 |
| B12-T5 | DONE | 백스텝 탄·높은 탄 정지0·회복 반격·Pause·복귀·반복·벽 소멸·전 fps 회피0·Quiet13/13 |
| B12-T6 | DONE | 낙하물·안전 지대 전 fps 피해0·Pause·복귀·10회·Quiet13/13 |
| B12-T7 | DONE | 전환1회·전환 중 피해·4연타·3도약·회복.967s·사망 1페이즈 초기화·Quiet13/13 |
| B12-T8 | DONE | 실제 런처·활성 이름·8패턴 순환·꺼진 패턴·Error0·Quiet13/13 |
| B12-T9 | DONE | 13절 추가만·기존 문장 유지·링크0·Quiet13/13 |
| B12-T10 | DONE | 실제 P2 런처 HP14·첫 X HP13/2페이즈1회·최종 Quiet13/13 |

## 2. 결정 대기

| ID | 질문·선택지 | AI 추천·이유 | 풀리는 항목 |
|---|---|---|---|
| (없음) | | | |

## 2-1. 결정 기록

| ID | 결정 | 날짜 | 근거 |
|---|---|---|---|
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

조정 값 위치와 Play 종료 후 반영은 [LEVEL_V4](LEVEL_V4.md). 기본 보스(B3, D-B3 10/14)는 [기본 BossTuning](Assets/ScriptableObjects/BossTuning.asset)에서 조정하고(Boss-B3 런처는 [후보 Asset](Assets/BossCandidates/B3/BossTuning.asset)을 쓴다) Play 종료 후 원하는 값을 명시 저장·재빌드한다.

## 4. 열린 버그

| ID | 증상 | 근거 |
|---|---|---|
| B07-V2-BOSS-REPLAY / B09-REGRESSION | 새 빌드 V2 고정 입력이 W22 보스에서 실제 사망(tick285, 진입편차.082u), 이전 B06b 빌드는 PASS. 원인 미확정 | [B07-T25](Validation/B07-T25-summary.md), [B09-T11](Validation/B09-T11-summary.md) |
| B07-A02-TRAVERSE | A02 필수 계단·재방문 굴뚝: 자동 입력 미완주, 사람 확인 필요 | [B07-R-A02](Validation/B07-R-A02-summary.md) |
| B07-C03-OPTIONAL | C03 첫 선택 포고 도전: 자동 입력 미완주, 사람 확인 필요 | [B07-R-C03](Validation/B07-R-C03-summary.md) |

B16-GENERATOR는 B16b에서 해결(컴파일0·수정본6방 생성·실제exe PASS). B12-CHASE-VERIFY는 B15-BL5a에서 해결. 이전 실패 원문은 각 Validation에 보존한다. 열린 버그3건, 결정 대기0건. V5 정적 후보는 [검사 결과](Validation/B16b-room-rules-v5-A.md)에 기록했으며 실제 플레이 불가로 확정하지 않았다.

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

## 6. 최근 배치 결과 (B10)

| Task | 상태 | 현재 결과 |
|---|---|---|
| B10-T1 | DONE | 장치9개·기존 경로10개 저장 입력/전체 경로2회 PASS |
| B10-T2 | DONE | 재개 설계 70/70 저장 전체 입력2회 PASS; A02/wall·C02/low·Z01/up_A05 해소. r54~r58 이전 Scene 결과 제외; r59/r61/r63 현재 Scene·설계 hash 확인 |
| B10-T3 | DONE | covered floor trigger inset 수정 47b20ac, Claude 설계 수정 662cc0c / V4 재생성 24ceeda, 21방 검사 PASS |
| B10-T4 | PARTIAL | 실제 전체 왕복 저장2회 PASS34건, 나머지 탐색/판정 미완료 |
| B10-T5 | PARTIAL | 최소 능력 다른 출구 저장2회 PASS36입구, 고립0 미증명 |
| B10-T6 | PARTIAL | 새 게임·대시·벽 점프·B05 자연 등반→C02; S02 미완주 |
| B10-T7 | DONE | 보스전→Slam→천장→S01 저장 입력2회 연속 PASS |
| B10-T8 | DONE | 동일 HEAD 과거 빌드 고정 입력2회 PASS; 빌드 구성으로 범위 축소, 원인 커밋 미확정/수정0 |
| B10-T9 | DONE | 작성된 시작→끝 16구간, 전체 저장 입력2회 PASS; 장치 작동은 T1 별도 |
| B10-T10 | PARTIAL | 70경로30fps43PASS/27FAIL,120fps59PASS/11FAIL; 자연 미실시 |
| B10-T11 | DONE | 27 실제 입력 조합 unexpected=0, Error0 |
| B10-T12 | PARTIAL | Claude 보고서·실제 Scene4방 촬영; 남은 분기 물리 원인 미확정 |
| B10-T13 | PARTIAL | 기본13+RouteAll+Pogo PASS, V2 W22사망; 전달1개·4기동PASS, 자연 저장2개 미확보 |

| 항목 | 상태 | 결과 |
|---|---|---|
| BL1 | DONE | V4 회귀 등록1회63/69 PASS,6 FAIL |
| BL2 | DONE | 장치19경로 기본17/후보1 11/후보2 12 PASS, 기본값 유지 |
| BL3 | PARTIAL | C01까지 완료 구간 시간, 전체 미완주 |
| BL4 | BLOCKED | 참조된 B09 Backlog4~9가 BATCH-09에 없음(1~3만 있음), 합격 기준 결정 필요 |

Task별 요약: [T1](Validation/B10-T1-summary.md) · [T2](Validation/B10-T2-summary.md) · [T3](Validation/B10-T3-summary.md) · [T4](Validation/B10-T4-summary.md) · [T5](Validation/B10-T5-summary.md) · [T6](Validation/B10-T6-summary.md) · [T7](Validation/B10-T7-summary.md) · [T8](Validation/B10-T8-summary.md) · [T9](Validation/B10-T9-summary.md) · [T10](Validation/B10-T10-summary.md) · [T11](Validation/B10-T11-summary.md) · [T12](Archive/validation-2026-10/B10-geometry-report.md) · [T13](Validation/B10-T13-summary.md) · [BL1](Validation/B10-BL1-summary.md) · [BL2](Validation/B10-BL2-summary.md) · [BL3](Validation/B10-BL3-summary.md)

## 7. 다음 배치 후보

- D-B3 결정(10/14)으로 기본 보스는 B3. 추적 회피·2페이즈 fps·3바퀴 보강 검증은 B15-BL5a/b/c에서 완료했다. 다음 콘텐츠 후보는 D-V5-PILOT 층 A 설계·데이터 수신 후 파일럿이다.
- 기존 D-NEXT 전체 자연 플레이 결과와 B07 A02/C03·V2 W22 검토는 유지한다. 자동 경로 탐색이나 Sprint Gate 통과는 이번 배치에 포함하지 않았다.

## 8. Approved Backlog

이전 승인 Backlog 소진. Batch11 BL1 완료. 추가 작업 없음.

## 9. Latest Handoff — Batch16b

### 직접 플레이 확인

기존 3절 및 [Batch15 인계](Validation/B15-summary.md)의 확인 순서를 먼저 유지한다. V5는 다음 순서로 약20분 확인한다.

- [Play-V5](Launchers/current/Play-V5.cmd): A01 등불 포고 높은 선반·천장 빛·금 간 바닥 → A02 아래 통로·중간 선반·감실 → A03 두 갈래 올라가기·대시 문 → A04 위 통로·도랑 → A05 대시 획득·벽 부수기·지름길.
- [Play-V5-AfterDash](Launchers/current/Play-V5-AfterDash.cmd): A03에서 대시 문 표식(stub은 계속 닫힘) 확인. [Play-V5-DJ](Launchers/current/Play-V5-DJ.cmd): A01 천장 DJ 입장 → A06 조각·A02로 나가는 일방 문. 세 런처에 `-NoMap`을 붙일 수 있다.

플레이 뒤 질문은 [WORLD_V5 7절](Design/v5/WORLD_V5.md#7-사용자-확인결정-플레이-뒤). 감각·방 문법 수락 전이므로 REVIEW로 둔다. Scene 배치 원본은 `Assets/Scenes/V5/`; 순찰 끝점은 각 적 하위 Transform이며 기존 PlayerTuning·CombatTuning은 변경0. Play 중 조정은 종료 시 되돌아가므로 유지할 값은 Edit 모드 Scene에 명시 저장하고 재빌드한다. Design 기반 재생성은 Scene 수동 배치를 덮어쓰므로 설계 좌표 변경은 사용자/Claude 쪽 원본과 함께 반영한다.

### Task별 결과

- 입력 수정본 `62cc8c1`을 요청한 별도 커밋으로 먼저 보존했다. 이전 입력의 임시Scene은 폐기했고, 이전 측정은 현재 인수 근거에서 제외했다. Design에 에이전트 수정0.
- T1a `02dbc88`, T1b `40683b4`, T1c `17e1270`, T2 `535ec2f`, T3 `bc59cc0`. 주요 변경은 B09Authoring·V5 Scene6개·V5Launch·V5PatrolBinding·검증기다. [생성](Validation/B16b-generation.md), [순찰](Validation/B16b-patrol.md), [Scene/요소표](Validation/B16b-scenes.md), [문/저장](Validation/B16b-runtime.md).
- T4 정적 검사는 완료, 데이터FAIL10/WARN4를 원문으로 남겼다. A02 도달성9·머리공간3, A04 도달성1, A06 머리공간1. 낙관적 포고 그래프라 실제 물리 도달성을 보장하지 않는다. A04 적x13이 patrol[4,12] 밖에서 시작하는 데이터도 보존했다.
- T4 `a777653`, T5 `a0f103c`, T6 `9269fad`, T7 `f0b088f`, BL1 `add41ab`, BL2 `fbfc6ee`, BL3 `077ee71`. 주요 변경/실제 검증/문제는 [Task별 요약](Validation/B16b-summary.md).
- T5 [전달 결과](Validation/B16b-delivery.md): `Builds/B12-delivery` 최신, `Builds/B13-delivery` 직전 유지. 기존 V4 런처 내용 변화0. V5 전용 슬롯은 실행 때 fixture로 초기화한다. 임의 방 fixture Continue는 공식 A01/A03 외 도착을 보장하지 않는다. V3 안내 규칙은 그대로라 대시 뒤 파일럿 밖 닫힌 문을 가리킬 수 있다.
- [실제 입력 경로](Validation/B16b-route.md): A01→A02→A03→A04 자연 연결. A04 위 발판 접근 x15.51/y2.77에서120초 입력 한도, 전체 자연 대시 완주 미확인. A05는 독립 낙하 도착에서1.50초에 대시 수집. 입력 보정1회 후 결과를 확정했고 지형은 변경하지 않았다.
- 자동 문 검사는 문 앞 fixture에서 실제 접촉해 확인했다. 자연 입력 완주와 사람의 시야·길 찾기·손맛은 별도다. Backlog1~3 완료. 마지막 기본 회귀13/13·카드 결과는 [배치 요약](Validation/B16b-summary.md). push·Sprint Gate 없음.

### 다음 Batch 후보

V5 직접 플레이 결과와 데이터 후보를 Claude에게 검토할 자료로 전달한다. 층 B/C·수치 수정은 승인하지 않았다. 기존 D-NEXT 전체 플레이와 B07 A02/C03·V2 W22 검토는 유지한다. Approved Backlog는 비어 있다.
