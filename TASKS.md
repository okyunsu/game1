# 개발 작업과 기록

원문 전체: [TASKS-before-2026-10-12](Archive/TASKS-before-2026-10-12.md). 이 파일은 원문을 옮겨 요약했으며 새 판단을 추가하지 않았다.

## 1. 현재 상태

- Sprint3 / Batch10 예산 종료(2026-10-12 19:06 KST). 다음 Batch는 사용자 승인 후 진행. 기본 채택·감각 수락·Sprint Gate 미실시.
- 기준 커밋: B10 종료 50a3662, 정리 후 HEAD는 `git log -1`.
- 숫자 카드(50a3662..b4907a5, 정리 작업): 2071파일 +3069/-198줄. B10 Task: DONE 9 / PARTIAL 7 / BLOCKED 1 (아래 6번 표).
- 회귀: 기본 `REGRESSION 13/13` (Tools/Run-Regression.ps1, 2026-10-12). B10 종료 시 RouteAll·Pogo PASS, V2 W22 실제 사망.

## 2. 결정 대기

| ID | 무엇을 결정 | 결정 방법 | 필요한 것 | 생긴 배치 | 원문 링크 |
|---|---|---|---|---|---|
| D-COMBAT | 전투 손맛: B01 히트스톱 0.05s 유지/조정/끄기, B04-AV1 사운드·형태·명도 채택과 B2/fast/slow 보스 선택, Q6 A/B/C 피드백 후보 선택, F2R 표시 수락 | 플레이 각 2~5분 비교 후 선택 | 없음(재빌드 필요) | B01, B04, Q-night4, F2R | [B01 결과](Validation/B02-C-summary.md), [B04](Validation/B04-final-summary.md), [Q6](Validation/Q6-feedback-candidates.txt), [F2R](Validation/F2R-summary.txt) |
| D-MOVE | 이동 손맛: B06 벽 점프 프리셋(기본1/끈적2/빠름3)·V2 가이드 문구, B10 장치 기본/후보1/후보2/수정(BL2, AI 추천 기본 유지) | 장치 코스 플레이 10분(F로 후보 순환) | [Play-V4-Mechanics](Launchers/current/Play-V4-Mechanics.cmd); B06 프리셋 코스는 없음(재빌드 필요) | B06, B10 | [BL2](Validation/B10-BL2-summary.md), [원문](Archive/TASKS-before-2026-10-12.md) |
| D-LOOK | 화면·방: B05-V2 CC0 픽셀 스타일 유지/교체, A지역 밀도·필수/선택 난이도, V2 기본 교체 시점 | 플레이 후 선택 | 없음(재빌드 필요) | B05 | [LEVEL_V2](LEVEL_V2.md), [원문](Archive/TASKS-before-2026-10-12.md) |
| D-RULE-T5 | T5 "다른 문" 기준: 같은 문 귀환·자연 획득/엔딩 재개를 탈출로 허용 / 문 기준 유지 후 설계 별도 승인. BL4: B09 Backlog4~9 원문 제공 / 범위 제외 | 선택지 | 불필요(선택지) | B10 | [T5](Validation/B10-T5-summary.md) |
| D-RULE-DIST | 낙하 거리: B09 15개 약2.2u·Z01→S02 2u 예외 허용 / .3~.5u 이동 승인. Claude 수정 수락 / 수정 요청 / 취소 | 선택지(확인용 새 게임 25~30분) | [Play-V4](Launchers/current/Play-V4.cmd) | B09, B10 | [B09-T6](Validation/B09-T6-summary.md), [T12 보고서](Validation/B10-geometry-report.md) |
| D-NEXT | 다음 배치 방향: 남은 자동 경로 재시도 Batch 승인 / 먼저 직접 플레이. B07 A02·C03 후보 유지·수정 포함 | 선택지 | 불필요(선택지) | B07, B10 | [원문](Archive/TASKS-before-2026-10-12.md) |
| D-BOSS | V2 W22 보스 고정 입력 사망: 조사 / 보류 | 선택지 | 불필요(선택지); V2 재현 빌드는 없음(재빌드 필요) | B07, B09 | [B07-T25](Validation/B07-T25-summary.md), [B09-T12](Validation/B09-T12-summary.md), [B10-T8](Validation/B10-T8-summary.md) |

## 3. 직접 플레이 확인

