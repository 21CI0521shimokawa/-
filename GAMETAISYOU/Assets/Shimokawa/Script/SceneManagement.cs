using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace SceneDefine
{
    /// <summary>
    /// �Q�[���ɑ��݂���V�[�����Ǘ�����enum
    /// </summary>
    public enum SceneName
    {
        GAMECLER,
        TITLE,
        S01,
        S02,
        S03,
        S11,
        S12,
        S13,
        S21,
        S22,
        S23,
        S24,
        S31,
        S32,
        S33,
        S34,
        S41
    }
    public class SceneManagement
    {
        /// <summary>
        /// ���̃V�[���Ɉڍs����֐� 
        /// </summary>
        /// <param name="FadeTime"></param>
        public static void LoadNextScene(float fadeTime)
        {
            try
            {
                foreach (var seek in NowSceneNameDictionary.Keys)
                {
                    //���݂̃V�[���̖��O���擾
                    string nowSceneName = SceneManager.GetActiveScene().name;
                    //���݂̃V�[���̖��O�ƃf�B�N�V���i���[�̒��ň�v���閼�O�����邩
                    if (NowSceneNameDictionary[seek] == nowSceneName)
                    {
                        //Enum����string�ɕϊ�
                        string loadSceneName = GetNextSceneString(seek);
                        //���̃V�[�������[�h����
                        FadeManager.Instance.LoadScene(loadSceneName, fadeTime);
                    }
                }
                //���O���擾�o���Ȃ�������G���[�𓊂���
                throw new KeyNotFoundException();
            }
            catch (Exception error)
            {
                //�G���[���b�Z�[�W���o��
                Debug.LogError(error.Message);
            }
        }
        /// <summary>
        /// ���ǂݍ��ރV�[�������擾
        /// </summary>
        /// <param name="nowSceneName"></param>
        /// <returns></returns>
        private static string GetNextSceneString(SceneName nowSceneName)
        {
            //���݂̃V�[��+1���Ď��̃V�[�����w��
            return NowSceneNameDictionary[nowSceneName + 1];
        }

        /// <summary>
        /// �o�^���Ă���V�[�����Ǘ���string�^�ɂ��Ή�������Dictionary
        /// </summary>
        private static readonly Dictionary<SceneName, string> NowSceneNameDictionary = new Dictionary<SceneName, string>()
        {
           {SceneName.GAMECLER,"GameClear" },
           {SceneName.TITLE,"Title" },
           {SceneName.S01,"S0-1" },
           {SceneName.S02,"S0-2" },
           {SceneName.S11,"S1-1" },
           {SceneName.S12,"S1-2" },
           {SceneName.S13,"S1-3" },
           {SceneName.S21,"S2-1" },
           {SceneName.S22,"S2-2" },
           {SceneName.S23,"S2-3" },
           {SceneName.S24,"S2-4" },
           {SceneName.S31,"S3-1" },
           {SceneName.S32,"S3-2" },
           {SceneName.S33,"S3-3" },
           {SceneName.S34,"S3-4" },
           {SceneName.S41,"S4-1" },
        };
    }
}
