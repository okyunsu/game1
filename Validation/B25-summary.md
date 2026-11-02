# Batch25 — Unity 차단 종료·독립 도구 완료

종료 카드(T18 `52259cf` 기준): 변경5파일(+222/-53줄), Scripts/Scenes/Prefabs/Editor/바이너리0, 허용 범위 밖0, 결정3·버그5·깨진 TASKS 링크0·없는 빌드/런처0. 이 요약을 추가한 최종 카드는 `Validation/_raw/25/batch-card-final.txt`에 보존한다. 카드의 과거 REGRESSION은 이번 통과 증거가 아니다.

## 결과와 재개

**새 플레이 가능 범위는 늘지 않았다.** Unity Preflight exit198로 제품 구현·Scene 생성·게임 검증을 시작하지 않았다. 독립 작업을 소진해 DONE4/PARTIAL1/BLOCKED20, 총25 Task로 종료한다(상한32). 밤샘 분량을 Unity 실패 반복으로 채우지 않았다. Batch 밖 Ready Queue0.

사용자 2026-11-02 B25 실행 지시 및 `Design/art_v1/` 선행 커밋 포함 직접 승인. Claude 설계 변경 `e6b5172`: draft6 AB12방 데이터·WORLD18절·직접 제작 아트·B25 지시문·D60. 원본 그대로 커밋했고 이후 Design 변경0. 실행 Base `e6b5172`, 이전 B23/B24 33커밋 보존. fetch/push/squash 없음. B25 태그는 차단 종료 위치를 가리키며 제품 완성을 의미하지 않는다.

Hub를 로그인 상태로 실행·유지한 뒤 환경 변경 근거로 새 Preflight를 실행한다. T1/T2→T4~T12→T13 import/T14~T17→Q1~Q5 순서, 완료 T0/T3/Q6 반복 불필요. T18은 새 결과로 갱신한다. 기존 B24/B23 빌드·런처·저장 슬롯은 보존했다. B25 빌드/Graybox 런처는 없다.

## Task별 결과

| Task | 상태·커밋 | 결과 |
| --- | --- | --- |
| T0 | DONE `b52f24f` | TASKS2-1에 사전 결정5행·실행 승인/출처 구분. B24 결정2건은 안 선택 완료, 제품 반영 대기 |
| T3 | DONE `8968b29` | 증기 y0→top 정적 연결·self-test. AB FAIL0/WARN55→47, 목표 도달 WARN8 해소 |
| T13 | PARTIAL `2d9d3a8` | PNG18 raw 스테이징·해시 일치·RESOURCES 출처 등록. Unity import/Assets 복사/.meta 미실시 |
| Q6 | DONE `d9d782d` | 기존 사람 플레이 로그를 방별 체류 CSV로 집계. 실제 과거 로그·중단/일시정지/재방문·입력/덮어쓰기 보호 검증 |
| T18 | DONE `52259cf` | README·TASKS0/3/9에 현재 빌드·재개·미래 검수30~40분·한계 안내 |
| T1/T2 | BLOCKED | 승강기 수정/왕복3회·V4 B01/C01, 획득대 sink/일반점프10회 미실시 |
| T4/T5/T6 | BLOCKED | AB 생성기·12방 생성·Scene/요소 수·층 B 지도 미실시. T5 정적 부분만 T3에서 수행 |
| T7~T12 | BLOCKED | 첫 통과/포고/보스/벽 재방문·첫4조각HP5·확장 고리·Continue 미실시 |
| T14~T17 | BLOCKED | 아트 표시층·12방 적용·24장/목업 비교·빌드·전체 회귀·smoke 미실시 |
| Q1~Q5 | BLOCKED | 사운드/입자·보스 카메라/정지·적 예고·12방 자동경로 묶음 미실시 |

## 원문과 직접 검증

Preflight [원본](_raw/25/preflight-20261102-165715-327-dff360f2/preflight.txt)·동일 폴더의 Unity 로그/JSON:

```text
MEASURE licensingClients=0 hubLicensingClient=False
WARN no Unity Hub licensing client running; agent-launched Unity previously failed with exit198 in this state. Open Unity Hub (signed in) and keep it running.
MEASURE unityExitCode=198
FAIL Unity startup exit=198; Unity tasks BLOCKED(external dependency); do not retry unchanged environment; non-Unity tasks may continue
MEASURE likelyCause=no Unity Hub licensing client; user action: open Unity Hub signed in, keep it running, then resume
```

정상 사용자 LOCALAPPDATA에서 샌드박스 밖 실행, 종료198. 같은 환경 재시도0. 시작 Git clean·HEAD e6b5172·로컬 origin/main 대비34커밋. Hub 프로세스 수만으로 판정하지 않았다.

T3 [수정 전](_raw/25/T3-before.md)→[수정 후](_raw/25/T3-after.md), 같은 AB draft6 JSON/규칙에 증기 연결만 추가:

