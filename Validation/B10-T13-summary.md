# B10-T13 회귀·전달

상태 PARTIAL. 기본13·RouteAll60·Pogo60 PASS, V2 W01~W21 PASS 뒤 W22 실제 사망, W23 미실시. Development 전달 한 개 생성, 새 게임/시험 코스/실제 대시 후/벽 점프 후 실행기 smoke PASS. 보스 전·엔딩 후 자연 저장이 없어 해당 두 실행기는 미완료 상태를 안내하고 종료한다. 실제 저장 대신 fixture를 만들지 않았다.

## 실제 회귀

`Validation/Run-Regression.ps1 -Tag B10-final` 1회 실행. 원본은 `_raw/B10/regression/REG-B10-final-summary.txt`와 같은 폴더의 각 evidence 파일에 보존. 전체15건 PASS. 기존 V2 입력 라이브러리로 current native Release r63 전체 실행 1회:

```text
FAIL frame-observed actual death V2-W22
MEASURE V2-W22 goal=Ending ticks=285 scene=C06 pos=(13.45, 2.53)
FAIL System.Exception: V2-W22 fixed replay continuous Ending
Error/Exception/Assert=0
```

원본 `_raw/B10/T13-resume-r63-v2-fixed.txt`. T8에서 같은 HEAD의 이전 빌드2회 PASS와 비교해 빌드 구성으로 범위를 좁혔으나 원인 커밋 미확정. 조건부 핵심 수정0. 입력 경로를 잘못 지정했던 앞선 도구 호출은 제품 회귀 근거에서 제외했다.

## 전달

`Builds/B10-delivery/Afterglow.exe` Development, 2026-10-12 18:06 생성. 원본 `_raw/B10/B10-delivery-20261012-180618.txt`:

```text
Succeeded errors=0 warnings=0
```

현재 게임 Scene/튜닝을 담으며 r74 기준 opt-in 검사 도구 포함. 뒤의 r75~r78 변경은 추가 검색/자연 접근 계획만 수정했고 게임 코드/Scene/튜닝 변경0. 마지막 자연 후보 입력 계획은 Release r78에서만 시도했으며 배포 게임 동작 차이 없음.

`Launchers/B10/` 새 실행기6개, B09 실행기 그대로 유지. 새 게임은 A01, Mechanics는 시험 코스. 실제 저장의 이어하기는 타이틀에서 **이어하기**를 선택한다. 실행할 때 선택한 snapshot을 이 실행기 전용 슬롯에 복사하므로 반복 실행은 같은 단계부터 시작한다. 각 슬롯의 저장은 Build의 slots/ 아래에 분리한다.

| 실행기 | 실제 시작 상태 | smoke 결과 |
|---|---|---|
| Play-V4.cmd | A01 새 게임 | PASS, M키21방/방문갱신 포함 |
| Play-V4-Mechanics.cmd | MechanicsTest | PASS |
| Play-V4-AfterDash.cmd | 자연 저장 CP-A03, 대시 있음 | PASS, 능력복원/착지 |
| Play-V4-AfterWall.cmd | 자연 저장 CP-B01, 대시+벽 점프 | PASS, 능력복원/착지 |
| Play-V4-BeforeBoss.cmd | 자연 저장 미확보 | 실행 불가 안내; 검사 미실시 |
| Play-V4-AfterEnding.cmd | 자연 저장 미확보 | 실행 불가 안내; 검사 미실시 |

원본 `_raw/B10/T13-delivery-{NewGame,Mechanics,AfterDash,AfterWall}.txt`, 모두 Error/Exception/Assert=0. 실제 snapshot 복사본 `Launchers/B10/saves/AfterDash.json` / `AfterWall.json`, 원본 자연 로그의 저장줄로 출처 확인 가능. 최초 빈 slot 호출은 예상대로 snapshot 읽기 FAIL, 복사 후 위 실제 smoke 성공과 구분했다.

미실시: 자연 보스 전/엔딩 후 저장·그 두 실행기 smoke, V2 W23(선행 사망), 사용자 감각 수락 및 Sprint Gate. push0, 기본·V2·V3 Scene/Tuning·productName·실제 사용자 저장 경로 변경0.
