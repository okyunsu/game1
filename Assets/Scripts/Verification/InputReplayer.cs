#if UNITY_EDITOR || DEVELOPMENT_BUILD
// Same recorded metadata/tick driver, separately named CLI replay component.
public sealed class InputReplayer:InputRecorder{}
#endif
