using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip bellSound, bookCloseSound, bookOpenSound, bookPage1Sound, bookPage2Sound, bookPage3Sound, parchment1Sound, parchment2Sound, parchment3Sound, slipDown1Sound, slipDown2Sound, slipUp1Sound, slipUp2Sound, stampSound;
    static AudioSource audioSrc;

    void Start() 
    {
        bellSound = Resources.Load<AudioClip> ("bell");
        bookCloseSound = Resources.Load<AudioClip> ("bookClose");
        bookOpenSound = Resources.Load<AudioClip> ("bookOpen");
        bookPage1Sound = Resources.Load<AudioClip> ("bookPage1");
        bookPage2Sound = Resources.Load<AudioClip> ("bookPage2");
        bookPage3Sound = Resources.Load<AudioClip> ("bookPage3");
        parchment1Sound = Resources.Load<AudioClip> ("parchment1");
        parchment2Sound = Resources.Load<AudioClip> ("parchment2");
        parchment3Sound = Resources.Load<AudioClip> ("parchment3");
        slipDown1Sound = Resources.Load<AudioClip> ("slipDown1");
        slipDown2Sound = Resources.Load<AudioClip> ("slipDown2");
        slipUp1Sound = Resources.Load<AudioClip> ("slipUp1");
        slipUp2Sound = Resources.Load<AudioClip> ("slipUp2");
        stampSound = Resources.Load<AudioClip> ("stamp");

        audioSrc = GetComponent<AudioSource> ();
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
                audioSrc.PlayOneShot (slipDown1Sound);
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
