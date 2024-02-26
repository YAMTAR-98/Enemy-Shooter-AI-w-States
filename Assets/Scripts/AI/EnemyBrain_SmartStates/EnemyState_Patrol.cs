using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Patrol : IState
{
    EnemyReferances enemyReferances;
    PatrolPointManager patrolPoint;
    EnemyState_PatrolDelay patrolDelay;
    StateMachine stateMachine;

    public EnemyState_Patrol(EnemyReferances enemyReferances, PatrolPointManager patrolPoint, float delay){
        this.enemyReferances = enemyReferances;
        this.patrolPoint = patrolPoint;
        stateMachine = new StateMachine();

        patrolDelay = new EnemyState_PatrolDelay(delay);


        At(this, patrolDelay, () => HasArrivedDestination());
        At(patrolDelay, this, () => patrolDelay.IsDone());

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);
    }
    public void OnEnter()
    {
        PatrolPoint patrolPoint = this.patrolPoint.GetRandomPoint();
        enemyReferances.navMeshAgent.SetDestination(patrolPoint.transform.position);
        enemyReferances.animator.SetFloat("Speed", 0.5f);
    }
    
    public void OnExit()
    {
        enemyReferances.animator.SetFloat("Speed", 0f);
    }

    public void Tick()
    {   }
        
    public bool HasArrivedDestination(){
        return enemyReferances.navMeshAgent.remainingDistance <=0.1f;
    }
}
