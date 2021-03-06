using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrangeMushroom : MonoBehaviour, ConsumableInterface {
    public Texture t;
    private Rigidbody2D mushroomBody;
    private SpriteRenderer mushroomSprite;
    private Vector2 velocity;
    private int speed = 10;
    private int springSpeed = 10;
    private bool moveRight;
    private bool stop = false;

    void Start() {
        mushroomBody = GetComponent<Rigidbody2D>();
        mushroomSprite = GetComponent<SpriteRenderer>();

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
        if (col.gameObject.CompareTag("Left Wall") || col.gameObject.CompareTag("Right Wall") || col.gameObject.CompareTag("Pillars")) {
            moveRight = !moveRight;
        }

        else if (col.gameObject.CompareTag("Player")) {
            gameObject.SetActive(false);
            Debug.Log("orange mushroom collected");
            CentralManager.centralManagerInstance.addPowerup(t, 1, this);
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void consumedBy(GameObject player) {
        player.GetComponent<PlayerController>().maxSpeed *= 2;
        StartCoroutine(removeEffect(player));
    }

    IEnumerator removeEffect(GameObject player) {
        yield return new WaitForSeconds(5.0f);
        player.GetComponent<PlayerController>().maxSpeed /= 2;
    }
}
