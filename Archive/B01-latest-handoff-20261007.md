# Batch 01 이관된 인계 이력

## Latest Handoff

### 결정 질문지

- F1-R 히트스톱 0.05s: **유지 / 조정 / 끄기(0)**. 추천은 직접 플레이 후 유지 여부 판단. 명중·빗나감·중첩과 프레임별 시간은 확인했지만 손맛은 자동 검사로 수락할 수 없다. 답은 F1-R 체감 수락을 풀며 동시 Pause/사망 기술 미검증을 대신하지 않는다.
- 다음 Batch: **동시 Pause/사망 확인 우선 / 다른 승인 Task 우선**. 추천은 미검증 두 상황을 기존 수단 또는 직접 QA로 먼저 확인하는 것. 새 도구와 보호 파일 변경은 이번 범위에 포함하지 않았다. Sprint 3 Gate는 별도 사용자 완주 판단 대기다.

### 직접 플레이 확인 — 사용자 QA 요약

확인용 Windows 빌드: [Afterglow-B01-hitstop.exe](../Builds/B01-hitstop/Afterglow-B01-hitstop.exe). 같은 폴더의 Data/DLL을 함께 유지한다. 이전 S3T-01 빌드와 구분하며 이번 exe의 RouteAll은 통과했다.

1. 기존 수락 대기: Menu 저장/이어하기(S2-04), A04·C06 공격/포고·표시(F2R), C05 보스→엔딩·전체 완주(U1/Gate). 약 10~20분. 이전 상세 항목은 [S3T-01 QA](../Validation/S3T01-user-qa.md)와 Archive의 이전 인계를 참고한다. 이번 자동 검사는 감각 수락을 대신하지 않는다.
2. 신규 F1-R: A04 또는 C06에서 명중·빗나감·연속 명중, 정지 중 Pause→재개와 피격 사망→복귀. 약 5분. **바뀐 점:** 명중 때만 0.05초 정지. **의심 지점 3개:** 재개 뒤 멈춤 잔류, 사망 복귀 뒤 배율 이상, 연속 명중의 과도한 끊김. 앞의 두 상황은 실행 미검증이다.

조정 위치: `Assets/ScriptableObjects/FeedbackTuning.asset`의 `hitStopSeconds`(실시간 초, 0~0.2, 0은 끄기). Play를 종료하고 Asset 값을 저장한 뒤 다시 빌드해야 exe에 반영된다. 현재 0.05는 Batch 지정 임시값이며 체감 수락 대기다.

### Task별 결과

| Task | 상태 / 로컬 커밋 | 변경·실제 검증·남은 문제 |
| --- | --- | --- |
| A / F1-R | PARTIAL / c37b0fa | HitFeedback 명중 진입점, FeedbackTuning 필드·Asset, HitStop.cs/.meta. F2R 30/60/120fps 명중·빗나감·중첩, T2v2, 보스 PASS/Error0. 동시 Pause/사망·0 값·물리 패드·체감 미실시. [요약](../Validation/B01-A-summary.md) |
| B / 문서 | DONE / dfbcfe2 | TASKS Current Sprint 5줄, GAME_DESIGN §18 키보드 5행, 해결한 UI 문서 불일치 Archive 이관. 게임 규칙 변경 없음. [요약](../Validation/B01-B-summary.md) |
| C / 빌드 | DONE / 4944d98 | Windows x64 18 Scene 빌드/Error0/Warning0. 실제 exe 보스 접촉 사망·복귀3회→처치→엔딩 저장 PASS/Error0. A PARTIAL을 명시 허용한 확인용 빌드. README 실행 안내. [요약](../Validation/B01-C-summary.md) |
| R06 | DONE / b50b8fd | 기존 경로 라이브러리의 R06 입력만 정상 연속 귀환에서 교체, 첫 접근1회 성공. 전체 exe R01~R17 PASS/사망0/Error0/엔딩→타이틀. 실제 R06 시작 편차0.082u. 다른16개 입력 불변. [요약](../Validation/B01-R06-summary.md) |
| 기본13 | DONE / edcfdca | 기존 Run-Regression 기본 부분1회, 13/13 PASS. 새 도구·검사 파일 수정 없음. Pogo 별도 재실행 없음. [요약](../Validation/REG-B01-final-summary.txt) |

START_COMMIT: `50bb3324f88a6c5b2dc2657e8fe4bcadbf3c5f2a`. 게임 파일 diff는 FeedbackTuning.asset, FeedbackTuning.cs, HitFeedback.cs, 새 HitStop.cs/.meta의 5개뿐. Scene/Prefab 배치·Player core·입력·다른 튜닝·Packages/ProjectSettings·PRD/AGENTS 불변. 새 플레이 범위는 명중 히트스톱이며 R06 시작 위치 가정 문제를 해소해 현재 exe 전체 경로 리스크를 줄였다. 기존 Editor Pogo 재생 한계와 사용자 수락 대기는 유지한다.

원본 로그·CSV·입력·Camera 출력은 `Validation/_raw/B01/`에 보존(미커밋). Task별 요약1개씩만 커밋. 진단 빌드는 추가하지 않았고 최종 빌드1개 생성. Task5개로 승인 예산8개 이내, Backlog 소진으로 종료. push 없음.

구현 판단: GameState는 관찰만 하며 Pause/전환/사망/Completing이 시간 배율 소유권을 가져가면 히트스톱이 복구하지 않는다. 기존 게임 상태의 복구 처리를 따른다.

### 다음 Batch 후보

F1-R 동시 Pause/사망·0 값 직접 확인 → 체감 수락, 기존 Pogo 재생 한계의 승인 범위 결정, 사용자 완주 후 Sprint 3 Gate 판단. 새 Batch 승인 전 기능·콘텐츠를 확장하지 않는다.

## B01-HITSTOP 해소 전 결정 이력

### B01-HITSTOP — 동시 상황 확인·0.05s 체감 / PARTIAL
명중·빗나감·중첩과 30/60/120fps 정지 시간, T2v2, 보스 명중은 기존 도구로 확인했다. 히트스톱 중 Pause→재개, 정지 중 사망→0.6초 복귀를 겹치는 기존 검사 진입점은 확인하지 못해 실행 PASS로 기록하지 않는다. 보호 파일·새 검증 도구 없이 구현한 상태 소유권 처리의 직접 QA와 0.05s 유지/조정 판단이 남는다. C 빌드는 PARTIAL 의존을 명시 허용한 Batch 범위에서 생성했다. [실제 결과·미실시](../Validation/B01-A-summary.md).


