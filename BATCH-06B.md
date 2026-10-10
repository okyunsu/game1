이번 실행은 Batch 06b만 수행한다. AGENTS.md의 Batch Approval·Git Rule을 따른다.

## Batch 06b — 벽 점프 짧은 킥·현장 조절, 엔딩 후 이어하기

- 작성 2026-10-10 / 승인: 사용자(옥윤수)
- 배경: Batch 06 직접 플레이 결과 **프리셋 1(기본)이 가장 좋음**. 다만 **벽 점프 한 번의 최소 이동이 너무 큼**(누르기만 하면 항상 벽에서 멀리, 크게 튕겨 나감). 짧게 튕기는 방법과 현장에서 값을 조절하는 수단을 만든다. V3 레벨 작업(Batch 07)은 이 손맛이 수락된 뒤 시작하므로 **이번에도 V3 레벨·재배선은 하지 않는다.**
- 브랜치: 현재 브랜치 `review/batch05-v2`. main 전환·main 커밋 금지.
- 읽을 범위: AGENTS.md, 이 파일, TASKS Latest Handoff, Validation/B06-B1·B2·F1 요약, WORLD_V3.md 5절.
- 예산: **Task 최대 12개 또는 6시간** 중 먼저 도달하는 쪽. 본 Task 5개 이후 남은 예산으로 Backlog를 진행한다.

### 사전 결정 (사용자 결정, 2026-10-10)

| 결정 | 답 | 막히면 대체(비우면 멈춤) |
| --- | --- | --- |
| 벽 점프 기준값 | **프리셋 1(기본) 채택**: slide 2.5 / horizontal 8 / velocity 12 / lock 0.15 / coyote 0.08. 현재 PlayerTuning.asset 값 그대로이며 DEC-WJ-001로 기록 | |
| 이전 프리셋 2(끈적)·3(빠름) | 폐기. 테스트 씬 숫자키 2·3은 아래 새 후보로 교체 | |
| 짧은 킥 | 점프 순간 **벽 쪽 방향키를 누르고 있으면 짧은 킥**, 중립·반대 방향이면 지금의 긴 킥 | |
| 한쪽 벽 오르기 | 짧은 킥은 잠금이 짧아 같은 벽에 다시 붙을 수 있다(Hollow Knight처럼 한 벽을 타고 오르기 가능). 표시된 벽에만 해당 | |
| 유도 가이드 | **현재 상태로 유지.** Pause 메뉴 "목표 표시" 토글 Backlog는 취소 | |
| 엔딩 후 이어하기 | CP-C01에서 저장된 능력으로 재개, 보스 재등장 없음 | 보스 비활성화가 C06_V2 Scene 컴포넌트 추가만으로 불가능하면 BLOCKED |

### 허용 범위

- **수정 가능:** PlayerMotor.cs, PlayerTuning.cs, `PlayerTuning.asset`(**새 필드 추가만, 기존 값 변경 금지**), WallJumpTestCourse 등 B06 테스트 전용 스크립트, `Assets/Scenes/Test/WallJumpTest.unity`, ProgressSave.cs(PendingScene 경로만), 새 컴포넌트 스크립트, `Assets/Scenes/V2/C06_V2.unity`(컴포넌트 **추가만**), 빌드 스크립트·실행기, 문서(TASKS, GAME_DESIGN 4·5·21절의 결정 기록, README 실행 안내)
- **수정 금지:** 기본 Scene, 다른 V2 Scene, BossEncounter.cs·BossTuning, 기존 Tuning 값, Input Actions Asset, Packages, ProjectSettings, 기존 Validation 파일
- 테스트 전용 키는 `Keyboard.current`로 직접 읽고 WallJumpTest 안에서만 동작한다. 방향키·Z·X·C·Esc와 A/D/W/S(이동 바인딩)는 쓰지 않는다.

### Task

**T1. 짧은 킥 + 붙기 방식 옵션** — 위험 High(Player core)
- PlayerTuning "Wall jump" 그룹에 추가(단위·Tooltip·Range 포함):
  - `wallJumpShortHorizontalSpeed` 기본 **8**(= 긴 킥과 같아서 기본 게임 동작 변화 0), 범위 0~11, u/s
  - `wallJumpShortInputLock` 기본 **0.15**(= 긴 킥과 같음), 범위 0~0.25, 초
  - `wallAttachWhileRising` 기본 **false**. true면 상승 중에도 벽에 붙는다(상승 속도는 유지하고, 미끄러짐 속도 제한은 하강할 때만 적용)
- 동작: 벽 점프 순간 `input.Move.x`가 벽 쪽으로 0.5 이상이면 짧은 킥(Short 값 사용), 아니면 긴 킥(기존 값). 수직 속도와 점프 컷(버튼 떼면 낮게)은 두 킥 모두 같다.
- 기본 Asset 값으로는 기존 동작이 바뀌지 않아야 한다.
- 합격(30/60/120fps):
  - 기본 Asset 값에서 B06 벽 점프 검사 전부 그대로 PASS
  - 짧은 킥 3.5/0.06 설정에서 한쪽 벽(높이 12u)만으로 정상까지 오름, 킥 1회당 벽에서 떨어지는 최대 거리 ≤ 1.0u
  - 긴 킥의 최대 거리는 기존과 같음(±0.05u)
  - `wallAttachWhileRising=true`에서 상승 중 부착 발생, false에서 0회
  - 한 입력으로 점프 2회 0회
- 회귀: `Validation/Run-Regression.ps1 -Tag B06b-T1` 1회. P03 간헐 실패는 기준 비교로만 판단.

