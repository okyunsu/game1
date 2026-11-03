# 개발 작업과 기록

## 0. 지금

- 갱신: 2026-11-03, Codex — DOC-01 문서·기록 정리 진행 중(90분/Task9).
- Base `f1a6f5a`; 현재 HEAD는 `git log -1`. origin/main은 로컬 참조, 이번 fetch/push 없음.
- 시작 미푸시24커밋(B25 재개23+별도 Claude 아트1), 이번 커밋은 별도 DOC-01로 기록.
- 사용자 기존 변경4경로 보존: Design/v5 2파일·Design/proposals-1103-B26/·Validation/D61-room-rules-v5-AB.md.
- Unity/게임 코드/Scene/Assets/ProjectSettings/PRD/GAME_DESIGN/Design 수정 금지. Preflight -SkipUnity PASS.
- REVIEW 21개(고유 Task ID, B20-T4 본문 포함). 다음 콘텐츠 Batch 전에 사용자 QA 1회 필요.
- Codex DOC-01 차례. Ready Queue0, 기존 결정3건/직접 플레이 순서 유지.
- B25 제품 상태는 아래 Current Tasks와 기존 인계/Validation을 유지하며 이번 문서 정리로 재판정하지 않음.

## 1. 현재 상태 / Current Sprint

- Sprint3: V5 draft6 층 A·B12방, 건물 거리·대시/벽 점프 재방문·조각 HP 성장·보스1/2·직접 제작 아트와 효과 구현. 사용자 감각 수락·Sprint Gate 대기.
- 반환 승강기는 B25 T1/T12에서 기술적으로 해소했고 A 고리/Continue를 확인했다. B02/B06/B05 자동 경로 잔여는 아래 PARTIAL·열린 버그를 따른다.
- [Batch25](Design/proposals-1101-B25/01_CODEX_BATCH25.md)는 사용자 2026-11-02 실행 승인, Hub 실행 후 재개. 최초 차단 증거와 B24/B23 당시 상태는 보존한다.

## 1-DOC. Current Tasks (DOC-01)

[사용자 승인 지시·T0 시작 기록](Batches/BATCH-DOC-01.md), 문서 전용 / needs-unity=no / Base `f1a6f5a`.

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| DOC-01-T0 | DONE | 지정4경로 외 변경0·staged0, 시작 바이트 기록, Preflight -SkipUnity PASS/Unity 미실시 |
| DOC-01-T1 | DONE | D1~D6/D11 반영·기존 권한/보호/push 유지, 삭제·통합 문장별 남은 절은 DOC-01 요약 |
| DOC-01-T2 | DONE | 목표/시간·상한30/Hub 점검/Unity 없는 목록/A·B·C 링크·증거/REVIEW 규칙 정리 |
| DOC-01-T3 | DONE | B21~B24 원문 보관·미해결 단일 표, 상태 행 전후 동일·REVIEW 21(본문 B20 포함) |
| DOC-01-T4 | TODO | 결정 장부 |
| DOC-01-T5 | TODO | 옛 설계 참조·이동 |
| DOC-01-T6 | TODO | raw 정리 |
| DOC-01-T7 | TODO | README 상태 링크 |
| DOC-01-T8 | TODO | 종료 요약·태그 |

## 1-0. Current Tasks (Batch25)

사용자 2026-11-02 [Batch25](Design/proposals-1101-B25/01_CODEX_BATCH25.md) 실행 및 `Design/art_v1/` 선행 커밋 포함 직접 승인. Claude 설계 변경: `e6b5172`, Design/v5 draft6 AB·Design/art_v1·B25 지시문. 실행 Base `e6b5172`. 기존 B23/B24 33커밋 보존, fetch/push 없음.

