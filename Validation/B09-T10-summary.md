# B09-T10 — 자연 완주와 이어하기

상태 PARTIAL. 60fps 실제 입력 자연 실행에서 A01→A02→A03까지 진행, A03 첫 지붕 발판에서 입력 보정 후에도 미완주. 능력 부여/위치 배치를 사용하지 않은 자연 검사다. 이후 능력 획득·보스·엔딩·Z01→S02 자연 연속 완주, 30/120fps는 미실시.

원문 r5: `MEASURE mode=world PASS=2 FAIL=1`
`FAIL world A03 platform1 pos=(26.35, 2.44) actual-input-limit; no grants/placement`
관련 `_raw/B09/T-world-r4.txt`, `T-world-r5.txt`. 보스 별도 입력 fixture도 r5/r6 두 번 `FAIL System.Exception: actual boss victory grants Slam`; 추가 재시도 중단. 코드 보상 연결은 있으나 실제 승리 PASS로 기록하지 않는다.

이어하기는 실제 메뉴 입력으로4 fixture 상태 모두 PASS (`T-save-r6b.txt`, Error0): Dash/CP-A01, WallJump/CP-B01, DJ/CP-C01, 엔딩+Slam+S02/CP-A01. 능력·도착방·S02 flag·착지 확인. 처음 CP-B01 공중 도착 실패 후 생성기의 표지 아래 Ground 위치를 쓰는 수정으로 PASS. 자연 진행으로 얻은 저장4지점과는 구분한다.

V4 엔딩 제출은 A01로 복귀하며 저장 능력을 유지하도록 구현. 최종 AfterBoss 실행기는 독립 QA 저장에 전체 능력+Slam을 준비한다. 이것을 자연 완주 대체로 인정하지 않는다.
