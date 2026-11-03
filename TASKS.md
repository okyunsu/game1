# 개발 작업과 기록

## 0. 지금

- 갱신: 2026-11-03 Codex — 승인 Batch26 진행 중. Base `b845145`, Claude 설계 시작 커밋 `e2bc7d1`.
- Unity Preflight PASS: Hub licensing=True, exit0. fetch/push 없음, 기존33커밋 보존.
- REVIEW: 기존 QA 대상6 / 목록 밖 기존0(ID 없음, Current Tasks 기준) / 이번 Batch 신규0(진행 중).
- D-REVIEW-CLEANUP 21행 처리: 폐기10·수락3·미채택2·QA유지6. 상세 [T0](Validation/B26-T0-review-cleanup.md).
- draft6.1 AB12방 재생성 완료, 검사·아트·경로 진행 중. Codex 차례.
- 결정 대기2건(D-BODY·D-B23-RULE), 승인 Ready Queue4행은 B26 지시문 기준.

## 1. 현재 상태 / Current Sprint

- Sprint3: V5 draft6 층 A·B12방, 건물 거리·대시/벽 점프 재방문·조각 HP 성장·보스1/2·직접 제작 아트와 효과 구현. 사용자 감각 수락·Sprint Gate 대기.
- 반환 승강기는 B25 T1/T12에서 기술적으로 해소했고 A 고리/Continue를 확인했다. B02/B06/B05 자동 경로 잔여는 아래 PARTIAL·열린 버그를 따른다.
- [Batch25](Design/proposals-1101-B25/01_CODEX_BATCH25.md)는 사용자 2026-11-02 실행 승인, Hub 실행 후 재개. 최초 차단 증거와 B24/B23 당시 상태는 보존한다.

## 1-26. Current Tasks (Batch26)

[사용자 승인 지시문](Design/proposals-1103-B26/01_CODEX_BATCH26.md). 2026-11-03 사용자 직접 실행 승인. 10시간·정리40분·상한30. Claude 설계 변경: `e2bc7d1` Design/v5 draft6.1 및 제안 지시문, 원본 내용 변경 없음.

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| B26-T0 | DONE | 사전 결정5건 장부 기록·REVIEW21행 처리·목록밖0. Validation/B26-T0-review-cleanup.md |
| B26-T1 | DONE | draft6.1 AB12방 생성·요소수 일치, 정적 FAIL0·Scene FAIL0. raw/26/T1-* |
| B26-T2 | DONE | B02바닥5/5·위하강5/5(E3피격0)·B06조각5/5(피격각1)·B05기둥조각3/3, 머리0. Validation/B26-T2-routes.md |
| B26-T3 | DONE | 새 PNG12개 SHA 일치·배경320×360, Import manifest 기록. Assets 복사0 |
| B26-T4 | REVIEW | v1.1 28PNG import·종류별 스프라이트·meta 생성. Collider직렬화불변·T1/T4 Scene 원문동일. 감각 QA 대기 |
| B26-T5 | REVIEW | 배경22.5u 아래정렬·세로반복 제거·sky1·증기alpha0.25 상승입자. Collider불변·T1 Scene원문동일. T7 캡처 후 감각QA |
| B26-T6 | DONE | 갈색0.4×0.8 안내 SignPost. 12방 가이드 SpriteRenderer만 기본숨김·충돌/상호작용불변. raw/26/T6-guide-display |
| B26-T7 | DONE | 최종 카메라24 PNG·contact sheet·B03 목업 비교, raw/26/art/final·T7-art-verified Error0. physics꺼짐 fixture |
| B26-T8 | DONE | 기존실패6건 재시도: PASS5(A05/A06/B02/B04/B06)·FAIL1(A03 4u벽 지형). 입력/도구 수정, 설계수정0. Validation/B26-T8-route-ledger.md |
| B26-T9 | TODO | 승인 지시문 기준 진행 |
| B26-T10 | DONE | RenderTexture 지도 방문전/후 PNG2장 PASS, AB12/문18·Error0. 초기 도구 중복지도인자 수정, raw/26/T10-map-r2 |
| B26-T11 | TODO | 승인 지시문 기준 진행 |
| B26-T12 | TODO | 승인 지시문 기준 진행 |

## 1-DOC. Current Tasks (DOC-01)

