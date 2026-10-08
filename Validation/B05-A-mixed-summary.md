# B05 A지역 혼합 빌드 — DONE

`Builds/B05-V2-A/Afterglow-B05-V2-A.exe`에서 A01_V2→A05_V2 실제 Dash 획득→A03_V2 G-D→기존 B01까지 8구간 고정 입력 연속 재생 PASS. 시작 이후 위치 배치·능력 지급 없이 실행했다. 입력은 `Routes/V2/V2-A-continuous.json`, 원문은 `_raw/B05/continuous-A-delivery-replay.txt`.

```
MEASURE start deviation V2-R06=0.121u; no placement
PASS V2-R05 prescribed fixed tick replay room=A05 pos=(46.89, 2.27)
PASS V2-R08 prescribed fixed tick replay room=B01 pos=(3.61, 1.81)
Error/Exception/Assert=0
```

기본 빌드 회귀는 `_raw/B05/base13/REG-B05base-summary.txt`의 기본13/13 및 기존 RouteAll-60fps 1회 PASS. PogoTest 추가 반복은 요청 범위에 포함하지 않아 미실시. 기존 입력·방·튜닝 변경0. 혼합 빌드는 당시 A지역 마무리 증거로 보존했으며 사용자 플레이에는 최신 전체 V2-delivery 실행기를 권장한다.

실패 원문은 방별 요약에 보존했다. 사람 시간·난이도·스타일은 A지역 REVIEW 유지.
