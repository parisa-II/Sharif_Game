using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{

    public void PlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1");
    }
}
