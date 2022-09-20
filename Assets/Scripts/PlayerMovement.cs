using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameObject LaneParent;
    float maxSpeed = 20.0f;
    float acceleration = 0.01f;
    int lane;
    // Start is called before the first frame update
    void Start()
    {
        LaneParent = GameObject.Find("LaneParent");
    }

    // Update is called once per frame
    void Update()
    {
        // accelerate until player hits max speed -- slow on collision with obstacle
        // check left/right input -- change lane based on input. Input does not loop around pac-man style

        if (Input.GetKeyDown(KeyCode.A))
        {
            lane--;
            if (lane < 0)
            {
                lane = 0;
            }
            Debug.Log("Current lane: " + lane);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            lane++;
            if (lane > 5)
            {
                lane = 5;
            }
            Debug.Log("Current lane: " + lane);
        }

        if (this.gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > maxSpeed)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,  1 * maxSpeed);
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity += Vector2.up * acceleration; 
        }

        //collision detection for player could only be done on active lanes
    }
}
