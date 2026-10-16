# Batch 16b 결과 — V5 층 A 파일럿 이어하기

V5 플레이 가능·REVIEW. Scene6개·실제exe·15런처+NoMap3 PASS, Release V5 제외 PASS. Base `74732cae4b93a74f113dc283a1a2946d37f1a777`, 시작 2026-10-16 13:34:36 KST. 예산12 Task/6시간, 19:04:36 이후 새 Task 시작 안 함.
유형별 Task: feature5 / verification5 / doc2 / cleanup0 =12.

## Task별 결과

| Task | 유형 | 상태 | 결과 |
|---|---|---|---|
| T1a | verification | DONE | 기존 CLI 프로브에서 Z01 stub 대상 조회 예외 재현, 제품 변경0, SHA변경0, [원문](B16b-probe.md) |
| T1b | feature | DONE | 컴파일 오류0·V4재생성 SHA동일·V5임시6방 성공. Quiet13/13, [결과](B16b-generation.md) |
| T1c | feature | DONE | [patrol13/13·끝점26](B16b-patrol.md), V4 SHA동일·Quiet13/13 |
| T2 | feature | REVIEW | V5 선택·출력·fixture·요소 검사 코드 컴파일0, 입력62cc8c1 수신, [Scene6방·요소 일치](B16b-scenes.md). 수정본 Quiet13/13, T3 실제exe14문·저장PASS, 직접플레이대기 |
| T3 | verification | DONE | [실제exe PASS·14문·Error0](B16b-runtime.md), Quiet13/13 |
| T4 | verification | DONE | a777653·수정본 FAIL10/WARN4 기록·자체검사PASS·Quiet13/13 |
| T5 | feature | REVIEW | Development0오류·0경고, 15런처+V5 NoMap3 PASS·Release제외PASS·Quiet13/13 |
| T6 | doc | DONE | 인계·링크0불량·Quiet13/13 |
| T7 | verification | DONE | Quiet13/13·기본13/13·카드 범위/링크/없는런처0·현재런처18PASS |
| BL1 | feature | REVIEW | [지도6방/12문](B16b-map.md)·stub×4/화살표1·V4지도PASS·Quiet13/13 |
| BL2 | verification | DONE | [실제입력](B16b-route.md)·자연A01→A04·A04구간BLOCKED·독립A05대시1.48s·Quiet13/13 |
| BL3 | doc | TODO | 수정 데이터 비교 |

## 기획자에게

수정 데이터에서 Scene을 생성하고 요소 수·문14방향·순찰13개 재연결·조각4개·Dash 수집·Pause/Continue 저장 복원을 실제 exe로 확인했다. 문 검사는 문 앞 fixture의 물리 접촉 검사다. 자연 입력은 Backlog에서 별도로 측정하며, 자동 검사는 사람의 길 찾기·시야·포고 손맛을 판단하지 못한다.

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

## 종료 점검

진행 중. T7과 승인 Backlog를 마친 뒤 최종 카드·기본 회귀·SHA·태그를 기록한다. Base 앞의 미push 커밋은 합치지 않는다. push·Sprint Gate 없음.

T7 중간 점검: Quiet13/13. 카드(a777653 이후 문서/런처 포함9269fad) 허용 범위 밖0·TASKS 링크불량0·없는 빌드/런처0, 결정0·열린 버그3. 사용자 입력 Design3경로는 명시적 별도 커밋 허용을 카드에 반영했다. V4 보호63파일 SHA 변경0. T5의 현재 동일 전달exe 런처18검사를 재사용했다(이후 제품 변경0). 기본 회귀는 별도 명령으로 진행 중.


T7 최종: B16b-T7 Quiet13/13, B16b-T7-default 기본13/13. 카드 원본 _raw/B16b/T7-card.txt. 본 Task 완료 후 승인 Backlog1~3 진행.
