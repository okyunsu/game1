# Batch25 재개 결과 — 2026-11-03

<!-- BATCH-CARD -->
Task 커밋 후 Batch Card를 여기에 기록한다. 검사 기준은 e6b5172, 이번 최종 회귀는 B25-delivery-final이다.
<!-- /BATCH-CARD -->

V5 층 A·B12방, 직접 제작 그래픽18개·배경, 효과음/짧은 입자·적 예고·보스 연출을 실제 빌드에 반영했다. 반환 승강기와 B2 획득대 예외를 수정했고 A 고리/Continue와 B04 실전 승리를 확인했다. **전체 AB 자동 완주는 남았다.** T6/T7/T8/T10/T12 PARTIAL, T14 REVIEW이며 사용자 감각 수락·Sprint Gate는 대신하지 않는다.

현재 [Loop](../Launchers/current/Play-V5-Loop.cmd)·[Graybox](../Launchers/current/Play-V5-Graybox.cmd)와 V4 런처는 Builds/B25-delivery를 사용한다. 직전 B24 보존, B23 슬롯/파일목록은 [_raw/25/T17-housekeeping](_raw/25/T17-housekeeping)에 보존 후 옛 빌드만 제거했다.

## 실행·통합 증거

- 승인 지시 [Batch25](../Design/proposals-1101-B25/01_CODEX_BATCH25.md), 실행 Base e6b5172/재개 Base fb63b98. Claude 설계 변경은 e6b5172의 Design/v5·art_v1·지시문/D60. 이후 Design 원본 변경0.
- 최초 Preflight exit198 원문과 독립 no 결과는 [최초 종료](B25-summary.md)에 보존. 사용자 Hub 실행 후 새 [_raw/25/preflight-20261102-223351-597-a0db2b7c](_raw/25/preflight-20261102-223351-597-a0db2b7c) hubLicensingClient=True/exit0으로 재개했다.
- **최종 Development** [_raw/25/T17-build-final/result.txt](_raw/25/T17-build-final/result.txt): `Succeeded errors=0 warnings=0`. Unity6000.3.24f1, B22Authoring.Build, V5 A01~B06·V4·B1/B2 시험장 포함. ProjectSettings 수정0.
- **최종 전체 회귀 `REGRESSION 13/13`**. 앞선 B25-final은13/13, P1/P2/P3/P4 Quiet도 각각13/13. 최종 결과는 [_raw/regression/B25-delivery-final/result.txt](_raw/regression/B25-delivery-final/result.txt).
- 최종 [_raw/25/T17-smoke-final](_raw/25/T17-smoke-final): Loop·B1·B2·Graybox·V4 각각 exit0/`Error/Exception/Assert=0`. 시작/설정 smoke이며 자연 완주 증명과 구분한다.
- [_raw/25/T17-B3-cycle-final/B3Cycle.txt](_raw/25/T17-B3-cycle-final/B3Cycle.txt): 8패턴 `0,4,1,6,2,5,7,3,0`·비활성 건너뜀/잔류위험0 PASS. [_raw/25/T17-B3-phase-final/B3Phase.txt](_raw/25/T17-B3-phase-final/B3Phase.txt): 1회 전환·강화·CP-C04사망/HP28·위험정리 PASS/Error0. HP100/단일패턴 설정 fixture이며 자연승리 아님.
- B3 최초 실행은 `Missing or duplicate initial Spawn ID` 뒤 `FAIL System.Exception: Timeout room ready`, `ArgumentException: The scene is invalid.`/Error1. cycle·phase 모두 전투 진입 전 같은 fixture 실패였다. B24 동일 cycle baseline은 PASS여서 새 빌드 진입 연결 문제로 구분했다. Editor 전용 CP fixture를 Development의 명시 `-boss-fixture`에서도 등록하도록 보완 후 위 최종 검사가 통과했다. V4 Scene·저장형식·B3튜닝 변경0. 이 실패를 보스 패턴 FAIL2회로 읽지 않았다. [실패](_raw/25/T17-B3-cycle/B3Cycle.txt), [B24 비교](_raw/25/T17-B3-B24-baseline/B3Cycle.txt).
- **정적** [_raw/25/T17-static.md](_raw/25/T17-static.md): AB 데이터FAIL0/WARN47, 이전 동일 입력55에서 증기 도달WARN8 해소. self-test PASS(일부러 잘못 만든 synthetic/fixture FAIL 원문 포함). 유효 max-step evidence가 없어 도랑 탈출 MEASURE 미실시, 정적 후보는 실제 완주 증명 아님.
- **Scene** [_raw/25/T17-clearance/result.txt](_raw/25/T17-clearance/result.txt): Assets/Scenes/V5 12개를 V5Batch18.Clearance로 검사, FAIL0/WARN0. [_raw/25/T17-clearance-comparison.txt](_raw/25/T17-clearance-comparison.txt) `identical=True`: 아트 적용 후 T14와 최종 결과 원문 완전동일. T14 적용 중 모든 Collider 직렬화/Transform/layer/enabled 비교도 동일.
- 보호134경로 SHA256 변경0([원문](_raw/25/T17-protected.txt)). B1·기본Tuning·Player prefab·Design·PRD·GAME_DESIGN·ProjectSettings 보존. Unity 에셋/메타는 Editor API로 생성/저장했다.

