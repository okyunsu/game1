# B04-B-R — 후보 사망 복귀 표현 연결

상태 DONE. 코드 검토에서 RoomSession.Respawn이 기본 SpriteRenderer.enabled를 다시 true로 설정하는 것을 확인했다. AV1 PresentationVisual만 LateUpdate에서 원본 renderer를 끄고, HP0/Respawning 동안 자식 visual을 숨겨 복귀 후 후보 몸체만 표시한다. 보호 코드·충돌체·이동 쓰기0.

B04-AV1-r2 실제 exe 전체 이벤트 fixture PASS, `PASS respawn keeps candidate visual only`, audio10/effect 모두 기록, Pause0/중복 억제/콜라이더 diff0, Error/Exception/Assert=0. 새 빌드 Succeeded errors=0 warnings=0. 원본 Validation/_raw/B04/av1-r2-events.txt/.log 및 B04-AV1-r2-build.txt. 자연 플레이 감각은 기존 AV1 REVIEW에 남긴다.
