# B04 Backlog 3 — 지정9방 사진

상태 DONE. AV1-final-r4 실제 exe의 A01/A04/A05/B02/B03/B04/C02/C03/C06을 1920×1080으로 촬영했다. PNG9개 모두 크기/비검정 검사 PASS, contact sheet 육안 검토 완료, Error/Exception/Assert=0.

과거 Q7/N7 hidden swapchain EOF 검정 실패를 검토하고 실제 Scene Camera를 임시 RenderTexture로 렌더링했다. 원래 target/active texture를 복원하며 opt-in 촬영 때만 실행한다. 방·배치·배우 시각 사진으로, OnGUI HUD/화면 overlay는 포함하지 않는다. Entry fixture의 카메라 시야이며 자연 완주 또는 모든 방 영역의 사진으로 표기하지 않는다. 이번 검정 사진0/검정 보정0회.

첫9장도 유효했으나 E2 표식이 진행 방향을 따르지 않는 것을 육안 검토로 확인했다. E2 Direction·E3 direction·Player Facing에 맞춰 후보 자식의 x scale 부호만 바꾸고 최종9장을 다시 저장. 기본 root/collider diff0. 최종 exe audio/effect/사망복귀/Pause PASS, 30/60/120 위치·속도 hash 기존 기본3개와 동일. 최초9장과 contact/JSON도 raw에 보존.

| Scene | PNG | Pixels | Max RGB variance |
|---|---|---|---:|
| A01 | [A01](_raw/B04/shots-camera2/A01.png) | 1920×1080 | 2252.537 |
| A04 | [A04](_raw/B04/shots-camera2/A04.png) | 1920×1080 | 3021.824 |
| A05 | [A05](_raw/B04/shots-camera2/A05.png) | 1920×1080 | 2162.873 |
| B02 | [B02](_raw/B04/shots-camera2/B02.png) | 1920×1080 | 2430.434 |
| B03 | [B03](_raw/B04/shots-camera2/B03.png) | 1920×1080 | 2738.802 |
| B04 | [B04](_raw/B04/shots-camera2/B04.png) | 1920×1080 | 2459.94 |
| C02 | [C02](_raw/B04/shots-camera2/C02.png) | 1920×1080 | 2522.252 |
| C03 | [C03](_raw/B04/shots-camera2/C03.png) | 1920×1080 | 3203.235 |
| C06 | [C06](_raw/B04/shots-camera2/C06.png) | 1920×1080 | 2553.905 |

코드: B04Capture.cs(새 opt-in 도구), PresentationVisual.cs(방향 보정). PNG/JSON/contact sheet는 Validation/_raw/B04/에서 미커밋. 원문 출력 shots-camera2.txt/.log, 최종 이동 motion-av1-final-{30,60,120}.txt/.csv/.log. 이미지에 AI 생성/외부 리소스 사용0.
