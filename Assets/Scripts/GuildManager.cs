using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GuildManager : MonoBehaviour
{
    private int _gold = 100;
    private int _unpaidDaysCounter = 0;
    private TextMeshProUGUI _goldText;
    private GameObject DaysOverObject;

    public int Gold { get { return _gold; } set { _gold = value; GoldChanged?.Invoke(this, value); UpdateGoldDisplay() ; } }
    private int UnpaidDaysCounter { get { return _unpaidDaysCounter; } set { _unpaidDaysCounter = Mathf.Clamp(value, 0, 3); } }


    public EventHandler<int> GoldChanged;

    private void Awake()
    {
        _goldText = GameObject.Find("Main UI/Gold").GetComponentInChildren<TextMeshProUGUI>();
        GameObject.Find("TimeSystem").GetComponent<TimeSystem>().TickAdded += CheckDebt;
        DaysOverObject = GameObject.Find("Main UI/DaysOver");
    }

    public void UpdateGoldDisplay()
    {
        _goldText.text = _gold.ToString();
    }

    public void CheckDebt(object src, GameTime time)
    {
        if (time.hour == 16)
            UpdateDebt();

    }

    public void UpdateDebt()
    {
        Debug.Log("Updating Debt");
        if (_gold < 0)
            UnpaidDaysCounter++;
        else
            UnpaidDaysCounter--;

        if (UnpaidDaysCounter == 3)
            return; //game over screen

        else
        {

            DaysOverObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            DaysOverObject.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            DaysOverObject.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);

            for (int i = 0; i < UnpaidDaysCounter; i++)
            {
                DaysOverObject.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }

        }
    }

}
