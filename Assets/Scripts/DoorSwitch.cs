using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour {
    public Door door;
    public bool isLocked = false;
    public Material greenLights;
    public Material redLights;
    public Renderer renderer;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Material[] materials = renderer.materials;
        if(isLocked) {
            materials[0] = redLights;
        }
        else {
            materials[0] = greenLights;
        }
        renderer.materials = materials;
    }

    public void Press() {
        if(!isLocked) {
            door.ToggleDoor();
        }
    }
}
