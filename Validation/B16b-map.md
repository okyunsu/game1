# B16b BL1 — V5 지도 문 표시

상태 REVIEW. 실제 exe PASS, Quiet13/13. 컴파일0·Development 빌드 오류0/경고0. 원본 world 데이터와 생성된 지도6방·12문의 ID/stub 대응을 Python으로 확인했다. V4 지도JSON·Scene·Resources 변경0.

실제 M 입력으로 Large, map-all에서6방/12문을 확인했다. 아래 라벨은 OnGUI가 사용하는 동일 표시 함수의 런타임 측정값이다. stub은 능력을 보유해도 닫힌 색+×, b 방이 없으면 ?를 그리지 않는다. A06→A02는 오른쪽 화살표. 화면 캡처는 미실시, 사람의 가독성 판단은 직접 플레이 대기.

| 문 | a→b | 종류 | 표시 |
|---|---|---|---|
| A01-A02 | A01→A02 | both | 선 |
| A01-A06 | A01→A06 | both/DJ | J |
| A01-Z01 | A01→Z01 | stub/Slam | S × |
| A02-A03 | A02→A03 | both | 선 |
| A02-A03U | A02→A03 | both | 선 |
| A02-B05 | A02→B05 | stub/WallJump | W × |
| A02-A06 | A06→A02 | oneway | → |
| A03-A04 | A03→A04 | both | 선 |
| A05-A03 | A05→A03 | shortcut | 점선 |
| A03-B01 | A03→B01 | stub/Dash | D × |
| A04-A05 | A04→A05 | down | ↓ |
| A05-Z01 | A05→Z01 | stub/Slam | S × |

동일 exe의 기존 B15MapVerifier도 PASS: V4 21방·비승강기31문·M3상태·입력맵/시간 유지·방 이동/위치 갱신, 추가10초 Error/Exception/Assert=0. V5도 추가10초 Error0. 원본 `_raw/B16b/BL1-v5-map.txt/log`, `BL1-v4-map.txt/log`, `BL1-build.txt/log`, `BL1-map-generation.log`, `BL1-compile.log`.
최종 Quiet B16b-BL1 REGRESSION13/13. 기술 완료, 지도 가독성은 V5 직접 플레이 REVIEW에 포함.
