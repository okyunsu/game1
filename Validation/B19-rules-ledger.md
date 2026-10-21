# B19 T5 — 검사 규칙 근거 장부

Batch19는 검사 판정만 정리한다. Design·Scene·PlayerTuning과 게임 능력·진행 규칙을 바꾸지 않는다. WORLD_V5 3절/9절의 기존 FAIL 표기와 이번 검사 WARN이 다른 항목은 Batch19의 명시 지시(실제 버그 근거 없는 FAIL을 WARN/MEASURE로 낮춤)를 적용한 것이다. 설계 원문은 그대로 보존한다.

## FAIL — 실제 배치 버그와 연결한 검사

|규칙/구현|근거가 된 실제 사례|이번 검증|
|---|---|---|
|머리 위 clearance / headroom / edge-headroom|draft2 A01 solid7↔platform0, A02 solid13↔platform0, A06 solid6↔platform2, A06 solid7↔platform3. A02 가장자리 머리 걸림은 B18 T1 물리 재현 및 WORLD_V5 10·11절|draft2 Scene 머리 위 FAIL4 보존, draft3 FAIL0. [T2](B19-clearance.md)|
|room-height|A05 높이10에서 카메라가 고정된 B17 T1 측정. WORLD_V5 9절의 높이12 반영|높이10 fixture 검출. [T3](B19-room-rules-v5-A.md)|
|covered-trench|WORLD_V5 9절: A04·A05 도랑 위 바닥이 덮여 도랑이 사라진 실제 데이터 오류|덮인 도랑 fixture 검출. [T3](B19-room-rules-v5-A.md)|
|적 배치: enemy-inside-solid / enemy-spawn|A02 선반의 solid14 안에 적이 박힘(B18 T1·T2)|draft2 FAIL1 재검출·draft3 0. [T2](B19-clearance.md)|
|같은 적 배치 불변식: enemy-support / patrol-support / patrol-end|위 A02 적 배치 오류와 WORLD_V5 9절의 A04 적 x13/순찰[4,12] 배치 불일치가 점검 배경. **지지면·순찰 지지 실패 자체를 실제 버그로 별도 재현한 이력은 없다.** Batch19 T2가 이 검사들을 명시적으로 FAIL 유지하도록 지시함|기존 분류 유지. enemy-support/patrol-support 위반 fixture 검출, 실제 draft3 이상0. 별도의 새 FAIL 규칙으로 추가하지 않음|

검사 실행에 필요한 prefab collider가 없는 경우의 `enemy-shape missing`, 격리 저장 경로 누락, fixture 기대값 불일치 등은 검사 자체의 기술 실패다. 위 게임 배치 규칙의 개수와 혼합하지 않는다. 지지면·순찰 검사는 독립된 실제 버그가 있었다고 꾸며 쓰지 않고, 사용자의 명시 유지 지시와 실제 재현 범위를 구분했다.

## WARN / MEASURE — 실제 버그 근거 없음·포락선 한계·취향

|규칙/분류|현재 등급|이유/출력 유지|
|---|---|---|
|closed-door / side / pit-edge clearance|MEASURE|닫힌 문·벽 옆·구덩이 가장자리의 확장 몸 겹침. draft3 6/43/20건을 방별로 보존|
|side-gap|WARN|draft2·draft3 모두0, 폭0.5 fixture만 WARN1. 실제 끼임 버그 근거 없음|
|door-safety|WARN(기존 FAIL→WARN)|문에서2u 안전거리 위반 fixture는 있으나 실제 버그 원문 없음|
|overlap|WARN(기존 FAIL→WARN)|일반 JSON 문/블록 겹침 후보. 실제 머리 걸림은 별도 headroom FAIL로 계속 검출|
|ability-gate|WARN(기존 FAIL→WARN)|알 수 없는 gate 문자열은 정적 검토 대상. 실제 잘못된 gate 사례 없음|
|height|WARN(기존 FAIL→WARN)|일반 단차2u 포락선은 능력·포고·실제 점프와 다름|
|arrival / reachability 실패 후보|WARN(기존 FAIL→WARN)|벽 가림·공중 조작·능력·입력 타이밍을 완전히 풀지 않는 정적 계산. 성공 측정행은 MEASURE 유지|
|pit-patrol / pit-width|WARN(기존 FAIL→WARN)|구덩이 폭3u·순찰 교차는 현재 실제 버그 근거 없음. fixture는 계속 검출|
|size / density / floor-item / horizontal-gap / ascent|WARN 유지|방 크기·밀도·아이템 분포·일반 이동 포락선·구성 취향|
|pogo-chain / pogo-only-climb|WARN 유지|등불 연속·작은 발판 없는 오름은 플레이 피로를 볼 후보|
|무능력/능력 조건 도달 노드·pogo-route|MEASURE|A02 WallJump 조건, A06 유일 입구 DJ 조건을 명시. 첫 방문 소유나 실제 자연 완주 주장 아님|
|일방 발판·정상 적 지지·정상 순찰 끝·방별 개수|MEASURE 유지|관찰값. 현재 V4/V5 발판은 고체이며 일방 발판은0|

근거 없이 남긴 독립적인 게임 규칙 FAIL은 없다. 적 지지·순찰 FAIL은 위 명시 유지 예외로 분리했으며, 독립된 실제 재현이 있었다고 주장하지 않는다. 분류 변경 후에도 위반 fixture4개(머리 여유·적 박힘·적 지지·포고 연속)를 기대 등급으로 재검출한다. 온전한 seal 횡단 fixture는 FAIL/WARN 모두0이어야 통과하도록 확인한다.

실제 검증·최종 인계·링크 점검 결과는 [B19 요약](B19-summary.md)에 기록한다. 예전 Validation을 수정하지 않는다. 새 플레이 콘텐츠나 수치 후보는 이번 범위에 없다.

T5 실제 검증: Python 컴파일 및 fixture4 기대 등급 재검출, 온전한 seal 후보0, 문서10개 링크 오류0·보호380파일 SHA변경0·결정 대기0, Quiet REGRESSION13/13. [정적 원문](_raw/B19/T3-final-ledger-selftest.md), [문서·보호 원문](_raw/B19/document-protection.txt), [회귀](_raw/regression/B19-T5/result.txt).
