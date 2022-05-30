using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConsumableMushroomController : MonoBehaviour {
    private Rigidbody2D mushroomBody;
    private Vector2 velocity;
    private int speed = 10;
    private int springSpeed = 7;
    private bool moveRight;
    private bool stop = false;

    void Start() {
        mushroomBody = GetComponent<Rigidbody2D>();

        int random = Random.Range(0, 2);
        if (random == 0) moveRight = false;
        else moveRight = true;

        mushroomBody.AddForce(Vector2.up * springSpeed, ForceMode2D.Impulse);
    }

    void FixedUpdate() {
        if (!stop) {
            if (moveRight) velocity = new Vector2(speed, mushroomBody.velocity.y);
            else velocity = new Vector2(-speed, mushroomBody.velocity.y);

            mushroomBody.velocity = velocity;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        moveRight = !moveRight;

        if (col.gameObject.CompareTag("Player")) {
            stop = true;
            mushroomBody.velocity = Vector2.zero;
        }
    }
}
