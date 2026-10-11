# B09-T12 — 회귀·전달·문서

상태 DONE: 요청한 회귀1회/V2 전체 입력1회·최종 전달·문서 작성 실행 완료. 실패를 PASS로 바꾸지 않았다. V4 전체 제품 수락은 별도 대기한다.

Run-Regression -Tag B09-final 1회: 기본13 PASS, RouteAll/Pogo FAIL, 각 Error0. 원문은 `_raw/B09/regression/`에 이동 보존, Camera PNG도 같은 폴더. 기존 검증 파일을 덮어쓰거나 삭제하지 않았다.
```
RouteAll-60fps FAIL exit=1 evidence=RouteAll-60fps-B09-final.txt
PogoTest-60fps FAIL exit=1 evidence=PogoTest-60fps-B09-final.txt
MEASURE continuous start deviation R01=0.000u; no placement performed
FAIL System.Exception: R01 fixed-tick replay recording exhausted room=A01 pos=(34.15, 3.87)
FAIL System.Exception: P01 input goal pogos=3
```

현재 V2 전체 고정 입력1회 (`_raw/B09/T-v2-r6b.txt`): W01~W21 PASS, W22 actual death, W23 미실시. 기본·V2 원인 수정은 T11의 조건(T0 causal)이 충족되지 않아 하지 않았다.
```
MEASURE V2-W22 start deviation=0.082
FAIL frame-observed actual death V2-W22
MEASURE V2-W22 goal=Ending ticks=286 scene=C06 pos=(13.45, 2.27)
FAIL System.Exception: V2-W22 fixed replay continuous Ending
```

최종 Development 전달 폴더는 `Builds/B09-delivery/` 한 개. 처음 기동 확인 후 V4 disabled trigger 우회가 발견돼 같은 폴더를 수정 빌드로 갱신했다(Development 컴파일2회, 별도 Development 전달 폴더 추가0). 초기/최종 증거 모두 보존. 최종 `Succeeded errors=0 warnings=0` (`_raw/B09/B09-delivery-final-build.txt`). 세 실행기 actual input PASS/Error0 (`T12-final-launch-{NewGame,Mechanics,AfterBoss}.txt`), 수정 출구 물리 겹침 PASS/Error0 (`T6-final-gates.txt`).

`Launchers/B09/` 실행기를 전달 폴더에도 복사했다. 슬롯 독립/기존 실행기 보존, 개발 M 지도21방/방문 갱신 PASS. LEVEL_V4/README/TASKS 갱신, 이전 TASKS를 Archive 보존. 기본/V2/V3 Scene·기존 PlayerTuning·Packages·ProjectSettings·productName·기본 저장 경로 변경0, Design 데이터/T1 상수 수정0. 브랜치 review/batch05-v2, push0.

전체 회귀는 마지막 V4 전용 출구 수정 뒤 반복하지 않았다: 수정은 V4ExitRule에만 적용되며 관련 게이트·실행기만 재검증했다. 코스 연속 통과/입구별 최소 상태 탈출/자연 V4 완주/30·120fps는 미검증 또는 미완주이며 T4/7/9/10의 PARTIAL을 유지한다.
