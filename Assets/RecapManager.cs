using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapManager : MonoBehaviour
{
    private TimeSystem timeSystem;
    // Start is called before the first frame update
    void Start()
    {
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Go into recap mode
    public void StartRecap(){
        transform.Find("Filter").gameObject.SetActive(true);
        timeSystem.StopTimer();
    }

    //Get out of recap mode
    public void EndRecap(){
        transform.Find("Filter").gameObject.SetActive(false);
        timeSystem.StartTimer();
    }
}
