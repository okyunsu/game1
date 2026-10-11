# B09-T3 — 6종 장치 + E4

상태 REVIEW. Release 실제 입력 단위 검사 PASS, Error/Exception/Assert=0.
공유 장치 수치는 MechanicsTuning Asset 한 곳, 포고 속도는 기존 CombatTuning 11 그대로.
등불 아래 공격 포고, 가시 접촉1피해, 일반 착지 pad 반응0, Slam 착지 bounce, 증기 off 상승0/on8u/s, 승강기 초기 위 고정·양쪽 레버 공격 호출·해금/방향 디스크 저장, E4 HP2·아래 공격 피해1+포고·접촉1피해·제자리 ±.5u PASS.

원문 핵심 수치:
MEASURE pad-rise=7.514202
MEASURE crumble delay=0.566668197278911
MEASURE crumble restore=3.01667779903628
MEASURE E4 vertical span=1

Crumble delay 관찰은 Setup에서 .08초 기다린 뒤 시작하므로 전체 접촉 지연은 약.64초(물리 틱 포함). Restore는 낙하 관찰 이후3.017초.
첫 발판 FAIL은 접촉점이 아닌 이미 상승한 프레임에서 높이를 재던 도구 오류. 실제 접촉 높이로 보정 뒤 PASS.
원문 `_raw/B09/T2-3-devices-a1.txt`, `_raw/B09/T2-3-devices-a2.txt`, `_raw/B09/T-extra-a1.txt`.
장치 감각은 사용자 판단 대기. 저장 후 Scene 재입장 복원은 Start의 flag 읽기 코드 확인, 별도 재시작 시나리오는 미실시.

최종 r6: 레버 공격을 별도 처리해 아래 공격의 불필요한 포고를 제거하고, 해금 후 양쪽 레버가 방향을 전환하도록 정리했다. 실제 키 입력 extra 검사 PASS/Error0 (`_raw/B09/T-extra-r6b.txt`). 기본 적의 방향 처리에는 기본 false인 선택 필드만 추가해 V4 E3 화살표를 연결했다.

원문 실패·수치 보존:
```
FAIL B09MechanicsVerifier actual=exe
MEASURE pad-rise=7.080976
FAIL System.Exception: pad apex 7.5u +/- .3u
```
