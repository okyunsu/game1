# Resource Register

`리소스 / 출처 URL 또는 직접 제작 / 라이선스·확인일 / 사용 위치 / 표기 의무 / 실제 표기 위치`

| 리소스 | 출처 | 라이선스·확인일 | 사용 위치 | 표기 의무 | 실제 표기 위치 |
|---|---|---|---|---|---|
| Block.png | 직접 제작 2×2 흰색 도형 | 외부 라이선스 없음 / 2026-09-17 | 기존 Scene·Prefab | 없음 | 이 문서 |
| Original art v1 (타일16·배경2 PNG) | 직접 제작(Claude 생성 스크립트 `Design/art_v1/make_art.py`), 외부 다운로드 없음 | 외부 라이선스 없음 / 2026-11-02 | 원본 `Design/art_v1/`; 예정 `Assets/Art/Original/v1/` (B25 Unity import 차단, 아직 미적용) | 없음 | 이 문서 |
| Kenney Pixel Platformer 1.2 (18×18px) | https://kenney.nl/assets/pixel-platformer | CC0, 공식 페이지 확인 2026-10-08 | Assets/Art/External/KenneyPixelPlatformer 원본 / V2 지형·배경·인물·적·보스 Variant | 없음 | 이 문서·동봉 License |

Batch04 AV1은 기존 직접 제작 후보이며 이번에 수정하지 않는다. 원본 파일·라이선스는 외부 에셋 폴더에 그대로 보존한다.

B25 import 예정 설정: Sprite(2D and UI), 16 PPU, Point 필터, TextureImporterCompression.Uncompressed. Unity exit198로 import 설정은 **미적용·미검증**이다. 18 PNG의 해시 일치 복사본과 설정 manifest는 `Validation/_raw/25/art-import/`·`art-import-manifest.json`에 보존했다. Unity가 만든 `.meta`와 함께 커밋해야 하므로 Unity가 없는 이번 실행에서는 Assets에 미완성 에셋을 추가하지 않았다. 재개 시 원본을 `Assets/Art/Original/v1/`로 복사하고 Unity Editor API로 import 설정·`.meta` 생성 후 함께 커밋한다. 원본 스크립트는 재실행하지 않았다.

