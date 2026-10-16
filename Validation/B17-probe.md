# B17 T1 / T3 — V5 플레이 피드백 원인 측정

Base `2559c4025c643fab8c242728359e42cc66018ce4`, 시작 2026-10-16 22:50:24 KST. Design draft2 읽기 전용. T1은 기존 Scene SHA 변경0을 확인했다. 제품 코드 수정0·Scene 수정0, opt-in 검증 코드만 추가했다. T1 DONE, Quiet13/13.

## 재생성 전 (T1)

**원인 분류: A02·A05 모두 Scene 지형·경계 데이터. 코어 로직 수정 근거 없음.**

|항목|측정|분류|
|---|---|---|
|A02 오른쪽→왼쪽|x3.50→9.94 성공, 복귀 x6.36 정지. 점프도 x6.37 정지|`platform0` 측면·`platform1` 머리 충돌, 첫 발판 아래1.4u/발판 사이1.5u < 몸체1.6u|
|입력·문|복귀 입력 중 잠금0, 지속적인 왼쪽 입력. 도착 억제 해제, 막힌 곳은 문 밖|문 잠금·입력 잠금 아님|
|A05 낙하 직후5초|플레이어 x6.635u/y6.231u 이동, 카메라 x/y 변화0, (25,5) 고정|방 높이10 < 화면11, OversizeWindow 꺼짐. Follow 정상|
|A05 대시 수집 직후5초|자연 수집 성공, 플레이어 y2.530u 이동, 카메라 변화0|수집·입력 정상, 같은 경계 문제|
|A04 높이12 비교5초|카메라 x4.4355u/y1u 이동, Follow 정상|높이12에서는 추적·경계 제한 정상|

CSV는 위치·속도·접지·입력/피격 잠금·접촉 이름·문 트리거 겹침(진입은 연속 프레임 차이)·카메라 위치·Follow·Confiner·경계·도착 억제를 매 렌더 프레임 기록한다. 가상 키보드 입력을 사용했다. 방 이동은 작성된 도착 spawn fixture만 사용하며 방 안 이동에는 위치 보정을 쓰지 않았다. 첫 발판 구간의 실제 충돌을 포함한다. phase 이름은 다음 fixture 전환 프레임에도 잠시 유지되므로 복귀 입력 분석에서 fixture 전환 프레임은 제외한다. 캡처는 실제 방 카메라 RenderTexture, HUD/OnGUI 제외.

[프레임 원본](_raw/B17/T1-before-final.csv), [로그](_raw/B17/T1-before-final.log), [낙하 직후 캡처](_raw/B17/before-final-A05-arrival.png), [사당 후 캡처](_raw/B17/before-final-A05-after-shrine.png).

```text
PASS B17V5Probe actual=exe
PASS isolated save
PASS arrival A02 A01-A02
MEASURE A02-right-first-platform fromX=3.500 target=10 position=(9.94, 1.98) grounded=False locked=False reached=True elapsed=1.133
MEASURE A02-left-walk fromX=9.944 target=1.5 position=(6.36, 1.02) grounded=True locked=False reached=False elapsed=8.066
MEASURE A02-left-jump fromX=6.365 target=1.5 position=(6.37, 2.68) grounded=False locked=False reached=False elapsed=8.067
PASS arrival A05 A04-A05
MEASURE camera A05-arrival roomHeight=10 cameraXRange=0.0000 cameraYRange=0.0000 playerXRange=6.6350 playerYRange=6.2310 dash=False follow=Player(Clone) oversize=False cameraFirst=(25.00, 5.00, -10.00) cameraLast=(25.00, 5.00, -10.00) boundary=(50.00, 10.00, 0.00) locked=False
MEASURE shrine naturalPickup=True position=(37.43, 3.19)
MEASURE camera A05-after-shrine roomHeight=10 cameraXRange=0.0000 cameraYRange=0.0000 playerXRange=0.0600 playerYRange=2.5300 dash=True follow=Player(Clone) oversize=False cameraFirst=(25.00, 5.00, -10.00) cameraLast=(25.00, 5.00, -10.00) boundary=(50.00, 10.00, 0.00) locked=False
PASS arrival A04 A03-A04
MEASURE A04-comparison-position fromX=3.500 target=16 position=(16.05, 7.34) grounded=False locked=False reached=True elapsed=2.783
MEASURE camera A04-height-comparison roomHeight=12 cameraXRange=4.4355 cameraYRange=1.0000 playerXRange=4.2144 playerYRange=6.3360 dash=True follow=Player(Clone) oversize=False cameraFirst=(15.83, 6.50, -10.00) cameraLast=(20.26, 5.50, -10.00) boundary=(50.00, 12.00, 0.00) locked=False
PASS height comparison remained in A04
Error/Exception/Assert=0
```

