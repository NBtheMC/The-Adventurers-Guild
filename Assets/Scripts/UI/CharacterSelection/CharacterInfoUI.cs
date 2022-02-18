using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterInfoUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public Text characterName {get; private set;}
    private Text combat;
    private Text exploration;
    private Text diplomacy;
    private Text stamina;
    private RectTransform transformer; // defines the rectangle reference for this dragger.
    [HideInInspector] public GameObject charObject;

    // Start is called before the first frame update
    void Awake()
    {
        characterName = transform.Find("Name").gameObject.GetComponent<Text>();
        combat = transform.Find("Stats/Combat").gameObject.GetComponent<Text>();
        exploration = transform.Find("Stats/Exploration").gameObject.GetComponent<Text>();
        diplomacy = transform.Find("Stats/Diplomacy").gameObject.GetComponent<Text>();
        stamina = transform.Find("Stats/Stamina").gameObject.GetComponent<Text>();

        transformer = this.GetComponent<RectTransform>();
    }

    public void SetupCharacterInfoUI(CharacterSheet characterSheet)
    {
        characterName.text = characterSheet.name;
        combat.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Combat).ToString();
        exploration.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Exploration).ToString();
        diplomacy.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Exploration).ToString();
        stamina.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Exploration).ToString();
    }

    public void DestroyUI()
    {
        if(charObject)
        {
            CharacterInfoDisplay infoDisplay = charObject.GetComponent<CharacterInfoDisplay>();
            infoDisplay.isDisplayed = false;
            infoDisplay.CharacterInfo = null;
        }
            
        Destroy(this.gameObject);
    }

    /// <summary>
    /// For when this UI object is being dragged.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        transformer.position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        this.transform.SetAsLastSibling();
    }
}
