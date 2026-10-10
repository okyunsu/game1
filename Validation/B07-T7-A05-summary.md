# B07 T7 — A05 / REVIEW
2026-10-10 23:21–23:26 KST. 72×16u, no enemies/lethal pits. Actual route starts without Dash, collects green DashPickup24/1.8, traverses practice lattice42/2.5 (1×3, separator roof above4), attacks directional wall V3-shortcut-A05 from its allowed left side, enters A03 FromA05 lower tunnel. Roof center64/y4.5/16×1 blocks wall-top shortcut. Existing tuning preserved.

First main input failed at roof after optional stairs; initial optional PASS. Moved optional to entrance x8 to15 so it does not lead onto shortcut roof. Optional three landings top2.6/4.3/6 widths3/2/3, dash at middle, discovery15/6.8. User may revisit after Dash pickup; optional is a timing challenge, not a designated G-D ability gate.

Final Development B07-A05-a2 exit0; actual record and fixed replay main/optional PASS/Error0:
```
MEASURE route seconds=12.264874 ticks=598 stage=0 pos=(7.03, -2.19) pogos=0
PASS A05 main actual input replay
MEASURE route seconds=2.2971645 ticks=112 stage=0 pos=(14.64, 6.88) pogos=0
PASS A05 optional actual input replay
```
First failure preserved:
```
MEASURE route seconds=35.0590703 ticks=1752 stage=4 pos=(65.07, 5.81) pogos=0
FAIL System.Exception: A05 main actual input record
```
Screenshot Validation/_raw/B07/A05-overview.png inspected. Raw A05-* logs/inputs preserved. Both fixtures isolated saves; main ability pickup and wall breaking occurred through actual input, without traversal teleporting.

Remaining: wrong-side wall/gate combinations T22; natural feel REVIEW. Full world integration T24 still depends on PARTIAL A02 path.