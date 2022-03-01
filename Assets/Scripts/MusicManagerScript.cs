using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    public static AudioClip ricercare16Music;
    static AudioSource audioSrc;

    void Start()
    {
        ricercare16Music = Resources.Load<AudioClip> ("ricercarePre");
        
        audioSrc = GetComponent<AudioSource> ();
        PlayMusic ("ricercare16");
    }

    void Update()
    {        

    }

    public static void PlayMusic (string clip)
    {
        switch (clip)
        {            
            case "ricercare16":
                audioSrc.PlayOneShot (ricercare16Music);
                break;
        }
    }
}
