# B16b T3 — 실제 exe V5 검증

상태: DONE. 실제 exe PASS, Quiet 13/13(`B16b-T3`). 수정본62cc8c1로 생성한 Scene을 사용했다. Development 빌드 Succeeded errors=0 warnings=0. 기존 B12-delivery 갱신, 기존 V4 런처 경로·저장 슬롯 보존.

문 검사는 시작 방/문 앞을 fixture로 배치한 뒤 실제 물리 트리거로 전환했다. 자연 입력 완주 검사는 BL2로 분리한다. 내부 문 전환14건(왕복 포함)과 스폰 확인, stub4개/일방 받는 쪽/낙하 역방향은 모든 능력을 가진 상태에서도 전환0, DJ 문은 미보유 차단·보유 통과를 확인했다.

순찰13개가 Live EnemyPatrol에 연결됐고 HP 복원으로 새 적을 만든 뒤에도 재연결됐다. Scene 조각은 A02 2개/A04 1개/A06 1개, 실제 대시 사당 접촉·저장과 조각 접촉을 확인했다. Pause→타이틀→Continue가 V5 A01과 Dash를 복원했고 지도는1개였다. 검사 끝 추가10초 포함 Error/Exception/Assert=0.

핵심 측정 원문:

```text
PASS B16V5Verifier actual=exe
MEASURE scene=A01 patrol=2 shards=0
MEASURE scene=A02 patrol=2 shards=2
MEASURE scene=A03 patrol=4 shards=0
MEASURE scene=A04 patrol=3 shards=1
MEASURE scene=A05 patrol=1 shards=0
MEASURE scene=A06 patrol=1 shards=1
MEASURE door contact=A01-A02 source=A01 player=(47.50, 1.05)
MEASURE door=A01-A02 destination=A02 spawn=(3.50, 1.00, 0.00) player=(3.50, 0.93)
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
MEASURE door contact=A03-A04 source=A03 player=(49.50, 11.05)
MEASURE door=A03-A04 destination=A04 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A03-A04 source=A04 player=(0.50, 1.05)
MEASURE door=A03-A04 destination=A03 spawn=(46.50, 11.00, 0.00) player=(46.50, 11.01)
MEASURE door contact=A04-A05 source=A04 player=(46.00, 0.05)
MEASURE door=A04-A05 destination=A05 spawn=(46.00, 7.30, 0.00) player=(46.00, 7.25)
MEASURE door contact=A05-A03 source=A05 player=(0.50, 1.05)
MEASURE door=A05-A03 destination=A03 spawn=(46.50, 1.00, 0.00) player=(46.50, 1.01)
MEASURE door contact=A05-A03 source=A03 player=(49.50, 1.05)
MEASURE door=A05-A03 destination=A05 spawn=(3.50, 1.00, 0.00) player=(3.50, 1.01)
MEASURE door contact=A02-A06 source=A06 player=(47.50, 5.05)
MEASURE door=A02-A06 destination=A02 spawn=(3.50, 21.00, 0.00) player=(3.50, 21.01)
MEASURE door contact=A01-A06 source=A01 player=(32.00, 15.05)
MEASURE door=A01-A06 destination=A06 spawn=(36.85, 1.00, 0.00) player=(36.85, 0.94)
MEASURE door contact=A01-A06 source=A06 player=(32.00, 0.05)
MEASURE door=A01-A06 destination=A01 spawn=(32.00, 13.30, 0.00) player=(32.00, 13.25)
Error/Exception/Assert=0
```

원본: `_raw/B16b/T3-build.txt`, `T3-build.log`, `T3-v5.txt`, `T3-v5.log`, `T3-save.json`. 기본 V4 동작은 별도 회귀·기존 런처 검사로 확인한다.
