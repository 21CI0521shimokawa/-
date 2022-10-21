using UnityEngine;

[RequireComponent(typeof(CanvasGroup))] //CanvasGroup�R���|�[�l���g���A�^�b�`����Ă��Ȃ��ꍇ�A�A�^�b�`
public class FadeIn : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f; //�t�F�[�h�����鎞�Ԃ�ݒ�
    private float timer; //�o�ߎ��Ԃ��擾

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x�����Ă΂��֐�
    /// </summary>
    void Start()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0; //alpha�l��0(�����j�ɂ���B
    }

    /// <summary>
    /// ���t���[���Ă΂��֐�
    /// </summary>
    void Update()
    {
        timer += Time.deltaTime; // �o�ߎ��Ԃ����Z
        this.gameObject.GetComponent<CanvasGroup>().alpha = timer / fadeTime; //�o�ߎ��Ԃ�fadeTime�Ŋ������l��alpha�ɓ����
    }
}