**T2. 테스트 씬 후보·현장 조절** — T1 이후, 위험 Low
- WallJumpTest 숫자키 프리셋 교체(모두 프리셋 1 기반, Asset은 바꾸지 않음):
  - 1 기준: 긴 킥만(short = 8 / 0.15), 상승 중 부착 끔
  - 2 짧은 킥: short 3.5 / 0.06, 상승 중 부착 끔
  - 3 짧은 킥 + 상승 부착: short 3.5 / 0.06, 상승 중 부착 켬
- 현장 조절 키(테스트 씬 한정, 런타임 복사본에만 적용):
  - 7 / 8: 짧은 킥 수평 속도 −0.5 / +0.5
  - 9 / 0: 짧은 킥 잠금 −0.01 / +0.01초
  - 6: 상승 중 부착 켜기/끄기
  - 화면 오른쪽 위에 현재 값 전부(긴 킥·짧은 킥·잠금·부착 방식) 표시
  - 값이 바뀔 때마다 `Logs/walljump-tuning-last.txt`에 현재 값 전체를 덮어쓴다(사용자가 고른 값을 그대로 옮기기 위함)
- 한쪽 벽 오르기 구간 1개 추가: 표시 벽 하나, 높이 12u, 반대편 벽 없음, 꼭대기에 착지 발판. 기존 굴뚝·대조군 배치는 그대로.
- 합격: 키 입력마다 표시·로그 값 일치, 프리셋 1/2/3 각각 4u 굴뚝 완등 PASS, 프리셋 2·3은 한쪽 벽 완등 PASS.

**T3. 굴뚝 측정 표시** — 위험 Low
- WallJumpTest 오른쪽 아래: 각 굴뚝·한쪽 벽 구간의 바닥 출발 → 꼭대기 도착 시간과 벽 점프 횟수, 직전 기록 3개. 구간에서 떨어지면 해당 시도 초기화.
- 합격: 자동 등반 1회에서 표시 시간과 로그 시간 차이 ≤ 0.05초.

**T4. 엔딩 후 이어하기 → CP-C01** — 위험 High(저장·진행)
- `ProgressSave.PendingScene`: `endingReached`면 C01(V2 사용 시 C01_V2)로, 체크포인트는 CP-C01. 저장된 능력·부순 벽 유지. 보스 처치 직후·엔딩 전에 종료한 경우는 기존대로 Ending.
- C06_V2에 새 컴포넌트 추가: 시작 시 `ProgressSave.BossDefeated`면 보스 오브젝트 비활성, 잠금 해제 상태. BossEncounter.cs는 수정하지 않는다.
- 합격(격리 QA 저장): 엔딩 도달 → 타이틀 → 이어하기 → C01_V2·CP-C01 시작, 능력 유지 / C06_V2 재진입 시 보스 없음·출구 이용 가능·엔딩 재발생 0 / 새 게임은 기존대로 A01 / `endingReached`가 없는 기존 저장은 기존 동작.

**T5. 문서 + 전달 빌드**
- 문서: DEC-WJ-001(프리셋 1 채택, 짧은 킥·상승 부착은 후보), 가이드 현 상태 유지·Pause 토글 취소, DEC-END-002에 엔딩 후 이어하기 구현 상태 기록.
- `Builds/B06b-delivery/`: Development 빌드 + 실행기 3개(`Play-WallJumpTest.cmd`, `Play-V2-Guide.cmd`, `Play-V2-NoGuide.cmd`). 엔딩 후 이어하기 확인용 `QA-AfterEnding.cmd`(엔딩 도달 저장을 만든 별도 QA 슬롯으로 시작) 추가.
- 합격: 실행기 4개 기동 PASS/Error 0, 보호 검사(기본 Scene·기존 Tuning 값·ProjectSettings·Packages·기존 Validation) 변경 0.

### Backlog (위에서부터)

1. **벽 점프 레벨 메트릭 표** — 기본값과 프리셋 2 기준으로 실측: 긴 킥 1회 최대 수평 거리·최고 높이, 짧은 킥 1회 수평 거리·높이 이득, 굴뚝 폭별(3/4/5/6u) 완등 가능 여부와 횟수, 벽 점프 → 대시로 건널 수 있는 최대 틈. `LEVEL_METRICS_WALLJUMP.md`에 표로 정리(Batch 07 굴뚝·승강로 치수 기준). 합격: 각 값에 원본 MEASURE 줄 링크.
2. **WallJumpTest 패드 안내** — 화면 안내 문구를 마지막 입력 장치에 맞게 키보드/패드로 바꿔 표시. 합격: 두 장치 전환 시 문구 전환.

### 종료 조건

위 Task·Backlog 전부 완료·BLOCKED, 또는 예산 소진.

### 인계 (Latest Handoff)

- **직접 플레이 확인:**
  1. `Play-WallJumpTest.cmd` 10분: 숫자 1/2/3 비교 → 7·8·9·0·6으로 직접 맞춰 보기 → 마음에 드는 값이면 `Logs/walljump-tuning-last.txt` 그대로 두기
  2. `QA-AfterEnding.cmd` 3분: 이어하기가 C01에서 시작하는지, C06에 보스가 없는지
- **결정 질문지:** 짧은 킥 채택 여부와 값, 상승 중 부착 채택 여부, 채택 값을 기본 Asset에 넣을지.
- 다음 Batch 후보: Batch 07(V3 연결 재배선 + A지역 그레이박스 + 더블 점프 C05 이동 + V3 목표 표 교체), 메트릭 표를 치수 기준으로 사용.
