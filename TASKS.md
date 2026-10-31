# 개발 작업과 기록

## 0. 지금

- 갱신: 2026-11-01, Codex (Batch24 진행 중).
- Base `88f458e`, 기존 B23 미푸시16커밋·B23 태그 보존, Claude draft5 문서 선행 커밋 완료. fetch/push 없음.
- Preflight PASS: hubLicensingClient=True·Unity exit0. 기존 사용자 변경은 승인된 선행9파일 커밋으로 보존.
- 승인 범위: P1~P5, T0~T14. 사용자 직접 지시로 P3 승인(지시문 보류 표기보다 우선), PRD 지정3곳·조각 저장/HP 포함.
- P1 Quiet13/13, P2 감실·적 검증 DONE·고리 잔여. P2 Quiet13/13, P3 조각/HP·저장 검증·Quiet13/13 완료. P4 보스2 아레나 생성·검증 중. 단계 실패는 독립 다음 단계로 분리, 사람 검수는 끝에1회. Ready Queue0.
- 기존 수락 대기·직접 플레이 순서 보존. 방 좌표·기본 튜닝 원본은 변경하지 않음.

## 1. 현재 상태 / Current Sprint

- Sprint3: V5 draft4.2 층 A 파일럿. B23 등장·복귀·승강기 구현과 개별 실측 완료. 방 기하 기준 미달·새 자동 고리 미완주, 결정4건.
- B19 검사 판정 정리와 B20 결과는 [B19](Validation/B19-summary.md)·[B20](Validation/B20-summary.md) 참조.
- 이번 [Batch23](Design/proposals-1031/01_CODEX_BATCH23.md)는 사용자 2026-10-31 실행·재개 지시로 승인됐다.

## 1-0. Current Tasks (Batch24)

실행 지시: [Batch24](Design/proposals-1101/01_CODEX_BATCH24.md). 사용자 2026-11-01 “읽고 진행해줘 P3의 경우는 승인해서 변경하는쪽으로”. 선행 Claude 설계9파일 `88f458e`, 이전16커밋 보존.

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| B24-T0 | DONE | 전달 피드백·사전 결정·P3 직접 승인 반영 |
| B24-T1 | DONE | draft5 Scene6 재생성·정적 FAIL0/WARN27·Scene FAIL0(기존 A02 overhead4 해소)·A01 E4/감실·A02/A05 E2 요소 수 일치. raw/24/T1-* |
| B24-T2 | DONE | A03 4·A02 5·A04 2·A06 3구간 각 머리0/5·착지5/5. A02 조각 P3는 코요테 이륙 필요(보통 이륙5/5 머리, 수정 입력0/5). raw/24/T2-head2-editor·T2-shard-coyote |
| B24-T3 | PARTIAL | 연속 승강기→A04 포고→B1 처치13.017s HP3→대시→도랑→A03 건물/W옥상 PASS. 반환 승강기 surface13/feet1로 재탑승 실패·Continue 미실시. raw/24/T6-revisit2-reboard |
| B24-T4 | DONE | A01 감실 무능력0/5·대시5/5·실제 조각 수집 PASS. raw/24/T4-vault1 |
| B24-T5 | DONE | A01 E4 포고1·피해0, A02 E2 점프5/5·A05 E2 점프대시5/5 모두 피해0·Error0. 정적 적 배치 FAIL0. raw/24/T5-enemies1 |
| B24-T6 | BLOCKED | 반환 승강기 탑승이2회 실패, T3 후반/Continue가 선행이라 A01 왕복은 실행 전 중단. 실패한 재탑승 점프 실험 제거·원문 보존. P5 exe의 다른 검증 접근만 확인 예정. raw/24/T6-revisit1·2 |
| B24-T7 | DONE | 사용자 직접 P3 승인으로 02_FRAGMENT_HP의 PRD71/83/111 세 문장만 원문 일치 확인 후 그대로 패치. 새 성장/ID/최대HP 저장 예외 |
| B24-T8 | DONE | 실제4조각 maxHP4→5·HP3→5·안내1.500s, ID/bonus 즉시 저장·중복거절·Continue·재진입숨김·NewGame초기화·구version1 PASS/Error0. raw/24/T8-fragments1 |
| B24-T9 | DONE | 충격파 높이는 기존 보스별 waveHeight 재사용, 양방향/첫예고/패턴순서 필드 기본은 기존 유지. B1·B3 cycle/phase/사망 회귀 PASS, 금지 튜닝diff0. raw/24/T9-* |
| B24-T10 | DONE | 실제 획득→3u 이탈 잠금·인트로1.817s·첫예고1.000s, 양방향 실 collider2.800u PASS. 초기 비활성 RoomExit 트리거 처리 보완. raw/24/T12-patterns3-physics |
| B24-T11 | REVIEW | 후보HP26·피해1·실제 패턴2,3,7만 순서 관측·페이즈전환0. 실높이2.800·양방향·첫예고1.000s 확인. 양벽 ClimbableWall 시험장, 감각 수락 대기. raw/24/T12-patterns3-physics |
| B24-T12 | TODO | 승인 단계 순서대로 진행 |
| B24-T13 | TODO | 승인 단계 순서대로 진행 |
| B24-T14 | TODO | 승인 단계 순서대로 진행 |

