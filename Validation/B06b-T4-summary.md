# B06b T4 — 엔딩 후 이어하기

DONE. PendingScene getter만 변경: endingReached 우선 CP-C01로 pending checkpoint 정규화·C01/V2 resolve. 보스 처치 후 엔딩 전은 기존 Ending 유지. 저장 형식/능력/부순 벽은 변경하지 않음. C06_V2 새 DefeatedBossSuppression 컴포넌트13줄 추가/삭제0, 시작 시 처치 보스 inactive·출구 enabled·warning 숨김. BossEncounter/BossTuning 변경0.

실제 V240 Scene Development exe 격리 슬롯: 엔딩 진입→도달 flag 기록→타이틀→Continue C01_V2/CP-C01, 능력3/3·벽1/1 유지 PASS. C06 재진입 보스 inactive/출구 unlocked·물리출구 C04 사용·엔딩 재발생0, NewGame A01 능력초기화, endingReached 없는 기존 저장 Ending PASS/Error0.
`MEASURE post-ending Continue C01_V2/CP-C01 abilities3/3 brokenWalls1/1 boss0 exits usable ending repeats0 legacy PASS`

첫 Editor 검사는 V2 Scene이 기존 BuildSettings에 없어 도구 경로 실패:
`FAIL System.Exception: Timeout room ready`
`Scene 'Assets/Scenes/V2/C01_V2.unity' couldn't be loaded because it has not been added to the active build profile or shared scene list or the AssetBundle has not been loaded.`
ProjectSettings를 수정하지 않고 V2 포함 실제 exe 접근으로 전환1회, PASS.
[원본 exe](./_raw/B06b/T4-ending-exe-a1.txt), [Editor 실패](./_raw/B06b/T4-ending-a1.txt), [빌드](./_raw/B06b/B06b-T4-build.txt). 미실시: 사용자 자연 완주/재방문.
