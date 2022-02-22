using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBookManager : MonoBehaviour
{
    private GameObject QuestDisplay;
    private GameObject CharInfoUIPrefab;
    private GameObject CharInfoSpawn;
    private List<GameObject> adventurers;
    private GameObject activeObject;
    private int displayIndex = 0;
    // Start is called before the first frame update

    void Awake()
    {
        CharInfoUIPrefab = Resources.Load<GameObject>("CharacterInfoUI");
        CharInfoSpawn = GameObject.Find("CharInfoSpawn");
        QuestDisplay = GameObject.Find("QuestDisplay");
    }
    void Start()
    {
        CharacterSheetManager charSheetManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
        adventurers = new List<GameObject>();

        foreach (CharacterSheet character in charSheetManager.FreeAdventurers)
        {
            AddCharacter(character);
        }
        adventurers[0].SetActive(true);
        activeObject = adventurers[0];
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = activeObject.transform.position;
    }

    public void DisplayAdjacent(int i)
    {
        adventurers[displayIndex].SetActive(false);

        displayIndex += i;
        if (displayIndex == -1)
            displayIndex = adventurers.Count - 1;
        else if (displayIndex == adventurers.Count)
            displayIndex = 0;

        adventurers[displayIndex].SetActive(true);
        adventurers[displayIndex].transform.position = transform.position;

        activeObject = adventurers[displayIndex];
        activeObject.transform.SetAsLastSibling();
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
                activeObject.transform.SetAsLastSibling();
            }
        }
    }

    public void AddCharacter(CharacterSheet character)
    {
        GameObject CharInfoUIObject = Instantiate(CharInfoUIPrefab);
        CharInfoUIObject.transform.SetParent(QuestDisplay.transform, false);
        CharInfoUIObject.GetComponent<RectTransform>().anchoredPosition = CharInfoSpawn.transform.localPosition;
        CharInfoUIObject.transform.SetAsLastSibling();
        CharInfoUIObject.SetActive(false);
        CharInfoUIObject.transform.Find("Next").GetComponent<Button>().onClick.AddListener(delegate { DisplayAdjacent(1); });
        CharInfoUIObject.transform.Find("Prev").GetComponent<Button>().onClick.AddListener(delegate { DisplayAdjacent(-1); });

        CharacterInfoUI characterInfoUI = CharInfoUIObject.GetComponent<CharacterInfoUI>();
        characterInfoUI.SetupCharacterInfoUI(character);

        adventurers.Add(CharInfoUIObject);
    }
}
