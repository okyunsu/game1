```text
B20-T1 Quiet REGRESSION 13/13
B20-T2-final Quiet REGRESSION 13/13
B20-T3 Quiet REGRESSION 13/13
B20-T4-final Quiet REGRESSION 13/13
B20-T5 Quiet REGRESSION 13/13
B20-BL1 Quiet REGRESSION 13/13

S1-02 PASS | S1-02 PASS evidence=S1-02-night-S2-B20-closing-default.txt
S1-04 PASS | S1-04 PASS evidence=S1-04-night-S2-B20-closing-default.txt
S1-07 PASS | S1-07 PASS evidence=S1-07-night-S2-B20-closing-default.txt
S1-08 PASS | S1-08 PASS evidence=S1-08-night-S2-B20-closing-default.txt
Camera PASS | Camera PASS evidence=S1-06-lead0-night-S2-B20-closing-default.txt
T1 PASS | T1 PASS evidence=Verification-night-T1-B20-closing-default.txt
T2 PASS | T2 PASS evidence=Verification-night-T2-B20-closing-default.txt
T3 PASS | T3 PASS evidence=Verification-night-T3-B20-closing-default.txt
T4 PASS | T4 PASS evidence=Verification-T4-night-S2-B20-closing-default.txt
S2 PASS | S2 PASS evidence=S2-01-integration-night-S2-B20-closing-default.txt
F2R PASS | F2R PASS evidence=F2R-feedback-night-S2-B20-closing-default.txt
LOGR PASS | LOGR PASS evidence=LOGR-playtest-night-S2-B20-closing-default.txt
Boss PASS | Boss PASS evidence=boss-recovery-night-S2-B20-closing-default.txt
REGRESSION 13/13

BATCH CARD 1a27b1d5646ba03cd21998693552b8cf6cda539a..HEAD (5cb8580)
변경 파일: 44 (+5631 / -29 줄, 바이너리 0)
  Scripts: 19
  Scenes: 2
  Prefabs: 0
  Editor: 6
  Validation: 9
  문서: 2
  기타: 6
허용 범위: Assets/Editor/**, Assets/Scripts/Verification/**, Assets/Scripts/Dev/**, Assets/Scripts/Dev.meta, Assets/Scenes/Test/BodySizeTest.unity, Assets/Scenes/Test/BodySizeTest.unity.meta, Tools/Check-RoomRules.py, Tools/fixtures/**, Launchers/current/*V5*, Launchers/current/Play-BodySize.cmd, Validation/B20-*, TASKS.md, README.md, Batches/BATCH-20.md, Builds/**
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 73 / REVIEW 13 / DOING 0 / BLOCKED 3 / PARTIAL 12
결정 대기(## 2. 결정 대기 표의 D- 행): 2
열린 버그(## 4. 열린 버그 표 행): 6
직접 플레이 확인 대기(## 3. 목록 항목): 7
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 13/13 (Validation/_raw/regression/B20-closing-default/result.txt)
```

# Batch 20 — A05·점프 머리 접촉 측정과 크기 비교 도구

종료 2026-10-23 01:05 KST — 실행8Task 상한으로 종료. 최종 기본 회귀·Batch-Card 원문은 맨 위에 기록했다. Base `1a27b1d5646ba03cd21998693552b8cf6cda539a`, 시작2026-10-22 23:55 KST. 첫 Unity 시작은 정상 LOCALAPPDATA에서도 exit198이었다. 실패 원문을 보존하고 Hub 재실행 뒤23:58 샌드박스 밖 재검사 exit0을 확인해 원래 승인 Batch를 계속했다. 라이선스 파일·계정 설정 변경은 하지 않았다. [최초 실패](B20-blocked.md), [성공 원문](_raw/B20/recheck-20261022-235846/startup.log).

새로 사용할 수 있는 것은 F6 크기 비교 코스와 A01 실제 방 비교 런처다. 기본 크기·게임 규칙·Design·V5 Scene·Player prefab·PlayerTuning·PlayerMotor·RoomSession·RoomCameraRig는 그대로다. 지름길 우회와 무능력 도랑 통과를 재현하고, 점프 머리 접촉·사거리 손실을 수치로 남겼다. draft4 작성과 기본 크기 채택은 사용자/Claude에게 넘긴다.

