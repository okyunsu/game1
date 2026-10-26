# B21 T4 — 가로 간격 실측

DONE(측정). Claude 설계 변경 `7d40918`은 지름길 계약 표시뿐이며 이 측정에서 Design·기본 수치 Asset·Player prefab 변경0. 새 [B21Arena Scene](../Assets/Scenes/Test/B21Arena.unity)을 Unity API로 생성하고 실제 Development exe·기본 몸1.6으로 측정했다. [빌드](_raw/21/resume-20261026-210738/T4-build.txt) 오류0/경고0, [실측 전체 원문](_raw/21/resume-20261026-210738/T4-gaps.txt) Error/Exception/Assert0, [Quiet](_raw/regression/B21-T4-resume/result.txt) 13/13.

각 값은 양쪽 바닥의 수평 빈 간격 u다. 성공 조건은 반대편 바닥에 접지하고 그 가장자리에서 중심0.2u 이상 진입. 0.05u 정수 격자 이분 탐색, 아래 성공/다음 실패 경계를 모두 시험했다. 각3회이며 프레임 입력 요청 시점에 따른 0.05u 편차를 그대로 남긴다.

| 레시피 | 1회 성공/실패 | 2회 성공/실패 | 3회 성공/실패 | 성공 최소 | 최대 | 평균 |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 달리기 점프 | 4.60/4.65 | 4.55/4.60 | 4.60/4.65 | 4.55 | 4.60 | 4.5833 |
| 정점 부근 공중 대시 점프 | 7.35/7.40 | 7.40/7.45 | 7.40/7.45 | 7.35 | 7.40 | 7.3833 |
| 지상 대시 직후 점프 | 4.30/4.35 | 4.30/4.35 | 4.30/4.35 | 4.30 | 4.30 | 4.3000 |
| 대시만 | 3.05/3.10 | 3.05/3.10 | 3.05/3.10 | 3.05 | 3.05 | 3.0500 |

```text
MEASURE gap-limit recipe=run-jump trial=1 success=4.60 failure=4.65 resolution=0.05 body=1.6
MEASURE gap-limit recipe=run-jump trial=2 success=4.55 failure=4.60 resolution=0.05 body=1.6
MEASURE gap-limit recipe=run-jump trial=3 success=4.60 failure=4.65 resolution=0.05 body=1.6
MEASURE gap-limit recipe=apex-dash-jump trial=1 success=7.35 failure=7.40 resolution=0.05 body=1.6
MEASURE gap-limit recipe=apex-dash-jump trial=2 success=7.40 failure=7.45 resolution=0.05 body=1.6
MEASURE gap-limit recipe=apex-dash-jump trial=3 success=7.40 failure=7.45 resolution=0.05 body=1.6
MEASURE gap-limit recipe=ground-dash-jump trial=1 success=4.30 failure=4.35 resolution=0.05 body=1.6
MEASURE gap-limit recipe=dash-only trial=1 success=3.05 failure=3.10 resolution=0.05 body=1.6
```

B18의 평지 제자리/달리기 4.5/4.75u 중 이번 달리기 레시피는 4.75 대비0.15~0.20u 차이로 ±0.25u 안이다. 제자리 점프를 새로 측정했다고 주장하지 않는다. 지상 대시 뒤 점프는 대시 종료 때 PlayerDash.Cancel이 수평 속도를0으로 지운 뒤 다시 가속하는 레시피여서 달리기 점프보다 짧았다. 대시만의 3.05u는 몸 반폭·착지 판정이 포함된 **틈 통과 한계**이며, B20의 실제 대시 활성 이동2.87988u와 다른 양이다.

레시피: 출발 중심(-3.5,0.015), 오른쪽 이동 후 중심 x≥-0.35에서 점프/대시 요청. 공중 대시는 상승 후 vy≤0.6u/s에서 요청. 지상 대시→점프는 x-3.5에서 대시를 끝낸 직후 점프. 대시만은 점프 입력0. 각 시도 전 접지·대시 소유·DJ/벽점프 미보유를 확인했다. 간격은 Scene의 B21ArenaCourse.gapUnits 한 곳에서 조절한다.

실행 인자: `-b21-arena -no-map -batchmode -verify-b21-gaps -save-path <전용JSON> -verify-output <새결과> -logFile <새로그>`. Scene은 Development 빌드에만 포함한다. 후보 기능은 아직 없다. 입력 레시피별 실측이며 인간 조작의 절대적인 최대 거리나 실제 모든 V5 경로의 완주 증명이 아니다.

Ruling: 간격 탐색0.5~14u·출발 x-3.5·점프 x-0.35 / 달리기 속도 확보와 0.05u 해상도 비교 / 입력 타이밍이 달라지면 경계가 달라질 수 있어 모든 시도 좌표를 원문에 남김.
