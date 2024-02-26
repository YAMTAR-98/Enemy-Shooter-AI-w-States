using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_RunToCover : IState
{
    EnemyReferances enemyReferances;
    CoverArea coverArea;
    public EnemyState_RunToCover(EnemyReferances enemyReferances, CoverArea coverArea){
        this.enemyReferances = enemyReferances;
        this.coverArea = coverArea;
    }
    public void OnEnter()
    {
        Debug.Log("RunToCover");
        Cover nextCover = this.coverArea.GetRandomCover(enemyReferances.transform.position);
        enemyReferances.navMeshAgent.SetDestination(nextCover.transform.position);
        enemyReferances.navMeshAgent.speed = 3.5f;
        enemyReferances.animator.SetFloat("Speed", 1f);
    }

    public void OnExit()
    {
        enemyReferances.animator.SetFloat("Speed", 0f);
    }

    public void Tick()
    {
        enemyReferances.animator.SetFloat("Speed", enemyReferances.navMeshAgent.desiredVelocity.sqrMagnitude);
    }
    public Color GizmoColor(){
        return Color.blue;
    }
    public bool HasArrivedDestination(bool recalculate){
        if(recalculate)
            return false;
        else
            return enemyReferances.navMeshAgent.remainingDistance <=0.1f;
    }

}
