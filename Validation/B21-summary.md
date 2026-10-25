# Batch21 — 정적 잠금 계약 완료, Unity 경로 차단

| 종료 검사 | 이번 결과 |
| --- | --- |
| Preflight | **FAIL exit198**. Unity 라이선스 외부 의존성. 동일 환경 재시도0 |
| 회귀 Quiet/기본 | **미실시**. Preflight exit198. 과거 13/13을 이번 통과로 세지 않음 |
| V5 정적 | 동일 draft3.1·기본 h1.6: FAIL1→0, WARN0→0, 새 FAIL0. self-test PASS |
| V5 Scene clearance | **미실시**. Unity exit198. Scene 재생성·저장 없음 |
| Batch-Card | T3 커밋 `736c6aa` 기준 범위 밖0·TASKS 깨진 링크0·없는 런처0. [원문](_raw/21/T3-20261026-001128/batch-card-task-committed.txt). 종료 커밋 후 [최종 카드](_raw/21/T3-20261026-001128/batch-card-final.txt)도 보존 |
| 보호/작업 트리 | Base 이후 보호88파일 SHA 변경0. [보호 기록](_raw/21/T3-20261026-001128/protection.txt). 커밋 사이 검사와 별도로 최종 status/diff·미추적 범위·문서 링크 확인 |

2026-10-26 사용자 “Batches/BATCH-21.md 읽고 진행해줘” 실행 승인. Base `7d40918`은 지시대로 기존 세 파일만 커밋한 직후다. **Claude 설계 변경:** 이 커밋의 `Design/v5/WORLD_V5.md`·`rooms_v5_A.json`은 지름길 잠금/벽 방향 계약만 추가했으며 좌표·수치 변경 없음. Base 이후 Design·게임 C#·에셋·기본 수치·Player.prefab·README·빌드·런처 변경 없음.

## 결과와 다음 재개 지점

| Task | 상태 | 커밋·결과·한계 |
| --- | --- | --- |
| T1 | BLOCKED | Unity exit198로 생성기/런타임 구현 미착수. 라이선스 환경 변경 후 여기서 재개 |
| T2 | BLOCKED | T1·Unity 의존. draft3.1 Scene6개 재생성·잠금/공격 방향/양방향/Continue 실제 exe 5항목 미실시 |
| T3 | DONE | `736c6aa`. [검사기·fixture·근거](B21-room-rules.md). 잠긴 우회 MEASURE, 누락/잘못된 키/다른 벽 키/봉인 우회 FAIL. 6방 전체 지름길 외 판정·요소 동일 |
| T4 | BLOCKED | Unity 의존. B21Arena 미생성, 가로 점프/공중 대시/지상 대시 후 점프/대시만 간격 0.05u·3회 측정 미실시 |
| T5 | BLOCKED | Unity 의존. 위험 지형 복귀 후보·후보 Asset/런처 미생성, 도랑 폭4.5/5/5.5/6u×능력2종×3회 미실시 |
| T6 | BLOCKED | Unity 의존. 머리 모서리 보정 후보 미구현, 허용값 미선택. PlayerMotor 수정0, 꺼짐 동일성·켜짐36면·10회 동작 미검증 |
| T7 | BLOCKED | needs-unity=no이나 선행 T4 차단. 36면 부족분 표 미작성. 기존 B20 자료를 새 측정으로 제시하지 않음 |
| T8 | BLOCKED | T1~T6·Unity 의존. 전달 빌드·8런처각10초·회귀·Scene 검사 미실시. 실행 가능한 정적/보호/카드 점검만 종료 절차로 수행 |
| T9 | BLOCKED | T8 의존. 후보 전달 안내·D-HAZARD-RETURN/D-CORNER-FIX 추가 보류. 존재하지 않는 후보 런처로 수락을 요청하지 않음. 의무 종료 인계는 이 문서와 TASKS에 완료 |
| BL1 | BLOCKED | T7 의존. 몸 높이1.3/1.1 여유 표 미작성 |
| BL2 | BLOCKED | T6·Unity 의존. 대시 모서리 보정 미착수 |
| BL3 | BLOCKED | T5·Unity 의존. A04 도랑 포고 경로5회 미실시 |

새로 플레이 가능해진 범위는 없다. 실제 A05 잠금·도랑·머리 걸림 버그는 유지된다. 이번 안정 결과는 draft3.1의 검사 계약과 회귀 fixture다. Task1개(T3) 수행·커밋 완료, 필수8개·Backlog3개는 선행/외부 의존성으로 차단. Queue0이며 새 범위를 임의로 시작하지 않았다. 예산6시간·Task상한20 도달 때문이 아니다.

