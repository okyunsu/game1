# B05 B01 — REVIEW

84u G-J 허브. 기존 CP-B01, top4.6u/폭12u 상단 착지, B05 문 (82.25,5.5), FromUpper (77,5.41). 필수 B02는 Dash만, B05는 Double Jump로 분리.

새 Scene: Assets/Scenes/V2/B01_V2.unity. 기존 방·원본 Prefab 수정0. 설계는 LEVEL_V2.md, 지정 입력은 Validation/Routes/V2/V2-B01-{main,optional}.json.

## 실제 검증

- 필수 입구→선택한 출구 가상 입력 기록 및 고정 재생 PASS: Validation/_raw/B05/B01-main-replay-delivery.txt
- 발견 접촉+발판 착지 고정 재생 PASS: Validation/_raw/B05/B01-optional-replay.txt
- 실제 KillZone 접촉 사망 / Health API 치명피격 / 체크포인트·HP 복귀 / 물리문 양방향 접촉 fixture PASS: Validation/_raw/B05/B01-life-a5.txt
- 적 방은 재입장 Live instance 교체, CP 방은 타이틀·Continue Scene 매핑도 실제 PASS. 처치·사망 fixture와 자연 플레이 검증을 구분한다.
- 각 최종 검사 Error/Exception/Assert=0.

핵심 측정 원문:
```

MEASURE optional B01 stage=0 pos=(41.62, 7.61) ticks=345
Error/Exception/Assert=0
```

## 실패·미실시

실패 없음.

사람 난이도·자연 이동 시간·CP→주요 도전 30~90초·지역 감각·비승 난이도는 미실측/사용자 REVIEW. 예상 시간은 계획치이며 자동 입력 시간을 사람 시간으로 사용하지 않았다. Sprint Gate·기본값 채택0.

통합 경로에서 Upper 문의 기존 좌표가 새 G-J 선반과 어긋난 문제를 발견해 (82.25,5.5)로 정렬했다. B02 귀환 Spawn은 선반 아래이므로 x69.5 출발점으로 먼저 돌아간 뒤 추가 점프한다. 통합 a5의 B01→B05 실제 입력 PASS. 미보유 점프+Dash가 높은 선반에 올라가지 못함은 `special-a5.txt`로 확인했다. 문/Spawn ID·게이트 순서·수치는 바꾸지 않았다.
