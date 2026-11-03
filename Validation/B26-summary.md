# Batch26 종료 — 2026-11-03

**자율 처리 B 별도 보고:** 기존 시스템 안에 표시 컴포넌트·shader·오디오 피크필터를 추가했다. 아트/배경/증기/HUD·적 사망/반짝임·플레이어 탄성/잔상·흰피격·방이름 표시층이며 충돌체·피격/사망 판정시점·B1/B2전투값 변경0. 감각 수락은 신규 REVIEW9개로 남겼다.

<!-- BATCH-CARD -->
**Batch Card @`d8e0338` (`b845145..d8e0338`): 변경120파일(+17742/-11453줄·바이너리28), 허용 범위 밖0. TASKS 깨진 링크0·없는 빌드/런처0.** 표의 REVIEW14 + 본문 B20-T4 1 = 총15. 별도 `git status/diff`도 확인했고 마지막 변경은 이 카드 기록 문서뿐이다. [기계 원문](_raw/26/batch-card-final.txt). 이번 실제 회귀는 `_raw/regression/B26-final/result.txt`의13/13을 직접 확인했다.
<!-- /BATCH-CARD -->

## 결과와 인계

- 승인22Task 실행: DONE13 / REVIEW9. 기존 QA6 / 목록 밖 기존0(ID 없음) / 신규9 = REVIEW15. **다음 콘텐츠 Batch 전에 사용자 QA1회 필요.** Task상한30·10시간 안에서 승인 Backlog/Queue가 소진돼 종료했다.
- 시작 Base `b845145`(15:56 KST), Claude 설계 시작 커밋 `e2bc7d1`. 지시문 목표 상태 문구를 확인하고 승인된6파일을 내용 변경 없이 커밋했다. fetch/push/squash 없음. 기존 미푸시33커밋을 보존해 합치기 조건을 충족하지 않으므로 squash하지 않았다. 최종 HEAD/태그는 `git log -1`·`git show B26`으로 확인한다.
- draft6.1·아트v1.1 AB12방. B02바닥/위하강·B06조각 각5/5, B05기둥조각3/3·모든 머리0. 기존 실패6경로 중 실제출구5통과, A03 서쪽 M건물4u가 설계 결정 잔여다.
- **전체 AB 완주 아님:** 중간Continue 없는 A고리64.507s→B02바닥→B04승리/B03실제귀환까지, E4구간(12.312,11)에서 HP1→0 이후 `GoX(15.8)` 실패로 종료. T9은 지시문이 허용한 첫 막힘/원문 기록 기준으로 DONE, 뒤 구간·최종Continue는 미실시.
- 사용자 차례: [QA 목록](../TASKS.md#3-직접-플레이-확인), BodySize약5분 + [Play-V5-Loop](../Launchers/current/Play-V5-Loop.cmd)30~40분. [Claude용 한 장](B26-for-claude.md)에 방/좌표/원문·결정3건을 정리했다.

## 실제 검증

| 대상 | 결과·이번 원문 |
| --- | --- |
| Preflight | Hub licensing=True, Unity exit0. `_raw/26/preflight-20261103-155635-526-296852e7/` |
| 빌드 | Development `Builds/B26-delivery`, Succeeded errors=0 warnings=0. `_raw/26/delivery-direct-loop/result.txt` |
| 전체 회귀 | **REGRESSION13/13**, `_raw/regression/B26-final/result.txt`. 이후 변경은 B06 귀환/중간Continue 제거·HP추적 등 검사 입력/운영도구뿐이며 최종 제품소스는 동일. 카드의 과거 REGRESSION행을 증거로 쓰지 않음 |
| 최종 런처 | Loop·Graybox(각30초/CSV생성)·B1·B2·V4 exit0/Error0, `_raw/26/final-{Loop,Graybox,B1,B2,V4}.txt` |
| 최종 B3 | cycle `0,4,1,6,2,5,7,3,0`, phase1회/사망CP-C04/HP28·위험정리 PASS/Error0. `_raw/26/final-B3Cycle/`·`final-B3Phase/`. HP100/단일패턴 fixture이며 자연승리 아님 |
| 정적 | AB JSON·world, FAIL0/WARN48·self-test PASS. `_raw/26/final-static.md`, T1과 바이트 동일. B25 WARN47 대비 새1건은 `WARN B02 height: platform=1 nearest lower rise=7 (static candidate)`; 승인 위통로 분할에 따른 후보, 실제통과5/5. 기존/신규FAIL0. max-step 실측 미실시 |
| Scene | Unity6000.3.24f1 `V5Batch18.Clearance -verify-b18-clearance`, V5 12Scene FAIL0/WARN0. `_raw/26/final-clearance/result.txt`는 T1과 바이트 동일 |
| 아트 | staged12 PNG SHA 일치·bg320×360·Unity28PNG16PPU/Point/무압축, 최종 카메라24장/컨택트시트·목업 비교 시각 확인. `_raw/26/art/delivery/`. pose fixture/physics꺼짐·OnGUI제외로 경로 증명과 구분 |
| 지도/HUD | 지도 방문전후 RenderTexture PNG2장(T10-map-r2)·AB12/문18. HUD는 실제 Sprite/count의 격리 RenderTexture(BL1-hud), 전체 데스크톱/GUI 배치 촬영 아님 |
| CSV | Loop 런처30초 smoke → `Playtest/smoke-20261103-164224-075-Loop-42848f29.csv`와 `-room-times.csv`, A03 1방/1방문30.060717s. 실제 조작 없이 기록 시작/종료 연결 검증. 최종 Loop/Graybox도 새 CSV·요약 생성 |
| 새 연출 | 적4종 사망 즉시/표시0.300s·잔여0, 반짝임1.5s·밝기0.663/1.487 PNG2장, 플레이어 점프/착지 각1·잔상3·Collider불변, 적4/보스2 흰피격0.0667s, 방이름12개 각1.200s. `_raw/26/BL3-cleanup-final`·`BL5-sparkle`·`Q1-player-art`·`Q2-white-flash-r2`·`Q3-room-names` |

실행명세: Python `C:/Users/gosu3/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe`, Unity `C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe`, Scene `Assets/Scenes/V5/*.unity`12개, 전달 실행파일 `Builds/B26-delivery/Afterglow.exe`. 정적 기준은 같은 검사기의 B25 `Validation/_raw/25/T17-static.md`, Scene 기준은 이번 T1 생성 직후 결과다. 새 출력은 `Validation/_raw/26/final-static.md`·`final-clearance/result.txt`.

```text
<Python> -B Tools/Check-RoomRules.py Design/v5/rooms_v5_AB.json --v5 --self-test --world Design/v5/world_v5_AB.json --output Validation/_raw/26/final-static.md
<Unity> -batchmode -quit -projectPath <repo> -executeMethod V5Batch18.Clearance -verify-b18-clearance -verify-output <new-output> -logFile <new-log>
```

검사 명령/Python 경로·기준은 [T1](B26-T1-world.md). 정적 합성fixture의 고의 FAIL은 self-test 통과와 구분한다. Scene/정적 검사는 실제 자연 완주의 증명이 아니다.

## Task별 커밋·파일·결과

`B22Probe.B26.cs`는 `Assets/Scripts/Verification/`, 표시 C#은 `Assets/Scripts/`, Unity 편집기는 `Assets/Scripts/Editor/` 아래다. 각 행의 실제 검증/미실시는 위 표·Task 상세를 따른다.

| Task | 상태 | 커밋 | 주요 파일·결과/문제 |
| --- | --- | --- | --- |
| T0 | DONE | 3a8e4c4 | TASKS·DECISIONS, [21행 전후표](B26-T0-review-cleanup.md). 폐기10·수락3·미채택2·QA6 유지 |
| T1 | DONE | e457081 | V5 12Scene·[T1](B26-T1-world.md). 재생성/요소수·정적/Scene FAIL0 |
| T2 | DONE | b8e7692 | B22Probe.B26.cs·[4구간 측정](B26-T2-routes.md). 5/5·5/5·5/5·3/3·머리0, 독립진입 fixture |
| T3 | DONE | 65cf6ce | [스테이징](B26-T3-art-staging.md)·raw manifest. SHA12/12, 이 Task의 Assets 복사0 |
| T4 | REVIEW | b51af02 | B25ArtImport/ArtAuthoring/ArtRoom/Settings·Original/v1.28PNG·종류별 sprite, meta/Collider불변 |
| T5 | REVIEW | 6fbf874 | B26SteamVisual·B26ArtAuthoring·V5 Scene. 아래정렬/sky1·투명증기·표시입자 |
| T6 | DONE | e9400b8 | [갈색 표지 원인](B26-T6-guide.md). SignPost SpriteRenderer만 숨김, 메시지/판정 보존 |
| T7 | DONE | ea577ef | SkyBlend Unity Asset·Steam 표시 보완·[캡처](B26-T7-art.md). 최종24장과 목업/컨택트시트 |
| T8 | DONE | d04767a·9380d81 | B22Probe.B26.cs·Run-RoomRoutes.ps1·[원인 장부](B26-T8-route-ledger.md). 실제출구5/6, A03 지형 판단 |
| T9 | DONE(측정) | 5123235·3c49831 | [전체고리 기록](B26-T9-AB-loop.md). 중간Continue 제거·B03 E4구간 HP0/첫실패·완주 미실시 |
| T10 | DONE | 955a5b6 | V4WorldMap.ExportPng·B25Map probe·Run-B22Probe.ps1, PNG2장/중복지도인자 수정 |
| T11 | DONE | 5ab8ccc·종료커밋 | Start-V4/V5·최종 빌드·회귀13/13·smoke5종/B3 PASS |
| T12 | DONE | 04de38d·종료커밋 | README·TASKS3/9, 기존6/신규9 QA1회·기록칸·조정위치 |
| BL1 | REVIEW | f0ae397 | GameHUD·fragment Sprite/count·B26HudCapture, 격리 HUD PNG |
| BL2 | DONE | 603faf8 | PlaytestLogger·Start-V5·Export-RoomTimes 연결, 30초 CSV. Continue는 새 suffix로 원본 보존 |
| BL3 | REVIEW | 936951a | B26DeathDisplay/FadingSprite·B25EffectsHost, 4종0.300s/판정즉시·잔여0 |
| BL4 | REVIEW | a62af29 | B26AudioHeadroom·AudioAuthoring·Feedback.asset, 볼륨표/PCM클리핑0. 청감 미확인 |
| BL5 | REVIEW | 7421318 | B26IdleSparkle·V5Scene·Settings,1.5초/PNG2장·Collider불변 |
| Q1 | REVIEW | dca4693 | B26PlayerArtMotion·ArtRoom·Settings, flip/0.1초/잔상3·root/Collider불변 |
| Q2 | REVIEW | 19384c9 | B26WhiteFlash.shader/HitFlash·Unity WhiteFlash.mat,6종0.0667초·Collider불변 |
| Q3 | REVIEW | 30dbece | B26RoomNotice·12Scene, JSON title의 — 앞부분·1.200초 |
| Q4 | DONE | 0de17d4 | [Claude 한 장](B26-for-claude.md), 실패/REVIEW/결정·좌표/원문 링크 |

## 오디오 볼륨·측정

이벤트별 gain은 기존 값을 유지하고 공통 master0.8, 과도한 buffer 피크만0.85로 제한한다(일반 buffer gain1). `_raw/26/BL4-audio-levels-final/result.txt`·`BL4-audio/result.txt`.

| 이벤트 | 선형 gain |
| --- | ---: |
| jump | 0.300 |
| dash | 0.300 |
| swing | 0.300 |
| hit | 0.300 |
| hurt | 0.300 |
| death | 0.300 |
| checkpoint | 0.300 |
| ability | 0.300 |
| bossTell | 0.340 |
| victory | 0.300 |
| fragment | 0.280 |
| doorLock | 0.280 |
| doorOpen | 0.280 |
| lift | 0.280 |
| steam | 0.180 |

`MEASURE B26 audio master=0.800000 listenerPeak=0.000000 clippedSamples=0 filterBlocks=58 filterOutputPeak=0.000000 PCMstressPre=11.983310 PCMstressPost=0.850000 stressClips=0`

PCM 스트레스는 모든 이벤트 샘플8중첩을 동일 필터로 처리한 측정이다. 런타임 listener가0이어서 실제 장치 청감/음량을 검증했다고 주장하지 않는다. 처음의 과도한 전체감쇠 master0.075346는 채택하지 않았고 최종 master0.8/filter 방식만 전달한다.

## 실제 실패 원문 색인

실패 로그는 삭제/덮어쓰기 없이 `_raw/26/<실행>/result.txt`에 남겼다. 아래는 원인과 시도를 확인하기 위한 예외 첫 줄이며, 최종 통과 증거로 대신하지 않는다.

| 실행 | FAIL 원문 |
| --- | --- |
| Q2-white-flash | `FAIL System.ArgumentException: JsonUtility.ToJson does not support engine types.` |
| T10-map-r1 | `FAIL System.Exception: AB12 map loaded` |
| T2-B02-floor-r1 | `FAIL System.Exception: B02 floor lamps island landing` |
| T2-B05-r1 | `FAIL System.Exception: B05 actual pillar fragment` |
| T2-B05-r2 | `FAIL System.Exception: wall route B05 left pillar` |
| T2-B05-r3 | `FAIL System.Exception: Timeout B05 pillar top` |
| T8-A03-r1 | `FAIL System.Exception: physical jump A03 M roof from street` |
| T8-A05-r1 | `FAIL System.Exception: GoX timeout target=30.6 actual=(39.36, 1.02)` |
| T8-B02-r1 | `FAIL System.Exception: physical jump B02 E1` |
| T8-B02-r2 | `FAIL System.InvalidOperationException: Sequence contains no matching element` |
| T8-B02-r3 | `FAIL System.Exception: Timeout B02 physical exit B03 room=A03 pos=(41.80, 1.02) move=(1.00, 0.00) locked=False hp=4` |
| T8-B06-return | `FAIL System.Exception: B06 independent return3 lands` |
| T9-AB-loop-r1(중간Continue 변형) | `FAIL System.Exception: GoX timeout target=15.8 actual=(20.37, 1.02)` |
| T9-AB-direct(최종연속) | `FAIL System.Exception: GoX timeout target=15.8 actual=(20.37, 1.02)`; 직전 `MEASURE B26 loopHit room=B03 HP=0 pos=(12.312, 11.000)` |

T2 실패는 증기 안으로 되돌아간 runup/굴뚝 fixture·이탈 높이 입력을 한도 안에서 수정했다. T8의 B02는 E2 회피와 E1 실제 공격, B06 귀환은 섬 목표 중심 감속으로 통과했다. T10 최초는 도구가 A/AB 지도 인자를 중복해 첫 값을 읽은 문제, Q2 최초는 JsonUtility의 엔진형 직렬화 도구 오류였고 속성 비교로 수정했다. A03 지형과 T9 B03 E4접촉 잔여는 해소 주장 없이 보존한다. 초기T9의 중간Continue는 최종연속증거에서 제외하고 T9-AB-direct로 재측정했다.

## 종료 검사·보관·Ruling

- 허용 범위는 승인 지시문 그대로이며 시작6파일 Design 예외를 포함한다. 시작 이후 Design/PRD/GAME_DESIGN/ProjectSettings/V4 Scene·B1/B2 전투 Asset 수정0. 보호149경로는 `e2bc7d1` Git 시작기준/current clean-filter bytes가 동일. **시작 시 live 디스크 해시는 수집하지 못해 Git object에서 소급 구성했다**는 provenance를 raw audit에 명시했다.
- 최종 빌드는 B26, 직전 B25를 유지하고 B24 빌드를 Builds 내부 절대경로 확인 후 정리했다. B24를 가리키던 README도 같이 갱신했다. regression B23/B24 원본170개는 새 zip에서 수/각SHA256 일치 확인 후 압축 원본을 정리했다. 최근 B25/B26만 풀어 두며 이전 archive zip은 보존했다. raw 안에 새 빌드/worktree/dll/Python환경을 만들지 않았다.
- Ruling: 12방 목표이므로 정적 검사 대상은 승인 `rooms_v5_AB.json`·`world_v5_AB.json` / A기준 파일을 보존하면서 실제대상을 검사 / 틀렸을 때 비용=대상 재검사.
- Ruling: 배경 원본 상단 RGB15/18/24와 sky1 차이는 상단2u 표시 전환층으로 연결 / 설계·원본PNG 수정 없이 이음새 해소 / 비용=표시층 제거·재생성.
- Ruling: audio 일반 gain을 유지하고 과도 피크만 제한 / 획일 감쇠0.075346 대신 청감 가독성 유지 / 비용=공통 filter/master 조정·재빌드.
- Ruling: HUD는 실제 Sprite/count의 격리 RenderTexture를 캡처 / 기존 ScreenCapture 모듈 실패 경로를 반복하지 않음 / 한계=실제 전체 GUI 배치는 사용자 QA.
- 미실시: 전체AB 후반·최종Continue, 자연 연속 조각수집 완주, 사람 QA·청감·Sprint Gate. 이전 B25 결과를 이번 통과 증거로 쓰지 않았다.

종료 시각: 2026-11-03T17:32:29 KST, Preflight부터 약96분. 10시간/상한30에 도달하지 않았고 승인 Backlog/Queue 소진으로 종료.
