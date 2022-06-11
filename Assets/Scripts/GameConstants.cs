using UnityEngine;


[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject {
    int currentScore;
    int currentPlayerHealth;

    Vector3 turtleSpawnPointStart = new Vector3(2.5f, -0.45f, 0);

    public int consumeTimeStep = 10;
    public int consumeLargestScale = 4;

    public int breakTimeStep = 30;
    public int breakDebrisTorque = 10;
    public int breakDebrisForce = 10;

    public int spawnNumberOfDebris = 10;

    public int rotatorRotateSpeed = 6;

    public int testValue;

    public int highScore;
}
