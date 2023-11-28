using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSet : MonoBehaviour {

    public List<Transform> pathPoints;

    void Start() {
        
    }

    void Update() {
        
    }

    public int Length() {
        return pathPoints.Count;
    }

    public Transform GetPoint(int index) {
        return pathPoints[index];
    }
}