using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour {
    public AudioSource gameMusic;

    void Awake() {
        Time.timeScale = 0.0f;

        // start music
        gameMusic = GameObject.Find("Music").GetComponent<AudioSource>();
        gameMusic.Play();
    }

    public void StartButtonClicked() {
        foreach (Transform eachChild in transform) {
            if (eachChild.name != "Score") {
                eachChild.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }

    public void RestartButtonClicked() {
        SceneManager.LoadScene(0);
    }
}
