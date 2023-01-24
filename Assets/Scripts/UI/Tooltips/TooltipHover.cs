using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipHover : MonoBehaviour
{
    public enum TooltipPosition { TopLeft, TopMiddle, TopRight, MiddleLeft, MiddleRight, BottomLeft, BottomMiddle, BottomRight };
    [System.Serializable] public struct TooltipConfig
    {
        public string key;
        public TooltipPosition CursorPosition;
        public TextAsset TooltipFile;
    }

    public TooltipConfig[] tooltips;

    private TextAsset TooltipFile;
    private TooltipPosition CursorPosition;

    private Dictionary<string, (TextAsset, TooltipPosition)> TooltipMap = new Dictionary<string, (TextAsset, TooltipPosition)>();

    public CanvasScaler scaler;
    public Vector2 ScreenScale { get; private set; }
    public Vector3 padding;


    private GameObject TooltipParentObj;
    private GameObject TooltipChildObj;
    private Text HeaderObject;
    private Text DescriptionObject;

    public float HoverTime = 0f;
    private bool HoverTimerRunning = false;

    private bool displayTooltip = false;
    private bool enteredHoverArea = false;
    private Vector3 lastMousePosition;

    public EventSystem eventSystem;
    public GraphicRaycaster raycaster;
    private PointerEventData pointer;
    private GameObject CurrentPointerTarget;

    public void Start()
    {
        TooltipParentObj = GameObject.Find("TooltipObject");
        TooltipChildObj = TooltipParentObj.transform.GetChild(0).gameObject;
        HeaderObject = TooltipChildObj.transform.GetChild(0).GetComponent<Text>();
        DescriptionObject = TooltipChildObj.transform.GetChild(1).GetComponent<Text>();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        raycaster = GameObject.Find("Main UI").GetComponent<GraphicRaycaster>();
        scaler = GameObject.Find("Main UI").GetComponent<CanvasScaler>();
        ScreenScale = new Vector2(scaler.referenceResolution.x / Screen.width, scaler.referenceResolution.y / Screen.height);

        foreach(TooltipConfig config in tooltips)
        {
            TooltipMap.Add(config.key, (config.TooltipFile, config.CursorPosition));
        }
    }

    public void Update()
    {
        CheckTooltipHoverZone();
        if (enteredHoverArea)
        {
            //mouse has moved
            if (Input.mousePosition != lastMousePosition)
            {
                lastMousePosition = Input.mousePosition;
                //update tooltip position if it is displayed
                if (displayTooltip)
                {
                    TooltipParentObj.transform.position = Input.mousePosition + GetPositionOffset();
                }
                //stop timer if tooltip not displayed
                else
                {
                    StopCoroutine(StartHoverTimer());
                    HoverTimerRunning = false;
                }
            }
            else
            {
                //begin hover timer
                if (!HoverTimerRunning && !displayTooltip)
                    StartCoroutine(StartHoverTimer());
                else if (displayTooltip)
                    TooltipParentObj.transform.position = Input.mousePosition + GetPositionOffset();
            }
        }
    }

    private void CheckTooltipHoverZone()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            pointer = new PointerEventData(eventSystem);
            pointer.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointer, results);

            if (results.Count > 0)
            {
                GameObject hoverTarget = results[0].gameObject;
                if (hoverTarget.tag == "TooltipHoverZone")
                {
                    CurrentPointerTarget = hoverTarget;
                    enteredHoverArea = true;
                }
                else
                {
                    CurrentPointerTarget = null;
                    enteredHoverArea = false;
                    HideToolTip();
                }
            }
        }
    }

    public void OnDisable()
    {
        enteredHoverArea = false;
        HideToolTip();
    }

    private void ShowToolTip()
    {
        TooltipParentObj.transform.position = Input.mousePosition + GetPositionOffset();
        TooltipChildObj.gameObject.SetActive(true);
        if(CurrentPointerTarget)
            GetTooltipData(CurrentPointerTarget.name);
        displayTooltip = true;
    }

    private void HideToolTip()
    {
        if (TooltipChildObj)
            TooltipChildObj.SetActive(false);
        displayTooltip = false;
    }


    private IEnumerator StartHoverTimer()
    {
        HoverTimerRunning = true;
        yield return new WaitForSecondsRealtime(HoverTime);
        ShowToolTip();
        HoverTimerRunning = false;
    }

    private void GetTooltipData(string tooltipName)
    {
        TooltipFile = TooltipMap[tooltipName].Item1;
        CursorPosition = TooltipMap[tooltipName].Item2;

        //read data from file and split the lines
        string tooltipText = TooltipFile.text;
        string[] splitText = tooltipText.Split('\n');

        HeaderObject.text = "";
        DescriptionObject.text = "";

        //first line is the Header
        HeaderObject.text = splitText[0];
        //remaining lines contain description text
        for (int i = 1; i < splitText.Length; i++)
            DescriptionObject.text += splitText[i];
    }

    private Vector3 GetPositionOffset()
    {
        Rect tooltipRect = TooltipParentObj.GetComponent<RectTransform>().rect;
        Vector3 offset;
        switch (CursorPosition)
        {
            case TooltipPosition.TopLeft:
                offset = new Vector3((tooltipRect.width / 2 + padding.x) / ScreenScale.x, 0, 0);
                break;
            case TooltipPosition.TopRight:
                offset = new Vector3(-(tooltipRect.width / 2 + padding.x) / ScreenScale.x, 0, 0);
                break;
            case TooltipPosition.TopMiddle:
                offset = new Vector3(0, 0, 0);
                break;
            case TooltipPosition.BottomLeft:
                offset = new Vector3((tooltipRect.width / 2 + padding.x) / ScreenScale.x, (tooltipRect.height + padding.y) / ScreenScale.y, 0);
                break;
            case TooltipPosition.BottomRight:
                offset = new Vector3(-(tooltipRect.width / 2 + padding.x) / ScreenScale.x, (tooltipRect.height + padding.y) / ScreenScale.y, 0);
                break;
            case TooltipPosition.BottomMiddle:
                offset = new Vector3(0, (tooltipRect.height + padding.y) / ScreenScale.y, 0);
                break;
            case TooltipPosition.MiddleLeft:
                offset = new Vector3((tooltipRect.width / 2 + padding.x) / ScreenScale.x, (tooltipRect.height / 2 + padding.y) / ScreenScale.y, 0);
                break;
            case TooltipPosition.MiddleRight:
                offset = new Vector3(-(tooltipRect.width / 2 + padding.x) / ScreenScale.x, (tooltipRect.height / 2 + padding.y) / ScreenScale.y, 0);
                break;
            default:
                offset = new Vector3(0, 0, 0);
                break;
        }

        return offset;
    }
}
