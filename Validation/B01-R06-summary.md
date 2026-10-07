# B01 Backlog 1 — S3T01-R06 입력 교체 / DONE

- 기존 RouteRunner 생성기를 수정 없이 재사용. 서로 다른 접근1개, 시도1회. 새 검증 도구0, 경로 검사 코드 수정0, 게임 배치·씬 수정0.
- 기존 R01~R05 입력을 실제 새 게임부터 연속 재생한 뒤 R06 기록. 시작 위치(32.471199,1.814780)는 FromRight(32.5,1.81)의 실제 물리 안정화 위치다. 이전 R06(15.971199,1.814780)의 CP 시작 가정을 교체했다.

```text
PASS recording prefix R05
R06 attempt=1 ticks=373 startTeleport=False PASS
PASS continuous recording R06
Training complete; per-segment outcomes are in library. Training success does not mean all-route acceptance.
Error/Exception/Assert=0
```

첫 두 PASS 원문의 줄 끝에 공백이 있다. 상세 원본은 _raw에 그대로 보존했다.

- 기존 라이브러리에서 R06 객체만 교체한 새 후보로 RouteAll을 별도 실행했다. R05·R06 사망0, A03→B01 출구를 통해 G-D 통과, 다음 R07 및 R01~R17 전체 엔딩→타이틀 PASS / Error/Exception/Assert=0. 원본 CSV의 전체 death 행0도 확인했다. 입력은 텔레포트·능력/HP 지급 없이 연속 재생했다.

```text
PASS R05 fixed-tick replay
MEASURE continuous start deviation R06=0.000u; no placement performed
PASS R06 fixed-tick replay
PASS R07 fixed-tick replay
PASS all ends at title
Error/Exception/Assert=0
```

위 PASS replay 원문의 줄 끝 공백은 _raw에서 그대로 보존했다.

- 최종 입력: Validation/N8-route-final-library.json. R06 외16개 객체의 JSON 의미 동일성 확인, 원문 치환으로 해당16개 텍스트도 그대로 유지했다. 기존 시작점·입력은 Git START_COMMIT 및 _raw 후보에 보존했다. R06 승인 항목을 Validation 기존 파일 수정 금지의 명시 예외로 적용했다.
- 실제 실행 바이너리: Builds/B01-hitstop/Afterglow-B01-hitstop.exe. 추가 빌드는 만들지 않았다. 사람이 플레이하는 난이도·완주 시간·Sprint Gate 수락은 별도다.
- 원본: Validation/_raw/B01/R06-a1.txt, R06-a1-R06-a1.txt, R06-a1-library.json, R06-proposed-library.json, R06-all.txt/log/save.json, 20261007-220854.csv. 자동 활동 시간을 사람의 플레이 시간으로 해석하지 않는다.
