# B21 T2 — V5 draft3.1 지름길

Claude 설계 변경: `7d40918`, WORLD_V5.md·rooms_v5_A.json의 잠금/벽 방향 계약만 추가. 좌표·수치 변경 없음. 이번 작업은 그 계약을 기존 V5 Scene6개에 생성기로 반영했다. Design 수정0.

[생성·설정 원문](_raw/21/resume-20261026-210738/T2-scene-generation.txt), [기하 이전](_raw/21/resume-20261026-210738/T2-geometry-before.txt)·[이후](_raw/21/resume-20261026-210738/T2-geometry-after.txt): 6방의 Transform·Collider·컴포넌트 종류/수·문 목적지·gate가 동일하다. A03 잠금 키1개·A05 Right 방향만 실제 설정 변경. 다른 4방의 Scene diff는 새 문자열의 빈 값 직렬화다. 기존 B18 이후 동일한 draft3 배치를 그대로 보존했다.

| 방 | BoxCollider | 문 표식 | 적 | 포고 | 조각 |
| --- | ---: | ---: | ---: | ---: | ---: |
| A01 | 19 | 3 | 2 | 0 | 0 |
| A02 | 32 | 5 | 2 | 0 | 2 |
| A03 | 30 | 5 | 4 | 1 | 0 |
| A04 | 22 | 2 | 3 | 4 | 1 |
| A05 | 20 | 3 | 1 | 0 | 0 |
| A06 | 23 | 2 | 3 | 3 | 1 |

[이번 Development 빌드](_raw/21/resume-20261026-210738/T2-build.txt) `Succeeded errors=0 warnings=0`. [실제 exe 원문](_raw/21/resume-20261026-210738/T2-shortcut.txt):

```text
PASS B21ShortcutProbe actual=exe
PASS 1 new-game A03 approach cannot enter A05; user route bypass0
PASS left fixture actual attack overlaps wall
PASS 5 left-side attack cannot break wall
PASS 2 inward-side actual left attack breaks wall
MEASURE door A05-A03 to=A03 feet=(46.5000, 1.0111)
MEASURE door A05-A03 to=A05 feet=(3.5000, 1.0089)
PASS 3 shortcut traverses both directions
PASS 4 Continue preserves open shortcut
PASS B18 fourteen directed doors retained after shortcut unlocked
Error/Exception/Assert=0
```

왼쪽 fixture는 A05 출구 콜백을 잠깐 비활성화한 뒤 x0.3에서 실제 오른쪽 공격의 Sequence 증가·Hitbox와 벽 겹침·Hits0을 확인했다. 안쪽 x2.8에서는 실제 왼쪽 공격으로 파괴했다. 임시 설정은 씬 저장에 반영하지 않았다. 14문은 파괴 후 검사하고, stub4개와 일방 반대편2개가 여전히 disabled임을 별도로 확인했다. 새 게임 A03 동쪽 아래로 실제 오른쪽 이동한 사용자 경로에서는 전환0이었다. 자연 전체 완주를 주장하지 않는다.

Quiet 결과는 [이번 회귀](_raw/regression/B21-T2-resume/result.txt)를 참조한다. 기본 회귀와 전달 런처8개·V5 clearance는 종료 T8에서 수행한다.

Ruling: 생성기의 출력 경로 인자 추가 / 과거 B18 원본을 덮어쓰지 않고 새 `_raw/21`에 기록 / 기본 생성 동작 영향 없음.

최종 물리 잠금 보완 후 [다시 실행한 실제 exe](_raw/21/resume-20261026-210738/postdoor-shortcut.txt)에서도 5항목·14방향·stub4·일방역2·Continue 유지가 모두 PASS/Error0이었다. 새 게임 A03 동쪽 접근은 발(48.6350,1.0150), 몸 오른끝48.9850에서 정지해 문 x49를 넘지 못했다. Scene 재생성 없이 런타임의 기존 Collider 상태만 바꾸었으며 기하 원본은 그대로다.
