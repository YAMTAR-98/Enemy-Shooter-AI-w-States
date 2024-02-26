using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoverArea : MonoBehaviour
{
    public List<Cover> covers = new List<Cover>();
    private void Awake() {
        //covers = covers.Add(gameObject.GetComponentInChildren<Cover>().);
    }
    public Cover GetRandomCover(Vector3 location){
        return covers[Random.Range(0, covers.Count - 1)];
    }
}
