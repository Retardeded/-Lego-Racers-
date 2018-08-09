using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    public Vector3 offset;

    float cameraXRotation = 15f;

	void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {

        transform.localPosition = offset;
	}
}
