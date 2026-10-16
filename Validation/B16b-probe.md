# B16b T1a — 기존 생성기 프로브

상태: DONE(프로브 수행 완료, 생성 결과는 실패). 제품 코드 수정0.

기존 N5Args CLI는 `-name value` 쌍을 읽으므로 -room-design / -room-output / -map-design에 경로를 별도 인자로 전달했다. 출력: Assets/Scenes/_tmp-gen-check. Unity exit1, 첫 방의 외부 stub 대상 Z01 조회에서 실패했다. Scene 저장 전 실패하여 임시 Scene0, 빈 폴더와 meta는 확인 후 삭제했다.

```text
KeyNotFoundException: The given key 'Z01' was not present in the dictionary.
  at B09Authoring.Generate (System.String dataPath, System.String destinationFolder, System.String worldPath) [0x00c71] in C:\Users\gosu3\OneDrive\문서\ChatGPT\game1\Assets\Editor\B09Authoring.cs:79
executeMethod method B09Authoring.Generate threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
```

기존 V4 Scene·Resources 모든 파일 시작 전 SHA256 대비 변경0. 원본: `Validation/_raw/B16b/T1a-probe.log`, 시작 hash CSV. T1b에서 대상 방 존재 검사와 stub 차단을 최소 추가한다.

입력 불일치: 지시문은 수정4개를 말하지만 Base에는 WORLD_V5/rooms의 A04 수정만 수신됐다. A02 조각·A05 spawn·world rect/문 ID는 아직 이전 값. 사용자에게 확인 요청했고 데이터는 수정하지 않는다.

T1a Quiet: REGRESSION 13/13. 원본 Validation/_raw/regression/B16b-T1a/result.txt.
