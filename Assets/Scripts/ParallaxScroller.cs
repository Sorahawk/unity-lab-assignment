using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParallaxScroller : MonoBehaviour {
    public Renderer[] layers;
    public Transform player;
    public Transform mainCamera;
    public float[] speedMultiplier;

    private float previousXPositionPlayer;
    private float previousXPositionCamera;
    private float[] offset;

    void Start() {
        offset = new float[layers.Length];

        for (int i = 0; i < layers.Length; i++) offset[i] = 0.0f;

        previousXPositionPlayer = player.transform.position.x;
        previousXPositionCamera = mainCamera.transform.position.x;
    }

    void Update() {
        if (Mathf.Abs(previousXPositionCamera - mainCamera.transform.position.x) > 0.001f) {
            for (int i = 0; i < layers.Length; i++) {
                if (offset[i] > 1.0f || offset[i] < -1.0f) offset[i] = 0.0f;

                float newOffset = player.transform.position.x - previousXPositionPlayer;
                offset[i] = offset[i] + newOffset * speedMultiplier[i];
                layers[i].material.mainTextureOffset = new Vector2(offset[i], 0);
            }
        }

        previousXPositionPlayer = player.transform.position.x;
        previousXPositionCamera = mainCamera.transform.position.x;
    }
}
