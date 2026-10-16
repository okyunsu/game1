# Batch 16 결과 — V5 층 A 파일럿

**V5 전달 실패: T1 BLOCKED로 Scene·빌드·V5 런처 미생성. 기존 12런처 각각 10초 PASS, Error/Exception/Assert=0.**
유형별 Task: feature 4 / verification 4 / doc 2 / cleanup 0 = 10(의존 SKIPPED 포함). 상태: DONE 2 / PARTIAL 2 / BLOCKED 1 / SKIPPED 5. 새 플레이 가능 범위 0.

```text
T1: REGRESSION 13/13
T4: REGRESSION 13/13
BL3: REGRESSION 13/13
T6: REGRESSION 13/13
T7: REGRESSION 13/13
final-default: REGRESSION 13/13
```

<!-- CARD START -->
```text
BATCH CARD 6860a5fc8f6f9e8b897e294f0457fe5cf31cf2f9..HEAD (1a3a6e4)
변경 파일: 7 (+370 / -15 줄, 바이너리 0)
  Scripts: 0
  Scenes: 0
  Prefabs: 0
  Editor: 0
  Validation: 4
  문서: 2
  기타: 1
허용 범위: Assets/Scripts, Assets/Editor, Assets/Scenes/V5, Assets/Maps/world_v5_A.json, Assets/Resources/V5, Launchers/current, Tools/Check-RoomRules.py, README.md, TASKS.md, Validation/B16-*.md, Builds, Batches/BATCH-16.md
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 30 / REVIEW 4 / DOING 0 / BLOCKED 2 / PARTIAL 10
결정 대기(## 2. 결정 대기 표의 D- 행): 0
열린 버그(## 4. 열린 버그 표 행): 5
직접 플레이 확인 대기(## 3. 목록 항목): 5
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 13/13 (Validation/_raw/regression/B16-final-default/result.txt)

```
<!-- CARD END -->

Base: `6860a5fc8f6f9e8b897e294f0457fe5cf31cf2f9`(지시문·설계 데이터를 먼저 커밋). 시작 약 2026-10-16 10:08 KST. B15 선행 확인, 시작 미추적 파일은 지정된 지시문·Design/v5뿐. Design 읽기 전용, push 없음. 종료 시각은 TASKS에 기록한다.

## 기획자용 설명

자동 검사는 기존 게임 회귀, 기존 12런처 시작, JSON 정적 기하, 가짜 위반 3종 검출, V4/V5 데이터 합계 비교를 수행했다. V5 Scene 생성 전에 생성기 코드의 경로 문자열 컴파일 오류가 발생했고 1회 보정 후에도 실패해 해당 변경을 모두 원복했다. 자동 검사는 V5 포고 손맛·길 찾기·실제 문 전환·능력 획득을 확인하지 못했다.

독립 결과로 방 규칙 검사기와 6방 비교표를 남겼다. V5 층 A 면적은 8260→4848u², 적은 7→12, 방별 문 표식은 22→20이다. 다음 승인 배치에서는 정상 C# 경로 문법으로 생성기 확장을 다시 구현하고 T2/T3/T5 및 지도·경로 검사를 이어갈 수 있다. 이번 배치에서는 재시도 한도를 다시 열지 않았다.

