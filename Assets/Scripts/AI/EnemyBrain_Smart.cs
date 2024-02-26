using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyBrain_Smart : MonoBehaviour
{
    private EnemyReferances enemyReferances;
    private StateMachine stateMachine;
    CoverArea coverArea;
    PatrolPointManager patrolPoint;
    EnemyState_RunToCover runToCover;
    
    EnemyState_Patrol patrol;
    EnemyState_PatrolDelay patrolDelayState;
    EnemyState_Shoot shoot;

    [Header("AI Brain")]
    public float patrolDelay = 5f;
    public Transform target;
    public bool attack;

    [Header("FOV")]
    public float radius;
    [Range(0, 360)] public float angle;
    public GameObject playerRef;
    public LayerMask playerMask, enviromentMask;
    public bool canSeePlayer;

    void Start()
    {
        enemyReferances = GetComponent<EnemyReferances>();
        stateMachine = new StateMachine();
        coverArea = FindObjectOfType<CoverArea>();
        patrolPoint = FindObjectOfType<PatrolPointManager>();
        playerRef = target.gameObject;

        //State Referances
        runToCover = new EnemyState_RunToCover(enemyReferances, coverArea);
        patrol = new EnemyState_Patrol(enemyReferances, patrolPoint, patrolDelay);
        patrolDelayState = new EnemyState_PatrolDelay(patrolDelay);
        shoot = new EnemyState_Shoot(enemyReferances, playerRef.transform);

        RunBoyRun();
        StartCoroutine(DistanceCheckDelay());
        StartCoroutine(FOVRoutine());
    }
    public void RunBoyRun(){
        //STATES
        if(attack){
            var delayAfterRun = new EnemyState_Delay(2f);
            var cover = new EnemyState_Cover(enemyReferances, target);

            At(runToCover, delayAfterRun, () => runToCover.HasArrivedDestination(false));
            At(delayAfterRun, cover, () => delayAfterRun.IsDone());
            Debug.Log("RUN BOY RUN ATTACK");
        }else if(canSeePlayer){
            Any(shoot, () => canSeePlayer);
        }

        //Start State
        if(coverArea.covers.Count > 0 && attack){
            stateMachine.SetState(runToCover);
            enemyReferances.navMeshAgent.speed = 3.5f;
        }
        else if(!attack){
            At(patrol, patrolDelayState, () => patrol.HasArrivedDestination());
            At(patrolDelayState, patrol, () => patrolDelayState.IsDone());
            
            stateMachine.SetState(patrol);
            enemyReferances.navMeshAgent.speed = 1.75f;
        }
        else{
            Debug.Log("No Near Cover");// TODO: Search And Destroy
            attack = false;            //Done but Check again...
            canSeePlayer = false;
            Any(patrol, () => !attack && !canSeePlayer);
            
        }

        //Fonctions And Coditions
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);
    }

    void Update()
    {
        stateMachine.Tick();
    }

    public IEnumerator FOVRoutine(){
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while(true){
            yield return wait;
            if(!attack)
                FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerMask);
        if(rangeChecks.Length != 0){
            Transform m_FOVTarget = rangeChecks[0].transform;
            Vector3 directionToTarget = (m_FOVTarget.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < angle/2){
                float distanceToTarget = Vector3.Distance(transform.position, m_FOVTarget.position);
                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, enviromentMask))
                    canSeePlayer = true;
                else  
                    canSeePlayer = false;
            }else   
                canSeePlayer = false;
        }else if(canSeePlayer)
            canSeePlayer = false;
        
        EnemyInFOV();
    }

    void EnemyInFOV(){
        if(canSeePlayer){
            stateMachine.SetState(shoot);
            enemyReferances.navMeshAgent.SetDestination(transform.position);
        }
    }

    public IEnumerator DistanceCheckDelay(){
        bool oneTime = true;
        yield return new WaitUntil(GetComponent<EnemyShooter>().ShouldBeReload);
        if(GetComponent<EnemyShooter>().ShouldBeReload() && !attack){
            attack = true;
        }
        while(true){
            yield return new WaitForSeconds(2f);
            if(Vector3.Distance(target.position, transform.position) > 30f && attack){
                enemyReferances.animator.SetBool("Shooting", false);
                enemyReferances.animator.SetBool("Combat", false);
                enemyReferances.animator.SetBool("Cover", false);
                enemyReferances.animator.SetFloat("Speed", 1f);
                runToCover.HasArrivedDestination(false);
                stateMachine.SetState(runToCover);
                RunBoyRun();
            }else if (attack && runToCover.HasArrivedDestination(false) && oneTime){
                RunBoyRun();
                oneTime = false;
                enemyReferances.animator.SetBool("Shooting", false);
                enemyReferances.animator.SetBool("Combat", true);
            }
        }
    }

    private void OnDrawGizmos(){
        if(stateMachine  != null){
            Gizmos.color = stateMachine.SetGizmoColor();
            Gizmos.DrawSphere(transform.position + Vector3.up * 3, 0.4f);
        }
    }

    private void OnDestroy(){
        StopAllCoroutines();
    }
}
