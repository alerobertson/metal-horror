using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public Animator anim;
    public bool open;
    public BoxCollider doorCollider;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        anim.SetBool("open", open);
        doorCollider.enabled = !open;
    }

    public void OpenDoor() {
        open = true;
        audioSource.Play();
    }

    public void CloseDoor() {
        open = false;
        audioSource.Play();
    }

    public void ToggleDoor() {
        open = !open;
        audioSource.Play();
    }
}
