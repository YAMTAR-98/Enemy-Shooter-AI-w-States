using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyReferances : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public EnemyShooter enemyShooter;
    public CoverArea coverArea;

    [Header("Stats")]
    public float pathUpdateDelay = 0.2f;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyShooter = GetComponent<EnemyShooter>();
        coverArea = FindObjectOfType<CoverArea>();
    }
}
