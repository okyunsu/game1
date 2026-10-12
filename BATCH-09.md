이번 실행은 Batch 09만 수행한다. AGENTS.md의 Batch Approval·Git Rule·Validation 규칙을 따른다.

## Batch 09 — 설계 데이터(rooms_v2) → V4 Scene, 오르기 장치 6종 + 내려찍기, 전 연결 왕복 검증

- 작성 2026-10-11 / 승인: 사용자(옥윤수)
- 배경: 방 구조를 사람이 아닌 **설계 데이터 + 물리 검사기**로 만들었다(Claude 작성, 21방 전부 검사 PASS). 이번 Batch는 그 데이터를 Unity Scene으로 옮기고, 새 장치와 능력을 구현하고, 실제 입력으로 검증한다.
- **BATCH-08은 실행하지 않고 이 Batch로 대체한다.** BATCH-08의 문 배치·A02·B03·C03 Task는 데이터가 해결했다. BATCH-08의 T2(연속 전환 방지), T4(양방향 왕복), T5(고립 검사), T7(회귀 원인 진단)은 아래에 다시 포함했다.
- 브랜치: `review/batch05-v2` 유지, main 전환 금지. productName·저장 경로 변경 금지.
- 예산: **10시간 또는 Task 45개**(먼저 닿는 쪽). 방별 검사용 빌드는 Release, Development 빌드는 마지막 전달 1개만(방화벽 팝업 방지).
- 읽을 범위: AGENTS.md, 이 파일, `Design/MECHANICS_V2.md`, `Design/rooms_v2.json`, `Design/world_v7.json`, LEVEL_V3.md(기존 V3 구조 참고), TASKS의 Latest Handoff·Bugs(B07-*), GAME_DESIGN 8절·22절. `Design/room_check.py`는 실행만 하고 고치지 않는다(T1 상수 갱신 제외).

### 데이터 읽는 법 (중요)

- 단위 1 = Unity 1u. 방마다 원점은 방 왼쪽 아래(로컬 좌표). 월드 위치는 `world_v7.json`의 rect.
- 사각형은 모두 `[x, y, w, h]`(왼쪽 아래 기준). 바닥 윗면은 대부분 y=1(B04만 2).
- `solid` 벽·바닥·천장 / `platform` 발판 / `climb` 벽 점프 표시 벽(ClimbableWall) / `seal` 금 간 바닥(내려찍기로만 파괴) / `cover`·`breakable` 공격으로 깨지는 덮개·벽 / `hazard` 피해 구역 / `pad` 내려찍기 발판 / `pogo` 등불·가시 포고 지점 / `lifts` 레버 승강기 / `steam` 증기 기둥 / `crumble` 무너지는 발판 번호(platform 인덱스) / `enemies` 적(`id`가 있는 것은 포고용 E4) / `marks` 표지·획득물·조각·표지판(텍스트로 종류 판단) / `doors` 문(트리거, `id`=연결 ID) / `spawns` 도착 지점(`id`=연결 ID, `fall: true`는 위에서 떨어져 들어오는 도착).
- **문과 도착 ID는 연결 ID 하나로 짝을 맞춘다.** 예: A02의 문 `A02-A03`로 나가면 A03의 spawn `A02-A03`에 도착.
- `routes`는 검사용 경로다. 자동 검증의 경유지 목록으로 쓴다.

### 사전 결정 (사용자 결정)

