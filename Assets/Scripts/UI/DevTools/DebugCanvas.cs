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
        // Checks for the key to turn objects on or off in this canvas.
        if (Input.GetKeyDown(KeyCode.BackQuote))
		{
            debugActive = !debugActive;
            if (debugActive)
			{
                foreach (Transform t in transform)
				{
                    // set all objects active.
                    if (!t.gameObject.activeSelf) { t.gameObject.SetActive(true); }
				}
			}
            else if (!debugActive)
			{
                foreach(Transform t in transform)
				{
                    // set all objects inactive.
                    if (t.gameObject.activeSelf) { t.gameObject.SetActive(false); }
				}
			}
		}
    }
}
