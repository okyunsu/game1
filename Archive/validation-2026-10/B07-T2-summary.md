# B07 T2 — V3 connection parts

DONE. 2026-10-10 22:15–22:20 KST. New prefabs: solid DecorativeGate, OneWayDrop exit rule, SummitEnding trigger. New V3BossDoor opens the upper exit/stairs on victory, suppresses saved defeated boss and preserves active combat arena geometry (post-victory stairs inactive during combat). BossEncounter source unchanged. Original EndingFlow returns early only for V3. V3 victory saves CP-C04 before summit; S01 fade2s→Ending, one-time CP-C01 at ending. EndingMenu branches text only for V3.

Actual Development exe V3 standalone fixture PASS/Error0: rising overlap rejected; downward overlap transitions A02; summit door solid/closed and stairs inactive during fight; boss defeated event opens door but remains C06_V3 after2.3s; exit→title→Continue CP-C04; C06 revisit boss absent/door open; actual physical upper door transitions S01; central trigger starts fade; ending menu after2.0446067s; saved ending CP-C01; Continue C01_V3. Explicit fixtures and API boss defeat used; not natural full play.

Same new exe without `-v3`, existing B06bEndingVerifier PASS/Error0: V2 old pre-ending save→Ending; post-ending C01/abilities/walls restored; C06 boss absent/physical return door usable; NewGame resets; legacy save behavior preserved. Compiled decorative prefab structural check PASS solid collider/no exit/no BreakableWall/no interaction. Development build `Succeeded errors=0 warnings=0`.

```text
MEASURE parts: downward only, boss door+suppression, CP-C04 before ending, summit fade2s, CP-C01 after ending
MEASURE post-ending Continue C01_V2/CP-C01 abilities3/3 brokenWalls1/1 boss0 exits usable ending repeats0 legacy PASS
```

[Parts actual](../../Validation/_raw/B07/T2-v3-a1.txt), [V2 actual](../../Validation/_raw/B07/T2-v2-ending-a1.txt), [decorative check](../../Validation/_raw/B07/T2-decoration.txt), [build](../../Validation/_raw/B07/B07-parts-build.txt). Protected originals hash differences0. Human ending emotion and natural boss/S01 traversal await room tasks/T24 and user REVIEW. C06/S01 currently foundation plus standalone parts, final layout not claimed.
