using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PowerUpManager : MonoBehaviour {
    public List<GameObject> powerupIcons;
    private List<ConsumableInterface> powerups;

    void Start() {
        powerups = new List<ConsumableInterface>();

        for (int i = 0; i < powerupIcons.Count; i++) {
            powerupIcons[i].SetActive(false);
            powerups.Add(null);
        }
    }

    public void addPowerup(Texture texture, int index, ConsumableInterface i) {
        if (index < powerupIcons.Count) {
            Debug.Log("adding powerup");

            powerupIcons[index].GetComponent<RawImage>().texture = texture;
            powerupIcons[index].SetActive(true);
            powerups[index] = i;
        }
    }

    public void removePowerup(int index) {
        if (index < powerupIcons.Count) {
            powerupIcons[index].SetActive(false);
            powerups[index] = null;
        }
    }

    void cast(int i, GameObject p) {
        if (powerups[i] != null) {
            powerups[i].consumedBy(p);
            removePowerup(i);
        }
    }

    public void consumePowerup(KeyCode k, GameObject player) {
        switch(k) {
            case KeyCode.Z:
                cast(0, player);
                break;
            case KeyCode.X:
                cast(1, player);
                break;
            default:
                break;
        }
    }
}
