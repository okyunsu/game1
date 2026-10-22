# B20 T5 — 지름길 장벽 규칙 추가 근거

기존 [B19 장부](B19-rules-ledger.md)는 수정하지 않았다. 아래 두 FAIL만 실제 버그 근거로 추가했다. [사용자 CSV](../Playtest/20261022-233114.csv) 9~10행과 [T1 실제 exe](B20-shortcut.md)가 A03→A05에서 온전한 벽을 이미 지난 x3.5 도착을 입증한다.

|규칙|등급|계약·근거|
|---|---|---|
|shortcut-closed|FAIL|shortcut의 장벽이 있는 방으로 들어오는 반대편 문에 `closed_until_wall_broken`을 표시해야 하고 값은 그 방의 실제 벽/봉인 저장 키여야 함. 현재 A03 문에는 없음|
|shortcut-spawn-bypass|FAIL|입구에서 도착점까지의 축 방향이 같은 방의 부서지는 벽/봉인을 완전히 건너면 검출. 입구 복도와 높이/가로가 겹치는 장벽만 검사. A05 spawn x3.5가 x1..2 벽을 이미 지남|
|trench-escape|MEASURE|T2 실측 최대 성공 턱2.5125u를 `--max-step-evidence` JSON으로 받아 도랑/구덩이 턱 깊이와 비교. 현재9곳 깊이0.6u는 일반 점프 탈출 범위. 피해·넉백·머리 충돌·가로 거리 완주를 풀지 않으며 FAIL 아님|

world는 A05=a, A03=b다. Batch 문구의 “반대편”은 T1 실측으로 확인한 A03→A05를 뜻한다고 해석했다. 검사기는 a/b 순서를 하드코딩하지 않고 실제 장벽이 있는 방을 찾는다. 순서를 뒤집은 fixture도 통과한다.

`closed_until_wall_broken`은 **검사 데이터 계약**이다. 현재 생성기/런타임이 이 필드를 지원한다는 뜻이 아니며 T1 결론처럼 다음 배치 코드 연결이 필요하다. Design 파일에는 이 필드를 추가하지 않았다. 저장 스키마 변경도 없다.

[고친 fixture](../Tools/fixtures/B20-shortcut.json)는 반대편 닫힘 키와 벽 바깥 도착 x0.5를 표시한다. 누락 키·존재하지 않는 키·벽 우회·봉인 우회도 각각 검출했다. 기존 네 fixture 검출과 clean fixture 결과를 유지했다. 실제 draft3는 두 규칙이 각1건 FAIL이며 다른 새 FAIL은0이다. [검사 보고서](B20-room-rules-v5-A.md).

실제 전체 draft3 복사본도 위 두 필드만 고쳐 전체 정적 검사 FAIL0을 확인했다. Design은 수정하지 않았다. [검증 원문](_raw/B20/T5-proof.txt):

```text
PASS current draft3 baseline FAIL=0 new shortcut-closed=1 shortcut-spawn-bypass=1 otherNewFAIL=0
PASS fixed draft3 full report FAIL=0; only checker fixture modified; Design unchanged
PASS B20 fixed shortcut fixture FAIL=0; missing lock/bad key/bypass/seal each detected; a/b order independent
PASS draft2 five synthetic rules detected; draft3 four JSON fixtures detected; clean fixture PASS
REGRESSION 13/13
```

Python 실제 import/실행·기존 fixture4·추가 fixture 검출 및 [Quiet13/13](_raw/regression/B20-T5/result.txt). 검사 데이터의 닫힘 표시는 런타임 기능 구현이나 draft4 수락을 대신하지 않는다.
