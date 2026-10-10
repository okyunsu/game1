이번 실행은 Batch 06만 수행한다. AGENTS.md의 Batch Approval·Git Rule을 따른다.

## Batch 06 — V3 준비: 벽 점프·부서지는 벽·유도 가이드·표지판·이름 변경

- 작성 2026-10-10 / 승인: 사용자(옥윤수)
- 배경: V2 플레이 결과 "맵이 단조롭고 재미없음" → 월드 구조를 V3로 다시 짠다([WORLD_V3.md](WORLD_V3.md)). 이번 Batch는 **레벨을 짓기 전에 필요한 시스템만** 만든다. V3 방 재배선·그레이박스는 Batch 07·08이다.
- 브랜치: 현재 브랜치 `review/batch05-v2`에서 작업한다. **main으로 전환하거나 main에 커밋하지 않는다.**
- 읽을 범위: AGENTS.md, 이 파일, WORLD_V3.md 5~9절, TASKS의 Current Tasks·Latest Handoff, GAME_DESIGN 4·5·11·21절, README의 PogoTest·회귀 실행 절.
- 예산: Task 최대 8개 또는 8시간 중 먼저 도달하는 쪽.

### 사전 결정 (사용자 결정, 2026-10-10)

| 결정 | 답 | 막히면 대체(비우면 멈춤) |
| --- | --- | --- |
| 새 능력 | **벽 점프**를 추가한다(GAME_DESIGN 4절 "벽점프·벽타기 없음" 규칙을 이 결정으로 변경). V3에서 B04 획득 예정 | |
| 벽 점프 가능한 벽 | `ClimbableWall` 컴포넌트가 붙은 콜라이더만. 새 Layer·Physics 설정을 만들지 않는다 | 컴포넌트 판별이 불가능하면 Tag 방식 1회 시도 |
| 더블 점프 위치 | V3에서 C05로 이동. **이번 Batch에서는 기존 V2/기본 Scene의 더블 점프 위치를 바꾸지 않는다** | |
| 부서지는 벽 | 추가한다. 숏컷용(한쪽에서만 부서짐)과 비밀용(금 간 표시) 2종. GAME_DESIGN 11절 "비밀 벽 금지"를 이 결정으로 변경 | |
| 표지판 | 허브에 한 줄 문구. 대화 시스템·선택지 없음 | |
| 엔딩 | V3에서 "텅 빈 정상"으로 변경(Batch 08). 이번엔 문서 기록만 | |
| 엔딩 후 이어하기 | CP-C01에서 저장된 능력으로 재개, 보스 재등장 없음 (Backlog 1) | |
| 게임 이름 | 화면 표시 제목·문서 제목만 **"메트로 1"**. `productName`/`companyName`(Afterglow), exe 이름, 저장 경로, 에디터 메뉴 `Afterglow >`, `CreateAssetMenu` 경로는 **바꾸지 않는다** | |
| 저장 형식 | `ProgressData`에 필드만 추가하고 `version=1` 유지. 필드가 없는 기존 저장은 기본값(false/빈 배열)으로 읽혀야 한다 | 호환이 불가능하면 해당 Task만 BLOCKED |
| 유도 가이드 | ① 목표 한 줄 HUD(기본 켬) ② 필수 출구 등불(기본 켬) ③ F1 디버그 길잡이(에디터·Development Build 전용). 실행 인자 `-no-guide`면 ①②③ 모두 끔 | |

### 허용 범위

- **수정 가능:** `Assets/Scripts/`의 PlayerMotor, PlayerDash, PlayerTuning, RoomSession, ProgressSave, GameHUD, SaveMenu(제목 문자열만), PlayerAttack(부서지는 벽 명중 전달이 꼭 필요한 경우만) / 새 스크립트·Prefab / `Assets/ScriptableObjects/PlayerTuning.asset`(**새 필드 추가만, 기존 값 변경 금지**) / 새 Scene `Assets/Scenes/Test/WallJumpTest.unity` / V2 Scene(`Assets/Scenes/V2/*_V2.unity`)에 표지판·출구 등불 **추가만** / 문서(WORLD_V3.md, BATCH-06.md 커밋, GAME_DESIGN·PRD·README·TASKS 해당 부분)
- **수정 금지:** 기존 기본 Scene(`Assets/Scenes/*.unity`), 원본 Prefab의 기존 값, 기존 Tuning 값(Player·Combat·Boss·Feedback·UI), Input Actions Asset, Packages, ProjectSettings, 기존 Validation 파일, V2 Scene의 지형·적·게이트·CP 위치
- F1·숫자키 같은 테스트 전용 키는 Input Actions Asset을 고치지 말고 `Keyboard.current`로 직접 읽는다(에디터·Development Build에서만).

