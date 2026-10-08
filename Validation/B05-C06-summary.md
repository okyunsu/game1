# B05 C06 — REVIEW

20u×36u 세로 보스 방. 원본 아레나 floorY1/leftWallX1/rightWallX19/startX5와 BossTuning 그대로. 선택 7단 전망 끝 y20u, 창3개. 고아 경고를 제거하고 시각 자식만 축소, collider diff0.

새 Scene: Assets/Scenes/V2/C06_V2.unity. 기존 방·원본 Prefab 수정0. 설계는 LEVEL_V2.md, 지정 입력은 Validation/Routes/V2/V2-C06-{main,optional}.json.

## 실제 검증

- 필수 입구→선택한 출구 가상 입력 기록 및 고정 재생 PASS: Validation/_raw/B05/C06-main-replay-delivery.txt
- 발견 접촉+발판 착지 고정 재생 PASS: Validation/_raw/B05/C06-optional-replay-delivery.txt
- 실제 KillZone 접촉 사망 / Health API 치명피격 / 체크포인트·HP 복귀 / 물리문 양방향 접촉 fixture PASS: Validation/_raw/B05/C06-life-a5.txt
- 적 방은 재입장 Live instance 교체, CP 방은 타이틀·Continue Scene 매핑도 실제 PASS. 처치·사망 fixture와 자연 플레이 검증을 구분한다.
- 각 최종 검사 Error/Exception/Assert=0.

핵심 측정 원문:
`

MEASURE optional C06 stage=0 pos=(13.05, 20.01) ticks=308 found=True room=C06 pogos=0
Error/Exception/Assert=0 ```

## 실패·미실시

원본에는 Entry가 없으므로 독립 fixture를 FromLeft로 고쳤다(실제 연결 Spawn ID 변경0). 선택 첫 점프 출발이 귀환 문/윗발판에 겹치던 위치를 분리했다. 원본 boss Ground collider는 복사 방 지형 제거 때 함께 제거되어 새 Variant에 동일 위치·tuning·leftExit를 다시 바인딩했다. 실제 보스 처치 전체 경로는 별도 V2 전체 요약.

```
C06-main-a1:
FAIL B05Verifier actual=exe
FAIL System.Exception: C06 entry to exit via virtual input pos=(4.00, 1.82)
C06-life-a1:
FAIL B05Lifecycle actual=exe
FAIL System.Exception: enabled physical exit C06
C06-optional-a1:
FAIL B05Optional actual=exe
MEASURE optional C06 stage=58 pos=(31.43, 10.00) ticks=2251
FAIL System.Exception: C06 optional discovery contact + shelf landing recording
C06-optional-a3:
FAIL B05Optional actual=exe
MEASURE optional C06 stage=0 pos=(4.54, 4.29) ticks=2217
FAIL System.Exception: C06 optional discovery contact + shelf landing recording
C06-optional-a4:
FAIL B05Optional actual=exe
MEASURE optional C06 stage=0 pos=(6.47, 1.81) ticks=2218 found=False room=A01
FAIL System.Exception: C06 optional discovery contact + shelf landing recording
```

사람 난이도·자연 이동 시간·CP→주요 도전 30~90초·지역 감각·비승 난이도는 미실측/사용자 REVIEW. 예상 시간은 계획치이며 자동 입력 시간을 사람 시간으로 사용하지 않았다. Sprint Gate·기본값 채택0.

