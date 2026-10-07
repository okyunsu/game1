# B02-A / BOSS-2 — BLOCKED (설계값 결정 대기)

첨부의 새 패턴·후보 전용·기본값 불변은 승인 범위다. 다만 패턴 사이 걷기 속도·좌우 중심 위치와 도약 궤적의 높이 값/탐색 범위는 지정되지 않았다. AGENTS Batch Approval의 표에 없는 설계 결정은 해당 경로만 중단 규칙 적용.
현재 C06: 보스 중심 x17.5/y2.5, 크기2x3u, 내부벽 x1/19. 기본 BossTuning recovery=0.8s이므로 돌진 회복0.8과 공통 recovery 유지 문구는 현재 값에서 일치한다.
질문: 후보 초기값 걷기2.5u/s, 좌우 중심x5/17.5u, 도약 높이4u 허용 / 이동 없는 대체+높이4u / 직접 지정.
사용자 답변 전 BossTuning/BossEncounter/BossWave·기본 Asset 수정 없음. 후보 Asset/후보 빌드·새 패턴30/60/120fps·Pause/사망·엔딩 검증·비교 측정 미실시.
기존 BossEncounter.DeferDeath, CP-C04 복귀와 기존 2패턴 흐름은 보존. 독립 B/C 및 Backlog 계속.
