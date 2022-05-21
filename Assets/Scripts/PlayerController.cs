using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Vector2 velocity;

    private float speed = 100;
    private float maxSpeed = 150;
    private float upSpeed = 15;

    private float friction = 0.15f;
    private float airDrag = 0.1f;

    private bool onGround = true;
    private bool onLeftWall = false;
    private bool onRightWall = false;

    void Start() {
        Application.targetFrameRate = 60;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) {
            marioSprite.flipX = true;
        }

        else if (Input.GetKeyDown("right") || Input.GetKeyDown("d")) {
            marioSprite.flipX = false;
        }
    }

    void FixedUpdate() {
        // manually calculate horizontal drag, with different values on ground and in air
        velocity = marioBody.velocity;
        
        if (onGround) velocity.x *= (1 - friction);
        else velocity.x *= (1 - airDrag);

        marioBody.velocity = velocity;


        // move character
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0) {
            Vector2 movement = new Vector2(moveHorizontal, 0);

            if (marioBody.velocity.magnitude < maxSpeed) {
                marioBody.AddForce(movement * speed);
            }
        }

        // deal with jumping on ground and from walls
        if (onLeftWall) {
            if (Input.GetKeyDown("space")) {
                marioBody.velocity = new Vector2(3, upSpeed);
                onLeftWall = false;
                onGround = false;
            }

            else if (Input.GetKeyDown("right") || Input.GetKeyDown("d")) {
                onLeftWall = false;
            }
        } else if (onRightWall) {
            if (Input.GetKeyDown("space")) {
                marioBody.velocity = new Vector2(-3, upSpeed);
                onRightWall = false;
                onGround = false;
            }

            else if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) {
                onRightWall = false;
            }
        } else if (onGround) {
            if (Input.GetKeyDown("space")) {
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGround = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground")) onGround = true;
        else if (col.gameObject.CompareTag("Left Wall")) onLeftWall = true;
        else if (col.gameObject.CompareTag("Right Wall")) onRightWall = true;

        else if (col.gameObject.CompareTag("Enemy")) {
            //Debug.Log("Collided with Gomba!");
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
    }
}