| 결정 | 답 | 막히면 대체 |
| --- | --- | --- |
| 새 Scene 위치 | `Assets/Scenes/V4/<방ID>_V4.unity`, 실행 인자 `-v4`로 진입. **V3·V2·기본 Scene은 수정 금지**(비교용 보존) | |
| 데이터가 원본 | Scene은 `rooms_v2.json`에서 **생성기(Editor 메뉴)로 생성**한다. 손으로 옮기지 않는다. 다시 생성해도 같은 결과(재생성 가능) | |
| 데이터 수정 권한 | Codex는 원칙적으로 데이터를 고치지 않는다. 실제 측정으로 길이 막히면 **해당 요소만 1u 이내로** `Design/rooms_def.py`에서 옮기고 `python Design/rooms_def.py` → `python Design/room_check.py Design/rooms_v2.json <임시폴더>` PASS 확인 후 재생성. 변경 내역을 표로 기록. 1u를 넘으면 그 방만 BLOCKED | Python 실행 불가면 json 직접 수정 + 기록 |
| 발판 종류 | 기존 프로젝트의 발판 처리 방식을 그대로 쓴다. 검사기는 발판을 단단한 판으로 가정했다(보수적) | |
| 장치 수치 | `Design/MECHANICS_V2.md` 초안 수치. 새 `MechanicsTuning` ScriptableObject에 모아 조정 가능하게 | |
| 포고 | 기존 PogoTarget + CombatTuning.pogoVelocity(11) 재사용. 등불=PogoTarget(충돌 없음), 가시=DamageBlock+PogoTarget | |
| E4 떠다니는 벌레 | 새 적. 체력 2, 접촉 피해 1, 제자리 ±0.5u 상하 이동, 추적 없음. 아래 공격 시 포고 | |
| 내려찍기(Slam) | MECHANICS_V2.md 끝 절 그대로. 입력: **공중에서 아래 + 대시**. `hasSlam` 저장. V4 C06 보스 처치 시 획득 | 입력 충돌 발견 시 아래+공격 길게 누르기로 대체하고 기록 |
| 엔딩 흐름 | C06 처치 → 천장 열림 → S01 엔딩 트리거에서 기본 엔딩 → 타이틀이 아니라 **A01 시작 지점에서 재개**(능력 유지, hasSlam=true). Z01 → 엘리베이터 → S02에는 "무언가 있다" 표지만. S02 도착을 저장 플래그로 기록 | |
| 승강기 초기 상태 | 위에 고정. 위층 레버를 치면 내려오고 이후 양쪽 호출 가능. 상태 저장 | |
| 도착 규칙 | spawn 좌표 그대로. 위에서 떨어지는 도착(`fall`)은 그 좌표에서 낙하 시작 | |
| 연속 전환 방지 | 도착 직후 모든 출구가 플레이어를 무시, 1u 이상 이동하거나 0.4초가 지나면 활성화. **V4에만 적용**(기본·V2·V3 동작 변화 0) | 공용 코드가 기존 동작을 바꾸면 V4 전용 컴포넌트로 분리 |
| 안내(목표 줄·출구 등불·F1) | 기존 V3 안내 시스템을 V4 그래프로 연결만 한다. 새 안내 기능 추가 금지 | 연결이 어렵으면 목표 줄만 |
| 회귀 원인 진단 | T0(efd6406) 직전·직후 커밋을 별도 폴더(git worktree)에서 Release 빌드해 V2 W22 보스 고정 입력과 RouteAll R09를 비교. 현재 작업 트리 변경 금지 | |
| 회귀 수정 | T0 변경이 원인으로 확정되면 기본·V2를 T0 이전과 같게 되돌리는 최소 수정 허용(벽 점프 관련은 hasWallJump=true일 때만 동작) | 확정 못 하면 진단만 기록 |

### Task

**T0. 시작 점검 + 설계 데이터 커밋** — 작업 트리 확인. `Design/` 폴더(사용자·Claude 작성 설계 데이터)를 그대로 커밋한다(`Design/out*`, `__pycache__` 제외). Python이 있으면 `room_check.py` 실행 결과를 Validation에 기록. 합격: 21방 PASS 기록, 커밋 1개.

**T1. 실측으로 검사기 가정 확인** — 테스트 Scene에서 실제 값을 잰다: 제자리 점프 최고 높이, 달리기 점프 최대 거리(같은 높이·1.8u 높이), 대시 점프 거리, 더블 점프 최고 높이, 등불 포고의 튕김 높이(판정 높이 포함), 벽 점프 굴뚝(폭 4) 오르기 성공. 검사기 가정(MEASURE 대비 표)과 비교. 실측이 가정보다 10% 넘게 나쁘면 `room_check.py` 상단 상수만 실측값으로 바꾸고 재검사. 합격: 표 + 재검사 결과.

**T2. 내려찍기(Slam) 구현** — 사전 결정대로. SealFloor 컴포넌트(내려찍기로만 파괴), 디버그로 능력 부여 가능(개발 빌드 전용). 합격: 공중 발동·땅 불가·대시 중 불가, SealFloor는 일반 공격·포고·대시로 안 깨지고 내려찍기로만 깨짐, 기존 아래 공격·포고·더블 점프 입력 변화 0(기존 검증 통과).

**T3. 장치 6종 + E4 구현** — PogoLantern, SpikePogo, SlamPad, SteamVent, LeverLift(저장 포함), CrumblePlatform, E4. `MechanicsTuning` 하나에 수치. 합격: 각 장치 단위 검증(자동 입력)으로 MECHANICS_V2.md 동작 확인. 예: SlamPad는 일반 착지에 반응 0, 내려찍기 착지 시 상승 7.5u±0.3. SteamVent는 꺼짐 상태에서 상승 0. Crumble은 0.6초 뒤 낙하·3초 뒤 복구.

**T4. 장치 시험 코스 Scene** — `MechanicsTest` Scene(메뉴 Afterglow > Open Mechanics Test)에 6종 + Slam + E4를 나란히. 사용자가 손맛을 볼 수 있게. 합격: Scene 열기·통과 자동 검증 PASS.

**T5. V4 Scene 생성기** — Editor 메뉴 `Afterglow > Build V4 From Design`. json의 모든 요소를 위 표에 따라 생성. 방 카메라 경계 = 방 크기. 표지(marks) 텍스트로 획득물 배치: "대시 사당"→DashPickup, "벽 점프 사당"→WallJumpPickup, "더블 점프 사당"→DoubleJumpPickup, "CP"→Checkpoint, "표지판"→SignPost, "조각"→DiscoveryReward, "엔딩 트리거"→엔딩, BOSS→기존 보스, "무언가 있다"→표지만. 매핑 못 한 표지는 목록으로 기록. 합격: 21개 Scene 생성, 생성기 재실행 시 차이 0, 요소 수 일치 표(json 개수 = Scene 개수).