## 도구 실패·제외 자료

초기 sandbox Unity 라이선스 채널 연결 실패→일반 환경에서 재실행. 캡처는 기존 프로젝트의 런타임 PNG 모듈 Reflection 방식으로 전환했다. 에디터 V5 Scene 빌드 목록 미등록→실제 exe 방식으로 전환. 원본 로그 보존.

```text
Assets\Scripts\Verification\B17V5Probe.cs(58,3): error CS0103: The name 'ScreenCapture' does not exist in the current context
Assets\Scripts\Verification\B17V5Probe.cs(59,229): error CS1061: 'Texture2D' does not contain a definition for 'EncodeToPNG'
FAIL B17V5Probe actual=Editor
FAIL System.Exception: Timeout room ready
Scene 'Assets/Scenes/V5/A01_V5.unity' couldn't be loaded because it has not been added to the active build profile or shared scene list or the AssetBundle has not been loaded.
```

원본 T1-before.log, T1-before-r1.log, T1-before-r2.log, T1-before-r3.txt/log. 최초 exe A04 비교는 hazard 사망→A01 복귀로 오염되어 제외. 입력 접근1회 보정: A04 이동 x20 제한·비교 전 HP 복원·방 유지 확인. T1-before-exe.txt/log/csv의 A02·A05 관찰은 재현됐지만 최종 인수는 T1-before-final만 사용한다. 제외 원문:

```text
MEASURE camera A04-height-comparison roomHeight=16 cameraXRange=14.3258 cameraYRange=0.0000 playerXRange=14.5825 playerYRange=4.1098 dash=True follow=Player(Clone) oversize=False cameraFirst=(15.86, 5.50, -10.00) cameraLast=(28.33, 5.50, -10.00) boundary=(48.00, 16.00, 0.00) locked=False
```

## 재생성 후 (T3)

T3 기술 기준 통과. 같은 가상 키보드 접근으로 측정했다. A02 첫 발판 위(발 y5.01, grounded=True)까지 간 뒤 x1.51로 왼쪽 복귀1.466초 성공. 기존 x6.36 정지 해소. A05 높이12에서 카메라 x39~40을 따라가며 방 중앙x25 고정 해소. 수집 직후에는 플레이어가 사당 위에서 점프하므로 카메라 y5.5 하한 제한은 정상이다(상하 이동 공간은1u). Follow·입력 정상. 코어 로직 수정0.

|측정|재생성 전|재생성 후|
|---|---|---|
|A02 x10→x1.5|x6.36 정지|x1.51 복귀 성공|
|A05 낙하5초 카메라x/y 범위|0 / 0|0.8572 / 1.0000|
|A05 사당후5초 카메라x/y 범위|0 / 0|0.2741 / 0.0000(하한 제한)|
|A04 높이12 비교 카메라x/y 범위|4.4355 / 1|4.5575 / 1|
|구덩이6개|없음|모두 탈출0.400~0.417초·달리며 점프 건넘|

구덩이 테스트는 각 구덩이 중앙 낙하/앞쪽 출발 fixture만 놓고 이후 키 입력으로 측정했다. 낙하 발 위치0.415, jump minFeetOverPit1.015~2.448로 바닥0.4에 빠지지 않고 반대편에 도달했다. A03/A04의 낮은 최솟값은 발이 가장자리를 지날 때의 값이다. 대시·더블점프 입력은 사용하지 않았다.