|Task|유형|상태|결과|
|---|---|---|---|
|T1|verification|DONE|[지름길](B20-shortcut.md): A03→A05 x3.5, 벽 x1..2 온전. 파괴 후 왕복·저장/Continue 유지. **문을 벽 저장 키에 연결하는 새 코드 필요**|
|T2|verification|DONE|[도랑·이동](B20-trench.md): 일반 점프로 대시 없이 통과/피해1, 걷기만 실패·사망. A04/A05 피해·넉백만 있고 입구 복귀 없음|
|T3|verification|DONE|[머리 여유](B20-headbump.md): 크기별86면. h1.6 짧음3/풀만33, h1.3·1.1 짧음0/풀만32. 초기11표본9일치·문 전환2, BL1에서 격리 재확인. 코너 수평 보정 없음|
|T4|feature|REVIEW|[크기 도구](B20-bodysize.md): 3크기×접지/걷기/착지 각10회·막대18/18·포고/E1 각 성공. 새 런처3개 각10초Error0. Release 제외. D-BODY 대기|
|T5|feature|DONE|[규칙](B20-room-rules-v5-A.md): shortcut-closed 1FAIL / spawn-bypass 1FAIL. 고친 fixture FAIL0·기존 fixture4 유지·그 외 새FAIL0. [근거 추가](B20-rules-ledger-add.md)|
|T6|doc|DONE|TASKS·README의 버그/결정/비교 순서/인계 갱신. 문서10개 링크0·결정2/8|
|T7|verification|DONE|기본13회귀13/13·런처6개 각10초exit0/Error0·범위 밖0·깨진 링크0·보호387 SHA변경0|
|BL1|verification|DONE|[전체 머리 접촉](B20-headbump-all.md): 격리36면33일치/3불일치, 초기 문 전환2 재측정 일치. 총8Task 상한 도달|

## draft4용 실제 수치

각3회, u 단위. 턱은 이 고정 입력 recipe에서 성공한 최대 높이이며 다음 실패 경계2.52500, 분해능0.0125u다. 절대적인 인간 입력 한계로 확정하지 않는다.

|항목|최소|최대|평균|
|---|---:|---:|---:|
|일반 점프 최대 성공 턱|2.51250|2.51250|2.51250|
|지상 대시 실제 활성 거리|2.87988|2.87988|2.87988|
|공중 대시 실제 활성 거리|2.87988|2.87988|2.87988|
|지상/공중 대시 활성 중 높이 변화|0|0|0|
|풀 점프 발 상승|2.52002|2.52002|2.52002|
|짧은 점프 발 상승|1.00800|1.00800|1.00800|

PlayerTuning 연속식 풀 점프2.4u / 즉시 cut0.6u도 읽어 기록했다. 실제 짧음은 약0.0333초 누른 recipe다. T3는 실측으로 분류한다. 대시 요청 전후에는 추가 이동이 있어 실제 활성8틱과 별도 기록했다. [T2 상세](B20-trench.md).

|h|전체 면|기존 c<2.2|짧은 점프 충돌 b|풀 점프만 c|충돌 없음 d|
|---:|---:|---:|---:|---:|---:|
|1.6|86|0|3|33|50|
|1.3|86|0|0|32|54|
|1.1|86|0|0|32|54|

최종 실제 접촉은 BL1에서36면33/36(91.67%)이었다. 최초 표본의 불일치2는 문 전환이 섞인 검사 문제였고 격리 재측정에서는 접촉했다. 최종 불일치3은 A02 solid4/solid11, A03 solid5이며 원인 미확정이다.

면의 가장 좁은 샘플을 기준으로 한 측정 후보이며 면 전체의 진행 불가 판정이 아니다. 기존 기준 a는 b/c/d와 별도 지표. 문 주변·벽 윗면·가장자리 ±0.35까지 포함했고 작은 겹침의 예측2곳은 실제 접촉과 불일치했다. Claude의 대략 계산과 차이를 숨기지 않았다.

|h|서기/걷기/점프 착지|막대 예측|포고/E1|공격 길이|아래 공격 길이|
|---:|---|---|---|---:|---:|
|1.6|각10/10|6/6|각 성공|1.10000|1.30000|
|1.3|각10/10|6/6|각 성공|0.89375|1.05625|
|1.1|각10/10|6/6|각 성공|0.75625|0.89375|

