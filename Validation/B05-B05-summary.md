# B05 B05 — REVIEW

60u 세로 진입. 필수 상승 top3.8→6.5→9→9.6u, 착지폭10/10/12/8u. 선택 탑은 x50부터로 분리해 첫 필수 점프 아랫면 간섭을 없앴다.

새 Scene: Assets/Scenes/V2/B05_V2.unity. 기존 방·원본 Prefab 수정0. 설계는 LEVEL_V2.md, 지정 입력은 Validation/Routes/V2/V2-B05-{main,optional}.json.

## 실제 검증

- 필수 입구→선택한 출구 가상 입력 기록 및 고정 재생 PASS: Validation/_raw/B05/B05-main-replay-delivery.txt
- 발견 접촉+발판 착지 고정 재생 PASS: Validation/_raw/B05/B05-optional-replay-delivery.txt
- 실제 KillZone 접촉 사망 / Health API 치명피격 / 체크포인트·HP 복귀 / 물리문 양방향 접촉 fixture PASS: Validation/_raw/B05/B05-life-a1.txt
- 적 방은 재입장 Live instance 교체, CP 방은 타이틀·Continue Scene 매핑도 실제 PASS. 처치·사망 fixture와 자연 플레이 검증을 구분한다.
- 각 최종 검사 Error/Exception/Assert=0.

핵심 측정 원문:
```

MEASURE optional B05 stage=0 pos=(55.75, 7.61) ticks=459 found=True room=B05 pogos=0
Error/Exception/Assert=0
```

## 실패·미실시

초기 중심→중심 입력 거리 부족 뒤 가장자리 출발로 변경. 선택 발판 아랫면/입구 문 간섭은 선택 탑을 끝 구간 x50에 분리하여 해소. 이전 FAIL을 PASS로 덮어쓰지 않았다.

```
B05-main-a1:
FAIL B05Verifier actual=exe
FAIL System.Exception: B05 entry to exit via virtual input pos=(30.02, 1.81)
B05-main-a2:
FAIL B05Verifier actual=exe
FAIL System.Exception: B05 entry to exit via virtual input pos=(15.97, 1.82)
B05-optional-a1:
FAIL B05Optional actual=exe
MEASURE optional B05 stage=0 pos=(26.39, 1.82) ticks=2248
FAIL System.Exception: B05 optional discovery contact + shelf landing recording
B05-optional-a3:
FAIL B05Optional actual=exe
MEASURE optional B05 stage=0 pos=(6.59, 2.91) ticks=2250
FAIL System.Exception: B05 optional discovery contact + shelf landing recording
```

사람 난이도·자연 이동 시간·CP→주요 도전 30~90초·지역 감각·비승 난이도는 미실측/사용자 REVIEW. 예상 시간은 계획치이며 자동 입력 시간을 사람 시간으로 사용하지 않았다. Sprint Gate·기본값 채택0.

