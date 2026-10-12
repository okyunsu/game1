# B04-B — AV1 기하형/시각 애니메이션

상태 REVIEW. 기술 검증 완료, 가독성·형태·손맛 사용자 수락 대기.

플레이어 눈, 넓고 낮은 E1, 전면 마름모 E2, 포신 E3, 큰 몸체와 왕관/중심 표식 Boss. 기존 Block sprite만 재사용한다. 걷기 기울기8°, 공중 stretch14%, 착지 squash20%/.12s, 대시 afterimage3개/.12s, 피격 기존 alpha 관찰, 사망 fragment4개/.3s(기존 DeathPop 보강). 전부 collider 없는 AV1 Visual 자식/독립 효과이며 root/physics/input 쓰기0. 기존 적 예고 RGB·HP/flicker alpha는 유지한다. 값의 이유: 짧고 작은 실루엣 변화로 콜라이더 공간을 유지하면서 상태/적 역할 구분. 공유 후보값은 PresentationProfile.asset Inspector의 단위/Tooltip을 따른다.

실제 기본/AV1 exe 핵심 효과·5종 form 로그 모두 PASS, 기본 visual 추가0. 플레이어 및 E1 BoxCollider size/offset/root scale diff0, AV1 visual 자식 Collider0. 나머지 적 역시 동일 collider 없는 시각 코드이며 root/Collider 변경 명령 없음. 기존 S1-02/S1-04 회귀 PASS.

## 실제 이동 중립성

각 30/60/120fps, 새게임 A01 고정 위치에서 동일150 물리틱 방향·점프·DJ·대시 입력. 후보 표현만 달라지는 통제 fixture, 자연 완주와 구분. 매 틱 위치/속도 CSV의 SHA256은 6개 모두 동일.

```
MEASURE motion fps=30 AV1=False ticks=150 sha256=5B3CAFA8CE5512CA6BC6D22D36CE7E1EDD01094906BDEF5AA5F2DFAE11FB3C43
Error/Exception/Assert=0
MEASURE motion fps=30 AV1=True ticks=150 sha256=5B3CAFA8CE5512CA6BC6D22D36CE7E1EDD01094906BDEF5AA5F2DFAE11FB3C43
Error/Exception/Assert=0
MEASURE motion fps=60 AV1=False ticks=150 sha256=5B3CAFA8CE5512CA6BC6D22D36CE7E1EDD01094906BDEF5AA5F2DFAE11FB3C43
Error/Exception/Assert=0
MEASURE motion fps=60 AV1=True ticks=150 sha256=5B3CAFA8CE5512CA6BC6D22D36CE7E1EDD01094906BDEF5AA5F2DFAE11FB3C43
Error/Exception/Assert=0
MEASURE motion fps=120 AV1=False ticks=150 sha256=5B3CAFA8CE5512CA6BC6D22D36CE7E1EDD01094906BDEF5AA5F2DFAE11FB3C43
Error/Exception/Assert=0
MEASURE motion fps=120 AV1=True ticks=150 sha256=5B3CAFA8CE5512CA6BC6D22D36CE7E1EDD01094906BDEF5AA5F2DFAE11FB3C43
Error/Exception/Assert=0
```

effect 활성화 로그: Validation/_raw/B04/av1-events.txt/.log. 각틱 원본은 motion-*.csv/.txt/.log. 초기 검증 도구 Convert.ToHexString은 Unity API에 없어 BitConverter로 수정한 뒤 compile/6개 실제실행 PASS.
```
Assets\Scripts\Verification\B04MotionVerifier.cs(13,1249): error CS0117: 'Convert' does not contain a definition for 'ToHexString'
```

미실시: 사람 시각 수락, 모든 Enemy 인스턴스의 프레임별 이동 비교(공통 시각 코드/소스의 root쓰기0/기본 회귀로 검증 범위를 제한).
