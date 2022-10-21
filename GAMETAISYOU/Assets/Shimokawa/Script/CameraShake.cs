using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraShake : MonoBehaviour
{
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(CameraDoShake(duration, magnitude));
    }

    private IEnumerator CameraDoShake(float Duration, float Magnitude)
    {
        var Positon = transform.localPosition;

        var Elapsed = 0f;

        Gamepad Gamepad = Gamepad.current;

        if (Gamepad != null)
        {
            Gamepad.SetMotorSpeeds(1.0f, 1.0f);
        }

        while (Elapsed < Duration)
        {
            #region ƒJƒƒ‰‚Ì—h‚ê•Žw’è
            var x = Positon.x + Random.Range(-1f, 1f) * Magnitude;
            var y = Positon.y + Random.Range(-1f, 1f) * Magnitude;
            #endregion
            transform.localPosition = new Vector3(x, y, Positon.z);

            Elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = Positon;

        if (Gamepad != null)
        {
            Gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }
}