using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 20f);
	}
	
}
