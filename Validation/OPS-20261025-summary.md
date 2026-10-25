# OPS-20261025 — 운영 개선 검증

진행 중. Base B20 `bf9693a`, 기존 미푸시 커밋 2개 보존. 이번 게임 변경 없음.

| Task | 상태 | 결과 |
| --- | --- | --- |
| OPS-T1 | DONE | TASKS 원문 정규화 비교 일치·과거 미종결18행 유지·7문서 링크불량0·두 skill frontmatter/원본 경로 확인. 원문: `_raw/OPS-20261025/T1-docs.txt` |
| OPS-T2 | DONE | AGENTS·Batch 템플릿·보호 목록 정리. 숫자 없는 문서 Task 허용, 위험도별 검증, 위반 변경만 제거, Queue 승인 범위 합산, V5 FAIL 원문 판정 명시 |
| OPS-T3 | TODO | Preflight·최종 검증 |

미실시: Unity 기동·컴파일·게임 회귀·플레이. 이번 작업은 게임 소스·에셋·설계를 바꾸지 않는다.

T1 도구 한계: skill-creator quick_validate.py는 `ModuleNotFoundError: No module named 'yaml'`로 두 진입 파일 모두 실행 실패. 패키지 추가 없이 이번의 단순 name/description frontmatter와 원본 참조를 직접 검사로 대체했다. Claude Code 실제 /skills 표시 및 새 Codex 세션 자동 발견은 미실시. Codex 저장소 skill 위치는 [공식 문서](https://learn.chatgpt.com/docs/customization/overview)로 확인했다.

Ruling: 운영 작업 식별자는 OPS-20261025 / 게임 Batch21과 승인을 혼동하지 않기 위함 / 다음 게임 배치 번호에 영향 없음.

T2 확인: 기본 보호 패턴은 Design·PRD·GAME_DESIGN·Tuning asset/meta·Player prefab/meta·ProjectSettings이며, B20의 당시387개 스냅샷과 구분했다. 보호 파일이 필요하다는 이유만으로 안전한 Task 결과를 전부 되돌리지 않는다. Ready Queue는 승인된 제품 일감이 없어 0건 유지. P8·PRD 변경·README 정리는 제외했다.
