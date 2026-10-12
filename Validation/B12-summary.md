# Batch 12
시작: 2026-10-13 07:34:45 KST. 종료(기술 검증·인계): 2026-10-13 08:41 KST.
Base: e5a981c9ebd5c48ba86a065b11d42a3bced94d7d. 큐 소진; DONE 9 / PARTIAL 1. 로컬 통합·태그 B12, push 없음.

<!-- CARD START -->
```text
BATCH CARD e5a981c..B12 (통합 전 카드; 최종 파일 내용 동일)
변경 파일: 24 (+1090 / -50 줄, 바이너리 0)
  Scripts: 8
  Scenes: 0
  Prefabs: 4
  Editor: 2
  Validation: 1
  문서: 2
  기타: 7
허용 범위: Assets/Scripts, Assets/Editor, Assets/BossCandidates/B3, Assets/BossCandidates/B3.meta, Assets/ScriptableObjects/BossTuning.asset, Assets/Prefabs/BossB3*, Launchers/current, Builds, Validation/B12-summary.md, Validation/_raw, TASKS.md, GAME_DESIGN.md
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 18 / REVIEW 0 / DOING 0 / BLOCKED 1 / PARTIAL 8
결정 대기(## 2. 결정 대기 표의 D- 행): 1
열린 버그(## 4. 열린 버그 표 행): 4
직접 플레이 확인 대기(## 3. 목록 항목): 6
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 13/13 (Validation/_raw/regression/B12-final-default/result.txt)
```
<!-- CARD END -->
카드 TASKS 상태는 보존된 이전 배치 이력까지 포함한다. 이번 배치는 위 9/1이다.
Task별 Quiet13/13, 최종 Quiet13/13, [최종 기본 회귀13/13](./_raw/regression/B12-final-default/result.txt).

## 사람이 할 일
- [Boss-B3](../Launchers/current/Boss-B3.cmd): 5~10분. 제목에서 Z로 이어하기, C04에서 오른쪽 문으로 C06 진입. 8패턴 예고·회피·회복 반격을 보고 전체 채택 / 패턴별 끄기 / 수치 조정을 선택한다. 추적 연타 회피는 검증 미완료다.
- [Boss-B3-P2](../Launchers/current/Boss-B3-P2.cmd): 5~10분. HP14·1페이즈로 시작, 첫 타격 후 전환 경고와 추적4회·도약3회·짧아진 회복을 확인한다.
- D-B3: 기존4패턴 회복 .8초 유지 / 1초로 조정도 선택. 배치의 기존4 값 유지와 공통 회복≥1초 지시가 충돌하여 후보는 기존값을 보존했다. AI 추천은 B2-fast 기본 유지 후 후보 체험이다.
- 조정 위치: Assets/BossCandidates/B3/BossTuning.asset Inspector(단위·Tooltip). Play 중 변경했다면 값을 기록하고 Stop 후 Asset에 반영하고 재빌드한다. 기본값 채택은 사용자 결정이다.
- 이전 직접 플레이 대기 항목은 TASKS의 순서를 유지했다. 이 배치는 Sprint Gate·자연 HP4 보스 승리를 판정하지 않았다.

## Task 표
아래 커밋은 통합 전 Task별 로컬 커밋이다. 최종 B12 커밋으로 합쳤으며 상세 증거는 다음 기록에 연결했다.
| Task | 상태 | 로컬 커밋 | 주요 변경 |
|---|---|---|---|
| T1 | DONE | d4f1c8f | 기본 BossTuning |
| T2 | DONE | fc7ecea | BossTuning/B12BossAssets/B3 Asset |
| T3 | PARTIAL | c333384 | BossEncounter.B3/B04BossVerifier |
| T4 | DONE | d084bfe | BossEncounter.B3/B04BossVerifier |
| T5 | DONE | 02435ae | BossB3Hazard/새 탄 Prefab/Verifier |
| T6 | DONE | 09e3788 | BossEncounter.B3/새 낙하 Prefab/Verifier |
| T7 | DONE | 1fcad8d | BossEncounter/BossEncounter.B3/Verifier |
| T8 | DONE | 3b06dd0 | BossCandidateSelection/런처/Verifier |
| T9 | DONE | 2f87744 | GAME_DESIGN 13절 |
| T10 | DONE | cd1e6d7 | BossEncounter/런처/Verifier |