## 실제 원문과 미실시

[Preflight 기록](_raw/21/preflight-20261026-001034-506-6ac6d75a/preflight.txt)·[Unity 로그](_raw/21/preflight-20261026-001034-506-6ac6d75a/unity.log):

```text
MEASURE LOCALAPPDATA=C:\Users\gosu3\AppData\Local
MEASURE localAppDataExistingAbsolute=True sandboxLocalAppData=False expectedUsualSuffix=AppData\Local
MEASURE unityExecutable=C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe timeoutSeconds=300
MEASURE unityExitCode=198
FAIL Unity startup exit=198; Unity tasks BLOCKED(external dependency); do not retry unchanged environment; non-Unity tasks may continue
No valid Unity Editor license found. Please activate your license.
Exiting without the bug reporter. Application will terminate with return code 198
```

샌드박스 밖의 실제 Unity 기동 결과다. Hub 프로세스0을 실패 원인으로 판정하지 않았다. exit198 규칙대로 재조사·재실행하지 않고 독립 T3만 계속했다. 새 실행 로그는 `_raw/21`에 보존했다.

[T3 원문](B21-room-rules.md)의 핵심:

```text
FAIL A05 shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0
MEASURE A05 shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0; 잠금으로 해소 key=V5-A05-breakable0; runtime lock NOT VERIFIED
PASS draft3.1 full static FAIL=0 WARN=0; old FAIL=1 WARN=0
PASS B21 locked spawn bypass MEASURE; unlocked/bad-key/unrelated-key/seal bypass FAIL; a/b order independent
PASS draft2 five synthetic rules detected; draft3 four JSON fixtures detected; clean fixture PASS
```

FAIL은 변경 전 판정이며 이번 정적 결과와 구분한다. 새 음성 fixture의 FAIL 원문과 기존 합성 FAIL/WARN은 [self-test 전체](_raw/21/T3-20261026-001128/current-self-test.md)에 있다. 대상은 `Design/v5/rooms_v5_A.json`·`world_v5_A.json`, Python 경로·전체 명령·동일 입력 비교는 [T3](B21-room-rules.md)에 기록했다. Unity 경로는 위 로그와 같고 Scene 대상은 `Assets/Scenes/V5/*.unity`; Development 빌드/새 기준/Scene 출력은 생성하지 않았다. `--max-step-evidence` 미지정으로 trench-escape MEASURE도 미실시.

후보 꺼짐/켜짐 비교 수치, T4 가로 한계, T5 도랑 폭 표, T7 여유 수치는 **이번 실행에서 없음**. B20의 풀점프2.52002·짧은점프1.00800·턱2.51250·대시2.87988은 [과거 근거](B20-trench.md)로만 남으며 이번 재현 PASS가 아니다.

## Ruling·인계

- self-test 출력 루트 옵션 추가 / 기존 fixture 출력을 Batch21의 `_raw/21` UUID 폴더로 모으기 위함 / 검사 판정·게임 영향 없음.
- T7·BL1도 선행 T4/T7을 준수해 차단 / needs-unity=no가 선행을 면제하지 않음 / 표 작성은 재개 시 수행.
- 새 후보 결정2개는 생성하지 않음 / T9 선행 미충족·전달 가능한 Asset/런처 없음 / 기존 결정2개와 수락 순서 유지, 후보가 전달되면 추가.
- Archive는 이번 허용 범위가 아니므로 이전 OPS 표를 TASKS에 유지 / 범위 밖 쓰기 방지 / 원문 인계는 [시작 스냅샷](_raw/21/T3-20261026-001128/TASKS-before.md)에 보존, 기존 [OPS 요약](OPS-20261025-summary.md) 연결 유지.

사용자에게 필요한 것은 Unity 라이선스 활성화 등 **환경이 바뀌었다는 확인**이다. 그 후 B21 미완료 경로를 재개한다. 기존 V4/V5·BodySize 수락 순서는 유지하며 이번에 추가할 후보 플레이는 없다. 기존 D-BODY(1.6/1.3/1.1)·D-V5-DRAFT4 판단은 별개다. 감각 수락·Sprint Gate·기본 크기 변경 없음.

이전8개 미푸시 커밋이 있어 squash하지 않고 시작 계약/T3/종료 커밋을 보존했다. 최종 종료 커밋에 B21 태그를 붙이며 push는 하지 않는다. 카드의 과거 REGRESSION 행은 이번 증거가 아니다.
