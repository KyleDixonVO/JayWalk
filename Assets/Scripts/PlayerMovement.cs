using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameObject LaneParent;
    PlayerStats playerStats;
    SoundManager soundManager;
    float maxSpeed = 15.0f;
    float acceleration = 0.01f;
    public float currentSpeed;
    int lane;
    float laneSwapCooldown;
    // Start is called before the first frame update
    void Start()
    {
        // starts player in middle lane
        lane = 2;
        LaneParent = GameObject.Find("LaneParent");
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (this.gameObject.transform.position.y >= LaneParent.GetComponent<LaneParent>().levelLength || !playerStats.isAlive)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }
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
            if (lane > 4)
            {
                lane = 4;
            }
            Debug.Log("Current lane: " + lane);
        }

        if (this.gameObject.GetComponent<Rigidbody2D>().velocity.y > maxSpeed)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,  1 * maxSpeed);
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity += Vector2.up * acceleration; 
        }

        currentSpeed = this.gameObject.GetComponent<Rigidbody2D>().velocity.y;

        this.gameObject.transform.position = new Vector2(LaneParent.transform.GetChild(lane).transform.position.x, this.gameObject.transform.position.y);

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerStats.TakeDamage(1);
            soundManager.PlayDamageAudio();
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("HealthUp"))
        {
            playerStats.Heal(1);
            soundManager.PlayHealAudio();
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Currency"))
        {
            playerStats.IncreaseMoney(1);
            soundManager.PlayMoneyAudio();
            collision.gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        lane = 2;
        this.gameObject.transform.position = new Vector2(LaneParent.transform.GetChild(lane).transform.position.x, 0);
    }
}
