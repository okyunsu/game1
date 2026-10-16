# B17 T2 — draft2 V5 6방 재생성

상태 REVIEW(기술 기준 통과, 방 구조·플레이 감각 수락 대기). Design 수정0. `V5Batch17.Generate`→기존 `B09Authoring.Generate`를 V5 폴더에만 실행. V4 Scene/Resources·Prefab·Tuning·기존 V4 런처·Design 보호 SHA256 변경0.

|방|고체|발판|적|등불|가시 포고|hazard|문|봉인|벽|조각|바닥 구덩이|
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
|A01|9|3|2|2|0|0|3|1|0|0|1|
|A02|16|3|2|1|0|0|5|0|0|2|1|
|A03|13|5|4|6|0|0|5|0|0|0|2|
|A04|11|2|3|5|0|1|2|0|0|1|1|
|A05|11|0|1|0|0|1|3|1|1|0|1|
|A06|11|5|3|0|3|1|2|0|0|1|0|
PASS six scenes; all element counts match; six shallow pits; hazard trenches have no covering floor >0.4u


모든 요소는 JSON 배열과 실제 Scene 컴포넌트 수를 각각 비교했다. 구덩이는 y0..0.4 고체 중 hazard와 수평으로 겹치지 않는 구간이다. A01 1/A02 1/A03 2/A04 1/A05 1=6, A06은 가시 도랑만 있어0. A04 x22..40·A05 x6..12는 y0..0.4 도랑 바닥만 존재하고 그 위를 덮는 y0 시작 바닥 고체0. A04 섬은 y0.4에서 시작하므로 유지한다.

A05 높이12, A04 world y11..23, A03 위 문 로컬y13을 데이터 그대로 사용했다. `Assets/Maps`는 허용 범위 밖이라 바꾸지 않았다. 전달 exe 지도는 V5 전용 Editor build callback이 readonly draft2 world를 빌드 Scene 복사본의 TextAsset으로 삽입한다(`-b17-draft2-map`). 원본 Scene·V4·Resources·지도 Asset 변경0. T3에서 실제 지도 좌표까지 확인한다.

생성 컴파일 오류0·실행 exit0. [생성 원본](_raw/B17/T2-generation.log), [요소표](_raw/B17/T2-counts.md), [보호 기준선](_raw/B17/protected-before.json). Quiet 결과는 종료 후 추가한다.

Quiet B17-T2: REGRESSION 13/13.
