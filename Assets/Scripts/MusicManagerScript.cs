using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    public AudioClip ricercare16Music;
    public AudioClip fantasiaMusic;
    static AudioSource audioSrc;

    void Start()
    {
        ricercare16Music = Resources.Load<AudioClip> ("ricercarePre");
        fantasiaMusic = Resources.Load<AudioClip> ("fantasiaPre");
        audioSrc = GetComponent<AudioSource> ();
        PlayMusic("ricercare16");
    }

    public void StopMusic ()
    {
        audioSrc.Stop();
    }

    public void PlayMusic (string clip)
    {
        switch (clip)
        {            
            case "ricercare16":
                audioSrc.clip = ricercare16Music;
                audioSrc.Play();
                break;
            case "fantasia":
                audioSrc.clip = fantasiaMusic;
                audioSrc.Play();
                break;
        }
    }
}
