# Batch 16b 결과 — V5 층 A 파일럿 이어하기

V5 플레이 불가(진행 중). Base `74732cae4b93a74f113dc283a1a2946d37f1a777`, 시작 2026-10-16 13:34:36 KST. 예산12 Task/6시간, 19:04:36 이후 새 Task 시작 안 함.
유형별 Task: feature5 / verification5 / doc2 / cleanup0 =12.

## Task별 결과

| Task | 유형 | 상태 | 결과 |
|---|---|---|---|
| T1a | verification | DONE | 기존 CLI 프로브에서 Z01 stub 대상 조회 예외 재현, 제품 변경0, SHA변경0, [원문](B16b-probe.md) |
| T1b | feature | DOING | stub·oneway 최소 수정, 수정마다 컴파일 |
| T1c | feature | TODO | Scene 순찰 끝점 연결 |
| T2 | feature | TODO | 6 Scene·V5 모드·아이템 수 |
| T3 | verification | TODO | 실제 exe 문·도착·능력 검사 |
| T4 | verification | TODO | 포고/도착 시작점/위아래 아이템 정적 검사 |
| T5 | feature | TODO | Development·Release 제외·3런처 |
| T6 | doc | TODO | 실제 상태 인계 |
| T7 | verification | TODO | 마지막 회귀·카드·모든 런처 |
| BL1 | feature | TODO | stub×·oneway 화살표 지도 |
| BL2 | verification | TODO | 실제 입력 경로 측정 |
| BL3 | doc | TODO | 수정 데이터 비교 |

## 수신 데이터

Base에 수신한 변경은 WORLD_V5/rooms_v5_A의 A04 변경뿐. 지시문에 적힌 A02 조각·A05 도착·world rect/문 ID 수정은 미수신으로 사용자에게 확인했다. 사용자가 추가 수정본 반영 예정이라고 답했으므로 기다리면서 독립 생성기 작업을 수행한다. 에이전트는 Design을 수정하지 않는다. Batch16 검증 파일은 읽기 전용.
