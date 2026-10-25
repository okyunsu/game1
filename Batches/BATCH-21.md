이번 실행은 사용자 승인 Batch 21(A05 지름길 잠금 구현, draft4 설계용 실측, 머리 걸림·도랑 후보 동작 2종)만 수행한다. AGENTS.md를 따른다. push는 하지 않는다. **Design/·기본 수치 Asset·Player.prefab은 바꾸지 않는다. 후보 동작은 기본값 꺼짐이다.**

## 목적
1. **A05 지름길(V5-A05-SHORTCUT):** draft3.1 데이터 계약(`closed_until_wall_broken`, `breakable_side`)을 생성기·런타임에 연결한다. A03 쪽 문은 벽을 부수기 전까지 닫히고, 벽은 A05 안쪽(오른쪽)에서만 부서진다.
2. **draft4 설계 근거:** Claude가 draft4(도랑·머리 여유)를 쓰는 데 필요한 수치를 잰다. 가로 점프·대시 점프 한계, 도랑 폭별 통과, 머리 걸림 36면의 부족한 여유.
3. **후보 2종(기본 꺼짐):** 사용자가 직접 비교할 수 있게 만든다. (a) 위험 지형에 닿으면 마지막 안전 지점으로 복귀 (b) 점프 중 머리가 모서리에 살짝 걸리면 옆으로 밀어 주는 보정. 채택은 사용자 결정이다.

## 읽을 파일
AGENTS.md, TASKS.md(0·2·4·9·10절), `Design/v5/WORLD_V5.md`(10~12절), `Design/v5/rooms_v5_A.json`, `Design/v5/world_v5_A.json`, `Validation/B20-summary.md`, `B20-shortcut.md`, `B20-trench.md`, `B20-headbump.md`, `B20-headbump-all.md`, `B20-bodysize.md`, `B20-rules-ledger-add.md`, `Tools/Check-RoomRules.py`, V5 생성기(`Assets/Editor/B09Authoring.cs`), `RoomExit.cs`, `V4ExitRule.cs`, `BreakableWall.cs`, `ProgressSave.cs`, `DamageBlock.cs`, `PlayerHealth.cs`, `PlayerMotor.cs`.

## 시작 조건
- 작업 트리 변경이 `Batches/BATCH-21.md`(미추적), `Design/v5/rooms_v5_A.json`, `Design/v5/WORLD_V5.md`뿐이어야 한다. 그 외 변경이 있으면 시작하지 않고 멈춘다.
- 위 세 파일만 먼저 커밋한다("docs: V5 draft3.1 shortcut lock contract and Batch 21 instructions"). 이 커밋 직후 HEAD가 Base다. 이 커밋의 Design 변경은 **Claude 설계 변경**(지름길 잠금 데이터 계약 추가, 좌표·수치 변경 없음)으로 Task별 결과 첫 줄에 기록한다.
- HEAD가 `B20` 태그를 포함해야 한다. push 안 된 이전 커밋(B20·OPS·c5e229b)이 있어도 정상이다.
- `Tools/Preflight.ps1 -Batch 21`을 샌드박스 밖에서 실행한다(기본 timeout 300초). PASS가 아니면 AGENTS Environment대로 needs-unity=yes Task만 BLOCKED로 두고 no Task를 계속한다.

## 예산·큐
- 예산 **6시간**, 종료 정리 40분 확보(5시간 20분이 지나면 새 Task를 시작하지 않는다). Task 안전 상한 20개.
- 재시도: 같은 접근은 최초 실패 후 1회(총 2회), Task당 접근 최대 3개.
- Batch Task → Backlog → TASKS Ready Queue 순. Ready Queue는 현재 0건이다.
- 회귀: 코드를 바꾼 Task마다 `Run-Regression.ps1 -Quiet`. PlayerMotor·문 전환을 건드리므로 종료 시 기본 회귀 1회(High Risk).
- push 안 된 다른 Batch 커밋이 origin/main..HEAD에 있으면 합치지 않고 Task 커밋을 보존한다. 태그 `B21`은 마지막 커밋에.

