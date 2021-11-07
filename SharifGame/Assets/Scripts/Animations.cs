using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public Animator PointerAnim;
     
    public void PointerFiring()
    {
        PointerAnim.Play("Fire");
    }
    public void PointerIdle()
    {
        PointerAnim.Play("idle");
    }
}
