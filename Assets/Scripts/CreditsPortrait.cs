using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPortrait : MonoBehaviour
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
        //move down by currentSpeed depending on y position
        //rigidbody2d
    }

    public void SetSpeed(float newSpeed){
        maxSpeed = newSpeed;
        currentSpeed = maxSpeed;
    }
}
