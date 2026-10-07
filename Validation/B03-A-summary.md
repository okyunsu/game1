# BOSS-2 / B03-A — PARTIAL

START_COMMIT c60d938dc7772e7c54b67ae1fe39c365e43f1288. 기본 BossTuning.asset·C06 Scene·기존 Prefab 변경 없음. extendedPatterns 기본 false; B2 후보 Asset/빌드만 true. 근접→도약→충격파→돌진 고정 순환, 회복 후 플레이어에서 먼 x5/x17.5로 걷기2.5u/s, 도약 높이4u. 도약 .7s 예고/.6s 비행/폭3u, 양쪽 높이1u·속도6.5u/s 파동; 돌진 .6s 예고/9u/s, 기존 회복.8s·피해2 유지.

에이전트 결정: 벽 여유 .1u(벽 관통 방지), 표적 x는 보스 반폭+여유로 벽 안쪽 clamp, 예고 시작 시 플레이어 x와 방향 고정(예고 후 추적 없음). 후보 이동 콜라이더는 전투 중 trigger로 전환해 정적 콜라이더 이동이 플레이어를 바닥 아래로 밀지 않게 함; 걷기 접촉/돌진은 겹침 피해, 도약 비행은 접촉 피해 없이 착지 판정. 한 패턴 토큰에서 착지·두 파동·돌진·접촉 중 성공한 피해를 1회만 허용. 기본 콜라이더·2패턴 분기는 그대로.

실제 최종 검증: Unity 컴파일/기본 RouteAll R01~R17 PASS; B03-B2-final 빌드 errors=0/warnings=0. 실제 후보 exe에서 runtime extendedPatterns=true 확인, 실제 이동·점프·포고 입력만으로 처치→저장→Ending→타이틀 PASS/Error0. 체감 수락 아님.

원문 수치 (`_raw/B03/fight-B2-final-exe.txt`):
```
MEASURE candidate=B2 killGameSeconds=20.468 killRealSeconds=22.255 hits=0
```
근접·걷기·도약·충격파를 관찰했고 돌진 예고 중 처치. 해당 로그의 `dodge Charge safe=1 attempts=1`은 예고만으로 집계된 도구 오류이므로 **돌진 회피 성공 근거에서 제외**. 후속 비교 도구는 실제 Recovery까지 도달한 패턴만 집계하도록 수정.

실패/제한: `_raw/B03/pattern60.txt` 대시 방향 입력 실패, `pattern60-a2.txt` 파동 점프 시점 실패, `pattern60-a3.txt` 내려찍기 회피0/실패1회, 돌진 회피1회 피격. 이들은 후보 trigger 전환 **이전 버전**의 원본 이력이며 최종 버전의 합격 근거가 아님.
```
MEASURE fps=60 pattern=1 dodge=True hitEvents=0 HP=4 pos=(7.69, 3.74)
MEASURE fps=60 pattern=1 dodge=False hitEvents=1 HP=2 pos=(10.60, 1.18)
MEASURE fps=60 pattern=3 dodge=True hitEvents=1 HP=2 pos=(10.18, 1.82)
FAIL System.Exception: designated dodge zero hits / failed dodge one event
```
자동 승인 검토가 4번째 패턴 검사 실행을 거부: Task당 접근3개 소진 판단. 그 경로 재실행·우회 없음. 최종30/60/120fps 각 새 패턴 회피/실패1회, 도약·돌진 Pause/재개·CP-C04 사망/HP·순서 초기화·동시프레임 보스0 우선은 **미실시**. 코드의 기존 DeferDeath 흐름 유지가 실행 검증을 대신하지 않음. 현재 구현은 컴파일·실제 처치·기본 완주를 막지 않아 PARTIAL 보존. 추가 패턴 검증 승인 필요.

첫 B03-B2 빌드는 증분 씬 캐시로 기본2패턴을 실행했다(`fight-B2-exe.txt`). B2 합격에서 제외. CleanBuildCache 재빌드와 런타임 후보 플래그 assertion을 추가한 최종 결과만 위에 채택. 원본 로그는 모두 보존.