### 이전 Current Tasks (Batch23)


실행 지시: [01_CODEX_BATCH23](Design/proposals-1031/01_CODEX_BATCH23.md), 사용자 2026-10-31 “읽고 진행”. Base `e085ed9`. 별도 Batches 파일은 생성하지 않았다.

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| B23-T0 | DONE | 전달 플레이 기록·사전 결정 2-1/10절 반영. 복귀 딜레이 요구 유지 |
| B23-T1 | REVIEW | 등장1.800s·피해0·이동만 허용·B3 패턴/페이즈/엔딩 PASS. B1 감각 수락 대기 |
| B23-T2 | DONE | Unity API HP14→22만 변경. 실제 무능력 처치12.867s·HP3·패턴3종·Error0. raw/23/T1-T2-boss1 |
| B23-T3 | REVIEW | getUp0.450s·추가 피해0·총복귀0.650s·HP0 기존 사망 PASS. 기존3값 유지. 일어나기 감각 수락 대기 |
| B23-T4 | DONE | low 잠금 없음·실제 X 레버 상→하→상 탑승 PASS. V4 B01/C01 기존 위 시작·하단 잠금·상단 해제 PASS. raw/23/T4-lift-centered |
| B23-T5 | PARTIAL | 새 규칙·승강기 양방향 self-test PASS. 첫 고리 접근19→0, A06 FAIL3 잔존. 원문의20건은1.1u 오름 포함으로 본문≥1.5와 충돌. raw/23/T5-comparison.txt |
| B23-T6 | PARTIAL | 6 Scene·요소 수 일치. Scene A02 overhead FAIL4·정적 A06 FAIL3. 좌표 변경 금지로 보존, raw/23/T6-* |
| B23-T7 | PARTIAL | 연습30회 각0/5·5/5 PASS. A01/A02 접근0, A04 두 번째접근1/5 접촉·4/5착지. 무너짐3·가시복귀2방·높은조각 PASS. raw/23/T7-field2/practice2 |
| B23-T8 | BLOCKED | 자동 경로 재시도 종료·실패한 전투 입력 보완만 T7 소스로 복원. 승강기·A04 진입 PASS, 일반 적 피해/무너짐 착지에서 중단. 새 전체 고리·그 뒤 Continue 미통과. 구성요소 PASS와 구분 |
| B23-T9 | PARTIAL | README·질문2/기록4칸·새 빌드 런처 smoke2 PASS. T8 미완주로 안내에 한계 명시 |
| B23-BL1 | DONE | A06 본문 FAIL3(간격1/1/1.4u·오름2), 참고 APPROACH5(추가 오름0.9 두 곳). 데이터 수정0. raw/23/T5-comparison.txt·BL1-reference-d42.txt |

