# B05-A CC0 그래픽 도입 — 기술 구현 완료 / 스타일 REVIEW

Kenney Pixel Platformer 1.2, 공식 https://kenney.nl/assets/pixel-platformer 에서 CC0·18×18 타일 확인(2026-10-08). 캐릭터 원본은 24px 계열, 규격 16~32px 안. 원본 ZIP은 `_raw/B05/kenney_pixel-platformer.zip`, 원본 배포 파일·License.txt는 `Assets/Art/External/KenneyPixelPlatformer/`에 보존. Point·PPU18·무압축·mipmap off. 원본 이미지 수정 없음.

## 실제 검증

- Tilemap **1회** 저장·재개방 usedTiles=1 PASS (`_raw/B05/tilemap.txt`). V2 바닥은 Tilemap 시각 + 기존 프리팹 Collider. 원본 ROOM-001 이력은 보존하며 이번 최소 재현 성공을 과거 원인 해결로 해석하지 않음.
- Landing/Wall/Ceiling/Door/KillZone/PogoTarget/Player/E1/E2/E3 **Prefab Variant**, BoxCollider size/offset/trigger/local transform diff0 (`skin-collider-check.txt`). 원본 프리팹 변경0.
- 보스 원본은 Prefab이 아닌 C06 Scene 오브젝트. C06을 저장하지 않고 동일 body/tuning을 새 `BossBase_V2.prefab`으로 추출해 `Boss_V2.prefab` Variant 생성. Scene별 leftExit는 후보 Scene에서 바인딩해야 함.
- V2 시각 자식을 원본 MovementTest의 메모리상 인스턴스에 붙인 **S1-02 PASS / S1-04 PASS**, 보호된 테스트 Scene 저장0. raw `skin-S1-02-editor.log`·`skin-S1-04-editor.log`, 결과는 `_raw/B05/skin-reg/`로 보존.
- 실제 exe `SkinTest_V2`에서 Player 스킨 표시, 가상 X 공격으로 E1 **기존 HP3 타수 처치**, 피격 1HP, E2/E3 기존 tuning HP 및 ReceiveHit 후 제거 PASS/Error0 (`skin-runtime-a1.txt`). E1 공격 fixture는 순찰을 멈춰 처치 타수를 독립 확인했으며 자연 전투 수락을 대신하지 않음.
- 실제 V2 A01 방 카메라 **1920×1080 PNG** 촬영·육안 확인 (`A01-shot-final.png`). OnGUI HUD를 제외한 오프스크린 방 카메라 사진. 스킨 테스트 Scene도 별도로 보존.

## 실패·미실시

첫 Unity 샌드박스 실행은 라이선스 접근 실패 후 권한 실행으로 해소. 최초 캐릭터 Sprite 이름을 `character0`으로 예상해 아래 오류 발생, 실제 importer 이름 `_0` 참조로 1회 보정:

```
InvalidOperationException: Sequence contains no matching element
```

사용자 스타일·가독성·체감 수락, 물리 게임패드 조작 미실시. A 배경은 2층 및 파이프 랜드마크 적용. B/C 지역 실제 적용은 승인된 해당 지역 V2 Task에서 이어간다. 기존 AV1·전투/이동 기본 수치 변경0.