## 플레이 가능 범위·측정

| Task | 결과·근거 |
| --- | --- |
| T1 DONE | LeverLift V5 startLow 접촉 skin=2×Physics2D.defaultContactOffset. 레버 공격 관성으로 수평0.015u 벗어난 rider를 놓치던 원인. 왕복3회·다른층호출재탑승·V4 B01/C01 PASS/Error0. [T1](_raw/25/T1-contact-skin/result.txt) |
| T2 DONE | 보스 전투잠금 때 획득대를 floorY로 내리고 사망reset 복원. `MEASURE B25 noWallJump floorHits=10/10 pedestalAvoidances=0/10 pedestalTop=1.000`. [T2](_raw/25/T2-sink10/result.txt) |
| T4/T5 DONE | AB 입력·12Scene/모든 요소 수·B04 우측진입/천장spawn→등장시 바닥착지·CP-B01. B05 중복 solid8은 같은영역 cover0가 충돌 담당. [T5 counts](_raw/25/T5-counts-final/result.txt) |
| T6 PARTIAL | 지도AB12방/B6방/18문·방문전숨김/방문후표시·연결/stub PASS. batch 캡처 미생성, exe도 `Failed to capture screen shot.` 두 번/Error2. Computer Use `Computer Use app approval timed out`으로 실제창 캡처 미확보. [exe 실패](_raw/25/T6-map-exe/result.txt) |
| T7 PARTIAL | A03 천장→B01·B01 시장/지붕·B03 증기2개/문 각5/5 머리0. B02 바닥 포고는 정지0/달리기1회 뒤 산성복귀, 미완주. [달리기 실패](_raw/25/T7-B02-running/result.txt) |
| T8 PARTIAL | B02 증기/위통로/조각5/5 머리0. B06 일반/공중대시 각포고1 뒤복귀, 조각미수집. [실패](_raw/25/T8-B06-airdash/result.txt) |
| T9 DONE | B04 실제획득/저장·3u이탈잠금·등장1.817s·첫예고1.005s/양방향높이2.800·사망CP-B01/우측재시도. `MEASURE B2 fight seconds=32.350 defeated=True bossHP=0 HP=1 wallJumps=7 pogos=26`. 실제 입력 처치/ID저장/재진입보스없음. [원문](_raw/25/T9-B04-fight1/result.txt) |
| T10 PARTIAL | B01탑/조각/B05·A02왕복/이동조각·B03굴뚝/B06 각3/3. A02-B05의 남은 stub은 WORLD18/B25-T4 명시 연결 우선으로 생성기/Scene만 해소, Design그대로. B05 서쪽은3접근 모두 A02낙하. [최종 실패](_raw/25/T10-B05-cover-early/result.txt) |
| T11 DONE | A01선반 V5-A01-3·A04섬 V5-A04-0·A01감실 V5-A01-4·B02위 V5-B02-0 실제접촉4개 maxHP5/전회복·ID/보너스저장Continue. 접촉위치 fixture이며 자연연속수집 아님. [원문](_raw/25/T11-first-four/result.txt) |
| T12 PARTIAL | `MEASURE B24 loop seconds=65.207 complete=True`·재실행65.027, A고리 실제입력 및 Continue PASS. 이어 B01 CP/지붕/B02위까지 연속PASS. E3긴도약HP1/후속포고HP0 실패2회, 전체AB/마지막Continue 미실시. [원문](_raw/25/T12-expanded1/result.txt)·[두번째](_raw/25/T12-expanded2-E3/result.txt) |
| T13 DONE / T14 REVIEW | 원본18PNG SHA일치·16PPU/Point/무압축/FullRect, 원본스프라이트·층별배경0.3 적용·Collider불변. 아트가독성/감각은 사용자수락 대기. [import](_raw/25/T13-import/result.txt) |
| T15/T16 DONE | Graybox 별도slot/표시전환 smoke0. 실제 게임카메라 12방×2 PNG24장·B03 전체·목업과 나란히 비교, 육안확인. [24장](_raw/25/art/contact-sheet-24.png)·[비교](_raw/25/art/B03-mock-vs-unity.png). 촬영 pose/physics-off·HUD제외라 경로/지도증거로 쓰지 않음 |

부분 실패의 실제 원문:

```text
MEASURE loop jump=B02 upper E3/acid departure landed=False end=(37.6070, 5.2610) hp=1
FAIL System.Exception: physical jump B02 upper E3/acid departure
MEASURE B05 floor hole crossed=False room=A02 pos=(14.000, 33.246)
FAIL System.Exception: B05 floor hole crossed to west by actual jump
```

