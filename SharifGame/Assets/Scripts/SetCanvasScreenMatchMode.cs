using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class SetCanvasScreenMatchMode : MonoBehaviour
{
    public static List<SetCanvasScreenMatchMode> lst = new List<SetCanvasScreenMatchMode>();
    private void Awake()
    {
        lst.Add(this);
    }
    private void OnDestroy()
    {
        lst.Remove(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        cav = GetComponent<CanvasScaler>();
        SetMatch();
    }
    CanvasScaler cav;
    // Update is called once per frame
    public void SetMatch()
    {
        if (Screen.height / Screen.width > 1.7f)
        {
            cav.matchWidthOrHeight = 0;
        }
        else
            cav.matchWidthOrHeight = 1;
    }
}
