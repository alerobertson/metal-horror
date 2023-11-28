using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interactor : MonoBehaviour {
    public Camera camera;
    public LightSwitch[] lightSwitches;
    public Text alertText;
    public GameObject cursor;

    private int switchesRemaining;
    public float alertUpTime = 0;
    private bool alert = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        int switchCounter = 0;
        for(int i = 0; i < lightSwitches.Length; i++) {
            LightSwitch lightSwitch = lightSwitches[i];
            if(lightSwitch.state) {
                switchCounter++;
            }
        }
        switchesRemaining = switchCounter;
        Transform obj = ObjectInSight();
        if(IsLightSwitch(obj) || IsDoorSwitch(obj)) {
            if(Vector3.Distance(obj.position, transform.position) < 1f) {
                cursor.SetActive(true);
            }
            else {
                cursor.SetActive(false);
            }
        }
        else {
            cursor.SetActive(false);
        }

        if(obj.GetComponent<LookAtMeBox>() != null) {
            LookAtMeBox box = obj.GetComponent<LookAtMeBox>();
            box.LookedAt();
        }

        if(alertUpTime < 6f && switchesRemaining < 5) {
            alertText.gameObject.SetActive(true);
        }
        else {
            alertText.gameObject.SetActive(false);
        }
        alertUpTime += Time.deltaTime;
    }

    public void OnFire(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Started) {
            Transform objectInSight = ObjectInSight();
            if(Vector3.Distance(objectInSight.position, transform.position) < 1f) {
                if(IsLightSwitch(objectInSight)) {
                    LightSwitch lightSwitch = objectInSight.GetComponent<LightSwitch>();
                    if(lightSwitch.state) {
                        lightSwitch.Activate();
                    }
                }

                if(IsDoorSwitch(objectInSight)) {
                    DoorSwitch doorSwitch = objectInSight.GetComponent<DoorSwitch>();
                    doorSwitch.Press();
                }
            }

        }
    }

    private bool IsLightSwitch(Transform obj) {
        return obj.GetComponent<LightSwitch>() != null;
    }

    private bool IsDoorSwitch(Transform obj) {
        return obj.GetComponent<DoorSwitch>() != null;
    }

    private Transform ObjectInSight() {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Transform objectHit = null;
        
        if (Physics.Raycast(ray, out hit)) {
            objectHit = hit.transform;
        }

        return objectHit;
    }

    public void LightSwitchPressed() {
        switch(switchesRemaining) {
            case 1:
                alertText.text = "One Switch Left";
                break;
            case 0:
                alertText.text = "It Can Hear You";
                break;
            default:
                alertText.text = switchesRemaining + " Switches Left";
                break;
        }
        alertUpTime = 0;
    }
}
