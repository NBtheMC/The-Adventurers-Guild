using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsText : MonoBehaviour
{
    private float maxSpeed;
    private float currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //move up by currentSpeed depending on y position
        //recttransform
    }

    public void SetSpeed(float newSpeed){
        maxSpeed = newSpeed;
        currentSpeed = maxSpeed;
    }
}
