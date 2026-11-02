# Resource Register

`리소스 / 출처 URL 또는 직접 제작 / 라이선스·확인일 / 사용 위치 / 표기 의무 / 실제 표기 위치`

| 리소스 | 출처 | 라이선스·확인일 | 사용 위치 | 표기 의무 | 실제 표기 위치 |
|---|---|---|---|---|---|
| Block.png | 직접 제작 2×2 흰색 도형 | 외부 라이선스 없음 / 2026-09-17 | 기존 Scene·Prefab | 없음 | 이 문서 |
| Original art v1 (타일16·배경2 PNG) | 직접 제작(Claude 생성 스크립트 `Design/art_v1/make_art.py`), 외부 다운로드 없음 | 외부 라이선스 없음 / 2026-11-02 | 원본 `Design/art_v1/`; 적용 `Assets/Art/Original/v1/` (2026-11-03 Unity import PASS) | 없음 | 이 문서 |
| Kenney Pixel Platformer 1.2 (18×18px) | https://kenney.nl/assets/pixel-platformer | CC0, 공식 페이지 확인 2026-10-08 | Assets/Art/External/KenneyPixelPlatformer 원본 / V2 지형·배경·인물·적·보스 Variant | 없음 | 이 문서·동봉 License |

Batch04 AV1은 기존 직접 제작 후보이며 이번에 수정하지 않는다. 원본 파일·라이선스는 외부 에셋 폴더에 그대로 보존한다.

B25 import 적용(2026-11-03): Sprite(2D and UI), 16 PPU, Point, Uncompressed, FullRect, mipmap 꺼짐. PNG18개 원본/Assets SHA256 일치, Unity 생성 `.meta`와 함께 커밋. 설정 원문은 `Validation/_raw/25/T13-import/result.txt`, 바이트 대조는 `T13-hashes.txt`. 이전 차단 때의 raw 스테이징도 보존했다. 생성 스크립트 재실행·외부 다운로드 없음. 공유 표시 설정은 `Assets/Art/Original/v1/ArtSettings.asset`(캐릭터 Sprite·배경 패럴랙스)이다.

B25 효과음은 기존 직접 제작 `Assets/Audio/AV1/*.wav`(B04 합성 원본)를 재사용한다. 외부 다운로드·새 라이선스 없음. `Assets/Audio/B25/Feedback.asset`에 이벤트별 클립·선형 볼륨·피치 변화, 입자 수명/속도/크기를 둔다. 게임 난수 상태를 소비하지 않는 결정적 피치 변화를 사용한다.
