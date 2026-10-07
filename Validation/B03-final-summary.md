# B03-FINAL — DONE (빌드/기본 회귀)

기본 B03-main-final / 후보 B03-B2-final: Windows x64, 플레이18방+Menu/Ending/PogoTest=21씬, 각 `Succeeded errors=0 warnings=0`. CleanBuildCache 씬 처리로 후보 Asset만 build copy에 대체, 원본 기본 BossTuning/C06/Prefab 불변. Builds는 Git 제외, Data/DLL과 함께 보존.

기본 actual exe RouteAll R01~R17 연속 지정 입력 PASS/텔레포트0/Error0 (`_raw/B03/RouteAll-final.txt`). 기본13개 현재 소스 Unity검사 1회 13/13 PASS (`REG-B03-final-summary.txt`): Input, Buffer, Rooms, Checkpoints, Camera, T1~4, S2, F2R, LOGR, BossRecovery. 후보 실제exe는 런타임 true 확인·처치/Ending/타이틀 PASS/Error0 (`fight-B2-final-exe.txt`). B06 실제exe 입력 왕복과 사망/초기화 PASS. 전체 BOSS-2의 추가 패턴별 검증은 별도 PARTIAL이며 이 Task DONE이 그 상태를 바꾸지 않는다.

핵심 원문 수치:
```
MEASURE: 60fps coyote target=0.08s actual=0.0800s velocityY=12.000
MEASURE: 60fps buffer lead=0.1000s flight=0.3200s jumped=True
MEASURE: largest reversal camera step=0.0915u; automated positional check, human feel pending
MEASURE death=(8.00, 1.81) active=1.448678 pause=3.016338; hash=+uSnDOKC21Xvb0Skc9/7xE2pLKsRMQufJzpA0yaY+4E=
MEASURE candidate=B2 killGameSeconds=20.468 killRealSeconds=22.255 hits=0
```

실패 이력은 보존하고 최종 PASS와 구분:
```
FAIL System.Exception: physical natural route reaches A03
FAIL System.Exception: R09 fixed-tick replay wrong authored exit destination B06
```
첫 줄 `RouteAll-main.txt`: 런처의 `all` 값 위치 오류로 다른 자연경로 검사 실행. 둘째 줄 `RouteAll-main-a2.txt`: disabled RoomExit가 자동 진입한 제품 문제, 새 입구 컴포넌트만 수정 후 최종 전 구간 PASS. 첫 후보exe `fight-B2-exe.txt`는 씬 증분 캐시가 기본2패턴을 포함했으므로 B2 합격에서 제외. 모두 원본 그대로 raw 보존.

미실시: 자연 사람 완주·Sprint Gate·A06/C05/B06 사람1~3분·체감, B2 30/60/120fps 패턴별 회피/실패·Pause/사망/동시프레임. [B2 상세](B03-A-summary.md), [B06 상세](B03-B06-summary.md). A06/C05 PARTIAL로 18방 전부 PASS 조건 미충족; PRD/GAME_DESIGN Target15 줄은 변경하지 않았다.
