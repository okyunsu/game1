# B04-A — AV1 후보 사운드

상태 REVIEW: 기술 구현/실제 exe 검증 완료, 청감·기본 채택은 사용자 수락 대기.

Assets/Candidates/AV1/PresentationProfile.asset 하나가 이벤트 clip/volume/pitch/throttle 및 공유 시각 값을 제어한다. 짧은 mono PCM WAV 10개를 이번 Batch의 새 일회성 생성 도구로 직접 생성했다(외부 리소스·음악 없음). 기본/원본 Scene에는 bootstrap이 없고 AV1 build scene copy에만 serialized profile을 넣는다. 기존 상태·Health 이벤트 및 새 HitFeedback.Presented 관찰 이벤트를 구독. 보호 파일 수정0. 후보 전용 AudioSource/Listener만 생성, 기존 listener가 있으면 후보 listener를 끈다. 모든 *Tell 상태는 동일 bossTell clip을 사용, 기존 예고 시각 유지.

## 실제 exe

```
PASS Pause input
PASS Pause blocks jump
PASS Pause blocks dash
PASS Pause blocks swing
PASS Pause blocks hit
PASS Pause blocks hurt
PASS Pause blocks death
PASS Pause blocks checkpoint
PASS Pause blocks ability
PASS Pause blocks bossTell
PASS Pause blocks victory
PASS Pause new sounds0
PASS duplicate same-event throttle
MEASURE audio jump clip=jump plays=2
MEASURE audio dash clip=dash plays=1
MEASURE audio swing clip=swing plays=1
MEASURE audio hit clip=hit plays=2
MEASURE audio hurt clip=hurt plays=1
MEASURE audio death clip=death plays=1
MEASURE audio checkpoint clip=checkpoint plays=4
MEASURE audio ability clip=ability plays=1
MEASURE audio bossTell clip=bossTell plays=1
MEASURE audio victory clip=victory plays=1
Error/Exception/Assert=0
MEASURE basic presentation audio=0 visual=0
Error/Exception/Assert=0
```

초기 컴파일: HitFeedback 이벤트 선언 삽입 실패와 PlayerAttack.Sequence uint/int 불일치. 선언/자료형 수정 후 Unity 컴파일 PASS. 원문:
```
Assets\Scripts\PresentationBootstrap.cs(9,211): error CS0117: 'HitFeedback' does not contain a definition for 'Presented'
Assets\Scripts\PresentationBootstrap.cs(10,56): error CS0117: 'HitFeedback' does not contain a definition for 'Presented'
Assets\Scripts\HitFeedback.cs(11,26): error CS0103: The name 'Presented' does not exist in the current context
Assets\Scripts\PresentationBootstrap.cs(16,399): error CS0266: Cannot implicitly convert type 'uint' to 'int'. An explicit conversion exists (are you missing a cast?)
Assets\Scripts\PresentationBootstrap.cs(18,278): error CS0266: Cannot implicitly convert type 'uint' to 'int'. An explicit conversion exists (are you missing a cast?)
```

검증의 체크포인트/피해/사망/능력/승리/적 제거는 명시 fixture이며 자연 완주와 구분. AV1 로그에는 실제 AudioSource.PlayOneShot 호출 뒤 clip 이름을 기록한다. 실제 출력 장치 청감·믹스·예고 인지는 직접 플레이 대기. 원본 로그 Validation/_raw/B04/{basic-events,av1-events}.txt/.log 보존. 후보값: gain .30(보스 .34), pitch ±.04, same-event throttle .05s — 짧은 신호/보스 예고 우선/중복 방지 목적.
