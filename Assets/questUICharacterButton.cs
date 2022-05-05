using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class questUICharacterButton : MonoBehaviour, IPointerClickHandler
{
    private QuestUI questUI;
    public int slot;
    public void OnPointerClick(PointerEventData eventData)
    {
        questUI = GameObject.Find("QuestDisplayManager/QuestDisplay/CurrentItemDisplay/Quest/QuestUI(Clone)").GetComponent<QuestUI>();

        if (eventData.button == PointerEventData.InputButton.Left)
        {

        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            questUI.RemoveCharacter(slot);
        }
    }
}
