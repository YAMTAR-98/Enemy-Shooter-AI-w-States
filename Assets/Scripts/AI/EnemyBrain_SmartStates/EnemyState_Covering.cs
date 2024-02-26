using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Covering : IState
{
    private EnemyReferances enemyReferances;
    private StateMachine stateMachine;

    public EnemyState_Covering(EnemyReferances enemyReferances){
        this.enemyReferances = enemyReferances;
        stateMachine = new StateMachine();

    }
    public void OnEnter()
    {
        Debug.Log("Covering");
        enemyReferances.animator.SetBool("Cover", true);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }
}
