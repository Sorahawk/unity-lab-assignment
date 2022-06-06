using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : MonoBehaviour {
    private bool isBroken = false;
    public GameObject debrisPrefab;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !isBroken) {
            isBroken = true;

            for (int x = 0; x < 10; x++) Instantiate(debrisPrefab, transform.position, Quaternion.identity);

            gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<EdgeCollider2D>().enabled = false;

            Destroy(gameObject);
        }
    }
}
