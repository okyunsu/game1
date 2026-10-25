# B10 결과 및 이전 다음 배치 후보

현재 지시가 아닌 과거 기록이다.

## 6. 최근 배치 결과 (B10)

| Task | 상태 | 현재 결과 |
|---|---|---|
| B10-T1 | DONE | 장치9개·기존 경로10개 저장 입력/전체 경로2회 PASS |
| B10-T2 | DONE | 재개 설계 70/70 저장 전체 입력2회 PASS; A02/wall·C02/low·Z01/up_A05 해소. r54~r58 이전 Scene 결과 제외; r59/r61/r63 현재 Scene·설계 hash 확인 |
| B10-T3 | DONE | covered floor trigger inset 수정 47b20ac, Claude 설계 수정 662cc0c / V4 재생성 24ceeda, 21방 검사 PASS |
| B10-T4 | PARTIAL | 실제 전체 왕복 저장2회 PASS34건, 나머지 탐색/판정 미완료 |
| B10-T5 | PARTIAL | 최소 능력 다른 출구 저장2회 PASS36입구, 고립0 미증명 |
| B10-T6 | PARTIAL | 새 게임·대시·벽 점프·B05 자연 등반→C02; S02 미완주 |
| B10-T7 | DONE | 보스전→Slam→천장→S01 저장 입력2회 연속 PASS |
| B10-T8 | DONE | 동일 HEAD 과거 빌드 고정 입력2회 PASS; 빌드 구성으로 범위 축소, 원인 커밋 미확정/수정0 |
| B10-T9 | DONE | 작성된 시작→끝 16구간, 전체 저장 입력2회 PASS; 장치 작동은 T1 별도 |
| B10-T10 | PARTIAL | 70경로30fps43PASS/27FAIL,120fps59PASS/11FAIL; 자연 미실시 |
| B10-T11 | DONE | 27 실제 입력 조합 unexpected=0, Error0 |
| B10-T12 | PARTIAL | Claude 보고서·실제 Scene4방 촬영; 남은 분기 물리 원인 미확정 |
| B10-T13 | PARTIAL | 기본13+RouteAll+Pogo PASS, V2 W22사망; 전달1개·4기동PASS, 자연 저장2개 미확보 |

| 항목 | 상태 | 결과 |
|---|---|---|
| BL1 | DONE | V4 회귀 등록1회63/69 PASS,6 FAIL |
| BL2 | DONE | 장치19경로 기본17/후보1 11/후보2 12 PASS, 기본값 유지 |
| BL3 | PARTIAL | C01까지 완료 구간 시간, 전체 미완주 |
| BL4 | BLOCKED | 참조된 B09 Backlog4~9가 BATCH-09에 없음(1~3만 있음), 합격 기준 결정 필요 |

Task별 요약: [T1](../Validation/B10-T1-summary.md) · [T2](../Validation/B10-T2-summary.md) · [T3](../Validation/B10-T3-summary.md) · [T4](../Validation/B10-T4-summary.md) · [T5](../Validation/B10-T5-summary.md) · [T6](../Validation/B10-T6-summary.md) · [T7](../Validation/B10-T7-summary.md) · [T8](../Validation/B10-T8-summary.md) · [T9](../Validation/B10-T9-summary.md) · [T10](../Validation/B10-T10-summary.md) · [T11](../Validation/B10-T11-summary.md) · [T12](../Archive/validation-2026-10/B10-geometry-report.md) · [T13](../Validation/B10-T13-summary.md) · [BL1](../Validation/B10-BL1-summary.md) · [BL2](../Validation/B10-BL2-summary.md) · [BL3](../Validation/B10-BL3-summary.md)

## 7. 다음 배치 후보

- D-B3 결정(10/14)으로 기본 보스는 B3. 추적 회피·2페이즈 fps·3바퀴 보강 검증은 B15-BL5a/b/c에서 완료했다. 다음 콘텐츠 후보는 D-V5-PILOT 층 A 설계·데이터 수신 후 파일럿이다.
- 기존 D-NEXT 전체 자연 플레이 결과와 B07 A02/C03·V2 W22 검토는 유지한다. 자동 경로 탐색이나 Sprint Gate 통과는 이번 배치에 포함하지 않았다.

