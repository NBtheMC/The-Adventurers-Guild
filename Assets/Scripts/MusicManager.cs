using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip ricercare16Music;
    public AudioClip fantasiaMusic;
    public AudioClip deliveranceMusic;
    static AudioSource audioSrc;

    void Start()
    {
        ricercare16Music = Resources.Load<AudioClip>("Ricercare");
        fantasiaMusic = Resources.Load<AudioClip>("Fantasia");
        deliveranceMusic = Resources.Load<AudioClip>("Deliverance");
        audioSrc = GetComponent<AudioSource>();
        PlayMusic();
    }

    public void StopMusic ()
    {
        audioSrc.Stop();
    }

    public void PlayMusic ()
    {
        int clip = Random.Range(0, 3);
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
            case 2: //play deliverance
                audioSrc.clip = deliveranceMusic;
                audioSrc.Play();
                break;
        }
    }
}
