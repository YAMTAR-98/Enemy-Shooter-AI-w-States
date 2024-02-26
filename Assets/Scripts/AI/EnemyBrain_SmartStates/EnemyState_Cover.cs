
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Cover : IState
{
    EnemyReferances enemyReferances;
    private StateMachine stateMachine;
    public EnemyState_Cover(EnemyReferances enemyReferances, Transform target){
        this.enemyReferances = enemyReferances;

        stateMachine = new StateMachine();

        //STATES
        var enemyShoot = new EnemyState_Shoot(enemyReferances, target);
        var enemyDelay = new EnemyState_Delay(0.5f);
        var enemyReload = new EnemyState_Reload(enemyReferances);
        var enemyCovering = new EnemyState_Covering(enemyReferances);
        

        //Transitions
        At(enemyShoot, enemyCovering, () => enemyReferances.enemyShooter.ShouldBeReload());
        At(enemyCovering, enemyDelay, () => enemyReferances.enemyShooter.ShouldBeReload());
        At(enemyDelay, enemyReload, () => enemyDelay.IsDone() && enemyReferances.enemyShooter.ShouldBeReload());
        At(enemyReload, enemyDelay, () => !enemyReferances.enemyShooter.ShouldBeReload());
        At(enemyDelay, enemyShoot, () => enemyDelay.IsDone() && !enemyReferances.enemyShooter.ShouldBeReload());

        //Start State
        stateMachine.SetState(enemyShoot);

        //Fonctions And Coditions
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);
    }
    public void OnEnter()
    {
        Debug.Log("Cover");
        enemyReferances.animator.SetBool("Combat", true);
    }

    public void OnExit()
    {
        enemyReferances.animator.SetBool("Cover", false);
        enemyReferances.animator.SetBool("Combat", true);


    }

    public void Tick()
    {
        stateMachine.Tick();
    }
    public Color GizmoColor(){
        return stateMachine.GetGizmoColor(Color.yellow);
    }

}
