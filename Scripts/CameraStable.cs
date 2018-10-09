using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStable : MonoBehaviour {

    public GameObject car;
    public float zDistance = 15f;
    float carYrotation;
	void Start () {
		
	}
	
	void Update () {
        carYrotation = car.transform.eulerAngles.y;
        this.transform.eulerAngles = new Vector3(zDistance, carYrotation, 0f);
	}
}
