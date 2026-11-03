# B26-T9 전체 AB 고리 측정

상태: DONE(지시문 합격 기준인 완주 또는 처음 막힌 단계/원문 기록). **전체 AB 완주 아님.** 최초 실패 뒤 후속 구간·최종 Continue는 미실시.

실제 연속 입력: A03 CP→A04 보스1(12.987s·HP3)→A05 대시→A03 연습/선반·A 고리 Continue(64.946s)→B01 CP/지붕→B02 바닥 포고/대시/E1 실제 공격→B03 증기→B04 실제 벽점프 획득/보스2 승리(27.702s·HP1·벽점프5·포고26)까지 통과. 두 보스 ID와 대시/벽점프는 save.json에 기록.

처음 막힌 후속 단계: B04 처치 뒤 B03 귀환/동쪽 굴뚝 접근의 GoX(15.8). 원문에는 target15.8/actual20.37,1.02. 분류=도구: 좌표만 검사하는 GoX가 사망/다른방 복귀를 감지하지 않고 계속 입력. 처치 후 death 오디오가 관측됐지만 플레이어/적 공유 이벤트이며 정확한 피격 원인·첫 방이탈 프레임은 이번 도구로 확정 못함. 지형 결함으로 단정하지 않는다. 후속 측정에는 현재방/HP/사망 엣지 추적이 필요하다. 원문 실패를 숨기거나 뒤 구간으로 순간이동하지 않았다.

원문: _raw/26/T9-AB-loop-r1/result.txt·player.log·save.json. actual=exe, Error0. 구간별 시간:
```
MEASURE B26 loop stage=begin CP-A03 to A-loop seconds=0.000 room=A03 pos=(32.500, 1.012) HP=4
MEASURE B26 loop stage=A04 B1 A05 Dash A03 practice ledge Continue seconds=64.946 room=A03 pos=(32.500, 1.015) HP=4
MEASURE B26 loop stage=A03 ledge B01 seconds=17.719 room=B01 pos=(5.000, 1.013) HP=4
MEASURE B26 loop stage=B01 roof B02 seconds=8.300 room=B02 pos=(3.500, 1.013) HP=4
MEASURE B26 loop stage=B02 bottom B03 seconds=13.768 room=B03 pos=(3.500, 1.012) HP=4
MEASURE B26 loop stage=B03 steam B04 seconds=9.267 room=B04 pos=(20.500, 1.012) HP=4
MEASURE B26 loop stage=B04 actual B2 victory seconds=31.953 room=B04 pos=(6.758, 5.763) HP=1
```

실패 원문:
```
FAIL System.Exception: GoX timeout target=15.8 actual=(20.37, 1.02)
```
