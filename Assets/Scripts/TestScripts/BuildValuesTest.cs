using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildValuesTest : MonoBehaviour
{
    public WorldStateManager state;
    public Text storyletNumber;
    public CSVToQuests thingy;
    public Text eventNumber;
    public TimeSystem timeSystem;
    public Text currentTime;
    public int ticker;
    public Text tickertext;

	private void Start()
	{
        ticker = 0;
        timeSystem.TickAdded += AddNumber;
	}

	// Update is called once per frame
	void Update()
    {
        storyletNumber.text = $"Storylet Number: {state.storylets.Count.ToString()}";
        eventNumber.text = $"Event Number: {thingy.allEvents.Count}";
        currentTime.text = $"Current Ticks: day {timeSystem.GameTime.day}, hour {timeSystem.GameTime.hour}, tick {timeSystem.GameTime.tick}";
        tickertext.text = $"Total Elasped {ticker}";
    }

    void AddNumber(object sender, GameTime time)
	{
        ticker++;
	}
}
