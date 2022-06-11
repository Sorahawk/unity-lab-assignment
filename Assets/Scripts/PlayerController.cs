using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Animator marioAnimator;

    // movement
    private Vector2 velocity;
    private float speed = 100;
    private float maxSpeed = 150;
    private float upSpeed = 13;

    private float friction = 0.15f;
    private float airDrag = 0.1f;

    // collision
    public GameObject platform;
    public bool onGround = true;
    public int onSideWalls = 0;

    // score
    public Transform enemyLocation;
    public Text scoreText;
    public Text highScoreText;
    public Text resultText;
    public Button restartButton;
    private bool countScoreState = false;
    private bool gameOver = false;
    private int score = 0;
    private int highScore;

    // audio
    public AudioSource gameMusic;
    public AudioSource jumpAudio;
    public AudioSource coinAudio;
    public AudioSource loseAudio;
    public AudioSource winAudio;
    public AudioSource highScoreAudio;

    // particles
    public ParticleSystem dustCloud;

    // game constants
    public GameConstants gameConstants;

    void Start() {
        Application.targetFrameRate = 60;

        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();

        // retrieve high score from player preferences
        highScore = gameConstants.highScore;
        Debug.Log("High Score: " + highScore.ToString());
    }

    void Update() {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        // flip Mario orientation according to direction of movement
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) {
            marioSprite.flipX = true;

            if (Mathf.Abs(marioBody.velocity.x) > 1) marioAnimator.SetTrigger("onSkid");
        }

        else if (Input.GetKeyDown("right") || Input.GetKeyDown("d")) {
            marioSprite.flipX = false;

            if (Mathf.Abs(marioBody.velocity.x) > 1) marioAnimator.SetTrigger("onSkid");
        }

        // score counting
        if (!onGround && countScoreState) {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f && Mathf.Abs(transform.position.y - enemyLocation.position.y) < 5) {
                countScoreState = false;
                score++;
            }
        }
    }

    void FixedUpdate() {
        // manually calculate horizontal drag, with different values on ground and in air
        velocity = marioBody.velocity;
        if (onGround) velocity.x *= (1 - friction);
        else velocity.x *= (1 - airDrag);
        marioBody.velocity = velocity;

        // character movement handling
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0) {
            Vector2 movement = new Vector2(moveHorizontal, 0);

            if (marioBody.velocity.magnitude < maxSpeed) {
                marioBody.AddForce(movement * speed);
            }
        }

        if (Input.GetKeyDown("space") && (onGround || onSideWalls != 0)) {
            // wall jumping
            if (onSideWalls != 0) {
                // move character and play sound effect
                marioBody.velocity = new Vector2(onSideWalls * 3, upSpeed);
            }

            // normal jumping
            else if (onGround) {
                // move character and play sound effect
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            }

            jumpAudio.Play();

            countScoreState = true;
            onSideWalls = 0;
            marioAnimator.SetBool("onWall", false);

            onGround = false;
            marioAnimator.SetBool("onGround", onGround);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if ((col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Pillars")) && !gameOver) {
            if (!onGround) {
                onGround = true;
                dustCloud.Play();
            }

            marioAnimator.SetBool("onGround", onGround);
            countScoreState = false;
            scoreText.text = "Score: " + score.ToString();
        }

        else if (col.gameObject.CompareTag("Left Wall")) {
            onSideWalls = 1;
            marioAnimator.SetBool("onWall", true);
            marioSprite.flipX = false;
        }

        else if (col.gameObject.CompareTag("Right Wall")) {
            onSideWalls = -1;
            marioAnimator.SetBool("onWall", true);
            marioSprite.flipX = true;
        }

        else if (col.gameObject.CompareTag("Enemy")) {
            gameOver = true;
            gameMusic.Stop();

            // slow down end screen
            Time.timeScale = 0.1f;

            // check direction of collision
            float collisionDirection = transform.position.y - col.gameObject.transform.position.y;

            Debug.Log("Collision Value: " + collisionDirection.ToString());

            // sideways hit should be about -0.505, stomping on it gave values like -1.21 and -1.46
            if (collisionDirection < 0.8f) {
                // player loses, blast Mario off after switching off collisions
                GetComponent<BoxCollider2D>().enabled = false;
                marioBody.velocity = new Vector2(0, 50);

                // play game lost sound effect
                loseAudio.Play();

                resultText.text = "You Lose!";
            }

            // player wins
            else {
                // switch off collision and shoot enemy into the sky
                col.gameObject.GetComponent<Collider2D>().enabled = false;
                col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(30, 10);

                // play normal win music
                winAudio.Play();

                // add bonus points
                score += 15;
            }

            // check if new high score
            if (highScore < score) {
                highScore = score;

                gameConstants.highScore = highScore;

                // play high score music
                loseAudio.Stop();
                winAudio.Stop();
                highScoreAudio.Play();
            } else {
                highScoreText.fontSize = 46;
                highScoreText.text = "High Score: " + highScore.ToString();

            }

            // display results and reveal start button
            scoreText.text = "Score: " + score.ToString();

            resultText.gameObject.SetActive(true);
            highScoreText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Pillars")) {
            onGround = true;
            marioAnimator.SetBool("onGround", onGround);
        }

        else if (col.gameObject.CompareTag("Left Wall")) {
            onSideWalls = 1;
            marioAnimator.SetBool("onWall", true);
        }

        else if (col.gameObject.CompareTag("Right Wall")) {
            onSideWalls = -1;
            marioAnimator.SetBool("onWall", true);
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Pillars")) {
            onGround = false;
            marioAnimator.SetBool("onGround", onGround);
        }

        else if (col.gameObject.CompareTag("Left Wall") || col.gameObject.CompareTag("Right Wall")) {
            onSideWalls = 0;
            marioAnimator.SetBool("onWall", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Coin")) {
            // sound effect
            coinAudio.Play();

            // add bonus score
            score += 10;
            scoreText.text = "Score: " + score.ToString();

            // remove coin and platform
            other.gameObject.SetActive(false);
            platform.gameObject.SetActive(false);
        } else if (other.gameObject.CompareTag("Fire")) {
            gameOver = true;
            gameMusic.Stop();

            // slow down end screen
            Time.timeScale = 0.1f;

            // player loses, blast Mario off after switching off collisions
            GetComponent<BoxCollider2D>().enabled = false;
            marioBody.velocity = new Vector2(0, 50);

            // play game lost sound effect
            loseAudio.Play();

            resultText.text = "You Lose!";
        }
    }
}
