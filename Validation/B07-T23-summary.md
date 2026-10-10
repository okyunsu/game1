# B07 T23 V3 guide / DONE
2026-10-11 08:08–08:36 KST. Dedicated GuideObjectives_V3/GuideGraph_V3 resources, WORLD_V3 9-1 nine stages. Actual Scene exits generate graph;31 mandatory direction lanterns, optional/shortcut exits unmarked. Original V2 objective/graph and scenes untouched. Release final normal/no-guide runs PASS/Error0; all19 rooms match actual exit graph and no-guide shows0 lanterns/goals/debug. F1 Development behavior validated separately in T26. Cached launch flag and GUI style; one Stage calculation per OnGUI.
```text
EVIDENCE T23-guide-final-release.txt
PASS V3 objective stage 1
PASS V3 objective stage 2
PASS V3 objective stage 3
PASS V3 objective stage 4
PASS V3 objective stage 5
PASS V3 objective stage 6
PASS V3 objective stage 7
PASS V3 objective stage 8
PASS V3 objective stage 9
PASS F1 build and no-guide visibility
MEASURE V3 guide stages=9 rooms=19 requiredLanterns=31
Error/Exception/Assert=0
EVIDENCE T23-no-guide-final-release.txt
PASS V3 objective stage 1
PASS V3 objective stage 2
PASS V3 objective stage 3
PASS V3 objective stage 4
PASS V3 objective stage 5
PASS V3 objective stage 6
PASS V3 objective stage 7
PASS V3 objective stage 8
PASS V3 objective stage 9
PASS F1 build and no-guide visibility
MEASURE V3 guide stages=9 rooms=19 requiredLanterns=31
Error/Exception/Assert=0
```
Raw T23-* retained; graph inventory at T23-graph.txt. Nine states were exercised in progression order with explicit verification state fixtures; natural full-world routing remains a separate Task.
