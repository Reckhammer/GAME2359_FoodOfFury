using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author(s):Abdon J. Puente IV
//
// Description: This class manages the fade to black
//
//----------------------------------------------------------------------------------------

public class ScreenFade : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DeathFade()
    {
        animator.SetTrigger("FadeIn");
    }

    public void Restart()
    {
        animator.SetTrigger("Reset");
    }

}
