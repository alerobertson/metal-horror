using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    PATROL,
    FOLLOWING,
    CHEAT
}

public class EnemyMovement : MonoBehaviour {
    public float movementSpeed = 1;
    public NavMeshAgent agent;
    public PathSet pathSet;
    public int pathStartingPoint = 0;
    public LayerMask playerScanMask;
    public bool debugLines = false;
    public Transform playerScanStartPoint;
    public float visionLimit = 45;
    public float cheatTimeLimit = 1f;
    public EnemyState currentState;
    public Animator anim;

    private int destinationIndex = 0;
    private Transform playerTransform;
    private float timeSeenPlayer = 0;
    private float timeCheating = 0;
    private Vector3 playerLastSeenLocation;
    public GameObject killer;
    public AudioSource runningSound;

    // Start is called before the first frame update
    void Start() {
        if(pathSet != null) {
            destinationIndex = pathStartingPoint;
            Transform destination = pathSet.GetPoint(destinationIndex);
            agent.SetDestination(destination.position);
        }
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        anim.SetBool("chase", currentState != EnemyState.PATROL);
        if(pathSet == null) { return; }
        switch(currentState) {
            case EnemyState.PATROL:
                agent.speed = 0.8f;
                // Handle PathSet
                if(agent.remainingDistance <= 0) {
                    destinationIndex++;
                    if(destinationIndex >= pathSet.Length()) {
                        destinationIndex = 0;
                    }
                    Transform destination = pathSet.GetPoint(destinationIndex);
                    agent.SetDestination(destination.position);
                }
                // Handle Player Spotting
                if(CanSeePlayer()) {
                    timeSeenPlayer += Time.deltaTime;
                    if(timeSeenPlayer > 1f) {
                        BeginHunting();
                    }
                }
                else {
                    if(timeSeenPlayer > 0) {
                        timeSeenPlayer -= Time.deltaTime;
                    }
                }
                break;
            case EnemyState.FOLLOWING:
                agent.speed = 2;
                if(CanSeePlayer()) {
                    agent.SetDestination(playerTransform.position);
                }
                else {
                    if(agent.remainingDistance <= 0) {
                        StartCheating();
                    }
                }
                break;
            case EnemyState.CHEAT:
                agent.speed = 2;
                if(CanSeePlayer()) {
                    // Immediately switch to FOLLOWING if player is in sight
                    currentState = EnemyState.FOLLOWING;
                }
                else {
                    agent.SetDestination(playerTransform.position);
                    timeCheating += Time.deltaTime;
                    if(timeCheating >= cheatTimeLimit) {
                        StopHunting();
                    }
                }
                break;
        }

        if(currentState != EnemyState.PATROL) {
            if(DistanceToPlayer() < 1f) {
                killer.SetActive(true);
                playerTransform.gameObject.SetActive(false);
                transform.gameObject.SetActive(false);
            }
        }
    }

    private bool CanSeePlayer() {
        bool canSee = false;

        float distanceBetween = Vector3.Distance(playerTransform.position, playerScanStartPoint.position);
        Vector3 direction = playerTransform.position - playerScanStartPoint.position;
        float visionAngle = Mathf.Abs(Vector3.Angle(transform.forward, direction));

        // Check if looking in a valid direction before proceeding
        if(visionAngle < visionLimit) {
            RaycastHit playerSightHit;
            if (Physics.Raycast(playerScanStartPoint.position, direction, out playerSightHit, distanceBetween, playerScanMask)) {
                if(debugLines) { Debug.DrawLine(playerScanStartPoint.position, playerSightHit.point, Color.yellow); }
                if(playerSightHit.transform.gameObject.layer == 8) {
                    canSee = true;
                    if(debugLines) { Debug.DrawLine(playerScanStartPoint.position, playerSightHit.point, Color.red); }
                }
            }
        }

        return canSee;
    }

    private void StopHunting() {
        timeSeenPlayer = 0;
        runningSound.Stop();
        currentState = EnemyState.PATROL;
        Transform destination = pathSet.GetPoint(destinationIndex);
        agent.SetDestination(destination.position);
    }

    private void BeginHunting() {
        agent.SetDestination(playerTransform.position);
        currentState = EnemyState.FOLLOWING;
        runningSound.Play();
    }

    private void StartCheating() {
        timeCheating = 0;
        currentState = EnemyState.CHEAT;
    }

    private float DistanceToPlayer() {
        return Vector3.Distance(transform.position, playerTransform.position);
    }
}
