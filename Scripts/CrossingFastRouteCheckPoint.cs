using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingFastRouteCheckPoint : MonoBehaviour {

    public int fastCheckPointNumber;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
        {
            if(other.GetComponent<Dot_Truck_Controller>().isBot)
            {
                other.GetComponent<Dot_Truck_Controller>().fastCheckPointNumber = fastCheckPointNumber;
            }
        }
    }
}
