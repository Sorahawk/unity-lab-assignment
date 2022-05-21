using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour {
    private Rigidbody2D enemyBody;
    private Vector2 velocity;

    private int moveRight = 1;
    private float speed = 30;
    private float maxSpeed = 100;

    void Start() {
        enemyBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        velocity = new Vector2(moveRight * speed, 0);

        if (enemyBody.velocity.magnitude < maxSpeed) {
            enemyBody.AddForce(velocity);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Left Wall")) {
            moveRight = 1;
        } else if (col.gameObject.CompareTag("Right Wall")) {
            moveRight = -1;
        }
    }
}
