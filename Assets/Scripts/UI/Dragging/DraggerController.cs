using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This controller allows a player to select and drag a character around.
/// </summary>
public class DraggerController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public DropHandler dropHandler; // Where to call drop commands.

    public ObjectDropPoint objectDropPoint; // The home drop point.
    private RectTransform transformer; // defines the rectangle reference for this dragger.
    public DropHandler.DropType dropType = DropHandler.DropType.character; // Defines what this dragger represents.

    //Event Stuff
    public delegate void OnDropCharacter();
    public static event OnDropCharacter onDropCharacter;

    private bool beingDragged = false;
    private Transform QuestDisplayTransform;
    private Transform Portrait;

    void Awake()
    {
        transformer = this.GetComponent<RectTransform>();
        QuestDisplayTransform = GameObject.Find("QuestDisplayManager/QuestDisplay").transform;
        Portrait = transform.parent.Find("Portrait");
    }

    void Update()
	{
        // Keeps character locked to their drop point as long as it's not being dragged.
        if (beingDragged == false) 
        {
            //transformer.position = objectDropPoint.GetComponent<RectTransform>().position;
            transformer.position = Portrait.position;
        }
        //this.transform.parent.transform.SetAsLastSibling();
	}

    void Start()
	{
        Debug.Assert(objectDropPoint != null, "ObjectDropPoint cannot be null on draggers.");
        transformer.position = objectDropPoint.GetComponent<RectTransform>().position;
    }

    /// <summary>
    /// For when this UI object is beginning to be dragged.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (PauseMenu.gamePaused)
            return;
        beingDragged = true;

        //set character drop point to unassigned state
        GameObject oldDropPoint = this.transform.parent.gameObject;
        oldDropPoint.transform.Find("Name").gameObject.SetActive(false);
        oldDropPoint.transform.Find("EmptyCharacter").gameObject.SetActive(true);
        oldDropPoint.transform.Find("FilledCharacter").gameObject.SetActive(false);

        this.transform.SetParent(QuestDisplayTransform);
        var i = Random.Range(0, 2);
        if (i == 0) {SoundManagerScript.PlaySound("slipUp1");}
        else {SoundManagerScript.PlaySound("slipUp2");}
    }

    /// <summary>
    /// For when this UI object is being dragged.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        transformer.position += new Vector3(eventData.delta.x,eventData.delta.y);
    }

    /// <summary>
    /// For when this object is finished dragging.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        print("End drag");
        beingDragged = false;
        if (dropHandler.inDropPoint(this))
		{
            Debug.Log("Successful Drop");

            var i = Random.Range(0, 2);
            if (i == 0) {SoundManagerScript.PlaySound("slipDown1");}
            else {SoundManagerScript.PlaySound("slipDown2");}
		}
		transformer.position = objectDropPoint.GetComponent<RectTransform>().position;
        this.transform.SetParent(objectDropPoint.transform);

        //display character name on drop point
        GameObject characterName = objectDropPoint.transform.Find("Name").gameObject;
        characterName.SetActive(true);
        characterName.GetComponent<Text>().text = this.gameObject.GetComponent<CharacterTileController>().characterSheet.name;

        objectDropPoint.transform.Find("EmptyCharacter").gameObject.SetActive(false);
        objectDropPoint.transform.Find("FilledCharacter").gameObject.SetActive(true);

        Portrait = transform.parent.Find("Portrait");
    }

    public void RefreshCharacterOnDrop(){
        if(onDropCharacter != null){
            onDropCharacter();
        }
    }
}
