using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour {
    private Rigidbody2D enemyBody;
    private SpriteRenderer enemySprite;
    private Vector2 velocity;
    public GameConstants gameConstants;

    private int moveRight = 1;
    private float speed = 20;
    private float maxSpeed = 100;

    private int isJump = 0;

    void Start() {
        enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();

        GameManager.OnPlayerDeath += EnemyRejoice;
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
        if (isJump != 1) isJump = 0;

        if (col.gameObject.CompareTag("Left Wall")) {
            moveRight = 1;
            enemySprite.flipX = false;
        } else if (col.gameObject.CompareTag("Right Wall")) {
            moveRight = -1;
            enemySprite.flipX = true;
        } else if (col.gameObject.CompareTag("Enemy")) {
            if (moveRight == 1) moveRight = -1;
            else moveRight = 1;
            enemySprite.flipX = !enemySprite.flipX;
        }

        else if (col.gameObject.CompareTag("Player")) {
            float collisionDirection = col.gameObject.transform.position.y - transform.position.y;

            if (collisionDirection > 0.8f) {
                GetComponent<Collider2D>().enabled = false;
                KillSelf();
            }
            else CentralManager.centralManagerInstance.damagePlayer();
        }
    }

    public void KillSelf() {
        CentralManager.centralManagerInstance.increaseScore(15);
        StartCoroutine(Flatten());
        SpawnManager.spawnManagerInstance.spawnFromPooler(ObjectType.turtleEnemy);
        Debug.Log("Enemy killed");
    }

    IEnumerator Flatten() {
        int steps = 5;
        float stepper = 1.0f / (float) steps;

        for (int i = 0; i < steps; i++) {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z);

            this.transform.position = new Vector3(this.transform.position.x, gameConstants.groundSurface + GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
            yield return null;
        }

        this.gameObject.SetActive(false);
        yield break;
    }

    void EnemyRejoice() {
        Debug.Log("Enemy killed Mario");
        StartCoroutine(Dance());
    }

    IEnumerator Dance() {
        for (int i = 0; i < 100; i++) {
            enemySprite.flipX = !enemySprite.flipX;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
