# N9 최종 인계 — PARTIAL

총 소요 **103.1분**(2026-10-06 06:54~08:37 KST). 승인 범위 실행 종료, push 없음.
**ATK-2/3/4 기술 확인·REVIEW / PogoTest REVIEW / S5-hard 빌드·지정 동작 PASS / 자동 연속 경로 BLOCKED / 최종 회귀 14/15 PASS.** 감각 수락과 Sprint3 Gate는 사용자 판단을 기다린다.

## 켤 것

- 본편: `Builds/S5-hard/Afterglow-S5-hard.exe`
- 포고: 같은 폴더 `PogoTest 실행.bat`(한 줄 exe `-pogotest`). Editor는 Afterglow > Open Pogo Test. 타이틀 메뉴·Editor Build Settings는 그대로다.
- 후보: `Builds/H1/Afterglow-H1.exe`, `Builds/H2/Afterglow-H2.exe`, `Builds/H3/Afterglow-H3.exe`. 검사한 v2 exe를 요청 폴더에 복사했고 해시 일치를 확인했다. 초기 잘못된 후보는 별도 보존했다.

## 실제 확인 결과

- 아래 공격만 FixedUpdate, 공통 Time.timeAsDouble. 판정 1.2×1.3u, 명중 틱 vy11, 피해1·공격당 포고1, 빗나감·미획득 능력 보호, 가시 선행 포고 생존·몸 우선 사망을 30/60/120fps에서 확인했다. 최고 높이 모두2.128u/차이0. 기존 위·좌우·가상 패드 공격도 PASS.
- 원래 ATK2 검사 마지막 소모능력 fixture는 FAIL 상태로 보존했다(보정3회 종료). 실제 PogoTest에서 지상 점프→더블 점프→대시를 소모한 뒤 포고 회복을 독립 검증했다. 검사 전체를 PASS로 바꿔 쓰지 않았다.
- E1/E2/E3/보스 위·아래 피해·피격·처치 효과 PASS. 실제 보스 회복6구간에서 자연 포고1타, 대시 포고 최대2타/사망0 → 승인 조건에 따라 최종 HP28. 중간HP24 빌드와 증거 보존.
- Pogo P01/P02/P03, 구간별 실제 사망·체크포인트 복귀, 획득·소모한 능력 회복: exe3회 연속 및 최종 배포 exe PASS. 안내 겹침을 보정하고 [최종 화면](N9-delivery-course.png)을 실제 확인했다.
- 최종 배포 exe: 위 공격 FPS/패드/좌우 검사 PASS, 포고 구간 PASS, 보스3회 사망→처치→저장된 엔딩 PASS(처치21.734s), 런타임 Error/Exception/Assert0. 18씬 빌드 errors0/warnings0.
- 기존 실제 물리 설정은 약50Hz(0.02s), 최대 낙하 이동0.36u/틱이다. 첨부의60Hz 설명과 구분했으며 보호된 설정을 바꾸지 않고 실제 설정에서 검사했다.

## 후보 비교 — 최종 유효 결과

| 후보 | Player HP / 무적(s) | 보스 HP | 고정 입력 완주 | 사망·중단 | 보스3회 재시도 후 처치(s) |
| --- | --- | --- | --- | --- | --- |
| H1 현재 Hard | 4 / 0.8 | 28 | FAIL | R05 A04(22.055,4.299) 사망 | PASS 21.734 |
| H2 빠른 적·보스 | 4 / 0.7 | 32 | FAIL | R06 A04로 잘못 나감, 사망 없음 | PASS 21.784 |
| H3 H2 전투·실수 허용 | 5 / 1.0 | 32 | FAIL | R06 같은 입력 불일치, 사망 없음 | PASS 21.767 |

H2/H3: E1 이동3, E2 예고0.408/돌진8, E3 주기1.36, 보스 근접·파형 예고0.5/0.65·회복0.7·파형속도7.5. 원본 Tuning 해시 불변, 복사 Asset/Prefab만 빌드 참조에 연결했다. 실제 플레이어 HP/무적·보스HP/회복 검사와18씬 재변환 증거를 확인했다. 초기 H2/H3 씬 캐시 누락 결과와 H1 legacy모드 결과는 무효로 명시하고 보존했다. [전체 유효 표](N9-difficulty-final.txt), [요청한 결과 파일·이력 포함](N9-difficulty-candidates.txt).

## 남은 차단·검증 한계

1. 최종 회귀 [14/15](REG-N9-final-summary.txt): 기본13/13 + Pogo1/1 PASS, RouteAll1개 FAIL. N8 6회 및 N9 초기60fps PASS의 CSV에 실제 R05 사망이 있었고, 기존 FixedUpdate 관찰이 TimeScale0 복귀를 놓쳐 CP-A03을 목표로 인정했다. 의도한 R05 귀환 출구·G-D 통과를 건너뛴 검사여서 이전 완주 판정을 철회했다. 게임의 사망 규칙을 바꾼 것이 아니다.
2. 새 프레임 관찰 후 입력 후보도 R05에서 반복 실패. 도구 보정3회 한도·동일 오류 중단 규칙으로 경로 BLOCKED. 30fps R16 새 입력의 훈련 성공은 전체 경로 성공이 아니다. H2/H3 R06도 이전 입력의 체크포인트 시작 가정 때문에 불일치한다. 후보 고정 입력은 비교 중 수정하지 않았다. [상세](../../Validation/N9-route-recheck.txt), [N8 원본 CSV 감사](N9-N8-route-csv-audit.txt).
3. Editor Pogo 고정 입력은 P01에서3포고 후 목표 입력 소진으로 FAIL/Error0. 도구 한도에 따라 이 재생 경로도 BLOCKED. 실제 exe 구간 성공과 구분했다. 실제 물리 패드 연결·손맛·사람 완주 시간은 사용자 확인 사항이다. 후보 타이틀 이름은 컴파일된 표시를 확인했고, 별도 타이틀 화면 촬영은 하지 않았다.

## 사용자 확인과 수정 위치

H1/H2/H3 각10분 비교 후 하나 선택, PogoTest10분(높이·판정 크기·능력 회복 감각), S5-hard 완주(쉬움·불공정한 곳), `LEVEL_PROPOSALS.md` 후보 선택. 후보를 자동으로 기본값에 채택하지 않았다.

공통 포고 값은 `Assets/ScriptableObjects/CombatTuning.asset`의 pogoVelocity/pogoRefreshesAir/downAttackWidth/downAttackHeight, 보스 HP는 `BossTuning.asset`의 maxHP. 새 과정 배치·안내는 `Assets/Scenes/Test/PogoTest.unity`, 반복 표적은 `Assets/Prefabs/PogoTarget.prefab`, 후보 설정은 `Assets/N9Candidates/H2`, `H3`. Scene 변경은 Play 종료 후 재입력·저장하고, Asset은 원하는 최종 값을 명시적으로 입력·저장한다.

주요 변경: PlayerAttack·CombatTuning·새 PogoTarget/PogoTestCourse·GameHUD·새 Scene/Prefab·N9Build·Verification 도구·설계/작업/실행 문서. Motor/Dash는 public 메서드 추가만. 기존 입력·사망·저장·카메라·GameState·Scene/Prefab·Packages/ProjectSettings·LEVEL_PROPOSALS는 불변. 실제 증거와 모든 실패 기록은 보존했고, 추가 콘텐츠를 확장하지 않았다.
