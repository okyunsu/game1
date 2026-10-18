# B18 T3 — draft3 Scene 再生성

|방|고체|발판|작은 발판|적|등불|가시 포고|hazard|문|봉인|벽|조각|구덩이|
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
|A01|9|5|2|2|0|0|0|3|1|0|0|1|
|A02|15|5|3|2|0|0|0|5|0|0|2|1|
|A03|13|10|5|4|1|0|0|5|0|0|0|2|
|A04|11|3|1|3|4|0|1|2|0|0|1|1|
|A05|11|0|0|1|0|0|1|3|1|1|0|1|
|A06|11|5|1|3|0|3|1|2|0|0|1|0|

6개 Scene 생성 성공. 모든 칸은 실제 Scene 요소 수를 JSON과 비교한 값이며 불일치 시 생성 검사가 예외로 중단된다. A05는 draft2와 데이터가 같아 재저장 후 Git diff가 없다. 등불 A03=1/A04=4, A06 가시 포고=3, 작은 발판 총12, 적15, 구덩이6. V4·Resources·Prefab·튜닝·V4 런처 보호 파일 SHA 변경0. Design 수정0.
Development 빌드 Succeeded errors=0 warnings=0. 실제 exe의 문14방향·도착·stub4·일방·능력 문 차단·적15 순찰 연결/재생성·조각4·대시 수집/저장·Continue 복원 PASS, Error0. 감각·방 문법 수락은 사용자 대기이므로 REVIEW.
원문: [요소 수](_raw/B18/T3-counts.md), [빌드](_raw/B18/T3-build.txt), [문 검사](_raw/B18/T3-doors.txt).

```text
MEASURE scene=A01 patrol=2 shards=0
MEASURE scene=A02 patrol=2 shards=2
MEASURE scene=A03 patrol=4 shards=0
MEASURE scene=A04 patrol=3 shards=1
MEASURE scene=A05 patrol=1 shards=0
MEASURE scene=A06 patrol=3 shards=1
PASS physical blocked transition0 A01-Z01
PASS physical blocked transition0 A02-B05
PASS physical blocked transition0 A03-B01
PASS physical blocked transition0 A05-Z01
PASS physical blocked transition0 A02-A06
PASS physical blocked transition0 A04-A05
PASS physical blocked transition0 A01-A06
MEASURE door contact=A01-A02 source=A01 player=(47.50, 1.05)
MEASURE door=A01-A02 destination=A02 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A01-A02 source=A02 player=(0.50, 1.05)
MEASURE door=A01-A02 destination=A01 spawn=(44.50, 1.00, 0.00) player=(44.50, 1.01)
MEASURE door contact=A02-A03 source=A02 player=(27.50, 1.05)
MEASURE door=A02-A03 destination=A03 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A02-A03 source=A03 player=(0.50, 1.05)
MEASURE door=A02-A03 destination=A02 spawn=(24.50, 1.00, 0.00) player=(24.50, 1.01)
MEASURE door contact=A02-A03U source=A02 player=(27.50, 9.05)
MEASURE door=A02-A03U destination=A03 spawn=(3.50, 9.00, 0.00) player=(3.50, 9.01)
MEASURE door contact=A02-A03U source=A03 player=(0.50, 9.05)
MEASURE door=A02-A03U destination=A02 spawn=(24.50, 9.00, 0.00) player=(24.50, 9.01)
MEASURE door contact=A03-A04 source=A03 player=(49.50, 13.05)
MEASURE door=A03-A04 destination=A04 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A03-A04 source=A04 player=(0.50, 1.05)
MEASURE door=A03-A04 destination=A03 spawn=(46.50, 13.00, 0.00) player=(46.50, 13.01)
MEASURE door contact=A04-A05 source=A04 player=(46.50, 0.05)
MEASURE door=A04-A05 destination=A05 spawn=(46.50, 9.30, 0.00) player=(46.50, 9.25)
MEASURE door contact=A05-A03 source=A05 player=(0.50, 1.05)
MEASURE door=A05-A03 destination=A03 spawn=(46.50, 1.00, 0.00) player=(46.50, 1.01)
MEASURE door contact=A05-A03 source=A03 player=(49.50, 1.05)
MEASURE door=A05-A03 destination=A05 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A02-A06 source=A06 player=(47.50, 5.05)
MEASURE door=A02-A06 destination=A02 spawn=(3.50, 21.00, 0.00) player=(3.50, 21.01)
MEASURE door contact=A01-A06 source=A01 player=(32.00, 15.05)
MEASURE door=A01-A06 destination=A06 spawn=(36.85, 1.00, 0.00) player=(36.85, 1.01)
MEASURE door contact=A01-A06 source=A06 player=(32.00, 0.05)
MEASURE door=A01-A06 destination=A01 spawn=(32.00, 13.30, 0.00) player=(32.00, 13.25)
PASS Continue restores V5 checkpoint and Dash
Error/Exception/Assert=0
```

T3 REVIEW. Quiet 실제 REGRESSION 13/13. [원문](_raw/regression/B18-T3/result.txt). WORLD_V5 4절 A06 적1 표기와 JSON 적3 불일치는 Claude 정리 항목이며 Scene은 JSON과 일치.
