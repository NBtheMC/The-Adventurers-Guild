using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterBookManager : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private GameObject QuestDisplay;
    private GameObject CharInfoUIPrefab;
    private GameObject CharInfoSpawn;
    private List<GameObject> adventurers;
    private int displayIndex = 0;
    // Start is called before the first frame update

    void Awake()
    {
        CharInfoUIPrefab = Resources.Load<GameObject>("CharacterInfoUI");
        CharInfoSpawn = GameObject.Find("CharInfoBook");
        QuestDisplay = GameObject.Find("QuestDisplay");
    }
    void Start()
    {
        CharacterSheetManager charSheetManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
        adventurers = new List<GameObject>();
        

        //add active character pages to book
        foreach (CharacterSheet character in charSheetManager.FreeAdventurers)
        {
            AddCharacter(character);
        }
        //adventurers[0].SetActive(true);
    }

    public void DisplayCharacter(CharacterSheet character)
    {
        for (int i = 0; i < adventurers.Count; i++)
        {
            if (adventurers[i].GetComponent<CharacterInfoUI>().charSheet != character)
                adventurers[i].SetActive(false);
            else
            {
                adventurers[i].SetActive(true);
                displayIndex = i;
                adventurers[displayIndex].transform.position = transform.position;
            }
        }

        this.transform.SetAsLastSibling();
    }

    public void AddCharacter(CharacterSheet character)
    {
        GameObject CharInfoUIObject = Instantiate(CharInfoUIPrefab);
        CharInfoUIObject.transform.SetParent(this.transform, false);
        CharInfoUIObject.transform.SetAsFirstSibling();
        CharInfoUIObject.SetActive(false);

        CharacterInfoUI characterInfoUI = CharInfoUIObject.GetComponent<CharacterInfoUI>();
        characterInfoUI.SetupCharacterInfoUI(character);

        //Add character portrait
        if (character.portrait != null)
        {
            //CharInfoUIObject.AddComponent<Image>().sprite = character.portrait;
            CharInfoUIObject.transform.Find("PortraitFrame").Find("Portrait").GetComponent<Image>().sprite = character.portrait;
        }

        adventurers.Add(CharInfoUIObject);
    }


    /// <summary>
    /// For when this UI object is being dragged.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        pointerEventData.useDragThreshold = false;
        this.transform.SetAsLastSibling();
    }
}
