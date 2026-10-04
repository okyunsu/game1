# N5 최종 인계 — 2026-10-04

**1부 PASS, S2-04 REVIEW 복귀. T-A~T-G 모두 도구를 제작·실행했다.** 실행 기록 기준 약 69분(15:17~16:26 KST; 검토 시간 별도), 단계별 회귀 총 **104/104 PASS**. `main`, 단계별 로컬 커밋, push 없음. 완료 기준을 통과하지 못한 확장 검증은 아래에 명시했다.

| 항목 | 실제 결과 |
|---|---|
| 1부 | TryLoad catch에 InvalidDataException만 추가. 손상 4종 실제 exe 키보드 새 게임·원본 보존/Error0, 기존 저장 검사 PASS. [증거](N5-Part1-save-fix.txt) |
| T-A | **부분 PASS / KNOWN-LIMIT**. Ending 새 프로세스 Continue→타이틀→재Continue PASS. 읽기 전용 파일 검사는 지형 앞 이동 실패 2회, 권한 복원. 폴더 ACL·실패 후 다음 저장은 미검증. [증거](N5-TA-save-failure.txt) |
| T-B | **PASS**. HP20/첫 패턴/출구 잠금, 물리 사망→CP-C04→재진입 3회, 실제 가상 입력 처치→엔딩/Error0. 최초 처치 실패와 1회 보정 보존. [증거](N5-TB-boss-retry.txt) |
| T-C | **PASS(짧은 경로)**. A01→A02 tick299, 60fps 3회+30/120 재생 모두 진입 틱 차0·위치 차0. 사용자 관찰 녹화는 최종 exe에서도 JSON 저장 확인. 임의 전체 경로 재생은 별도 검증 필요. [증거](N5-TC-record-replay.txt), [기준 녹화](N5-TC-recording.json) |
| T-D | **부분 PASS / KNOWN-LIMIT**. 실제30/60/120/144fps, A05/G-J/B04/C02/C03 32칸 결과 일관됨. G-D 8칸은 시작 높이가 선반보다 낮아 무효; 보정 한도 종료, 제품 버그로 판정하지 않음. [행렬](N5-TD-fps-matrix.txt) |
| T-E | **KNOWN-LIMIT**. Pause 고정틱 대기 실패→실시간 대기 보정 후 A04에 없는 KillZone 선택으로 재실패. **60분·메모리/오브젝트 +15%·300/50/100 행동 기준 미검증**. [증거](N5-TE-soak.txt), attempt1/2 CSV 보존 |
| T-F | **PASS / 포커스 KNOWN-LIMIT**. D3D11 7장면×4해상도×창/전체 화면=56 PNG, D3D12 시작 probe1장, 총57 유효 PNG·분산>0·Error0. 실제 Screen/GUI/보스·예고 뷰포트 기록. 사망 경로 1회 보정, 원래 실패 증거 보존. 강제 focus 상실/복귀는 미검증. [최종 결과](N5-TF-display-final.txt) |
| T-G | **빌드/손상/최종 재생 PASS, 자연 완주 KNOWN-LIMIT**. S4-pre3 errors0/warnings0. 새 게임에서 두 능력 자연 획득·G-D 통과까지 확인했지만, 1회 보정 후 B01에서 B05 대신 B02 진입으로 재실패. **새 게임→엔딩 자동 완주 미검증**. [증거](N5-TG-final.txt) |

회귀 통과는 위 미검증 조건을 대신하지 않는다. S2-04와 U1/F2R/LOGR 및 기존 S3 감각·설계 REVIEW, Sprint3 Gate 사용자 판단은 유지한다. Q1-SAVE-001은 수정·재검증으로 해결됐으며, N5에서 새 제품 버그로 확정한 항목은 없다.

## 변경·검증 범위

