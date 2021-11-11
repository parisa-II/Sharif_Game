using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fire : MonoBehaviour
{
    public static bool IsFiring;
    public static int HitedBlock;
    public static bool HoldFire;

    public GameObject Pointer;
    public FixedJoystick Joystick;
    public Animations animations;
    public Rigidbody2D PointerRig;
    public AudioManager audioManager;

    private float Speed = 3.5f; // 0.035f // 2.2f //400f
    private float Heigh;
    private float Width;
    private float MoveRatio = 1.6f;

    private bool CanMove;

    private Vector2 FirstTouchPos;
    private Vector2 FirstPointerPos;
    private Vector2 DiffPos;
    private Vector2 JoyStickDiffPos;

    void Start()
    {
        CanMove = false;
        IsFiring = false;
        HoldFire = false;
        Heigh = Screen.height/2;
        Width = Screen.width/2;
        HitedBlock = 0;
    }

    
    void Update()
    {
        
        if (Joystick.Horizontal != 0 || Joystick.Vertical != 0)
            CanMove = true;
        else
            CanMove = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                HoldFire = true;
                StartFiring();
                FirstTouchPos = touch.position;
                FirstPointerPos = Pointer.transform.localPosition;
                DiffPos = FirstPointerPos - FirstTouchPos;
                JoyStickDiffPos = FirstTouchPos;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                StopFiring();
                HoldFire = false;
            }
            else if(CanMove)
            {
                JoyStickDiffPos = touch.position - JoyStickDiffPos;
                JoyStickDiffPos = JoyStickDiffPos * MoveRatio;
                Pointer.transform.localPosition += new Vector3(JoyStickDiffPos.x, JoyStickDiffPos.y , 0f);
                JoyStickDiffPos = touch.position;
            }
        }
    }

    private void StartFiring()
    {
        if (CanMove)
        {
            IsFiring = true;
            animations.PointerFiring();
            audioManager.PlayFireClip();
        }
    }
    private void StopFiring()
    {
        IsFiring = false;
        animations.PointerIdle();
        audioManager.StopFireClip();
    }
}
