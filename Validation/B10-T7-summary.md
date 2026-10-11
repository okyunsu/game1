# B10 T7 — 실제 보스 승리와 S01

상태: DONE. V4 C06, Release B10-release-r15, 실제 키 입력. 시작 준비는 C06 경로 시작 x3.5, 대시·벽 점프·DJ만 소유, Slam/Boss 승리 미소유. 준비 이후 위치 배치·능력 부여·보스 우회 없음.

패턴 Tell을 관찰해 점프와 공격 간격을 탐색했다. 성공한 입력을 파일로 저장하고 다시 읽어 **보스전 → Slam → 열린 천장 → S01**을 2회 연속 재생했다. 천장 이동은 T1 탐색에서 얻은 C06/exit 4구간 입력 사용. 보스 승리 후 실제 착지를 기다리는 중립 입력 포함.

원본: `_raw/B10/T7-boss-r15.txt`, `.live.txt`, `.log`. 승리 입력은 해당 원본의 마지막 PASS `boss saved replay` 행에 적힌 `inputs/C06_boss-2319447698.json`; 출구는 `C06_exit_1..4` 및 마지막 검증된 `-r` 후보. 원본과 이전 실패 입력 보존.

```text
PASS boss saved replay repetition=2 input=Validation/_raw/B10/inputs\C06_boss-2319447698.json
PASS boss -> Slam -> opened roof -> S01 saved replay repetition=2
PASS boss victory -> Slam -> roof opened replay=2
PASS boss ceiling traversal S01 replay=2 actual=S01
MEASURE B10 mode=boss PASS=1 BLOCKED-GEOMETRY=0 budget-deferred=0
```

초기 실패 원문:

```text
FAIL boss exit segment=2 pos=(7.64, 4.37)
FAIL boss -> Slam -> opened roof -> S01 saved replay repetition=1
```

승리 직후 남은 점프/착지 상태에서 출구 입력을 시작하면 일부 구간이 달라졌다. 실제 착지 대기와 전체 연결 재생으로 성공 후보를 구분했다. 보스 HP·패턴·전투 수치·Scene 변경0. 기본/V2 보스 수정0.

미실시: 자연 T6 경로에서의 이 보스 입력 연결, 30/120fps, 플레이 감각 수락. 이 승리는 T6 전체 완주를 의미하지 않는다.