[사용자 승인 지시·T0 시작 기록](Batches/BATCH-DOC-01.md), 문서 전용 / needs-unity=no / Base `f1a6f5a`.

| Task | 상태 | 현재 결과 |
| --- | --- | --- |
| DOC-01-T0 | DONE | 지정4경로 외 변경0·staged0, 시작 바이트 기록, Preflight -SkipUnity PASS/Unity 미실시 |
| DOC-01-T1 | DONE | D1~D6/D11 반영·기존 권한/보호/push 유지, 삭제·통합 문장별 남은 절은 DOC-01 요약 |
| DOC-01-T2 | DONE | 목표/시간·상한30/Hub 점검/Unity 없는 목록/A·B·C 링크·증거/REVIEW 규칙 정리 |
| DOC-01-T3 | DONE | B21~B24 원문 보관·미해결 단일 표, 상태 행 전후 동일·REVIEW 21(본문 B20 포함) |
| DOC-01-T4 | DONE | 기존 결정 ID 전부 보존·제안/날짜 미확인4행 구분, DOC-01 사용자 결정 장부·참조 갱신 |
| DOC-01-T5 | DEFERRED | 보호 문서가 참조해 이동 보류(사용자 결정 2026-11-03), 다음 GAME_DESIGN 수정 때 함께 처리. 이번 재개에서 이동 금지 |
| DOC-01-T6 | DONE | 지정 사본 비텍스트20000파일 정리·텍스트4925 SHA256/경로 보존, regression4635개 zip 수/해시 확인 후 삭제. raw2372.6→915.1MB |
| DOC-01-T7 | DONE | README Batch 진행·과거 검사 상태를 TASKS 링크로 통합. 실행/조작/런처 전부·최신 B25 직접 플레이 문단 원문 보존 |
| DOC-01-T8 | DONE | 직전 중간 인계 커밋·T6/T7 기록·최종 T0 바이트 비교·Batch Card·종료 커밋. 로컬 DOC-01 태그, push 없음 |

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
| B22-T8 | DEFERRED | B26 사용자 결정: V5로 대체. 실제 연속 고리46.731s·보스전8.618s·일반 적 포함·대시 획득·3연습·Continue PASS·Error0. 자연 사용자 고리 수락 대기 |
| B21-T5 | DEFERRED | B26 사용자 결정: V5로 대체. 기본 꺼짐·A05 통과0/5/동쪽복귀5/5·도랑폭24회·Quiet13/13. A04 섬 접지 PASS(포고3·복귀2). 후보 채택 대기. [근거](Validation/B21-hazard-return.md) |
| B21-T6 | DEFERRED | B26 사용자 결정: 모서리 보정 미채택 종료, 기본 꺼짐 유지. 0.25u 모서리 후보·꺼짐 B20 최대값 재현(짧은1회편차 보존)·접촉33→10/36·벽점프/포고/대시각10·몸1.3/1.1 실제보정·Quiet13/13. 종료 기본13/13. [근거](Validation/B21-corner.md) |
| B21-BL2 | DEFERRED | B26 사용자 결정: 모서리 보정 미채택 종료, 기본 꺼짐 유지. 기본꺼짐 별도토글·PlayerDash 후보. 꺼짐 T6개별값동일·켜짐0.2u턱10/10·Quiet13/13. [근거](Validation/B21-dash-corner.md) |
| B18-T3 | DEFERRED | B26 사용자 결정: V5로 대체. draft3 Scene6·작은 발판12·적15·구덩이6·요소 일치·문14/stub/일방/저장 PASS·Quiet13/13 |
| B18-T4 | PARTIAL | A02 두 원인 해소·적0/순찰끝0, 전체 후보76→69. 모두 측면 겹침, overhead-only0. 후보0 미충족·Quiet13/13 |
| B18-T5 | PARTIAL | 추가 규칙FAIL0/WARN0·위반JSON4개 검출. 기존 도달성20/단차2 FAIL22 보존, 전체FAIL0 미충족·Quiet13/13 |
| B18-T6 | DEFERRED | B26 사용자 결정: V5로 대체. Development 빌드·V5+NoMap6모드 각10초Error0. B18/B12 보존·V4 런처 변경0·Quiet13/13 |
| B17-T2 | DEFERRED | B26 사용자 결정: V5로 대체. draft2 6방·요소 일치·구덩이6·도랑 바닥 수정 반영·보호 SHA0변경, Quiet13/13 |
| B17-T5 | DEFERRED | B26 사용자 결정: V5로 대체. B17 Development·V5 3런처10초Error0·최신+직전2개, Quiet13/13 |
| B17-T7 | BLOCKED | 앞선 Quiet13/13·18런처Error0·카드0문제는 유효. 종료 기본 회귀는 Editor 라이선스 exit198로 실행 전 중단 |
| B16b-T2 | DEFERRED | B26 사용자 결정: V5로 대체. 수정본 Scene6개·요소 일치·A02조각2/A04·A06각1·Quiet13/13 |
| B16b-T5 | DEFERRED | B26 사용자 결정: V5로 대체. Development 전달·15런처+NoMap3 PASS·Release 제외 검증 |
| B16b-BL1 | DEFERRED | B26 사용자 결정: V5로 대체. 지도6방/12문·stub×4/화살표1·V4지도PASS·Quiet13/13 |
| B16-T1 | BLOCKED | 생성기 경로 문자열 컴파일 오류 1회 보정 후 재실패, 제품 코드 전체 원복. Quiet13/13 |
| B16-T4 | PARTIAL | 정적 방 규칙·가짜 문 안전/겹침/높이 위반 검출. 실제 gate·물리 도달성 미검증 |
| B16-T7 | PARTIAL | Quiet13/13·기본13/13, 기존12런처 Error0. 카드 범위밖/깨진링크/없는런처0. V5 전달 기준 미충족 |
| B15-T2 | DONE | B26 사용자 수락: 지도 자동 검증으로 충분. 방문방·위치·M3상태, 실제 exe·시간·입력맵 유지, Quiet13/13 |
| B15-T3 | DONE | B26 사용자 수락: 지도 자동 검증으로 충분. 문 약자·종류·미방문 ?, map-all/no-map/비V4 검사, Quiet13/13 |
| B15-T5 | DEFERRED | B26 사용자 결정: V5로 대체. 전달 Development·NoMap·3런처 10초 Error0, Quiet13/13 |
| B15-BL4 | DONE | B26 사용자 수락: 지도 자동 검증으로 충분. 보유 능력 문 열림색·CP, 저장/Continue·no-map/비V4·Quiet13/13 |
| B12-T3 | PARTIAL | 추적 연타·정지 피격·P2 회복 X·Quiet13/13; 회피 지형 간섭, Pause/복귀/10회/fps 일부 미검증 |

