using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBrightnessChange : MonoBehaviour
{
    [SerializeField] Light2D light;

    [SerializeField] float changeTime;

    [SerializeField] float afterOuterRadius;
    float beforeOuterRadius;

    bool isStart;
    float timeCount;

    // Start is called before the first frame update
    void Start()
    {
        beforeOuterRadius = light.intensity;

        isStart = false;
        timeCount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            timeCount += Time.deltaTime;

            float ratio = timeCount / changeTime;

            if(ratio >= 1.0f)
            {
                ratio = 1.0f;
                isStart = false;
            }

            light.pointLightOuterRadius = Mathf.SmoothStep(beforeOuterRadius, afterOuterRadius, ratio);
        }
    }

    public void BrightnessChangeStart()
    {
        isStart = true;
    }
}
