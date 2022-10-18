using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float maxSpeed = 15.0f;
    float acceleration = 0.01f;
    public float currentSpeed;
    public float elapsedTime = 0;
    public float elapsedJumpIFrameTime;
    int lane;
    private bool runningIFrames;
    private bool runningJumpCooldown;
    private bool runningJumpIFrames;
    public static PlayerMovement playerMovement;

    void Awake()
    {
        if (playerMovement == null)
        {
            playerMovement = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (playerMovement != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // starts player in middle lane
        lane = 2;
        elapsedTime = PlayerStats.playerStats.jumpCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // Stops the player from moving if they are dead or if they are at the end of a level
        if (this.gameObject.transform.position.y >= LaneParent.laneParent.levelLength || !PlayerStats.playerStats.isAlive)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        // accelerate until player hits max speed -- slow on collision with obstacle
        // check left/right input -- change lane based on input. Input does not loop around pac-man style
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lane--;
            if (lane < 0)
            {
                lane = 0;
            }
            Debug.Log("Current lane: " + lane);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            lane++;
            if (lane > 4)
            {
                lane = 4;
            }
            Debug.Log("Current lane: " + lane);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerStats.playerStats.canJump)
            {
                PlayerStats.playerStats.canJump = false;
                runningJumpCooldown = true;
                runningJumpIFrames = true;
            }
        }

        JumpCooldown(PlayerStats.playerStats.jumpCooldown);
        JumpingIFrames(PlayerStats.playerStats.jumpIFrames);

        if (this.gameObject.GetComponent<Rigidbody2D>().velocity.y > maxSpeed)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,  1 * maxSpeed);
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity += Vector2.up * acceleration; 
        }

        currentSpeed = this.gameObject.GetComponent<Rigidbody2D>().velocity.y;

        this.gameObject.transform.position = new Vector2(LaneParent.laneParent.transform.GetChild(lane).transform.position.x, this.gameObject.transform.position.y);

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerStats.playerStats.isJumping) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (!PlayerStats.playerStats.invincible)
            {
                PlayerStats.playerStats.TakeDamage(1);
                SoundManager.soundManager.PlayDamageAudio();
            }

            if (!runningIFrames)
            {
                StartCoroutine(IFrames(PlayerStats.playerStats.invincibilityTime));
            }

            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("HealthUp"))
        {
            PlayerStats.playerStats.Heal(1);
            SoundManager.soundManager.PlayHealAudio();
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Currency"))
        {
            PlayerStats.playerStats.IncreaseMoney(1);
            SoundManager.soundManager.PlayMoneyAudio();
            collision.gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        lane = 2;
        this.gameObject.transform.position = new Vector2(LaneParent.laneParent.transform.GetChild(lane).transform.position.x, 0);
    }

    public void JumpCooldown(float timer)
    {
        if (!runningJumpCooldown)
        {
            elapsedTime = 0f;
        }
        else if (!PlayerStats.playerStats.canJump)
        {
            elapsedTime += Time.deltaTime / timer;
            Debug.Log(elapsedTime);
        }
        if (elapsedTime >= 1)
        {
            PlayerStats.playerStats.canJump = true;
            runningJumpCooldown = false;

        }

    }

    IEnumerator IFrames(float timer)
    {
        runningIFrames = true;
        PlayerStats.playerStats.invincible = true;
        Debug.Log("I-Frames Starting");
        float normalizedTime = 0f;
        while(normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / timer;
            //Debug.Log(normalizedTime);
            yield return null;
        }
        PlayerStats.playerStats.invincible = false;
        runningIFrames = false;
    }

    public void JumpingIFrames(float timer)
    {
        if (!runningJumpIFrames)
        {
            elapsedJumpIFrameTime = 0f;
        }
        else if (elapsedJumpIFrameTime < 1)
        {
            PlayerStats.playerStats.isJumping = true;
            runningJumpIFrames = true;
            elapsedJumpIFrameTime += Time.deltaTime / timer;
            //Debug.Log(elapsedJumpIFrameTime);
        }

        if(elapsedJumpIFrameTime >= 1)
        {
            PlayerStats.playerStats.isJumping = false;
            runningJumpIFrames = false;
        }
    }
}
