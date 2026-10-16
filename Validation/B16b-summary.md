# Batch 16b 결과 — V5 층 A 파일럿 이어하기

V5 플레이 가능·REVIEW. Scene6개·실제exe·15런처+NoMap3 PASS, Release V5 제외 PASS. Base `74732cae4b93a74f113dc283a1a2946d37f1a777`, 시작 2026-10-16 13:34:36 KST. 예산12 Task/6시간, 19:04:36 이후 새 Task 시작 안 함.
유형별 Task: feature5 / verification5 / doc2 / cleanup0 =12.

## Task별 결과

| Task | 유형 | 상태 | 결과 |
|---|---|---|---|
| T1a | verification | DONE | `02dbc88` [프로브](B16b-probe.md)·Quiet13/13 |
| T1b | feature | DONE | `40683b4` [생성기](B16b-generation.md)·컴파일0·V4재생성SHA동일·Quiet13/13 |
| T1c | feature | DONE | `17e1270` [patrol13/끝점26](B16b-patrol.md)·Quiet13/13 |
| T2 | feature | REVIEW | `535ec2f` [Scene6개/요소](B16b-scenes.md)·수정본Quiet13/13 |
| T3 | verification | DONE | `bc59cc0` [문14방향/저장](B16b-runtime.md)·실제exe/Quiet13/13 |
| T4 | verification | DONE | `a777653` [정적FAIL10/WARN4](B16b-room-rules-v5-A.md)·자체검사/Quiet13/13 |
| T5 | feature | REVIEW | `a0f103c` [런처15+NoMap3/Release제외](B16b-delivery.md)·Quiet13/13 |
| T6 | doc | DONE | `9269fad` [인계](B16b-handoff.md)·링크0불량/Quiet13/13 |
| T7 | verification | DONE | `f0b088f` Quiet13/13·기본13/13·종료카드 |
| BL1 | feature | REVIEW | `add41ab` [지도6방/12문](B16b-map.md)·V4지도PASS/Quiet13/13 |
| BL2 | verification | DONE | `fbfc6ee` [자연A01→A04·A04구간BLOCKED](B16b-route.md)·독립A05대시/Quiet13/13 |
| BL3 | doc | DONE | `077ee71` [수정 비교표](B16b-v4-vs-v5-floorA.md)·합계일치/Quiet13/13 |

## 기획자에게

수정 데이터에서 Scene을 생성하고 요소 수·문14방향·순찰13개 재연결·조각4개·Dash 수집·Pause/Continue 저장 복원을 실제 exe로 확인했다. 문 검사는 문 앞 fixture의 물리 접촉 검사다. 자연 입력은 별도로 측정했으며 A04에서 미완주했고, 자동 검사는 사람의 길 찾기·시야·포고 손맛을 판단하지 못한다.

이제 층 A 여섯 방을 새 시작·대시 보유·DJ 보유로 직접 플레이할 수 있다. V4 방·Resources·수치와 게임 규칙은 보존했다. 다음 발전은 직접 플레이 결과를 바탕으로 설계 원본을 고치는 다음 승인 Batch에서 진행한다. 층 B/C나 방 좌표는 이번에 조정하지 않았다.

