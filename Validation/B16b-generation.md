# B16b T1b — 문 생성 최소 확장

상태: T1b DONE. 구현·생성 검사 완료, Quiet REGRESSION 13/13.

수정마다 Unity 컴파일을 실행했다: stub 대상 조회, 일방/낙하 역방향 차단, V5 로그 분리, 검증 실행기. 모두 컴파일 오류0. 새 차단은 V5 입력에만 적용한다. stub4개와 A02 일방 받는 쪽·A05 낙하 역방향은 disabled exit + 닫힌 충돌 영역으로 만든다. V5 생성 로그는 B16b 원본 폴더에 새 이름으로 저장해 B15 원본을 보존한다.

첫 SHA 검사에서 V4의 기존 down_only도 새 처리에 포함되어 5개 Scene이 달랐다. 검사가 즉시 해당 Scene을 복원했다. V5 조건으로 범위를 한정하고 컴파일을 통과한 뒤 1회 재시도했다.

```text
FAIL System.Exception: V4 default regeneration SHA differs; originals restored
PASS T1b-r1 actual=Editor
PASS six temporary rooms; default SHA equality
```

최종: V4 Scene21개·Resources 기존 전체 파일 재생성 전후 SHA256 동일(실제 바이트), V5 데이터 임시6방 생성 오류0. 임시 폴더 삭제. 원본: `_raw/B16b/T1b-generation.txt`(최초), `T1b-r1-generation.txt`(최종), `T1b-changed/`(복원 전 산출물), 컴파일별 로그.
