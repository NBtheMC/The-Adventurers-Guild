using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to a Text Mesh Pro Field.
/// </summary>
public class QuestCharacterPicker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string adventurerName { get; private set; }

    private CharacterSheetManager charSheetManager; // The charactermanager in the game.
    private RectTransform transformer; // this object's rect transform
    private bool overSpace;

    void Awake()
	{
        transformer = this.GetComponent<RectTransform>();
        overSpace = false;
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
			
		}
	}
}