## Task별 검증·문제

- T1 DONE: B2-fast와 전체 직렬화 필드 일치, 빌드 `Succeeded errors=0 warnings=0`, 인자 없는 BeforeBoss `extendedPatterns=True`, Play-V4 시작 PASS, Quiet 회귀13/13. 원본: [_raw/B12](./_raw/B12/T1-before.txt), [회귀](./_raw/regression/B12-T1-unlocked/result.txt).
- 최초 sandbox 실행 라이선스 초기화 실패 및 중복 프로젝트 잠금: 제품 실패와 구분. 실제 통과 근거는 unlocked 실행뿐.
- T2 DONE: 별도 Unity Compile → Create → Verify → Build 성공. 신규 필드 없는 B2-fast 복사본에서 b3Patterns=False 재현, SerializedObject 설정·SaveAssets 후 다음 프로세스에서 True/피해1/HP28 및 모든 새 값 유지. 빌드 import assert0, Quiet13/13. B11 내부 원인은 미확정이나, 신규 속성을 포함하지 않은 직렬화 상태에서 false가 되는 문제를 재현하고 분리 생성·저장으로 해소했다. [생성](./_raw/B12/T2-Create.log), [재로드](./_raw/B12/T2-Verify.log), [빌드](./_raw/B12/T2-assert-build.log), [회귀](./_raw/regression/B12-T2/result.txt).
- T3 PARTIAL: .5s 예고·3u/s 추적·최대4u·3연타(.35s/.25s 예고/2.5u)·벽 정지·1.2s 회복 구현. 정지 피격1, Error0, 빌드 성공, Quiet13/13. fixture C04의 잘못된 spawn과 이전 arena 전환을 보정1회. 이후 뒤로 대시는 기존 낮은 지형에 막혀 x6.37에서 피해1: 의도한 회피 합격 근거 아님. 도구 보정 한도에 따라 회피/반격/Pause/복귀/10회/30·120fps는 미검증. 유용하고 안정적인 후보 구현 유지. [초기](./_raw/B12/T3-60.txt), [보정](./_raw/B12/T3-fixed-60.txt), [회귀](./_raw/regression/B12-T3/result.txt).
- T4 DONE: 첫 착지 충격파0 / 다음 착지 양방향2, .7→.3→.5s 예고·회복1.5s. 60fps 정지 피해2·회피0·실제 DJ1·X 반격·Pause·CP-C04 복귀·초기HP28·파동0·10회 Error0. 30/120fps 회피0. 고정 타이밍 회피 실패 후 기존 충격파 ETA 입력으로 보정1회. Quiet13/13. [60](./_raw/B12/T4-fixed-60.txt), [30](./_raw/B12/T4-fixed-30.txt), [120](./_raw/B12/T4-fixed-120.txt), [초기 실패](./_raw/B12/T4-60.txt), [회귀](./_raw/regression/B12-T4/result.txt).
- T5 DONE: 실제 탄 높이 .5/2.8/.5, 백스텝3u·벽 정지, 60fps 정지 피해2·점프회피0·회복 안의 X 피해·높은 탄 정지0·탄 Pause·사망 정리·10회 Error0·벽 소멸. 30/120 회피0. Quiet13/13. 검사 보정1회로 반격 순간 상태·높이·벽 소멸을 명시 확인. [60](./_raw/B12/T5-final-60.txt), [30](./_raw/B12/T5-final-30.txt), [120](./_raw/B12/T5-final-120.txt), [회귀](./_raw/regression/B12-T5/result.txt). 초기 T5 peak는 미측정 seed였으며 최종 peak3.487만 실제 측정으로 사용.
- T6 DONE: .8s 보스 예고→1s 세 지점 예고, 폭1.5/간격3u(안전 폭1.5). 천장14u·낙하10u/s·높이.8u는 후보 선택값(충분한 낙하 가시 시간과 좁은 바닥 피해용). 60fps 정지 피해1·안전 지대 x14.632 피해0·회복 X·낙하3개 Pause·CP-C04 복귀/낙하0·10회 Error0. 30/120 안전 지대0. Quiet13/13. [60](./_raw/B12/T6-60.txt), [30](./_raw/B12/T6-30.txt), [120](./_raw/B12/T6-120.txt), [회귀](./_raw/regression/B12-T6/result.txt).
- T7 DONE: T4/T5 DONE로 선행 충족. HP50% 피격→.6s 정지·경고1회, 전환 중 추가 HP 감소, 추적4회·도약3회, 회복 측정 .966750503117914s(설정1.2×.8=.96, 최소.8), CP-C04 사망 복귀 후 1페이즈/HP28/탄·낙하·파동0. Error0·Quiet13/13. [페이즈](./_raw/B12/T7-phase.txt), [회귀](./_raw/regression/B12-T7/result.txt). 임계 HP setup과 추가 피해는 ReceiveHit 호출 검증이며 자연 전투 승리를 뜻하지 않는다.
- T8 DONE: 실제 Boss-B3.cmd 시작(CP-C04·AV1·b3), 로그 활성8개 이름 확인. 순환0,4,1,6,2,5,7,3,0, 비활성 건너뛰기, 전체 비활성 Dormant·남은 공격 소멸, Error0·Quiet13/13. 순환은 처음만 플레이어 HP100을 주는 안정성 fixture이며 자연 HP4 승리/회피 검사가 아니다. [런처](./_raw/B12/T8-launch.txt), [이름 로그](./_raw/B12/T8-launch.log), [순환](./_raw/B12/T8-cycle.txt), [회귀](./_raw/regression/B12-T8/result.txt).
- T9 DONE: GAME_DESIGN 13절만 19줄 추가, 기존 문장 삭제0. B2-fast 채택·B3 후보 8패턴 이름/예고/회피/수치·2페이즈·조정 위치 기록. 문서 링크 깨짐0·Quiet13/13. [링크](./_raw/B12/T9-links.txt), [회귀](./_raw/regression/B12-T9/result.txt).
- T10 DONE: 실제 Boss-B3-P2.cmd 로그 HP14/max28/phase1. 첫 실제 X로 HP13·전환1회, Error0·최종 Quiet13/13. B3 원본 최대 HP28 유지, 개발 빌드 인자에서만 초기HP14. [런처](./_raw/B12/T10-launch.log), [첫 X](./_raw/B12/T10-p2.txt), [회귀](./_raw/regression/B12-T10-final-quiet/result.txt). 이 검사에서도 추적 회복의 X 반격은 확인됐으나 T3 회피·Pause·10회·fps 검증 전체 완료를 뜻하지 않는다.

