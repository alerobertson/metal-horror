using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoKillPlayer : MonoBehaviour {
    public NavMeshAgent agent;
    private Transform playerTransform;
    public GameObject killer;
    public Animator anim;
    private float timeSinceSpawn = 0;
    // Start is called before the first frame update
    void Start() {
        playerTransform = GameObject.Find("Player").transform;
        agent.speed = 15;
        anim.SetBool("chase", true);
        // agent.SetDestination(playerTransform.position);
    }

    // Update is called once per frame
    void Update() {
        agent.SetDestination(playerTransform.position);
        if(agent.remainingDistance < 1f && timeSinceSpawn > 0.5f) {
            killer.SetActive(true);
            playerTransform.gameObject.SetActive(false);
            transform.gameObject.SetActive(false);
        }
        timeSinceSpawn += Time.deltaTime;
    }
}
