using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    public static AudioClip ricercare16Music;
    public static AudioClip secondSong;
    static AudioSource audioSrc;

    void Start()
    {
        ricercare16Music = Resources.Load<AudioClip> ("ricercarePre");
        //assign second song here
        audioSrc = GetComponent<AudioSource> ();
        PlayMusic("ricercare16");
    }

    void Update()
    {        

    }

    public static void PlayMusic (string clip)
    {
        switch (clip)
        {            
            case "ricercare16":
                audioSrc.clip = ricercare16Music;
                audioSrc.Play();
                break;
            case "secondSong":
                audioSrc.clip = secondSong;
                audioSrc.Play();
                break;
        }
    }
}
