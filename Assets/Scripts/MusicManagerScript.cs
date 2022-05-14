using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    public static AudioClip ricercareMusic, fantasiaMusic;
    static AudioSource musicSrc;
    private static bool paused1, paused2;
    public static float fadeTime = 1;

    void Start()
    {
        ricercareMusic = Resources.Load<AudioClip> ("ricercare");
        fantasiaMusic = Resources.Load<AudioClip> ("fantasia");
        
        musicSrc = GetComponent<AudioSource> ();

        paused1 = paused2 = true;
    }

    public static void PlayMusic (string clip)
    {
        Debug.Log("Music played");
        switch (clip)
        {            
            case "ricercare":
                musicSrc.Play (ricercareMusic);
                paused1 = false;
                // FadeSoundUp();
                break;
            case "fantasia":
                musicSrc.Play (fantasiaMusic);
                paused2 = false;
                // FadeSoundUp();
                break;
        }
    }

    public static void StopMusic()
    {
        musicSrc.Stop();
        paused1 = paused2 = true;
        return;
    }

    public static void PauseMusic()
    {
        Debug.Log("Music paused");
        if (!paused1)
        {
            // FadeSound();
            musicSrc.Pause();
            paused1 = true;
        } else if (!paused2)
        {
            // FadeSound();
            musicSrc.Pause();
            paused2 = true;
        } else
        {
            return;
        }
    }

    // public void FadeSound()
    // {
    //     if (fadeTime == 0)
    //     {
    //         musicSrc.volume = 0;
    //         return;
    //     }
    //     StartCoroutine(_FadeSound());
    // }

    // IEnumerator _FadeSound()
    // {
    //     float t = fadeTime;
    //     while (t > 0)
    //     {
    //         yield return null;
    //         t-= Time.deltaTime;
    //         musicSrc.volume = t/fadeTime;
    //     }
    //     yield break;
    // }

    // public void FadeSoundUp()
    // {
    //     if (fadeTime == 0)
    //     {
    //         musicSrc.volume = 0;
    //         return;
    //     }
    //     StartCoroutine(_FadeSound());
    // }

    // IEnumerator _FadeSoundUp()
    // {
    //     float t = 0;
    //     while (t > fadeTime)
    //     {
    //         yield return null;
    //         t+= Time.deltaTime;
    //         musicSrc.volume = t/fadeTime;
    //     }
    //     yield break;
    // }
}