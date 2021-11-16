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
    public AudioClip HitBallBlockClip;
    public AudioClip FailClip;

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

    public void PlayHitBallBlockClip()
    {
        SoundsAudio.PlayOneShot(HitBallBlockClip);
    }

    public void PlayFailClip()
    {
        SoundsAudio.PlayOneShot(FailClip);
    }

    IEnumerator FireLoop()
    {
        SoundsAudio.PlayOneShot(FireClip, 0.2f);
        yield return new WaitForSeconds(LvlSceneManager.RateInSecond);

        if(IsFiring)
            StartCoroutine(FireLoop());
    }
}
