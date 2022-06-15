using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour {
    public static SpawnManager spawnManagerInstance;
    public GameConstants gameConstants;

    public void spawnFromPooler(ObjectType i) {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);

        if (item != null) {
            item.transform.position = new Vector3(Random.Range(-4.5f, 4.5f), item.transform.position.y, 0);
            item.SetActive(true);
        } else Debug.Log("not enough items in the pool");
    }

    void Awake() {
        Debug.Log(ObjectPooler.SharedInstance);

        spawnManagerInstance = this;
        for (int j = 0; j < 2; j++) spawnFromPooler(ObjectType.turtleEnemy);
    }
}
