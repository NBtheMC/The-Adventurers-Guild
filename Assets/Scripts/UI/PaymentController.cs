using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaymentController : MonoBehaviour
{
    [HideInInspector] public CharacterSheet charSheet;
    [SerializeField] private Text goldAmount;
    public Text daysToPayText;
    public int days;

    private DebriefReport debriefReport;

    // Start is called before the first frame update
    void Start()
    {
        days = 0;
        debriefReport = GameObject.Find("DebriefReport").GetComponent<DebriefReport>();
    }

    public void IncrementDay()
    {
        if(days < charSheet.daysUnpaid + 1)
        {
            days++;
            debriefReport.UpdateGoldRemaining(-charSheet.salary);
        }      

        daysToPayText.text = days.ToString();
    }

    public void DecrementDay()
    {
        if (days > 0)
        {
            days--;
            debriefReport.UpdateGoldRemaining(charSheet.salary);
        }    

        daysToPayText.text = days.ToString();
    }

}
