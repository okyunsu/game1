# B06b BL2 — 입력 장치별 테스트 안내

DONE. WallJumpTest 안내가 PlayerInputReader의 마지막 게임 입력 장치에 따라 키보드(Z/C)·패드(A/B)로 바뀐다. 기존 Input Actions/PlayerInputReader 변경0.

Unity Editor 및 최종 Development exe에서 가상 Input System 키보드→패드→키보드 두 번 전환 PASS/Error0.
`MEASURE device guide switches=2 keyboard/pad/keyboard actual Input System virtual devices`

Backlog 반영 후 최종 Builds/B06b-delivery 재빌드 errors0/warnings0. 실행기4개 최종기동 PASS/Error0, 실제 exe device 전환·측정 HUD PASS. 초기 전달 빌드 증거는 T5-initial-delivery-build.txt로 보존했고 최종 빌드 증거도 남겼다.
[Editor](../../Validation/_raw/B06b/BL2-device-a1.txt), [exe](../../Validation/_raw/B06b/BL2-final-device.txt), [최종 빌드](../../Validation/_raw/B06b/B06b-delivery-build.txt), BL2-final-{guide,noguide,wall,ending,metrics}.txt 같은 폴더. 미실시: 패드 실물 연결/손맛·사람 가독성.