### 이전 Current Tasks (Batch22)

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| B22-T0 | DONE | 01_DECISIONS 반영. 사용자 대화(Claude 앱) 전달 결정과 추천을 구분. B21 완료 결과 유지 |
| B22-T1 | DONE | ID 저장·구 version1 로드·Continue/초기화 PASS. CompletionBuildVerification 처치 즉시 저장+별도 프로세스 Continue/엔딩 PASS·Error0. raw/22/T1-save·T1-completion |
| B22-T2 | DONE | ID/아레나/트리거/덮개 인스턴스화. BossRecovery·B3 8패턴·2페이즈·사망 초기화·Slam/엔딩 PASS. 후보 선택 주입은 최종 보스만 |
| B22-T3 | DONE | HP14/피해1·돌진7.5u/s·도약/충격파/돌진만 관측·페이즈전환0. 실제 윗면포고1·vy11.000u/s. raw/22/T4-complete |
| B22-T4 | DONE | BossTest_B1 실제 무능력 처치8.500s·HP3·사망→CP-A03·HP14초기화·ID저장·재진입 보스/게이트/덮개 없음 PASS·Error0. Play-BossTest-B1 제공 |
| B22-T5 | REVIEW | V5 인자 없이 HazardReturn·안전바닥/복귀 실측 PASS. V4 기본 hazard=False·Error0, 기존 DamageBlock 변경0 |
| B22-T6 | DONE | C-1/C-2/C-6 필드 fixture 및 잘못된 spawn 거절 PASS, A03 CP 하나·6방 잠금/벽0 검증 |
| B22-T7 | DONE | 6방 생성. A04 직행0/5·좌복귀5/5·등불2포고보상5/5, A05 무능력0/10·진입쪽복귀10/10·대시10/10, A03 각각0/5·5/5. 정적FAIL0/WARN16·SceneFAIL0 |
| B22-T8 | REVIEW | 실제 연속 고리46.731s·보스전8.618s·일반 적 포함·대시 획득·3연습·Continue PASS·Error0. 자연 사용자 고리 수락 대기 |
| B22-T9 | DONE | README·TASKS3/9·고리/보스 런처·질문2개/기록 칸 |
| B22-BL1 | DONE | 몸1.3 연속 고리46.716s·모든 단계/Continue PASS·Error0. 기본 크기 변경0. [원문](Validation/_raw/22/BL1-body13/result.txt) |

### B21 결과 인계 처리

T1/T2 잠금 코드·T3 검사기는 유지하며 draft4.1 데이터에 잠금/벽이 없으면 비활성을 확인한다. T4 가로 한계는 설계 근거, T5 REVIEW는 V5 기본 적용 후 고리 수락 대기, T6/BL2는 기본 꺼짐 유지, T7/BL1은 D-BODY 참고다. BL3의 draft3 섬 성공은 draft4.1 실측을 대신하지 않는다.

### 이전 Current Tasks (Batch21)

