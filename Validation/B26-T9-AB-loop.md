# B26-T9 연속 AB 고리 측정

DONE(지시문이 허용한 완주 또는 처음 막힌 단계/원문 기록). **전체 AB 완주·최종Continue는 미실시.** 최종 유효 실행은 `T9-AB-direct`이며 중간 Continue·HP/위치 보정이 없다. B01 CP에서 실제 플레이로 HP가3→4로 복원됐다.

A03 CP→A04 B1(12.985s·HP3)→A05 대시→A03 연습/선반64.507s→B01/CP/지붕→B02바닥 포고·산성/실제 E1공격→B03증기→B04 실제 벽점프 획득/보스2 승리(27.618s·HP1·벽점프4·포고26)→B03 실제 귀환까지 통과했다.

**첫 막힘: 입력.** B03 상부 귀환(3.000,17.013)에서 GoX15.8로 이동·낙하하다 (12.312,11.000)에서 HP1→0. 근처 유일 적은 승인 JSON의 E4(x13,y12), 원문 Hit 엣지에 `enemy:` collider가 함께 기록됐다. 사망 뒤에도 좌표만 보는 드라이버가 진행해 최종 GoX timeout으로 가려졌다. 지형 수정 필요로 단정하지 않는다. 이후 B06·B02상부·B01탑·B05/A02·최종Continue는 미실시.

원문: `_raw/26/T9-AB-direct/result.txt`·`player.log`·`save.json`, actual=exe/Error0. 구간별 시간:

```text
MEASURE B26 loop stage=begin CP-A03 to A-loop; no intermediate Continue seconds=0.000 room=A03 pos=(32.500, 1.012) HP=4
MEASURE B26 loop stage=A04 B1 A05 Dash A03 practice ledge; no Continue seconds=64.507 room=A03 pos=(20.735, 15.019) HP=3
MEASURE B26 loop stage=A03 ledge B01 seconds=3.167 room=B01 pos=(5.000, 1.012) HP=3
MEASURE B26 loop stage=B01 roof B02 seconds=8.351 room=B02 pos=(3.500, 1.013) HP=4
MEASURE B26 loop stage=B02 bottom B03 seconds=12.750 room=B03 pos=(3.500, 1.012) HP=4
MEASURE B26 loop stage=B03 steam B04 seconds=9.250 room=B04 pos=(20.500, 1.013) HP=4
MEASURE B26 loop stage=B04 actual B2 victory seconds=31.869 room=B04 pos=(6.912, 5.495) HP=1
MEASURE B26 loop stage=B04 exit B03 entry seconds=2.750 room=B03 pos=(3.000, 17.013) HP=1
```

첫 HP0/실패 원문:

```text
MEASURE B26 loopHit room=B03 HP=0 pos=(12.312, 11.000) nearby=enemy::BoxCollider2D,Player(Clone):BoxCollider2D,Camera Boundary:PolygonCollider2D,steam:V1:BoxCollider2D
FAIL System.Exception: GoX timeout target=15.8 actual=(20.37, 1.02)
```

초기 `T9-AB-loop-r1`은 A고리 뒤 중간Continue를 포함한 경로 변형이었다. 원본은 보존하지만 최종 연속 경로 증거로 쓰지 않는다. 종료 검토에서 이를 제거하고 위 direct 실행으로 다시 측정했다.
