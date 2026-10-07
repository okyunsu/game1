# B01-A / F1-R — 히트스톱 / PARTIAL

START_COMMIT: 50bb3324f88a6c5b2dc2657e8fe4bcadbf3c5f2a. 변경은 HitFeedback 명중 진입점, FeedbackTuning 필드1개, 기본 설정의 hitStopSeconds=0.05, 새 HitStop.cs/.meta뿐. 베기·흰 번쩍임·HP 칸 구현과 기존 모든 설정 값은 유지했다.

## 구현 판단

- HitFeedback.Hit에서만 시작하는 지속 컴포넌트. 실시간 마감으로 timeScale 정지를 해제하며 이미 정지 중이면 마감을 연장하지 않는다.
- GameState는 읽기·Changed 관찰만 한다. Pause/사망/전환/Completing이 정지를 넘겨받으면 히트스톱을 취소하고 시간 배율을 복구하지 않는다. 기존 GameState가 재개·복귀 배율을 관리한다.
- Inspector는 FeedbackTuning.asset의 Hit stop / hitStopSeconds(실시간 초, Range 0~0.2, Tooltip), 0이면 정지 없음. Play 종료 후 원하는 값 또는 이전 값을 명시 저장해야 다음 빌드에 반영된다. 0.05는 Batch가 지정한 값이며 체감 수락은 대기한다.

## 실제 실행

기존 F2RVerification.Run, T2LockCheck.Run, BossRecoveryCheck.Run을 실행했다. 새 검증 도구·검사 코드 변경 없음. 첫 샌드박스 Unity는 라이선스 접근 제한으로 exit198/검사 미진입, 기존 라이선스에 접근할 수 있는 실행에서 검사했다.

- F2R: PASS / Error/Exception/Assert=0. 6개의 빗나감은 추가 정지0, 30/60/120fps 양 방향 더미 명중6회와 E2/E1 동시 명중2회를 총 정지7회로 처리. 동시 명중은 기존 마감을 연장하지 않음. 베기/번쩍임/HP/처치 효과 기존 검사 PASS.
- 원본 실시간 측정:

```text
MEASURE HitStop elapsed=0.065499s reason=elapsed scale=1.0 fps=30
MEASURE HitStop elapsed=0.066432s reason=elapsed scale=1.0 fps=30
MEASURE HitStop elapsed=0.066426s reason=elapsed scale=1.0 fps=60
MEASURE HitStop elapsed=0.066429s reason=elapsed scale=1.0 fps=60
MEASURE HitStop elapsed=0.058100s reason=elapsed scale=1.0 fps=120
MEASURE HitStop elapsed=0.058121s reason=elapsed scale=1.0 fps=120
MEASURE HitStop elapsed=0.066208s reason=elapsed scale=1.0 fps=60
```

- T2v2: 30/60/120fps PASS / Error/Exception/Assert=0. D0 기준의 직접 잠금 측정을 사용, Legacy assertion은 사용하지 않음.

```text
MEASURE fps=30 unlock=0.133340s maxObservedFrame=0.033342s configured=0.120000s opposingFrames=3
MEASURE fps=60 unlock=0.133340s maxObservedFrame=0.016710s configured=0.120000s opposingFrames=7
MEASURE fps=120 unlock=0.124995s maxObservedFrame=0.008340s configured=0.120000s opposingFrames=14
```

- 보스: 기존 실제 X 명중/표시1회/HP·회복 설정 불변 PASS / Error/Exception/Assert=0.

```text
MEASURE HitStop elapsed=0.063670s reason=elapsed scale=1.0 fps=60
```

## 미실시 및 상태

히트스톱 진행 중 Pause 입력→재개, 정지 중 사망→기존0.6초 복귀를 겹쳐 실행하는 기존 검사 진입점은 확인하지 못했다. 코드상 상태 소유권 처리만 검토했고 동시 상황의 실행 PASS로 기록하지 않는다. 0 값의 실제 플레이, 실제 패드·사용자 손맛도 미실시다. 기술적으로 일부 조건이 남았으나 후속 개발에 사용 가능하므로 PARTIAL. Batch 종료의 독립 Backlog 기본 회귀1회는 13/13 PASS였다. [결과](REG-B01-final-summary.txt).

원본 자료: Validation/_raw/B01/A-F2R.log(라이선스 미진입), A-F2R-authorized.log, A-T2v2.log, A-boss.log 및 해당 결과 .txt. 원본은 ignore 대상이며 이 요약1개만 커밋한다.
