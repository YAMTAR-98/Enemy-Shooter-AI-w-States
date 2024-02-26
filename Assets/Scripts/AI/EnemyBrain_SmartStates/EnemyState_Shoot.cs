using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyState_Shoot : IState
{
    EnemyReferances enemyReferances;
    Transform target;
    StateMachine stateMachine;


    public EnemyState_Shoot(EnemyReferances enemyReferances, Transform target){
        this.enemyReferances = enemyReferances;
        this.target = target;
        stateMachine = new StateMachine();
        var reload = new EnemyState_Reload(enemyReferances);
        

        At(this, reload, () => enemyReferances.GetComponent<EnemyShooter>().ShouldBeReload());


        //Fonctions And Coditions
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);
    }
    public void OnEnter()
    {
        Debug.Log("Shoot");
        enemyReferances.animator.SetBool("Combat", true);
        enemyReferances.animator.SetBool("Shooting", true);
    }

    public void OnExit()
    {
        Debug.Log("Stopped Shooting");
        enemyReferances.animator.SetBool("Shooting", false);
    }

    public void Tick()
    {
        if(target != null ){
            Vector3 lookPos = target.position - enemyReferances.transform.position;
            lookPos.y = 0;
            quaternion rotation = Quaternion.LookRotation(lookPos);
            enemyReferances.transform.rotation = Quaternion.Slerp(enemyReferances.transform.rotation, rotation, 0.2f);

            enemyReferances.animator.SetBool("Shooting", true);
        }
    }
    public Color GizmoColor(){
        return Color.red;
    }

}
