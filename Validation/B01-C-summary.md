# B01-C — 확인용 Windows 빌드 / DONE

- 기존 N9Build.Run, candidate 미지정(원본 Hard), tag B01-hitstop, Windows x64 빌드1개. 원본 씬·프리팹·Build Settings·Packages·ProjectSettings 변경 없음.
- 빌드 원문:

```text
Succeeded errors=0 warnings=0
18 scenes incl opt-in PogoTest; EditorBuildSettings unchanged. Candidate=main Hard; source tuning unchanged by candidate build.
```

- 배포 경로: Builds/B01-hitstop/Afterglow-B01-hitstop.exe. 같은 폴더 Data/DLL을 함께 보존. 이 빌드는 Git 제외이며 checkout만으로 exe가 생기지 않는다.
- 실제 exe를 기존 BossRetryVerifier로 실행했다. 격리 Validation/_raw/B01/C-save.json에서 CP-C04 Continue, 실제 이동→보스 접촉 사망/복귀3회, 입력 공격으로 처치→저장 엔딩 PASS / Error/Exception/Assert=0. 일반 사용자 슬롯은 사용하지 않았다.

```text
MEASURE final boss fight seconds=21.719
PASS three retries then actual input victory reaches saved ending
Error/Exception/Assert=0
```

- 실제 exe에 HitStop BEGIN과 MEASURE HitStop 로그 다수 확인. 마지막 명중 때 Completing 상태가 정지를 넘겨받아 reason=game state, scale=0.0으로 취소한 로그도 확인했다. Pause/사망과 정지가 겹친 검증을 대신하지 않는다.

```text
MEASURE HitStop elapsed=0.066631s reason=elapsed scale=1.0 fps=60
MEASURE HitStop elapsed=0.066564s reason=elapsed scale=1.0 fps=60
MEASURE HitStop elapsed=0.007665s reason=game state scale=0.0 fps=60
```

- 직접 QA: 일반 exe 실행 후 A04 전투·C06 보스에서 정지감과 기존 F2R 베기·번쩍임·HP 칸 확인(약5분). 사람 완주·실물 패드·동시 Pause/사망은 미실시, A는 PARTIAL 유지. 전체 RouteAll 결과는 별도 R06 요약을 따른다.
- 원본: Validation/_raw/B01/C-build.log, N9-B01-hitstop-build.txt, N9-B01-hitstop-scene-map.txt, C-exe.log, C-exe.txt. 이 요약1개만 커밋했다. 빌드 자체는 최종 확인 산출물이므로 Builds 바로 아래에 보존하고 검사 전용 추가 빌드는 만들지 않았다.
