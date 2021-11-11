using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public Animator PointerAnim;
    public Animator BorderAnim;
     
    public void PointerFiring()
    {
        PointerAnim.Play("Fire");
    }
    public void PointerIdle()
    {
        PointerAnim.Play("idle");
    }
    public void BorderFiring()
    {
        BorderAnim.Play("Border Hit Animation");
    }
    public void BorderIdle()
    {
        BorderAnim.Play("Border Idle Animation");
    }
}
