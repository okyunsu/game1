## Latest Handoff

**Q0 SKIPPED / Q1 FAIL(저장 버그 1건·2조건) / Q2 BLOCKED / Q3 BLOCKED / Q4 BLOCKED / Q5 BLOCKED / Q6 PASS / Q7 KNOWN-LIMIT. 큐 기록 기준 총 40.4분(사전 문서 검토 제외).**

2026-10-04 night4 큐를 Q0~Q7까지 검토·실행하고 종료했다. **Q0~Q7 종료 회귀는 각각 13/13 PASS**(Q0 최초 격리 라이선스 오류 후 보정, Q6 참조 보정 후 재회귀 포함). 이 통과는 아래 추가 저장 실패·미검증 조건을 덮지 않는다. **S2-04 BLOCKED(Q1-SAVE-001)**, U1/F2R/LOGR·S3-06~10 REVIEW, Sprint 3 Gate 사용자 판단 유지. main / 단계별 로컬 커밋 / push 없음. [진행·시각](Validation/Q-night4-progress.txt), [원문](Validation/Q-night4-approved-request.txt), [이전 작업 검토](Validation/Q-night4-prior-review.txt), [직전 인계](Validation/TM-U1-final-handoff.md).

| 큐 | 결과·실제 범위 | 증거 |
| --- | --- | --- |
| Q0 | SKIPPED: 완주 메모 비어 있음. PT-001/사용자 CSV 요약/문제 후보·수락 판단 생성 안 함 | [승인 원문](Validation/Q-night4-approved-request.txt) |
| Q1 | FAIL·버그 1건: 빈 파일/미지원 버전에서 미처리 예외. Dash/Double Jump/CP/보스/엔딩 저장 감지 후 실제 프로세스 강제 종료, 능력·CP 재실행 PASS. 읽기전용·폴더 쓰기 실패/보스 사망3회/엔딩 후 Continue 완전 검증은 미실시 | [결과](Validation/Q1-dod-robustness.txt), [빈 파일](Validation/Q1-corrupt-empty.txt), [미지원](Validation/Q1-corrupt-version.txt) |
| Q2 | BLOCKED: 4개 요청 해상도에서 실제 exe UI 상태/Error0 PASS. 렌더 픽셀 좌표·보스/예고 시야·전체화면·크기변경·포커스 미검증. 별도 Windows API 확인했으나 격리 CLI 창이 대상 목록에 없음 | [결과](Validation/Q2-display.txt) |
| Q3 | BLOCKED: Editor 30/60/120fps 설정 일반점프2.535u, 더블4.965/5.055/5.049u, G-J 여유 PASS/Error0. 144fps/전체 구간 행렬/수평 최대거리 미실시 | [표](Validation/Q3-fps-baseline.txt), [실측](Validation/Q3-night4-jump.txt) |
| Q4 | BLOCKED: 기존 성공 경로는 출구에 직접 배치함. 실제 구간 최단 이동 시간으로 사용할 수 없어 목표 대비 표는 NOT MEASURED | [근거·목표](Validation/Q4-route-times.txt) |
| Q5 | BLOCKED: 45분·50사망·100Pause·300전환 반복과 내부 GameObject/EnemyShot 샘플을 지원하는 기존 exe 도구 없음. 45분 시험/CSV 생성 안 함 | [한계](Validation/Q5-soak.txt) |
| Q6 | PASS: A/B/C 빌드0errors/0warnings, 실제 AttackSlash·적 명중/Error0·후보 Asset 이름/4수치 일치, 보정 후 회귀13/13 | [후보 표·비교](Validation/Q6-feedback-candidates.txt), [1회 보정](Validation/Q6-tool-correction.txt), [회귀](Validation/REG-Q6-night4-correction-summary.txt) |
| Q7 | KNOWN-LIMIT: 실제 exe ScreenCapture 첫 호출 오류. D3D11+영문 임시 경로 1회 보정에서 6장 모두 같은 검정 이미지. 시각검수 FAIL, 유효 12장 없음, 1920 후속 촬영 중단. 최종 회귀13/13 | [결과](Validation/Q7-screenshots.txt), [실패 증거](Validation/Q7-shots/NOT-CAPTURED.txt), [최종 회귀](Validation/REG-Q7-night4-summary.txt) |

- **새 버그:** High Q1-SAVE-001. `ProgressSave.TryLoad`의 `InvalidDataException`이 catch 필터에 잡히지 않아 빈 파일·version999에서 `SaveMenu.Start` 예외/Error1. 정상 저장 손실은 확인하지 않았으나 안내/새 게임 안전 경로와 DoD 오류0 조건 미충족. Q1 수정 금지로 미수정, S2-04 BLOCKED로 되돌림. 잘린 JSON·잘못된 JSON의 안내·명시 새 게임·원본 보존은 PASS. 일반 슬롯/기존 사용자 CSV는 수정하지 않음.
- **Q0 Sprint 4 후보:** 사용자 완주 메모가 없어 생성하지 않음. 자동 검사 CSV를 사용자 PT-001로 취급하지 않았음. [자동 CSV 구분](Validation/Q-night4-auto-csv/README.txt). 사용자 감각 수락/DONE 전환 없음.
- **사용자가 비교할 빌드(각 2분):** [A 현재](Builds/Q6-A/Afterglow-Q6-A.exe), [B 강하게](Builds/Q6-B/Afterglow-Q6-B.exe), [C 약하게](Builds/Q6-C/Afterglow-Q6-C.exe). 타이틀 왼쪽 아래 후보명 확인 → A01 공격 표시 → A04 명중 번쩍임/HP칸/처치 효과 비교. 같은 폴더 Data/DLL 유지. `Q6-*-attempt1`은 참조 보정 전 증거이므로 비교용으로 쓰지 않음.
- **판단·다음 작업:** Q6에서 선호 후보를 선택하되 원본 기본값 반영은 별도 명시 결정. 저장 버그 수정 승인 여부, Q2~Q5에 필요한 검증 도구/입력 경로의 별도 범위 결정. Q4는 실측이 없어 초과 구간 판단을 요청하지 않음. U1 HUD 가독성·Sprint 3 완주/Gate 판단 유지. 승인 없이 새 Sprint 콘텐츠·레벨/규칙·기본값 변경 없음.
- **수정 위치:** 후보만 `Assets/Resources/Q6/Feedback-A/B/C.asset`(기존 FeedbackTuning의 단위·Tooltip·그룹 유지), 원본 `Assets/ScriptableObjects/FeedbackTuning.asset` 불변. Play 후 원하는 값/이전 값을 다시 명시 저장하고 후보 빌드를 재생성해야 exe에 반영됨. 기존 UI는 `Assets/Resources/UITuning.asset`이며 변경하지 않음. Scene 배치도 변경 없음.
- **주요 파일·보존:** 신규 Q6NightBuild/Q6CandidateLabel(후보 define에서만 실행·표시 참조 바인딩)/후보 Asset, Q7NightBuild/Q7ExeCapture(`-capture` 없으면 비활성), TASKS/README/Validation. 기존 게임 로직·Scene·Prefab·Tuning·Packages·ProjectSettings baseline diff0, 원본 튜닝 SHA256 일치([보존 검사](Validation/Q-night4-preservation.txt)). Q7 파일 존재 기반 PASS는 검정 화면 QA로 무효화해 KNOWN-LIMIT으로 기록. 실행하지 않은 검증은 통과로 기록하지 않았음.
