# B10-T4 연결 왕복

상태 PARTIAL: 현재 설계에서 저장된 전체 입력 2회 연속 PASS 34건. 미통과를 합격으로 바꾸지 않았다. 19:06 예산 종료로 더 진행하지 않았으며, 권한 대기 후 시계 확인에서 초과를 발견해 실행 중 탐색/큐를 종료했다. 원본 live 로그 보존.

r54~r58 이전 Scene 결과 제외. r59 이후 native Unity 빌드만 사용. 같은 연결의 단방향 취득/구간 성공은 전체 왕복 PASS에 포함하지 않음. 입구 탈출은 해당 입구 이외 출구에 실제 도달한 전체 저장 입력만 포함.

| 방/연결 | 결과 | 최소 능력(mask) | 원본 |
|---|---|---|---|
| A01/A01-A02 | 미완료/설계 제외 확인 필요 | 0 | 아래 미완료 기록 |
| A01/A01-A06 | 미완료/설계 제외 확인 필요 | 7 | 아래 미완료 기록 |
| A01/A01-Z01 | 미완료/설계 제외 확인 필요 | 15 | 아래 미완료 기록 |
| A02/A01-A02 | 미완료/설계 제외 확인 필요 | 0 | 아래 미완료 기록 |
| A02/A02-A03 | 미완료/설계 제외 확인 필요 | 0 | 아래 미완료 기록 |
| A02/A02-A06 | 미완료/설계 제외 확인 필요 | 3 | 아래 미완료 기록 |
| A02/A02-B05 | 미완료/설계 제외 확인 필요 | 3 | 아래 미완료 기록 |
| A02/A02-B06 | PASS replay2 | 3 | T4-resume-r61-B-B06.txt |
| A03/A02-A03 | PASS replay2 | 0 | T4-resume-r72-recheck-A02.txt |
| A03/A03-A04 | 미완료/설계 제외 확인 필요 | 0 | 아래 미완료 기록 |
| A03/A03-B01 | PASS replay2 | 1 | T4-resume-r61-B-B01.txt |
| A03/A05-A03 | PASS replay2 | 1 | T4-resume-r59-A-A05.txt |
| A03/B01-A03pit | 미완료/설계 제외 확인 필요 | 1 | 아래 미완료 기록 |
| A04/A03-A04 | 미완료/설계 제외 확인 필요 | 0 | 아래 미완료 기록 |
| A04/B02-A04 | 미완료/설계 제외 확인 필요 | 1 | 아래 미완료 기록 |
| A05/A04-A05 | 미완료/설계 제외 확인 필요 | 0 | 아래 미완료 기록 |
| A05/A05-A03 | 미완료/설계 제외 확인 필요 | 0 | 아래 미완료 기록 |
| A05/A05-Z01 | 미완료/설계 제외 확인 필요 | 15 | 아래 미완료 기록 |
| A06/A01-A06 | PASS replay2 | 7 | T4-resume-r59-A-A01.txt |
| A06/A02-A06 | PASS replay2 | 3 | T4-resume-r72-recheck-A02.txt |
| A06/A06-B06pit | 미완료/설계 제외 확인 필요 | 3 | 아래 미완료 기록 |
| B01/A03-B01 | PASS replay2 | 1 | T4-resume-r59-A-A03.txt |
| B01/B01-B02 | PASS replay2 | 1 | T4-resume-r61-B-B02.txt |
| B02/B01-B02 | PASS replay2 | 1 | T4-resume-r61-B-B01.txt |
| B02/B02-B03 | PASS replay2 | 1 | T4-resume-r61-B-B03.txt |
| B02/B04-B02 | 미완료/설계 제외 확인 필요 | 3 | 아래 미완료 기록 |
| B03/B02-B03 | PASS replay2 | 1 | T4-resume-r61-B-B02.txt |
| B03/B03-B04high | PASS replay2 | 3 | T4-resume-r61-B-B04.txt |
| B03/B03-B04low | PASS replay2 | 3 | T4-resume-r61-B-B04.txt |
| B04/B03-B04high | PASS replay2 | 1 | T4-resume-r61-B-B03.txt |
| B04/B03-B04low | PASS replay2 | 1 | T4-resume-r61-B-B03.txt |
| B05/A02-B05 | 미완료/설계 제외 확인 필요 | 3 | 아래 미완료 기록 |
| B05/B05-B06 | PASS replay2 | 3 | T4-resume-r61-B-B06.txt |
| B05/B05-C01 | 미완료/설계 제외 확인 필요 | 3 | 아래 미완료 기록 |
| B06/A02-B06 | PASS replay2 | 3 | T4-resume-r72-recheck-A02.txt |
| B06/B05-B06 | PASS replay2 | 3 | T4-resume-r61-B-B05.txt |
| C01/B05-C01 | PASS replay2 | 3 | T4-resume-r61-B-B05.txt |
| C01/C01-C02high | PASS replay2 | 3 | T4-resume-r61-C-C02.txt |
| C01/C01-C02low | PASS replay2 | 3 | T4-resume-r61-C-C02.txt |
| C02/C01-C02high | PASS replay2 | 3 | T4-resume-r61-C-C01.txt |
| C02/C01-C02low | PASS replay2 | 3 | T4-resume-r61-C-C01.txt |
| C02/C02-C03high | PASS replay2 | 3 | T4-resume-r76-C03.live.txt |
| C02/C02-C03low | PASS replay2 | 3 | T4-resume-r76-C03.live.txt |
| C03/C02-C03high | PASS replay2 | 3 | T4-resume-r61-C-C02.txt |
| C03/C02-C03low | PASS replay2 | 3 | T4-resume-r61-C-C02.txt |
| C03/C03-C04 | 미완료/설계 제외 확인 필요 | 7 | 아래 미완료 기록 |
| C03/C03-C05 | PASS replay2 | 7 | T4-resume-r61-C-C05.txt |
| C04/C03-C04 | 미완료/설계 제외 확인 필요 | 7 | 아래 미완료 기록 |
| C04/C04-C06 | PASS replay2 | 15 | T4-resume-r61-C-C06.txt |
| C04/C05-C04 | PASS replay2 | 7 | T4-resume-r61-C-C05.txt |
| C05/C03-C05 | PASS replay2 | 3 | T4-resume-r76-C03.live.txt |
| C05/C05-C04 | PASS replay2 | 7 | T4-resume-r72-recheck-C04.txt |
| C06/C04-C06 | 미완료/설계 제외 확인 필요 | 7 | 아래 미완료 기록 |
| C06/C06-S01 | PASS replay2 | 15 | T4-resume-r61-C-S01.txt |
| S01/C06-S01 | PASS replay2 | 15 | T4-resume-r61-C-C06.txt |
| Z01/A01-Z01 | 미완료/설계 제외 확인 필요 | 15 | 아래 미완료 기록 |
| Z01/A05-Z01 | 미완료/설계 제외 확인 필요 | 미관찰 | 아래 미완료 기록 |