### Task (트랙 A·C·D·E는 서로 독립, B2는 B1 이후)

**A1. 문서 기록** — 위험 Low
- `WORLD_V3.md`, `BATCH-06.md`(사용자가 저장소 루트에 둔 파일)를 그대로 커밋한다.
- GAME_DESIGN에 결정 ID로 기록: DEC-WORLD-003(V3 구조, WORLD_V3.md 링크), DEC-ABIL-002(벽 점프 추가·B04 / 더블 점프 C05 이동), DEC-LEVEL-002(부서지는 벽·비밀 3곳), DEC-END-002(텅 빈 정상·S01·엔딩 후 CP-C01), DEC-UI-002(유도 가이드), DEC-NAME-001(표시 이름 "메트로 1"). 4·5·8·11·21절 해당 문장 옆에 "V3 적용 예정, 현재 V2/기본은 기존 규칙" 표시. 1절 콘셉트는 WORLD_V3 1절 요약으로 교체.
- PRD: S01은 "엔딩 연출 방"으로 18방 범위 밖에 따로 적는다.
- README·PRD·GAME_DESIGN 맨 위 제목을 "메트로 1"로. 빌드·저장 경로 안내(Afterglow)는 그대로.
- 합격: 문서만 변경, 게임 파일 diff 0.

**B1. 벽 점프 구현 + 테스트 씬** — 위험 High(Player core)
- `PlayerMotor.hasWallJump`(기본 false). `ClimbableWall` 마커 컴포넌트.
- PlayerTuning에 "Wall jump (Prototype Values)" 그룹 추가, 단위·Tooltip·Range 포함:
  - `wallSlideMaxFallSpeed` 2.5u/s (범위 1~5)
  - `wallJumpHorizontalSpeed` 8u/s (6~11)
  - `wallJumpVelocity` 12u/s (10~13)
  - `wallJumpInputLock` 0.15초 (0.08~0.25)
  - `wallCoyoteTime` 0.08초 (0.04~0.12)
- 동작:
  - 공중 + `ClimbableWall` 벽에 옆면 접촉(법선 |x|>0.7) + 벽 쪽 방향 입력 + 하강 중 → 미끄러짐(낙하 속도 상한 = wallSlideMaxFallSpeed)
  - 미끄러짐 중이거나 벽에서 떨어진 뒤 wallCoyoteTime 안에 점프 → 속도 (벽 반대 방향 × wallJumpHorizontalSpeed, wallJumpVelocity). wallJumpInputLock 동안 수평 입력 무시. 기존 점프 컷(버튼 떼면 낮은 점프)은 똑같이 적용
  - 벽에 붙는 순간 공중 대시 1회·더블 점프 1회 회복
  - 점프 소비 순서: 접지/코요테 → 벽 점프 → 더블 점프 → 착지 버퍼. 한 입력으로 두 점프가 나가지 않는다
  - 대시 시작 시 미끄러짐 해제. 미끄러짐 중 좌우 공격 방향은 벽 반대쪽
  - 위로 기어오르기·천장 매달리기 없음. 일반 벽·DashGate·보스 아레나 벽에는 붙지 않음
  - 일시정지·방 전환·리스폰·입력 초기화 때 벽 상태 초기화
