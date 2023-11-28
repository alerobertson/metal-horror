using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {
    public bool state = true;
    public Animator anim;
    public Renderer renderer;

    public Material greenLights;
    public Material redLights;
    public Material noLights;
    public GameObject[] thingsToEnable;
    public GameObject[] thingsToDisable;
    public Door[] doorsToOpen;
    public DoorSwitch[] switchesToUnlock;
    public AudioSource[] audioToStop;
    public float timeToActivate = 2f;

    private float timeSinceActivation = 0;
    private bool lightsSwitched = false;
    private Transform playerTransform;
    private Interactor interactor;

    void Start() {
        playerTransform = GameObject.Find("Player").transform;
        interactor = playerTransform.GetComponent<Interactor>();
    }

    void Update() {
        anim.SetBool("switchIsOn", state);
        if(!lightsSwitched && !state) {
            timeSinceActivation += Time.deltaTime;
            if(timeSinceActivation > timeToActivate) {
                SwitchLights();
                DisableThings();
                EnableThings();
                OpenDoors();
                EnableSwitches();
                StopAudio();
                interactor.LightSwitchPressed();
            }
        }
    }

    public void Activate() {
        state = false;
        anim.SetBool("switchIsOn", state);
    }

    private void SwitchLights() {
        lightsSwitched = true;
        Material[] materials = renderer.materials;
        materials[0] = noLights;
        materials[1] = redLights;
        renderer.materials = materials;
    }

    private void DisableThings() {
        for(int i = 0; i < thingsToDisable.Length; i++) {
            GameObject go = thingsToDisable[i];
            go.SetActive(false);
        }
    }

    private void EnableThings() {
        for(int i = 0; i < thingsToEnable.Length; i++) {
            GameObject go = thingsToEnable[i];
            go.SetActive(true);
        }
    }

    private void OpenDoors() {
        for(int i = 0; i < doorsToOpen.Length; i++) {
            Door door = doorsToOpen[i];
            door.OpenDoor();
        }
    }

    private void EnableSwitches() {
        for(int i = 0; i < switchesToUnlock.Length; i++) {
            DoorSwitch doorSwitch = switchesToUnlock[i];
            doorSwitch.isLocked = false;
        }
    }

    private void StopAudio() {
        for(int i = 0; i < audioToStop.Length; i++) {
            AudioSource audio = audioToStop[i];
            audio.Stop();
        }
    }
}