Base `7d40918`(Claude 설계 계약·지시문 시작 커밋). 이전8커밋 보존. [T3 검증](Validation/B21-room-rules.md)·[재개 종료 요약](Validation/B21-resume-summary.md). [이전 차단 원문](Validation/B21-summary.md) 보존. 재개 Base `544ea5e`에서 T1부터 진행하며 T3 `736c6aa`는 반복하지 않는다.

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| B21-T1 | DONE | 생성기 잠금 키/벽 방향·V4ExitRule AND 조건·잠김 고체/열림 trigger 복원. 최종 빌드0오류·규칙 exe Error0·몸 오른끝48.9850 문앞 정지·Quiet13/13. [근거](Validation/B21-lock.md) |
| B21-T2 | DONE | Scene6개 재생성·기하/요소 보존·실제 exe5항목/14문/stub4/일방역2 PASS·Error0·Quiet13/13. [근거](Validation/B21-shortcut.md) |
| B21-T3 | DONE | 잠긴 우회 MEASURE, 무잠금/틀린 키/다른 벽 키/봉인 FAIL 검출. 전체 정적 FAIL0/WARN0·기존 fixture 유지 |
| B21-T4 | DONE | B21Arena 생성·4레시피 각3회/0.05u 실측. 달리기4.55~4.60·공중대시7.35~7.40·지상대시후4.30·대시만3.05u. Quiet13/13. [근거](Validation/B21-gaps.md) |
| B21-T5 | REVIEW | 기본 꺼짐·A05 통과0/5/동쪽복귀5/5·도랑폭24회·Quiet13/13. A04 섬 접지 PASS(포고3·복귀2). 후보 채택 대기. [근거](Validation/B21-hazard-return.md) |
| B21-T6 | REVIEW | 0.25u 모서리 후보·꺼짐 B20 최대값 재현(짧은1회편차 보존)·접촉33→10/36·벽점프/포고/대시각10·몸1.3/1.1 실제보정·Quiet13/13. 종료 기본13/13. [근거](Validation/B21-corner.md) |
| B21-T7 | DONE | 36면 y/현재 여유/4.22u 대비 부족분0.12~1.82u·오름·천장/외곽 비교 완료. Design 변경0. [표](Validation/B21-headroom-table.md) |
| B21-T8 | DONE | 최종 Development0오류·런처8+대시후보+BodySizeTest각10초Error0·최종기본/Quiet각13/13·정적/Scene FAIL0/WARN0·보호88변경0·카드범위/링크/런처0 |
| B21-T9 | DONE | 후보 채택 결정2개 추가·직접 플레이 순서·README·0/9절 인계. 이후 승인 Backlog 수행 |
| B21-BL1 | DONE | 높이1.3/1.1 동일36면 표·원자료 후보각32면·부족분/오름/천장 비교. Design 변경0. [표](Validation/B21-headroom-small.md) |
| B21-BL2 | REVIEW | 기본꺼짐 별도토글·PlayerDash 후보. 꺼짐 T6개별값동일·켜짐0.2u턱10/10·Quiet13/13. [근거](Validation/B21-dash-corner.md) |
| B21-BL3 | DONE | A04 실제 적제외·능력없음5/5 섬보상·피해0/복귀0·포고2·Quiet13/13. [근거](Validation/B21-a04-island.md) |

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
| D-BODY | [BodySizeTest](Launchers/current/Play-BodySize.cmd)와 V5에서 비교 후 높이1.6 유지 / 1.3 / 1.1 중 선택 | 1.3을 비교 우선 후보로 추천. 키·폭이 약19% 줄고 기본보다 공격 사거리가19% 줄어드는 절충. 채택은 직접 플레이 후 판단 | 기본 크기 반영 다음 배치·B20-T4 REVIEW. [측정](Validation/B20-bodysize.md). 현재 런처는 Builds/B22-delivery, B20 슬롯12개는 B21 raw에 보존 |
| D-CORNER-FIX | [머리 후보](Launchers/current/Play-V5-CornerFix.cmd) 0.25u 머리만 / 머리+대시 / 미채택(허용값 재비교 가능) | 0.25u 직접 비교 추천. 머리 접촉33→10/36·작은 몸에서도 실제 보정. 넓은 천장은 계속 막힘 | B21-T6·BL2 REVIEW와 기본 적용 다음 배치. [머리](Validation/B21-corner.md)·[대시](Validation/B21-dash-corner.md) |
| D-B23-GEOMETRY | A02 platform3(상면10.5)·climb0(하단11) Scene overhead FAIL4, A04 다음 platform2 실제 머리 접촉1/5. Claude 데이터 수정 / 문제 경로 보류 중 선택 | 데이터 수정 추천. 승인 범위상 Codex 좌표 변경 금지 | B23-T6 FAIL0·T7 머리0 기준 |
| D-B23-RULE | 접근 규칙 본문(오름≥1.5)과 참고 스크립트·20건 기대가 불일치. 엄격 본문 유지 / 낮은 오름 포함 선택. A06 기존 위반도 전체 FAIL0과 충돌 | 본문 유지 추천. 첫 고리19→0, A063건은 다음 draft에서 수정 | B23-T5/T6 전체 FAIL0 및 기대20건 |


결정 대기4건. draft4.1·V5 HazardReturn 기본 켜짐은 B22 승인으로 이동했다. 기본 크기·모서리 후보 채택은 대기다. D-V5-CLEARANCE와 WORLD의 A06 적 표기 정리는 B19에서 해소한 상태를 유지한다. [기존 근거 장부](Validation/B19-rules-ledger.md), [B20 추가](Validation/B20-rules-ledger-add.md).

## 2-1. 결정 기록

| ID | 결정 | 날짜·출처 |
| --- | --- | --- |
| D-B24-EXEC | 확장 Batch24 P1~P5 실행·단계별 회귀·사람 검수 마지막1회 | 2026-11-01 이번 사용자 직접 지시·[지시문](Design/proposals-1101/01_CODEX_BATCH24.md) |
| D-FRAGMENT-HP | **P3 승인**: 조각4개마다 최대 HP+1·전회복·ID 저장·HUD, PRD 지정3곳 패치. 지시문 첫 줄 `P3: 보류`보다 직접 지시 우선 | 2026-11-01 사용자 “P3의 경우는 승인해서 변경하는쪽으로” |
| D-HUB-STREET | 빈 평지 대신 A03 건물 거리 | 사용자 2026-11-01 결정의 Claude 전달, [00_README](Design/proposals-1101/00_README.md) |
| D-BIG-BATCH | 작은 반복 대신 큰 묶음, 사람 검수 마지막1회 | 사용자 요청의 Claude 전달, 같은 문서 |
| D-DRAFT5 | A01 대시 감실·E4/E2·A06 수정 데이터 반영 | Claude 추천, 이번 실행 범위 승인. 감각 수락 별도 |
| D-BOSS2 | 벽 점프 획득 후 싸우는 보스2 시험 아레나. 규칙/수치는 [사양](Design/proposals-1101/03_BOSS2_SPEC.md) | 사용자 D-BOSS4 원칙·Claude 추천 수치, 이번 시험 구현 승인 |


