using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour {
    private Rigidbody2D enemyBody;
    private SpriteRenderer enemySprite;
    private Vector2 velocity;

    private int moveRight = 1;
    private float speed = 20;
    private float maxSpeed = 100;

    private int isJump = 0;

    void Start() {
        enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        // jump
        enemyBody.AddForce(Vector2.up * isJump * 2, ForceMode2D.Impulse);
        isJump = 0;

        // move
        velocity = new Vector2(moveRight * speed, 0);
        if (enemyBody.velocity.magnitude < maxSpeed) {
            enemyBody.AddForce(velocity);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        // one-third chance of jumping upon each collision
        isJump = Random.Range(1, 4);
        if (isJump != 1) {
            isJump = 0;
        }

        if (col.gameObject.CompareTag("Left Wall")) {
            moveRight = 1;
            enemySprite.flipX = false;
        } else if (col.gameObject.CompareTag("Right Wall")) {
            moveRight = -1;
            enemySprite.flipX = true;
        }
    }
}
