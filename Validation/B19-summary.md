# Batch 19 — V5 층 A 검사 판정과 DJ 경로 원인

종료 2026-10-22 00:21 KST. T1~T6 DONE6. Task별 Quiet 모두13/13·종료 기본13/13·V5 런처3 exit0/Error0. 최종 Batch Card 원문은 종료 커밋 뒤 이 파일 맨 위에 추가한다.

Base `8a9a1b53e56934ba22837dffe4605a5b7a154b05`, 재개2026-10-21 23:46 KST. 시작 조건용 Batch19/WORLD_V5/첫 실패 기록은 사용자가 이미 이 커밋에 저장했고 B18은 조상이다. 앞선 실패 기록2개를 보존한 상태에서 사용자가 명시적으로 재개를 승인했다. 정상 LOCALAPPDATA의 Unity 시작 검사 exit0을 재사용했다. 예산6 Task/최대7 Task·3시간, 다음 Task 시작 제한2시간30분 이내. 새 제품 기능·Scene·게임 수치 변경은 없다.

|Task|유형|상태|로컬 커밋|주요 결과|
|---|---|---|---|---|
|T1|verification|DONE|`b22d23c`|[후보 분류](B19-candidates.md): draft3 머리 위0/문6/측면43/구덩이20, draft2 머리 위4·양옆 틈 양쪽0. Quiet13/13|
|T2|feature|DONE|`7ddf894`|[Scene 검사](B19-clearance.md): draft3 FAIL0/WARN0, draft2 기존 머리 위4/적1 검출, 0.5 틈WARN1. 컴파일0·임시Scene 삭제·Quiet13/13|
|T3|feature|DONE|`4497104`|[능력 조건](B19-room-rules-v5-A.md): 원래22건→0(새FAIL0). WallJump 조건9/DJ13. 무능력 계산 유지·fixture4·Python 컴파일·Quiet13/13|
|T4|verification|DONE|`a736fbe`|[DJ 측정](B19-dj-route.md): 원인=입력 타이밍, 목표 좌표 일치. 실제exe7200프레임·120초, DJ 상승4.884u·Error0·Quiet13/13|
|T5|doc|DONE|`11297f1`|[규칙 장부](B19-rules-ledger.md)·TASKS·README 인계·링크0·보호380 SHA0변경·결정0·Quiet13/13|
|T6|verification|DONE|`B19`|Quiet13/13·기본13/13·V5 런처3 exit0/Error0·보호380 SHA0변경·허용 범위 밖0|

유형: verification3 / feature2 / doc1. 이번 배치에서 플레이 가능 범위를 확장하지 않았다. 대신 실제 머리 걸림 검출을 유지하면서 정상 벽 옆69개를 오류로 세는 리스크와 능력 미반영22개를 제거했고, DJ 자동 경로 정지 원인을 측정했다. 이전 B18-T3/T6와 기존 체감·설계 REVIEW는 그대로다.

|현재 draft3 검사 행|이전 FAIL/WARN/MEASURE|이후 FAIL/WARN/MEASURE|해석|
|---|---|---|---|
|실제 Scene 검사 전체|69 / 0 / 52|0 / 0 / 121|동일69 후보를 문·측면·구덩이 MEASURE로 보존. 나머지는 정상 적/순찰/요약 등|
|JSON 정적 검사 전체|22 / 0 / 21|0 / 0 / 21|능력 조건 도달성 적용. A02 WallJump 조건9건·A06 유일 입구DJ 조건13건 해소|
|Scene 기존 clearance 후보만|69 / 0 / 0|0 / 0 / 69|머리 위0·닫힌 문6·측면43·구덩이20. side-gap은0|

규칙 종류: 기존 근거 없는 정적 FAIL8종(door-safety/overlap/ability-gate/height/arrival/reachability/pit-patrol/pit-width)은 WARN으로 낮췄다. 현재 데이터에 이 WARN이0인 것과 규칙 자체를 삭제한 것은 다르다. 실제 위반 fixture는 계속 검출한다. 머리 여유·방 높이·도랑 덮임·적 박힘 FAIL은 유지한다. 적 지지면·순찰 불변식은 T2의 명시 유지 예외이며 독립적인 실제 실패가 있었던 것으로 꾸며 쓰지 않았다. [근거 장부](B19-rules-ledger.md).

T3 사라진22건의 각 원문과 원인은 [T3 표](B19-room-rules-v5-A.md)에 있다. 잔여FAIL0·새FAIL0. 무능력으로는 A02 후보9/A06 후보13이 유지되며 현재는 WARN이다. 조건부 그래프는 벽 가림·입력·적 이동을 시뮬레이션하지 않는다. A02 첫 도착의 WallJump 소유나 전체 경로 완주를 주장하지 않는다.

