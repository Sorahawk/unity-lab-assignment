using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour {
    private float originalX;
    private float maxOffset = 6.0f;
    private float enemyPatrolTime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;

    void Start() {
        enemyBody = GetComponent<Rigidbody2D>();
        originalX = transform.position.x;
        ComputeVelocity();        
    }

    void Update() {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset) {
            MoveGomba();
        } else {
            moveRight *= -1;
            ComputeVelocity();
            MoveGomba();
        }
    }

    void ComputeVelocity() {
        velocity = new Vector2(moveRight * maxOffset / enemyPatrolTime, 0);
    }

    void MoveGomba() {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }
}
