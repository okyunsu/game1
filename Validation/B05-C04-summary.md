# B05 C04 — REVIEW

54u 보스 전 휴식. CP-C04 (48,1.81)로 출구 근처 이동, ID 불변. 전투 없음, 선택 탑 별도.

새 Scene: Assets/Scenes/V2/C04_V2.unity. 기존 방·원본 Prefab 수정0. 설계는 LEVEL_V2.md, 지정 입력은 Validation/Routes/V2/V2-C04-{main,optional}.json.

## 실제 검증

- 필수 입구→선택한 출구 가상 입력 기록 및 고정 재생 PASS: Validation/_raw/B05/C04-main-replay.txt
- 발견 접촉+발판 착지 고정 재생 PASS: Validation/_raw/B05/C04-replay-a4.txt
- 실제 KillZone 접촉 사망 / Health API 치명피격 / 체크포인트·HP 복귀 / 물리문 양방향 접촉 fixture PASS: Validation/_raw/B05/C04-life-a1.txt
- 적 방은 재입장 Live instance 교체, CP 방은 타이틀·Continue Scene 매핑도 실제 PASS. 처치·사망 fixture와 자연 플레이 검증을 구분한다.
- 각 최종 검사 Error/Exception/Assert=0.

핵심 측정 원문:
`

MEASURE optional C04 stage=0 pos=(28.52, 7.61) ticks=232 found=True room=C04
Error/Exception/Assert=0 ```

## 실패·미실시

기록 종료 직전 접지 판정 경계에서 재생 FAIL. 새 V2 입력의 끝에 중립5틱만 덧붙인 뒤 발견+착지 PASS. 기존 Route 입력 변경0.

```
C04-optional-replay:
FAIL B05Optional actual=exe
MEASURE optional C04 stage=0 pos=(28.81, 7.61) ticks=233
FAIL System.Exception: C04 optional discovery contact + shelf landing prescribed replay
```

사람 난이도·자연 이동 시간·CP→주요 도전 30~90초·지역 감각·비승 난이도는 미실측/사용자 REVIEW. 예상 시간은 계획치이며 자동 입력 시간을 사람 시간으로 사용하지 않았다. Sprint Gate·기본값 채택0.

