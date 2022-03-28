using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraShake : MonoBehaviour
{
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    /// <summary>
    /// ÉRÉãÅ[É`Éì
    /// </summary>
    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = transform.localPosition;

        var elapsed = 0f;

        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(1.0f, 1.0f);
        }

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = pos;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }
}