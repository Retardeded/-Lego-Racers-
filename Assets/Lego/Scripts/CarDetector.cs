using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDetector : MonoBehaviour {

    public bool successfulDetection = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
        {
            successfulDetection = true;
        }
    }
}
