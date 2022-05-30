using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestionBoxController : MonoBehaviour {
    public Rigidbody2D rigidBody;
    public SpringJoint2D springJoint;
    public GameObject consumablePrefab;
    public SpriteRenderer spriteRenderer;
    public Sprite usedQuestionBox;
    private bool hit = false;

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player") && !hit) {
            hit = true;
            Instantiate(consumablePrefab, new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), Quaternion.identity);
        }
    }
}
