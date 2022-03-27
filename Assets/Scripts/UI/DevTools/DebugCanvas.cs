using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    private bool debugActive; // Whether this canvas is currently being shown to the player.

	private void Awake()
	{
        debugActive = false;
	}

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
		{
            debugActive = !debugActive;
            
		}
    }
}