B23 실행은 2026-10-31 사용자 “Design/proposals-1031/01_CODEX_BATCH23.md 읽고 진행”, 환경 조치 후 “다시해줘 틀어놨어”로 재개했다. 아래 전달 출처는 Claude 작성 [00_README](Design/proposals-1031/00_README.md)이며 사용자 대화 전문은 없다. 추천 수치는 이번 Batch 구현 범위 승인과 최종 플레이 수락을 구분한다.

| ID | 내용 | 출처 구분 |
| --- | --- | --- |
| D-HUB-CLEAR | A03 구조물 삭제, 필드 구덩이·오름에 이유 있는 요소 | 사용자 결정 2026-10-31 전달 |
| D-BOSS-INTRO | 등장 방향·무피해는 사용자, 총1.8s는 Claude 추천 | B23 구현 승인, 감각 수락 별도 |
| D-BOSS1-HP | 체력으로 난이도 보완 방향은 사용자, 14→22 값은 Claude 추천 | B23 구현 승인 |
| D-RETURN-GETUP | 일어나는 딜레이 방향은 사용자, 0.45s는 Claude 추천 | B23 구현 승인, 감각 수락 별도 |
| D-CLIMB-GAP | 떠 있는 발판 오름≥1.5u의 가로 간격≥1.8u | Claude 추천, 이번 검사 규칙 범위 |
| D-HUB-LIFT | 거리↔위층 동쪽 승강기, low 시작 | Claude 추천, 이번 구현 범위 |
| D-HAZARD-RETURN-B23 | V5 기본 복귀 유지, 사용자 추가 요구는 딜레이 | B22-T5 REVIEW 유지: 명시 수락 대신 개선 요구로 기록 |


아래 10/28 설계 결정의 전달 출처는 사용자 대화(Claude 앱) → 01_DECISIONS.md다. 직접 대화 전문은 없으며 D-BOSS1-LINK·D-HUB-ENEMY는 Claude 추천값을 이번 실행 지시 범위에서 사용한다.

| ID | 결정 | 날짜 | 근거 |
|---|---|---|---|
| D-B22-EXEC | 보스1·draft4.1 통합 실행. 기존 최종 보스 호환을 유지하는 ID별 저장 확장(T1) 승인. 계약·사전 결정의 추천값도 이번 범위에서 실행하며 감각 수락과 구분 | 2026-10-28 | 사용자: 지정 06_CODEX_BATCH22_BOSS1.md “읽고 진행해줘” |
| D-PROC-1028 | 잔수정→검수 반복 대신 방 목적+요소 스펙 단위로 요청. 검수는 구역(고리) 단위로 1회. 운영 체계(AGENTS·Batch 승인)는 유지 | 2026-10-28 | 방 5~6개 1~2분 소요, 수정해도 체감 변화 없음. 원인은 내용물 부족 |
| D-V5-DRAFT4 | **채택**: draft4.1 (WORLD_V5 13·14절). A03 허브 적0+CP, A03 대시 연습, A04 두 갈래+보스1 아레나, A05 벽 삭제·도랑 게이트 | 2026-10-28 | 기존 결정 대기 해소 |
| D-A05-TRENCH | A05 부서지는 벽 삭제. A03→A05 직행은 도랑이 막음 | 2026-10-28 | 벽 잠금 계약이 반복 문제. 지형이 같은 역할을 단순하게 함 |
| D-HAZARD-RETURN | **V5 기본 켜짐**(B21 T5 후보, 마지막 안전 바닥 복귀). V4는 기존 유지. 최종 채택 여부는 고리 플레이에서 판단 | 2026-10-28 | 도랑 게이트에 필요. 실측 A05 우회0/5·동쪽 복귀5/5 |
| D-A04-BOSS | A04 일반 적 1, 오른쪽 x24~49 보스1 아레나 | 2026-10-28 | 대표 구역에 전투 정점 필요 |
| D-PRACTICE | 능력 연습은 얻은 방이 아니라 다음 방에서 | 2026-10-28 | 사용자 판단 |
| D-BOSS4 | 보스 4. 홀수 싸우고 얻음 / 짝수 얻고 싸움. 1 A04 → 대시, 2 B04 벽 점프 획득 후 벽 점프로 잡음, 3 C05 처치 → 더블 점프, 4 C06 최종 모든 능력(처치 시 Slam 유지). Target 4 / Minimum 2 | 2026-10-28 | PRD·GAME_DESIGN 반영됨(로컬 커밋 10/28) |
| D-SPLIT | 방 데이터는 Claude 담당, Codex는 V5 Design JSON 수정 금지. B22는 Codex가 보스1 구현 + draft4.1 씬 반영까지 한 번에 | 2026-10-28 | 방 데이터 확정 후 한 번 실행으로 합치기까지 |
| D-A03-PRACTICE-HAZARD | A03 대시 연습 3단계의 공중 hazard는 이번에 넣지 않음 | 2026-10-28 | 검사기 수정 필요. 우선순위 낮음 |
| D-B21-RESUME | Hub 실행 후 T1부터 재개, T3 반복 금지. Unity·Unity 호출 스크립트는 샌드박스 밖에서 실행 | 2026-10-26 | 사용자 이번 대화 직접 지시. 환경 변경 근거로 Preflight 재확인 허용 |
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