T7/T8/T10/T12 각 경로 한도에서 중단하고 실패 입력 실험은 제거했다. 승인 없이 좌표·수치·규칙을 고쳐 통과 처리하지 않았다. A01 자연 왕복은 이번 재측정 대상이 아니어서 미실시이며, B24 당시 상태 이력은 자동 DONE으로 바꾸지 않았다.

## Queue

- Q1/Q2 DONE: 기존 AV1 직접 제작 wav 재사용, B25 공유 Feedback Asset. `MEASURE B25 combat swing=1 hit=1 death=1 hurt=1 bursts=3 sourcePlaying=True lifetime=0.180`. 조각/레버/증기/문잠금/열림 각1/Error0. 실제 공격·트리거+일부 ReceiveHit/TakeDamage API fixture, 자연전투 증거와 구분. [Q1](_raw/25/Q1-events/result.txt)·[Q2](_raw/25/Q2-events/result.txt). Q2 종료시 `MEASURE HitStop ... reason=destroyed`는 강제 종료 정리 로그이며 정지시간 측정은 Q3만 사용.
- Q3 DONE: B1/B2 등장1.800/1.817s 안 minZoom0.9000→1.0000복원, 처치정지요청0.100s 실제0.1168/0.1167s(60fps). B3 optionalFX 없음/줌1. [Q3](_raw/25/Q3-measure/result.txt).
- Q4 DONE: E1 예측0.280/실접촉0.267s, E2 예고0.280/실돌진0.283s, E3 예고0.283/실발사0.283s/Error0. AI시간 변경0. [Q4](_raw/25/Q4-measure/result.txt).
- Q5 DONE(측정 기준): [Run-RoomRoutes](../Tools/Run-RoomRoutes.ps1), 최종게임코드가 같은 첫Development exe에서12방 모두 시도·PASS6/FAIL6. 마지막 재빌드는 명시B3 fixture만 추가하여 Q5 V5인자에는 영향 없음. 실패원문 허용 기준 충족이며 12/12통과 아님. [CSV](_raw/25/Q5-final-routes/routes.csv)·[요약](_raw/25/Q5-final-routes/summary.txt). A01/A02/A04/B01/B03/B05 PASS. A03 건물앞 GoX timeout, A05 바닥접근 timeout, A06 출구높이에서 이탈·최종다른방, B02 상부 E3/긴도약, B04 인트로뒤벽접근, B06 포고 FAIL. 새 진입 fixture 입력의 한계도 있어 제품 진행불가로 단정하지 않음. 각방 result/game.log 보존, 추가재시도0.
- Q6 DONE: [Export-RoomTimes](../Tools/Export-RoomTimes.py), CSV/README·pause/재방문/마지막방/잘못된시간/누락출구/덮어쓰기거절 검증. 과거사람CSV 8방문/4방 active95.926496s는 B25 신플레이 시간 아님.

## 실행 판단·인계

Ruling: 실패한 P3 경로에 의존하지 않는 Queue를 최종 P5 이전에 실행 / 지시문이 단계차단 시 독립Queue 허용, 한번의통합빌드에 포함 / 틀렸을 때 비용=추가회귀로 확인하며 제품범위 증가 없음.

Ruling: Map TextAsset을 허용범위 Scripts/Resources에 두고 V5 AB를 fallback으로 읽음 / Assets/Maps가 허용밖 / 비용=Resources중복 가능성이므로 명시이름 로딩과 Editor/exe12방 확인.

Ruling: Q4 E1은 순찰접촉 ETA의 마지막0.3s 색예고, E2/E3은 기존Warning 마지막0.3s / 새로운 AI지연·밸런스 변경 없이 승인시각예고 구현 / 비용=E1 예고는 이동에 따라 예측적이며 실제0.267s 실측.

Ruling: B3 실패는 전투전 개발 fixture 누락으로 분리하여 명시 플래그에서만 CP 생성 / 보호Scene 수정과 일반게임 저장계약 변경 회피 / 비용=시험모드 진입 차이, 최종B3회귀 확인.

사용자 확인: [README](../README.md)의 Loop1회30~40분·기존 [TASKS3](../TASKS.md#3-직접-플레이-확인) 순서 유지. B02/B06/B05에서 막히면 방/위치/입력, 머리접촉·보스시도·전체시간·아트가독성을 기록한다. 공유값은 `Assets/Audio/B25/Feedback.asset`, `Assets/Art/Original/v1/ArtSettings.asset`, 배치는 V5 Scene. Play중 변경은 메모→Stop→원본저장→재빌드.

Task별 커밋은 [TASKS9](../TASKS.md#9-latest-handoff--batch25-재개-종료). 최초태그 B25=fb63b98은 현재 로컬origin/main과 같아 게시이력을 보호하고 이동/합치기하지 않음. 재개 태그 B25-resume, fetch/push 없음. 원격 최신이라는 주장은 하지 않는다. Archive 이동은 이번허용밖으로 과거표를 TASKS에 보존. 다음Batch는 플레이결과에 따른 경로설계/지도캡처/감각수락 정리 후보이며 새승인 아님.
