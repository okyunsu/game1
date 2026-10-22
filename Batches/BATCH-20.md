이번 실행은 Batch 20(V5 층 A 3차 플레이 피드백: A05 지름길·도랑 측정, 점프 머리 부딪힘 측정, 캐릭터 크기 비교 도구, 지름길 장벽 검사)만 수행한다. AGENTS.md의 Batch Approval·Git Rule·Housekeeping을 따른다. push는 하지 않는다. **`Design/`·V5 Scene·게임 수치·Player.prefab은 바꾸지 않는다.**

## 목적
사용자 직접 플레이(2026-10-22 23:31, `Playtest/20261022-233114.csv`)에서 나온 문제를 처리한다.
1. **A03 → A05 지름길이 막혀 있지 않다.** A05 왼쪽 부서지는 벽(x1~2)은 "A05 안쪽에서만 부순다"는 의도인데, A03 동쪽 아래 문(`A05-A03`)으로 들어가면 도착 위치가 x3.5라 이미 벽 안쪽에 선다. 로그 9~10행에서 실제로 A03→A05를 이 문으로 통과했다.
2. **A05 대시 도랑(x6~12)을 대시 없이 밟고 지나갈 수 있다.** 도랑 바닥 y0.4, 양옆 바닥 y1.0으로 깊이 0.6이다.
3. **점프가 불쾌하다.** 사용자 판단으로 ① 화면에서 캐릭터가 크다 ③ 머리가 위 구조물에 자주 부딪힌다. 현재 규칙은 서 있을 때 머리 위 2.2만 보장하고, 점프 중 머리(몸 1.6 + 점프 2.4 ≈ 4.0)는 보지 않는다.

1·2는 Claude의 설계 데이터 실수다(draft3). **이번 배치는 측정과 도구만 만든다.** 수정 데이터(draft4)는 이 배치 결과를 보고 Claude가 작성하고, Scene 반영은 다음 배치에서 한다. 캐릭터 크기는 사용자가 비교 도구로 직접 고른 뒤 따로 반영한다.

## 읽을 파일
AGENTS.md, TASKS.md(1절·2절·4절·9절 최신 인계), `Design/v5/WORLD_V5.md`(9~11절), `Design/v5/rooms_v5_A.json`, `Design/v5/world_v5_A.json`, `Validation/B19-summary.md`, `B19-rules-ledger.md`, `B18-hop-limits.md`, `B19-dj-route.md`, `Playtest/20261022-233114.csv`, V5 생성기(Assets/Editor), 문·부서지는 벽·위험 지형 런타임 코드, `PlayerMotor.cs`(읽기만), `Tools/Check-RoomRules.py`.

## 시작 조건
- 작업 트리 변경이 `Batches/BATCH-20.md`(미추적)뿐이어야 한다. 그 외 변경이 있으면 시작하지 않고 멈춘다. 이 파일만 먼저 커밋하고("docs: Batch 20 instructions") 그 직후 HEAD를 Base로 기록한다.
- HEAD가 `B19` 태그를 포함해야 한다. push 안 된 이전 커밋이 있어도 정상이다.
- **Unity와 Unity를 호출하는 스크립트(회귀·빌드 포함)는 샌드박스 밖에서 실행한다.** 먼저 `LOCALAPPDATA`가 `C:\Users\gosu3\AppData\Local` 같은 일반 경로인지 확인하고 Unity 실행 확인(`-batchmode -quit`, 종료 코드 0)을 한다. 실패하면 `Validation/B20-blocked.md`에 원문만 남기고 종료한다. 그 외 작업은 샌드박스 안에서 한다.

