# B17 BL2 — V5 draft1 / draft2 비교

계산만 수행. draft1은 Batch16b 최종 `4edc334`의 JSON, draft2는 Base `2559c40`의 사용자 입력과 동일한 현재 readonly JSON이다. 층 A 6방만 비교한다. 블록=solid 배열 수(바닥·천장·벽 포함), 발판=platform, 등불은 pogo의 lantern만 세며 가시 포고3개는 별도다. 머리 여유는 T4와 같은 노출된 solid/platform 윗면→겹치는 아랫면 계산이며 맞닿은 내부 면은 제외한다.

|방|크기 draft1→draft2|면적1→2 u²|적1→2|등불1→2|블록1→2|발판1→2|머리 여유 최소1→2 u|
|---|---|---:|---:|---:|---:|---:|---:|
|A01|48×16→48×16|768→768|2→2|2→2|6→9|4→3|1.4→3|
|A02|28×36→28×36|1008→1008|2→2|1→1|12→16|5→3|1→2.9|
|A03|50×20→50×20|1000→1000|4→4|5→6|7→13|8→5|1.4→3|
|A04|50×12→50×12|600→600|3→3|5→5|7→11|3→2|1.4→3|
|A05|50×10→50×12|500→600|1→1|0→0|11→11|0→0|3→3|
|A06|48×14→48×14|672→672|1→3|0→0|8→11|6→5|1.4→2.4|
|합계/최소|6방→6방|4548→4648|13→15|13→14|51→71|26→18|1→2.4|

PASS row sums == independently recomputed JSON totals. 면적 +100u²는 A05 높이10→12의50×2, 적 +2는 A06의1→3이다. 바닥 구덩이6개는 draft2 추가. 가시 포고는 두 버전 모두3개다. 등불은13→14개다(방별 표의 합계).

주의: readonly WORLD_V5 4절의 총면적4,548과 A05 50×10 표기는 draft1이며, 9절·JSON draft2는 A05 50×12/합계4,648이다. Batch 사전 결정에 따라 draft2 JSON을 생성 원본으로 썼다. Design 표기 정리는 Claude에게 돌린다(에이전트 수정0).

SHA256 draft1: `d5653a66cec9873bacd36757651d776e8cc23b73070fb7e176e5c92e04555473`
SHA256 draft2: `9c5306c73ecab345ed4ad10b12ddb3671076ac7c9b792d47f7ddc941d8c3d4a2`

원본 보존: `_raw/B17/draft1-rooms.json`. 실제 점프·도달성은 이 계산의 판정 대상이 아니다. 합격 기준 합계 일치 충족, 상태 DONE. Quiet 결과는 별도 원문으로 덧붙인다.

Task 종료 Quiet: Editor 라이선스 exit198로 검사 실행 전 중단. 문서 계산의 합계 검산은 PASS이며 게임 실행 결과와 구분한다.

```text
S1-02 FAIL | S1-02 FAIL exit=198 evidence=S1-02-night-S2-B17-BL2.txt mode=headless
S1-04 FAIL | S1-04 FAIL exit=198 evidence=S1-04-night-S2-B17-BL2.txt mode=headless
S1-07 FAIL | S1-07 FAIL exit=198 evidence=S1-07-night-S2-B17-BL2.txt mode=headless
S1-08 FAIL | S1-08 FAIL exit=198 evidence=S1-08-night-S2-B17-BL2.txt mode=headless
Camera FAIL | Camera FAIL exit=198 evidence=S1-06-lead0-night-S2-B17-BL2.txt mode=graphics-640x360
T1 FAIL | T1 FAIL exit=198 evidence=Verification-night-T1-B17-BL2.txt mode=headless
T2 FAIL | T2 FAIL exit=198 evidence=Verification-night-T2-B17-BL2.txt mode=headless
T3 FAIL | T3 FAIL exit=198 evidence=Verification-night-T3-B17-BL2.txt mode=headless
T4 FAIL | T4 FAIL exit=198 evidence=Verification-T4-night-S2-B17-BL2.txt mode=headless
S2 FAIL | S2 FAIL exit=198 evidence=S2-01-integration-night-S2-B17-BL2.txt mode=headless
F2R FAIL | F2R FAIL exit=198 evidence=F2R-feedback-night-S2-B17-BL2.txt mode=headless
LOGR FAIL | LOGR FAIL exit=198 evidence=LOGR-playtest-night-S2-B17-BL2.txt mode=headless
Boss FAIL | Boss FAIL exit=198 evidence=boss-recovery-night-S2-B17-BL2.txt mode=headless
REGRESSION 0/13
```