## 2. 결정 대기

| ID | 질문·선택지 | AI 추천·이유 | 풀리는 항목 |
|---|---|---|---|
| D-BODY | [BodySizeTest](Launchers/current/Play-BodySize.cmd)와 V5에서 비교 후 높이1.6 유지 / 1.3 / 1.1 중 선택 | 1.3을 비교 우선 후보로 추천. 키·폭이 약19% 줄고 기본보다 공격 사거리가19% 줄어드는 절충. 채택은 직접 플레이 후 판단 | 기본 크기 반영 다음 배치·B20-T4 REVIEW. [측정](Validation/B20-bodysize.md). 현재 런처는 Builds/B25-delivery, B20 슬롯12개는 B21 raw에 보존 |
| D-B26-A03 | A03 서쪽 입구→거리 M건물(16~22, 높이4u)을 어떻게 통과할지: 낮은 디딤 추가 / 거리 통로로 변경 | 낮은 디딤 설계를 Claude가 검토하도록 추천. 점프2.5u보다 높은 4u벽에서 실제입력 실패, 좌표 원본 수정은 사용자 권한 | B26-T8 A03 잔여, [원문](Validation/_raw/26/T8-A03-r1/result.txt) |
| D-B23-RULE | 접근 규칙 본문(오름≥1.5)과 참고 스크립트·20건 기대가 불일치. 엄격 본문 유지 / 낮은 오름 포함 선택. A06 기존 위반3건은 draft5로 해소 | 본문 유지 추천. 첫 고리19→0, 기대20과 차이는 오름1.1u의 포함 여부 | B23-T5/T6 전체 FAIL0 및 기대20건 |



