using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{   
    [Header("General")]
    public Transform shootPoint;
    public Transform gunPoint;
    public LayerMask layerMask;

    [Header("Gun")]
    public Vector3  spread = new Vector3(0.06f,0.06f,0.06f);
    public TrailRenderer bullettrail;
    public GameObject bulletImpact;
    private EnemyReferances enemyReferances;
    public int ammo = 30;
    int currentAmmo = 30;
    private void Awake() {
        enemyReferances = GetComponent<EnemyReferances>();
    }
    public void Shoot(){
        
        if(ShouldBeReload())
            return;

        Vector3 direction = GetDirection();
        if(Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, float.MaxValue, layerMask)){
            Debug.DrawLine(shootPoint.position, shootPoint.position + direction * 50f, Color.red, 1f);

            GameObject bullet = Instantiate(bulletImpact, hit.point, Quaternion.Euler(0f, 90f, 0f));
            StartCoroutine(SpawnTrail(bullet, hit)); 

            currentAmmo -= 1;
        }
    }
    public bool ShouldBeReload(){
        return currentAmmo <= 0;
    }
    public void Reload(){
        currentAmmo = ammo;
    }

    private Vector3 GetDirection(){
        Vector3 direction = transform.forward;
        direction += new Vector3(
            Random.Range(-spread.x, spread.x),
            Random.Range(-spread.y, spread.y),
            Random.Range(-spread.z, spread.z)
        );
        direction.Normalize();
        return direction; 
    }

    private IEnumerator SpawnTrail(GameObject bullet, RaycastHit hit){
        yield return new WaitForSeconds(5f);
        Destroy(bullet);
    }
    
}
