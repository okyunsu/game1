# B20 T4 — 발 고정 캐릭터 크기 비교 도구

상태 REVIEW: 기술 구현·검증 완료, 기본 크기 채택은 사용자 D-BODY 판단 대기. 기본 Player.prefab·PlayerTuning·V5 Scene 변경 없음. Editor/Development에서만 동작한다. Release 실제 빌드에서 BodySizeOverride/Bootstrap/Course 및 새 검증 타입 부재를 확인했다.

새 [BodySizeTest](../Assets/Scenes/Test/BodySizeTest.unity)에서 **F6**: 1.6→1.3→1.1→1.6u. 화면 오른쪽 위에 높이·공격 사거리가 보인다. 키 맵에서 F6 중복 없음. [실행](../Launchers/current/Play-BodySize.cmd), 실제 A01 비교 [1.3u](../Launchers/current/Play-V5-Body13.cmd) / [1.1u](../Launchers/current/Play-V5-Body11.cmd). 기본 [Play-V5](../Launchers/current/Play-V5.cmd)는 덮어쓰기 컴포넌트 없이 기존 크기다. 비교 런처는 크기별 전용 슬롯이며 기존 V5 슬롯을 덮어쓰지 않는다.

|높이 u|폭 u|서 있기|걷기|점프 착지|풀 점프 머리 접촉 막대|포고|E1 실제 타격|공격 사거리 u|아래 공격 길이 u|
|---:|---:|---|---|---|---|---|---|---:|---:|
|1.6|0.70000|10/10|10/10|10/10|2.2/2.6/3.0/3.5/4.0|1회|성공|1.10000|1.30000|
|1.3|0.56875|10/10|10/10|10/10|2.2/2.6/3.0/3.5|1회|성공|0.89375|1.05625|
|1.1|0.48125|10/10|10/10|10/10|2.2/2.6/3.0/3.5|1회|성공|0.75625|0.89375|

막대6개×3크기=18/18에서 T3와 같은 `c < h+실제 풀 점프 상승2.52002` 예측과 일치. 높이 전환은 실제 가상 F6 입력, 발 위치 오차0.003u 이내. E1은 서 있는 실제 E1이며 collider 앞면 사이 간격0.82500/0.67031/0.56719u에서 X 공격이 HP를 줄였고, 사거리는 실제 PlayerAttack.HitboxSize.x에서 읽었다. 최대 적중 거리의 연속 탐색은 하지 않았다.

`BodySizeOverride`는 원본 높이를 실제 Collider에서 읽고 Transform 전체를 균일 축소한 뒤 Collider 발 높이 차이만큼 위치를 옮긴다. 스프라이트·콜라이더·자식 판정이 함께 줄어든다. PlayerAttack은 자식 Transform이 아니라 월드 단위 OverlapBox를 쓰므로, **런타임 CombatTuning 복사본의 공격 크기6개도 같은 비율로 축소**한다. 공격 피해·속도·포고 속도·HP·이동·점프·중력·카메라 값은 바꾸지 않는다. 설정 Asset과 PlayerAttack/PlayerMotor를 수정하지 않았다. 종료 시 원본 튜닝 참조를 복원하고 복사본을 제거한다.

PlayerMotor의 고정 허용치(접지 normal≥0.7, 발 위0.08u 등)는 그대로이며 세 크기 모두 핵심 접지 검증에서 문제가 없었다. 일반 벽 접촉을 포함하는 플레이어 core 변경은 없다. 축소에 따른 사거리 감소는 보정하지 않았다.

새 Scene의 배치는 후보 전용으로 정했다. 바닥116×1, 카메라 크기5.5 유지. 막대는 x6부터8u 간격·폭5u, 여유2.2/2.6/3.0/3.5/4.0/4.5. 계단5개는 폭1.8u·오름1.5/1.625/1.75/1.875/2.0u, 가로 빈틈1.0/1.125/1.25/1.375u로 승인 범위 안이다. 같은 점프를 반복 비교하기 쉽게 난도를 단계적으로 올렸다. 등불 x86/top3.5, 서 있는 E1 x100/발1.0. 조정할 배치는 이 Scene의 Transform, 비교 높이와 비율 적용은 Dev 컴포넌트 한 곳이다. Play 중 바꾼 값은 종료 전에 기록 후 Edit 모드에서 명시 저장해야 유지된다. 기본 크기 반영은 별도 배치다.

검증 도구 실패도 보존했다. 프로젝트에 ScreenCapture 모듈 참조가 없어 컴파일 오류였고 기존 B17 카메라 렌더 방식으로 대체해 해결했다. 최초 포고 시험은 공격 전에 낙하하는 fixture 타이밍으로1.3에서 실패, 고정 위치 재시도는 직전 접지 상태가 남아1.6에서 실패했다. 최종은 기존 B18 방식처럼 실제 공중·낙하·공격 거리 조건을 보고 아래 공격을 입력해 세 크기 모두 실제 포고가 발생했다. PlayerMotor나 포고 규칙은 수정하지 않았다. 실패 원문 [첫 시험](_raw/B20/T4-body.txt), [고정 위치 시험](_raw/B20/T4-final/body.txt).

```text
MEASURE pogo height=1.6 pressed=True pressFeet=4.6900 attackDirection=-1 count=1
MEASURE pogo height=1.3 pressed=True pressFeet=4.5100 attackDirection=-1 count=1
MEASURE pogo height=1.1 pressed=True pressFeet=4.3120 attackDirection=-1 count=1
Error/Exception/Assert=0
RELEASE type=BodySizeOverride present=False
RELEASE type=BodySizeBootstrap present=False
RELEASE type=BodySizeCourse present=False
```

실제 exe [전체 검증 원문](_raw/B20/T4-reactive/body.txt), [Development 빌드](_raw/B20/T4-reactive-development-build.txt), [Release 제외](_raw/B20/T4-final-release-build.txt). 세 새 런처 각각10초 exit0/Error0: [1.3](_raw/B20/T4-launch-body13.txt), [1.1](_raw/B20/T4-launch-body11.txt), [코스](_raw/B20/T4-launch-course.txt). 카메라 크기 모두5.5. 세 크기의 카메라 렌더 이미지도 직접 확인했다([1.6](_raw/B20/T4-reactive/body-1.6.png), [1.3](_raw/B20/T4-reactive/body-1.3.png), [1.1](_raw/B20/T4-reactive/body-1.1.png)); OnGUI는 별도 렌더여서 이 이미지에는 없으며 런처 HUD는 실행 코드에서 제공한다. [Quiet 최종](_raw/regression/B20-T4-final/result.txt).

전달 빌드는 `Builds/B20-delivery`. 기존 V4 실행에 필요한 B12는 보존하고 대체된 B18과 Release 측정 빌드를 정리해 전달 빌드2개만 남겼다. B18의 슬롯 원본은 `Validation/_raw/B20/retired-B18-slots`에 보존했다. V4 런처·기존 Validation은 변경하지 않았다.
