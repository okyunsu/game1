# B10-T12 Claude용 현재 설계 실패 좌표

상태 PARTIAL. T2의 전체 70경로는 저장된 입력 전체 재생2회 PASS, 남은 T2 BLOCKED-GEOMETRY 0. 아래는 T4/T5의 실패 분기이며, 다른 출구 성공과 탐색 예산 미완료를 구분한다. 원본에 `classification=pending-physical-cause`가 있는 행을 실제 지형 수정 권한으로 바로 해석하지 않는다. 전체 연결/입구의 모든 대안까지 소진하지 못했으므로 고립0·실제 막힘 전수 판정은 미완료다.

## 남은 분기 및 요소 대응

| 방/분기 | rooms_def.py 요소 | 실제 좌표·최고 근접값 | 원인 범위·필요 이동량 |
|---|---|---|---|
| A02 A02-B06→A02-A06 | 문 A02-A06, 굴뚝 벽 | (1.27,39.01), closest8.549556, peak41.53491, travel2.231999 | 이 입구는 B05로 탈출 PASS. 실패 분기를 고립으로 세지 않음. 이동0 권고 |
| A03 A03-A04→A05-A03 | 문 A05-A03, 플랫폼 `village` | (58.73,15.02), closest12.53854, peak17.53491, travel2.231983 | 이 입구는 A02로 탈출 PASS. 장거리 하강/중간 경유 계획 재검사. 이동0 권고 |
| A03 B01-A03pit→A03-A04 | 문 A03-A04, `village`, 지붕 계단 | (50.73,13.73), closest9.199004, peak27.662, travel12.64789 | 낙하 도착에서 단일 점프 분기65개 소진. 다른 경유 계획 미완료. 지형 이동량 미확정 |
| C03 C03-C04→C03-C05 | `ledge`(platform0), `K`(platform4) | (55.31,11.42), closest47.81565, peak17.388, travel8.687923, contacts=platform4 | 도착 후 왼쪽 ledge까지 하강/계단 연결 미완료. 하나의 장거리 구간으로 처리한 계획과 실제 발판 경로를 먼저 비교. 이동량 미확정 |
| B05 A02-B05→B05-B06 | `crate`(solid6), 바닥 구멍 | (12.41,1.98), closest1.976197, peak3.535, travel3.635038, 758후보 | 동일 입구는 C01 탈출 PASS. 바닥 역방향 구간만 실패. 이동0 권고 |
| C04 C03-C04→C05-C04 | 바닥 구멍 x14~18, 문 C05-C04 | (16.47,0.88), closest16.05084, peak3.53491, travel4.380829,64후보 | 시작 spawn은 단단한 발판 token이 아니므로 도약 그리드가 좁음. 바닥 구멍을 걷다 C03로 전환한 결과 포함. 마지막 대시 접근 후보 미완료. 지형 변경 전 대시/도약 위치 확대 필요; 이동량 미확정 |
| C04 C06 도착→platform2 | 플랫폼 `b4`(platform2) | (35.36,11.46), closest3.669155, peak15.388,travel5.562988,contacts=platform2,573후보 | T4 C04/C04-C06 전체 왕복 저장2회 PASS도 확보. 이 실패 분기는 전체 연결 막힘 근거로 사용하지 않음 |

원인 분류 (a) 단단함/일방향, (b) 플레이어 크기/원점, (c) 생성기 오차, (d) 검사기 가정 중, 위 실패에서 새 (c)는 입증되지 않았다. 검사기 상수 수정0. T2 70경로 성공과 자연 B05 실제 통과는 해당 기본 경로의 물리 통행 가능 증거다. 현재 남은 미판정 구간에는 데이터 이동을 적용하지 않았다. 무한대·시도0인 cursor 재개 행은 신규 측정으로 쓰지 않는다.

## 자연 진행 별도 실패

