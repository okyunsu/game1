# PROCESS_METRICS — PROC-001

## 정의 (고정)

- **실측 시간:** progress 파일 등에 시작·종료 시각이 명시된 경우만. 사용자 대기·중단 시간은 빼지 않고 그대로 두되 비고에 적는다.
- **커밋 활동 구간:** 실측 기록이 없을 때 첫 커밋~마지막 커밋 시각 차이. 실제 작업 시간이 아니며, 생각·대기·중단이 섞일 수 있다. 실측과 같은 열에 쓰되 **활동**으로 표시한다.
- **플레이 가능 기능:** 사용자가 exe 또는 Editor에서 직접 해 볼 수 있는 새 기능이나 완성된 기능. 내부 검사 도구·문서·리팩터링·검증 결과는 세지 않는다.
- **BLOCKED/복원:** 그 실행에서 BLOCKED 처리되거나 되돌려진 Task 수.
- **발견 주체:** 버그·문제를 처음 찾은 쪽. 기존 자동 회귀 / 직접 플레이 QA / 구현 중 Codex 자체 발견 중 하나. 근거가 없으면 기록 없음.

집계 단위: 승인 Task의 독립적인 플레이 결과를 기능 묶음1개로 센다. 예를 들어 공격·포고는1개, 후보 H1~H3 묶음은1개다. 통합 전투·연결 레벨 묶음도 같은 원칙으로 세며, 단계 인계의 Task와 실제 플레이 가능한 산출물로 확인한다. 빌드만 다시 만든 것은 새 기능이 아니다.

BLOCKED와 복원이 겹친 Task는1개만 센다. 종속 산출물은 부모 Task에 포함되면 중복으로 세지 않는다(예: N7 ATK4의 S5-attack). SKIPPED·REVIEW·KNOWN-LIMIT은 BLOCKED로 임의 변환하지 않는다. 명시적으로 별도 BLOCKED 처리한 검증 경로는1개로 센다. 비고의 `[대기]`는 감각 수락 대기를 나타내며 BLOCKED에 자동 합산하지 않는다. 당시 실패의 게임/도구 원인이 확정되지 않은 경우 그 불확실성을 그대로 적는다.

활동 구간은 승인·보존 커밋부터 해당 인계 커밋까지, 별도 실행으로 구분된 기록을 묶었다. 첫 커밋 이전 시간과 마지막 커밋 이후 시간은 알 수 없다. 실측 일부 구간만 있는 경우 **일부**라고 표시하고 전체 실행 시간으로 취급하지 않는다. 아래 숫자는 기능 품질이나 사람의 완주 가능성을 수락하는 지표가 아니다.

## 실행 표

