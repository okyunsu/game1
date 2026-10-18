# B18 T1 — draft2 Scene 원인 측정

기술 측정 완료. 제품 코드·Scene 변경0. 임시 Editor 측정기는 실행 후 제거하며 소스는 `_raw/B18/T1-probe-source.cs`에 보존한다. Player.prefab의 BoxCollider2D 0.7×1.6 읽기 성공, fallback 없음.

|대상|실제 측정|분류|
|---|---|---|
|A02 solid13 → platform0|가로0.5u, 머리 여유1.5u. 블록 끝 x7.5에서 오른쪽 요구0.9u 중 실제0.14u 이동 후 platform0 접촉. x7.85에서는 위로 요구0.9u 중 아래0.12u 밀림|데이터 간격·배치|
|A02 선반 적|중심(19,9.51), 몸1×1. OverlapBox에 solid14(x18..20,y9..10.8) 포함|데이터 배치|
|발판 V4 A02 vs V5 A02|양쪽 모두 고체 BoxCollider2D. 한 방향 통과 설정 없음|설정 동일. V5만의 설정 회귀 근거 없음|

|설정|V4 A02 발판3개|V5 A02 발판3개|
|---|---|---|
|PlatformEffector2D|없음|없음|
|usedByEffector|False|False|
|surfaceArc|n/a|n/a|
|실제 두께|0.5u|0.5u|
|옆·아래|고체 충돌|고체 충돌|

접촉 시험은 실제 Scene 콜라이더와 프리팹 크기를 사용한 분리 Rigidbody2D의 15×0.02초 물리 시뮬레이션이다. 속도는 오른쪽/위3u/s이며 위치마다 독립 초기화했다. 게임 입력·PlayerMotor 검증으로 세지 않는다. x7.15·7.5에서는 수직 탈출이 가능하고 x7.85에서는 위쪽도 막혔다. 영구 갇힘 판정은 하지 않는다. OverlapBox의 바닥 접촉 경계와 양의 침투는 구분해야 한다.

원문: [측정](_raw/B18/T1-probe.txt), [Unity 로그](_raw/B18/T1-probe.log).

```text
MEASURE screenshot-pair block=solid13 platform=platform0 horizontalGap=0.5 headroom=1.5
MEASURE enemy position=(19.00, 9.51, 0.00) size=(1.00, 1.00) overlap=solid14|platform1
MEASURE edge x=7.500 feet=3.010 velocity=(3.00, 0.00) displacement=(0.14, 0.00) contacts=solid13|platform0 overlap=solid13|platform0
MEASURE edge x=7.850 feet=3.010 velocity=(0.00, 3.00) displacement=(0.01, -0.12) contacts=solid13|platform0 overlap=solid13|platform0
```

T1 DONE. Quiet 실제 REGRESSION 13/13. [원문](_raw/regression/B18-T1/result.txt). Scene·제품 코드 Git 변경0.
