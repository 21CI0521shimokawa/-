using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallControll : MonoBehaviour
{
    #region —h‚ê‚é •Ï”éŒ¾
    public CameraShake shake;
    #endregion
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    #region public function
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player" || collision.gameObject.tag == "Slime")
        {
            shake.Shake(0.25f, 0.2f);
        }
    }
    #endregion
}