| Run | 작업 범위 | 시간(실측/활동) | 플레이 가능 기능 | BLOCKED/복원 | 발견 주체 | 비고 |
| --- | --- | --- | --- | --- | --- | --- |
| S2-night / G0 포함 | 대시·전투 통합, A05 획득, 저장·이어하기 | 활동41.1분, 10-02 23:48~10-03 00:29 (`f29b8ab`~`7b48ef4`) | 3: S2-01 통합 / S2-02 획득 / S2-04 저장 | 1: S2-03 복원 | 구현 중 Codex 자체 발견 | 벽/출구 assertion 재실패, 당시 원인 미확정. `[대기]` 체감. [결과](Validation/S2-03-gate-rerun.txt), [TASKS](TASKS.md) |
| S2-day / K1~K3 | 키 변경·대시 게이트 재개 | 활동12.7분, 10-03 11:43~11:55 (`3c70614`~`c28da2e`) | 2: K1 조작 / K3 게이트 | 1: K2 복원 | 직접 플레이 QA | 사용자 조작 불편·대시 카메라 밀림·출구 간섭 관찰. `[도구]` K2 비교 판정은 물리 틱 차이 가능성/원인 미확정. [TASKS K2/K3](TASKS.md) |
| S3-B / G2 포함 | 더블 점프·E2·B01/B02·E3/B03·B04 획득 | 활동36.5분, 10-03 12:39~13:15 (`a097a4e`~`c3bd8f5`) | 5: S3-01~05 | 0 | 구현 중 Codex 자체 발견 | 검사 발판이 탄을 막은 문제 보정, B04 높이 승인 보정. `[대기]` 당시 감각. [결과](Validation/S3-B-runtime.txt), [TASKS](TASKS.md) |
| S3-full / G3 포함 | B02 소개·C01/C02·C03/C04 | 활동20.6분, 10-03 15:57~16:17 (`77aac15`~`c88412d`) | 3: S3-06~08 | 1: S3-09 | 구현 중 Codex 자체 발견 | `[규칙]` 보스 시야/기존 카메라 범위 충돌. S3-10 종속 SKIPPED는 제외. [인계](Validation/S3-v2-prior-handoff.md), [TASKS](TASKS.md) |
| F1 / 좁은 보스 / S3-complete v1 | 피드백·보스 시도 후 복원 빌드 | 활동22.5분, 10-03 22:37~23:00 (`f5ab8c8`~`56aa13b`) | 0: 두 변경 복원, 빌드만 재생성 | 2: F1·S3-09 복원 | 구현 중 Codex 자체 발견 | `[도구]` F1 표시 측정 실패/원인 미확정. `[게임]` 보스 첫 파형 조기 소멸; 회복 명중 재실패 뒤 복원. S3-10 SKIPPED 제외. [TASKS](TASKS.md), [복원](Validation/S3-complete-restoration.txt) |
| F1 v2 / S3-complete v2 | 피드백 재시도·보스·엔딩 | 활동28.4분, 10-03 23:34~10-04 00:02 (`5c54277`~`6c5a3ae`) | 2: S3-09 보스 / S3-10 엔딩 | 1: F1 복원 | 기존 자동 회귀 | T2 실패로 복원했으나 원인 확정 아님; 후속 D0 기준 상태도 실패. `[대기]` 보스·엔딩 감각. [인계](TASKS.md), [실제 exe](Validation/S3-complete-v2-runtime.txt) |
| F2/LOG | 피드백 재시도·로그 | 활동13.9분, 10-04 01:50~02:04 (`7ca75e6`~`bd7d5c2`) | 0 | 2: F2 복원·LOG 차단 | 기존 자동 회귀 | F2 T2 실패 원인 미확정. `[규칙]` LOG 관찰 이벤트 부재/보호 파일 제한. [복원](Validation/F2-rollback-summary.txt) |
| D0/F2R/LOGR | 기준 비교·피드백·CSV 관찰 | 활동29.4분, 10-04 02:31~03:01 (`b5e9828`~`80c9dbf`) | 1: F2R 피드백; 내부 LOGR은 제외 | 0 | 기존 자동 회귀 | `[도구]` 기준 T2/T4/S2 검사도 실패, 신규 게임 회귀로 확정하지 않음. `[대기]` 피드백. [D0](Validation/D0-summary.txt), [F2R](Validation/F2R-summary.txt), [LOGR](Validation/LOGR-summary.txt) |
| TM/U0/U1 | 기존 회귀 보정·HUD/안내·사망 표시 | 활동29.4분, 10-04 03:15~03:45 (`3fdcf8f`~`4e99845`) | 1: U1 통합 UI | 1: 촬영 경로 | 구현 중 Codex 자체 발견 | `[도구]` ScreenCapture/PNG 참조 실패, 기능 보존·U1 REVIEW. `[대기]` 가독성. [인계](Validation/TM-U1-final-handoff.md) |
| Q-night4 | Q0~Q7·저장 확장 검사·피드백 후보 | 실측40.4분, 10-04 11:48:07~12:28:30 KST | 1: Q6 후보 A~C 묶음 | 5: S2-04·Q2~Q5 | 구현 중 Codex 자체 발견 | `[게임]` Q1-SAVE-001 1건/2조건. `[도구]` Q2~Q5 측정/환경/관찰 한계4개. Q0 SKIPPED·Q7 KNOWN-LIMIT 제외. [진행](Validation/Q-night4-progress.txt), [인계](Validation/Q-night4-final-handoff.md) |
| N5 | 저장 복구·T-A~G 확장 검사 | 실측69.0분, 10-04 15:17:18~16:26:20 KST | 1: 손상 저장에서 새 게임 가능하도록 저장 기능 완성 | 0: 정식 BLOCKED/복원 없음 | 구현 중 Codex 자체 발견 | `[도구]` T-A/T-D/T-E/T-G KNOWN-LIMIT·T-F focus 부분 한계, 아래 별도 집계. `[대기]` 기존 체감. [진행](Validation/N5-progress.txt), [인계](Validation/N5-final-handoff.md) |
| N6 | 시작 조건 검토·사용자 중단 | 기록 없음 | 0 | 1: 시작 조건 검토 | 구현 중 Codex 자체 발견 | `[규칙]` HEAD가 N5 인계 이후가 아님. `[대기]` 판단 후 중단 선택; 대기 시간 기록 없음. 별도 재개 전 실행하지 않음. [검토](Validation/N6-preflight-review.txt) |
| N7 본 작업 | 방향 공격 시도·Hard 후보·레벨 제안·V2~V5 | 활동28.9분, 10-05 01:17~01:46 (`75322be`~`bb6ad1f`); soak 제외 | 1: Hard 후보; 공격 복원·레벨 문서 제외 | 6: ATK1~4·PogoTest·V5 | 구현 중 Codex 자체 발견 | `[규칙]` 입력 Asset 미허용·종속 기능·V5 원문 부재. ATK1 복원은 중복 계수 안 함, S5-attack은 ATK4 포함. `[도구]` V4는 KNOWN-LIMIT. `[대기]` 입력/감각 판단. [인계](Validation/N7-current-handoff.txt), [승인 범위](Validation/N7-approved-request.txt) |
| N7 V1 / 후속 기록 | 장시간 검사·결과 보존 | 기록 없음: 실행 전체 시작/종료는 없음; 시험 elapsed7201.264s 별도 | 0 | 0 | 기록 없음 | 시작01:30:32는 기록됐으나 종료 벽시각은 없음. 약120분 시험 PASS는 작업 실측으로 변환하지 않음. [시작](Validation/N7-V1-process.txt), [시험](Validation/N7-V1-soak-a1.txt), [후속 감사](Validation/N8-soak-review.txt), `fb7b8b2` |
| N8 | 위 공격·포고 검토·자동 경로·사망 화면 | 실측81.3분, 10-06 00:28~01:49 KST(요청 저장 이후) | **1: 위 공격만** | 6: ATK2~4·PogoTest·S5-hard, 후속 경로 | 구현 중 Codex 자체 발견 | `[규칙]` 보호된 즉시 사망/포고 우선 순서5개. **`[도구]` 완주 판정 철회**: R05 A04 사망 후 CP-A03 복귀를 목표로 잘못 인정. 당시 자동17구간 PASS는 기능에 세지 않음. [인계](Validation/N8-final-handoff.md), [N9 감사](Validation/N9-N8-route-csv-audit.txt) |
| N9 | 아래 공격·포고·PogoTest·난이도 후보 | **실측103.1분**, 10-06 06:54:50~08:37:53 KST | **3: 아래 공격·포고 / PogoTest / H1~H3 후보 묶음** | 2: RouteAll·Editor Pogo 재생 | 구현 중 Codex 자체 발견 | **`[도구]` 입력 라이브러리 R05 사망**; Editor 목표 입력 소진. 최종14/15, 초기 후보 캐시 누락 결과 무효화 후 재검사. `[대기]` 포고·난이도 선택. [진행](Validation/N9-progress.txt), [인계](Validation/N9-final-handoff.md) |

