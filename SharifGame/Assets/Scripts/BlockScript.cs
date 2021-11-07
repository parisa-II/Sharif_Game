using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
    private Rigidbody2D MyRigid;
    private Animator BlockAnim;
    public GameObject Pointer;
    public float width, heigh;
    public TMPro.TMP_Text BlockCounterText;
    public Transform DestroyParticlePref;
    public Spawner spawner;
    private int BlockCounter;
    private float RateInSecond = 0.1f;
    private bool IsHitting;
    private Transform DestroyParticle;
    public GameObject saver;
    public int Index;

    private Image BlockBG;

    private Color32 LowColor = new Color32(210, 203, 235, 255);
    private Color32 HighColor = new Color32(149, 133, 208, 255);

    private float MaxNum = 8;
    private float MinNum = 1;

    private bool HitEndLine;

    private void Awake()
    {
        BlockAnim = GetComponent<Animator>();
        BlockBG = GetComponent<Image>();
        MyRigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //DestroyParticle = Instantiate(DestroyParticlePref, Camera.main.ScreenToWorldPoint(transform.localPosition), DestroyParticlePref.rotation);
        //DestroyParticle.SetParent(transform, false);
        //DestroyParticle.gameObject.SetActive(true);

        IsHitting = false;
        HitEndLine = false;
        BlockCounter = int.Parse(BlockCounterText.text);
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Fire.IsFiring)
            if(!IsHitting)
                StartCoroutine(StartHit());

        if (BlockCounter == 0)
        {
            DestroyParticle = Instantiate(DestroyParticlePref, Camera.main.ScreenToWorldPoint(transform.localPosition), DestroyParticlePref.rotation);            //DestroyParticle.SetParent(transform, false);
            DestroyParticle.SetParent(transform, false);
            DestroyParticle.SetParent(saver.transform, true);
            DestroyParticle.gameObject.SetActive(true);
            Destroy(DestroyParticle.gameObject, 2f);
            //spawner.CreateNewBlock(Index);
            //CameraEffects.ShakeOnce();
            Fire.HitedBlock++;
            if (PlayerPrefs.GetInt("Record") < Fire.HitedBlock)
                PlayerPrefs.SetInt("Record", Fire.HitedBlock);
            PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 1);
            Destroy(gameObject);
        }

        if (MyRigid.velocity.y >= 0f)
        {
            if(HitEndLine)
                LvlSceneManager.IsGameOver = true;
        }
    }

    private void SetColor()
    {
        float Ratio = BlockCounter / (70 - MinNum); //MaxNum
        Color col = Color.Lerp(LowColor, HighColor, Ratio);
        BlockBG.color = col;
    }

    IEnumerator StartHit()
    {
        IsHitting = true;
        if (Fire.IsFiring && Mathf.Abs(Pointer.transform.localPosition.x - transform.localPosition.x) <= Mathf.Abs(width) && Mathf.Abs(Pointer.transform.localPosition.y - transform.localPosition.y) <= Mathf.Abs(heigh))
        {
            CameraEffects.ShakeOnce();
            BlockCounter--;
            BlockCounterText.text = BlockCounter.ToString();
            BlockAnim.Play("BlockShake Animation");
            SetColor();
            yield return new WaitForSeconds(RateInSecond);
            StartCoroutine(StartHit());
        }
        else
        {
            IsHitting = false;
            BlockAnim.Play("BlockIdle Animation");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "End")
            HitEndLine = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "End")
            HitEndLine = false;
    }
}
