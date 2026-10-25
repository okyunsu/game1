# B21 T3 — draft3.1 잠금 검사 계약

상태 DONE. **Claude 설계 변경:** 시작 커밋 `7d40918`, `Design/v5/WORLD_V5.md`·`rooms_v5_A.json`에 지름길 잠금/벽 방향 계약만 추가됨. 좌표·수치 변경 없음. 이후 Design 변경 없음. 런타임 T1·Scene T2는 Unity exit198로 BLOCKED이므로 정적 PASS는 실제 지름길 수정 완료를 뜻하지 않는다.

`shortcut-spawn-bypass`는 그 입구의 `closed_until_wall_broken`이 실제 우회한 breakable의 저장 키와 일치하면 MEASURE(잠금으로 해소)로 출력한다. 잠금 누락·잘못된 키·다른 실제 벽의 키·봉인 우회는 FAIL이다. 저장 키로 해제되는 breakable과 SealFloor는 구분하며, 기존 B20 fixture 파일은 변경하지 않았다.

## 이번 검증

- 기본 h1.6/폭0.7, draft3.1 `rooms_v5_A.json`·`world_v5_A.json` 동일 입력과 Base 검사기를 비교했다. 기존 FAIL1→0, WARN0→0, 새 FAIL0. 6방의 지름길 외 모든 판정·요소 수 동일.
- 기존 draft2 합성5종·draft3 JSON fixture4종·clean fixture·B20 정상/키 누락/틀린 키/봉인/a-b 교환 확인. 유효한 잠금이 있는 우회는 이번 승인 계약에 따라 FAIL→MEASURE로 바뀐다.
- 새 B21 fixture: 잠긴 우회 MEASURE, 무잠금/틀린 키/다른 벽 키/봉인 우회 FAIL, a-b 교환 결과 동일. Python compile·실제 CLI self-test 성공.

실제 명령(PowerShell, 저장소 루트):

```powershell
& 'C:/Users/gosu3/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe' -B Tools/Check-RoomRules.py Design/v5/rooms_v5_A.json --v5 --self-test --self-test-output-dir Validation/_raw/21/T3-20261026-001128/fixtures --world Design/v5/world_v5_A.json --output Validation/_raw/21/T3-20261026-001128/current-self-test.md
```

원본은 [정적·self-test 전체](_raw/21/T3-20261026-001128/current-self-test.md), [변경 전](_raw/21/T3-20261026-001128/baseline.md), [동일 입력 비교](_raw/21/T3-20261026-001128/proof.txt). 기존 출력은 덮어쓰지 않았다. 명령 재실행 때는 새 출력 경로가 필요하다.

```text
FAIL A05 shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0
MEASURE A05 shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0; 잠금으로 해소 key=V5-A05-breakable0; runtime lock NOT VERIFIED
PASS draft3.1 full static FAIL=0 WARN=0; old FAIL=1 WARN=0
MEASURE removed existing FAIL=1; remaining FAIL=0; new FAIL=0; only shortcut classification changed
FAIL B21-fixture-unlocked shortcut-closed: door=A05-A03 from=A03 to=A05 missing valid closed_until_wall_broken key
FAIL B21-fixture-unlocked shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0
FAIL B21-fixture-bad-key shortcut-closed: door=A05-A03 from=A03 to=A05 missing valid closed_until_wall_broken key
FAIL B21-fixture-bad-key shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0
FAIL B21-fixture-unrelated-key shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past breakable0
FAIL B21-fixture-seal shortcut-spawn-bypass: door=A05-A03 from=A03 spawn=(3.5,1) already past seal0
PASS B21 locked spawn bypass MEASURE; unlocked/bad-key/unrelated-key/seal bypass FAIL; a/b order independent
PASS draft2 five synthetic rules detected; draft3 four JSON fixtures detected; clean fixture PASS
```

첫 FAIL은 변경 전 실제 데이터 판정이다. `B21-fixture-*`와 `synthetic`의 FAIL은 의도한 위반 검출이며 제품 FAIL 합계에 넣지 않는다. 전체 원문에는 기존 합성 FAIL·WARN도 보존했다.

## 미실시·판정 한계

Preflight [원문](_raw/21/preflight-20261026-001034-506-6ac6d75a/preflight.txt):

```text
MEASURE unityExitCode=198
FAIL Unity startup exit=198; Unity tasks BLOCKED(external dependency); do not retry unchanged environment; non-Unity tasks may continue
```

Unity Quiet/기본 회귀·Scene clearance·실제 exe 잠금/방향/저장 검증은 미실시. 게임 C#·에셋 변경 없음. `--max-step-evidence`는 지정하지 않았으므로 trench-escape MEASURE는 미실시다. 이전 B20 측정을 이번 실측으로 대체하지 않는다.

Ruling: self-test에 `--self-test-output-dir`를 추가해 UUID별 원본을 이번 Batch의 `_raw/21` 아래 보존 / 과거 `_raw/B16b`·B17·B18에 새 로그를 섞지 않기 위함 / 기본 검사·게임 동작 영향 없음.