초기 Preflight `preflight-20261102-165715-327-dff360f2`는 exit198로 차단했다. 사용자 Hub 실행 후 새 `preflight-20261102-223351-597-a0db2b7c`는 hubLicensingClient=True/exit0으로 재개. 원문은 raw/25에 보존하며 이전 B24 통과를 이번 증거로 재사용하지 않는다.

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| B25-T0 | DONE | 사전 결정 5행 출처 구분·실행 승인 기록. B24 결정 2건은 설계안 선택 완료, 구현 검증은 T2/T4~T10 대기 |
| B25-T1 | DONE | 접촉 skin 2×Physics2D.defaultContactOffset를 V5(startLow)에만 반영. 공격 관성으로 lift 옆0.015u 이탈이 원인. A03 왕복3회·다른층 호출/재탑승·V4 B01/C01 PASS/Error0. raw/25/T1-contact-skin, 원문실패 T1-launch-diagnosis·T1-call-trace. 초기 저장진입 일시실패는 별도 기록 |
| B25-T2 | DONE | 전투 lock 때 기존 획득대를 floorY로 내림·사망reset 복원, 다른보스 ref null 유지. 실제 획득/3u이탈/1.817s 등장·양방향2.8u, 바닥10/10 피격·획득대 회피0/10 PASS/Error0. raw/25/T2-sink10, P1 Quiet13/13 |
| B25-T3 | DONE | 증기 y0→top 정적 연결·혼합 승강기 인덱스/증기 부재·먼 증기 fixture PASS. AB FAIL0/WARN55→47, B02 위 통로·B03 선반/B04 문 reachability WARN8 해소. raw/25/T3-before.md·T3-after.md. 실제 물리 미검증 |
| B25-T4 | DONE | AB12 입력 wrapper·수평 구멍 문·B04 보스/우측 재도전/획득대sink·CP-B01 연결. fixture/생성12방 PASS, B05 중복 solid8 충돌은 동일영역 cover0에 위임. Scene은 T5에서 검증/커밋. raw/25/T4-fixtures·T4-generate |
| B25-T5 | DONE | AB12방 Unity 재생성·실제 요소 일치. 정적FAIL0/WARN47·SceneFAIL0/WARN0. B04는 JSON 천장spawn13을 보존하고 등장 시 바닥floorY 착지. raw/25/T5-skyspawn·T5-counts-final·T5-clearance-final·T5-static.md |
| B25-T6 | PARTIAL | AB12방/B6방·18문·방문전숨김/방문후표시·stub 체크 PASS. 허용 Scripts/Resources에 지도TextAsset, V5 기본AB. Editor batch 캡처 미생성·exe 캡처오류2·Computer Use 앱승인시간초과로 이미지 잔여. raw/25/T6-map-play·T6-map-exe·T6-native |
| B25-T7 | PARTIAL | A03→B01·B01시장/지붕·B03증기2/문 각5/5·머리0·Error0. B02 바닥 자동 포고는 정지0회/달리기1회 뒤 산성복귀로 실패, 경로별재시도 종료·좌표수치변경0. raw/25/T7-A03-five·T7-B01-five·T7-B03-five·T7-B02-five/running. 초기 도구실패 원문 보존 |
| B25-T8 | PARTIAL | B02 증기→위통로→조각5/5·머리0·HP4 PASS. B06 등불→무너지는 발판은 보통/공중대시 입력 각포고1 뒤 산성복귀 실패, 조각 미획득. 실패 대시 입력 보완 제거·원문보존, 좌표수치변경0. raw/25/T8-B02-upper-five·T8-B06-five/airdash |
| B25-T9 | DONE | B04 실제획득/저장→3u이탈잠금→등장1.817s/바닥착지·받침하강·첫예고1.005s/실높이2.800 양방향 PASS. 사망CP-B01·능력유지·우측재입장/중앙재시작·실입력32.350s 처치HP1/벽점프7/포고26·ID저장/재진입없음 Error0. raw/25/T9-B04-fight1 |
| B25-T10 | PARTIAL | B01탑/조각/B05·B05↔A02왕복/이동조각·B03굴뚝/B06 각3/3 PASS/Error0. A02 stale stub 해소(Design원본보존). B05서쪽 조각은 바닥구멍에서 A02로 전환되어 도보/늦은/앞당긴이륙3접근 실패·종료, 실패입력보완제거. raw/25/T10-* |
| B25-T11 | DONE | A01선반 V5-A01-3·A04섬 -0·A01감실 -4·B02위 -0 실제 트리거접촉4개→maxHP5/HP5·ID/보너스저장·Continue PASS/Error0. 접촉위치fixture이며 연속자연수집증명과 구분. raw/25/T11-first-four |
| B25-T12 | PARTIAL | 실제 A고리(B1→대시→반환승강기→연습/선반)65.207s·65.027s 및 그 뒤 Continue PASS, B01 CP/지붕/B02위통로까지 연속PASS. B02 E3앞 긴도약 HP1/후속포고 HP0 실패2회·중단, 실패포고실험제거. 전체AB/최종Continue 미실시. raw/25/T12-expanded1·expanded2-E3 |
| B25-T13 | DONE | PNG18개 원본바이트/SHA256 일치·Assets/Art/Original/v1 복사, Unity16PPU/Point/무압축/FullRect·meta/ArtSettings Asset 생성 PASS. RESOURCES 적용등록. raw/25/T13-import·T13-hashes.txt |
| B25-T14 | REVIEW | 12방 표시 Sprite자식/윗면/벽/위험/문/조각·캐릭터/적·배경0.3패럴랙스 적용. Collider직렬화/위치/크기 불변, Scene검사원문 T10/T14 완전동일 FAIL0. 실제카메라24장 Error0·플레이어.7×1.6/적콜라이더불변. 감각수락대기. raw/25/T14-apply·T14-clearance·T14-geometry-comparison.txt |
| B25-T15 | DONE | 12방 적용·Play-V5-Graybox.cmd/별도slot·같은AB Scene의표시만전환. Start-V5 B25/AB 연결·PowerShell구문0·최종Graybox launcher exit0/Error0. raw/25/T17-smoke-final/Graybox.txt |
| B25-T16 | DONE | 실제 게임카메라12방×2 PNG24장(1280×720)·B03전체768×1152·목업/실제 나란히1장·24장컨택트시트 생성/시각확인. 접촉fixture/physics꺼짐·HUD제외로 경로증명과 구분. raw/25/art·T16-art-capture·T16-B03-overview Error0 |
| B25-T17 | DONE | 최종Development errors0/warnings0·전체REGRESSION13/13·smoke5종exit0/Error0·B3 cycle/phase/사망복귀PASS. CP-C04 개발fixture누락은 opt-in등록으로 해소, V4Scene변경0. raw/25/T17-build-final·T17-smoke-final·T17-B3-*-final·raw/regression/B25-delivery-final |
| B25-T18 | DONE | README/TASKS0·3·9·새재개종료요약에 현재B25/Graybox·1회30~40분검수·조정Asset·실제실패/미실시 구분. 문서링크0오류, Batch Card/미커밋검사 결과는 요약 맨 위 |
| B25-Q1 | DONE | 기존AV1 직접클립 재사용·Assets/Audio/B25/Feedback.asset 단일볼륨·AttackFeedback 확장. swing/hit/death/hurt 각재생1, SourcePlaying=True·입자burst3/lifetime0.180s/소멸·Error0. API이벤트fixture로 자연전투증거와구분. raw/25/Q1-events |
| B25-Q2 | DONE | 실제조각접촉·레버공격·증기On·보스문잠금/열림 각재생1/Error0, 볼륨/클립공유Asset. raw/25/Q2-events. 즉시처치API는Audio이벤트fixture이며 자연전투증거아님 |
| B25-Q3 | DONE | B1/B2 등장1.800/1.817s 안 minZoom0.9000→1.0000 복원, 처치정지요청0.100s 실측0.1168/0.1167s(60fps). B3 FX컴포넌트없음/줌1 PASS/Error0. 실제획득·트리거+처치API연출fixture, 자연승리와구분. raw/25/Q3-measure |
| B25-Q4 | DONE | E1 접촉예측0.280/실접촉0.267s·E2 예고0.280/실돌진0.283s·E3 예고0.283/실발사0.283s PASS/Error0. 공유Asset lead0.3/brightness1.8, AI·공격시간 변경0. raw/25/Q4-measure |
| B25-Q5 | DONE | Run-RoomRoutes 실제Development exe12방 전부시도, PASS6/FAIL6·Error0. 합격기준의 실패원문보존 충족이며 12/12통과 아님. A01/A02/A04/B01/B03/B05통과; A03/A05 접근 timeout·A06 경로이탈·B02 긴도약·B04 접근·B06포고 실패. raw/25/Q5-final-routes, 추가재시도/좌표수정0 |
| B25-Q6 | DONE | Tools/Export-RoomTimes.py·README. 실제 Playtest CSV8방문/4방 출력, 활성95.926496s(과거 샘플). 일시정지 제외·재방문·마지막방·비정상종료·누락출구·잘못된 시간·덮어쓰기 거절 확인. raw/25/Q6-checks.txt·Q6-human-room-times.csv. 신규 B25 플레이 측정 아님 |

