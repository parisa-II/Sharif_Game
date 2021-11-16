using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
    private Rigidbody2D MyRigid;
    private Animator BlockAnim;
    public Animations animations;
    public AudioManager audioManager;
    public GameObject Pointer;
    public float width, heigh;
    public TMPro.TMP_Text BlockCounterText;
    public Transform DestroyParticlePref;
    public Spawner spawner;
    private int BlockCounter;
    private bool IsHitting;
    private Transform DestroyParticle;
    public GameObject saver;
    public int Index;

    private Image BlockBG;

    private Color32 LowColor_1 = new Color32(204, 248, 255, 255);
    private Color32 HighColor_1 = new Color32(0, 217, 255, 255);
    private Color32 LowColor_2 = new Color32(255, 203, 225, 255);
    private Color32 HighColor_2 = new Color32(255, 50, 137, 255);
    private Color32 LowColor_3 = new Color32(244, 216, 255, 255);
    private Color32 HighColor_3 = new Color32(209, 98, 255, 255);
    private Color32 BombColor = Color.red; //new Color32(255, 94, 79, 255);
    private Color32 BallBlockColor = Color.yellow; //new Color32(255, 40, 255, 255);

    private float MaxNum = 8;
    private float MinNum = 1;
    public static float IncreaseRate = 0.5f;

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

        if(this.tag == "IncreaserBlock")
            StartCoroutine(IncreaseBlockNum());
    }


    void Update()
    {
        if (Fire.IsFiring)
            if(!IsHitting)
                StartCoroutine(StartHit());

        if (BlockCounter <= 0)
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
            LvlSceneManager.BlockCounter--;
            audioManager.PlayDestroyBlockClip();
            if (this.tag == "BallBlock")
            {
                audioManager.PlayHitBallBlockClip();
                spawner.SetBall(transform);
            }
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
        Color col;
        if (LvlSceneManager.FireRatio < 5)
            col = Color.Lerp(LowColor_1, HighColor_1, Ratio);
        else if (LvlSceneManager.FireRatio < 10)
            col = Color.Lerp(LowColor_2, HighColor_2, Ratio);
        else
            col = Color.Lerp(LowColor_3, HighColor_3, Ratio);

        BlockBG.color = col;

        if (this.tag == "Bomb")
            BlockBG.color = BombColor;
        if (this.tag == "BallBlock")
            BlockBG.color = BallBlockColor; ;
        if (this.tag == "IncreaserBlock")
            BlockBG.color = Color.grey;
    }

    IEnumerator StartHit()
    {
        IsHitting = true;
        if (Fire.IsFiring && Mathf.Abs(Pointer.transform.localPosition.x - transform.localPosition.x) <= Mathf.Abs(width) && Mathf.Abs(Pointer.transform.localPosition.y - transform.localPosition.y) <= Mathf.Abs(heigh))
        {
            if (this.tag == "Bomb")
                spawner.SetBombExplotion(transform);

            //CameraEffects.ShakeOnce();
            animations.BorderFiring();
            BlockCounter -= LvlSceneManager.FireRatio;
            if (BlockCounter < 0)
                BlockCounter = 0;
            BlockCounterText.text = BlockCounter.ToString();
            BlockAnim.Play("BlockShake Animation");
            SetColor();
            yield return new WaitForSeconds(LvlSceneManager.RateInSecond);
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
        if (collision.gameObject.tag == "BombArea")
        {
            BlockCounter = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "End")
            HitEndLine = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (this.tag == "Bomb")
            {
                spawner.SetBombExplotion(transform);
                animations.BorderFiring();
            BlockCounter -= LvlSceneManager.FireRatio;
            if (BlockCounter < 0)
                BlockCounter = 0;
            BlockCounterText.text = BlockCounter.ToString();
            BlockAnim.Play("BlockShake Animation");
            SetColor();
            }

            audioManager.PlayBallHitClip();
            animations.BorderFiring();
            BlockCounter --;
            if (BlockCounter < 0)
                BlockCounter = 0;
            BlockCounterText.text = BlockCounter.ToString();
            BlockAnim.Play("BlockShake Animation");
            SetColor();
        }

        if(this.tag == "BallBlock" && collision.gameObject.tag != "Ball")
        {
            Vector2 pos = collision.gameObject.transform.position;
            float X_Diff = Mathf.Abs(transform.position.x - pos.x);
            float Y_Diff = pos.y - transform.position.y;
            if (X_Diff < 10f && Y_Diff > 0 && Y_Diff < 140f)
            {
                this.tag = "Untagged";
                SetColor();
            }
        }
    }

    IEnumerator IncreaseBlockNum()
    {
        BlockCounter++;
        BlockCounterText.text = BlockCounter.ToString();
        yield return new WaitForSeconds(IncreaseRate);

        if(BlockCounter != 0)
            StartCoroutine(IncreaseBlockNum());
    }
}
