# B05-A3 문 인스턴스 스킨 — REVIEW

18개 V2 방과 SkinTest의 문에 CC0 Door_V2 시각 자식을 적용했다. 기존 RoomExit의 렌더러가 루트가 아닌 자식에 있어 첫 스킨 적용에서 누락된 것을 촬영으로 확인했다. 문 ID·Spawn·원본 트리거·콜라이더·수치는 유지했다. C06의 원본 잔여 경고 그림은 V2 Scene에서만 제거하고 보스 자식 경고는 유지했다.

실제 검사: `_raw/B05/door-instance-colliders.txt`의 19/19 collider diff0 PASS. 최종 V2-delivery 빌드 Succeeded errors=0, 실제 exe `skin-final.txt`의 원본 HP 공격 횟수·피격1HP·E2/E3 처치 PASS/Error0. 전체 문 연결은 같은 배포 exe의 `world-final.txt` 연속23구간 PASS, CP-C04 왕복·Continue는 `C04-save-final.txt` PASS/Error0.

1920×1080 실제 Camera PNG를 직접 확인했다: [A01](_raw/B05/A01-shot-delivery.png), [C06](_raw/B05/C06-shot-delivery-v2.png), [스킨 테스트](_raw/B05/SkinTest-shot-delivery.png). 촬영은 Camera 렌더이며 OnGUI HUD는 포함하지 않는다. 픽셀 캐릭터·바닥·배경·문·보스가 보이고 C06의 떠 있던 잔여 경고가 제거됨을 확인했다.

핵심 원문:
```
PASS C06 Door Variant visual children; collider diff0
PASS SkinTest Door Variant visual children; collider diff0
PASS real virtual attacks kill E1 in original HP number of hits
PASS hurt exactly 1 HP
Error/Exception/Assert=0
```

미실시: 사람 스타일·가독성·손맛 수락. 원본 프리팹 수정0. Tilemap 재시도0. 원본 로그·PNG 삭제0.


