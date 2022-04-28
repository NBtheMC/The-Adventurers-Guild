using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvasController : MonoBehaviour
{
	private bool firstStart = true;
    private bool currentlyactive = false;

	// Update is called once per frame
	void Update()
    {
		if (firstStart) { setChildren(currentlyactive); firstStart = false; }

        if (Input.GetKeyUp(KeyCode.BackQuote)){
            currentlyactive = !currentlyactive; // reverses current course.
            setChildren(currentlyactive); // Set all the children to the correct state.
		}
    }

    private void setChildren(bool state)
	{
        foreach(Transform child in transform)
		{
            child.gameObject.SetActive(state);
		}
	}
}
