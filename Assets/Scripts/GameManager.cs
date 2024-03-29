using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int day;
    public TimeSystem timeSystem;
    
    public Scene currentScene;

    public enum Scene{
        Menu,
        Questing,
        Instructions,
        Recap
    };

    private void Awake()
    {
        //do not destroy stuff
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if(currentScene != Scene.Menu){
            timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();    
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if(currentScene != Scene.Menu){
            timeSystem.SetDay(day);
        }
    }

    public void ChangeScenes(string toSwitchTo){
        switch (toSwitchTo){
            case "Menu":
                SceneManager.LoadScene("Menu");
                currentScene = Scene.Menu;
                break;
            case "Credits":
                SceneManager.LoadScene("CreditsScreen");
                break;    
            case "Instructions":
                SceneManager.LoadScene("Instructions");
                currentScene = Scene.Instructions;
                break;
            case "Recap":
                SceneManager.LoadScene("Recap");
                break;
            case "Game":
                SceneManager.LoadScene("MainUIScreen");
                currentScene = Scene.Questing;
                break;
        }
    }

    public void QuitGame(){
        Debug.Log("GET ME OUT");
        Application.Quit();
    }

    public void SetDay(int currentDay){
        this.day = currentDay;
    }
}
