<!-- B17 verification start -->
**종료 기본 회귀 BLOCKED — 아래 exit198/0/13은 Editor 라이선스로 검사 미실행. 이전 T1~T7 Quiet는 각13/13.**

```text
S1-02 FAIL exit=198 evidence=S1-02-night-S2-B17-final-default.txt
S1-04 FAIL exit=198 evidence=S1-04-night-S2-B17-final-default.txt
S1-07 FAIL exit=198 evidence=S1-07-night-S2-B17-final-default.txt
S1-08 FAIL exit=198 evidence=S1-08-night-S2-B17-final-default.txt
Camera FAIL exit=198 evidence=S1-06-lead0-night-S2-B17-final-default.txt
T1 FAIL exit=198 evidence=Verification-night-T1-B17-final-default.txt
T2 FAIL exit=198 evidence=Verification-night-T2-B17-final-default.txt
T3 FAIL exit=198 evidence=Verification-night-T3-B17-final-default.txt
T4 FAIL exit=198 evidence=Verification-T4-night-S2-B17-final-default.txt
S2 FAIL exit=198 evidence=S2-01-integration-night-S2-B17-final-default.txt
F2R FAIL exit=198 evidence=F2R-feedback-night-S2-B17-final-default.txt
LOGR FAIL exit=198 evidence=LOGR-playtest-night-S2-B17-final-default.txt
Boss FAIL exit=198 evidence=boss-recovery-night-S2-B17-final-default.txt
REGRESSION 0/13
TOOL BLOCKED: Unity Editor license exit198 before checks ran; wrapper exited before normal result normalization.

BATCH CARD 2559c4025c643fab8c242728359e42cc66018ce4..HEAD (ae90387)
변경 파일: 23 (+4960 / -2027 줄, 바이너리 0)
  Scripts: 3
  Scenes: 6
  Prefabs: 0
  Editor: 2
  Validation: 8
  문서: 2
  기타: 2
허용 범위: Assets/Scenes/V5/**, Assets/Editor/**, Assets/Scripts/Verification/**, Assets/Scripts/V5*.cs, Tools/Check-RoomRules.py, Launchers/current/*V5*, Validation/B17-*, Batches/BATCH-17.md, TASKS.md, README.md, Builds/**
허용 범위 밖: 0
TASKS 상태(표 둘째 칸): DONE 45 / REVIEW 9 / DOING 0 / BLOCKED 3 / PARTIAL 10
결정 대기(## 2. 결정 대기 표의 D- 행): 1
열린 버그(## 4. 열린 버그 표 행): 3
직접 플레이 확인 대기(## 3. 목록 항목): 5
TASKS 깨진 링크 수: 0
TASKS가 가리키지만 없는 빌드·런처 수: 0
마지막 REGRESSION: REGRESSION 0/13 (Validation/_raw/regression/B17-final-default/result.txt)
```

카드는 통합 전 HEAD에서 계산했다. 마지막 합치기는 Git tree를 유지하며 Base 이후 이력만 바꾼다. 카드의 TASKS 전체 상태 합계는 이전 Batch 표도 포함한다.
<!-- B17 verification end -->

# Batch 17 — V5 층 A draft2

**종료: 구현·전달·측정 완료, T7 종료 기본 회귀는 Unity Editor 라이선스로 BLOCKED.** 입력 보존 Base `2559c4025c643fab8c242728359e42cc66018ce4`, 시작2026-10-16 22:50:24 KST. 종료 2026-10-16T23:40+09:00. 예산9/10 Task·5시간 이내. Batch Backlog1~2 소진, TASKS Approved Backlog 비어 있음.

**원인 분류: A02·A05 모두 Scene 지형·경계 데이터.** draft2 Scene6개를 재생성하여 A02 왼쪽 복귀·A05 추적·머리 여유 기준을 통과했다. 구덩이6개 모두 탈출0.400~0.417초·달리며 점프 PASS. 코어 로직 수정0. V4/Resources/튜닝/Prefab/V4런처/Design 보호206파일 SHA변경0.

