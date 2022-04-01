using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //public GameObject WorldState; //parent of all worldstates
    
    // public Scene currentScene;

    // public enum Scene{
    //     Menu,
    //     Questing,
    //     Instructions,
    //     Recap
    // };

    private void Awake()
    {
        // if (Instance != null)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        // Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

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

    // public static void Save(SaveData saveData){
    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.OpenOrCreate);
    //     //SaveData saveData = new SaveData (); not needed as the object is being passed
    //     bf.Serialize(file, saveData);
    //     file.Close();
    // }

}

// public class SaveData{
        
// }
