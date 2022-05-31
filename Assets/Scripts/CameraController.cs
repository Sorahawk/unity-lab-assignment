using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {
    public Transform player;
    public Transform endLimit;

    private float offset;
    private float startX;
    private float endX;
    private float viewportHalfWidth;

    void Start() {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);

        offset = this.transform.position.x - player.position.x;
        startX = this.transform.position.x;
        endX = endLimit.transform.position.x - viewportHalfWidth;
    }

    void Update() {
        float desiredX = player.position.x + offset;

        if (desiredX > startX && desiredX < endX) {
            this.transform.position = new Vector3(desiredX, this.transform.position.y, this.transform.position.z);
        }
    }
}
