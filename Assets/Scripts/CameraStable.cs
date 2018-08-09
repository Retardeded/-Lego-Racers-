using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStable : MonoBehaviour {

    public GameObject car;
    float carYrotation;
	void Start () {
		
	}
	
	void Update () {
        carYrotation = car.transform.eulerAngles.y;
        this.transform.eulerAngles = new Vector3(15f, carYrotation, 0f);
	}
}
