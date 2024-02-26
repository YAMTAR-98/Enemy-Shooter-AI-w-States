using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_PatrolDelay : IState
{
    private float waitForSercond;
    private float deadLine;
    public EnemyState_PatrolDelay(float waitForSercond){
        this.waitForSercond = waitForSercond;
    }
    public void OnEnter()
    {
        deadLine = Time.time + waitForSercond;
    }

    public void OnExit()
    {
        Debug.Log("Enemy Delay");
    }

    public void Tick()
    {
        
    }
    public Color GizmoColor(){
        return Color.white;
    }
    public bool IsDone(){
        return Time.time >= deadLine;
    }
}
