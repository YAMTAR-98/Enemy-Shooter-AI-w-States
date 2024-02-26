using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain_Stupid : MonoBehaviour
{
    public Transform target;
    private EnemyReferances enemyReferances;
    float pathUpdateDeadline;
    float shootingDistance;
    private void Awake() {
        enemyReferances = GetComponent<EnemyReferances>();
    }

    void Start()
    {
        shootingDistance = enemyReferances.navMeshAgent.stoppingDistance;
    }


    void Update()
    {
        if(target != null){
            bool inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;

            if(inRange)
                LookAtTarget();
            else
                UpdatePath();

            enemyReferances.animator.SetBool("Shooting", inRange);
        }
        enemyReferances.animator.SetFloat("Speed", enemyReferances.navMeshAgent.desiredVelocity.sqrMagnitude);
    }
    void UpdatePath(){
        if(Time.time >= pathUpdateDeadline){
            Debug.Log("Updating Path");
            pathUpdateDeadline = Time.time + enemyReferances.pathUpdateDelay;
            enemyReferances.navMeshAgent.SetDestination(target.position);
        }
    }
    void LookAtTarget(){
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2.0f);
    } 
}
