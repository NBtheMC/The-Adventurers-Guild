using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum Scene{
        Menu,
        Questing,
        Recap
    };

    void Awake(){
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScenes(string toSwitchTo){
        switch (toSwitchTo){
            case "Menu":
                break;
            case "Questing":
                SceneManager.LoadScene("MainUIScreen");
                break;
            case "Recap":
                break;
        }
    }
}