C02의 `m1`(solid9, x38~41, y1~2.8): r76 자연 진행은 하강 경로를 통과한 뒤 F:40 목표에서 (41.17,2.82)에 머물렀다. 도랑 전 상자를 경유한 뒤 대시하는 T2 입력은 PASS다. r78 자연 입력에 `m1` 경유를 넣었지만 예산 종료로 결과 미확보. 이동0, 후보 입력 연결을 우선한다. 전체 경로 통행 불가로 분류하지 않는다.

## 실제 Scene 스크린샷

아래 각 방에 overview/detail1920×1080 두 장, native Release 실제 Scene 렌더 PASS/Error0. B05·C03 overview를 직접 열어 플랫폼·굴뚝 연결을 확인했다. 촬영은 게임 입력 통과 증거가 아니다.

- `_raw/B10/T12-resume-r61-C04-render.overview.png`, `.detail.png`
- `_raw/B10/T12-resume-r73-B05-render.overview.png`, `.detail.png`
- `_raw/B10/T12-resume-r76-A03-render.overview.png`, `.detail.png`
- `_raw/B10/T12-resume-r76-C03-render.overview.png`, `.detail.png`

## 원본 실패 행

```text
BLOCKED-GEOMETRY route A02/isolation-A02-B06-A02-A06 segment=1 source=spawn:A02-B06 target=door:A02-A06 failure=(1.27, 39.01) closest=8.549556 peak=41.53491 travel=2.231999 contacts= attempts=65 grid=65 part=0 parts=1 reached=False classification=pending-physical-cause
BLOCKED-GEOMETRY route A03/isolation-A03-A04-A05-A03 segment=1 source=spawn:A03-A04 target=door:A05-A03 failure=(58.73, 15.02) closest=12.53854 peak=17.53491 travel=2.231983 contacts= attempts=65 grid=65 part=0 parts=1 reached=False classification=pending-physical-cause
BLOCKED-GEOMETRY route A03/isolation-B01-A03pit-A03-A04 segment=1 source=spawn:B01-A03pit target=door:A03-A04 failure=(50.73, 13.73) closest=9.199004 peak=27.662 travel=12.64789 contacts= attempts=65 grid=65 part=0 parts=1 reached=False classification=pending-physical-cause
BLOCKED-GEOMETRY route C03/isolation-C03-C04-C03-C05 segment=1 source=spawn:C03-C04 target=platform0 failure=(55.31, 11.42) closest=47.81565 peak=17.388 travel=8.687923 contacts=platform4 attempts=65 grid=65 part=0 parts=1 reached=False classification=pending-physical-cause
BLOCKED-GEOMETRY route B05/isolation-A02-B05-B05-B06 segment=2 source=solid1~16 target=solid6 failure=(12.41, 1.98) closest=1.976197 peak=3.535 travel=3.635038 contacts= attempts=758 grid=758 part=0 parts=1 reached=False classification=pending-physical-cause
BLOCKED-GEOMETRY route C04/isolation-C03-C04-C05-C04 segment=1 source=spawn:C03-C04 target=door:C05-C04 failure=(16.47, 0.88) closest=16.05084 peak=3.53491 travel=4.380829 contacts= attempts=64 grid=64 part=0 parts=1 reached=False classification=pending-physical-cause
BLOCKED-GEOMETRY route C04/return-C04-C06-C04-C06 segment=1 source=spawn:C04-C06 target=platform2@dj failure=(35.36, 11.46) closest=3.669155 peak=15.388 travel=5.562988 contacts=platform2 attempts=573 grid=573 part=0 parts=1 reached=False classification=pending-physical-cause
```

원본 위치는 [T4 요약](B10-T4-summary.md), [T5 요약](B10-T5-summary.md)의 표와 `_raw/B10/T4-resume-collected.json`, `T5-resume-collected.json`. r54~r58 구 Scene 결과는 제외. 미실시: 예산 미완료 대안의 전수 확대 탐색·남은 요소 이동량 확정.
