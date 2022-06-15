using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
    public Text score;
    private int playerScore = 0;
    public delegate void gameEvent();
    public static event gameEvent OnPlayerDeath;

    public void increaseScore(int scoreIncrease) {
        playerScore += scoreIncrease;
        score.text = "Score: " + playerScore.ToString();
    }

    public void damagePlayer() {
        OnPlayerDeath();
    }
}
