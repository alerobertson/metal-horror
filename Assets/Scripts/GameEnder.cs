using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour {
    public float timeTillGameEnd = 3f;
    public GameObject[] thingsToDisable;
    public LightSwitch finalSwitch;
    private float currentTime = 0;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        currentTime += Time.deltaTime;
        if(currentTime > timeTillGameEnd) {
            if(finalSwitch.state) {
                SceneManager.LoadScene("Game Over");
            }
            else {
                SceneManager.LoadScene("Good Over");
            }
            // Application.Quit();
        }
    }
}