## 예산·큐 규칙
- 최대 8 Task 또는 **4시간**. 3시간 30분이 지나면 새 Task를 시작하지 않고 종료 처리를 한다.
- BLOCKED는 그 Task 변경만 되돌리고 다음으로. 선행이 BLOCKED면 SKIPPED. 재시도는 같은 문제 1회.
- 회귀는 `Tools/Run-Regression.ps1 -Quiet`(코드를 바꾼 Task마다)와 마지막 기본 방식 1회. 래퍼의 종료 코드 판정과 결과 원문이 다르면 원문을 기록하고 원문으로 판정한다(B19 T6 사례).
- 기존 Validation은 덮어쓰지 않고 `B20-*`로 저장한다. C# 문자열에 역슬래시 금지, 코드 수정마다 컴파일 확인.
- push 안 된 이전 커밋이 있으면 합치기(squash)를 하지 않고 Task 커밋을 보존한다. 없으면 배치 끝에 하나로 합친다. 태그 `B20`은 마지막 커밋에.

## 허용 범위
`Assets/Editor/**`, `Assets/Scripts/Verification/**`, `Assets/Scripts/Dev/**`(새 폴더), `Assets/Scenes/Test/BodySizeTest.unity`(새 파일), `Tools/Check-RoomRules.py`, `Tools/fixtures/**`, `Launchers/current/*V5*`·`Launchers/current/Play-BodySize.cmd`(새 파일), `Validation/B20-*`, TASKS.md, README.md, `Batches/BATCH-20.md`, 빌드 산출물(Housekeeping 개수 지킴).
수정 금지: `Design/`(읽기 전용), `Assets/Scenes/V5/**`, V4 Scene·Resources·V4 런처, `MovementTest` Scene(회귀가 씀), `Player.prefab`, 수치 에셋(`PlayerTuning.asset` 등), 보스·층 B·C, `PlayerMotor.cs`·`RoomSession.cs`·`RoomCameraRig.cs`, 이전 `Validation/B*-*.md`.
원인이나 구현이 금지 파일의 수정을 요구하면 고치지 않고, 어느 파일의 어느 값·로직인지 기록해 Claude에게 돌려준다.

## 사전 결정
| 결정 | 답 | 막히면 대체(비우면 멈춤) |
| --- | --- | --- |
| 데이터 | `Design/v5/` draft3 그대로 | |
| 몸 크기 | `Player.prefab` BoxCollider2D를 읽어 쓴다(0.7×1.6) | 읽기 실패 시 0.7×1.6, 요약에 적는다 |
| 점프 높이 | 하드코딩하지 않고 `PlayerTuning.asset`을 읽어 계산한 값과 실제 측정값을 둘 다 적는다. 판정에는 실제 측정값을 쓴다 | 측정 실패 시 계산값, 요약에 적는다 |
| 크기 비교 방식 | 플레이어 전체를 **발 위치 고정으로 균일 축소**(콜라이더·스프라이트·자식 판정 모두). 높이 1.6 / 1.3 / 1.1 세 가지. 점프·이동·중력 수치와 카메라는 그대로 | |
| 공격·포고 판정 | 축소 시 공격 판정도 같이 줄어든다(기본). 줄어든 사거리는 측정만 하고 보정하지 않는다 | |
| 크기 바꾸기 적용 범위 | Editor·Development 빌드에서만 동작. 기본값은 1.6이고, Release 빌드에는 포함하지 않는다(B14 검증 코드 제외 방식과 같음) | |

