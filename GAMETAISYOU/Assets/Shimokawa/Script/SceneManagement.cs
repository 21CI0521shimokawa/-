using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace SceneDefine
{
    /// <summary>
    /// ゲームに存在するシーンを管理するenum
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
        /// 次のシーンに移行する関数 
        /// </summary>
        /// <param name="FadeTime"></param>
        public static void LoadNextScene(float fadeTime)
        {
            try
            {
                foreach (var seek in NowSceneNameDictionary.Keys)
                {
                    //現在のシーンの名前を取得
                    string nowSceneName = SceneManager.GetActiveScene().name;
                    //現在のシーンの名前とディクショナリーの中で一致する名前があるか
                    if (NowSceneNameDictionary[seek] == nowSceneName)
                    {
                        //Enumからstringに変換
                        string loadSceneName = GetNextSceneString(seek);
                        //次のシーンをロードする
                        FadeManager.Instance.LoadScene(loadSceneName, fadeTime);
                    }
                }
                //名前を取得出来なかったらエラーを投げる
                throw new KeyNotFoundException();
            }
            catch (Exception error)
            {
                //エラーメッセージを出す
                Debug.LogError(error.Message);
            }
        }
        /// <summary>
        /// 次読み込むシーン名を取得
        /// </summary>
        /// <param name="nowSceneName"></param>
        /// <returns></returns>
        private static string GetNextSceneString(SceneName nowSceneName)
        {
            //現在のシーン+1して次のシーンを指定
            return NowSceneNameDictionary[nowSceneName + 1];
        }

        /// <summary>
        /// 登録してあるシーンを管理しstring型にも対応させるDictionary
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
