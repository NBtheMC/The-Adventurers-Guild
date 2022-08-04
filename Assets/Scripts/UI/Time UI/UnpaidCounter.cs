using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpaidCounter : MonoBehaviour
{

    private WorldStateManager stateManager;
    // Start is called before the first frame update
    void Start()
    {
        stateManager = GameObject.Find("WorldState").GetComponent<WorldStateManager>();
        stateManager.IntChangeEvent += UpdateCounter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCounter(object o, string key) 
    {
        if(key == "DaysUnderPaid") 
        {
            int count = stateManager.GetWorldInt(key);
            if (count > 3)
                count = 3;
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).GetChild(1).gameObject.SetActive(false);

            for (int i = 0; i < count; i++) 
            {
                transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
                
        }
    }


}
