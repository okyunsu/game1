# B07 T0 — B06 review fixes

DONE. 2026-10-10 21:54–22:11 KST. Ending first writes CP-C01 once for V2; later checkpoints remain authoritative. Default world Continue→Ending preserved. Ground contact clears wall kick input lock; wall-slide dash faces away. Course-owned runtime tuning restores after death/room unload regardless of room ID. Save-writing N5 tools reject missing explicit save paths before writes.

Actual Unity Editor60: T0 fixes PASS/Error0; B06 wall PASS/Error0; B06b wall PASS/Error0; LEVEL_METRICS repeated PASS/Error0. Existing basic13 regression13/13 PASS; PogoTest PASS. Current-source development route build succeeded. Whole route first failed R09, second reached R11 and failed exactly like previous B06b control build (0.110u start deviation). R09 did not repeat; continuous route is not claimed PASS.

Original key lines:
```text
FAIL System.Exception: R09 fixed-tick replay recording exhausted room=B04 pos=(9.20, 3.21)
FAIL System.Exception: R11 fixed-tick replay recording exhausted room=B01 pos=(33.31, 5.41)
MEASURE preset=1 long horizontal=4.891083 peakHeight=2.47
MEASURE preset=1 short horizontal=1.560028 peakHeight=2.47 reattachGain=2.078001
MEASURE preset=1 wallJumpDash validatedGap=8.05 delay=0.55 resolution=0.05 clearance=0.22
MEASURE preset=2 long horizontal=4.891083 peakHeight=2.52
MEASURE preset=2 short horizontal=0.3140259 peakHeight=2.47 reattachGain=2.428
MEASURE preset=2 wallJumpDash validatedGap=8.05 delay=0.55 resolution=0.05 clearance=0.22
```
Maximum previous metric difference 0.05u; remaining key distances identical. Course checker explicitly covered real death to A01/Asset reference restoration, wallward-input dash away, landing with a remaining .15s lock, first ending C01, later B01, repeated ending and default Ending routing.

No-save B06bEndingVerifier and B06BreakVerifier deliberately FAIL at `isolated explicit save path`, Error0, user's default progress.json SHA256 unchanged. Initial T0 fixture had `NullReferenceException` from adding an active empty V2 map before setting rooms; fixture initialization corrected once and a2 PASS. Initial sandbox Unity launch could not initialize licensing; host execution succeeded. No game rollback or tuning change required.

[Fixes](../../Validation/_raw/B07/T0-fixes-a2.txt), [metrics](../../Validation/_raw/B07/T0-metrics-a1.txt), [regression](../../Validation/_raw/B07/REG-B07-T0-host-summary.txt), [route second](../../Validation/_raw/B07/T0-route-a2.txt), [previous build control](../../Validation/_raw/B07/T0-baseline-route.txt), [save hashes](../../Validation/_raw/B07/T0-nosave-hash.txt). All initial failures/raw data retained. Unperformed: human feel, real controller. V3 ending branch belongs to T1/T2, not this T0 validation.
