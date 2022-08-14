using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCursorController : MonoBehaviour
{
    [SerializeField] Sprite[] sources_;
    int[] animValue_ = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 1 };

    Image cursor_;

    float animCount_;

    // Start is called before the first frame update
    void Start()
    {
        animCount_ = 0.0f;
        cursor_ = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        animCount_ += Time.unscaledDeltaTime;

        if (animCount_ >= animValue_.Length / 10.0f)
        {
            animCount_ -= animValue_.Length / 10.0f;
        }

        cursor_.sprite = sources_[animValue_[(int)(animCount_ * 10)]];
    }
}
