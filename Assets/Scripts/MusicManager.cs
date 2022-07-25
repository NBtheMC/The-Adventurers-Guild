using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip ricercare16Music;
    public AudioClip fantasiaMusic;
    static AudioSource audioSrc;

    void Start()
    {
        ricercare16Music = Resources.Load<AudioClip> ("ricercarePre");
        fantasiaMusic = Resources.Load<AudioClip> ("fantasiaPre");
        audioSrc = GetComponent<AudioSource> ();
        PlayMusic();
    }

    public void StopMusic ()
    {
        audioSrc.Stop();
    }

    public void PlayMusic ()
    {
        int clip = Random.Range(0, 2);
        switch (clip)
        {            
            case 0: //play ricercare
                audioSrc.clip = ricercare16Music;
                audioSrc.Play();
                break;
            case 1: //play fantasia
                audioSrc.clip = fantasiaMusic;
                audioSrc.Play();
                break;
        }
    }

    public void ChangeVolume(System.Single vol)
    {
        audioSrc.volume = vol;
    }

    public float GetVolume()
    {
        return audioSrc.volume;
    }
}
