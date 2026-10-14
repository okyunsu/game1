#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B06bDeviceVerifier:N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B06bDeviceVerifier>("-verify-b06b-device");
    protected override IEnumerator Tests()
    {
        yield return Ready();yield return new WaitForSeconds(.2f);var course=FindFirstObjectByType<WallJumpTestCourse>();
        yield return Keys(Key.RightArrow);Check(!Player.UsingGamepad&&course.DeviceGuide.Contains("Z 점프"),"keyboard guide");yield return Keys();
        var pad=InputSystem.AddDevice<Gamepad>();InputSystem.QueueStateEvent(pad,new GamepadState{leftStick=Vector2.left});yield return null;yield return new WaitForSecondsRealtime(.04f);
        Check(Player.UsingGamepad&&course.DeviceGuide.Contains("A 점프")&&course.DeviceGuide.Contains("B 대시"),"keyboard to pad guide");
        InputSystem.QueueStateEvent(pad,new GamepadState());yield return null;yield return Keys(Key.RightArrow);
        Check(!Player.UsingGamepad&&course.DeviceGuide.Contains("Z 점프")&&course.DeviceGuide.Contains("C 대시"),"pad to keyboard guide");
        lines.Add("MEASURE device guide switches=2 keyboard/pad/keyboard actual Input System virtual devices");InputSystem.RemoveDevice(pad);yield return Keys();
    }
}
#endif