**T6. 전환 전수 검사 + 연속 전환 방지** — 모든 문에 대해 목적지 Scene에 같은 ID spawn이 정확히 1개인지, spawn이 문 쪽 벽인지(가로), 출구와 2.5u 이상 떨어졌는지. 연속 전환 방지 코드(사전 결정). 합격: 위반 0, 출구 앞 도착 + 방향키 3초 유지 시 전환 1회 이하.

**T7. 전 연결 양방향 왕복** — 모든 연결(한 방향 낙하·숏컷은 설계대로 한 방향)을 실제 입력으로: (a) 들어갈 때 누르던 방향키를 도착 후 2초 계속 누름 → 추가 전환 0, 3u 이상 이동. (b) 도착 → 1초 대기 → 들어온 문으로 복귀. 위·아래 문은 벽 점프·더블 점프 등 필요한 능력 상태로. 합격: 전부 PASS, 결과 표.

**T8. 방별 경로 재생** — 각 방 `routes`의 경유지를 순서대로 자동 입력으로 통과(능력 태그 `@dash` `@dj` `@wj` `@slam` `@steam` `@lift` 반영). 실패하면 원인 분류: 데이터(1u 이내 수정 허용) / 구현 / 자동 입력 한계. 합격: 경로 PASS율 표, 실패는 분류와 함께 기록. 자동 입력 한계로 인한 실패는 BLOCKED가 아니라 "수동 확인 필요"로 표시.

**T9. 고립 검사** — 각 방에 그 방에 들어올 수 있는 최소 능력 상태로 들어가서 다른 문으로 나갈 수 있는지(설계상 한 방향 제외). 합격: 갇힘 0. 갇히면 데이터 1u 수정 또는 BLOCKED.

**T10. V4 전체 완주** — 새 게임 → 대시 → 벽 점프 → 더블 점프 → 수문장 → S01 기본 엔딩 → A01 재개 → A01 금 간 바닥 내려찍기 → Z01 → 엘리베이터 → S02. 60fps 필수, 30/120fps 가능하면. 이어하기 4지점(대시 후, 벽 점프 후, 보스 전, 엔딩 후). 합격: 끊김 없이 PASS.

**T11. 회귀 원인 진단 → 수정** — 사전 결정대로(BATCH-08 T7과 동일). 합격: V2 W22 보스 고정 입력과 R09가 B06b 빌드와 같은 결과로 돌아오거나 진단 기록.

**T12. 회귀 + 전달 + 문서 + 인계** — Run-Regression 1회, V2 전체 경로 1회. `Builds/B09-delivery/` Development 1개 + 실행기: `Play-V4.cmd`(새 게임), `Play-V4-Mechanics.cmd`(시험 코스), `Play-V4-AfterBoss.cmd`(hasSlam 상태로 A01), 기존 실행기 유지. `LEVEL_V4.md` 작성(생성기 사용법, 데이터 수정 내역, 장치 목록). Latest Handoff.

### Backlog

1. **사람 플레이 CSV 자동 요약** — `Playtest/*.csv` → 방별 체류 시간, 3초 안에 같은 두 방 3회 이상 왕복이면 "전환 루프 의심". 합격: 20261011-201602.csv에서 A02↔A03 루프 검출.
2. **방 미니 지도(개발 빌드 전용, M키)** — 방문한 방과 현재 위치 텍스트 격자. 합격: 21방 표시·방문 갱신.
3. **장치 수치 후보 프리셋** — 증기 주기 1.5/2.5, 무너짐 0.4/0.8, 승강기 4u/s 후보를 시험 코스에서 F키로 전환(기본값은 바꾸지 않음).

### 실행 규칙

- Task마다 로컬 커밋, push 금지. 기존 Validation은 덮어쓰지 않고 새 파일.
- 같은 방법으로 1회 재시도, 두 번째 실패면 되돌리고 실패 기록만 커밋 후 다음 Task.
- 수정 금지: `Assets/Scenes/V3/*`, V2·기본 Scene, `PlayerTuning.asset` 값, productName·저장 경로, `Design/room_check.py`(T1 상수 제외).
- 위험한 변경 금지: Input System 처리를 Fixed Update로 옮기지 않는다(Pause 중 입력 문제).

### 종료 조건

전부 완료·BLOCKED 또는 예산 소진. V4 수락 전 다른 범위는 착수하지 않는다.

### 인계 (사용자 확인용)

- 직접 플레이 1 (10분): `Play-V4-Mechanics.cmd` — 6종 장치 손맛, 증기 타이밍·무너짐 속도가 답답한지.
- 직접 플레이 2 (25~30분): `Play-V4.cmd` 처음부터. 방 안에서 위·아래 길을 일부러 바꿔 타 보기, 되돌아가기.
- 직접 플레이 3 (10분): `Play-V4-AfterBoss.cmd` — 내려찍기로 숨은 조각 3개, A01 바닥 → Z01 → S02.
- 결정 질문지: 장치 수치 채택, Slam 입력(아래+대시) 유지 여부, 회귀 수정 채택, 마음에 안 드는 방 1~3개.
