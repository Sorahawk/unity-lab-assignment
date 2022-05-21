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
    private int score = 0;
    private int highScore;

    void Start() {
        Application.targetFrameRate = 60;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        
        highScore = PlayerPrefs.GetInt("highScore");
        Debug.Log("High Score: " + highScore.ToString());

        platform = GameObject.Find("Brick");
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

        // allow wall jumping
        if (onLeftWall) {
            if (Input.GetKeyDown("space")) {
                marioBody.velocity = new Vector2(10, upSpeed);
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
                marioBody.velocity = new Vector2(-10, upSpeed);
                onRightWall = false;
                onGround = false;
                countScoreState = true;
            }

            else if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) {
                onRightWall = false;
            }
        }

        // normal jumping
        else if (onGround) {
            if (Input.GetKeyDown("space")) {
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGround = false;
                countScoreState = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground")) {
            onGround = true;
            countScoreState = false;
            scoreText.text = "Score: " + score.ToString();
        }

        else if (col.gameObject.CompareTag("Left Wall")) onLeftWall = true;
        else if (col.gameObject.CompareTag("Right Wall")) onRightWall = true;

        else if (col.gameObject.CompareTag("Enemy")) {
            // slow down end screen while Mario blasts off
            Time.timeScale = 0.05f;
            marioBody.velocity = new Vector2(0, 100);

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
            // add bonus score
            score += 10;
            scoreText.text = "Score: " + score.ToString();

            // remove coin and platform
            other.gameObject.SetActive(false);
            platform.gameObject.SetActive(false);
        }
    }
}
