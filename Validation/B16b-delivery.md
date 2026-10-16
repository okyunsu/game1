# B16b T5 — V5 전달

상태: 기술 검증 PASS, Task Quiet13/13. 사용자 플레이 수락은 REVIEW.

`Builds/B12-delivery/Afterglow.exe`를 Development로 갱신했다(오류0·경고0). 이전 `Builds/B13-delivery`는 보존했다. V4 런처 12개의 내용·경로·저장 슬롯은 바꾸지 않았다.

| 런처 | 시작 | 능력 | 저장 슬롯 |
|---|---|---|---|
| `Launchers/current/Play-V5.cmd` | A01 start | 없음 | B12-delivery/slots/V5-NewGame.json |
| `Launchers/current/Play-V5-AfterDash.cmd` | A03 Entry | Dash | B12-delivery/slots/V5-AfterDash.json |
| `Launchers/current/Play-V5-DJ.cmd` | A01 start | DJ | B12-delivery/slots/V5-DJ.json |

세 런처 모두 `-NoMap` 지원. 전용 fixture는 기존 ProgressData를 쓰고 새 저장 필드를 추가하지 않았다. 실행 시 해당 V5 슬롯을 fixture로 초기화하므로 일반 V4 저장과 분리된다. A03 fixture는 기존 CP-A03, 나머지 공식 시작은 CP-A01을 사용한다.

실제 exe 시작 검사: 기존12 + V5 3 + V5 NoMap3 =18/18 PASS. 각10초 Error/Exception/Assert=0, NoMap 지도0. 원본 `_raw/B16b/T5-launchers-result.txt`, 각 `T5-*.txt/log`. `-Smoke`는 별도 슬롯을 만들고 실제 입력·검증기를 실행했다. .cmd 인자를 읽어 동일 Start 스크립트로 기동했다.

Release 빌드 Succeeded errors=0 warnings=0. 빌드 Scene 목록에 V5 없음, 빌드 지도 로그 `MAP build development=False layouts=world_v7`. Assembly-CSharp.dll에서 V4WorldMap/V4MiniMap/V5Launch/V5PatrolBinding/B16V5Verifier/N5Runner 6타입 모두 present=False. 원본 `_raw/B16b/T5-release.txt/log`. 검증용 Release는 절대 경로가 Builds 내부인지 확인한 뒤 삭제했고 최신B12·직전B13만 남겼다.


최종 Task Quiet: B16b-T5 REGRESSION 13/13. 기술 검증 완료, 직접 플레이 수락 대기 REVIEW.
최종 코드/빌드 재확인: Final-launchers-result.txt 18/18 PASS·각10초Error0, Final-v5.txt 문14방향/저장PASS, Final-release.txt/log V5제외PASS. 최종 전달파일은 BL2-r1-build 결과다.
