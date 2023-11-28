using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour {
    public GameObject[] thingsToEnable;
    public GameObject[] thingsToDisable;
    private bool triggered;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter() {
        if(!triggered) {
            for(int i = 0; i < thingsToEnable.Length; i++) {
                GameObject go = thingsToEnable[i];
                go.SetActive(true);
            }
            for(int i = 0; i < thingsToDisable.Length; i++) {
                GameObject go = thingsToDisable[i];
                go.SetActive(false);
            }
            triggered = true;
        }
    }
}