- [Play-V5-HazardReturn](Launchers/current/Play-V5-HazardReturn.cmd): B21 복귀 수락 항목은 아래 B22 고리 플레이에 통합. 이제 모든 V5에서 같은 기본 복귀를 사용하며 별도 꺼짐 비교가 아님.
- [Play-V5-CornerFix](Launchers/current/Play-V5-CornerFix.cmd): A01/A02 작은 머리 모서리에서 상승 유지와 옆 밀림 체감. 넓은 천장은 계속 막힘. 3~5분.
- [Play-V5-Candidates](Launchers/current/Play-V5-Candidates.cmd): 두 후보 동시, A01 머리/ A05 도랑 비교5~10분. `-BodyHeight 1.3/1.1` 인자로 크기와 함께 실행 가능.
- 선택 비교 [Play-V5-DashCornerFix](Launchers/current/Play-V5-DashCornerFix.cmd): 대시 발끝 모서리 보정 추가, A01/A02에서 대시 중 낮은 모서리3~5분. 기본/기존 머리 후보에는 이 토글 꺼짐.

- [Play-V5-Loop](Launchers/current/Play-V5-Loop.cmd): 기존 수락 순서 뒤 B23 등장/복귀·승강기/서쪽 대시 연습 확인, 예상10~15분. A03→승강기→A04 보스→A05 대시→A03 승강기→서쪽 연습/선반. 자동 전체 고리는 A04에서 미완주했다. 레버 X 후 중앙 탑승. 질문: ① 보스 등장이 “나타나는” 느낌인가? ② 위층 연습과 승강기가 자연스러운가? 기록: 고리 시간____ / 보스 시도____ / 막힌 곳____ / 머리 박은 곳____. [보스 단독](Launchers/current/Play-BossTest-B1.cmd)3~5분. A02/A04 머리 문제는 아래 인계 참고. 새로운 연출 감각은 수락 대기다.

조정 값 위치와 Play 종료 후 반영은 [LEVEL_V4](LEVEL_V4.md). 기본 보스(B3, D-B3 10/14)는 [기본 BossTuning](Assets/ScriptableObjects/BossTuning.asset)에서 조정하고(Boss-B3 런처는 [후보 Asset](Assets/BossCandidates/B3/BossTuning.asset)을 쓴다) Play 종료 후 원하는 값을 명시 저장·재빌드한다.

## 4. 열린 버그

| ID | 증상 | 근거 |
|---|---|---|
| B07-V2-BOSS-REPLAY / B09-REGRESSION | 새 빌드 V2 고정 입력이 W22 보스에서 실제 사망(tick285, 진입편차.082u), 이전 B06b 빌드는 PASS. 원인 미확정 | [B07-T25](Validation/B07-T25-summary.md), [B09-T11](Validation/B09-T11-summary.md) |
| B07-A02-TRAVERSE | A02 필수 계단·재방문 굴뚝: 자동 입력 미완주, 사람 확인 필요 | [B07-R-A02](Validation/B07-R-A02-summary.md) |
| B07-C03-OPTIONAL | C03 첫 선택 포고 도전: 자동 입력 미완주, 사람 확인 필요 | [B07-R-C03](Validation/B07-R-C03-summary.md) |
| V5-HEADBUMP | 사용자 점프 머리 걸림 피드백. h1.6 머리 충돌 후보36면·격리 실측33/36 접촉, 모서리 수평 보정 없음. 불일치3과 초기 문 전환2도 보존 | [플레이 CSV](Playtest/20261022-233114.csv), [B20 T3](Validation/B20-headbump.md), [전체 실측](Validation/B20-headbump-all.md) |