기획자는 [TASKS의 V5 순서](../TASKS.md#9-latest-handoff--batch16b)로 약20분 플레이하고 [WORLD_V5 7절](../Design/v5/WORLD_V5.md#7-사용자-확인결정-플레이-뒤)을 판단하면 된다. REVIEW는 그 수락을 기다린다. Scene 배치·순찰 끝점은 Edit 모드에서 저장하고 재빌드한다. 공통 이동/전투 값은 기존 설정 Asset에 있다.

## 수정본과 데이터 확인

추가 수신한 사용자 Design3개를 **62cc8c1 — docs: re-apply V5 floor A data fixes**로 먼저 저장했다. A05 도착x3.5, A04높이12/world rect, A02 조각2개, world ID A02-A06을 확인한 뒤 생성했다. 이전 임시Scene은 폐기했고 수정 전 측정은 현재 인수 근거에서 제외했다. 원본 증거는 삭제하지 않았다. 에이전트 Design 수정0. 최종 rooms SHA256 `d5653a66cec9873bacd36757651d776e8cc23b73070fb7e176e5c92e04555473`.

- 정적 후보 FAIL10/WARN4: A02 도달성9·머리 공간3, A04 도달성1, A06 머리 공간1. 포고 포함 낙관적 기하 그래프이며 실제 물리 판정과 구분한다. [전체 원문](B16b-room-rules-v5-A.md).
- A04 적 초기x13이 patrol[4,12] 밖이다. 끝점 연결 실패0이며 초기 데이터는 바꾸지 않았다.
- V3 안내는 유지하여 Dash 뒤 파일럿 밖 닫힌 문을 가리킬 수 있다. 공식 fixture 시작은 A01/A03이다. 임의 시작방 Continue는 공식 도착을 보장하지 않는다.

## 실패·경고 원문

```text
KeyNotFoundException: The given key 'Z01' was not present in the dictionary.
FAIL System.Exception: V4 default regeneration SHA differs; originals restored
WARN PATROL initial spawn outside endpoints A04 x=13
[Licensing::Module] Error: Timed-out after 60.01s, waiting for Licensing to initialize
Crash!!!
```

앞의 두 생성 실패는 T1a 프로브·T1b 첫 시도이며 수정/복원 후 최종 검사에서 해소됐다. 라이선스 실패는 T4 도구 실행 환경 문제로 제품 검사 진입 전 발생했고 정상 환경 재실행13/13. 정적 FAIL/WARN과 경로 원문은 해당 보고서 및 종료 결과에서 확인한다.

## 남은 경로 확인

최종 실제 입력은 A01→A02→A03→A04 자연 연결. A03은8.18초·포고3회로 동쪽 위 문을 통과했다. A04 위 발판 접근에서120초가 소진됐고 전체 자연 Dash 완주는 미확인이다. A05는 독립 낙하 도착에서1.50초에 대시를 수집했다. 입력 도구 보정1회 후 확정했으며 지형/수치 변경0. 포고 카운터는 동일 PlayerAttack의 누적값이다.

정적 후보와 입력 막힘 원문:

```text
WARN A02 headroom: platform=0 clearance=1.5
WARN A02 headroom: platform=2 clearance=1
WARN A02 headroom: platform=3 clearance=1.4
FAIL A02 reachability: start=A01-A02 surface=1 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A01-A02 surface=2 x=1 top=35 absent in pogo envelope
FAIL A02 reachability: start=A02-A03 surface=1 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-A03 surface=2 x=1 top=35 absent in pogo envelope
FAIL A02 reachability: start=A02-A03U surface=1 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-A03U surface=2 x=1 top=35 absent in pogo envelope
FAIL A02 reachability: start=A02-B05 surface=1 x=1 top=21 absent in pogo envelope
FAIL A02 reachability: start=A02-B05 surface=2 x=1 top=35 absent in pogo envelope
FAIL A02 reachability: start=A02-A06 surface=2 x=1 top=35 absent in pogo envelope
FAIL A04 reachability: start=A03-A04 surface=1 x=48 top=1 absent in pogo envelope
WARN A06 headroom: platform=1 clearance=1.5
BLOCKED route A03 goal=A04 elapsed=120.06 position=(15.64, 1.02) step=0 maxX=15.64 maxY=1.02 hp=4 pogos=0 reason=120s input budget exhausted
BLOCKED route A04 goal=A05 elapsed=120.05 position=(15.60, 2.60) step=3 maxX=15.60 maxY=5.20 hp=4 pogos=1 reason=120s input budget exhausted
PASS route A01 goal=A02 elapsed=5.59 position=(46.59, 1.02) step=0 maxX=46.59 maxY=3.54 hp=4 pogos=0
PASS route A02 goal=A03 elapsed=4.00 position=(26.61, 1.02) step=0 maxX=26.61 maxY=3.54 hp=4 pogos=0
PASS route A03 goal=A04 elapsed=8.12 position=(48.52, 11.02) step=13 maxX=48.52 maxY=13.60 hp=4 pogos=3
BLOCKED route A04 goal=A05 elapsed=120.06 position=(15.63, 1.80) step=3 maxX=15.63 maxY=5.42 hp=4 pogos=4 reason=120s input budget exhausted
PASS route A05 goal=Dash elapsed=1.48 position=(37.67, 3.38) step=1 maxX=46.00 maxY=7.25 hp=4 pogos=4
```

## 종료 점검

12 Task 수행: DONE9 / REVIEW3(T2·T5·BL1). 승인 Backlog1~3 완료, TASKS Approved Backlog도 비어 있어 종료한다. Sprint Gate·push 없음. 원본 로그/CSV/JSON은 _raw에 보존하고 커밋하지 않았다.

최종 전달 `Builds/B12-delivery`, 직전 `Builds/B13-delivery` 유지. 모든15런처+V5 NoMap3 재검사18/18 PASS·각10초Error0(`_raw/B16b/Final-launchers-result.txt`). 최종 실제exe 문14방향·순찰/수집/저장 PASS(`Final-v5.txt/log`). Release 오류0/경고0·V5 Scene/지도/검증 제외 PASS(`Final-release.txt/log`), 검증 임시빌드 정리. V4 Scene/Resources 보호63파일 SHA256 변경0(`Final-protected.txt`), 기존 V4 지도/런처/Prefab/Tuning/규칙 변경0.

각 Task의 Quiet12개는 모두13/13: T1a/T1b/T1c/T2-updated/T3/T4-r1/T5/T6/T7/BL1/BL2-r1/BL3. 기본 회귀 T7-default 및 종료 final-default. 원본은 `_raw/regression/B16b-<태그>/result.txt`에 있다. 최종 카드는 요약 맨 위에 붙인다.

Base 앞에 미push `6860a5f`, `7a0dc19`가 남아 있어 Batch 규칙에 따라 soft reset/합치기는 하지 않았다. 사용자 수정본 별도 커밋62cc8c1도 그대로 보존한다. 인계 태그 `B16b`는 종료 커밋에 붙인다.

종료 문서 도구 오류: BL2-r1-route.txt 1개가 요약 내용으로 덮어써져 인수 증거에서 제외했다. 실제 실행 log와 이미 보고서에 옮긴 측정 원문은 보존됐다. 동일 exe/입력으로 Final-route.txt/log를 새로 실행해 보완했다. [상세](B16b-route.md#종료-증거-보완).

최종 증거 재실행 PASS: 동일 exe/입력, Final-route.txt/log. 아래 원문이 최신 인수 근거이며 이전 실행 log·보고서 수치도 보존한다.
```text
PASS route A01 goal=A02 elapsed=5.58 position=(46.59, 1.02) step=0 maxX=46.59 maxY=3.54 hp=4 pogos=0
PASS route A02 goal=A03 elapsed=4.02 position=(26.61, 1.02) step=0 maxX=26.61 maxY=3.54 hp=4 pogos=0
PASS route A03 goal=A04 elapsed=8.18 position=(48.52, 11.02) step=13 maxX=48.52 maxY=13.36 hp=4 pogos=3
BLOCKED route A04 goal=A05 elapsed=120.06 position=(15.51, 2.77) step=3 maxX=15.51 maxY=5.42 hp=4 pogos=4 reason=120s input budget exhausted
PASS route A05 goal=Dash elapsed=1.50 position=(37.55, 3.38) step=1 maxX=46.00 maxY=7.28 hp=4 pogos=4
Error/Exception/Assert=0
```