## T1 변경 전 → B2-fast 채택

| 필드 | 변경 전 (누락은 코드 기본값) | 채택 |
|---|---|---|
| extendedPatterns | false | true |
| walkSpeed | 2.5 | 3 |
| leftAnchor / rightAnchor | 5 / 17.5 | 5 / 17.5 |
| leapTell / leapFlight / leapHeight | .7 / .6 / 4 | .56 / .5 / 4 |
| slamWidth | 3 | 3 |
| chargeTell / chargeSpeed | .6 / 9 | .48000002 / 10.8 |
| wallClearance | .1 | .1 |
| maxHP / damage | 28 / 2 | 28 / 2 |
| meleeTell / waveTell / recovery | .6 / .75 / .8 | .48000002 / .6 / .8 |
| meleeRange / waveSpeed / waveHeight | 3 / 6.5 / 1 | 3 / 7.8 / 1 |

새 패턴의 피해1·회복≥1초는 B3에 적용한다. T1은 승인된 B2-fast와 완전히 동일하게 채택한다.


## 실패 원문·핵심 측정
초기 환경 실패·검사 실패를 숨기지 않는다. 원본은 _raw에 보존하며 커밋하지 않는다.

### T3-60.txt
```text
FAIL B04BossVerifier actual=exe
MEASURE B12 pattern=4 fps=60 dodge=False hitEvents=1 hp=3
FAIL System.Exception: fresh B3 arena
```

### T3-fixed-60.txt
```text
FAIL B04BossVerifier actual=exe
MEASURE B12 pattern=4 fps=60 dodge=False hitEvents=1 hp=3
MEASURE B12 pattern=4 fps=60 dodge=True hitEvents=1 hp=3
FAIL System.Exception: stand hit / intended dodge zero
```

