# V3 실제 Scene 연결도
required는 가이드 등불 대상, unmarked는 선택/귀환. A02 필수/굴뚝은 PARTIAL이다.
```text
A01 -> A06 / Entry / unmarked
A01 -> A02 / Entry / required
A02 -> A03 / Entry / required
A02 -> A01 / Entry / required
A02 -> B05 / Entry / required
A03 -> A04 / Entry / required
A03 -> A05 / FromShortcut / unmarked
A03 -> B01 / Entry / required
A03 -> A02 / FromA03 / required
A04 -> A05 / Entry / required
A04 -> A03 / Entry / required
A05 -> A04 / Entry / required
A05 -> A03 / FromA05 / unmarked
A06 -> A01 / FromOptional / unmarked
B01 -> B02 / Entry / required
B01 -> B04 / FromShortcut / unmarked
B01 -> A03 / FromPit / required
B02 -> B01 / Entry / required
B02 -> B03 / Entry / required
B03 -> B06 / Entry / unmarked
B03 -> B04 / Entry / required
B03 -> B02 / Entry / required
B04 -> B03 / FromB04 / required
B04 -> B01 / FromB04 / unmarked
B05 -> C01 / Entry / required
B05 -> A02 / FromB05 / required
B06 -> B03 / FromB06 / unmarked
C01 -> B05 / FromC01 / required
C01 -> C02 / Entry / required
C02 -> C03 / Entry / required
C02 -> C01 / Entry / required
C03 -> C04 / Entry / required
C03 -> C02 / Entry / required
C03 -> C05 / Entry / required
C04 -> C06 / FromLeft / required
C04 -> C03 / FromC04 / required
C05 -> C03 / FromC05 / required
C06 -> S01 / Entry / required
C06 -> C04 / FromRight / required
S01 -> C06 / FromLeft / unmarked

```