| S3SW / PROC-001 | 운영 규칙·과거 지표·결정 문서 | 실측8.6분(일부: 10-06 21:25:52~21:34:27 KST; 초기 검토 시각 기록 없음) | 0 | 0 | 기록 없음 | 문서만 변경, Unity 미실행. 전체 작업 시간으로 취급하지 않음. [진행](Validation/S3SW-progress.txt), [QA](Validation/S3SW-user-qa.md) |

## 2단계 회고 — N5~N8 (이번 한 번만)

명시적 BLOCKED/복원 Task를 원인당 한 번만 집계했다. 후속 N9 감사로 철회된 N8 경로도 현재 정정 상태로 포함한다.

| 원인 태그 | 건수 | Task 목록·근거 |
| --- | --- | --- |
| `[규칙]` | 12 | N6 시작조건1([검토](Validation/N6-preflight-review.txt)); N7 ATK1~4·PogoTest·V5 6([인계](Validation/N7-current-handoff.txt), [ATK4 범위](Validation/N7-approved-request.txt)); N8 ATK2~4·PogoTest·S5-hard 5([인계](Validation/N8-final-handoff.md)) |
| `[도구]` | 1 | N8 자동 경로 판정 철회·후속 BLOCKED1([감사](Validation/N9-N8-route-csv-audit.txt), [TASKS 정정](TASKS.md)) |
| `[게임]` | 0 | 이 기간의 정식 BLOCKED/복원에서 새 실제 게임 결함으로 확정한 항목 없음 |
| `[대기]` | 0 | 단독 주원인으로 추가 계수한 Task 없음. N6/N7의 사용자 판단 대기는 규칙 원인 Task의 부연이며 중복 계수 안 함 |

