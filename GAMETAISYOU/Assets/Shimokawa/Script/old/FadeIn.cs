using UnityEngine;

[RequireComponent(typeof(CanvasGroup))] //CanvasGroupコンポーネントがアタッチされていない場合、アタッチ
public class FadeIn : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f; //フェードさせる時間を設定
    private float timer; //経過時間を取得

    /// <summary>
    /// ゲームが始まる時に一度だけ呼ばれる関数
    /// </summary>
    void Start()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0; //alpha値を0(透明）にする。
    }

    /// <summary>
    /// 毎フレーム呼ばれる関数
    /// </summary>
    void Update()
    {
        timer += Time.deltaTime; // 経過時間を加算
        this.gameObject.GetComponent<CanvasGroup>().alpha = timer / fadeTime; //経過時間をfadeTimeで割った値をalphaに入れる
    }
}