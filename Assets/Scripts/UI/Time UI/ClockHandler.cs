using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockHandler : MonoBehaviour
{
    private bool canUpdate = true;
    private Quaternion nextRotation;
    private int hour = 0;
    private int day = 0;
    Text dayCounter;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float speed;

    private TimeSystem currentTimeSystem;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        currentTimeSystem.TickAdded += UpdateClock;
        currentTimeSystem.NewDay += BeginDay;
        dayCounter = transform.parent.Find("DayCounter/Days").GetComponent<Text>();
    }

    void FixedUpdate()
    {
        if(canUpdate)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, speed);
        }
        if(transform.rotation == nextRotation)
            canUpdate = false;
    }
    void UpdateClock(object source, GameTime gameTime)
    {
        canUpdate = true;
        
        //calculate next clock hand position
        float theta = Mathf.PI/currentTimeSystem.totalTicksperActive() * ((gameTime.hour * currentTimeSystem.ticksperHour)+gameTime.tick);
        float z = Mathf.Sin(theta);
        nextRotation = new Quaternion(0f, 0f, -z, Mathf.Cos(theta));
    }

    void EndDay()
	{
        // Reset rotation to the top of the clock.
        nextRotation = new Quaternion(0f, 0f, -Mathf.Sin(0f), Mathf.Cos(0f)); canUpdate = true;
        
	}

    void BeginDay(object source, GameTime gameTime)
	{
        day++;
        dayCounter.text = "" + day;
	}
}
