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

                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit))
                //{
                //    print(hit.transform.name);
                //    //Select stage    
                //    if (hit.transform.name == "Handle")
                //    {
                //        StartFiring();
                //        FirstTouchPos = touch.position;
                //        FirstPointerPos = Pointer.transform.localPosition;
                //        DiffPos = FirstPointerPos - FirstTouchPos;
                //    }
                //}
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                StopFiring();
                HoldFire = false;
            }
            else if(CanMove)// if(CanMove)
            {
                JoyStickDiffPos = touch.position - JoyStickDiffPos;
                JoyStickDiffPos = JoyStickDiffPos * MoveRatio;
                Pointer.transform.localPosition += new Vector3(JoyStickDiffPos.x, JoyStickDiffPos.y , 0f);
                JoyStickDiffPos = touch.position;
            }
        }


        //Pointer.transform.localPosition = new Vector3(Joystick.Horizontal * Speed, 450 + Joystick.Vertical * Speed, 0f);
        //PointerRig.velocity = new Vector3(Joystick.Horizontal * Speed, Joystick.Vertical * Speed, 0f);




        //float sign_x = Mathf.Abs(Pointer.transform.localPosition.x) / Pointer.transform.localPosition.x;
        //float sign_y = Mathf.Abs(Pointer.transform.localPosition.y) / Pointer.transform.localPosition.y;

        //if (Pointer.transform.localPosition.x >= Width)
        //{
        //    PointerRig.velocity = new Vector3(Joystick.Horizontal * Speed * -1f, PointerRig.velocity.y, 0f);
        //    Pointer.transform.localPosition = new Vector3(Pointer.transform.localPosition.x - 0.1f * sign_x , Pointer.transform.localPosition.y, 0f);
        //}
        //else
        //    PointerRig.velocity = new Vector3(Joystick.Horizontal * Speed, PointerRig.velocity.y, 0f);

        //if (Mathf.Abs(Pointer.transform.localPosition.y) >= Heigh)
        //{
        //    PointerRig.velocity = new Vector3(PointerRig.velocity.x, Joystick.Vertical * Speed * -1f, 0f);
        //    Pointer.transform.localPosition = new Vector3(Pointer.transform.localPosition.x, Pointer.transform.localPosition.y - 0.1f * sign_y, 0f);
        //}
        //else
        //    PointerRig.velocity = new Vector3(PointerRig.velocity.x, Joystick.Vertical * Speed, 0f);

    }

    private void StartFiring()
    {
        if (CanMove)
        {
            IsFiring = true;
            animations.PointerFiring();
        }
    }
    private void StopFiring()
    {
        IsFiring = false;
        animations.PointerIdle();
    }
}
