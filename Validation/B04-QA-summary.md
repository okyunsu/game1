# 퇴근 후 확인 — 55분

확인 순서는 이전 수락 대기부터 따른다. `.cmd`를 더블클릭하면 그 폴더의 **QA 전용 저장 슬롯만 매번 초기화**한다. Menu에서 Z 이어하기. 같은 폴더의 Data/DLL을 함께 유지한다. 이동 ←→/↑↓, 점프 Z, 대시 C, 공격 X, 공중 ↓+X 포고, Esc Pause.

| 순서/시간 | 빌드·시작 | 비교할 것 |
|---|---|---|
| 1 · 5분 | [기본 C06](../Builds/B04-main-final/QA-C06.cmd) | 명중 히트스톱 .05s가 적절한가. 기본 2패턴 보스 유지. |
| 2 · 10분 | [A06](../Builds/B04-main-final/QA-A06.cmd) / [C05](../Builds/B04-main-final/QA-C05.cmd) | A04/C03 왼쪽 출구 위 문(x2,y5.6)에 점프 후 왼쪽. 발견→같은 입구 귀환, 각 방 사람 도전시간·흥미 확인. |
| 3 · 5분 | [B06](../Builds/B04-main-final/QA-B06.cmd) | CP-B01→오른쪽 B02→B03. x7 청록색 `↑ 선택 대시 도전` 앞에서 ↑. Dash/포고로 발견→귀환, 입구와 필수 경로 구분 확인. DJ 없음. |
| 4 · 10분 | [B2 C06](../Builds/B03-B2-final/QA-C06.cmd) | 근접→도약→파동→돌진. 예고·걷기·착지/접촉·DJ 돌진 회피·사망 재시도. B04 지정 돌진/실패/Pause/초기화 기술 PASS와 자연 전투 판단을 구분. |
| 5 · 10분 | [fast](../Builds/B03-B2-fast-final/QA-C06.cmd) / [slow](../Builds/B03-B2-slow/QA-C06.cmd) | 각5분. 예고 짧음+이동 빠름 / 예고 김+이동 느림 중 학습감·압박 비교. |
| 6 · 10분 | [AV1 시작](../Builds/B04-AV1-final-r4/QA-A01.cmd), [AV1 B구역](../Builds/B04-AV1-final-r4/QA-B06.cmd), [AV1 C06](../Builds/B04-AV1-final-r4/QA-C06.cmd) | 기본과 동일 방에서 눈/실루엣, 점프·착지·대시 잔상·피격·사망 조각, 사운드/동시 음량 비교. B02 전면 표식·B03 오른쪽 포탑 포신 확인. 무음에서도 보스 예고가 보이는가. A05/B04 능력 획득 소리는 자연 획득 때 확인. |
| 7 · 5분 | 아래 선택 기록 | 채택 여부와 Sprint Gate 판단 시점만 기록. 자연 전체 완주가 남았다면 Gate는 계속 대기. |

선택 기록: **AV1 사운드 / 형태 각각 채택·조정·폐기**, **보스 기본·B2·fast·slow 중 유지 후보**, **히트스톱 유지·조정**, **A06/B06/C05 수락·조정**, **Sprint Gate 오늘 자연 완주 후·다음 완주 후·현재 대기 유지**. 추천: AV1은 후보 유지 후 청감/가독성 조정, B2/변형은 직접 비교 후 선택, Gate는 자연 완주 후 판단. 기술 검증만으로 감각·Gate를 수락하지 않는다.

AV1 조정은 [PresentationProfile.asset](../Assets/Candidates/AV1/PresentationProfile.asset) Inspector의 clip/volume/pitch/throttle, bodyColors/hazardOpacity, formRatios, lean/stretch/squash/trail/fragment 값 한 곳에서 한다. 보스 후보는 `Assets/BossCandidates/{B2,B2-fast,B2-slow}/BossTuning.asset`, 히트스톱은 기존 FeedbackTuning.asset. Play에서 바꾼 값을 기록하고 종료 후 원하는 값인지 확인/다시 반영·저장·재빌드한다. 기본 채택은 별도 사용자 결정이다.

[9방 사진](B04-shots-summary.md) · [음량 표](B04-volume-summary.md) · [명도 표](B04-contrast-summary.md) · [돌진 증거](B04-C-summary.md). 사진은 방 카메라 렌더이며 HUD/화면 오버레이를 포함하지 않는다. 자연 전체 완주는 이 55분 비교와 별도로 남을 수 있다.
