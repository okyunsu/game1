# B04-C — B2 돌진 검증

상태: REVIEW. 추가 승인 첫 접근(A1)으로 기본 B2 30/60/120fps, fast/slow 60fps 모두 지정 DJ 회피0·실패1, 도약/돌진 Pause·재개·CP-C04 사망복귀/HP·첫패턴 초기화 PASS. Unity Editor 실제 물리 입력; 자연 전투 완주/사용자 체감 수락과 구분. Error/Exception/Assert=0.

이전 B03 실패3개 확인: 대시 방향 오판 → 도약 착지파동 시점 → 돌진 두번째 점프 한 프레임 후 jump-cut. A1은 물리 틱 입력 및 충돌 ETA .65s, 첫 점프 .18s/중립 .06s/두번째 .40s 유지로 변경. slow만 몸체 후방 통과 전에 착지하여 첫 허용 재시도에서 첫 점프 .28s/중립 .06s/두번째 .40s로 보정. 게임/기본 수치 변경0. 새 접근2/3 사용하지 않음.

## 원문 측정/실패

### charge60-new1
```
MEASURE fps=60 candidate=B2 charge dodge=True hitEvents=0 DJ=1 peak=6.225
MEASURE fps=60 candidate=B2 charge dodge=False hitEvents=1 DJ=0 peak=1.935
```

### charge30-new1
```
MEASURE fps=30 candidate=B2 charge dodge=True hitEvents=0 DJ=1 peak=6.339
MEASURE fps=30 candidate=B2 charge dodge=False hitEvents=1 DJ=0 peak=1.935
```

### charge120-new1
```
MEASURE fps=120 candidate=B2 charge dodge=True hitEvents=0 DJ=1 peak=6.303
MEASURE fps=120 candidate=B2 charge dodge=False hitEvents=1 DJ=0 peak=1.935
```

### charge-fast60-new1
```
MEASURE fps=60 candidate=B2-fast charge dodge=True hitEvents=0 DJ=1 peak=6.357
MEASURE fps=60 candidate=B2-fast charge dodge=False hitEvents=1 DJ=0 peak=1.935
```

### charge-slow60-new1
```
FAIL B04BossVerifier actual=Editor
MEASURE fps=60 candidate=B2-slow charge dodge=True hitEvents=1 DJ=1 peak=6.363
FAIL System.Exception: charge dodge0 / fail1
```

### charge-slow60-new1-a2
```
MEASURE fps=60 candidate=B2-slow charge dodge=True hitEvents=0 DJ=1 peak=6.729
MEASURE fps=60 candidate=B2-slow charge dodge=False hitEvents=1 DJ=0 peak=1.935
```

원본: Validation/_raw/B04/charge*.txt/.log, B03 원본도 보존. 미실시: B04에서 도약 회피 30/60/120 재검증·동시 HP0 우선순위는 요구 범위가 아니어서 미실시, 기존 BOSS-2 전체 PARTIAL은 이 잔여 항목과 사람 수락을 남김.
