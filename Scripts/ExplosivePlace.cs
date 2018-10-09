using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePlace : MonoBehaviour {

    public GameObject miniExplosionPrefab;

    public float maxLifeTime = 5f;
    void Start () {
        InvokeRepeating("CreateMiniExplosion", 0.1f, 0.3f);
        Destroy(gameObject, maxLifeTime);
	}
	
	void CreateMiniExplosion()
    {
        Instantiate(miniExplosionPrefab, transform.position, Quaternion.identity);
    }
}
