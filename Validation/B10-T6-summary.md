# B10-T6 자연 진행

상태 PARTIAL: 실제 새 게임부터 대시·벽 점프를 획득하고 B05 등반을 넘어 C02까지 진행했다(r73/r76). S02 연속 완주와 완주 입력은 미확보. 위치/능력 fixture·QA Scene 전환 없음.

B05 저장 입력 실패 뒤 실제 위치에서 양쪽 굴뚝 벽을 사용하는 회복 입력이 platform1/2에 도달했다. 같은 층 E3를 실제 공격으로 처치했다. C02 r73 직접 접근은 도랑에서 실패; r76은 작성된 하강 경로 PASS 후 F:40 앞 상자에서 바닥 목표 미도달. 마지막 r78은 상자 solid9를 경유하도록 보완했으나 권한 승인 대기 뒤 예산 초과가 확인되어 종료했다. r78 완주/개선 성공을 주장하지 않음.

실제 자연 저장: AfterDash CP-A03, AfterWall CP-B01. 보스 전/엔딩 후 미확보, 대체 fixture 저장을 만들지 않았다.

원본 실패·핵심 수치:
```text
T6-resume-r73-natural.txt
FAIL B10JumpSearch actual=exe
FAIL saved goal room=B05 target=platform1@wj actual-room=B05 pos=(4.64, 30.56) feet=30.56103 HP=4 frames=648
FAIL saved goal room=B05 target=platform2@wj actual-room=B05 pos=(1.37, 29.24) feet=29.24302 HP=3 frames=526
FAIL natural recovery from actual position target=platform2@wj end=(9.93, 32.01); no placement/grant
FAIL natural move C02 source=point:3.5,10.01496 target=solid1~56 end=(43.55, 0.41) HP=1 frames=665
FAIL natural approach C02/up pos=(43.55, 0.41)
PENDING natural new game -> S02 no placement/grants actual=C02
MEASURE observed active render-fps=60.0885009091046 frames=22096
MEASURE B10 mode=world PASS=0 BLOCKED-GEOMETRY=0 budget-deferred=2
FAIL System.Exception: Search budget reached before exhaustive classification; pending items retained
T6-resume-r76-natural.txt
FAIL B10JumpSearch actual=exe
FAIL saved goal room=B05 target=platform1@wj actual-room=B05 pos=(4.64, 30.55) feet=30.54704 HP=4 frames=648
FAIL saved goal room=B05 target=platform2@wj actual-room=B05 pos=(1.37, 29.24) feet=29.24302 HP=3 frames=526
FAIL natural recovery from actual position target=platform2@wj end=(9.93, 32.01); no placement/grant
FAIL natural move C02 source=point:35.95062,1.014999 target=solid0~40 end=(41.17, 2.82) HP=3 frames=421
FAIL natural C02 authored descent/dash approach
PENDING natural new game -> S02 no placement/grants actual=C02
MEASURE observed active render-fps=60.1017996628354 frames=22441
MEASURE B10 mode=world PASS=0 BLOCKED-GEOMETRY=0 budget-deferred=2
FAIL System.Exception: Search budget reached before exhaustive classification; pending items retained
```

원본 `_raw/B10/T6-resume-r*-natural.*`, 전체 실패 입력 `_raw/B10/inputs/natural-*.json`(완주 입력으로 사용 금지). 원본 저장을 실행기 saves/에 복사했다. 미실시: 새 게임→S02 60fps 완주, 그 입력의 재생30/120.
