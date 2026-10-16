# V5 층 A 방 규칙 검사

원본: `Design/v5/rooms_v5_A.json` (읽기 전용)

정적 기하 검사 후보 결과. FAIL도 설계 확정 판정이 아니며 Unity 물리·적 포고·공중 조작·벽 가림·능력 문 실제 동작은 미검증이다.
높이·도달성은 일반 점프 2u/가로 3u의 보수적 그래프. 포고 노드는 높이 y+.5이며 타격 높이·관성은 모델링하지 않는다. 시작 없는 방은 도달성 WARN. 능력 문 검사는 알려진 gate 이름만 확인하며 실제 차단은 T3에서 확인해야 한다.
밀도 요소=아이템 marks(조각/사당)+기존 11종. 높이20 초과 층은 수평 상단별 아이템 marks를 검사한다.

| 방 | 면적 u² | 요소 | /1000u² | 문 | FAIL | WARN | 판정 |
|---|---:|---:|---:|---:|---:|---:|---|
| A01 | 768 | 5 | 6.510 | 3 | 1 | 0 | FAIL |
| A02 | 1008 | 6 | 5.952 | 5 | 0 | 13 | WARN |
| A03 | 1000 | 9 | 9.000 | 5 | 0 | 1 | WARN |
| A04 | 900 | 8 | 8.889 | 2 | 0 | 1 | WARN |
| A05 | 500 | 5 | 10.000 | 3 | 0 | 1 | WARN |
| A06 | 672 | 6 | 8.929 | 2 | 0 | 2 | WARN |

```text
FAIL A01 reachability: surface=5 x=25 top=11.6 absent in 2u/3u static graph
WARN A02 size: 28x36 title-derived room class
WARN A02 floor-item: top=1 no item mark within 2u
WARN A02 floor-item: top=2.9 no item mark within 2u
WARN A02 floor-item: top=4.9 no item mark within 2u
WARN A02 floor-item: top=9 no item mark within 2u
WARN A02 floor-item: top=10.5 no item mark within 2u
WARN A02 floor-item: top=12.4 no item mark within 2u
WARN A02 floor-item: top=21 no item mark within 2u
WARN A02 floor-item: top=35 no item mark within 2u
WARN A02 headroom: platform=0 clearance=1.5
WARN A02 headroom: platform=2 clearance=1
WARN A02 headroom: platform=3 clearance=1.4
WARN A02 reachability: no start mark; cross-room arrival reachability unverified
WARN A03 reachability: no start mark; cross-room arrival reachability unverified
WARN A04 reachability: no start mark; cross-room arrival reachability unverified
WARN A05 reachability: no start mark; cross-room arrival reachability unverified
WARN A06 headroom: platform=1 clearance=1.5
WARN A06 reachability: no start mark; cross-room arrival reachability unverified
```

PASS self-test synthetic door-safety / overlap / height detected