- [Play-V4-Mechanics](Launchers/current/Play-V4-Mechanics.cmd): 6장치·E4·Slam 손맛, 10분.
- [Play-V4](Launchers/current/Play-V4.cmd): A01부터 V4 경로·되돌아가기, 25~30분(자동 자연 진행은 C02까지).
- [Play-V4-AfterDash](Launchers/current/Play-V4-AfterDash.cmd): 이어하기→CP-A03 대시 복원, 5분.
- [Play-V4-AfterWall](Launchers/current/Play-V4-AfterWall.cmd): 이어하기→CP-B01 대시+벽 점프 복원, 5분.
- [Play-V4-BeforeBoss](Launchers/current/Play-V4-BeforeBoss.cmd) / [Play-V4-AfterEnding](Launchers/current/Play-V4-AfterEnding.cmd): 자연 저장 없음, 실행 불가.

조정 값 위치와 Play 종료 후 반영은 [LEVEL_V4](LEVEL_V4.md).

## 4. 열린 버그

| ID | 증상 | 근거 |
|---|---|---|
| B07-V2-BOSS-REPLAY / B09-REGRESSION | 새 빌드 V2 고정 입력이 W22 보스에서 실제 사망(tick285, 진입편차.082u), 이전 B06b 빌드는 PASS. 원인 미확정 | [B07-T25](Validation/B07-T25-summary.md), [B09-T11](Validation/B09-T11-summary.md) |
| B07-A02-TRAVERSE | A02 필수 계단·재방문 굴뚝: 자동 입력 미완주, 사람 확인 필요 | [B07-R-A02](Validation/B07-R-A02-summary.md) |
| B07-C03-OPTIONAL | C03 첫 선택 포고 도전: 자동 입력 미완주, 사람 확인 필요 | [B07-R-C03](Validation/B07-R-C03-summary.md) |

Experimental-F1: 닫힘. F1 구성(히트스톱·표시)이 B01 F1-R(히트스톱, F2R 30/60/120fps 검사)과 F2R(표시 REVIEW)로 재구현됨. [B01 인계](Archive/B01-latest-handoff-20261007.md), [F2R](Validation/F2R-summary.txt).

## 5. 보관된 항목 색인

상세는 [원문](Archive/TASKS-before-2026-10-12.md)과 [이전 이력](Archive/TASKS_ARCHIVE.md).

| ID | 한 줄 요약 | 링크 |
|---|---|---|
| B10 종료 노트 | 70경로 성공 입력2회, 재생 안정성 PASS와 구분, r54~r58 결과 제외 | [원문](Archive/TASKS-before-2026-10-12.md) |
| B10-RESUME | Claude 수정 적용, 10방 이전 PASS 무효·T2-resume만 근거 | [원문](Archive/TASKS-before-2026-10-12.md) |
| B10-FAST-CLOCK | 무제한 렌더링 r19·r17 이전 T5 결과는 판정 제외 | [원문](Archive/TASKS-before-2026-10-12.md) |
| B09-INPUT-COVERAGE | 전 연결31 미통과·70경로41 미완주(당시), 추가 합격 주장 없음 | [B09-T8](Validation/B09-T8-summary.md) |
| B04-R11 | R11 시작편차0.110u 입력 소진, 제품 회귀 미확정 | [B06b-T1](Validation/B06b-T1-summary.md) |
| B03-P03 | P03 goal pogos=1 간헐 FAIL/Error0, 해결 주장 없음 | [B03-P03](Validation/B03-P03-summary.md) |
| B03-BOSS-VALIDATION | 최종 도약 회피 전체fps·동시HP0 미검증, 감각 수락 대기 | [B04-C](Validation/B04-C-summary.md) |
| N9-ROUTE | R05 사망으로 이전 완주 판정 철회, 추가 탐색 BLOCKED | [N9 재확인](Validation/N9-route-recheck.txt) |
| D0 | 기준 상태 T2/T4/S2 실패 재현, 회귀 판정 제외 | [D0](Validation/D0-summary.txt) |
| F2-T2 | F2 넉백 잠금 검사 재실패로 전체 복원 | [T2 재실패](Validation/Verification-night-T2-F2-rerun.txt) |
| ROOM-001 | 배치 모드 Tile 저장 0셀, 대안 B 블록 지형으로 우회, OPEN | [H5](Validation/ROOM-001-h5.txt) |
| RESET-001 | Sliced+autoTiling 콜라이더 불일치, Simple 방식 사용 | [원문](Archive/TASKS-before-2026-10-12.md) |
| N5 / UI-CAP-001 | 검증 도구 한계(T-A/D/E/F/G), 캡처 모듈 참조 불가 | [N5](Validation/N5-final-handoff.md) |
| S3T01-POGO | P03 고정 입력 반복5회 중 4PASS/1FAIL, REVIEW | [pogo5](Validation/B02-pogo5-summary.md) |
| S3T01-LEVEL | A-1 지정 입력 실패로 제외, 재적용은 별도 판단 | [A-1](Validation/S3T01-A-1-shelf-a3.txt) |
| Q-night4 | Q1 저장 수정 완료(S2-04 REVIEW), Q2~Q5 미완료, Q7 촬영 실패 | [원문](Archive/TASKS-before-2026-10-12.md) |
| LOG-001 | 사망 원인 정확 기록 미구현, 관찰 이벤트 허용 여부 판단 필요 | [LOG](Validation/LOG-playtest.txt) |
| CAM-002 / K2 | DEFERRED, 아트 적용 후 상대 거리로 재확인 | [원문](Archive/TASKS-before-2026-10-12.md) |
| Experimental-F1 | 닫힘(4번 근거) | [B01 인계](Archive/B01-latest-handoff-20261007.md) |
| Approved Backlog | Batch09 세 항목 처리 완료, 이전 승인 Backlog 소진 | [원문](Archive/TASKS-before-2026-10-12.md) |

