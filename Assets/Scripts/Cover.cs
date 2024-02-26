using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class Cover : MonoBehaviour
{
    CoverArea coverArea;
    float dist;
    GameObject target;
    public bool useThisCover = true;
    private void Start() {
        coverArea = GetComponentInParent<CoverArea>();
        target = GameObject.FindGameObjectWithTag("Player");
        GetDistance();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.3f);
    }
    private void FixedUpdate() {
        GetDistance();
    }
    void GetDistance(){
        Debug.LogWarning(target.name);
        dist = Vector3.Distance(target.transform.position, transform.position);
        if(dist < 20){
            useThisCover = true;
            if(coverArea.covers.Contains(gameObject.GetComponent<Cover>()))
                return;
            coverArea.covers.Add(gameObject.GetComponent<Cover>());
        }else {
            useThisCover = false;
            if(!coverArea.covers.Contains(gameObject.GetComponent<Cover>()))
                return;
            coverArea.covers.Remove(gameObject.GetComponent<Cover>());
        } 
            
    }
}