mask:0 없음,1 대시,2 벽 점프,4 더블 점프,8 Slam. 획득은 시험 중 추가하지 않으며 각 입구 첫 준비에만 최소 상태를 부여. 자연 진행 T6에서는 이 준비를 쓰지 않음.

설계 의존/실패 분기 원문:
```text
DESIGN-DEPENDENCY no reverse enabled door A03/B01-A03pit pos=(46.00, 28.30)
DESIGN-DEPENDENCY no reverse enabled door A04/B02-A04 pos=(60.00, 14.30)
DESIGN-DEPENDENCY no reverse enabled door A05/A04-A05 pos=(68.00, 11.30)
DESIGN-DEPENDENCY no reverse enabled door A06/A06-B06pit pos=(12.00, 17.30)
DESIGN-DEPENDENCY no reverse enabled door B02/B04-B02 pos=(30.00, 15.30)
BLOCKED-GEOMETRY route C04/return-C04-C06-C04-C06 segment=1 source=spawn:C04-C06 target=platform2@dj failure=(35.36, 11.46) closest=3.669155 peak=15.388 travel=5.562988 contacts=platform2 attempts=573 grid=573 part=0 parts=1 reached=False classification=pending-physical-cause
```

주의: 다른 출구가 PASS한 입구의 실패 분기는 고립 증거가 아니다. 전체 후보를 소진하지 않은 탐색은 예산 미완료이며 실제 막힘으로 확정하지 않는다. 상세 좌표/요소: [Claude 보고서](B10-geometry-report.md).

미실시: 모든 미완료 연결/입구의 확대 후보 전수 소진 및 전체 왕복/고립0 수락. 원본 `_raw/B10/T4-resume-*.txt`, live.txt, inputs/.