T4 원인 분류는 입력 타이밍. waypoint는 draft3와 일치한다. 공중에서 다음 목표로 넘어가 platform2 옆면 접촉 후 바닥으로 떨어지고 x24.947에서 수직 점프를 반복한다. 정지한 바닥1→발판8.8은 상승7.8이라 DJ4.884만으로 복구하지 못하지만 앞 발판6.8→8.8은 오름2/간격1.3이다. 마지막 발판11.6+DJ4.884=정점16.484로 문 y15 높이는 충분하다. 자연 A01→A06 완주와 실제 문 통과는 미확인이고, 입력 타이밍 개선·지형 변경은 하지 않았다.

검증 도구 실패는 별도 기록했다. Editor 경로는 Build Profile에 V5 Scene이 없어 `FAIL System.Exception: Timeout room ready`와 Scene 로드 실패로 종료했다. ProjectSettings를 건드리지 않고 임시 Development 빌드로 접근을 바꿨다. 실제exe 측정2개는 exit0/Error0. [실패·재측정 원문과 수치](B19-dj-route.md).

T6 Quiet 원문은13/13 PASS였지만 추가 실행 래퍼가 `$LASTEXITCODE`로 성공 여부를 다시 판정해 `B19-T6 Quiet regression failed`를 던졌다. 성공 시 명시적인 프로세스 종료 코드를 보장하지 않는 PowerShell 스크립트에 부적절한 검사를 붙인 도구 오류다. Quiet 결과는 [원문](_raw/regression/B19-T6/result.txt)을 따르며, 래퍼 중단 때문에 시작되지 않았던 종료 기본 회귀는 별도 실행했다. [래퍼 오류 원문](_raw/B19/T6-wrapper-failure.txt). 제품 회귀나 재실행한 Quiet 검사로 세지 않는다.

실행 권한: Unity·Unity 호출 스크립트는 정상 사용자 환경의 샌드박스 밖, 편집·JSON 분석·문서·범위 확인은 샌드박스 안이다. Task1 로컬 커밋은 `.git/index.lock: Permission denied`로 실패했고, 이후 사용자가 Git add/commit/tag만 추가 예외로 승인했다. push는 하지 않는다. Base 이전 미push 커밋8a9a1b5가 origin/main..HEAD에 있으므로 이번 배치를 squash하지 않는다. 종료 태그는 B19다.

열린 항목: 기존 제품 버그3개·V5 설계/체감 수락 대기·DJ 자동 입력 미완주. D-V5-CLEARANCE는 이번 승인된 분류/판정으로 해소했고 결정 대기0. WORLD_V5의 A06 적 표기는 사용자 시작 커밋에서3으로 수정되어 JSON3과 일치한다.

사용자가 할 일: 기존 수락 대기 순서대로 [Play-V5](../Launchers/current/Play-V5.cmd)의 A02 블록/선반 적·작은 발판 난이도, [DJ](../Launchers/current/Play-V5-DJ.cmd)의 A06 가시 포고·조각·일방 문을 플레이한다. 전체 목록은 [Latest Handoff](../TASKS.md#9-latest-handoff--batch19). 기존 빌드 B18/B12와 런처를 유지한다. 새 T4 검증 코드가 기존 B18 배포 exe에 들어갔다고 주장하지 않는다.

다음 Batch 후보: 승인 시 DJ 검증 입력의 착지 확인·이륙 타이밍·낙하 후 복구를 개선한다. 사용자 체감 수락·D-NEXT 전체 플레이는 유지한다. 자동 경로의 실패를 근거로 Scene/능력 규칙을 바꾸거나 층 B/C를 확장하지 않았다. Batch Backlog 없음·Approved Backlog 소진.

T6 실제 검증: [Task별 Quiet](_raw/regression/B19-T6/result.txt), [종료 기본 회귀](_raw/regression/B19-final-default/result.txt), [V5 세 런처 결과](_raw/B19/T6-launch-result.txt). 세 모드는 기존 공통 Start-V5.ps1의 Smoke 경로를 각각10초 실행했으며 고유 fixture 저장 슬롯을 사용했다. NewGame A01/AfterDash A03 dash/DJ A01 dj 모두 Error0. 새 배포 빌드 생성은 없고 측정용 Development 임시 빌드만 만들었다가 삭제해 B18/B12 두 빌드만 남겼다. Scene 임시 폴더도 삭제됐고 raw 로그·입력·프레임·측정 원문은 삭제하지 않았다.

보호380파일 SHA변경0, 제품 코어·Design·기존 Scene/Prefab/수치 에셋·V4 런처·V5 런처 Git 변경0. Validation B18 이전 요약도 변경0. 문서10개 링크 오류0·결정 대기0·기존 제품 버그3개를 유지한다. 단, 자동 검사 PASS가 기존 사람 수락 대기를 DONE으로 바꾸지는 않는다. B19 태그는 최종 인계 커밋에 붙이고 Task별 로컬 커밋을 보존한다.
