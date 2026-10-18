# B18 T6 — draft3 전달 빌드

기술 전달 완료, 사용자 플레이 수락은 REVIEW. Development 빌드 `Builds/B18-delivery/Afterglow.exe`, Succeeded errors=0 warnings=0. T3에서 만든 동일 exe이며 문14방향·도착·stub·일방·저장을 확인했다.

|런처 모드|시작|보유|지도 켬|NoMap|실행|
|---|---|---|---|---|---|
|Play-V5|A01|없음|PASS Error0|PASS Error0|각10초|
|Play-V5-AfterDash|A03|대시|PASS Error0|PASS Error0|각10초|
|Play-V5-DJ|A01|더블 점프|PASS Error0|PASS Error0|각10초|

각 원문: [NewGame](_raw/B18/T6-NewGame.txt), [AfterDash](_raw/B18/T6-AfterDash.txt), [DJ](_raw/B18/T6-DJ.txt), [NewGame NoMap](_raw/B18/T6-NewGame-NoMap.txt), [AfterDash NoMap](_raw/B18/T6-AfterDash-NoMap.txt), [DJ NoMap](_raw/B18/T6-DJ-NoMap.txt).

```text
PASS B16V5Verifier actual=exe
PASS V5 six-room layout
PASS launcher start room
PASS fixture dash
PASS fixture double jump
PASS map switch
PASS existing schema checkpoint saved
Error/Exception/Assert=0
```

빌드는 최신 B18 + V4 호환 보존본 B12 두 개만 유지한다. B17은 교체했으며 이전 V5 슬롯은 B18/slots와 Validation/_raw/B18/B17-slots에 보존했다. V4 런처는 읽기 전용이며 B12를 가리키므로 직전 보존본 B12를 유지한다(동일한 Batch17 보존 방식). TASKS의 삭제된 B17 현재 빌드 참조를 같은 Task 커밋에서 갱신한다. V4 런처·튜닝·Prefab·Resources 변경0.

아직 사람의 끼임·작은 발판 난이도·포고 구간 체감 수락은 미실시. Scene 검사 후보69와 정적 기존 FAIL22를 해결했다고 주장하지 않는다.

T6 REVIEW. Quiet 실제 REGRESSION 13/13. [원문](_raw/regression/B18-T6/result.txt).

종료 전달 갱신: Backlog3 빌드(모든 opt-in 검증 도구 포함, 오류0·경고0)를 같은 B18-delivery 경로로 연결했고 V5+NoMap6모드를 다시10초Error0으로 확인했다. 사용자 슬롯을 raw와 최종 빌드에 보존한 뒤 중간/측정 빌드를 제거했다. 최종 보존은 B18/B12 두 개다. [최종 원문](_raw/B18/Final-V5-result.txt).
