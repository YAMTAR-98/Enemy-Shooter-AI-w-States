using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    PatrolPointManager patrolPoints;

    void Start()
    {
        patrolPoints = GetComponentInParent<PatrolPointManager>();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.3f);
    }
}
