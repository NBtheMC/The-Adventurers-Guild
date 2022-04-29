using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplayManager : MonoBehaviour
{
    public GameObject questDisplay;
    public GameObject characterDisplay;
    public GameObject debriefDisplay;

    public GameObject lastActiveDisplay = null;
    // Start is called before the first frame update
    void Start()
    {
        characterDisplay.SetActive(false);
        questDisplay.SetActive(false);
        debriefDisplay.SetActive(false);
    }

    public void DisplayCharacter(bool display)
    {
        if (display)
            SetLastActiveDisplay();

        characterDisplay.SetActive(display);
        questDisplay.SetActive(false);
        debriefDisplay.SetActive(false);

        if (!display)
            DisplayLastActive(lastActiveDisplay);
    }
    public void DisplayQuest(bool display)
    {
        if (display)
            SetLastActiveDisplay();

        characterDisplay.SetActive(false);
        questDisplay.SetActive(display);
        debriefDisplay.SetActive(false);

        if (!display)
            DisplayLastActive(lastActiveDisplay);
    }

    public void DisplayDebrief(bool display)
    {
        if (display)
            SetLastActiveDisplay();

        characterDisplay.SetActive(false);
        questDisplay.SetActive(false);
        debriefDisplay.SetActive(display);

        if (!display)
            DisplayLastActive(lastActiveDisplay);
    }

    private void SetLastActiveDisplay()
    {
        if (questDisplay.activeSelf)
            lastActiveDisplay = questDisplay;
        else if (characterDisplay.activeSelf)
            lastActiveDisplay = characterDisplay;
        else if (debriefDisplay.activeSelf)
            lastActiveDisplay = debriefDisplay;
    }

    private void DisplayLastActive(GameObject g)
    {
        if(g != null)
            g.SetActive(true);
        lastActiveDisplay = null;
    }
}