해소한 문서 충돌(B25 재개): `rooms_v5_AB.json`의 A02-B05에는 이전 `stub:true`가 남았지만 WORLD18·world_v5_AB와 B25-T4는 양방향 연결을 명시한다. 사용자 승인 B25 연결 지시를 우선해 생성기/Scene에서 해당 닫힘만 제거하고 WallJump gate를 유지했다. Design 원본 수정0, T10-A02-link 근거.

결정 대기2건. DOC-01 옛 문서 이동은 사용자 2026-11-03 결정으로 보류하여 질문 목록에서 해소했다. D-B23-GEOMETRY·D-B24-B2-GATE는 B25 사전 결정으로 설계안 선택 완료(2-1절). 제품 반영·검증은 이번 T2/T10에서 진행했고, 감각 수락과 기존 B24 당시 PARTIAL 이력은 유지한다. draft4.1·V5 HazardReturn 기본 켜짐은 B22 승인 상태다. [기존 근거 장부](Validation/B19-rules-ledger.md), [B20 추가](Validation/B20-rules-ledger-add.md).

## 2-1. 결정 기록

[사용자 결정 장부 — DECISIONS.md](DECISIONS.md)

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

B16-GENERATOR·B12-CHASE-VERIFY·B18-CLEARANCE·B18-STATIC의 해소 상태는 유지한다. 이전 실패 기록은 기존 Validation에 보존했다. 열린 버그7건, 결정 대기2건이다. B24-RETURN-LIFT는 B25 T1 왕복3회/다른층호출 및 T12 실제 A 고리/Continue 통과로 해소했다. 이전 실패 원문은 유지한다. V5-A05-TRENCH는 B22 기본 복귀와 무능력0/10·대시10/10 실측으로 기술적으로 해소했고 최종 감각 수락은 T5/T8 REVIEW다([B22](Validation/B22-summary.md)). V5-A05-SHORTCUT은 B21 T1/T2에서 실제 잠금/방향/저장/14문을 확인해 해결했다([근거](Validation/B21-shortcut.md)). 정적 후보와 실제 진행 불가 판정은 구분한다.

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

## 9. Latest Handoff — DOC-01 종료

### 결정 질문지

새 결정 대기 없음. T5는 사용자 2026-11-03 결정으로 보류하고 다음 GAME_DESIGN 수정 때 함께 처리한다. 기존 D-BODY/D-CORNER-FIX/D-B23-RULE 3건은 2절에 유지한다.

### 직접 플레이 확인

문서 Batch의 새 플레이 항목은 없다. 기존 3절과 아래 B25 제품 인계의 순서·시간·항목을 유지한다. REVIEW21개이므로 다음 콘텐츠 Batch 전에 사용자 QA 1회 필요.

### Task별 결과

- T0~T4 DONE: d477cd7 / 7f16ce2 / e12dfc7 / 5a2acaa / 79e74c6. 규칙·템플릿·상태 행·결정 출처 보존 검증 PASS.
- T5 DEFERRED: 사용자 보류, 이동0. 직전 자동 승인 거부는 재시도하지 않음. 중간 인계 d7e96c8.
- T6 DONE: f6edb29. 지정 사본 비텍스트20000개 삭제, 텍스트4925개 경로/SHA256 일치. regression4635개를 zip 파일 수/해시 확인 후 삭제. raw1457.5MB 감소.
- T7 DONE: 8250ad9. README Batch 진행/과거 결과는 TASKS 링크, 런처27개 목록·순서/실행/조작/최신 직접 플레이 문단 원문 보존.
- T8 DONE: 종료 커밋은 `git log -1`·로컬 DOC-01 태그. 단일 요약에 최종 카드·T0 크기 비교·실패/미실시 기록. push·squash 없음.

원문·정리 전후·범위 확인은 [DOC-01-summary](Validation/DOC-01-summary.md). Unity 검증은 문서 전용이므로 실행하지 않았다. 기존 사용자 변경은 스테이징하지 않았다.

### 다음 Batch 후보

사용자 QA 후 기존 게임 잔여 항목을 별도 승인 Batch로 정한다. 루트 옛 문서 이동은 다음 GAME_DESIGN 수정 때 보호 링크와 함께 처리하며 이번에는 실행하지 않았다. Ready Queue0.

### 이전 B25 제품 인계 (수락·플레이 순서 유지)

