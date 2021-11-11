﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public Rigidbody2D MyRig;
    public GameObject Pointer;
    public float width, heigh;

    private float BallForce = 0.04f;
    private float MaxVl = 20f;

    private bool CanBeDestroyed;

    private void Start()
    {
        MyRig.AddForce(new Vector2(BallForce, BallForce));
        CanBeDestroyed = false;
        StartCoroutine(WaiteToAllowDestroy());
    }

    private void Update()
    {
        Vector2 vl = MyRig.velocity;
        if (vl.x > MaxVl)
            vl.x = MaxVl;
        if (vl.x < -MaxVl)
            vl.x = -MaxVl;
        if (vl.y > MaxVl)
            vl.y = MaxVl;
        if (vl.y < -MaxVl)
            vl.y = -MaxVl;
        MyRig.velocity = vl;

        //

        if (CanBeDestroyed && Fire.IsFiring && Mathf.Abs(Pointer.transform.localPosition.x - transform.localPosition.x) <= Mathf.Abs(width) && Mathf.Abs(Pointer.transform.localPosition.y - transform.localPosition.y) <= Mathf.Abs(heigh))
        {
            //CameraEffects.ShakeOnce();
            
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BombArea")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaiteToAllowDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        CanBeDestroyed = true;
    }
}
