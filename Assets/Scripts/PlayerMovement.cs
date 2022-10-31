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
    public float elapsedGlideTime;
    int lane;
    public bool atEndOfLevel;
    private bool runningIFrames;
    private bool runningJumpCooldown;
    private bool runningJumpIFrames;
    private bool runningGlideIFrames;
    private bool gliding;
    public float laneSwapTimer;
    public Vector2 Endpoint;
    public Vector2 GameObjectPosition;
    public GameObject wingsObject;
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
        laneSwapTimer = 0;
        atEndOfLevel = false;
        wingsObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Swap Speed Tier: " + PlayerStats.playerStats.laneSwapSpeed);
        if (UI_Manager.ui_manager.state != UI_Manager.UI_State.gameplay) return;


        // Stops the player from moving if they are dead or if they are at the end of a level
        if (this.gameObject.transform.position.y >= LaneParent.laneParent.levelLength || !PlayerStats.playerStats.isAlive)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            atEndOfLevel = true;
            return;
        }

        if (this.gameObject.GetComponent<Rigidbody2D>().velocity.y > maxSpeed)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1 * maxSpeed);
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity += Vector2.up * acceleration;
        }

        currentSpeed = this.gameObject.GetComponent<Rigidbody2D>().velocity.y;

        Endpoint = new Vector2(LaneParent.laneParent.transform.GetChild(lane).transform.position.x, this.gameObject.transform.position.y);
        //this.gameObject.transform.position = new Vector2(LaneParent.laneParent.transform.GetChild(lane).transform.position.x, this.gameObject.transform.position.y);
        this.gameObject.transform.position = Vector2.MoveTowards(transform.position, Endpoint, PlayerStats.playerStats.laneSwapSpeed * Time.deltaTime * 7);

        // accelerate until player hits max speed -- slow on collision with obstacle
        // check left/right input -- change lane based on input. Input does not loop around pac-man style
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lane--;
            laneSwapTimer = 0;
            if (lane < 0)
            {
                lane = 0;
            }
            //Debug.Log("Current lane: " + lane);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            lane++;
            laneSwapTimer = 0;
            if (lane > 4)
            {
                lane = 4;
            }
            //Debug.Log("Current lane: " + lane);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerStats.playerStats.canJump)
            {
                Debug.Log("Jumping");
                PlayerStats.playerStats.canJump = false;
                runningJumpCooldown = true;
                runningJumpIFrames = true;
                SoundManager.soundManager.PlayJumpAudio();
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (PlayerStats.playerStats.isJumping)
            {
                //Debug.Log("Attempting Glide");
                GlideIFrames();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            gliding = false;
            wingsObject.SetActive(false);
        }

        if (PlayerStats.playerStats.isJumping == false)
        {
            elapsedGlideTime = 0;
            //Debug.Log("Glide time reset");
        }


        JumpCooldown(PlayerStats.playerStats.jumpCooldown);
        JumpingIFrames(PlayerStats.playerStats.jumpIFrames);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (PlayerStats.playerStats.isJumping || gliding) return;
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
        else if (collision.gameObject.CompareTag("TallObstacle"))
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
        atEndOfLevel = false;
        
    }

    public void JumpCooldown(float timer)
    {
        if (gliding) return;
        if (!runningJumpCooldown)
        {
            elapsedTime = 0f;
        }
        else if (!PlayerStats.playerStats.canJump)
        {
            elapsedTime += Time.deltaTime;
            //Debug.Log("Cooldown time: " + elapsedTime);
        }

        if (elapsedTime >= PlayerStats.playerStats.jumpCooldown)
        {
            //Debug.Log(PlayerStats.playerStats.jumpCooldown);
            //Debug.Log("Cooldown complete");
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
        if (gliding) return;
        if (!runningJumpIFrames)
        {
            elapsedJumpIFrameTime = 0f;
        }
        else if (elapsedJumpIFrameTime < timer)
        {
            PlayerStats.playerStats.isJumping = true;
            runningJumpIFrames = true;
            elapsedJumpIFrameTime += Time.deltaTime;
            //Debug.Log(elapsedJumpIFrameTime);
        }


        if(elapsedJumpIFrameTime >= timer)
        {
            PlayerStats.playerStats.isJumping = false;
            runningJumpIFrames = false;
            SoundManager.soundManager.PlayLandingAudio();
        }
    }

    public void GlideIFrames()
    {
        if (PlayerStats.playerStats.wingsEnabled == false)
        {
            Debug.Log("No wings");
            return;
        }
        if (elapsedGlideTime < PlayerStats.playerStats.glideTime)
        {
            gliding = true;
            elapsedGlideTime += Time.deltaTime;
            Debug.Log("gliding");
            Debug.Log(elapsedGlideTime);
            wingsObject.SetActive(true);
        }
        if (elapsedGlideTime >= PlayerStats.playerStats.glideTime)
        {
            gliding = false;
            Debug.Log("no longer gliding");
            wingsObject.SetActive(false);

        }

    }
}
