# AI 작업 규칙

## Source of Truth

사용자의 명시적 지시를 우선한다. 문서 충돌은 아래 순서로 판단하고 TASKS의 Decisions Needed에 남긴다.

1. PRD.md — 제품 범위와 완료 조건
2. GAME_DESIGN.md — 게임 규칙과 설계
3. TASKS.md — 현재 작업·상태·검증 기록
4. README.md — 실행 안내

이 파일은 에이전트 실행 절차만 다룬다.

## Context Rule

매 작업마다 5개 문서를 전체 재독하지 않는다. AGENTS의 관련 규칙, TASKS의 Current Sprint / Current Tasks / Latest Handoff, GAME_DESIGN의 해당 Task 절, PRD의 관련 Scope만 확인한다. README는 실행 방법이나 프로젝트 구조가 필요할 때 읽는다.

Sprint 변경, Scope 변경, 설계 충돌, 사용자의 전체 검토 요청 때만 전체 문서를 다시 확인한다.

## Work Rule

- 기존 파일·사용자 변경·의존성을 확인하고 현재 승인된 Task만 수행한다.
- 관련 없는 기능·리팩터링·불필요한 패키지·미래 확장용 구조를 추가하지 않는다.
- 이미 정상 확인된 환경은 새로운 문제나 변경이 없으면 반복 조사하지 않는다.
- 승인 범위가 끝나면 종료한다. 의존성 충족은 새 작업의 승인을 뜻하지 않는다.
- Git 이력의 일회성 생성 스크립트를 복원하거나 재실행하지 않는다.

## Designer Control

주요 값은 Inspector 또는 ScriptableObject에 단위·Tooltip·논리적 그룹과 함께 노출한다. 공통 수치는 설정 Asset, 배치·영역은 Scene을 원본으로 두며 반복 오브젝트는 Prefab으로 만든다. 인계에는 수정 위치와 Play 종료 후 값 반영 방법을 안내한다.

## Design Change

PRD의 범위나 GAME_DESIGN의 규칙·능력·진행 구조 변경은 사용자 결정이 필요하다. Prototype Value는 명시된 탐색 범위 안에서 이전 값을 보존한 후보 실험이 가능하다. 가설·전후 값·실제 결과를 TASKS에 기록하며 사용자 승인 없이 후보를 기본값으로 확정하지 않는다.

## Review

상태와 의존성 표기는 TASKS를 따른다. REVIEW는 기술 구현과 관련 Unity 검증이 완료되어 사용자 판단을 기다리는 상태다. 감각·설계 수락이 필요한 Task는 사용자가 수락한 뒤 DONE으로 바꾼다. 별도 사용자 판단이 없는 Task는 완료 조건과 검증을 충족하면 DONE으로 둔다.

승인 범위의 기술 후속 작업은 REVIEW를 기반으로 진행할 수 있다. 감각·설계 수락 의존성은 DONE이어야 한다. 회귀로 선행 검증이 무효가 되면 해당 Task를 DOING 또는 BLOCKED로 되돌리고 영향받는 후속 경로는 재검증 전까지 중단한다.

## Validation

실행하지 않은 테스트를 통과했다고 기록하지 않는다. 구현 Task는 관련 Unity 실행·핵심 동작·회귀·런타임 오류·Editor 설정을 확인해야 REVIEW 또는 DONE이 된다. Unity 검증이 불가능하면 해당 경로를 BLOCKED로 기록한다. 문서·결정 작업에는 게임 실행 조건을 적용하지 않는다.

작은 Task마다 전체 검증을 반복하지 않고 변경 위험에 맞는 검사만 수행한다. 전체 회귀는 Vertical Slice, Sprint 종료, Core Loop 통합, Content Complete, Release Candidate에 집중한다. 증거와 미실시 항목을 TASKS에 남긴다.

## Handoff

TASKS의 Latest Handoff에 완료 작업·상태, 주요 변경 파일, 실제 검증 결과, 사용자 확인 사항, 문제, 다음 작업만 기록한다. 5분 안에 읽을 수 있게 유지하고 상세 증거는 링크한다.

## Stop Conditions

게임 규칙 변경, 승인 밖 기능, 사용자 개입, 동일 오류 해결 반복, 예상 범위의 큰 증가가 필요하면 해당 경로를 중단하고 TASKS의 Bugs 또는 Decisions Needed에 문제·영향·필요 판단을 적는다. 다른 독립적인 승인 Task는 계속할 수 있다. Sprint Gate를 통과하기 전 다음 Sprint 콘텐츠를 확장하지 않는다.
