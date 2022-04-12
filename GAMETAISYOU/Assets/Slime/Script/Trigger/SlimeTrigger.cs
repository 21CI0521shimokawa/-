using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrigger : MonoBehaviour
{
    public bool _onTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _onTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _onTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _onTrigger = false;
    }
}
