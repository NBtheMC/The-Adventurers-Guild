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
    private GameObject activeObject;
    private GameObject pageIndicator;
    private GameObject indicator;
    private int displayIndex = 0;
    // Start is called before the first frame update

    void Awake()
    {
        CharInfoUIPrefab = Resources.Load<GameObject>("CharacterInfoUI");
        CharInfoSpawn = GameObject.Find("CharInfoBook");
        QuestDisplay = GameObject.Find("QuestDisplay");
        pageIndicator = CharInfoSpawn.transform.Find("PageIndicator").gameObject;
        indicator = Resources.Load<GameObject>("Dot");
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
        adventurers[0].SetActive(true);
        activeObject = adventurers[0];
    }

    public void DisplayNext()
    {
        adventurers[displayIndex].SetActive(false);

        displayIndex++;
        if (displayIndex == adventurers.Count)
            displayIndex = 0;

        adventurers[displayIndex].SetActive(true);
        adventurers[displayIndex].transform.position = transform.position;

        activeObject = adventurers[displayIndex];
        SetActiveIndicator();

        var i = UnityEngine.Random.Range(0, 3);
        if (i == 0) {SoundManagerScript.PlaySound("parchment1");}
        else if (i == 1) {SoundManagerScript.PlaySound("parchment2");}
        else {SoundManagerScript.PlaySound("parchment3");}
    }
    public void DisplayPrev()
    {
        adventurers[displayIndex].SetActive(false);

        displayIndex--;
        if (displayIndex == -1)
            displayIndex = adventurers.Count - 1;

        adventurers[displayIndex].SetActive(true);
        adventurers[displayIndex].transform.position = transform.position;

        activeObject = adventurers[displayIndex];
        SetActiveIndicator();

        var i = UnityEngine.Random.Range(0, 3);
        if (i == 0) {SoundManagerScript.PlaySound("parchment1");}
        else if (i == 1) {SoundManagerScript.PlaySound("parchment2");}
        else {SoundManagerScript.PlaySound("parchment3");}
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
                activeObject = adventurers[displayIndex];
            }
        }
        SetActiveIndicator();
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
        
        GameObject dot = Instantiate(indicator);
        dot.transform.SetParent(pageIndicator.transform, false);
        SetPageIndicatorPositions();
        SetActiveIndicator();
    }

    public void SetPageIndicatorPositions()
    {
        RectTransform rt = pageIndicator.GetComponent<RectTransform>();
        float spaces = rt.rect.width / (adventurers.Count + 1);
        for(int i = 0; i < adventurers.Count; i++)
        {
            pageIndicator.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector3(spaces * (i+1), 0, 0);
        }
    }

    public void SetActiveIndicator()
    {
        for(int i = 0; i < pageIndicator.transform.childCount; i++)
        {
            Transform child = pageIndicator.transform.GetChild(i);
            if(i != displayIndex)
                child.GetChild(1).gameObject.SetActive(false);
            else
                child.GetChild(1).gameObject.SetActive(true);
        }
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
        this.transform.SetAsLastSibling();
    }
}