## 허용 범위
수정 가능: `Assets/Editor/**`, `Assets/Scripts/**`(아래 금지 파일 제외), `Assets/Candidates/**`(새 후보 Asset), `Assets/Scenes/V5/**`(draft3.1 재생성만), `Assets/Scenes/Test/B21Arena.unity`(새 측정 Scene, .meta 포함), `Tools/Check-RoomRules.py`, `Tools/fixtures/**`, `Launchers/current/*V5*`, `Validation/B21-*`, `Validation/_raw/21/**`, TASKS.md, README.md(실행 안내만), `Batches/BATCH-21.md`, `Builds/**`.
수정 금지: `Design/`(시작 커밋 제외), `Tools/protected-files.txt` 대상 전부(기본 Tuning Asset, Player.prefab, ProjectSettings, PRD, GAME_DESIGN), V4 Scene·Resources·V4 런처, `MovementTest`·`BodySizeTest` Scene, `RoomSession.cs`, `RoomCameraRig.cs`, 이전 `Validation/B*-*.md`.
`PlayerMotor.cs`는 **T6(모서리 보정 후보)에서만** 수정할 수 있다(사전 결정 참고).

## 사전 결정
| 결정 | 답 | 날짜 | 출처·사용자 승인 근거 | 막히면 대체 |
| --- | --- | --- | --- | --- |
| 데이터 | `Design/v5/` draft3.1(지름길 잠금 표시만 추가, 좌표·수치는 draft3과 같음) | 2026-10-26 | Claude 설계. 지름길 의도는 D-V5-PILOT 승인 설계(world 노트 "A05 안쪽에서만 부서지는 벽") | |
| 잠긴 문 표시 | 기존 능력 gate 문이 닫혀 있을 때와 같은 표시를 재사용. 새 아트 없음 | 2026-10-26 | Claude 설계 | |
| 후보 2종 | 기본 꺼짐. Development 빌드 + 후보 런처에서만 켬. 수치는 에이전트가 정하고 Ruling으로 기록(AGENTS Design Change 후보 작업) | 2026-10-26 | Claude 제안, 이 Batch 실행 승인 | |
| PlayerMotor 수정 | T6에서 후보가 꺼져 있을 때 동작이 기존과 같다는 조건으로만 허용(합격 기준 참고) | 2026-10-26 | 사용자가 이 Batch를 승인해 실행할 때 승인으로 본다 | 조건을 못 지키면 T6만 되돌리고 BLOCKED |
| 몸 크기 | 측정은 기본 1.6 기준. 1.3·1.1은 Backlog | 2026-10-26 | D-BODY 미결정 | |