#### 결정 질문지

기존3건은 순서 유지: D-BODY **1.6 유지/1.3/1.1**(추천1.3 비교, 화면 점유·공격거리 절충; B20-T4), D-CORNER-FIX **머리0.25/머리+대시/미채택**(추천 머리0.25 직접 비교; B21-T6/BL2), D-B23-RULE **엄격 오름≥1.5/낮은 오름 포함**(추천 본문 유지; B23-T5/T6). 링크·근거는 2절. 새 기본값 채택은 하지 않았다.

#### 직접 플레이 확인

기존 V4→몸 비교→모서리 후보→V5 복귀/고리 순서를 3절에 유지했다. 이번 확인은 [Loop](Launchers/current/Play-V5-Loop.cmd)에서 **1회30~40분**: A03→B1→대시→반환승강기/연습→B01→B02→B03→B04 B2→벽 점프 B06·B05·A02, [Graybox](Launchers/current/Play-V5-Graybox.cmd) 비교. B2 시험장은 필요할 때 선택한다. 기본1.6, 복귀·보스 등장·조각 동기·아트/소리 가독성을 함께 본다. 막힌 방·위치/입력, 머리 접촉, 보스 시도/전체 시간을 기록한다.

A 고리/Continue·B04 실제전투는 기술 통과. B02 바닥/상부E3·B06 포고·B05 서쪽조각과 전체AB 자동완주는 남았다. 좌표나 기본값을 바꿔 통과 처리하지 않았다. 지도 방문전후 로직은 통과했지만 실제 화면 캡처는 남았다.

#### Task별 결과

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

#### 다음 Batch 후보

사용자 플레이 결과에 따라 B02/B06/B05 미완주 경로 설계 검토·지도 이미지 확보·그래픽/효과 수락 정리. 이전 A01 왕복 미측정도 남아 있다. 이 목록은 승인 Queue가 아니며 현재 Ready Queue0/Batch25 Queue6개 소진이다. Sprint Gate·기본 몸/모서리 후보 채택은 사용자 판단이다.

Git: 이번 재개 전 결과 fb63b98이 현재 로컬 origin/main 및 기존 태그 B25다. 게시 이력 보호를 위해 squash/태그이동 없이 재개 Task커밋을 보존하고 B25-resume 태그를 사용한다. fetch/push는 하지 않는다. Archive 이동은 허용 밖이라 이전 표를 TASKS에 보존했다.

## 10. Playtest·Before-After

| 날짜 | 방·대상 | 사용자 피드백 / 변경 전→후 | 근거 |
| --- | --- | --- | --- |
| 2026-10-22 | A03→A05·A05 도랑·점프 | 지름길 우회·무능력 도랑 통과, 점프 불쾌 원인 ①크기 ③머리 충돌. B20은 측정·비교 도구 추가까지; 기본 설계 수정·개선 수락은 아직 없음 | [사용자 로그](Playtest/20261022-233114.csv), [전달 지시](Batches/BATCH-20.md), [측정 결과](Validation/B20-summary.md) |

| 2026-10-31 | A03~A05 고리 | 보스/처치 후 구멍 좋음·대시 익음. 보스 등장 딜레이·HP 증가 방향·복귀 일어나기 요구. A03 위 마을→S0 머리 충돌 불쾌, 맵 빈 느낌·얕은 구덩이 기대·계단 늘림 느낌. 시간·시도 수 미기록. B23 변경 후 결과는 미확인 | [Claude 전달 플레이 기록](Design/proposals-1031/00_README.md) |

| 2026-11-01 | B23 A03 허브 | 사용자 “A03 전부 사라졌는데 비었어”, 분량 추가 요청. Claude는 허브 평지화를 과잉 해석으로 기록, 건물 거리 draft4.3/5 작성. B24 반영 후 플레이 수락은 미실시 | [전달 기록](Design/proposals-1101/00_README.md) |

B24 Ruling: PRD77의 과거 DEC-LEVEL-001은 당시 세션 한정 발견 승인 기록으로 보존한다. 2026-11-01 사용자 D-FRAGMENT-HP 직접 승인과 지정3곳 패치가 이번 영구 조각/HP 구현의 근거이며, 승인 밖인77행은 바꾸지 않는다.
