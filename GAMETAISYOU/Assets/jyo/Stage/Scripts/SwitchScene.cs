using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SwitchScene
{
    //���݂̃V�[�����ċN������
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