## 6. 최근 배치 결과 (B10)

| Task | 상태 | 현재 결과 |
|---|---|---|
| B10-T1 | DONE | 장치9개·기존 경로10개 저장 입력/전체 경로2회 PASS |
| B10-T2 | DONE | 재개 설계 70/70 저장 전체 입력2회 PASS; A02/wall·C02/low·Z01/up_A05 해소. r54~r58 이전 Scene 결과 제외; r59/r61/r63 현재 Scene·설계 hash 확인 |
| B10-T3 | DONE | covered floor trigger inset 수정 47b20ac, Claude 설계 수정 662cc0c / V4 재생성 24ceeda, 21방 검사 PASS |
| B10-T4 | PARTIAL | 실제 전체 왕복 저장2회 PASS34건, 나머지 탐색/판정 미완료 |
| B10-T5 | PARTIAL | 최소 능력 다른 출구 저장2회 PASS36입구, 고립0 미증명 |
| B10-T6 | PARTIAL | 새 게임·대시·벽 점프·B05 자연 등반→C02; S02 미완주 |
| B10-T7 | DONE | 보스전→Slam→천장→S01 저장 입력2회 연속 PASS |
| B10-T8 | DONE | 동일 HEAD 과거 빌드 고정 입력2회 PASS; 빌드 구성으로 범위 축소, 원인 커밋 미확정/수정0 |
| B10-T9 | DONE | 작성된 시작→끝 16구간, 전체 저장 입력2회 PASS; 장치 작동은 T1 별도 |
| B10-T10 | PARTIAL | 70경로30fps43PASS/27FAIL,120fps59PASS/11FAIL; 자연 미실시 |
| B10-T11 | DONE | 27 실제 입력 조합 unexpected=0, Error0 |
| B10-T12 | PARTIAL | Claude 보고서·실제 Scene4방 촬영; 남은 분기 물리 원인 미확정 |
| B10-T13 | PARTIAL | 기본13+RouteAll+Pogo PASS, V2 W22사망; 전달1개·4기동PASS, 자연 저장2개 미확보 |

| 항목 | 상태 | 결과 |
|---|---|---|
| BL1 | DONE | V4 회귀 등록1회63/69 PASS,6 FAIL |
| BL2 | DONE | 장치19경로 기본17/후보1 11/후보2 12 PASS, 기본값 유지 |
| BL3 | PARTIAL | C01까지 완료 구간 시간, 전체 미완주 |
| BL4 | BLOCKED | 참조된 B09 Backlog4~9가 BATCH-09에 없음(1~3만 있음), 합격 기준 결정 필요 |

Task별 요약: [T1](Validation/B10-T1-summary.md) · [T2](Validation/B10-T2-summary.md) · [T3](Validation/B10-T3-summary.md) · [T4](Validation/B10-T4-summary.md) · [T5](Validation/B10-T5-summary.md) · [T6](Validation/B10-T6-summary.md) · [T7](Validation/B10-T7-summary.md) · [T8](Validation/B10-T8-summary.md) · [T9](Validation/B10-T9-summary.md) · [T10](Validation/B10-T10-summary.md) · [T11](Validation/B10-T11-summary.md) · [T12](Validation/B10-geometry-report.md) · [T13](Validation/B10-T13-summary.md) · [BL1](Validation/B10-BL1-summary.md) · [BL2](Validation/B10-BL2-summary.md) · [BL3](Validation/B10-BL3-summary.md)

## 7. 다음 배치 후보

- C02 상자 경유→대시 자연 입력부터 새 게임 재시도 → C03/C05/C04→보스→엔딩→Z01→S02 → 실제 보스 전/엔딩 후 저장 확보 → 자연 입력30/120.
- 남은 입구/왕복의 경유 노드·넓은 도약 위치 탐색, 실제 막힘 입증 후 승인 한도 데이터 수정.
- V2 W22 빌드 구성 비교는 별도 기록 기반. 기본 수치 채택·Sprint Gate는 사용자 수락 후.
