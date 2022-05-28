using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestUICharacterButton : MonoBehaviour, IPointerClickHandler
{
    public QuestUI questUI; // Set by QuestUI when this item is instantiated.
    public int slot;
    public bool active;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(!active){ questUI.RemoveCharacter(slot); ClearOut(); }
        }
    }

    /// <summary>
    /// Slot a character in here.
    /// </summary>
    /// <param name="character"></param>
    public void SlotIn(CharacterSheet character)
	{
        this.transform.Find("Portrait").GetComponent<Image>().sprite = character.portrait; // Set the sprite image
        this.transform.Find("Name").gameObject.GetComponent<Text>().text = character.name; // Set the name.
        this.transform.Find("Portrait").gameObject.SetActive(true); // Show the image.
        this.transform.Find("EmptyCharacter").gameObject.SetActive(false); // Get rid of the blank image
        this.transform.Find("FilledCharacter").gameObject.SetActive(true); // Show the filled character.
        this.transform.Find("Name").gameObject.SetActive(true); // Show the name.
    }

    /// <summary>
    /// Clear out any character that was in here.
    /// </summary>
    public void ClearOut()
	{
        this.transform.Find("Portrait").gameObject.SetActive(false); // Hide the portrait
        this.transform.Find("FilledCharacter").gameObject.SetActive(false); // Hide the filled character.
        this.transform.Find("Name").gameObject.SetActive(false); // Hide the name.
        this.transform.Find("EmptyCharacter").gameObject.SetActive(true); // Show the blank image
    }
}
