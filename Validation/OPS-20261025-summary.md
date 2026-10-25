# OPS-20261025 — 운영 개선 검증

Base B20 `bf9693a`, 기존 미푸시 커밋 2개 보존. 이번 게임 변경 없음. 최종 카드 결과는 Task 커밋 후 맨 위에 기록한다.

| Task | 상태 | 결과 |
| --- | --- | --- |
| OPS-T1 | DONE | TASKS 원문 정규화 비교 일치·과거 미종결18행 유지·7문서 링크불량0·두 skill frontmatter/원본 경로 확인. 원문: `_raw/OPS-20261025/T1-docs.txt` |
| OPS-T2 | DONE | AGENTS·Batch 템플릿·보호 목록 정리. 숫자 없는 문서 Task 허용, 위험도별 검증, 위반 변경만 제거, Queue 승인 범위 합산, V5 FAIL 원문 판정 명시 |
| OPS-T3 | DONE | Preflight 구문오류0·모의8경로 PASS·보호387 SHA변경0·문서 보관/링크 검사 PASS. 실제 Unity 미실시 |

미실시: Unity 기동·컴파일·게임 회귀·플레이. 이번 작업은 게임 소스·에셋·설계를 바꾸지 않는다.

T1 도구 한계: skill-creator quick_validate.py는 `ModuleNotFoundError: No module named 'yaml'`로 두 진입 파일 모두 실행 실패. 패키지 추가 없이 이번의 단순 name/description frontmatter와 원본 참조를 직접 검사로 대체했다. Claude Code 실제 /skills 표시 및 새 Codex 세션 자동 발견은 미실시. Codex 저장소 skill 위치는 [공식 문서](https://learn.chatgpt.com/docs/customization/overview)로 확인했다.

Ruling: 운영 작업 식별자는 OPS-20261025 / 게임 Batch21과 승인을 혼동하지 않기 위함 / 다음 게임 배치 번호에 영향 없음.

T2 확인: 기본 보호 패턴은 Design·PRD·GAME_DESIGN·Tuning asset/meta·Player prefab/meta·ProjectSettings이며, B20의 당시387개 스냅샷과 구분했다. 보호 파일이 필요하다는 이유만으로 안전한 Task 결과를 전부 되돌리지 않는다. Ready Queue는 승인된 제품 일감이 없어 0건 유지. P8·PRD 변경·README 정리는 제외했다.

## T3 실제 검증과 한계

원문: `Validation/_raw/OPS-20261025/T3-preflight-tests-final.txt`。

```text
PASS PowerShell parse errors=0
PASS skip-unity exit=0 status=SKIP_UNITY
PASS mock-success exit=0 status=PASS
PASS mock-license-198 exit=198 status=LICENSE_BLOCKED
PASS mock-other-error exit=1 status=UNITY_ERROR
PASS mock-timeout exit=124 status=TIMEOUT
PASS missing-executable exit=1 status=ERROR
PASS sandbox-localappdata exit=1 status=ERROR
PASS mock-project-open exit=3 status=PROJECT_OPEN
MEASURE actualUnityRuns=0; mocked process exits only; existing Editor was not stopped
```

- 実際の -SkipUnity 実行: Git·LOCALAPPDATA·Hub 정보 기록, Unity 미실행. 샌드박스 LOCALAPPDATA 리디렉션을 확인하고 Unity 경로에서는 사전 차단하도록 구현했다. Hub 프로세스0은 단독 실패 조건이 아니다.
- 每번 고유 preflight 폴더·preflight.txt·result.json·Unity 로그를 보존하며 기존 결과를 덮어쓰지 않는다. exit198은 그대로 반환하고 다른 Unity 오류·timeout과 구분한다. timeout은 자신이 시작한 프로세스만 종료한다.
- 검증 도구 첫 실행은 리디렉션된 stdout의 한글 경로가 잘못 해석되어 결과 JSON 조회에 실패했다. 제품 결과 파일은 정상 보존돼 있었으며, 도구만 새 evidence 폴더 탐색으로 바꿔 재검증했다. 최초 실패·재실행 폴더는 _raw에 보존했다.
- 원문 `closing-protection.txt`: `MEASURE protectedFiles=387 MEASURE protectedSHAChanges=0` (PowerShell 출력 한 줄). 문서7개 링크불량0·과거 TASKS 전체 원문 비교 일치·미종결18행 추적 유지.
- 미실시: 실제 Unity 라이선스·컴파일·13회귀·V5 물리·실제 Editor 프로젝트 잠금·실제 timeout 종료. 모의 PASS는 이 항목들의 통과를 뜻하지 않는다. 게임 파일을 바꾸지 않아 게임 회귀를 실행하지 않았다.
- 미실시: Claude Code /skills 및 새 Codex 세션의 자동 선택. 두 SKILL.md의 frontmatter·원본 경로만 확인했다.

Ruling: 실제 Unity 기동 대신 모의 프로세스로 종료 코드별 분기를 검증 / 라이선스 환경을 다시 조사하거나 게임 실행 없이 운영 스크립트의 제어 흐름 확인 / 실제 Unity 연동은 다음 needs-unity=yes Batch 시작 시 별도 확인 필요.