## Task
| Task | 유형 | needs-unity | 선행 | 내용 | 합격 기준 |
| --- | --- | --- | --- | --- | --- |
| T1 | feature | yes | 없음 | **지름길 잠금 구현.** 생성기가 rooms의 문 필드 `closed_until_wall_broken`(벽 저장 키)을 읽어 그 문을 해당 `BreakableWall.wallId`의 영구 파괴 상태(`ProgressSave.IsWallBroken`)에 연결한다. 방 필드 `breakable_side`(breakable 배열과 같은 순서, `left`/`right`)를 `BreakableWall.breakableFrom`에 연결한다. 문 조건은 `V4ExitRule`에 새 조건으로 추가하고 기존 gate·elevatorSeal 처리는 그대로 둔다 | 컴파일 0 오류. 필드가 없는 문·벽은 기존 동작과 같다(V4 Scene 재생성 없이 V4 런처 10초 Error0, Quiet 13/13). 저장 스키마의 기존 `brokenWalls`를 재사용하고 새 저장 필드는 만들지 않는다 |
| T2 | feature | yes | T1 | **V5 Scene 재생성(draft3.1) + 실제 exe 확인.** 6방 재생성 후 A03·A05의 문/벽 설정만 바뀌었는지 요소 수를 B18 기준과 비교한다. 실제 exe로: (1) 새 게임에서 A03 동쪽 아래 문 접근 시 A05로 넘어가지 않음 (2) A05 안쪽에서 왼쪽으로 공격해 벽 파괴 (3) 그 뒤 A03→A05, A05→A03 양방향 통과 (4) 저장·이어하기 후 유지 (5) 벽의 왼쪽 면을 친 공격은 부수지 못함(fixture로 왼쪽에 세워 확인) | 5항목 PASS 원문. 다른 4방 요소 수·문 14방향 결과가 B18과 같음. 사용자 로그(10/22) 경로 재현 시 우회 0. `Validation/B21-shortcut.md` |
| T3 | tool | no | 없음 | **검사기 보정.** `shortcut-spawn-bypass`는 그 입구에 유효한 `closed_until_wall_broken`이 있으면 FAIL로 세지 않고 MEASURE("잠금으로 해소")로 출력한다. 잠금 표시가 없거나 키가 틀리면 지금처럼 FAIL. fixture와 self-test를 갱신한다 | draft3.1 전체 정적 FAIL 0. 잠금 없는 우회·틀린 키·봉인 우회 fixture는 계속 FAIL로 검출. 기존 fixture 4개와 B20 fixture 결과 유지. `Validation/B21-room-rules.md` |
| T4 | verification | yes | 없음 | **가로 이동 한계 실측(draft4용).** 새 측정 Scene `B21Arena`(평지·간격 가변)를 생성기로 만들어 기본 몸 1.6에서 각 3회: (1) 달리기 점프 최대 간격 (2) 점프 후 정점 부근 공중 대시 최대 간격 (3) 지상 대시 직후 점프 최대 간격 (4) 대시만 최대 간격. 분해능 0.05u, 이분 탐색 | `Validation/B21-gaps.md`에 레시피별 최소/최대/평균 표와 원문. B18의 평지 4.5/4.75u와 ±0.25 안에서 일치하는지 대조(어긋나면 원인 기록). 수치 Asset 변경 0 |
| T5 | feature | yes | 없음 | **후보 (a) 위험 지형 복귀.** `DamageBlock` 접촉 시 기존 피해 1 뒤, 후보가 켜져 있으면 짧게 멈춘 다음 마지막 안전 지점으로 옮긴다. 안전 지점은 고체 바닥에 0.1초 이상 접지한 위치 중 위험 지형 가장자리에서 0.5u 이상 떨어진 곳. 움직이는/무너지는 발판 위는 기록하지 않는다. KillZone(가시·낙하 사망)은 바꾸지 않는다. HP가 0이 되면 기존 사망 처리. 후보 설정은 `Assets/Candidates/` 새 Asset, 켜는 방법은 Development 실행 인자. 후보 런처 `Play-V5-HazardReturn.cmd` | 꺼짐: B20 T2 도랑 결과 재현(일반 점프 통과·피해 1). 켜짐: A05 도랑에서 일반 점프 반복 시 통과 0회/5회, 각 시도 후 도랑 동쪽 바닥에 복귀. A04 도랑 등불 포고 경로는 통과(복귀가 경로를 막지 않음). `B21Arena`에서 도랑 폭 4.5/5.0/5.5/6.0u × (능력 없음, 대시 보유) 통과 여부 각 3회. Quiet 13/13. `Validation/B21-hazard-return.md` |
| T6 | feature | yes | 없음 | **후보 (b) 머리 모서리 보정.** 상승 중 머리가 위 구조물에 닿았을 때 겹친 가로 폭이 허용값 이하면 겹치지 않는 쪽으로 옆으로 밀고 상승 속도를 유지한다. 허용값은 0.15~0.35u 안에서 정하고 Ruling으로 기록. 설정은 `Assets/Candidates/` 새 Asset(PlayerTuning에 필드 추가 금지). 후보 런처 `Play-V5-CornerFix.cmd`, 두 후보 동시 `Play-V5-Candidates.cmd`. 크기 비교 인자와 함께 써도 동작해야 한다 | **꺼짐일 때 기존과 같음:** B20 T2 수치 재현(풀 점프 2.52002, 짧은 점프 1.00800, 턱 2.5125, 대시 2.87988, 각 ±0.001), Quiet 13/13, 종료 기본 13/13. 켜짐: B20-headbump-all의 36면 실측을 다시 돌려 접촉 수 변화(면별 전/후)를 표로 기록. 벽 점프·포고·대시 동작 각 10회 Error0. 조건을 못 지키면 T6만 되돌린다. `Validation/B21-corner.md` |
| T7 | verification | no | T4 | **머리 걸림 36면의 부족한 여유(draft4용, 측정만).** B20-headbump-all의 h1.6 접촉 36면마다: 서 있는 면 윗면 y, 위 구조물 아랫면 y, 현재 여유, 풀 점프 기준 필요 여유(1.6+2.52+0.1=4.22u) 대비 부족분, 위 구조물을 부족분만큼 올렸을 때 그 구조물로 올라가는 오름이 2.0u를 넘는지, 방 천장과 겹치는지. 수정안은 쓰지 않고 수치만 | `Validation/B21-headroom-table.md`에 방별 표. 36면 수가 B20과 일치. Design 변경 0 |
| T8 | verification | yes | T1~T6 | **전달 빌드와 마지막 점검.** Development 빌드를 만들고 V5 기본 3런처(Play-V5, AfterDash, DJ)·크기 2런처·새 후보 3런처를 각 10초 실행. 기본 회귀 13/13, AGENTS Housekeeping의 V5 정적·Scene 검사, Batch-Card(허용 범위 위 목록), 보호 파일 SHA 비교 | 런처 8개 Error0. 후보 런처 외에는 후보 동작 꺼짐 확인. 범위 밖 0, 보호 파일 변경 0, 깨진 링크 0 |
| T9 | doc | no | T8 | TASKS·README: V5-A05-SHORTCUT 상태 갱신, 열린 결정에 **D-HAZARD-RETURN**(채택/미채택), **D-CORNER-FIX**(채택/미채택, 허용값) 추가, 3절 직접 플레이에 후보 비교 순서(각 런처, 볼 것, 시간) 추가, 0절·9절 인계 | 결정 대기 8개 이하, 깨진 링크 0 |

