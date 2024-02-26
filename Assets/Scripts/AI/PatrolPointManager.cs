using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointManager : MonoBehaviour
{
    PatrolPoint[] points;
    void Start()
    {
        points = GetComponentsInChildren<PatrolPoint>();
    }

    public PatrolPoint GetRandomPoint(){
        return points[Random.Range(0, points.Length - 1)];
    }
}