- 전체12방 데이터 FAIL0 유지, WARN55→47. B02 reachability2개·B03 reachability6개 해소. 잔여47개는 A01 5/A03 29/B01 4/B02 1/B03 4/B05 4. 새로 생긴 데이터 FAIL/WARN0.
- `PASS B25 steam bottom-to-top / absent and remote jet rejection / mixed lift-steam fixtures`.
- `PASS B23 approach threshold/solid exclusion and bidirectional lift fixtures`. 기존 음성 fixture의 `FAIL synthetic ...`·`FAIL B21-fixture-...` 원문도 보존했으며 실제 방 데이터 실패와 구분한다. self-test assert 실패0.
- 증기는 x0~x1 폭의 y0/top 가상 끝점을 연결한다. 주기·실속도·벽·머리 접촉은 계산하지 않는다. 정적 후보가 실제 완주를 증명하지 않는다. 새 max-step 측정 JSON이 없어 trench-escape MEASURE 미실시.

실행 Python: `C:/Users/gosu3/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe`.

```powershell
& 'C:/Users/gosu3/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe' -B Tools/Check-RoomRules.py Design/v5/rooms_v5_AB.json --v5 --self-test --world Design/v5/world_v5_AB.json --self-test-output-dir Validation/_raw/25/T3-after-fixtures --output Validation/_raw/25/T3-after.md
```

T13 [manifest](_raw/25/art-import-manifest.json): PNG18(타일16·배경2), 원본/스테이징SHA256 불일치0. 직접 제작·외부 다운로드0·생성 스크립트 재실행0. 16 PPU/Point/무압축은 **예정 설정**이며 적용 PASS가 아니다. Unity .meta 생성 후 함께 커밋해야 하므로 Assets 안에 미완성 파일을 넣지 않았다.

Q6 [검증 원문](_raw/25/Q6-checks.txt)·[실제 샘플 출력](_raw/25/Q6-human-room-times.csv):

```text
PASS pause excluded; repeated room visits combined; clean final visit included
PASS truncated session lower bound flagged incomplete
PASS missing exit flags estimated visit incomplete
PASS malformed/backwards active time rejected
PASS existing output preserved; overwrite exits 1
MEASURE historical human sample rooms=4 visits=8 active_seconds=95.926496; not B25 gameplay evidence
```

기존 `Playtest/20261022-233114.csv`를 입력해 기본 출력 `Playtest/20261022-233114-room-times.csv` 생성. 일시정지 제외·방 재방문 합산·정상 마지막방 포함. 비정상 종료는 마지막 기록까지 하한, 출구 누락은 다음 입장까지 추정으로 incomplete 표시. 새 게임에서만 기록을 시작하는 기존 런타임 조건은 유지한다. Continue/fixture 자동 기록은 이번 도구 범위 밖이다. 과거 샘플 시간을 B25 고리 시간으로 사용하지 않는다.

## 미실시와 범위

Unity 컴파일·핵심 동작·Scene 검사·단계별 Quiet·전체13회귀·Development 빌드·런처 smoke·스크린샷·사람 검수 모두 미실시다. Unity 실행 경로가 차단됐고 게임 코드·에셋 변경0이라 회귀 스크립트 재호출도 하지 않았다. B24 통과 증거는 비교 기준만 유지한다.

재개 Scene 검사: Unity `C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe`, `-batchmode -quit -projectPath <저장소> -executeMethod V5Batch18.Clearance -verify-b18-clearance -verify-output <새 raw 출력> -logFile <새 raw 로그>`, 대상 `Assets/Scenes/V5/*.unity`. AB12방 재생성 뒤 같은 규칙 검사로 B24 기준/신규 Scene 결과를 분리한다. 현재 대상은 여전히 기존 A6방이며 B방 Scene은 생성하지 않았다. exe 인자만으로 V512방 검사를 했다고 기록하지 않는다.

Ruling: T13은 RESOURCES 등록·raw 스테이징만 보존 / Unity .meta 수동 작성·미완성 에셋 커밋을 피하면서 원본 출처와 import 재개 정보를 남김 / 틀렸을 때 비용은 PNG18개 재복사이며 원본·제품 영향 없음. Q6는 기존 관찰 CSV를 이용 / no-unity 승인 행에서 게임 코드 없이 사람 플레이 체류 측정 가능 / 잘못된 로그는 실패 또는 incomplete로 표시, 원본 보존.

## 종료 확인

Task 커밋 뒤 Batch-Card와 별도 git status/diff·보호해시·로컬 링크 확인을 수행했다. 실행 Base 이후 허용 변경은 `Tools/*`, TASKS/README/RESOURCES, 새 Validation 요약뿐이다. 초기 승인 Design33파일 커밋은 별도 선행 커밋으로 분리했다.

- [직접 검사](_raw/25/final-checks.txt): 보호134파일 SHA256 변경0, Design/Assets/ProjectSettings/PRD/GAME_DESIGN diff0. TASKS/README/RESOURCES/종료 요약 로컬 링크 누락0. B25 25개 Task 고유 ID·DONE4/PARTIAL1/BLOCKED20 확인.
- `git diff --check` 통과. T18 커밋 후 미커밋은 이 종료 요약1개뿐이며 최종 요약 커밋 후 별도 status/diff를 재확인한다. raw 로그/PNG/CSV/JSON은 Git 제외로 보존했다.
- 최종6파일 변경(도구2·문서3·이 요약1), 코드/에셋/빌드 변경0. 이전33커밋+승인 설계1+이번 Task5+종료1=로컬 origin/main 대비40커밋/3 Batch(B23/B24/B25), fetch 없음. 과거 커밋이 포함되어 soft reset/squash 조건 불충족. 최종 HEAD는 `git log -1`·B25 태그로 확인하며 push하지 않았다.
