using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterInfoUI : MonoBehaviour
{
    public Text characterName {get; private set;}
    private Text combat;
    private Text exploration;
    private Text diplomacy;
    private Text stamina;
    private Text biography;
    private Image portrait;

    [HideInInspector] public GameObject charObject;
    public CharacterSheet charSheet {get; private set;}

    // Start is called before the first frame update
    void Awake()
    {
        characterName = transform.Find("Name").gameObject.GetComponent<Text>();
        biography = transform.Find("Bio/Text").gameObject.GetComponent<Text>();
        portrait = transform.Find("Mask/Portrait").gameObject.GetComponent<Image>();
        combat = transform.Find("Stats/Combat").gameObject.GetComponent<Text>();
        exploration = transform.Find("Stats/Exploration").gameObject.GetComponent<Text>();
        diplomacy = transform.Find("Stats/Diplomacy").gameObject.GetComponent<Text>();
        stamina = transform.Find("Stats/Stamina").gameObject.GetComponent<Text>();
    }

    public void SetupCharacterInfoUI(CharacterSheet characterSheet)
    {
        charSheet = characterSheet;
        characterName.text = characterSheet.name;
        biography.text = characterSheet.biography;
        portrait.sprite = characterSheet.portrait;
        combat.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Combat).ToString();
        exploration.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Exploration).ToString();
        diplomacy.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Negotiation).ToString();
        stamina.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Constitution).ToString();
    }

    
    /// <summary>
    /// Hook this up to an event item from Character Tile Controller
    /// Destroys this UI.
    /// </summary>
    public void DestroyUI()
    {
        Destroy(this.gameObject);
        GameObject.Find("PlayerInterface").GetComponent<PlayerInterface>().SetItemDisplayNone();
    }
}