공격 판정도 같은 비율로 축소하며 사거리 보정은 하지 않았다. 발 위치·카메라5.5·이동/점프/중력은 유지한다. Release 바이너리에 비교 컴포넌트3개와 새 검증 타입 없음. 시험 도구의 컴파일/포고 입력 실패는 [T4](B20-bodysize.md)에 원문과 해결 범위를 기록했다. 제품 규칙을 바꿔 검증을 통과시키지 않았다.

## 사용자가 할 일

기존 TASKS의 수락 순서 뒤에 약10분 추가한다. [BodySizeTest](../Launchers/current/Play-BodySize.cmd)에서 F6으로 같은 막대·계단·등불·E1을 세 크기로5분 비교한다. 그 뒤 [기본1.6](../Launchers/current/Play-V5.cmd) → [1.3](../Launchers/current/Play-V5-Body13.cmd) → [1.1](../Launchers/current/Play-V5-Body11.cmd) 순서로 A01을5분 비교하고 D-BODY를 고른다. AI는1.3을 비교 우선 후보로 추천하지만 기본 채택하지 않는다.

Claude에게 T1~T3 결과를 전달해 draft4를 작성한다(D-V5-DRAFT4). 지름길은 닫힘 저장 키 연결·벽의 안쪽 공격 제한·도착 위치를 함께 검토해야 한다. 도랑은 실측 일반 점프/대시와 피해 후 복귀 미구현을 고려해야 한다. Scene 반영과 필요한 문 연동 코드는 다음 배치에서 승인한다. 층 B/C 확장·Sprint Gate·push는 하지 않았다.

실행8Task(verification5 / feature2 / doc1), DONE7 / REVIEW1. Backlog2는 8Task 상한으로 미시작, Approved Backlog는 기존 소진 상태다. Backlog의 검증 격리 코드 변경 후 Quiet13/13과 종료 기본13/13을 확인했다.

검증 도구 실패 원문도 보존한다. 아래는 제품 진행 실패가 아니라 첫 문 fixture의 진입 억제 시간, 캡처 모듈 참조, 포고 입력 fixture의 초기화/낙하 타이밍 문제였다. 마지막 실제 exe에서 각 관련 동작과 Error0을 확인했다. 최초 라이선스 exit198은 [별도 원문](B20-blocked.md)에 그대로 있다.

```text
FAIL System.Exception: Timeout door A05
Assets\Scripts\Verification\B20BodyProbe.cs(54,66): error CS0103: The name 'ScreenCapture' does not exist in the current context
FAIL System.Exception: one pogo 1.3
FAIL System.Exception: one pogo 1.6
```

최종 전달 빌드에서도 런처6개 각10초 exit0/Error0을 다시 확인했다([기동 원문](_raw/B20/closing-launchers.txt)). 보호387파일 SHA변경0([원문](_raw/B20/closing-protection.txt)), 문서11개 깨진 링크0·결정2/8([원문](_raw/B20/closing-docs.txt)). 기존 B19 및 이전 Validation 변경0. 새 Scene·Dev 폴더에 필요한 Unity `.meta`도 생성했고 카드 허용 목록에 이 필수 메타 파일을 명시했다. 빌드는 B20/B12 두 개, 별도 새 런처 폴더 없음.

Task별 Quiet는 T1/T2-final/T3/T4-final/T5/BL1 모두13/13이다. T6는 문서 작업이라 추가 게임 실행을 요구하지 않았고 T7에서 기본13/13을 확인했다. 이후 Backlog의 검증 코드가 바뀌어 배치 종료 기본 회귀를 최종 소스로 다시 실행해13/13이었다. 실제 실행한 검증만 적었다.

배치 시작 전 `origin/main..HEAD`의 이전 미push 커밋은0이었다. 지시문 선행 커밋1a27b1d 직후를 Base로 두었으며, 이번 Task 커밋들만 Base에 soft reset하여 합치고 태그 B20을 붙인다. push는 하지 않는다. Task별 합치기 전 커밋 기록은 [Latest Handoff](../TASKS.md#9-latest-handoff--batch20)에 남긴다. 최종 카드 이후 변경은 이 요약의 원문 첨부와 종료 문서뿐이며 제품 소스·Scene·런처·검증 코드는 바꾸지 않는다.