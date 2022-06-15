using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CentralManager : MonoBehaviour {
    public GameObject gameManagerObject;
    private GameManager gameManager;
    public static CentralManager centralManagerInstance;
    public GameObject powerupManagerObject;
    private PowerUpManager powerUpManager;

    void Awake() {
        centralManagerInstance = this;
    }

    void Start() {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        powerUpManager = powerupManagerObject.GetComponent<PowerUpManager>();
    }

    public void increaseScore(int score) {
        gameManager.increaseScore(score);
    }

    public void damagePlayer() {
        gameManager.damagePlayer();
    }

    public void consumePowerup(KeyCode k, GameObject g) {
        powerUpManager.consumePowerup(k, g);
    }

    public void addPowerup(Texture t, int i, ConsumableInterface c) {
        powerUpManager.addPowerup(t, i, c);
    }
}
