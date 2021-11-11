using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlSceneManager : MonoBehaviour
{
    public static bool IsGameOver;
    public static int BlockCounter;
    public static float RateInSecond = 0.1f;
    public static bool StartNewWave;
    public static bool EndOfWave;
    public static int FireRatio;

    public Sprite[] PauseBtnSprites;
    public Button PauseBtn;
    public TMPro.TMP_Text ScoreText;
    public TMPro.TMP_Text RecordText;
    public TMPro.TMP_Text TotalScore;
    public TMPro.TMP_Text FireRatioText;
    public GameObject GameoverPanel;
    public Spawner spawner;
    public GameObject NextWaveText;
    public AudioManager audioManager;

    private bool IsPaused;

    void Start()
    {
        BlockCounter = 0;
        Time.timeScale = 1f;
        FireRatio = 1;
        IsGameOver = false;
        IsPaused = false;
        StartNewWave = false;
        EndOfWave = false;

        if (!PlayerPrefs.HasKey("Record"))
            PlayerPrefs.SetInt("Record", 0);
        RecordText.text = "Record : " + PlayerPrefs.GetInt("Record");

        if (!PlayerPrefs.HasKey("TotalScore"))
            PlayerPrefs.SetInt("TotalScore", 0);
        TotalScore.text = PlayerPrefs.GetInt("TotalScore").ToString();
    }

    private void Update()
    {
        ScoreText.text = "Score : " + Fire.HitedBlock;
        TotalScore.text = PlayerPrefs.GetInt("TotalScore").ToString();

        if (IsGameOver)
        {
            Time.timeScale = 0f;
            GameoverPanel.SetActive(true);
        }

        if(StartNewWave && BlockCounter == 0)
        {
            StartNewWave = false;
            EndOfWave = true;
            //
            StartCoroutine(WaitToStartNewWave());
        }
    }

    public void HomeButton()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void PauseButton()
    {
        if(IsPaused)
        {
            IsPaused = false;
            PauseBtn.image.sprite = PauseBtnSprites[0];
            Time.timeScale = 1;
        }
        else
        {
            IsPaused = true;
            PauseBtn.image.sprite = PauseBtnSprites[1];
            Time.timeScale = 0;
        }
    }

    IEnumerator WaitToStartNewWave()
    {
        NextWaveText.SetActive(true);

        yield return new WaitForSeconds(3f);
        NextWaveText.SetActive(false);
        FireRatio++;
        FireRatioText.text = "X " + FireRatio;
        audioManager.PlayIncreaseFireRatio();
        //particle
        //change fire audio

        yield return new WaitForSeconds(1f);
        spawner.CreateNewBlock();
        EndOfWave = false;
    }
}
