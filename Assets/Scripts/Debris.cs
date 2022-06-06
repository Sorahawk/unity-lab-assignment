using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {
    private Rigidbody2D debrisBody;
    private Vector3 scaler;

    void Start() {
        debrisBody = GetComponent<Rigidbody2D>();
        scaler = transform.localScale / 30.0f;

        StartCoroutine("ScaleOut");
    }

    IEnumerator ScaleOut() {
        Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), 1);
        debrisBody.AddForce(direction.normalized * 10, ForceMode2D.Impulse);
        debrisBody.AddTorque(10, ForceMode2D.Impulse);

        yield return null;

        for (int step = 0; step < 30; step++) {
            this.transform.localScale = this.transform.localScale - scaler;
            yield return null;
        }

        Destroy(gameObject);
    }
}
