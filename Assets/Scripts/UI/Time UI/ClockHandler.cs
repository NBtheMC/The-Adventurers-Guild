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
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("TimeSystem").GetComponent<TimeSystem>().TickAdded += UpdateClock;
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
        hour = hour == 23 ? 0 : hour + 1;

        if(hour == 0)
        {
            day++;
            dayCounter.text = "Day: " + day;
        }
        
        //calculate next clock hand position
        float theta = (0.261799f * hour) / 2;
        float z = Mathf.Sin(theta);
        nextRotation = new Quaternion(0f, 0f, -z, Mathf.Cos(theta));
    }
}