## Latest Handoff

**회귀 묶음 13/13 PASS / KNOWN-FAIL 없음. U1 상태·실제 exe PASS / Error0. 요청 HUD 스크린샷 6장은 촬영 검사1회보정 재실패로 미실시.**

2026-10-04 승인 **TM → U0 → U1 → S4-pre2 빌드** 종료. **TM/U0 DONE, U1 REVIEW(명세 B 규칙: 기능 보존·촬영 경로 BLOCKED/가독성 사용자 대기)**. F2R 처치효과 생성·소멸 검사는 TM에서 재확인 PASS, 감각 수락 상태는 REVIEW 유지. LOGR·S3-06~10 REVIEW/Sprint3 Gate 사용자 판단 유지. main / 단계 로컬 커밋 / push 없음. [직전 인계](Validation/TM-U1-prior-handoff.md).

| 작업 | 결과 | 증거 |
| --- | --- | --- |
| 승인 | 원문 보존 / 3fdcf8f | [명세](Validation/TM-U1-approved-request.txt) |
| TM | DONE / a480ddf; 정식 T2 직접 시계·CP 분리, T2Legacy 보존, T4/S2/F2R 적 참조 정비 | [TM 요약](Validation/TM-summary.txt), [13/13](Validation/REG-TM2-summary.txt) |
| U0 | DONE / c946876; PRD 11개 UI 항목 점검 | [UI 표](Validation/U0-ui-audit.txt) |
| U1 | REVIEW / fba51b3; 표시 구현·Unity 상태·회귀 PASS, 촬영 한계 | [UI 결과](Validation/U1-ui.txt), [U1 회귀](Validation/REG-U1-summary.txt) |
| 최종 | Windows x64 17씬 errors0/warnings0, 실제 exe UI·보스/엔딩·CSV PASS | [전체 요약](Validation/S4-pre2-summary.txt), [빌드](Validation/S4-pre2-build-tool-rerun.txt), [UI exe](Validation/S4-pre2-ui-runtime-tool-rerun.txt), [CSV exe](Validation/S4-pre2-runtime.txt), [CSV](Validation/S4-pre2-session.csv), [최종 회귀](Validation/REG-S4-pre2-summary.txt) |

- **켤 exe:** [Afterglow-S4-pre2.exe](Builds/S4-pre2/Afterglow-S4-pre2.exe). 같은 폴더의 Afterglow-S4-pre2_Data/DLL 유지. Menu+15플레이방+Ending, 테스트 씬 제외. 기존 S4-pre/S3-complete/Experimental-F1 보존. Builds/Logs는 Git 제외.
- **UI 변경:** 타이틀·새 게임 확인창·Pause·엔딩·저장 실패 안내는 기존 구현이 있어 기능 보존. 텍스트 HP를 흰색/회색 5칸으로 교체, 대시/더블 점프 아이콘과 현재 장치 C/Z 또는 B/A 표기 통합, 방 ID/Scene notice 유지. 새 게임 첫 A01에서만 3초/첫 이동까지 조작 안내, Continue CP-A01(능력 없음)에서도 미표시. 기존 두 획득 안내는 통합 패널·장치 표기로 표시 정리. Respawning 상태 동안만 검정 음영, 기존 .6초 복귀·입력 잠금 불변.
- **실제 검증:** Unity Play 및 실제 exe에서 HP5→4→5, 보유 false/false→Dash→둘 다, 획득 안내3초/키보드·가상 패드, 새 게임/Continue 구분, 사망 오버레이 활성→복귀 비활성, 보스 처치→엔딩→타이틀 PASS/Error0. 1280×720 UI exe / 1920×1080 독립 CSV exe로 실행했지만 픽셀 배치 검수는 아님. 직접 방 배치·실제 픽업 접촉·보스 직접20피해이며 자연 완주/물리패드/사용자 눈 수락은 미실시. 최초 UI 보스 검사 Entry 실패는 C06 실제 FromLeft로 검사1회보정 후 PASS, 게임 불변.
- **스크린샷 문제:** [U1-shots](Validation/U1-shots/NOT-CAPTURED.txt)는 미촬영 안내만 있음. ScreenCapture 모듈 참조 실패 후 ReadPixels 방식 검사1회보정에서도 EncodeToPNG 참조 실패, 해당 촬영 경로 중단. 컴파일을 막는 검사 부분만 제거하고 기능/독립 검증 보존(B 규칙). [도구 기록](Validation/U1-tool-correction.txt), [보존 소스](Validation/U1-capture-stopped-source.txt). 다른 Camera 이미지들은 월드만 렌더하므로 HUD 스크린샷으로 대체하지 않음.
- **사용자 확인:** HUD 칸/능력 아이콘·키가 보이고 이동 공간을 가리지 않는지, 새 게임 첫 안내가 적절히 사라지는지, 사망 중 어두워지고 복귀 후 즉시 걷히는지, 기존 공격/적HP 표시도 계속 보이는지 판단. U1 가독성 수락 전 DONE 전환하지 않음.
- **수정 위치:** [UITuning.asset](Assets/Resources/UITuning.asset): HUD Position/Size(1280×720 기준 px), HP Pip Size/Gap, Ability Icon Size, Notice Seconds(3s), Font Size(16px), Death Display Seconds(.15s 음영 진입/기존 Respawning 안에서만), Death Opacity(.65). Play 종료 후 원하는 값·이전 값을 명시 저장. Scene 배치는 Stop 후 다시 입력/저장하되 이번 작업에서 변경하지 않음.
- **주요 파일/보호:** 신규 GameHUD.cs/UITuning.cs/UIVerification.cs/UICheck.cs/S4Pre2Build.cs, Assets/Resources/UITuning.asset, 검사 도구/회귀·보호 실행기/README/TASKS/Validation. 기존 입력·체력·RoomLabel·픽업은 OnGUI만 수정. [보호 검사](Validation/S4-pre2-preservation.txt): OnGUI 밖 diff0; 이동/대시/피격 게임 로직·GameState/RoomSession·저장·보스/적·카메라·모든 Scene/Prefab·기존 Tuning·Packages/ProjectSettings diff0.
- **다음:** 사용자 exe UI/완주 판단, 필요 시 별도 승인 촬영 도구 경로. GAME_DESIGN §18의 구키 표기는 Decisions Needed에 기록하고 최신 K1 명시 지시를 적용. Sprint3 Gate·튜닝/레벨배치·새 시스템/후속 Sprint는 수행하지 않음. 이번 승인 범위 종료.
