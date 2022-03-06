using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SwitchScene
{
    //Œ»İ‚ÌƒV[ƒ“‚ğÄ‹N“®‚·‚é
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