|Task|유형|상태|로컬 Task 커밋|실제 결과/증거|
|---|---|---|---|---|
|T1|검증|DONE|`3af35a9`|기존 Scene 원인 측정·Quiet13/13, [전후](B17-probe.md)|
|T2|기능|REVIEW|`3778480`|Scene6·요소 일치·구덩이6·도랑 덮임0·Quiet13/13, [생성](B17-generation.md)|
|T3|검증|DONE|`33bc8a2`|A02/A05 수정 확인·문14·stub/일방·저장·구덩이 PASS·Quiet13/13, [측정](B17-probe.md)|
|T4|기능|DONE|`5658814`|추가 규칙0위반·fixture5개 검출·Quiet13/13, [규칙](B17-room-rules-v5-A.md)|
|T5|기능|REVIEW|`b0b820e`|Development 오류0·경고0·V5런처3 Error0·Quiet13/13, [전달](B17-delivery.md)|
|T6|문서|DONE|`ad038a4`|원인/재플레이/빌드 인계·링크0불량·Quiet13/13, [인계](B17-handoff.md)|
|T7|검증|BLOCKED|`c71a5a3`+종료기록|앞선Quiet13/13·18런처Error0·카드0문제, 종료 기본은 license198로 미실행|
|BL1|검증|DONE(측정)|`caa91e4`|자연A01→A03·독립A05대시, A03/A04 각120초BLOCKED, [경로](B17-route.md)|
|BL2|문서|DONE(계산)|`9b301e2`|면적4548→4648·적13→15·등불13→14·합계 일치, [비교](B17-v5-draft1-vs-draft2.md)|

상태 DONE6 / REVIEW2 / BLOCKED1. 유형 기능3 / 검증4 / 문서2. Task 커밋은 마지막에 Base 이후만 합쳐 태그 `B17`로 인계한다. 입력 보존 커밋과 Base 이전 이력은 유지, push·Sprint Gate 없음.

## 실제 검증과 실행 불가

T1~T7 Quiet는 각각 실제13/13. 전체15런처+V5 NoMap3=18/18 PASS·각10초Error0. 23:32 BL1 입력 재시도 빌드부터 Editor가 라이선스 exit198로 시작 전에 종료됐다. 동일 빌드 재시도1회도 같아 해당 경로를 중단했고 미컴파일 입력 변경은 마지막 성공 상태로 복원했다. BL1/BL2 종료 Quiet와 종료 기본 회귀도 같은 외부 사유로 검사 미실행. 맨 위0/13은 게임 검사 실패13건을 뜻하지 않는다. 앞선13/13과 제품 exe 측정은 무효화하지 않았다.

기본 방식 wrapper가 내부 exit1로 정규화 전에 종료하여, 생성된 원본13줄을 `_raw/regression/B17-final-default/`로 옮겨 보존하고 PASS/FAIL 줄 수로0/13을 집계했다. [원본13줄](_raw/regression/B17-final-default/REG-B17-final-default-summary.txt), [집계](_raw/regression/B17-final-default/result.txt), [이전 T7 성공](_raw/regression/B17-T7/result.txt), [18런처](_raw/B17/T7-launchers-result.txt). 라이선스 원문:

```text
[Licensing::Module] Error: 'com.unity.editor.headless' was not found.
No valid Unity Editor license found. Please activate your license.
Exiting without the bug reporter. Application will terminate with return code 198
```

라이선스 중단 뒤에도 V5 3런처를 다시10초Error0으로 확인했다([원본](_raw/B17/Final-after-license-launchers-result.txt)). 최신 `Builds/B17-delivery` + 직전 `Builds/B12-delivery`만 남겼다. 검증 임시 빌드는 제거하고 원본 로그/CSV/PNG는 모두 보존했다.

## 열린 항목과 사용자에게 필요한 일

- **D-UNITY-LICENSE**: Unity Hub에서 유효한 Editor 라이선스를 복구한 뒤 T7 종료 기본 회귀를 재실행해야 한다. 복구 전에도 전달 exe는 플레이 가능하다.
- **직접 플레이 수락(T2/T5)**: [Play-V5](../Launchers/current/Play-V5.cmd)에서 A02 오른쪽→왼쪽 복귀, A05 낙하/사당 후 카메라, 낮은 통로/구덩이를10~15분 확인. A06은 [DJ](../Launchers/current/Play-V5-DJ.cmd). 기존 V4 수락 대기 순서 유지.
- A03 동쪽 포고(40.24,2.78)·A04 상부 진입(19.35,0.58)은 자동 입력120초BLOCKED. 자연 전체 대시 완주는 미확인. 물리적으로 불가능하다고 판정하지 않았으며, [원문](B17-route.md)을 Claude 검토 자료로 남겼다.
- 추가한 정적 규칙은0위반이나 기존 그래프 후보22FAIL(도달성20·단차2)은 보존했다. WORLD_V5 4절 총면적4548/A05높이10 표기는 구버전이며9절·draft2 JSON의4648/높이12를 우선했다. Design 수정0, 표기 정리는 Claude에게 돌린다.

결정 대기1, 기존 열린 제품 버그3건 유지. 층 B/C·게임 수치·규칙·PRD 범위 변경 없음.
