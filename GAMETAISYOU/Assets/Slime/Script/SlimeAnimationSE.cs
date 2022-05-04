using UnityEngine;

public class SlimeAnimationSE : MonoBehaviour
{
    [SerializeField] SlimeSE slimeSE;

    public void _PlayMoveSE()
    {
        slimeSE._PlayMoveSE();
    }

    public void _PlayLandingSE()
    {
        slimeSE._PlayLandingSE();
    }
}
