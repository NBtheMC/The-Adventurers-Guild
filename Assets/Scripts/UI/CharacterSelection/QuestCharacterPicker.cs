using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to a Text Mesh Pro Field.
/// </summary>
public class QuestCharacterPicker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private CharacterSheetManager charSheetManager; // The charactermanager in the game.
    private RectTransform transformer; // this object's rect transform
    private bool overSpace;
    private int adventurernumber; // The coded adventurer number from that master sheet that was last inputed.
    public CharacterSheet codedAdventurer { get; private set; } // So other scripts can take a look at what adventurer this was.

    void Awake()
	{
		transformer = this.GetComponent<RectTransform>();
        adventurernumber = 0;
		overSpace = false;
        codedAdventurer = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find the CharacterSheetManager
        charSheetManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transformer.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        overSpace = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transformer.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
        overSpace=false;
    }

    void Update()
	{
        if (overSpace== true)
		{
            // Checks for what key was just last pressed to corrospond with the code last done.
            int oldNumber = adventurernumber;
            if (Input.GetKeyDown(KeyCode.Alpha1)) { adventurernumber = 1; }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) { adventurernumber = 2; }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) { adventurernumber = 3; }
            else if (Input.GetKeyDown(KeyCode.Alpha4)) { adventurernumber = 4; }
            else if (Input.GetKeyDown(KeyCode.Alpha5)) { adventurernumber = 5; }
            else if (Input.GetKeyDown(KeyCode.Alpha6)) { adventurernumber = 6; }
            else if (Input.GetKeyDown(KeyCode.Alpha7)) { adventurernumber = 7; }
            else if (Input.GetKeyDown(KeyCode.Alpha8)) { adventurernumber = 8; }
            else if (Input.GetKeyDown(KeyCode.Alpha9)) { adventurernumber = 9; }
            else if (Input.GetKeyDown(KeyCode.Alpha0)) { adventurernumber = 10; }
            else if (Input.GetKeyDown(KeyCode.Minus)) { adventurernumber = 11; }
            else if (Input.GetKeyDown(KeyCode.Plus)) { adventurernumber = 12; }

            codedAdventurer = charSheetManager.GetCharacterSheetFromIndex(adventurernumber);
            if (codedAdventurer != null) { this.GetComponent<TMPro.TextMeshProUGUI>().text = codedAdventurer.name; }
            else { adventurernumber = oldNumber; }

        }
	}
}
