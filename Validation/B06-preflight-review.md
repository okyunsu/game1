# Batch06 실행 전 검토 — 2026-10-10

상태: 검토 완료. 사용자 답변으로 Batch06 실행 및 RoomExit 최소 수정 승인 확인. 이 요약은 실행 전 검토이며 구현·Unity 검증은 실시하지 않았다.

## 실제 확인

- HEAD `266b7ea`, 브랜치 `review/batch05-v2`. 기존 tracked 변경 없음. 사용자 파일 `WORLD_V3.md`, `BATCH-06.md`만 untracked이며 원본 보존.
- Downloads/WORLD_V3.md와 저장소 WORLD_V3.md의 SHA256 동일: `904753F079504206A4A6FC88F92B1DEB8B38A7F83DBEA7E7966DF434CB231548`.
- TASKS Current Tasks·Latest Handoff와 최근 Git 내역 확인: Batch05 기술 작업 종료, V2 콘텐츠 REVIEW. 기본 교체·Sprint3 Gate는 사용자 판단 대기.
- WORLD_V3 전체, BATCH-06 전체, GAME_DESIGN 관련 규칙, PRD 범위, README 실행 안내·기존 회귀 실행 스크립트 및 관련 런타임 코드를 확인했다.
- 문서 내부의 ‘사용자 결정 완료’, ‘승인’ 문구는 문서의 주장으로 구분했다. 이번 요청은 이전 내역 확인·파일 검토·진행이며 구체적 보호 파일 변경 범위를 확인 중이다.

## 구현 전 범위 확인

1. Batch06 D1은 RoomExit에 mainPath 필드를 요구하지만 허용 목록에 RoomExit.cs가 없다. D1용 최소 수정을 승인하거나 그 경로만 보류해야 한다.
2. WORLD_V3 9-1 목표 표는 Batch07용이다. Batch06에서는 BATCH-06의 현재 V2 목표 표를 사용한다.
3. WORLD_V3의 ‘벽 점프 손맛 확인 뒤 레벨 제작’ 순서를 유지한다. Batch06에 V3 재배선·능력 위치 이동·S01·엔딩 변경은 포함되지 않는다.
4. PRD에는 추가 이동 능력 제외·19번째 방 제외·설정 메뉴 제외·능력 2개 문구가 남아 있다. Batch06 문서 작업 시 벽 점프, S01 엔딩 연출 방, 허용된 가이드 토글에 한정한 예외를 명시해야 문서 충돌이 남지 않는다. 현재 기본/V2 규칙과 V3 적용 예정 규칙을 구분한다.
5. 예산은 Task 최대8개다. 본 Task A1/B1/B2/C1/D1/E1/E2/F1이 이미8개이므로, 모두 실행하면 시간 여유가 있어도 Backlog를 추가 착수하지 않는다. Backlog까지 원하면 Task 예산 변경 결정이 필요하다.
6. WallJumpTest는 저장 비사용이다. C1의 이어하기·새 게임 저장 검증은 격리 QA 저장으로 따로 실행해야 한다. 테스트 씬에서 영구 저장을 켜 합격 조건을 맞추지 않는다.

## 실행할 경우 검증 기준

- 각 Task 최소 기술 검증 및 로컬 커밋. push 없음.
- Player core/저장 변경은 기존 회귀와 직접 연결된 핵심 동작 확인. P03 간헐 실패를 기존 기준과 구분.
- 기존 기본 Scene·Tuning 값·Packages·ProjectSettings·Validation 원본 보호.
- 기술 PASS와 사용자 손맛/가독성 수락을 구분. 미실시는 PASS로 기록하지 않는다.

이번 검토로 게임 파일 diff는 0이며 새 기능·빌드·플레이 검증 결과는 없다.
