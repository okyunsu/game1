# B03-VARIANTS — REVIEW

B2-fast/slow는 새 후보 Asset와 build copy에서만 적용. 기본 BossTuning.asset·C06 Scene·기존 Prefab/플레이어 튜닝 변경0. 모든 이동(걷기·파동·돌진·도약 비행 속도)에 ±20%, 모든 공격 예고에 ∓20%; 도약 높이·위치·피해·회복은 보존. 비행 시간을 .6/이동배율로 바꿔 도약 이동 속도를 맞춤.

| 값/단위 | B2 원본 Asset | B2-fast 실제exe | B2-slow 실제exe |
|---|---|---|---|
| 근접 예고 s | .600 | .480 | .720 |
| 파동 예고 s | .750 | .600 | .900 |
| 도약 예고 s | .700 | .560 | .840 |
| 돌진 예고 s | .600 | .480 | .720 |
| 걷기 u/s | 2.500 | 3.000 | 2.000 |
| 파동 u/s | 6.500 | 7.800 | 5.200 |
| 돌진 u/s | 9.000 | 10.800 | 7.200 |
| 도약 비행 s | .600 | .500 | .750 |
| 도약 높이 u / 목표 x u | 4 / 5·17.5 | 동일 | 동일 |
| 회복 s / HP / 피해HP | .8 / 28 / 2 | 동일 | 동일 |

실제 exe에서 후보 flag 및 모든 변형 수치 assertion PASS. 최종 B03-B2-fast-final / B03-B2-slow 각각 빌드 errors0/warnings0, 실제 입력 처치→저장→Ending→타이틀 PASS/Error0. raw `fight-B2-fast-final.txt`, `fight-B2-slow-a2.txt`.
```
MEASURE tuning candidate=B2-fast meleeTell=0.480 waveTell=0.600 leapTell=0.560 chargeTell=0.480 walk=3.000 wave=7.800 charge=10.800 leapFlight=0.500 height=4.000 recovery=0.800 HP=28 damage=2
MEASURE candidate=B2-fast killGameSeconds=22.301 killRealSeconds=24.071 hits=1
MEASURE tuning candidate=B2-slow meleeTell=0.720 waveTell=0.900 leapTell=0.840 chargeTell=0.720 walk=2.000 wave=5.200 charge=7.200 leapFlight=0.750 height=4.000 recovery=0.800 HP=28 damage=2
MEASURE candidate=B2-slow killGameSeconds=20.417 killRealSeconds=22.220 hits=1
```
fast에서는 돌진/회복까지 실행 관찰(그 attack window 피해0), slow는 도약 패턴 중1피격 후 처치. **기본 B2의 지정 돌진 회피/실패1회·30/120fps·Pause/사망 검증을 대신하지 않는다.** 자동전략이 버전별 예고/이동과 다르게 상호작용하므로 fast/slow 이름을 실제 사람 난이도 수락으로 해석하지 않는다.

실패 원문 (`fight-B2-slow.txt/.log`):
```
FAIL System.Exception: input fight death HP=11 hits=3
```
실제 피해2회+MarkDead 이벤트, 첫 slow 입력 처치 실패. 같은 접근 첫 재시도 a2 PASS, 추가 게임/수치 수정0. 초기 B03-B2-fast도 PASS였으나 실제 수치 assertion을 추가한 최종 fast-final을 QA용으로 선택. 빌드가 아닌 verification 추가로 기본 회귀 반복하지 않음.

조정 원본: Assets/BossCandidates/B2-fast/slow/BossTuning.asset. Play 변경은 Stop 후 해당 Asset에 다시 적용·저장하고 후보를 재빌드. 후보 기본 채택·체감은 사용자 권한/REVIEW.
