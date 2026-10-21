# B19 T1 — 후보 재분류

상태 DONE. 기존 Unity Scene 원문을 읽기 전용으로 재분류했다. draft2 데이터는 `git show B17:Design/v5/rooms_v5_A.json`으로 임시 폴더에 읽은 뒤 폴더를 삭제했다. Scene·설계·수치 변경 없음. Base `8a9a1b5`.

후보 단위는 B18의 샘플×차단 콜라이더 행이다. overhead는 blocker minY > feet + 접촉 오차0.003, 문은 closed:, pit-edge는 발 높이≤0.4, 나머지는 측면이다. 양옆 틈은 서로 세로1.6u 이상 겹치는 구조물 사이 양의 가로 틈<0.9u를 별도 측정한다(몸 폭0.7+여유0.2). 새 FAIL 규칙이 아니다.

|버전|방|머리 위(a)|닫힌 문(b)|측면(c)|구덩이 가장자리(d)|양옆 틈(e)|기존 후보 합|
|---|---|---:|---:|---:|---:|---:|---:|
|draft2|A01|1|0|5|2|0|8|
|draft2|A02|1|3|8|2|0|14|
|draft2|A03|0|3|6|4|0|13|
|draft2|A04|0|0|6|6|0|12|
|draft2|A05|0|0|8|4|0|12|
|draft2|A06|2|0|13|2|0|17|
|draft2|합계|4|6|46|20|0|76|
|draft3|A01|0|0|5|2|0|7|
|draft3|A02|0|3|5|2|0|10|
|draft3|A03|0|3|6|4|0|13|
|draft3|A04|0|0|6|6|0|12|
|draft3|A05|0|0|8|4|0|12|
|draft3|A06|0|0|13|2|0|15|
|draft3|합계|0|6|43|20|0|69|

draft2 머리 위 네 사례: A01/solid7/platform0, A02/solid13/platform0, A06/solid6/platform2, A06/solid7/platform3.

```text
MEASURE draft2 overhead=4 closedDoor=6 side=46 pitEdge=20 sideGap=0
MEASURE draft3 overhead=0 closedDoor=6 side=43 pitEdge=20 sideGap=0
```

원문: [draft2](_raw/B19/draft2-categories.txt), [draft3](_raw/B19/draft3-categories.txt). T1은 로그 분석이며 새 Unity Scene 실행은 T2에서 수행한다. Quiet 회귀 실제 REGRESSION 13/13: [원문](_raw/regression/B19-T1/result.txt).
