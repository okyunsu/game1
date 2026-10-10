# B07 T1 — V3 foundation

DONE. 2026-10-10 22:10–22:15 KST. New V3SceneMap resolves rooms only for `-v3`, before existing V2 mapping. All 19 new V3 Scene foundations preserve originals; they are explicitly not authored V3 room layouts yet. New build inventory includes Menu, base22, V2-18, V3-19 (59 Scenes), without EditorBuildSettings edits. B07 output keeps previous V2 as its default mode.

ProgressData keeps version1, adds visitedRooms with missing-array default empty. Visits persist on V3 room entry and restore on Continue. RoomSession no-checkpoint V3 respawn resolves A01_V3. Ending's one-time CP-C01 rule is enabled for the V3 mapping too; boss-to-summit flow follows in T2.

Actual new Development exe: `-v3` NewGame A01_V3 PASS; CP-A03/B01/C01/C04 save→title→Continue maps to corresponding V3 room/checkpoint PASS; visitedRooms survive; legacy JSON without visitedRooms loads empty and Continue A01_V3 PASS. Same exe without `-v3`: previous V2 NewGame A01_V2 PASS. Error/Exception/Assert0 both runs. Build `Succeeded errors=0 warnings=0`. Base-mode scripts remain opt-in; T0 basic13 already passed and T25 repeats final regression. Foundation setup uses explicit scene transitions, not natural room traversal.

[Actual V3](./_raw/B07/T1-v3-a1.txt), [V2 default](./_raw/B07/T1-v2-a1.txt), [build inventory](./_raw/B07/B07-foundation-build.txt). Protected tracked Scene/Tuning/ProjectSettings/Packages/BossEncounter hash differences0. Unperformed: room traversal, gate geometry, natural full run, human feel; scheduled later Batch tasks.
