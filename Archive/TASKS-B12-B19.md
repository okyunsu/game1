# B12~B19 Task 기록

2026-10-25 이동. 과거 상태를 보존하며 DONE/REVIEW 등 재판정 없음.

## 1-0d. Current Tasks (Batch19)

Base `8a9a1b5`, 재개 2026-10-21 23:46 KST. 시작 검사 exit0 원문: `Validation/_raw/B19/startup-20261021-220750/startup.log`. 이전 실패 기록은 보존한다. 사용자의 재개 지시에 따라 기존 `Validation/B19-blocked.md`는 이번 실행의 선행 실패 기록으로 식별하고 유지한다. Unity·Unity 호출 스크립트는 샌드박스 밖에서 실행한다. Git 기록 명령도 사용자 추가 승인 예외다.

| Task | 상태 | 현재 결과 |
|---|---|---|
| B19-T1 | DONE | draft3 머리 위0/문6/측면43/구덩이20, draft2 머리 위4·양옆 틈 양쪽0. Quiet13/13 |
| B19-T2 | DONE | draft3 FAIL0/WARN0·MEASURE69, draft2 머리 위4/적1 검출·틈0.5 WARN·임시Scene 삭제·Quiet13/13 |
| B19-T3 | DONE | 능력 계산/무능력 계산 분리, 정적22→0·잔여0/새FAIL0·fixture4 검출·근거 없는8규칙 WARN·Quiet13/13 |
| B19-T4 | DONE | 실제 exe7200프레임·120초: 목표 일치/입력 타이밍 원인, platform2 옆면 뒤 낙하. DJ상승4.884u·Error0·Quiet13/13 |
| B19-T5 | DONE | 규칙 근거/명시 유지 예외·인계·링크0·보호380파일 SHA0변경·결정0·Quiet13/13 |
| B19-T6 | DONE | Quiet13/13·종료 기본13/13·V5 런처3 exit0/Error0·보호380 SHA0변경·범위 밖0/링크0·카드 PASS |

## 1-0c. Current Tasks (Batch18)

| Task | 상태 | 현재 결과 |
|---|---|---|
| B18-T1 | DONE | draft2 A02 gap0.5/head1.5·solid14 안 적. V4/V5 발판 두께0.5·Effector 없음. 제품 코드/Scene 변경0·Quiet13/13 |
| B18-T2 | DONE | A02 두 원인 FAIL 검출. Scene 샘플·적·순찰 검사, Editor/Development opt-in·Quiet13/13 |
| B18-T3 | REVIEW | draft3 Scene6·작은 발판12·적15·구덩이6·요소 일치·문14/stub/일방/저장 PASS·Quiet13/13 |
| B18-T4 | PARTIAL | A02 두 원인 해소·적0/순찰끝0, 전체 후보76→69. 모두 측면 겹침, overhead-only0. 후보0 미충족·Quiet13/13 |
| B18-T5 | PARTIAL | 추가 규칙FAIL0/WARN0·위반JSON4개 검출. 기존 도달성20/단차2 FAIL22 보존, 전체FAIL0 미충족·Quiet13/13 |
| B18-T6 | REVIEW | Development 빌드·V5+NoMap6모드 각10초Error0. B18/B12 보존·V4 런처 변경0·Quiet13/13 |
| B18-T7 | DONE | 현재 상태·결정1·직접 플레이·검사·값 반영 안내, 깨진 링크0·Quiet13/13 |
| B18-T8 | DONE | Quiet13/13·기본13/13·18런처Error0·Batch-Card 범위/링크/없는 런처0·Quiet13/13 |
| B18-BL1 | DONE | 능력 없이 자연 A01→A02→A03→A04→A05 대시까지5구간 PASS(합29.50초), 적 포고7·HP4→2. A03 동쪽3발판/A04 상부 통과·Quiet13/13 |
| B18-BL2 | DONE | draft2/draft3 방별 등불·작은 발판·블록·머리 여유·가로 간격 계산·합계 일치·Quiet13/13 |
| B18-BL3 | DONE | 오름2u 최대 간격 제자리3.25/달리기3.75u, 포고1회 몸 상승2.128u. 오름5종×2/포고2방식 실제 측정·Quiet13/13 |
| B18-BL4 | DONE | DJ 3PASS/1BLOCKED 측정: A01→A06 x24.95/발4.39·접촉없음·120초 한계. 독립A06 왼쪽 조각/일방 문/A02 조각 PASS, 자연 전체 완주 아님·Quiet13/13 |

## 1-0b. Current Tasks (Batch17)

| Task | 상태 | 현재 결과 |
|---|---|---|
| B17-T1 | DONE | 기존 Scene에서 A02 지형 충돌·A05 높이10/Confiner 고정 측정, 제품 코드/Scene 변경0, Quiet13/13 |
| B17-T2 | REVIEW | draft2 6방·요소 일치·구덩이6·도랑 바닥 수정 반영·보호 SHA0변경, Quiet13/13 |
| B17-T3 | DONE | A02 왼쪽 복귀·A05 추적·6구덩이 탈출/점프·문14·stub/일방·저장 PASS, Quiet13/13 |
| B17-T4 | DONE | 추가 규칙 위반0·위반 fixture5개 검출, 기존 정적 후보22FAIL은 보존, Quiet13/13 |
| B17-T5 | REVIEW | B17 Development·V5 3런처10초Error0·최신+직전2개, Quiet13/13 |
| B17-T6 | DONE | 피드백 원인·재플레이·빌드 인계 갱신, 링크0불량·결정0·Quiet13/13 |
| B17-T7 | BLOCKED | 앞선 Quiet13/13·18런처Error0·카드0문제는 유효. 종료 기본 회귀는 Editor 라이선스 exit198로 실행 전 중단 |
| B17-BL1 | DONE | 자연 A01→A03·독립A05대시2.10초, A03/A04 각120초BLOCKED 측정 완료; 입력 개선 재시도는 라이선스 중단 |
| B17-BL2 | DONE | 면적4548→4648·적13→15·등불13→14·머리 여유1→2.4u, 합계 일치; WORLD 4절 이전 표기는 Claude 정리 |

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

