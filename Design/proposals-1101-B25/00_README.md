# 11/01 — Batch 25: 층 B 6방 · 그래픽 1차 · 수정 · 다듬기 Queue

사용자 결정(2026-11-01): 층 B 6방으로 진행, 그래픽 적용도 같이. 진행 시간이 너무 짧았다(B24 약 1시간 40분) → 밤샘 2회 분량 + Ready Queue로 길게.

## 이번 묶음 (Claude 설계)
- **draft6** `Design/v5/rooms_v5_AB.json`·`world_v5_AB.json`(A+B 통합, 이후 원본), 그림 `Design/v5/floorB_v5.png`, 설명 WORLD_V5 18절.
- 층 B: B01 시장 허브 / B02 압연 공장 / B03 배관 수직갱 / B04 보일러실(보스2) / B05 숨은 승강로 / B06 증기실(선택).
- A03 → B01 연결 정정(선반 끝 천장 구멍), A02 조각 선반 수정(B24 결정 대기).
- 정적 검사 12방 FAIL 0(`Validation/D60-room-rules-v5-AB.md`).
- 그래픽 계획 `02_GRAPHICS.md`, Codex 지시 `01_CODEX_BATCH25.md`.

## 사용자 준비 (Codex 전에)
1. **그래픽은 직접 제작(사용자 2026-11-02 결정):** 외부 에셋 대신 `Design/art_v1/`(Claude가 만든 원본 타일·배경, 생성 스크립트 포함). 목업 `Design/art_v1/mockups/`를 먼저 보고 바꿀 점이 있으면 Codex 전에 말하기.
2. **push(권장):** 로컬에 B23·B24 커밋 33개가 원격에 없다. `git push origin main --tags`.

## 검수 (Batch 끝, 1회 약 30~40분)
1. Play-V5-Loop: A03 → B1 → 대시 → A03 연습 → 선반 → B01 → B02 → B03 → B04 보스2 → (벽 점프) B06·B05·A02 재방문
2. Play-V5-Graybox와 비교해 그래픽이 읽히는지
기록: 전체 시간·보스 시도 횟수·막힌 곳·머리 박은 곳·그래픽이 헷갈린 곳.