B16-GENERATOR·B12-CHASE-VERIFY·B18-CLEARANCE·B18-STATIC의 해소 상태는 유지한다. 이전 실패 기록은 기존 Validation에 보존했다. 열린 버그4건, 결정 대기2건이다. V5-A05-TRENCH는 B22 기본 복귀와 무능력0/10·대시10/10 실측으로 기술적으로 해소했고 최종 감각 수락은 T5/T8 REVIEW다([B22](Validation/B22-summary.md)). V5-A05-SHORTCUT은 B21 T1/T2에서 실제 잠금/방향/저장/14문을 확인해 해결했다([근거](Validation/B21-shortcut.md)). 정적 후보와 실제 진행 불가 판정은 구분한다.

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

## 9. Latest Handoff — Batch23

### 결정 질문지

- D-BODY·D-CORNER-FIX: 기존 크기/모서리 후보 선택 대기. 기존 비교 순서와 추천은2·3절 유지.
- D-B23-GEOMETRY: A02 조각 길(상면10.5·굴뚝 하단11, Scene FAIL4), A04 무너지는 발판 뒤 머리 접촉1/5를 Claude가 수정 / 현재 경로 보류. **수정 추천**: 머리0 기준 회복과 고리 검증을 위해 필요하다. Codex 좌표 변경은 금지라 보존했다. 승강기는 오른쪽 끝에 남으면 위층 발판 아래와 부딪혀 중앙 탑승이 필요하다.
- D-B23-RULE: 본문 오름≥1.5 유지 / 참고 스크립트처럼 낮은 오름 포함. **본문 유지 추천**. 첫 고리19→0이며 기대20과 차이는 A01 오름1.1이다. A06 기존3 FAIL은 다음 draft에서 수정해야 전체 FAIL0을 충족한다. T5/T6을 푼다.

### 직접 플레이 확인

기존 V4 장치·경로·저장·보스/엔딩 → BodySizeTest·몸 비교 → 머리/동시/대시 모서리 비교 순서는3절을 유지한다. B22 기본 복귀는 사용자가 딜레이 추가를 요청해 T5 REVIEW를 유지하고 이번 일어나기와 함께 본다.

그 뒤 [Play-V5-Loop](Launchers/current/Play-V5-Loop.cmd), A03→승강기→A04→A05→A03→승강기→서쪽 연습.10~15분 예상. 질문: ① 보스 등장이 “나타나는” 느낌인가? ② A03 위층 연습과 승강기가 자연스러운가? 기록: 고리 시간____ / 보스 시도____ / 막힌 곳____ / 머리 박은 곳____. **새 자동 고리는 미완주**이며 이전 B22 성공으로 대신하지 않는다. [보스 단독](Launchers/current/Play-BossTest-B1.cmd)은3~5분.

조정: B1 BossTuning.asset HP22·피해1·돌진7.5u/s, Scene BossEncounter `introDuration`1.8s·낙하높이·흔들림, B21HazardReturn.asset `fadeSeconds`0.15s·`getUpTime`0.45s. 기존 접지0.1s·여백0.5u·정지0.12s 유지. Play 종료 전 값을 기록하고 Edit 모드에서 명시 저장 후 재빌드한다. 기본 몸1.6·모서리 후보 꺼짐 유지.

### Task별 결과

Claude 설계 변경: `667543d`, Design/v5 3파일·Design/proposals-1031 3파일·GAME_DESIGN·D42 검증 문서. 기존 내용 그대로 선행 커밋했으며 이후 Design 변경0. [종료 요약](Validation/B23-resume-summary.md)에 실제 FAIL/MEASURE·원문·A06 목록·미실시를 연결했다. 이전 [B22 인계/결과](Validation/B22-summary.md)·현재 이전 Task 표와 미종결 목록을 보존한다. Archive는 이번 허용 범위 밖이라 옮기지 않았다.

