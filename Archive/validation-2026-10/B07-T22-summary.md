# B07 T22 gate and reverse-route checks / DONE
2026-10-11 08:08–08:30 KST. Actual Release B07-resume-a7; Error/Exception/Assert=0. G-D without Dash: jump/dash-button/attack denied. G-W no wall/Dash/DJ/Pogo remains below chimney. G-J all eight Dash/wall ownership and upward/downward attack combinations cannot reach C04 without DJ. Blocked-side real attacks leave both shortcut Hits0. Upward pit physical overlap rejected; A03 landing has no reverse exit and repeated DJ cannot return through pit.
Verifier corrections: a1 mistakenly treated walking under the thin G-J platform as bypass; changed to actual destination. a2/a3 gate checks pass but pit setup was already descending while testing rising overlap; final positive-velocity-before-overlap setup and Ready transition wait pass. These were verification fixture issues, not gameplay fixes.
```text
PASS G-D no Dash mode=0 x=43.135
PASS G-D no Dash mode=1 x=43.13501
PASS G-D no Dash mode=2 x=43.13501
PASS G-W no wall/Dash/DJ/Pogo peak=16.15295
PASS G-J no DJ ownership/attack mask=0 pos=(71.64, 1.81)
PASS G-J no DJ ownership/attack mask=1 pos=(71.64, 1.81)
PASS G-J no DJ ownership/attack mask=2 pos=(71.64, 1.81)
PASS G-J no DJ ownership/attack mask=3 pos=(71.64, 1.81)
PASS G-J no DJ ownership/attack mask=4 pos=(71.64, 1.81)
PASS G-J no DJ ownership/attack mask=5 pos=(71.64, 1.81)
PASS G-J no DJ ownership/attack mask=6 pos=(71.64, 1.81)
PASS G-J no DJ ownership/attack mask=7 pos=(71.64, 1.81)
PASS shortcut blocked-side actual attacks V3-shortcut-A05
PASS shortcut blocked-side actual attacks V3-shortcut-B04
PASS pit upward motion denied
PASS pit upward physical overlap rejected
PASS pit landing has no reverse B01 exit
PASS pit reverse jumping cannot enter B01
Error/Exception/Assert=0
```
Raw T22-gates-* preserved, including initial FAIL assertions. No gate ownership bits, boss or tuning changed by production code. Negative checks do not substitute for positive A02 traversal.
