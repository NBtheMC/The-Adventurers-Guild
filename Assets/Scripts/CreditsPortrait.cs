using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPortrait : MonoBehaviour
{
    private float maxSpeed;
    private float currentSpeed;

    private Rigidbody2D rb;

    void Start()
    {
        maxSpeed = 0.0f;
        rb = gameObject.AddComponent<Rigidbody2D>() as Rigidbody2D;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        Debug.Log("Does rb2d exist? " + rb);
    }

    void FixedUpdate()
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
        Debug.Log("new speed = " + newSpeed);
        Debug.Log("max speed = " + maxSpeed);

        Debug.Log("rb velocity = " + rb.velocity);
        rb.velocity = new Vector2(0.0f, 2.0f);
    }
}