## Task
| Task | 유형 | 내용 | 합격 기준 |
|---|---|---|---|
| T1 | verification | **A05 지름길 측정(변경 없음).** (1) 실제 exe에서 A03 동쪽 아래 문으로 A05에 들어간다: 도착 위치, 부서지는 벽(x1~2) 기준 어느 쪽인지, 벽이 그 시점에 존재하는지. (2) 반대로 A05 안쪽에서 벽을 부순 뒤 A03으로 나가고 다시 들어오기, 저장·이어하기 후 벽 상태 유지 여부. (3) 코드 조사: world의 문 `type: shortcut`이 생성기·런타임에서 어떻게 처리되는지, "부서지는 벽을 부수기 전까지 닫힌 문"을 만들 기존 수단(문 잠금 플래그·gate·저장 키)이 있는지, 없으면 무엇이 필요한지 | `Validation/B20-shortcut.md`에 (1)(2) 측정 원문과 판정, (3)은 관련 파일·함수·필드 목록과 "기존 수단으로 가능 / 새 코드 필요(필요한 것 한 줄)" 결론. 사용자 로그(9~10행)와 같은 통과가 재현되어야 한다 |
| T2 | verification | **도랑·대시 수치 측정(변경 없음).** (1) A05 도랑 x6~12를 능력 없이 걷기·점프로 건널 수 있는지, 도랑 바닥 위험 지형(hazard)을 밟았을 때 피해·복귀 동작이 있는지(A04 도랑과 비교. A04 노트는 "피해 1 후 도랑 입구로 복귀"를 가정). (2) draft4 설계용 수치: 평지에서 일반 점프로 올라설 수 있는 최대 턱 높이, 대시 수평 거리(지상·점프 중), 대시 중 높이 변화. 각 3회 측정 | `Validation/B20-trench.md`에 (1) 판정과 원문, (2) 수치 표(최소/최대/평균). 위험 지형 복귀 동작이 없으면 "없음"과 관련 코드 위치를 적는다 |
| T3 | verification | **점프 머리 부딪힘 측정(변경 없음).** V5 Scene 6개에서 서 있을 수 있는 모든 면(솔리드·발판 윗면, 가장자리 양옆 0.35 포함, 0.25 간격 샘플)마다 위쪽 막는 콜라이더까지의 세로 여유 c를 잰다. 몸 높이 h=1.6/1.3/1.1 각각에 대해 분류: (a) c < 2.2(기존 FAIL 기준) (b) c < h + 짧은 점프 높이(짧은 점프도 부딪힘) (c) c < h + 풀 점프 높이(풀 점프만 부딪힘) (d) 부딪히지 않음. 방별·면별로 묶어서 센다(샘플 수가 아니라 면 수). 또 (b)·(c) 중 방마다 최대 2곳을 실제 exe에서 풀 점프로 확인해 머리 접촉 프레임과 상승 속도 변화를 기록한다. `PlayerMotor`에 머리 모서리 보정(코너에 걸리면 옆으로 밀어 주는 처리)이 있는지 읽기만 하고 적는다 | `Validation/B20-headbump.md`에 h별·방별 면 수 표와 면 목록(방, 면 종류·번호, x범위, 윗면 y, c). 실측 샘플은 예측과 일치/불일치를 적는다. 판정은 하지 않는다(MEASURE). 참고: Claude의 데이터 대략 계산은 h=1.6에서 약 22면(짧은 점프 16면)이며 A02·A03에 몰림 |
| T4 | feature | **캐릭터 크기 비교 도구.** (1) `Assets/Scripts/Dev/`에 런타임 크기 덮어쓰기 컴포넌트를 만든다(사전 결정대로 발 고정 균일 축소, 1.6/1.3/1.1). (2) 새 Scene `BodySizeTest`를 생성기로 만든다: 평지, 머리 위 막대 6개(여유 2.2/2.6/3.0/3.5/4.0/4.5, 숫자 표시), 작은 발판 계단(간격 1.0~1.5, 오름 1.5~2.0, 5개), 등불 포고 1개, 서 있는 E1 1마리. 이 Scene에서 키 하나로 크기를 순환하고 현재 높이를 화면에 표시한다(기존 입력 맵과 겹치지 않는 키, README에 적음). (3) V5 런처에 크기 인자를 추가해 실제 방에서도 비교할 수 있게 한다: `Play-V5-Body13.cmd`, `Play-V5-Body11.cmd`(A01 시작), `Play-BodySize.cmd`. 기본 `Play-V5.cmd` 동작은 바꾸지 않는다 | 컴파일 0 오류. 세 크기 각각에서 BodySizeTest: 바닥 접지 판정 정상(서 있기·걷기·점프 착지 각 10회), 막대 아래 풀 점프 시 부딪히는 막대가 T3 분류와 일치, 포고 1회 성공, E1 타격 성공 여부와 공격 사거리 기록. 실제 exe로 세 런처 각 10초 Error0. Release 빌드에 컴포넌트가 없음 확인. 접지·벽 판정이 `PlayerMotor`의 고정 수치 때문에 축소 시 깨지면 BLOCKED, 해당 값·줄을 기록한다. `Validation/B20-bodysize.md` |
| T5 | feature | **지름길 장벽 검사 추가(근거 있는 규칙).** `Tools/Check-RoomRules.py`에: (1) world 문 `type: shortcut`의 반대편(b쪽이 아닌 방) 진입이 "부수기 전 닫힘"으로 표시되지 않았으면 FAIL (2) 문 도착 위치(spawn)가 같은 방의 부서지는 벽·봉인을 이미 지난 쪽에 있으면 FAIL (3) 도랑·구덩이마다 깊이와 T2의 최대 올라서기 높이를 비교해 "능력 없이 빠져나올 수 있음/없음"을 MEASURE로 출력(판정 없음). 근거는 2026-10-22 플레이 로그(A03→A05 통과)와 B20-T1 | 현재 draft3에서 (1)(2)가 A05-A03 **1건씩 FAIL로 잡혀야 한다**(실제 버그이므로 정상; draft4에서 해소 예정). 고친 fixture(반대편 닫힘 표시 + 도착 위치가 벽 바깥)는 PASS. 기존 fixture 4개 재검출 유지, 그 외 새 FAIL 0. `Validation/B19-rules-ledger.md`는 수정하지 않고 `Validation/B20-rules-ledger-add.md`에 새 규칙과 근거 사례를 적는다. `Validation/B20-room-rules-v5-A.md` |
| T6 | doc | TASKS.md·README: (1) 4절 열린 버그에 `V5-A05-SHORTCUT`, `V5-A05-TRENCH`, `V5-HEADBUMP`를 근거(플레이 CSV, B20 결과) 링크와 함께 추가 (2) 2절 결정 대기에 `D-BODY`(캐릭터 높이 1.6/1.3/1.1 중 선택, 사용자 직접 비교 후)와 `D-V5-DRAFT4`(Claude가 draft4 작성 → 다음 배치에서 Scene 반영)를 추가 (3) 3절 직접 플레이에 크기 비교 순서 추가 (4) 9절 인계 갱신 | 결정 대기 8개 이하, 깨진 링크 0 |
| T7 | verification | 마지막 점검: 기본 회귀 13/13 1회, Batch-Card, V5 런처 기존3+새2와 Play-BodySize 시작 로그, 보호 파일 SHA 변경 0(Design·V5 Scene·Player.prefab·PlayerTuning·금지 스크립트) | 허용 범위 밖 0, 깨진 링크 0 |

## Backlog (본 Task가 끝나고 예산이 남으면 위에서부터)
1. T3의 (b)·(c) 면 중 실측하지 않은 나머지도 실제 exe 풀 점프로 확인 — 합격 기준: 예측/실측 일치율과 불일치 목록을 `Validation/B20-headbump-all.md`에 기록, 변경 없음.
2. A01~A04 작은 발판 12개 구간에서 h=1.3·1.1일 때 머리 부딪힘이 사라지는 면 수 — 합격 기준: T3 표와 같은 형식으로 `Validation/B20-headbump-small.md`, 변경 없음.

## 종료 처리
- `Validation/B20-summary.md`: Task별 결과, 유형별 개수, T1 결론(기존 수단 가능/새 코드 필요), T2 수치 표, T3 h별 면 수 요약, T4 크기별 확인 결과와 공격 사거리, T5 새 규칙 결과, 열린 항목, 사용자가 할 일(크기 비교 순서·시간).
- Codex 요약과 로그가 다르면 로그 원문을 우선한다. push는 사용자가 한다.