문14방향·stub4·일방 역방향 차단·DJ·조각4·순찰15·대시 자연 수집/저장·Continue 복원 모두 PASS, Error0. draft2의 추가 적2마리로 순찰이13→15여서 기존 검증기에 기대 수 인자(기본13 보존)를 추가하고15를 입력했다. 실제 exe의 A04/A05 world rect·높아진 문 span도 PASS. Design/기본 지도 Asset 수정0, 빌드 복사본 지도만 draft2를 사용했다.

[CSV](_raw/B17/T3-after.csv), [측정 로그](_raw/B17/T3-after.log), [문 원본](_raw/B17/T3-doors.txt), [낙하 캡처](_raw/B17/after-A05-arrival.png), [사당후 캡처](_raw/B17/after-A05-after-shrine.png).

```text
PASS B17V5Probe actual=exe
PASS isolated save
PASS arrival A02 A01-A02
MEASURE A02-right-first-platform fromX=3.500 target=10 position=(9.96, 5.01) grounded=True locked=False reached=True elapsed=1.433
MEASURE A02-left-walk fromX=9.963 target=1.5 position=(1.51, 1.02) grounded=True locked=False reached=True elapsed=1.466
MEASURE A02-left-jump fromX=1.507 target=1.5 position=(1.47, 1.02) grounded=True locked=False reached=True elapsed=0.050
PASS arrival A05 A04-A05
MEASURE camera A05-arrival roomHeight=12 cameraXRange=0.8572 cameraYRange=1.0000 playerXRange=7.1350 playerYRange=8.2310 dash=False follow=Player(Clone) oversize=False cameraFirst=(40.22, 6.50, -10.00) cameraLast=(39.37, 5.50, -10.00) boundary=(50.00, 12.00, 0.00) locked=False
MEASURE shrine naturalPickup=True position=(37.39, 3.07)
MEASURE camera A05-after-shrine roomHeight=12 cameraXRange=0.2741 cameraYRange=0.0000 playerXRange=0.0120 playerYRange=2.5300 dash=True follow=Player(Clone) oversize=False cameraFirst=(37.65, 5.50, -10.00) cameraLast=(37.37, 5.50, -10.00) boundary=(50.00, 12.00, 0.00) locked=False
PASS arrival A04 A03-A04
MEASURE A04-comparison-position fromX=3.500 target=16 position=(16.05, 7.15) grounded=False locked=False reached=True elapsed=2.517
MEASURE camera A04-height-comparison roomHeight=12 cameraXRange=4.5575 cameraYRange=1.0000 playerXRange=4.3104 playerYRange=6.2080 dash=True follow=Player(Clone) oversize=False cameraFirst=(15.80, 6.50, -10.00) cameraLast=(20.36, 5.50, -10.00) boundary=(50.00, 12.00, 0.00) locked=False
PASS height comparison remained in A04
PASS arrival A01 Entry
MEASURE pit A01 interval=38..40.5 bottomFeet=0.415 escape=True elapsed=0.400 position=(41.24, 2.89)
MEASURE pit-jump A01 interval=38..40.5 crossed=True minFeetOverPit=2.448 position=(41.16, 2.21)
PASS arrival A02 Entry
MEASURE pit A02 interval=11..13.5 bottomFeet=0.415 escape=True elapsed=0.400 position=(14.31, 2.91)
MEASURE pit-jump A02 interval=11..13.5 crossed=True minFeetOverPit=2.448 position=(14.20, 1.98)
PASS arrival A03 Entry
MEASURE pit A03 interval=20.5..23 bottomFeet=0.415 escape=True elapsed=0.417 position=(23.64, 2.91)
MEASURE pit-jump A03 interval=20.5..23 crossed=True minFeetOverPit=1.488 position=(23.67, 3.01)
PASS authored fixture Entry
MEASURE pit A03 interval=36..38.5 bottomFeet=0.415 escape=True elapsed=0.400 position=(39.31, 2.91)
MEASURE pit-jump A03 interval=36..38.5 crossed=True minFeetOverPit=1.015 position=(39.15, 3.38)
PASS arrival A04 Entry
MEASURE pit A04 interval=17.5..20 bottomFeet=0.415 escape=True elapsed=0.400 position=(20.81, 2.91)
MEASURE pit-jump A04 interval=17.5..20 crossed=True minFeetOverPit=1.015 position=(20.70, 3.19)
PASS arrival A05 Entry
MEASURE pit A05 interval=32.5..35 bottomFeet=0.415 escape=True elapsed=0.417 position=(35.86, 2.91)
MEASURE pit-jump A05 interval=32.5..35 crossed=True minFeetOverPit=2.448 position=(35.64, 1.98)
Error/Exception/Assert=0
```

