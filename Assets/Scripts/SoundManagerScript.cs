using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip bellSound, bookCloseSound, bookOpenSound, bookPage1Sound, bookPage2Sound, bookPage3Sound, parchment1Sound, parchment2Sound, parchment3Sound, slipDown1Sound, slipDown2Sound, slipUp1Sound, slipUp2Sound, stampSound;
    static AudioSource audioSrc;

    void Start() 
    {
        bellSound = Audio.load<AudioClip> ("bell");
        bookCloseSound = Audio.load<AudioClip> ("bookClose");
        bookOpenSound = Audio.load<AudioClip> ("bookOpen");
        bookPage1Sound = Audio.load<AudioClip> ("bookPage1");
        bookPage2Sound = Audio.load<AudioClip> ("bookPage2");
        bookPage3Sound = Audio.load<AudioClip> ("bookPage3");
        parchment1Sound = Audio.load<AudioClip> ("parchment1");
        parchment2Sound = Audio.load<AudioClip> ("parchment2");
        parchment3Sound = Audio.load<AudioClip> ("parchment3");
        slipDown1Sound = Audio.load<AudioClip> ("slipDown1");
        slipDown2Sound = Audio.load<AudioClip> ("slipDown2");
        slipUp1Sound = Audio.load<AudioClip> ("slipUp1");
        slipUp2Sound = Audio.load<AudioClip> ("slipUp2");
        stampSound = Audio.load<AudioClip> ("stamp");
    }

    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "bell":
                audioSrc.PlayOneShot (bellSound);
                break;
            case "bookClose":
                audioSrc.PlayOneShot (bookCloseSound);
                break;
            case "bookOpen":
                audioSrc.PlayOneShot (bookOpenSound);
                break;
            case "bookPage1":
                audioSrc.PlayOneShot (bookPage1Sound);
                break;
            case "bookPage2":
                audioSrc.PlayOneShot (bookPage2Sound);
                break;
            case "bookPage3":
                audioSrc.PlayOneShot (bookPage3Sound);
                break;
            case "parchment1":
                audioSrc.PlayOneShot (parchment1Sound);
                break;
            case "parchment2":
                audioSrc.PlayOneShot (parchment2Sound);
                break;
            case "parchment3":
                audioSrc.PlayOneShot (parchment3Sound);
                break;
            case "slipDown1":
                audioSrc.PlayOneShot (slipDown2Sound);
                break;
            case "slipDown2":
                audioSrc.PlayOneShot (slipDown2Sound);
                break;
            case "slipUp1":
                audioSrc.PlayOneShot (slipUp1Sound);
                break;
            case "slipUp2":
                audioSrc.PlayOneShot (slipUp2Sound);
                break;
            case "stamp":
                audioSrc.PlayOneShot (stampSound);
                break;

        }
    }
}