### 이전 OPS-20261025 결과

Base `bf9693a`(B20). 기존 미푸시 2커밋 보존, push·squash·게임 태그 변경 없음.

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| OPS-T1 | DONE | TASKS 원문 완전 보관·과거 미종결18행 유지·관련7문서 링크0불량·skill frontmatter/참조 확인. 기본 검사기 PyYAML 부재로 직접 구조 검사 대체 |
| OPS-T2 | DONE | 완료·재시도·시간/상한·보호·Queue·증거 규칙 통일, V5 대상/판정·템플릿 명시. 게임 변경0 |
| OPS-T3 | DONE | Preflight 구문오류0·모의8경로 PASS·실제 Unity 미실시·보호387 SHA변경0. 카드 범위 밖0·깨진 링크0·없는 런처0 |

## 1-1. 미해결 목록

B20-T4 REVIEW: [크기 비교](Validation/B20-bodysize.md), D-BODY 사용자 수락 대기. B21~B24 표는 [보관 기록](Archive/TASKS_ARCHIVE.md#doc-01-보관--b21b24-current-tasks)으로 옮겼고, 미해결 행과 이전 B12~B19의 기록상 미종결 행은 아래 표 하나에 유지한다. 후속 Batch에서 해소·대체된 역사적 상태도 있으므로 현재 버그/차단 수에 합산하지 않는다. 원문은 [B12~B19](Archive/TASKS-B12-B19.md), 전체는 [B20 종료 스냅샷](Archive/TASKS-B20.md). 이번 정리는 상태를 재판정하지 않는다.

| Task | 상태 | 당시 결과·남은 확인 |
| --- | --- | --- |
| B24-T3 | PARTIAL | 연속 승강기→A04 포고→B1 처치13.017s HP3→대시→도랑→A03 건물/W옥상 PASS. 반환 승강기 surface13/feet1로 재탑승 실패·Continue 미실시. raw/24/T6-revisit2-reboard·P5-loop-exe |
| B24-T6 | BLOCKED | 반환 승강기 탑승이2회 실패, T3 후반/Continue가 선행이라 A01 왕복은 실행 전 중단. 실패한 재탑승 점프 실험 제거·원문 보존. P5 실제 exe도 같은 surface13/feet1.015 실패, 재시도 종료. raw/24/T6-revisit1·2·P5-loop-exe |
| B24-T11 | REVIEW | 후보HP26·피해1·실제 패턴2,3,7만 순서 관측·페이즈전환0. 실높이2.800·양방향·첫예고1.000s 확인. 양벽 ClimbableWall 시험장, 감각 수락 대기. raw/24/T12-patterns3-physics |
| B24-T12 | PARTIAL | 바닥 무벽점프10/10피격·실입력24.767s 처치 HP2/벽점프7/포고26·사망복귀/능력유지·ID저장/재진입없음 Error0. 획득대 일반점프 회피 예외 발견, D-B24-B2-GATE 대기. raw/24/T12-patterns5-wave-lifecycle·T12-fight2-chargeInput |
| B23-T1 | REVIEW | 등장1.800s·피해0·이동만 허용·B3 패턴/페이즈/엔딩 PASS. B1 감각 수락 대기 |
| B23-T3 | REVIEW | getUp0.450s·추가 피해0·총복귀0.650s·HP0 기존 사망 PASS. 기존3값 유지. 일어나기 감각 수락 대기 |
| B23-T5 | PARTIAL | 새 규칙·승강기 양방향 self-test PASS. 첫 고리 접근19→0, A06 FAIL3 잔존. 원문의20건은1.1u 오름 포함으로 본문≥1.5와 충돌. raw/23/T5-comparison.txt |
| B23-T6 | PARTIAL | 6 Scene·요소 수 일치. Scene A02 overhead FAIL4·정적 A06 FAIL3. 좌표 변경 금지로 보존, raw/23/T6-* |
| B23-T7 | PARTIAL | 연습30회 각0/5·5/5 PASS. A01/A02 접근0, A04 두 번째접근1/5 접촉·4/5착지. 무너짐3·가시복귀2방·높은조각 PASS. raw/23/T7-field2/practice2 |
| B23-T8 | BLOCKED | 자동 경로 재시도 종료·실패한 전투 입력 보완만 T7 소스로 복원. 승강기·A04 진입 PASS, 일반 적 피해/무너짐 착지에서 중단. 새 전체 고리·그 뒤 Continue 미통과. 구성요소 PASS와 구분 |
| B23-T9 | PARTIAL | README·질문2/기록4칸·새 빌드 런처 smoke2 PASS. T8 미완주로 안내에 한계 명시 |
| B22-T5 | REVIEW | V5 인자 없이 HazardReturn·안전바닥/복귀 실측 PASS. V4 기본 hazard=False·Error0, 기존 DamageBlock 변경0 |
| B22-T8 | REVIEW | 실제 연속 고리46.731s·보스전8.618s·일반 적 포함·대시 획득·3연습·Continue PASS·Error0. 자연 사용자 고리 수락 대기 |
| B21-T5 | REVIEW | 기본 꺼짐·A05 통과0/5/동쪽복귀5/5·도랑폭24회·Quiet13/13. A04 섬 접지 PASS(포고3·복귀2). 후보 채택 대기. [근거](Validation/B21-hazard-return.md) |
| B21-T6 | REVIEW | 0.25u 모서리 후보·꺼짐 B20 최대값 재현(짧은1회편차 보존)·접촉33→10/36·벽점프/포고/대시각10·몸1.3/1.1 실제보정·Quiet13/13. 종료 기본13/13. [근거](Validation/B21-corner.md) |
| B21-BL2 | REVIEW | 기본꺼짐 별도토글·PlayerDash 후보. 꺼짐 T6개별값동일·켜짐0.2u턱10/10·Quiet13/13. [근거](Validation/B21-dash-corner.md) |
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
| D-BODY | [BodySizeTest](Launchers/current/Play-BodySize.cmd)와 V5에서 비교 후 높이1.6 유지 / 1.3 / 1.1 중 선택 | 1.3을 비교 우선 후보로 추천. 키·폭이 약19% 줄고 기본보다 공격 사거리가19% 줄어드는 절충. 채택은 직접 플레이 후 판단 | 기본 크기 반영 다음 배치·B20-T4 REVIEW. [측정](Validation/B20-bodysize.md). 현재 런처는 Builds/B25-delivery, B20 슬롯12개는 B21 raw에 보존 |
| D-CORNER-FIX | [머리 후보](Launchers/current/Play-V5-CornerFix.cmd) 0.25u 머리만 / 머리+대시 / 미채택(허용값 재비교 가능) | 0.25u 직접 비교 추천. 머리 접촉33→10/36·작은 몸에서도 실제 보정. 넓은 천장은 계속 막힘 | B21-T6·BL2 REVIEW와 기본 적용 다음 배치. [머리](Validation/B21-corner.md)·[대시](Validation/B21-dash-corner.md) |
| D-B23-RULE | 접근 규칙 본문(오름≥1.5)과 참고 스크립트·20건 기대가 불일치. 엄격 본문 유지 / 낮은 오름 포함 선택. A06 기존 위반3건은 draft5로 해소 | 본문 유지 추천. 첫 고리19→0, 기대20과 차이는 오름1.1u의 포함 여부 | B23-T5/T6 전체 FAIL0 및 기대20건 |



해소한 문서 충돌(B25 재개): `rooms_v5_AB.json`의 A02-B05에는 이전 `stub:true`가 남았지만 WORLD18·world_v5_AB와 B25-T4는 양방향 연결을 명시한다. 사용자 승인 B25 연결 지시를 우선해 생성기/Scene에서 해당 닫힘만 제거하고 WallJump gate를 유지했다. Design 원본 수정0, T10-A02-link 근거.

결정 대기3건. D-B23-GEOMETRY·D-B24-B2-GATE는 B25 사전 결정으로 설계안 선택 완료(2-1절). 제품 반영·검증은 이번 T2/T10에서 진행했고, 감각 수락과 기존 B24 당시 PARTIAL 이력은 유지한다. draft4.1·V5 HazardReturn 기본 켜짐은 B22 승인 상태다. [기존 근거 장부](Validation/B19-rules-ledger.md), [B20 추가](Validation/B20-rules-ledger-add.md).

## 2-1. 결정 기록

| ID | 결정 | 날짜·출처 |
| --- | --- | --- |
| D-B25-EXEC | B25 P1~P5·Queue 실행. `Design/art_v1/`도 선행 커밋에 포함 | 2026-11-02 사용자 직접 “읽고 실행”, 추가 폴더 확인 뒤 “어 진행해줘” |
| D-FLOOR-B6 | 층 B 6방 draft6, AB JSON을 새 생성 입력으로 사용 | 사용자 2026-11-01 결정의 Claude 전달, [B25](Design/proposals-1101-B25/01_CODEX_BATCH25.md) 실행 승인 |
| D-ART-1 | 그래픽 1차. 직접 제작 원본 복사·16 PPU/Point/무압축, 외부 다운로드 없음 | 사용자 결정의 Claude 전달(11-01 적용·11-02 직접 제작), [그래픽](Design/proposals-1101-B25/02_GRAPHICS.md)·이번 실행 승인 |
| D-B24-B2-GATE | 전투 시작 시 획득대를 바닥으로 내림(`sink_on_fight`). 설계안 선택 완료, B25 T2 구현·기술검증 PASS/감각수락 별도 | 2026-11-01 Claude 추천; 2026-11-02 사용자 B25 실행 범위 승인. 플레이 수락은 별도 |
| D-B23-GEOMETRY | A02 조각길 발판 삭제·선반 이동, 벽 점프 재방문 경로. 설계안 선택 완료, B25 T5/T10 재생성·재방문검증 PASS | 2026-11-01 Claude 추천; 2026-11-02 사용자 B25 실행 범위 승인 |
| D-A03-B01 | A03 서쪽 벽 대시 문을 선반 끝 천장 구멍으로 변경 | 2026-11-01 Claude world 겹침 정정; 2026-11-02 사용자 B25 실행 범위 승인 |

추천안의 실행 승인은 제품 검증·감각 수락을 의미하지 않는다. 선행 설계·아트 커밋 `e6b5172`는 원본 내용 변경 없이 보존했다.

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

- [Play-V5-Loop](Launchers/current/Play-V5-Loop.cmd): 기존 대기 순서 다음에 B25 검수 **1회30~40분**. A03→승강기→A04 B1→A05 대시→A03 연습/선반→천장 구멍 B01→지붕→B02→B03 증기→B04 보스2→벽 점프 재방문(B06·B05·A02). 자동 A 고리65초/Continue와 B04 실제승리 통과, 전체 AB 자동완주는 미확인이다. B02 E3·B06 포고·B05 서쪽 조각에서 막히면 위치/입력 기록 후 다음 구간으로 간다. 아트 가독성·효과음/입자·적 예고0.3s·보스 등장 줌을 함께 확인한다.
- [Play-BossTest-B2](Launchers/current/Play-BossTest-B2.cmd), `Assets/Scenes/Test/BossTest_B2.unity`: 같은 검수에서 필요할 때5~10분. 중앙 벽 점프 획득→3u이탈→잠금/1.8s 등장→양벽 벽 점프. 전투 때 받침은 바닥으로 내려가며 일반점프 회피0/10 기술검증 완료. 사망/능력유지/중앙재시작 확인. [보스1 단독](Launchers/current/Play-BossTest-B1.cmd)은 선택3~5분.
- [Play-V5-Graybox](Launchers/current/Play-V5-Graybox.cmd): 같은 AB 배치의 원래 도형 표시와 비교3~5분. 별도 슬롯, 충돌체 불변. 바닥/발판/위험/적이 읽히는지 확인하며 아트의 기본 채택 감각 수락은 T14 REVIEW다.

기록: 전체 시간____ / 보스 시도 횟수____ / 막힌 방·위치____ / 머리 박은 곳____ / 그래픽이 헷갈린 곳____ / 소리·예고 느낌____. 기존 B21/B22 복귀·B23 등장·B24 고리/조각 수락을 이 플레이에서 함께 확인하되 과거 상태를 자동 DONE으로 바꾸지 않는다.

조정 값 위치와 Play 종료 후 반영은 [LEVEL_V4](LEVEL_V4.md). 기본 보스(B3, D-B3 10/14)는 [기본 BossTuning](Assets/ScriptableObjects/BossTuning.asset)에서 조정하고(Boss-B3 런처는 [후보 Asset](Assets/BossCandidates/B3/BossTuning.asset)을 쓴다) Play 종료 후 원하는 값을 명시 저장·재빌드한다.

## 4. 열린 버그

| ID | 증상 | 근거 |
|---|---|---|
| B07-V2-BOSS-REPLAY / B09-REGRESSION | 새 빌드 V2 고정 입력이 W22 보스에서 실제 사망(tick285, 진입편차.082u), 이전 B06b 빌드는 PASS. 원인 미확정 | [B07-T25](Validation/B07-T25-summary.md), [B09-T11](Validation/B09-T11-summary.md) |
| B07-A02-TRAVERSE | A02 필수 계단·재방문 굴뚝: 자동 입력 미완주, 사람 확인 필요 | [B07-R-A02](Validation/B07-R-A02-summary.md) |
| B07-C03-OPTIONAL | C03 첫 선택 포고 도전: 자동 입력 미완주, 사람 확인 필요 | [B07-R-C03](Validation/B07-R-C03-summary.md) |
| B25-ROUTES | B02 바닥 포고/위길 E3·B06 포고·B05 서쪽조각 자동입력 미완주. 지형/능력값 변경 없이 재시도 종료. 사람도 막히면 방/위치/입력 기록→설계 검토 필요 | [T7](Validation/_raw/25/T7-B02-running/result.txt)·[T8](Validation/_raw/25/T8-B06-airdash/result.txt)·[T12](Validation/_raw/25/T12-expanded1/result.txt), T10-cover 원문 raw/25 |
| B25-Q5-ROUTES | 독립12방 묶음 PASS6/FAIL6. A03/A05 접근 timeout, A06 경로이탈, B02 긴도약, B04 접근, B06 포고. 자동입력의 한계와 제품 결함을 구분, 전체12방 완주 증명 아님 | [12방 결과](Validation/_raw/25/Q5-final-routes/routes.csv) |
| B25-MAP-CAPTURE | AB12/방문전후/문 표시 로직은 PASS. Editor 캡처미생성, exe screen capture 실패 Error2, Computer Use 앱 접근승인시간초과. 실제 지도 이미지 잔여 | [원문](Validation/_raw/25/T6-map-exe/result.txt) |
| V5-HEADBUMP | 사용자 점프 머리 걸림 피드백. h1.6 머리 충돌 후보36면·격리 실측33/36 접촉, 모서리 수평 보정 없음. 불일치3과 초기 문 전환2도 보존 | [플레이 CSV](Playtest/20261022-233114.csv), [B20 T3](Validation/B20-headbump.md), [전체 실측](Validation/B20-headbump-all.md) |

B16-GENERATOR·B12-CHASE-VERIFY·B18-CLEARANCE·B18-STATIC의 해소 상태는 유지한다. 이전 실패 기록은 기존 Validation에 보존했다. 열린 버그7건, 결정 대기3건이다. B24-RETURN-LIFT는 B25 T1 왕복3회/다른층호출 및 T12 실제 A 고리/Continue 통과로 해소했다. 이전 실패 원문은 유지한다. V5-A05-TRENCH는 B22 기본 복귀와 무능력0/10·대시10/10 실측으로 기술적으로 해소했고 최종 감각 수락은 T5/T8 REVIEW다([B22](Validation/B22-summary.md)). V5-A05-SHORTCUT은 B21 T1/T2에서 실제 잠금/방향/저장/14문을 확인해 해결했다([근거](Validation/B21-shortcut.md)). 정적 후보와 실제 진행 불가 판정은 구분한다.

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

## 9. Latest Handoff — Batch25 재개 종료

### 결정 질문지

기존3건은 순서 유지: D-BODY **1.6 유지/1.3/1.1**(추천1.3 비교, 화면 점유·공격거리 절충; B20-T4), D-CORNER-FIX **머리0.25/머리+대시/미채택**(추천 머리0.25 직접 비교; B21-T6/BL2), D-B23-RULE **엄격 오름≥1.5/낮은 오름 포함**(추천 본문 유지; B23-T5/T6). 링크·근거는 2절. 새 기본값 채택은 하지 않았다.

### 직접 플레이 확인

기존 V4→몸 비교→모서리 후보→V5 복귀/고리 순서를 3절에 유지했다. 이번 확인은 [Loop](Launchers/current/Play-V5-Loop.cmd)에서 **1회30~40분**: A03→B1→대시→반환승강기/연습→B01→B02→B03→B04 B2→벽 점프 B06·B05·A02, [Graybox](Launchers/current/Play-V5-Graybox.cmd) 비교. B2 시험장은 필요할 때 선택한다. 기본1.6, 복귀·보스 등장·조각 동기·아트/소리 가독성을 함께 본다. 막힌 방·위치/입력, 머리 접촉, 보스 시도/전체 시간을 기록한다.

A 고리/Continue·B04 실제전투는 기술 통과. B02 바닥/상부E3·B06 포고·B05 서쪽조각과 전체AB 자동완주는 남았다. 좌표나 기본값을 바꿔 통과 처리하지 않았다. 지도 방문전후 로직은 통과했지만 실제 화면 캡처는 남았다.

### Task별 결과

Claude 설계 변경: `e6b5172`, Design/v5 draft6 AB·Design/art_v1·B25 지시문/D60. 사용자 추가 폴더 승인 후 원문 선행 커밋, 이후 Design 변경0. 최초 exit198 종료는 [이전 요약](Validation/B25-summary.md), 실제 재개 증거는 [재개 요약](Validation/B25-resume-summary.md). 각 Task 상세는 1-0절.

| Task·상태 | 커밋 | 주요 변경·검증 |
| --- | --- | --- |
| T0/T3/Q6 DONE | b52f24f / 8968b29 / d9d782d | 사전결정·증기 정적연결·방별CSV. 최초차단 때 완료한 독립작업 재사용 |
| T1/T2 DONE | bc97de0 / 2193b67 | LeverLift 접촉skin·B2 sink. 왕복3회/V4회귀·바닥10/10/회피0/10 |
| T4/T5 DONE | 2679939 / 4b1aae9 | B25WorldAuthoring·AB12 Scene/요소일치. 정적FAIL0/WARN47·SceneFAIL0/WARN0 |
| T6 PARTIAL | 466e285 | AB 지도12방/18문·방문전후 PASS, 캡처실패/앱접근시간초과 |
| T7/T8 PARTIAL | 3b4bfe5 / 0c0fda4 | A03/B01/B03 각5/5·B02위5/5, B02바닥/B06포고 실패 원문 |
| T9 DONE | 4de750a | B04 획득/사망CP-B01/우측재시도·실전32.350s HP1/벽점프7/포고26·저장 |
| T10 PARTIAL | 039c24f | B01탑·A02왕복/조각·B03굴뚝 각3/3. stale stub해소, B05서쪽조각3접근실패 |
| T11 DONE / T12 PARTIAL | f747a64 / 4e87cab | 실제4접촉 maxHP5/저장Continue, A고리65.207s/Continue PASS·B02 E3 확장실패 |
| T13 DONE / T14 REVIEW | c996a5a / 18cd4aa | 원본18장 import16PPU·표시층/패럴랙스. Collider불변·Scene원문동일. 감각수락 별도 |
| T15/T16 DONE | 19165df / 28a3e57 | Graybox 최종smoke0, 실제24장/목업비교 raw/25/art. 촬영fixture는 경로증명과구분 |
| Q1/Q2 DONE | 23275ff / feef556 | B25 공유Audio Asset·타격/장치별재생·입자0.18s, Error0 |
| Q3/Q4 DONE | b0a7485 / 66c4918 | 줌0.9/1.8s·정지0.1s·B3영향없음, E1/E2/E3 실예고0.267/0.283/0.283s |
| Q5 DONE | 2925805 | Tools/Run-RoomRoutes 12방 시도 PASS6/FAIL6 원문보존 기준충족, 12/12완주 아님 |
| T17/T18 DONE | T17 c9274c8 / T18은 git log 참조 | 최종 errors0/warnings0·전체13/13·런처5종Error0·B3 PASS. README/TASKS·새종료요약/Batch Card |

수치 조정은 `Assets/Audio/B25/Feedback.asset`, `Assets/Art/Original/v1/ArtSettings.asset`, 배치는 `Assets/Scenes/V5`. Play 중 변경은 기록한 뒤 Stop 후 원본에 저장·재빌드한다. B1/기본 튜닝과 보호134경로 변경0. B25/직전B24만 보존, B23 슬롯/목록은 raw/25/T17-housekeeping에 보존했다.

### 다음 Batch 후보

사용자 플레이 결과에 따라 B02/B06/B05 미완주 경로 설계 검토·지도 이미지 확보·그래픽/효과 수락 정리. 이전 A01 왕복 미측정도 남아 있다. 이 목록은 승인 Queue가 아니며 현재 Ready Queue0/Batch25 Queue6개 소진이다. Sprint Gate·기본 몸/모서리 후보 채택은 사용자 판단이다.

Git: 이번 재개 전 결과 fb63b98이 현재 로컬 origin/main 및 기존 태그 B25다. 게시 이력 보호를 위해 squash/태그이동 없이 재개 Task커밋을 보존하고 B25-resume 태그를 사용한다. fetch/push는 하지 않는다. Archive 이동은 허용 밖이라 이전 표를 TASKS에 보존했다.

## 10. Playtest·Before-After

| 날짜 | 방·대상 | 사용자 피드백 / 변경 전→후 | 근거 |
| --- | --- | --- | --- |
| 2026-10-22 | A03→A05·A05 도랑·점프 | 지름길 우회·무능력 도랑 통과, 점프 불쾌 원인 ①크기 ③머리 충돌. B20은 측정·비교 도구 추가까지; 기본 설계 수정·개선 수락은 아직 없음 | [사용자 로그](Playtest/20261022-233114.csv), [전달 지시](Batches/BATCH-20.md), [측정 결과](Validation/B20-summary.md) |

| 2026-10-31 | A03~A05 고리 | 보스/처치 후 구멍 좋음·대시 익음. 보스 등장 딜레이·HP 증가 방향·복귀 일어나기 요구. A03 위 마을→S0 머리 충돌 불쾌, 맵 빈 느낌·얕은 구덩이 기대·계단 늘림 느낌. 시간·시도 수 미기록. B23 변경 후 결과는 미확인 | [Claude 전달 플레이 기록](Design/proposals-1031/00_README.md) |

| 2026-11-01 | B23 A03 허브 | 사용자 “A03 전부 사라졌는데 비었어”, 분량 추가 요청. Claude는 허브 평지화를 과잉 해석으로 기록, 건물 거리 draft4.3/5 작성. B24 반영 후 플레이 수락은 미실시 | [전달 기록](Design/proposals-1101/00_README.md) |

B24 Ruling: PRD77의 과거 DEC-LEVEL-001은 당시 세션 한정 발견 승인 기록으로 보존한다. 2026-11-01 사용자 D-FRAGMENT-HP 직접 승인과 지정3곳 패치가 이번 영구 조각/HP 구현의 근거이며, 승인 밖인77행은 바꾸지 않는다.