- 기존 게임 코드: `Assets/Scripts/ProgressSave.cs` catch 한 줄. 새 도구: `Assets/Scripts/Verification/` 런타임9개+Editor2개. 공통 입력/마커/증거 기록 포함.
- 보호 대상 Scene/Prefab/모든 기본 튜닝/Camera/적·보스·이동 코드/Packages/ProjectSettings/저장 필드·버전은 기준 `483a6ad` 대비 불변. 신규 도구는 플래그가 없으면 생성되지 않는다.
- T-D 구간 시작 설정, T-D/T-E 방 사이 초기 설정 및 T-E KillZone 설정 외에는 위치를 직접 바꾸지 않는다. 최종 자연 경로는 입력만 사용했다. 촬영 빌드의 시작 씬 순서만 바꿨으며 Scene 파일은 수정하지 않았다.
- [최종 보호·회귀 검사](N5-final-checks.txt), [시각 기록](N5-progress.txt), [승인 문서](N5-approved-request.txt). 첨부 원문은 최초 읽은 전문으로 보존했다. 이후 Downloads 경로 조회에서 찾지 못해 원본 파일을 다시 복사하지 못했다.

## 도구 명령

프로젝트 루트 PowerShell, `EXE=Builds/S4-pre3/Afterglow-S4-pre3.exe`를 뜻한다. 모든 슬롯/출력은 새 경로를 사용한다. fixture 준비 및 녹화 3줄은 README 참고.

```text
EXE -record Logs/user.json -save-path Logs/user-save.json
EXE -replay Validation/N5-TC-recording.json -save-path <초기 CP-A01 슬롯> -verify-output Validation/replay-new.txt
EXE -record Logs/test-route.json -verify-record-route -save-path <초기 CP-A01 슬롯> -verify-output Validation/record-new.txt
EXE -verify-save-failure ending -save-path <Ending 도달 슬롯> -verify-output Validation/ending-new.txt
EXE -verify-boss-retry -save-path <CP-C04 두 능력 슬롯> -verify-output Validation/boss-new.txt
Unity.exe -batchmode -projectPath <프로젝트> -executeMethod N5Editor.Run -verify-fps-matrix -save-path <CP-A01 두 능력 슬롯> -verify-output Validation/matrix-new.txt
EXE -verify-soak 60 -save-path <CP-A01 두 능력 슬롯> -verify-csv Validation/soak-new.csv -verify-output Validation/soak-new.txt
Builds/N5-TF-Menu/Afterglow-N5-TF-Menu.exe -force-d3d11 -screen-width 1280 -screen-height 720 -screen-fullscreen 0 -verify-display start -save-path Logs/capture-new.json -verify-shot Validation/capture-new.png -verify-output Validation/capture-new.txt
EXE -verify-route -save-path Logs/route-new.json -verify-output Validation/route-new.txt
```

`-verify-soak`, 쓰기 실패 이동 분기, G-D 행렬, 자연 전체 경로는 현재 도구 한계가 있으며 통과 도구로 취급하지 않는다. 무한 보정하지 않았고 후속 수정은 새 승인 범위에서 판단한다. `N5-TF-display.txt`는 최초 행렬 증거, `-final.txt`가 보정 포함 최종 결과다. D3D12는 시작 probe만 검증했다.

## 사용자가 확인할 것

1. S4-pre3로 직접 새 게임→엔딩 완주. 가능하면 README의 `-record`로 녹화한다. 실제 사용자 입력·체감·자연 전체 플레이 시간은 자동 검사와 구분한다.
2. 기존 Q6-A/B/C exe를 비교한다. 후보 기본값 채택은 승인하지 않았고 원본 FeedbackTuning을 유지했다.
3. [A01 시작](N5-TF-shots/1280x720-0-start-D3D11.png), [보스 예고](N5-TF-shots/1280x720-0-boss-D3D11.png), [2560 전체 화면 사망](N5-TF-shots/2560x1440-1-death-r2-D3D11.png), [엔딩](N5-TF-shots/2560x1440-1-ending-D3D11.png)을 포함한 57장을 확인한다.

수정 위치는 기존 README의 Designer Control 표를 따른다. Scene의 Play 중 변경은 Stop 후 다시 입력·저장하고, 설정 Asset은 Stop 후 원하는 값 또는 이전 값으로 명시적으로 저장한다. 이번 큐는 종료했으며 다음 Sprint 콘텐츠로 확장하지 않는다.