정식 합계13개와 별도로 `[도구]` KNOWN-LIMIT/부분 한계 **7개 경로**를 확인했다: N5 T-A 이동·T-D G-D·T-E soak·T-F focus·T-G 전체 경로5, N7 V4 촬영1, N8 Editor R16 재생1. 이는 Task 전체 BLOCKED 수에 섞지 않는다. N5~N8의 실측·활동 시간을 서로 합쳐 작업 시간 총계나 효율 비율을 만들지 않는다.

**기존 자동 회귀가 새로 발견한 것으로 확정된 실제 게임 결함: 0건, 목록 없음.** N5는 앞선 Q-night4의 Q1-SAVE-001(빈 파일/미지원 version의 미처리 예외) 1건을 수정했으며 신규 발견이 아니다. Q1은 기존13회귀가 PASS인 상태에서 별도 확장 검사로 발견했으므로 기존 회귀의 발견 실적에 포함하지 않는다. N7 입력 범위 충돌은 제품 결함으로 확정하지 않았고, N8의 사망 목표 오판은 도구 결함이다. [N5 결론](Validation/N5-final-handoff.md), [Q1 근거](Validation/Q-night4-final-handoff.md).

**검사 도구 수정 관련 커밋: 메시지 기준 대략9개.** `483a6ad..6036b9e` 범위(N5~N8, N6/N7 포함)에서 `tool|repair|recording|matrix|soak|capture|replay|regression`을 대소문자 구분 없이 검색했다. 신규 도구 작성·보정·증거 기록을 포함하는 대략치이며, 실제 코드 수정 횟수나 결함 수가 아니다. 일치 해시: `df46a03`, `bdb0a35`, `ccb72fd`, `e22dd88`, `28f9eae`, `bb6ad1f`, `fb7b8b2`, `8b2518a`, `b22dd9c`. [메시지·시간 근거](Validation/S3SW-metrics-evidence.txt). 이 회고 계산은 앞으로 반복하지 않는다.

## 3단계 성공 기준 (고정)

**같거나 더 많은 플레이 가능 기능을 내면서, `[도구]`·`[규칙]` 원인의 BLOCKED가 줄고, 사람이 판단할 문제가 직접 QA에서 발견되는 것.** 기능 수가 크게 늘지 않아도 BLOCKED 감소만으로 개선으로 본다. 최소2~3번 실행한 뒤 판단한다.

앞으로 실행 종료마다 실행 표에 한 줄만 추가한다. 2단계 회고를 다시 계산하거나 새 검증 도구를 지표 수집 목적으로 만들지 않는다. 문서 실행도 기능0으로 기록한다.