## Backlog (위에서부터, 시간이 남으면)
1. T7과 같은 표를 몸 높이 1.3·1.1로 — 합격 기준: 면 수와 부족분 표를 `Validation/B21-headroom-small.md`에 기록, Design 변경 0. needs-unity=no, 선행 T7, 허용 범위: Validation/B21-*.
2. 후보 (b)의 대시 버전: 대시 중 몸 아래쪽이 모서리에 살짝 걸리면 위로 올려 주는 보정, 같은 후보 Asset에 별도 토글(기본 꺼짐) — 합격 기준: 꺼짐일 때 T6 꺼짐 기준 그대로, 켜짐일 때 B21Arena 턱 모서리 10회 결과 기록. needs-unity=yes, 선행 T6, 허용 범위: T6과 같음.
3. A04 도랑(x22~40) 폭·등불 간격에서 후보 (a)를 켰을 때 능력 없이 섬 보상까지 가는 시도 5회 — 합격 기준: 성공 횟수·실패 위치 기록, 지형 변경 0. needs-unity=yes, 선행 T5.

## 종료 처리
- `Validation/B21-summary.md` 맨 위에 Preflight 결과, 회귀, V5 검사, Batch-Card를 붙인다. Task별 결과, 후보 2종의 꺼짐/켜짐 비교 수치, draft4용 수치 요약(T4·T5 도랑 폭 표·T7), Ruling 목록, 미실시 항목, 사용자가 할 일(플레이 순서와 시간)을 적는다.
- 기록은 AGENTS Handoff 순서로 TASKS 0·9절에 남긴다. Codex 요약과 로그가 다르면 로그 원문을 우선한다. push는 사용자가 한다.
