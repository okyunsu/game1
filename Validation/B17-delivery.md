# B17 T5 — V5 draft2 전달 빌드

상태 REVIEW(기술 검증 완료 후 직접 플레이 수락 대기). 최신 `Builds/B17-delivery/Afterglow.exe`, 직전 `Builds/B12-delivery/Afterglow.exe`. 새 Development 빌드 오류0·경고0. V5 전용 공유 런처 `Launchers/current/Start-V5.ps1`의 빌드 대상만 B17로 갱신하여 세 cmd가 모두 새 빌드를 실행한다. 기존 V4 런처·저장 snapshot·Prefab·튜닝·Design SHA 변경0.

|런처|시작|10초|
|---|---|---|
|Play-V5|A01, 능력 없음|PASS, Error0|
|Play-V5-AfterDash|A03, 대시|PASS, Error0|
|Play-V5-DJ|A01, 더블 점프|PASS, Error0|

```text
Succeeded errors=0 warnings=0
PASS Play-V5 exit=0 10s Error0=True
PASS Play-V5-AfterDash exit=0 10s Error0=True
PASS Play-V5-DJ exit=0 10s Error0=True
```

[빌드 원본](_raw/B17/T5-build.txt), [기동 요약](_raw/B17/T5-launchers-result.txt), [정리 원본](_raw/B17/T5-build-cleanup.txt). 각 런처의 txt/log도 같은 `_raw/B17/`에 보존한다. V5 전용 fixture 슬롯·NoMap 지원은 유지한다. 새로운 저장 슬롯은 새 빌드의 slots에 생기며 기존 V4 슬롯은 직전 빌드에 남는다.

Housekeeping: B13-delivery 및 이번 측정용 B17-before/B17-after 임시 빌드만 절대 경로·Builds 내부 경계를 확인하고 삭제했다. 최신 B17 + 직전 B12 두 폴더만 남겼다. 원본 측정 로그/CSV/PNG는 삭제하지 않았다. 삭제한 B13을 가리키던 TASKS 문장을 같은 Task 커밋에서 현재 두 빌드로 갱신한다.

T3 실제 exe에서 draft2 지도 좌표·문14방향·stub/일방·구덩이6개·저장을 검증했다. T5에서는 동일 소스로 전달 빌드를 만들고 세 실행기를 통해 기동을 검증했다. 자연 전체 경로와 사람이 느끼는 카메라/머리 여유는 직접 플레이 대기다.

Quiet B17-T5: REGRESSION 13/13. 기술 기준 PASS, 최종 상태 REVIEW.

Editor 라이선스 중단 뒤에도 전달 exe의 V5 세 런처를 다시10초씩 확인했다.

```text
PASS Play-V5 exit=0 10s Error0=True
PASS Play-V5-AfterDash exit=0 10s Error0=True
PASS Play-V5-DJ exit=0 10s Error0=True
```
