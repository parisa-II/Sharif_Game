using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource SoundsAudio;

    public AudioClip FireClip;
    public AudioClip DestroyBlockClip;
    public AudioClip IncreaseFireRatio;
    public AudioClip BallHitClip;

    private bool IsFiring;

    private void Start()
    {
        IsFiring = false;
    }

    public void PlayDestroyBlockClip()
    {
        SoundsAudio.PlayOneShot(DestroyBlockClip);
    }

    public void PlayFireClip()
    {
        IsFiring = true;
        StartCoroutine(FireLoop());
    }
    public void StopFireClip()
    {
        IsFiring = false;
    }

    public void PlayIncreaseFireRatio()
    {
        SoundsAudio.PlayOneShot(IncreaseFireRatio);
    }

    public void PlayBallHitClip()
    {
        SoundsAudio.PlayOneShot(BallHitClip);
    }

    IEnumerator FireLoop()
    {
        SoundsAudio.PlayOneShot(FireClip);
        yield return new WaitForSeconds(LvlSceneManager.RateInSecond);

        if(IsFiring)
            StartCoroutine(FireLoop());
    }
}
