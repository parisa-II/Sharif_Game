using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlSceneManager : MonoBehaviour
{
    public static bool IsGameOver;

    public Sprite[] PauseBtnSprites;
    public Button PauseBtn;
    public TMPro.TMP_Text ScoreText;
    public TMPro.TMP_Text RecordText;
    public TMPro.TMP_Text TotalScore;
    public GameObject GameoverPanel;

    private bool IsPaused;

    void Start()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        IsPaused = false;

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
}