현재 기획자가 V5를 실행할 런처는 없다. 기존 V4 확인 대기 순서는 유지한다. V5 전달 후 직접 판단할 질문은 [설계 7절](../Design/v5/WORLD_V5.md#7-사용자-확인결정-플레이-뒤)에 있다. 아래 데이터 불일치를 Claude 쪽에서 검토할 수 있도록 기록했으며, 외부로 메시지를 보내거나 설계 원본을 수정하지 않았다.

## 데이터 확인 사항

- [정적 검사 전체](B16-room-rules-v5-A.md): 최종 FAIL1 / WARN18. A01 x25/top11.6 높은 선반은 2u/3u 정적 그래프에 연결되지 않는다. 적 포고·공중 물리를 모델링하지 않아 실제 통과 불가 판정이 아니다.
- A02 28×36은 초안 겹침형 30~40×24~30 밖이다. 수평 상단별 아이템 marks와 머리 공간 WARN. 상단별 층 구분은 근사이며 층 설계 확정 판단은 남아 있다.
- A02 감실 marks에는 '조각' 글이 없어 기존 생성기 계약으로는 보상 아이템이 생성되지 않는다(설계 8절과 불일치).
- A04 도면/ascii는 높이12, JSON size/world rect는 높이18이다. 원본 그대로 보존했다.
- A05 A05-A03 도착 spawn은 x46.5인데 해당 방의 문 rect x는0이다. A03에서 들어올 때 방 반대편에 도착할 후보 문제, 실제 exe 확인은 미실시.
- world 문 ID A06-A02와 room/spawn ID A02-A06이 다르다. 실행 코드는 room ID 쌍을 사용하며, 구현 시 지도 연계에 유의해야 한다.
- 기존 E1 prefab의 leftPoint/rightPoint는 null이므로 지정 patrol이 바로 연결되지 않는다. fallback 동작의 실제 범위 이탈 적 목록은 Scene 미생성으로 측정하지 못했다. 이탈 목록을 검증했다고 주장하지 않는다.

## Task별 결과

| Task | 유형 | 상태 | 주요 파일·실제 결과 | 문제·미실시 |
|---|---|---|---|---|
| T1 | feature | BLOCKED | [고정 ID/좌표 검토](B16-hardcoded.md), 생성기 변경 전부 원복, Quiet13/13 | 컴파일1회 보정 후 재실패. V4 기본 재생성 SHA·V5 임시 생성 미실시 |
| T2 | feature | SKIPPED | 변경 없음 | T1 의존. V5 6씬·요소 수 비교 미실시 |
| T3 | verification | SKIPPED | 변경 없음 | T1/T2 의존. 실제 exe 문/게이트/스폰 검사 미실시 |
| T4 | verification | PARTIAL | [Check-RoomRules.py](../Tools/Check-RoomRules.py), [검사표](B16-room-rules-v5-A.md), 가짜 안전/겹침/높이 위반 검출, Quiet13/13 | 실제 gate와 모든 방의 시작점 물리 도달성, 상승 갈래 확정 판정은 미완료 |
| T5 | feature | SKIPPED | 기존 B12-delivery + B13-delivery 보존 | 새 Development/Release 빌드·V5 런처 없음 |
| T6 | doc | DONE | TASKS 실제 실패 상태·기존 플레이 대기 보존, README 1줄, 결정 대기0 | V5 REVIEW/생성 완료라고 쓰지 않음 |
| T7 | verification | PARTIAL | 최종 회귀·카드·기존12런처 검사 | V5 관련 공통 합격 미충족, Release 재빌드 미실시 |
| BL1 | feature | SKIPPED | 변경 없음 | V5 지도·실행 모드 의존 |
| BL2 | verification | SKIPPED | 변경 없음 | V5 exe 의존, 실제 입력 구간 측정0 |
| BL3 | doc | DONE | [6방 비교](B16-v4-vs-v5-floorA.md), 면적/적/장치/문/요소 양쪽 합계 검산 | 판정·의견 없음 |

Task별 사전 로컬 커밋(최종 squash 전):

```text
1a3a6e4 B16 T7 PARTIAL: final regression and existing launcher checks
ed3150d B16 T6 DONE: document blocked V5 delivery and actual validation
b1c2509 B16 BL3 DONE: compare V4 and V5 floor A source totals
7906e04 B16 T4 PARTIAL: static V5 rules and synthetic violation checks
b5b9b03 B16 T1 BLOCKED: revert generator changes after retry limit
```

T2/T3/T5/BL1/BL2는 변경 자체가 없어 독립 구현 커밋을 만들지 않았다. 실행한 T1/T4/BL3/T6/T7에는 Task별 Quiet를 적용한다. SKIPPED는 배치가 지정한 의존 작업 상태다.

## 실패·보정 원문

최초 직접 Unity 호출은 컴파일 도중 로그가 끝나 완료 근거에서 제외했다. 이후 Start-Process -Wait로 실행해 아래 컴파일 오류를 확인했다. 원인은 에이전트 코드의 이스케이프 오류이며 설계 데이터 오류가 아니다. 첫 실패 후 경로 문자열을 1회 보정했으나 두 번째도 실패했다. 배치 규칙에 따라 제품 코드·새 Editor/실행 스크립트 전부 원복했다.

```text
T1-editor-r1.log
Assets\Editor\B09Authoring.cs(51,109): error CS1056: Unexpected character '\'
Assets\Editor\B09Authoring.cs(51,110): error CS1056: Unexpected character '\'
Assets\Editor\B09Authoring.cs(51,111): error CS1003: Syntax error, ',' expected
Assets\Editor\B09Authoring.cs(51,115): error CS1010: Newline in constant
Assets\Editor\B09Authoring.cs(51,131): error CS1003: Syntax error, ',' expected
Assets\Editor\B09Authoring.cs(52,3): error CS1525: Invalid expression term 'bool'
Assets\Editor\B09Authoring.cs(52,8): error CS1003: Syntax error, ',' expected
Assets\Editor\B09Authoring.cs(52,144): error CS1026: ) expected
Scripts have compiler errors.
Exiting without the bug reporter. Application will terminate with return code 1
T1-editor-r2.log
Assets\Editor\B09Authoring.cs(51,23): error CS1012: Too many characters in character literal
Assets\Editor\B09Authoring.cs(51,29): error CS1012: Too many characters in character literal
Assets\Editor\B09Authoring.cs(51,128): error CS1010: Newline in constant
Assets\Editor\B09Authoring.cs(51,128): error CS1012: Too many characters in character literal
Assets\Editor\B09Authoring.cs(51,131): error CS1003: Syntax error, ',' expected
Assets\Editor\B09Authoring.cs(52,3): error CS1525: Invalid expression term 'bool'
Assets\Editor\B09Authoring.cs(52,8): error CS1003: Syntax error, ',' expected
Assets\Editor\B09Authoring.cs(52,144): error CS1026: ) expected
Scripts have compiler errors.
Exiting without the bug reporter. Application will terminate with return code 1
```

T4 검사 도구 보정1회: 부동소수 2.000000000000001 오탐에 1e-6 허용 오차 적용. 첫 보고서는 `_raw/B16/room-rules-first.md`에 보존, 아래는 보정 전 원문이며 최종 판정 근거에서 제외한다.

```text
FAIL A01 reachability: surface=5 x=25 top=11.6 absent in 2u/3u static graph
FAIL A02 height: platform=1 nearest lower rise=2 (static candidate)
FAIL A03 height: platform=2 nearest lower rise=2 (static candidate)
FAIL A03 height: platform=5 nearest lower rise=2 (static candidate)
FAIL A04 height: platform=1 nearest lower rise=2 (static candidate)
```

최종 정적 검사 FAIL/WARN과 자체 검사 원문:

```text
FAIL A01 reachability: surface=5 x=25 top=11.6 absent in 2u/3u static graph
WARN A02 size: 28x36 title-derived room class
WARN A02 floor-item: top=1 no item mark within 2u
WARN A02 floor-item: top=2.9 no item mark within 2u
WARN A02 floor-item: top=4.9 no item mark within 2u
WARN A02 floor-item: top=9 no item mark within 2u
WARN A02 floor-item: top=10.5 no item mark within 2u
WARN A02 floor-item: top=12.4 no item mark within 2u
WARN A02 floor-item: top=21 no item mark within 2u
WARN A02 floor-item: top=35 no item mark within 2u
WARN A02 headroom: platform=0 clearance=1.5
WARN A02 headroom: platform=2 clearance=1
WARN A02 headroom: platform=3 clearance=1.4
WARN A02 reachability: no start mark; cross-room arrival reachability unverified
WARN A03 reachability: no start mark; cross-room arrival reachability unverified
WARN A04 reachability: no start mark; cross-room arrival reachability unverified
WARN A05 reachability: no start mark; cross-room arrival reachability unverified
WARN A06 headroom: platform=1 clearance=1.5
WARN A06 reachability: no start mark; cross-room arrival reachability unverified
PASS self-test synthetic door-safety / overlap / height detected
```

Python 기본 WindowsApps 별칭은 제한 환경에서 실행 불가였지만 번들 Python으로 검사에 성공했다. 자체 검사 가짜 JSON은 `_raw/B16/room-rules-fake.json`에 보존했다. 도구 자체 검사는 실제 게임 문 동작 검사가 아니다.

## 보존·검증 한계·다음 단계

- 원본 로그·임시 JSON·스크립트: `Validation/_raw/B16/`, 회귀는 `Validation/_raw/regression/B16-<Task>/`. 삭제하지 않고 커밋 제외.
- V4 Scene/Resources/Design은 git 비교 변화0. 기본 생성기를 재실행해 바이트 SHA 불변을 입증하는 T1 검사는 컴파일 전에 중단되어 미실시다. 원복 후 V5 제품 코드·지도 데이터가 없으며 Release 제외 검사를 새 빌드로 실행하지 않았다.
- 기존 Builds 두 개와 런처 경로 유지. 새 빌드가 없어 Housekeeping 삭제 대상도 없다. 자동 런처 검사는 별도 smoke 슬롯을 사용했다.
- Approved Backlog는 비어 있다. 실행 가능한 독립 작업까지 처리했으며, 다음 배치에서 T1부터 재개해야 한다. Sprint Gate 미실시.

종료 정리: 2026-10-16 11:01 KST. 카드는 T7 로컬 커밋 1a3a6e4 기준이며, 최종 커밋은 카드와 이 종료 기록을 추가한다. origin/main은 B15(9d81962)이고 Base 앞에 별도 미push 커밋이 없어 Base 이후 배치 커밋만 합쳐 태그 B16을 붙인다. 지시문·설계 입력 커밋 Base는 별도로 보존한다.

추가 보호 경로 확인: 68개 파일의 현재 SHA256을 `_raw/B16/protected-sha256.txt`에 기록했다. git checkout 필터의 CRLF 예상값과 기존 LF 파일은 바이트 비교가 달랐다(해당 원본을 변경하지 않음). 이어 Base blob과 줄바꿈 정규화 후 전부 동일함을 `_raw/B16/protected-normalized-sha256.txt`에서 확인했다. 이는 배치 시작 전 바이트 스냅샷이나 기본 재생성 검사를 대신하지 않는다.
