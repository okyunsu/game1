# B04 Backlog 2 — AV1 luminance contrast

상태 REVIEW. 기본 Scene/Prefab/색 변경0. Profile의 bodyColors 및 hazardOpacity만 후보색/불투명도 원본이며, runtime bootstrap은 해당 opacity를 KillZone의 시각 SpriteRenderer에만 적용한다(physics 변경0).

RGB를 sRGB 전송함수로 선형화한 상대 명도와 (Lmax+.05)/(Lmin+.05)를 계산했다. 텍스트 접근성 수락을 뜻하지 않는다. 배경은 A01/B01/C01 Camera RGB, 반투명 Level KillZone은 background와 alpha 합성한 근사다. 지역 타일·실제 조명·피격 flash는 표에 포함하지 않는다. 후보 목표는 body/배경 및 player/body 3 이상, 위험 영역은 기존 alpha .45의 낮은 대비를 개선. Enemy/hazard끼리의 모든 조합에 동일 명도 차이를 강제하지 않으며 형태/배치도 함께 확인한다.

| Pair | Before ratio | AV1 ratio |
|---|---:|---:|
| Player/A01 background | 17.39 | 17.39 |
| Player/B01 background | 17.39 | 17.39 |
| Player/C01 background | 17.39 | 17.39 |
| E1/A01 background | 10.85 | 5.06 |
| E1/B01 background | 10.85 | 5.06 |
| E1/C01 background | 10.85 | 5.06 |
| E2/A01 background | 6.85 | 5.82 |
| E2/B01 background | 6.85 | 5.82 |
| E2/C01 background | 6.85 | 5.82 |
| E3/A01 background | 9.03 | 5.72 |
| E3/B01 background | 9.03 | 5.72 |
| E3/C01 background | 9.03 | 5.72 |
| Boss/A01 background | 7.55 | 5.14 |
| Boss/B01 background | 7.55 | 5.14 |
| Boss/C01 background | 7.55 | 5.14 |
| Player/E1 | 1.60 | 3.44 |
| Player/E2 | 2.54 | 2.99 |
| Player/E3 | 1.93 | 3.04 |
| Player/Boss | 2.30 | 3.38 |
| Level hazard/A01 background (alpha .45->1) | 1.80 | 4.97 |
| Level hazard/B01 background (alpha .45->1) | 1.80 | 4.97 |
| Level hazard/C01 background (alpha .45->1) | 1.80 | 4.97 |

후보 body RGB: Player(.90,.98,1) 유지, E1(.78,.43,.10), E2(.94,.38,.30), E3(.62,.48,.90), Boss(.90,.28,.62). player와 구별하기 위해 적 몸체 명도를 낮췄고 기존 RGB 예고 변화는 유지한다. hazardOpacity=1로 영역 내부가 배경에 묻히지 않게 한다. 위험물 RGB·collider·기본색은 보존. 큰 색 면적과 형태의 실제 가독성은 직접 플레이 수락 대기. 원본 profile과 계산 JSON은 Validation/_raw/B04/에 보존.

실제 B04-AV1-delivery exe 이벤트/형태/피격·사망복귀/Pause/콜라이더 검사 PASS, Error/Exception/Assert=0. 원본 av1-delivery-events.txt/.log와 build-av1-delivery.log 보존. 위의 3은 이 후보에서 큰 기하형을 비교하기 위한 약3배 목표이며, Player/E2 실제값2.99를 임의로 3 이상 PASS로 표기하지 않는다.
