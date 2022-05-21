using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerController : MonoBehaviour {
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Vector2 velocity;

    // movement
    private float speed = 100;
    private float maxSpeed = 150;
    private float upSpeed = 15;

    private float friction = 0.15f;
    private float airDrag = 0.1f;

    // collision
    public GameObject platform;
    private bool onGround = true;
    private bool onLeftWall = false;
    private bool onRightWall = false;

    // score
    public Transform enemyLocation;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public Button restartButton;
    private bool countScoreState = false;
    private bool gameOver = false;
    private int score = 0;
    private int highScore;

    // audio
    public AudioSource gameMusic;
    public AudioSource jumpAudio;
    public AudioSource coinAudio;
    public AudioSource gameOverAudio;
    public AudioSource winAudio;

    void Start() {
        Application.targetFrameRate = 60;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        // retrieve high score from player preferences
        highScore = PlayerPrefs.GetInt("highScore");
        Debug.Log("High Score: " + highScore.ToString());

        platform = GameObject.Find("Brick");

        gameMusic = GameObject.Find("Music").GetComponent<AudioSource>();
        jumpAudio = GameObject.Find("Jump Audio").GetComponent<AudioSource>();
        coinAudio = GameObject.Find("Coin Audio").GetComponent<AudioSource>();
        gameOverAudio = GameObject.Find("Game Over Audio").GetComponent<AudioSource>();
        winAudio = GameObject.Find("Win Audio").GetComponent<AudioSource>();
    }

    void Update() {
        // flip Mario orientation according to direction of movement
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) {
            marioSprite.flipX = true;
        } else if (Input.GetKeyDown("right") || Input.GetKeyDown("d")) {
            marioSprite.flipX = false;
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

        // wall jumping
        if (onLeftWall) {
            if (Input.GetKeyDown("space")) {
                // sound effects
                jumpAudio.Play();

                // jump up-right
                marioBody.velocity = new Vector2(10, upSpeed);

                // set variables
                onLeftWall = false;
                onGround = false;
                countScoreState = true;
            }

            else if (Input.GetKeyDown("right") || Input.GetKeyDown("d")) {
                onLeftWall = false;
            }
        }

        else if (onRightWall) {
            if (Input.GetKeyDown("space")) {
                // sound effects
                jumpAudio.Play();

                // jump up-left
                marioBody.velocity = new Vector2(-10, upSpeed);

                // set variables
                onRightWall = false;
                onGround = false;
                countScoreState = true;
            }

            else if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) {
                onRightWall = false;
            }
        }

        // jump from ground
        else if (onGround) {
            if (Input.GetKeyDown("space")) {
                // sound effects
                jumpAudio.Play();

                // normal jump
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);

                // set variables
                onGround = false;
                countScoreState = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") && !gameOver) {
            onGround = true;
            countScoreState = false;
            scoreText.text = "Score: " + score.ToString();
        }

        else if (col.gameObject.CompareTag("Left Wall")) onLeftWall = true;
        else if (col.gameObject.CompareTag("Right Wall")) onRightWall = true;

        else if (col.gameObject.CompareTag("Enemy")) {
            gameOver = true;

            // slow down end screen
            Time.timeScale = 0.15f;

            // check direction of collision
            float collisionDirection = (col.gameObject.transform.position.y - transform.position.y);

            // sideways hit should be about -0.505, stomping on it gave values like -1.21 and -1.46
            if (collisionDirection > -0.6f) {
                // player loses, blast Mario off after switching off collisions
                GetComponent<BoxCollider2D>().enabled = false;
                marioBody.velocity = new Vector2(0, 50);

                // stop music and play game over sound effect
                gameMusic.Stop();
                gameOverAudio.Play();
            }

            // player wins
            else {
                // switch off collision and shoot Gomba into the sky
                col.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(30, 10);

                // stop music and play win sound effect
                gameMusic.Stop();
                winAudio.Play();

                // add bonus points
                score += 15;
            }

            // check if new high score
            if (highScore < score) {
                highScore = score;

                PlayerPrefs.SetInt("highScore", highScore);
                PlayerPrefs.Save();

                highScoreText.gameObject.SetActive(true);
            }

            // show high score text and reveal restart button
            scoreText.text = "High Score: " + highScore.ToString();
            restartButton.gameObject.SetActive(true);
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
        }
    }
}