문 검사 핵심 원문:

```text
PASS draft2 world A04/A05 and raised door
MEASURE scene=A01 patrol=2 shards=0
MEASURE scene=A02 patrol=2 shards=2
MEASURE scene=A03 patrol=4 shards=0
MEASURE scene=A04 patrol=3 shards=1
MEASURE scene=A05 patrol=1 shards=0
MEASURE scene=A06 patrol=3 shards=1
PASS patrol15 shards4 total
PASS physical blocked transition0 A01-Z01
PASS physical blocked transition0 A02-B05
PASS physical blocked transition0 A03-B01
PASS physical blocked transition0 A05-Z01
PASS physical blocked transition0 A02-A06
PASS physical blocked transition0 A04-A05
PASS physical blocked transition0 A01-A06
MEASURE door contact=A01-A02 source=A01 player=(47.50, 1.05)
MEASURE door=A01-A02 destination=A02 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A01-A02 source=A02 player=(0.50, 1.05)
MEASURE door=A01-A02 destination=A01 spawn=(44.50, 1.00, 0.00) player=(44.50, 1.01)
MEASURE door contact=A02-A03 source=A02 player=(27.50, 1.05)
MEASURE door=A02-A03 destination=A03 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A02-A03 source=A03 player=(0.50, 1.05)
MEASURE door=A02-A03 destination=A02 spawn=(24.50, 1.00, 0.00) player=(24.50, 1.01)
MEASURE door contact=A02-A03U source=A02 player=(27.50, 9.05)
MEASURE door=A02-A03U destination=A03 spawn=(3.50, 9.00, 0.00) player=(3.50, 9.01)
MEASURE door contact=A02-A03U source=A03 player=(0.50, 9.05)
MEASURE door=A02-A03U destination=A02 spawn=(24.50, 9.00, 0.00) player=(24.50, 9.01)
MEASURE door contact=A03-A04 source=A03 player=(49.50, 13.05)
MEASURE door=A03-A04 destination=A04 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A03-A04 source=A04 player=(0.50, 1.05)
MEASURE door=A03-A04 destination=A03 spawn=(46.50, 13.00, 0.00) player=(46.50, 13.01)
MEASURE door contact=A04-A05 source=A04 player=(46.50, 0.05)
MEASURE door=A04-A05 destination=A05 spawn=(46.50, 9.30, 0.00) player=(46.50, 9.25)
MEASURE door contact=A05-A03 source=A05 player=(0.50, 1.05)
MEASURE door=A05-A03 destination=A03 spawn=(46.50, 1.00, 0.00) player=(46.50, 1.01)
MEASURE door contact=A05-A03 source=A03 player=(49.50, 1.05)
MEASURE door=A05-A03 destination=A05 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A02-A06 source=A06 player=(47.50, 5.05)
MEASURE door=A02-A06 destination=A02 spawn=(3.50, 21.00, 0.00) player=(3.50, 21.01)
MEASURE door contact=A01-A06 source=A01 player=(32.00, 15.05)
MEASURE door=A01-A06 destination=A06 spawn=(36.85, 1.00, 0.00) player=(36.85, 1.01)
MEASURE door contact=A01-A06 source=A06 player=(32.00, 0.05)
MEASURE door=A01-A06 destination=A01 spawn=(32.00, 13.30, 0.00) player=(32.00, 13.25)
PASS Continue restores V5 checkpoint and Dash
Error/Exception/Assert=0
```

Quiet 결과는 종료 후 추가한다.

T3 Quiet: REGRESSION 13/13. T3 DONE.
