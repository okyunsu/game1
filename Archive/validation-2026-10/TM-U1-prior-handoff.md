## Latest Handoff

**D0: 표시 없는 기준에서도 기존 T2 잠금 검사가 같은 줄에서 FAIL. 직접 시각 측정 T2v2는 검사 위치의 CP 간섭을 피한 뒤 30/60/120fps PASS.** 기존 T4의 파괴된 적 참조 및 S2 통합 HP assertion도 기준/F2R 동일 FAIL. 이 결과를 F2R 신규 회귀로 보지 않는 새 사용자 결정 적용. F1의 모든 독립 영향을 해명했다는 뜻은 아님.

2026-10-04 승인 D0 → F2R → LOGR → 최종 빌드 종료. **main / 로컬 커밋 / push 없음. F2R REVIEW(표시 검사 도구 한계), LOGR REVIEW, S4-pre 실제 exe 및 최종 회귀 PASS.** S3-06~10 REVIEW / Sprint3 Gate 사용자 판단 유지, Sprint4 튜닝·콘텐츠 미착수. [직전 실패 인계](Validation/F2-first-prior-handoff.md).

| 단계 | 결과 / 커밋 | 증거 |
| --- | --- | --- |
| 승인 | 전문·새 사용자 판단 보존 / b5e9828 | [명세](Validation/F2R-LOGR-approved-request.txt) |
| D0 | 원본 T1/T3 PASS, T2/T4 기준FAIL; T2v2 보정PASS / 0a769f6 | [요약](Validation/D0-summary.txt), [측정](Validation/D0-rerun-T2v2.txt) |
| F2R | 표시·E2예고/보스회복/T2v2/S1/카메라 PASS, 처치효과 도구참조 한계 REVIEW / 7350171 | [요약](Validation/F2R-summary.txt), [표시+도구한계](Validation/F2R-feedback-tool-rerun.txt), [보스명중](Validation/F2R-boss-recovery.txt) |
| LOGR | CSV·사망위치·Pause제외·저장해시·S1 PASS, REVIEW / 531c51e | [결과](Validation/LOGR-playtest.txt), [CSV](Validation/LOGR-session.csv), [설계/한계](Validation/LOGR-summary.txt) |
| 최종 빌드 | Windows x64 errors0/warnings0, 실제exe/최종S1·카메라 PASS / 3da46e9 | [빌드](Validation/S4-pre-build.txt), [exe](Validation/S4-pre-runtime.txt), [CSV](Validation/S4-pre-session.csv), [보존](Validation/S4-pre-preservation.txt) |

- **켤 exe:** [Afterglow-S4-pre.exe](Builds/S4-pre/Afterglow-S4-pre.exe). 같은 폴더의 Afterglow-S4-pre_Data/DLL 유지. Menu+15플레이방+Ending(17씬), 테스트 씬 제외. S3-complete·Experimental-F1 보존. Builds/Logs는 Git 제외.
- **실제 검증:** 표시30/60/120fps 좌우 startup/active/end·판정 중심/크기0.01u 이내·hit1/miss0·남은HP 흰칸수·번쩍임±1프레임·E2 예고중 오버레이 PASS. 보스 회복 실제X 1명중/HP20→19/표시1회 PASS. T2v2 해제는 F2R .133326/.133336/.124994초, 설정 .12초+관찰프레임 안이며 반대방향 입력에도 잠금중 x부호 유지. 기존 T2/T4/S2통합 실패는 기준과 동일. F2R 처음 Asset GUID 불일치1회수정 후 도구 stale E2참조1회보정; 최종 도구는 stale E1 body에서 중단해 처치효과 생성/소멸 항목 재확인 못함. 기능은 명세 B 규칙으로 보존/REVIEW, 전체 표시검사PASS로 기록하지 않음.
- **LOGR/실제exe:** 새 게임 CSV 생성→A01 공격표시→A02 사망(8,1.81)/unrecorded→Pause3.017초 제외→A04 적 HitFeedback→C04/C06 직접 검증배치→보스 시작/직접20피해 처치→2초빛/Ending→Z 타이틀 PASS, Error/Exception/Assert0. 저장 SHA256은 방/사망/Pause 전후 동일. 직접 처치는 자연 보스전 완주 증거가 아님. CSV에서 boss_start/defeated/ending·지역/방별사망/시도1/누적Pause 요약 확인.
- **최종 회귀:** [S1-02](Validation/S1-02-night-S2-S4-pre.txt), [S1-04](Validation/S1-04-night-S2-S4-pre.txt), [S1-07](Validation/S1-07-night-S2-S4-pre.txt), [S1-08](Validation/S1-08-night-S2-S4-pre.txt), [카메라lead0](Validation/S1-06-lead0-night-S2-S4-pre.txt) PASS. F2R/LOGR 단계별 지정 회귀도 PASS. 가상 입력·자동 지정배치이며 물리패드·사용자눈·자연완주시간 미실시.
- **보호/주요 파일:** 입력·이동·대시·피격·GameState·RoomSession·KillZone·원래Tuning·모든Scene·RoomCameraRig·Packages·ProjectSettings diff0. NightTests/BossVerification 원본도 불변. PlayerAttack/ReceiveHit에는 허용 표시호출만. 7개 프리팹에 표시 컴포넌트/자식만 추가, boss/dummy는 새 FeedbackSceneBinding으로 연결. LOGR은 PlaytestLogger 자체 생성·관찰만, 기존 저장코드 수정 없음. 신규 스크립트/FeedbackTuning.asset·검사 도구·README/TASKS/Validation 변경.
- **사용자 확인/수정 위치:** 공격 표시, 적 HP칸·흰번쩍임·처치효과, 보스 기존 상단 HP 막대가 실제로 보이는지 확인. `Assets/ScriptableObjects/FeedbackTuning.asset`: slashAlpha .7/연노랑, hitFlashSeconds .10, hpPipSize .18u/gap .06u/offset .35u, deathPopSeconds .20/scale1.6. 히트스톱/소리/흔들림 없음. Asset은 Play 종료 후 원하는값/이전값을 명시 저장; Scene 변경은 Stop 뒤 다시 입력·저장.
- **CSV 위치:** `C:\Users\gosu3\AppData\LocalLow\Afterglow\Afterglow\Playtest\YYYYMMDD-HHMMSS.csv`. 새 게임부터 기록, 이어하기/메뉴/테스트는 새 세션 시작 안 함. 체크포인트는 ID 변화만(같은 CP 재활성은 기록 안 함), 사망원인은 의도적으로 unrecorded. 쓰기 실패는 한 경고 후 기록 세션만 중단. 검증 CSV 025024/025455는 자동검사 자료이고 사용자 관찰 기록이 아님.
- **문제/다음:** 도구 stale참조·기준T2/T4/S2 검사 정비는 별도 승인 범위. 표시 가독성/처치효과와 자연완주·CSV 활용을 사용자 확인 후 판단. S3 Gate·수치 채택·추가 작업은 수행하지 않음. 이번 승인 범위 종료.
