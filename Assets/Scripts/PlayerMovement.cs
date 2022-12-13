using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    float maxSpeed = 15.0f;
    float acceleration = 0.03f;
    public float elapsedTime = 0;
    public float elapsedJumpIFrameTime;
    public float elapsedGlideTime;
    int lane;
    public bool atEndOfLevel;
    private bool runningIFrames;
    private bool runningJumpCooldown;
    private bool runningJumpIFrames;
    private bool gliding;

    private Vector2 Endpoint;
    public GameObject wingsObject;
    public GameObject shadowObject;
    public static PlayerMovement playerMovement;
    public Animator PlayerAnim;

    void Awake()
    {
        //Singleton pattern
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
        PlayerAnim = gameObject.GetComponent<Animator>();
        lane = 2;
        elapsedTime = PlayerStats.playerStats.jumpCooldown;
        atEndOfLevel = false;
        wingsObject.SetActive(false);
        Color color = new Color();
        color.a = 0;
        shadowObject.GetComponent<SpriteRenderer>().color = color;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateAnimRefs();
        if (UI_Manager.ui_manager.state != UI_Manager.UI_State.gameplay) return;

        // Stops the player from moving if they are dead or if they are at the end of a level
        if (this.gameObject.transform.position.y >= LaneParent.laneParent.levelLength || !PlayerStats.playerStats.isAlive)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            atEndOfLevel = true;
            return;
        }

        // Accelerates player until they reach max speed, then clamps speed to max
        if (this.gameObject.GetComponent<Rigidbody2D>().velocity.y > maxSpeed)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1 * maxSpeed);
        }
        else
        {
            // accelerate until player hits max speed -- slow on collision with obstacle
            this.gameObject.GetComponent<Rigidbody2D>().velocity += Vector2.up * acceleration;
        }

        // Used to smoothly move the player between lanes
        Endpoint = new Vector2(LaneParent.laneParent.transform.GetChild(lane).transform.position.x, this.gameObject.transform.position.y);
        this.gameObject.transform.position = Vector2.MoveTowards(transform.position, Endpoint, PlayerStats.playerStats.laneSwapSpeed * Time.deltaTime * 7);

        
        // check left/right input -- change lane based on input. Input does not loop around pac-man style
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lane--;
            if (lane < 0)
            {
                lane = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            lane++;
            if (lane > 4)
            {
                lane = 4;
            }
        }


        // Player jumps on space down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerStats.playerStats.canJump)
            {
                PlayerStats.playerStats.canJump = false;
                runningJumpCooldown = true;
                runningJumpIFrames = true;
                SoundManager.soundManager.PlayJumpAudio();
            }
        }

        // Player glides if space is held while mid jump
        if (Input.GetKey(KeyCode.Space))
        {
            if (PlayerStats.playerStats.isJumping)
            {
                GlideIFrames();
            }
        }

        // Glide stops when space is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            gliding = false;
            wingsObject.SetActive(false);
        }

        // Resets glide time when the player is not jumping
        if (PlayerStats.playerStats.isJumping == false)
        {
            elapsedGlideTime = 0;
        }

        JumpCooldown(PlayerStats.playerStats.jumpCooldown);
        JumpingIFrames(PlayerStats.playerStats.jumpIFrames);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if player collided with a regular obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //ignores collision if the player is jumping or gliding
            if (PlayerStats.playerStats.isJumping || gliding) return;

            //if the player is not invincible they take damage and the damage audio is played
            if (!PlayerStats.playerStats.invincible)
            {
                PlayerStats.playerStats.TakeDamage(1);
                SoundManager.soundManager.PlayDamageAudio();
            }

            //if i-Frames are not running, the coroutine is started, this only occurs when the player is not invincible
            if (!runningIFrames)
            {
                StartCoroutine(IFrames(PlayerStats.playerStats.invincibilityTime));
            }

            //slows the player and deactivates the obstacle collided with
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
            collision.gameObject.SetActive(false);
        }

        //checks if player collided with a StageObstacle
        else if (collision.gameObject.CompareTag("StageObstacle"))
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
        }

        // checks if player collided with a TallObstacle
        else if (collision.gameObject.CompareTag("TallObstacle"))
        {
            //Tall obstacles cannot be jumped or glided over
        
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

        //checks if player collided with a healthUp
        else if (collision.gameObject.CompareTag("HealthUp"))
        {
            //heals one hitpoint, plays the appropriate audio, and deactivates the collectible
            PlayerStats.playerStats.Heal(1);
            SoundManager.soundManager.PlayHealAudio();
            collision.gameObject.SetActive(false);
        }

        //checks if player collided with currency
        else if (collision.gameObject.CompareTag("Currency"))
        {
            //increases the players currency count by one, plays the appropriate audio
            PlayerStats.playerStats.IncreaseMoney(1);
            SoundManager.soundManager.PlayMoneyAudio();

            //lerps the coin to a preset position on the hud to show that the coins are being collected
            UI_Manager.ui_manager.AddToLerpList(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Gem"))
        {
           //same as coins, but with a value of 5
            PlayerStats.playerStats.IncreaseMoney(5);
            SoundManager.soundManager.PlayGemAudio();
            UI_Manager.ui_manager.AddToLerpList(collision.gameObject);
        }
    }

    public void ResetRun()
    {
        //resets all necessary stats between runs
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        lane = 2;
        this.gameObject.transform.position = new Vector2(LaneParent.laneParent.transform.GetChild(lane).transform.position.x, 0);
        atEndOfLevel = false;
        gliding = false;
        wingsObject.SetActive(false);
        Color color = new Color();
        color.a = 0;
        shadowObject.GetComponent<SpriteRenderer>().color = color;
        elapsedGlideTime = 0;
    }

    private void JumpCooldown(float timer)
    {
        if (gliding) return;
        if (!runningJumpCooldown)
        {
            elapsedTime = 0f;
        }
        else if (!PlayerStats.playerStats.canJump)
        {
            elapsedTime += Time.deltaTime;
        }

        if (elapsedTime >= PlayerStats.playerStats.jumpCooldown)
        {
            
            PlayerStats.playerStats.canJump = true;
            if (runningJumpCooldown == true)
            {
                SoundManager.soundManager.PlayJumpChargeAudio();
            }
            runningJumpCooldown = false;
        }

    }

    IEnumerator IFrames(float timer)
    {
        // flips bool to true to ensure that the method cannot overlap with itself
        runningIFrames = true;
        PlayerStats.playerStats.invincible = true;
        Debug.Log("I-Frames Starting");
        float normalizedTime = 0f;

        // counts down the invincibility time
        while(normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / timer;
            //flashes player sprite red to indicate damage taken
            gameObject.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f);
            yield return null;
        }

        //reverts player sprite to normal
        gameObject.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.1f);
        PlayerStats.playerStats.invincible = false;
        runningIFrames = false;
    }

    private void JumpingIFrames(float timer)
    {
        if (gliding) return;
        if (!runningJumpIFrames)
        {
            elapsedJumpIFrameTime = 0f;
        }
        else if (elapsedJumpIFrameTime < timer)
        {
            //while the elapsed I-frame time is less than the timer limit

            if (!PlayerStats.playerStats.isJumping)
            {
                //turns player sprite grey to show invincibility
                gameObject.GetComponent<SpriteRenderer>().DOColor(Color.grey, 0.1f);
                shadowObject.GetComponent<SpriteRenderer>().DOFade(1, timer / 2);
            }
            PlayerStats.playerStats.isJumping = true;
            runningJumpIFrames = true;
            elapsedJumpIFrameTime += Time.deltaTime;
        }

        //when the elapsed time exceed the timer limit
        if(elapsedJumpIFrameTime >= timer)
        {
            PlayerStats.playerStats.isJumping = false;
            runningJumpIFrames = false;
            SoundManager.soundManager.PlayLandingAudio();
            //returns the player sprite to normal
            gameObject.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.1f);
            shadowObject.GetComponent<SpriteRenderer>().DOFade(0, timer / 2);
        }
    }

    private void GlideIFrames()
    {
        //the player cannot glide without wings
        if (PlayerStats.playerStats.wingsEnabled == false)
        {
            Debug.Log("No wings");
            return;
        }

        //while the elapsed glide time is less than the limit
        if (elapsedGlideTime < PlayerStats.playerStats.glideTime)
        {
            gliding = true;
            elapsedGlideTime += Time.deltaTime;
            Debug.Log("gliding");
            Debug.Log(elapsedGlideTime);
            wingsObject.SetActive(true);
        }

        //once the limit is reached
        if (elapsedGlideTime >= PlayerStats.playerStats.glideTime)
        {
            gliding = false;
            Debug.Log("no longer gliding");
            wingsObject.SetActive(false);

        }

    }

    //Sets animation references
    private void UpdateAnimRefs()
    {
        PlayerAnim.SetFloat("Speed", this.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude);
        PlayerAnim.SetFloat("Hangtime", elapsedJumpIFrameTime);
        PlayerAnim.SetBool("IsJumping", PlayerStats.playerStats.isJumping);
        PlayerAnim.SetBool("IsAlive", PlayerStats.playerStats.isAlive);
    }
}