| Task | 상태 | 커밋·변경·실제 결과 |
| --- | --- | --- |
| T0 | DONE | `8d4d3ab` TASKS 전달 피드백·추천/사용자 결정 구분 |
| T1 | REVIEW | `3c0416f` 등장1.800s·양쪽 피해0·이동만, B3 8패턴/페이즈/사망/엔딩 PASS |
| T2 | DONE | `1bbbbf0` B1 HP22, 무능력 실제 처치12.867s HP3 |
| T3 | REVIEW | `5b1d291` 일어나기0.450s·추가 피해0·HP0 기존 사망 PASS |
| T4 | DONE | `b370eb1` startLow·실제 레버 상하상 탑승, V4 B01/C01 위 시작/하단 잠금/상단 해제 PASS |
| T5 | PARTIAL | `d65bedc` Tools 검사기 양방향 승강기·간격 fixture PASS. 첫 고리19→0, A06 FAIL3·WARN24. 기대20 불일치 |
| T6 | PARTIAL | `0027aa3` Unity 생성6 Scene·요소 수 일치. Scene A02 overhead FAIL4·정적 A06 FAIL3 |
| T7 | PARTIAL | `cf37877` 연습 각0/5·5/5, 무너짐3·가시2방·높은조각 PASS. A04 머리1/5 잔존 |
| T8 | BLOCKED | `665753e`·`539fa2b` 실패 원문 보존·실험 제거. 연속 고리는 A04 일반 적 피해/무너짐 착지에서 미완주. 재시도 종료, 그 뒤 Continue 미실시 |
| T9 | PARTIAL | `698e041` 안내 커밋. README/3·9절·런처 갱신, V5 Loop/V4 BeforeBoss smoke PASS. 고리 검증 한계 명시 |
| BL1 | DONE | `343b203` A06 본문3·참고5 검출 목록, 데이터 수정0 |

기본 회귀13/13·Development 오류0/경고0. B1 패턴 관측은 HP100 fixture, 무능력 승리는 기본 HP 별도 입력이다. 방별 시험은 시작 pose/능력 fixture이며 전체 자연 고리 성공 증거와 구분한다. 고리 중 HP·위치·보스·획득 능력 쓰기는 없었다. Release·실제 패드·사람 감각·Sprint Gate는 미실시다.

### 다음 Batch 후보

Claude가 A02/A04 머리 기하와 승강기 탑승 동선을 검토하고 A06·접근 규칙 충돌을 정리한 뒤 새 고리 검증을 승인한다. 보스2~4·다른 층 확장은 하지 않았다. Backlog 처리·Queue0으로 종료한다.

## 10. Playtest·Before-After

| 날짜 | 방·대상 | 사용자 피드백 / 변경 전→후 | 근거 |
| --- | --- | --- | --- |
| 2026-10-22 | A03→A05·A05 도랑·점프 | 지름길 우회·무능력 도랑 통과, 점프 불쾌 원인 ①크기 ③머리 충돌. B20은 측정·비교 도구 추가까지; 기본 설계 수정·개선 수락은 아직 없음 | [사용자 로그](Playtest/20261022-233114.csv), [전달 지시](Batches/BATCH-20.md), [측정 결과](Validation/B20-summary.md) |

| 2026-10-31 | A03~A05 고리 | 보스/처치 후 구멍 좋음·대시 익음. 보스 등장 딜레이·HP 증가 방향·복귀 일어나기 요구. A03 위 마을→S0 머리 충돌 불쾌, 맵 빈 느낌·얕은 구덩이 기대·계단 늘림 느낌. 시간·시도 수 미기록. B23 변경 후 결과는 미확인 | [Claude 전달 플레이 기록](Design/proposals-1031/00_README.md) |

| 2026-11-01 | B23 A03 허브 | 사용자 “A03 전부 사라졌는데 비었어”, 분량 추가 요청. Claude는 허브 평지화를 과잉 해석으로 기록, 건물 거리 draft4.3/5 작성. B24 반영 후 플레이 수락은 미실시 | [전달 기록](Design/proposals-1101/00_README.md) |

B24 Ruling: PRD77의 과거 DEC-LEVEL-001은 당시 세션 한정 발견 승인 기록으로 보존한다. 2026-11-01 사용자 D-FRAGMENT-HP 직접 승인과 지정3곳 패치가 이번 영구 조각/HP 구현의 근거이며, 승인 밖인77행은 바꾸지 않는다.


