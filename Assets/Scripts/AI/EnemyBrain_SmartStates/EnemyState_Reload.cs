using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Reload : IState
{
    EnemyReferances enemyReferances;
    float deadLine = 1.25f;

    public EnemyState_Reload(EnemyReferances enemyReferances){
        this.enemyReferances = enemyReferances;  
    }

    public void OnEnter()
    {
        Debug.Log("Reloading");
        enemyReferances.animator.SetBool("Cover", true);
        enemyReferances.animator.SetTrigger("Reload"); 
    }

    public void OnExit()
    {
        enemyReferances.animator.SetBool("Cover", false);
    }

    public void Tick()
    {
        
    }
    public Color GizmoColor(){
        return Color.gray;
    }

}
