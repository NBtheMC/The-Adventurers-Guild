using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextAsset TooltipFile;
    private GameObject TooltipObject;
    private Text HeaderObject;
    private Text DescriptionObject;

    private string HeaderText;
    private string DescriptionText;

    public float HoverTime = 0f;
    private bool HoverTimerRunning = false;

    public enum TooltipPosition { TopLeft, TopMiddle, TopRight, MiddleLeft, MiddleRight, BottomLeft, BottomMiddle, BottomRight};
    public TooltipPosition CursorPosition;

    [SerializeField] private CanvasScaler scaler;
    public Vector2 ScreenScale;

    private bool displayTooltip = false;
    private bool enteredHoverArea = false;
    private Vector3 lastMousePosition;
    public Vector3 padding;

    public void Start()
    {
        if(TooltipObject == null)
            TooltipObject = GameObject.Find("TooltipObject");
        if (HeaderObject == null)
            HeaderObject = TooltipObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        if (DescriptionObject == null)
            DescriptionObject = TooltipObject.transform.GetChild(0).GetChild(1).GetComponent<Text>();

        if (scaler == null)
            scaler = GameObject.Find("QuestDisplayManager/QuestDisplay").GetComponent<CanvasScaler>();
        ScreenScale = new Vector2(scaler.referenceResolution.x / Screen.width, scaler.referenceResolution.y / Screen.height);

        //read data from file and split the lines
        string tooltipText = TooltipFile.text;
        string[] splitText = tooltipText.Split('\n');

        //first line is the Header
        HeaderText = splitText[0];
        //remaining lines contain description text
        for (int i = 1; i < splitText.Length; i++)
            DescriptionText += splitText[i];  
    }

    public void Update()
    {
        if (enteredHoverArea)
        {
            //mouse has moved
            if (Input.mousePosition != lastMousePosition)
            {
                lastMousePosition = Input.mousePosition;
                //update tooltip position if it is displayed
                if (displayTooltip)
                {
                    TooltipObject.transform.position = Input.mousePosition + GetPositionOffset();
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
                if(!HoverTimerRunning && !displayTooltip)
                    StartCoroutine(StartHoverTimer());
                else if(displayTooltip)
                    TooltipObject.transform.position = Input.mousePosition + GetPositionOffset();
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        enteredHoverArea = true;
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        enteredHoverArea = false;
        HideToolTip();
    }

    public void OnDisable()
    {
        enteredHoverArea = false;
        HideToolTip();
    }

    public void ShowToolTip()
    {  
        HeaderObject.text = HeaderText;
        DescriptionObject.text = DescriptionText;
        TooltipObject.transform.position = Input.mousePosition + GetPositionOffset();
        TooltipObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        TooltipObject.transform.GetChild(0).gameObject.SetActive(false);
        displayTooltip = false;
    }


    public IEnumerator StartHoverTimer()
    {
        HoverTimerRunning = true;
        yield return new WaitForSecondsRealtime(HoverTime);
        displayTooltip = true;
        ShowToolTip();
        HoverTimerRunning = false;
    }

    private Vector3 GetPositionOffset()
    {
        ScreenScale = new Vector2(scaler.referenceResolution.x / Screen.width, scaler.referenceResolution.y / Screen.height);
        Rect tooltipRect = TooltipObject.GetComponent<RectTransform>().rect;
        Vector3 offset;
        switch (CursorPosition)
        {
            case TooltipPosition.TopLeft:
                offset = new Vector3((tooltipRect.width / 2 + padding.x)/ ScreenScale.x, 0, 0);
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
                offset = new Vector3(-(tooltipRect.width / 2 + padding.x)/ ScreenScale.x, (tooltipRect.height / 2 + padding.y) / ScreenScale.y, 0);
                break;
            default:
                offset = new Vector3(0, 0, 0);
                break;
        }

        return offset;
    }
}