- 능력 연결: `RoomSession.GrantWallJump()`, `WallJumpPickup`(DoubleJumpPickup과 같은 방식), 저장 필드 `wallJump`, HUD 아이콘 + 획득 문구 "벽 점프 획득 — 표시된 벽에 붙어 Z"(패드: A).
- 테스트 씬 `WallJumpTest`: PogoTest와 같은 옵트인 방식(에디터 메뉴 `Afterglow > Open Wall Jump Test`, exe 인자 `-walljumptest`, Build Settings·타이틀 메뉴 미추가, 저장 안 씀). 시작 시 대시·벽 점프 보유. 배치:
  - 굴뚝 3개: 폭 3u / 4u / 5u, 높이 12u(양쪽 표시 벽)
  - 한쪽 벽 → 반대편 발판으로 건너기
  - 벽 점프 → 공중 대시 연결 구간
  - 벽 점프 → 포고 표적 연결 구간
  - 대조군: 표시 없는 일반 벽, DashGate 옆 벽(붙으면 안 됨)
  - 더블 점프 획득 오브젝트 1개(벽 점프 + 더블 점프 조합 비교용)
  - 안전 바닥만 두고 낙사·적 없음. 화면 위쪽에 조작 안내 한 줄
- 합격 기준(30/60/120fps):
  - 4u 굴뚝 12u를 벽 점프만으로 오름
  - 미끄러짐 최대 낙하 속도 ≤ 2.5u/s(+0.05 허용)
  - 일반 벽·DashGate 벽에서 미끄러짐 0회
  - 벽 점프 후 0.15초 안에 같은 벽 재부착 0회
  - 한 입력으로 점프 2회 발생 0회
  - 벽 점프 미보유(기본 false)일 때 기존 동작 변화 없음
- 회귀: `Validation/Run-Regression.ps1 -Tag B06-B1` 1회(기본13 + RouteAll + PogoTest). 이미 알려진 간헐 실패 P03(S3T01-POGO)은 기준 비교로만 판단하고 새 회귀로 쓰지 않는다.
- 상태: 기술 PASS면 **REVIEW**(손맛은 사용자 판단).

**B2. 벽 점프 후보 프리셋(테스트 씬 한정)** — B1 이후, 위험 Low
- WallJumpTest에서 숫자키 1/2/3으로 런타임 프리셋 전환. 화면에 현재 프리셋 이름 표시. **Asset 값은 바꾸지 않는다.**
  - 1 기본: 2.5 / 8 / 12 / 0.15
  - 2 끈적: 1.5 / 7 / 12 / 0.20
  - 3 빠름: 3.5 / 9.5 / 12.5 / 0.12
  - (순서: slide / horizontal / velocity / inputLock)
- 합격: 키 전환 후 B1 합격 기준 중 "4u 굴뚝 오르기"가 세 프리셋 모두 PASS.

**C1. 부서지는 벽** — 위험 Medium
- `BreakableWall`: 공격 명중 횟수 `hitsToBreak`(기본 3, Inspector), `breakableFrom`(Left / Right / Any), 고유 `wallId`. 기존 HitFeedback 재사용.
- 부서지면 영구 저장(`ProgressData.brokenWalls` 문자열 배열). 방 재입장·사망·이어하기에서도 열린 상태. 새 게임에서 초기화.
- Prefab 2개: `BreakableWall_Shortcut`(잠긴 쪽에서는 일반 벽과 같은 모양, 부술 수 있는 쪽에만 작은 표시), `BreakableWall_Secret`(금 간 무늬).
- WallJumpTest 바닥 구역에 두 종류 1개씩 배치.
- 합격: 3회에 부서짐, 금지 방향 공격은 피해 0, 재입장·사망·이어하기 후 유지, 새 게임 초기화, 포고 표적으로 오인 없음.

**D1. 유도 가이드 ①②③** — 위험 Medium
- 공통: `-no-guide` 인자면 ①②③ 모두 끔. 진행 단계 표는 ScriptableObject `GuideObjectives`(조건 → 문구 → 목표 방 ID)로 만들고 하드코딩하지 않는다.
- ① 목표 한 줄: 화면 위 가운데, 작은 글씨. 이번에는 **현재 V2 기준 표**로 채운다:
  - 대시 없음 → "대시를 찾아라 — 하부 회랑 깊은 곳" (A05)
  - 대시 있음·더블 점프 없음 → "격자를 지나 동력층으로, 더블 점프를 찾아라" (B04)
  - 더블 점프 있음·보스 생존 → "높은 턱을 넘어 탑 꼭대기로" (C06)
  - 보스 처치 → 문구 없음
  - V3 표(WORLD_V3.md 9-1절)는 Batch 07에서 교체한다.
