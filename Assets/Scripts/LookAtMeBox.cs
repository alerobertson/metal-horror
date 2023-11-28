using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMeBox : MonoBehaviour {
    public GameObject[] thingsToEnable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookedAt() {
        for(int i = 0; i < thingsToEnable.Length; i++) {
            GameObject go = thingsToEnable[i];
            go.SetActive(true);
        }
    }
}
