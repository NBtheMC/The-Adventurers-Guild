using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int day;
    public TimeSystem timeSystem;
    
    // public Scene currentScene;

    // public enum Scene{
    //     Menu,
    //     Questing,
    //     Instructions,
    //     Recap
    // };

    private void Awake()
    {
        //do not destroy stuff
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        timeSystem.SetDay(day);
    }

    public void ChangeScenes(string toSwitchTo){
        switch (toSwitchTo){
            case "Menu":
                SceneManager.LoadScene("Menu");
                //currentScene = Scene.Menu;
                break;
            case "Credits":
                SceneManager.LoadScene("Credits");
                break;    
            case "Instructions":
                SceneManager.LoadScene("Instructions");
                //currentScene = Scene.Instructions;
                break;
            case "Recap":
                SceneManager.LoadScene("EventRecap");
                break;
            case "Game":
                SceneManager.LoadScene("MainUIScreen");
                break;
        }
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void SetDay(int currentDay){
        this.day = currentDay;
    }
}

// public class SaveData{
        
// }
