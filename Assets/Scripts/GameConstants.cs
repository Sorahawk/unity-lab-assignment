using UnityEngine;


[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject {
    int currentScore;

    public int highScore;

    public int groundSurface = -6;

    public int playerSpeed = 100;
}