### T4-60.txt
```text
FAIL B04BossVerifier actual=exe
MEASURE B12 pattern=5 fps=60 dodge=False hitEvents=2 hp=2
MEASURE B12 pattern=5 fps=60 dodge=True hitEvents=1 hp=3
FAIL System.Exception: stand hit / intended dodge zero
```

### T4-fixed-60.txt
```text
MEASURE B12 pattern=5 fps=60 dodge=False hitEvents=2 hp=2 DJ=0 peak=1.810
MEASURE B12 pattern=5 fps=60 dodge=True hitEvents=0 hp=4 DJ=1 peak=4.285
MEASURE B12 isolated pattern=5 repeat=1
MEASURE B12 isolated pattern=5 repeat=2
MEASURE B12 isolated pattern=5 repeat=3
MEASURE B12 isolated pattern=5 repeat=4
MEASURE B12 isolated pattern=5 repeat=5
MEASURE B12 isolated pattern=5 repeat=6
MEASURE B12 isolated pattern=5 repeat=7
MEASURE B12 isolated pattern=5 repeat=8
MEASURE B12 isolated pattern=5 repeat=9
MEASURE B12 isolated pattern=5 repeat=10
```

### T5-final-60.txt
```text
MEASURE B12 pattern=6 fps=60 dodge=False hitEvents=2 hp=2 DJ=0 peak=1.810
MEASURE shot heights=0.5,2.8,0.5 backstep=3
MEASURE B12 pattern=6 fps=60 dodge=True hitEvents=0 hp=4 DJ=0 peak=3.487
MEASURE shot heights=0.5,2.8,0.5 backstep=3
MEASURE B12 isolated pattern=6 repeat=1
MEASURE B12 isolated pattern=6 repeat=2
MEASURE B12 isolated pattern=6 repeat=3
MEASURE B12 isolated pattern=6 repeat=4
MEASURE B12 isolated pattern=6 repeat=5
MEASURE B12 isolated pattern=6 repeat=6
MEASURE B12 isolated pattern=6 repeat=7
MEASURE B12 isolated pattern=6 repeat=8
MEASURE B12 isolated pattern=6 repeat=9
MEASURE B12 isolated pattern=6 repeat=10
```

### T6-60.txt
```text
MEASURE B12 pattern=7 fps=60 dodge=False hitEvents=1 hp=3 DJ=0 peak=1.810
MEASURE falling zones=3 playerX=13.612
MEASURE B12 pattern=7 fps=60 dodge=True hitEvents=0 hp=4 DJ=0 peak=1.810
MEASURE falling zones=3 playerX=14.632
MEASURE B12 isolated pattern=7 repeat=1
MEASURE B12 isolated pattern=7 repeat=2
MEASURE B12 isolated pattern=7 repeat=3
MEASURE B12 isolated pattern=7 repeat=4
MEASURE B12 isolated pattern=7 repeat=5
MEASURE B12 isolated pattern=7 repeat=6
MEASURE B12 isolated pattern=7 repeat=7
MEASURE B12 isolated pattern=7 repeat=8
MEASURE B12 isolated pattern=7 repeat=9
MEASURE B12 isolated pattern=7 repeat=10
```

### T7-phase.txt
```text
MEASURE phase two chase recovery=0.966750503117914
```

### T10-p2.txt
```text
MEASURE P2 launcher initial HP=14
```

초기 sandbox 라이선스 로그 원문: [S1-02.log](./_raw/regression/B12-T1/S1-02.log)
```text
Assertion failed on expression: 'SUCCEEDED(hr)'
Assertion failed on expression: 'wmiOpened'
Assertion failed on expression: 'ERROR_SUCCESS == status'
```

미실시: T3 의도한 회피 피해0(60/30/120fps), 회피 직후 반격의 전체 연계, Pause/재개·사망 복귀·잔류물 초기화·단독10회. T10의 추적 회복 X 확인은 이 묶음 전체 완료를 뜻하지 않는다. B11 내부 실패의 정밀 원인은 미확정; T2의 직렬화 생성·재로드 경로는 검증했다. T8은 초기HP100 안정성 fixture이며 자연 HP4 승리가 아니다.
후보 선택값: 백스텝 소요 .3초(3u 이동을 읽기 쉽게), 낙하 천장14u·속도10u/s·높이.8u(낙하 가시 시간과 좁은 바닥 피해). 기본 Asset에는 적용하지 않았다.
