# B05 V2 전체 빌드 — DONE / 콘텐츠 수락 REVIEW

최종 전달물은 `Builds/V2-delivery/Afterglow-V2-delivery.exe`, 별도 저장 실행기는 같은 폴더 `Play-V2.cmd`. `_raw/B05/V2-delivery-build.txt`: Success errors=0. 기본 Scene 목록/ProjectSettings는 변경하지 않고 B05Build의 V2 프로필에서만 18방 Scene·출구·메뉴 매핑을 추가한다.

실제 exe `world-final.txt`: 새 게임→A05 Dash→A03 G-D→B04 추가 점프→B01 G-J→C06 기존 보스 처치→Ending→Menu **23구간 고정 입력 연속 PASS/Error0**. 시작 이후 위치 배치·능력 지급 없음. 실제 사망은 프레임 관찰로 검사했다. 선택 A06/B06/C05는 별도 필수·발견·왕복 검사로 확인했다. 전체 연속 경로가 18방의 모든 선택 분기를 방문했다고 해석하지 않는다.

재사용 입력: `Routes/V2/V2-World-continuous.json`, A지역 시작 경계 여유 후보 `V2-A-continuous-margin.json`. 기존 R01~R17 수정0. 실제 저장/이어하기는 `C04-save-final.txt`에서 CP-C04 접촉→사망 복귀→Pause→타이틀→Continue가 C04_V2를 복원하여 PASS/Error0. 전체 완주의 저장 파일도 bossDefeated/endingReached·두 능력 획득을 확인했다. 완주 입력이 모든 CP를 활성화하지는 않았다.

각 방 필수/선택 입력 18/18 PASS, KillZone 접촉·Health API 치명피격·CP HP 복귀·문 양방향 접촉 fixture PASS, 적이 있는 방은 재입장 초기화 PASS. 방별 요약은 `B05-A01`~`B05-C06-summary.md`. 왕복 fixture의 출구 앞 배치는 전체 역방향 자연 이동과 구별한다. A03 Dash 미보유 차단과 B01 추가 점프 미보유 차단 PASS. B06은 추가 점프 없이 실제 포고2회+Dash 입력 PASS.

기본13개+기존 RouteAll 1회 PASS(`base13/REG-B05base-summary.txt`). START_COMMIT 대비 기존 Scene/Prefab/Tuning/후보/Packages/ProjectSettings·보호된 Player core·RoomSession 변경0, 기존 Validation 파일 변경0(`protected-final-diff-v2.txt`, 기존 JSON48개 확인). ProgressSave는 승인된 V2 Scene 매핑 wrapper만 추가했고 저장 형식·필드·CP ID는 유지했다.

최종 핵심 원문:
```
MEASURE V2-W22 goal=Ending ticks=1102 scene=Ending pos=(0.00, 0.00)
PASS V2-W22 fixed replay continuous Ending
MEASURE V2-W23 goal=Menu ticks=1 scene=Menu pos=(0.00, 0.00)
PASS new game continuous world complete; no placement or ability grants
PASS V2 Continue CP Scene mapping
Error/Exception/Assert=0
```

통합 실패와 해결: a3은 A03 입력 종료 여유 부족, a4는 B01 높은 문의 새 선반 정렬 및 귀환 출발 위치, a5는 보스 몸체 가까운 자동 입력에서 실제 사망했다. A 입력 끝 여유8틱, 문 (82.25,5.5), 귀환 후 x69.5에서 추가 점프, 보스 공격 입력 x13.95~14.10으로 보정했다. 게임 수치·보스 패턴은 변경하지 않았다. 각 접근 원본은 삭제하지 않았다.

```
MEASURE V2-R03 start deviation=0.082
MEASURE V2-R03 goal=A04 ticks=552 scene=A03_V2 pos=(69.41, 1.82)
FAIL System.Exception: V2-R03 fixed replay continuous A04
MEASURE V2-W16 goal=B05 ticks=2584 scene=B02_V2 pos=(3.39, 2.27)
FAIL System.Exception: V2-W16 recorded continuous B05
FAIL frame-observed actual death V2-W22
MEASURE V2-W22 goal=Ending ticks=94 scene=C06_V2 pos=(14.64, 1.82)
FAIL System.Exception: V2-W22 recorded continuous Ending
```

최초 보호 검사 `protected-final-diff.txt`의 기존 입력688개 판정은 빈 pathspec으로 전체 저장소를 센 도구 오류다. 원문을 보존하고 v2 증거에 정정했다. 제품 보호 파일 변경으로 해석하지 않는다.

미실시: 사람 자연 완주·방 시간·CP→주요 도전30~90초·필수 쉬움/선택 어려움 체감·그래픽 선호·물리 게임패드. 자동 입력 시간을 사람 시간으로 대체하지 않았다. 콘텐츠 REVIEW, 기본 교체·Sprint Gate 미수락. 원본 AV1/B2 후보는 유지했다.
