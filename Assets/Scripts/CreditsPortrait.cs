using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPortrait : MonoBehaviour
{
    private float maxSpeed;
    private float currentSpeed;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = .1f;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //move down by currentSpeed depending on y position
        //rigidbody2d
        if(rb.position.y > 1.05 ){
            SetSpeed(maxSpeed/3);
        }
        else if(rb.position.y > .95 ){
            SetSpeed(maxSpeed);
        }
    }

    public void SetSpeed(float newSpeed){
        maxSpeed = newSpeed;
        currentSpeed = maxSpeed;
        rb.velocity = new Vector2(0, maxSpeed);
    }
}
