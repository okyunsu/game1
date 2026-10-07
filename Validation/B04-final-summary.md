# Batch04 최종 검증

START_COMMIT `3ae8cc368377f9f8815bfd7ddac083565b813a42`, 2026-10-08 07:38 KST 시작 clean/Unity 닫힘. 예상2~4시간/최대8시간, 실제 약35분. 승인 트랙/Backlog 소진, 로컬 Task 커밋만 수행/push 없음. 사운드·형태·명도 후보는 REVIEW, B04-C/빌드·검증/음량표·사진·QA 안내는 DONE. 감각·기본 채택·Sprint Gate 미수락.

## 전달 빌드

| Build | 결과 | 실제 exe |
|---|---|---|
| 기본 `B04-main-final` | Succeeded errors=0 warnings=0, 18방+Menu/Ending/PogoTest=21씬 | presentation audio0/visual추가0, 기본2패턴 보스, RouteAll R01~R17→타이틀 PASS/Error0 |
| AV1 `B04-AV1-final-r4` | Succeeded errors=0 warnings=0, 동일21씬 | audio10/effect13종·Pause새소리0/중복억제·사망복귀/Collider diff0, 기본2패턴 보스, RouteAll R01~R17→타이틀 PASS/Error0 |

각 폴더의 `QA-A01/C06/A06/B06/C05.cmd`는 QA 전용 슬롯을 초기화한다. 실제 기본13 회귀는 1회 13/13 PASS: S1-02/S1-04/S1-07/S1-08/Camera/T1/T2/T3/T4/S2/F2R/LOGR/Boss. 원문 `Validation/_raw/B04/REG-B04final-summary.txt` 및 base13/ 로그·증거. 이전 검증 문서 덮어쓰기0.

## 핵심 결과

- 최종 AV1 30/60/120fps 각각150틱 입력의 위치·속도 CSV SHA256은 기본3개와 동일: `5B3CAFA8CE5512CA6BC6D22D36CE7E1EDD01094906BDEF5AA5F2DFAE11FB3C43`. 검증 시작 fixture 이후 이동·점프·DJ·대시는 실제 InputSystem 입력이다. root/Collider 변경0, 후보 시각 자식만 애니메이션.
- B2 기본30/60/120 및 fast/slow60: 실제 Unity Editor 물리 입력으로 DJ 돌진 회피0/실패1, 도약·돌진 Pause/재개·죽음/CP-C04/플레이어 HP·보스 HP/첫패턴 초기화 모두 PASS/Error0. 추가 승인 새접근1개 사용, slow 첫 재시도에서 첫점프 유지시간만 보정. BOSS-2 전체는 최종 도약회피 전체fps·동시HP0 미검증을 남긴 PARTIAL, 기존 실제 후보exe는 보존.
- PCM 길이/peak 표10종, peak 편차1.38dB/6dB outlier0. 후보 body 명도·hazard opacity 개선, 기본색/배치 불변. 지정9방 1920×1080 PNG9개 비검정/육안 확인, 실제 exe 방 카메라 렌더로 HUD/overlay 제외. PNG/원본표/CSV는 raw 미커밋.
- [55분 QA](B04-QA-summary.md): 이전 수락 대기→AV1 후보→Gate 판단 시점 순서. 자연 전체 완주 및 감각 수락은 자동 검사와 구분.

## 실패/한계 보존

최초 AV1(복귀 보정 전 빌드)의 R11 replay는 시작위치 편차 .110u에서 기록을 소진했다. 기본/수정r2/색보정delivery/최종r4의 연속 replay는 통과. 입력 소진의 간헐 민감성을 게임 회귀로 확정하지 않으며 소진 이력을 감추지 않는다. 원문:

```text
MEASURE continuous start deviation R11=0.110u; no placement performed
FAIL System.Exception: R11 fixed-tick replay recording exhausted room=B01 pos=(33.31, 5.41)
Error/Exception/Assert=0
```

초기 컴파일 도구 문제(이벤트 선언/uint, Unity 미지원 Convert.ToHexString)는 수정 후 성공. A/B Task 요약에 원문 포함, create.log/build-main.log 등 최초 로그 보존. slow 첫 회피 실패 원문은 [C 요약](B04-C-summary.md)에 보존. 음량 계산 Python stdout의 cp949 인코딩만 실패했으며, 먼저 저장된 표/CSV는 확인 완료. 검정 촬영0/검정 보정0회, 이전 swapchain 방식 실패는 새로운 카메라 렌더로 대체.

미실시: 사용자 청감·색/형태·난이도 수락, 자연 전체 플레이 시간, 기존 P03 이슈 수정, BOSS-2 잔여 도약회피/동시HP0 검증, HUD/오버레이 사진. 미실시를 PASS 또는 Sprint Gate로 표기하지 않는다.

## 보존 감사

START 대비 기존 Scene/Prefab/기본 Tuning/BossCandidates, PlayerInputReader/PlayerMotor/PlayerDash/PlayerHealth/GameState/RoomSession/KillZone, Packages/ProjectSettings, PRD/GAME_DESIGN diff0. 보호 관찰 이벤트 추가0. 기존 코드 변화는 관련 비보호 HitFeedback의 `Presented` 관찰 이벤트/호출뿐이며 기본 빌드에는 구독자0. AV1 bootstrap/profile은 build scene copy에만 삽입한다.

Task별 요약: [A](B04-A-summary.md), [B](B04-B-summary.md), [복귀](B04-B-recovery-summary.md), [C](B04-C-summary.md), [음량](B04-volume-summary.md), [명도](B04-contrast-summary.md), [사진](B04-shots-summary.md), [QA](B04-QA-summary.md). 원본 전체 `Validation/_raw/B04/`, AppData Playtest CSV는 원본을 유지한 복사본, 기존 B02/B03 raw 삭제0. 이전 B03 Task/Handoff는 Archive에 보존한다.