- ② 필수 출구 등불: `RoomExit`에 `mainPath` 표시(bool)를 추가하고, 켜진 출구 위에 작은 등불 스프라이트를 띄운다. V2 18방의 필수 경로 출구에만 켠다. 출입구 ID·연결은 바꾸지 않는다.
- ③ F1 디버그 길잡이(에디터·Development Build 전용): F1로 켜고 끔. 현재 방 ID, 진행 단계, 다음 목표 방, 그 방까지 최단 경로의 다음 출구 방향 화살표(화면 가장자리). 방 연결 그래프는 에디터 도구로 빌드 Scene의 `RoomExit`를 읽어 Asset으로 생성(능력 게이트는 무시).
- 합격: 새 게임 → 대시 → 더블 점프 → 보스 처치 순서로 ① 문구가 바뀜, `-no-guide`로 ①②③ 0개 표시, Release 빌드에서 F1 무반응, ②는 필수 출구 수와 등불 수 일치.

**E1. 표지판** — 위험 Low
- `SignPost`: 플레이어가 2u 안에 오면 머리 위에 한 줄 표시, 벗어나면 사라짐. 문구는 Inspector. 전투·입력에 영향 없음.
- V2 허브에 배치(지형 변경 없이 추가만):
  - A03_V2: "여기선 칼을 뽑지 않는다. 오래된 약속이지." / "위로 간 사람들은 편지를 보내지 않아."
  - B01_V2: "관문은 곧 열린다. 다들 그렇게 말한 지 몇 해째야." / "아래로 내려가는 놈들? 낙오자지."
  - C01_V2: "여기까지 와서 멈춘 이유? 위를 본 적이 있거든." / "정상에 가면 뭘 할 거냐고? …글쎄."
- 합격: 6개 모두 표시·숨김 동작, 적·공격 판정 영향 0.

**E2. 표시 이름 변경** — 위험 Low
- 타이틀 화면 제목 "Afterglow" → "메트로 1". 엔딩 문구는 이번에 바꾸지 않는다(Batch 08).
- 합격: 타이틀에 "메트로 1" 표시, `productName`·저장 경로 변경 0(ProjectSettings diff 0).

**F1. 전달 빌드** — 모든 Task 이후
- `Builds/B06-delivery/`에 V2 전체 빌드(Development Build, ③ 확인용) + 실행기 3개:
  - `Play-V2-Guide.cmd`: V2 + 가이드, 별도 QA 저장 슬롯
  - `Play-V2-NoGuide.cmd`: 같은 빌드 + `-no-guide`, 별도 QA 저장 슬롯
  - `Play-WallJumpTest.cmd`: `-walljumptest`
- 합격: 세 실행기 모두 기동, 런타임 Error/Exception 0, 새 게임→A01 시작.

### Backlog (위에서부터, 예산이 남으면)

1. **엔딩 후 이어하기 → CP-C01** — 합격: 엔딩 도달 저장 → 이어하기 → C01_V2 시작, 저장된 능력 유지, C06 재진입 시 보스 없음·출구 열림. 보스 처치 상태를 알 수 없어 보스가 다시 나오면 BLOCKED로 기록.
2. **Pause 메뉴 "목표 표시" 토글** — 합격: 켜고 끄기가 ①에만 적용, 기존 재개·타이틀 선택 동작 회귀 0.
3. **WallJumpTest 측정 로그** — 각 프리셋에서 굴뚝 3개 오르는 데 걸린 시간·벽 점프 횟수를 화면 오른쪽 아래에 표시(테스트 씬 한정).

### 종료 조건

위 Task·Backlog 전부 완료·BLOCKED, 또는 예산 소진.

### 인계 (Latest Handoff에 추가)

- **직접 플레이 확인** 순서:
  1. `Play-WallJumpTest.cmd` 10분: 프리셋 1/2/3 손맛 비교 → 하나 선택
  2. `Play-V2-Guide.cmd` 10분: 목표 문구·등불·F1이 길 찾기에 도움이 되는지, 표지판이 읽히는지
  3. 부서지는 벽 2종이 눈에 구분되는지
- **결정 질문지**: 벽 점프 프리셋 선택(1/2/3/직접 조정), 가이드 문구 톤 유지/수정.
- 다음 Batch 후보: Batch 07(연결 재배선 G-W·G-J·구덩이·숏컷, A지역 그레이박스, 더블 점프 C05 이동, V3 목표 표 교체).
