using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    private Text characterName;
    private Text combat;
    private Text exploration;
    private Text diplomacy;
    private Text stamina;
    // Start is called before the first frame update
    void Awake()
    {
        characterName = transform.Find("Name").gameObject.GetComponent<Text>();
        combat = transform.Find("Stats/Combat").gameObject.GetComponent<Text>();
        exploration = transform.Find("Stats/Exploration").gameObject.GetComponent<Text>();
        diplomacy = transform.Find("Stats/Diplomacy").gameObject.GetComponent<Text>();
        stamina = transform.Find("Stats/Stamina").gameObject.GetComponent<Text>();
    }

    public void SetupCharacterInfoUI(CharacterSheet characterSheet)
    {
        characterName.text = characterSheet.name;
        combat.text += characterSheet.getStat("combat");
        exploration.text += characterSheet.getStat("exploration");
        diplomacy.text += characterSheet.getStat("diplomacy");
        stamina.text += characterSheet.getStat("stamina");
    }

    public void DestroyUI()
    {
        Destroy(this.gameObject);
    }
}
