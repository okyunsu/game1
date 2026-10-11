# B09-T8 — 방별 경로 입력

상태 DONE (요청한 PASS율·실패 분류 기록 완료). Release/60fps 전체70경로 실제 입력, 위치/능력 fixture는 경로 시작 준비에만 사용. 태그에 따라 대시/DJ/벽 점프/Slam/포고/레버 공격 입력을 생성하고 증기는 실제 장치 물리로 적용한다. 기둥 주기 기다림과 승강기 양쪽 레버 접근 계획은 제한적이다.

원문: `MEASURE mode=routes PASS=29 FAIL=41` / 41.4%. Error/Exception/Assert=0.

| 방 | PASS/전체 | 비율 |
|---|---:|---:|
| A01 | 1/5 | 20.0% |
| A02 | 1/3 | 33.3% |
| A03 | 1/3 | 33.3% |
| A04 | 2/3 | 66.7% |
| A05 | 0/2 | 0.0% |
| A06 | 2/3 | 66.7% |
| B01 | 3/5 | 60.0% |
| B02 | 1/4 | 25.0% |
| B03 | 0/2 | 0.0% |
| B04 | 0/3 | 0.0% |
| B05 | 1/2 | 50.0% |
| B06 | 1/3 | 33.3% |
| C01 | 1/3 | 33.3% |
| C02 | 2/4 | 50.0% |
| C03 | 2/5 | 40.0% |
| C04 | 2/4 | 50.0% |
| C05 | 2/3 | 66.7% |
| C06 | 1/1 | 100.0% |
| S01 | 1/1 | 100.0% |
| S02 | 1/1 | 100.0% |
| Z01 | 4/10 | 40.0% |

미완주41개는 자동 입력 한계로 잠정 분류해 수동 확인 필요로 표시한다. 데이터나 구현 오류로 확정할 근거는 없어 데이터 이동0. 코스 단위 검증과 방 경로 통과는 구분한다. r5는 다음 Scene 준비/엔딩 처리 도구 예외로 중단, r8은21방 전체를 기록했다. r8의 전 능력·보스 완료 fixture이므로 후속 V4 비활성 문 수정은 정상 완료 문 상태의 경로 결과에 영향을 주지 않는다.

원문 미완주 항목 (`_raw/B09/T-routes-r8.txt`):
```
FAIL B09TraverseVerifier actual=exe
MANUAL route A01/slam_pad waypoints=2/3 pos=(14.04, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route A01/upper waypoints=2/5 pos=(32.76, 1.01) classification=automatic-input-limit; product/data fault not established
MANUAL route A01/dj waypoints=2/6 pos=(32.77, 1.01) classification=automatic-input-limit; product/data fault not established
MANUAL route A01/slam waypoints=2/3 pos=(6.00, 17.30) classification=automatic-input-limit; product/data fault not established
MANUAL route A02/wall waypoints=1/4 pos=(10.41, 2.87) classification=automatic-input-limit; product/data fault not established
MANUAL route A02/secret waypoints=1/2 pos=(12.71, 2.75) classification=automatic-input-limit; product/data fault not established
MANUAL route A03/roofs waypoints=2/9 pos=(16.56, 1.01) classification=automatic-input-limit; product/data fault not established
MANUAL route A03/high waypoints=1/9 pos=(40.40, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route A04/slam_pad waypoints=2/3 pos=(40.17, 7.81) classification=automatic-input-limit; product/data fault not established
MANUAL route A05/main waypoints=4/7 pos=(202.00, 17.30) classification=automatic-input-limit; product/data fault not established
MANUAL route A05/secret waypoints=3/4 pos=(70.64, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route A06/up waypoints=3/8 pos=(13.04, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route B01/lift waypoints=2/4 pos=(54.92, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route B01/slam_pad waypoints=2/3 pos=(62.71, 8.01) classification=automatic-input-limit; product/data fault not established
MANUAL route B02/low waypoints=2/5 pos=(60.00, 14.30) classification=automatic-input-limit; product/data fault not established
MANUAL route B02/steam waypoints=1/2 pos=(51.39, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route B02/high waypoints=1/9 pos=(60.00, 13.92) classification=automatic-input-limit; product/data fault not established
MANUAL route B03/climb waypoints=4/12 pos=(17.05, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route B03/top waypoints=1/8 pos=(12.76, 7.81) classification=automatic-input-limit; product/data fault not established
MANUAL route B04/low waypoints=1/2 pos=(25.59, 2.01) classification=automatic-input-limit; product/data fault not established
MANUAL route B04/wj waypoints=1/2 pos=(44.52, 3.17) classification=automatic-input-limit; product/data fault not established
MANUAL route B04/practice waypoints=1/2 pos=(28.71, 4.10) classification=automatic-input-limit; product/data fault not established
MANUAL route B05/climb waypoints=1/6 pos=(14.00, 46.30) classification=automatic-input-limit; product/data fault not established
MANUAL route B06/up waypoints=1/3 pos=(20.36, 2.38) classification=automatic-input-limit; product/data fault not established
MANUAL route B06/steam waypoints=1/2 pos=(35.73, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route C01/high waypoints=5/7 pos=(34.95, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route C01/lift waypoints=2/4 pos=(44.60, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route C02/mid waypoints=2/5 pos=(44.21, 2.86) classification=automatic-input-limit; product/data fault not established
MANUAL route C02/up waypoints=2/8 pos=(63.66, 1.98) classification=automatic-input-limit; product/data fault not established
MANUAL route C03/chimney waypoints=1/3 pos=(28.70, 2.52) classification=automatic-input-limit; product/data fault not established
MANUAL route C03/gj waypoints=3/5 pos=(20.85, 1.00) classification=automatic-input-limit; product/data fault not established
MANUAL route C03/c05 waypoints=1/2 pos=(4.47, 13.26) classification=automatic-input-limit; product/data fault not established
MANUAL route C04/wj waypoints=1/2 pos=(37.52, 2.17) classification=automatic-input-limit; product/data fault not established
MANUAL route C04/dj waypoints=3/6 pos=(27.42, 2.82) classification=automatic-input-limit; product/data fault not established
MANUAL route C05/up waypoints=1/2 pos=(20.31, 3.68) classification=automatic-input-limit; product/data fault not established
MANUAL route Z01/up_A01 waypoints=1/2 pos=(4.02, 1.87) classification=automatic-input-limit; product/data fault not established
MANUAL route Z01/gallery waypoints=1/5 pos=(100.21, 1.83) classification=automatic-input-limit; product/data fault not established
MANUAL route Z01/steam waypoints=1/2 pos=(143.96, 2.16) classification=automatic-input-limit; product/data fault not established
MANUAL route Z01/lower waypoints=4/6 pos=(172.03, 1.02) classification=automatic-input-limit; product/data fault not established
MANUAL route Z01/up_A05 waypoints=1/2 pos=(201.19, 8.80) classification=automatic-input-limit; product/data fault not established
MANUAL route Z01/elevator waypoints=1/3 pos=(36.00, 1.00) classification=automatic-input-limit; product/data fault not established
MEASURE mode=routes PASS=29 FAIL=41
FAIL System.Exception: Incomplete actual input checks; see per-item classifications
```

전체 경로 PASS·고립0·자연 완주를 뜻하지 않는다.
