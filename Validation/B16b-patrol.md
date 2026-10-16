# B16b T1c — Scene 순찰 끝점 연결

상태: T1c DONE. 생성·컴파일·Quiet REGRESSION 13/13. 기존 E1 프리팹·EnemyReset 원본 수정0. Scene의 EnemyReset 루트마다 V5PatrolBinding과 끝점 Transform2개를 만들고, 생성/사망복원으로 새로 생긴 Live EnemyPatrol 인스턴스의 leftPoint/rightPoint에 연결한다. V4 입력은 이 경로를 실행하지 않는다.

| 방 | 데이터 patrol 적 | 연결 | 끝점 Transform |
|---|---:|---:|---:|
| A01 | 2 | 2 | 4 |
| A02 | 2 | 2 | 4 |
| A03 | 4 | 4 | 8 |
| A04 | 3 | 3 | 6 |
| A05 | 1 | 1 | 2 |
| A06 | 1 | 1 | 2 |

합계13개/13개, 끝점26개. fallback0. V4 Scene21개와 Resources 기존 파일 재생성 SHA256 동일. 임시 출력 삭제. 실제 런타임 복원 연결은 T3에서 추가 확인한다.

```text
WARN PATROL initial spawn outside endpoints A04 x=13
```

위 경고는 데이터 x=13, patrol=[4,12]에 따른다. 데이터를 고치지 않았다. 원본: `_raw/B16b/T1c-generation.txt`, 해당 시각 generation 원본 로그.
