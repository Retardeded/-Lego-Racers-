using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatalCollisionDetector : MonoBehaviour {

    [SerializeField] Dot_Truck_Controller adivisedCar;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "CheckPoint" || other.tag == "Brick") { return; }

        adivisedCar.fatalCollision = true;
        adivisedCar.recoverDirection = -adivisedCar.horizontalDirection;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CheckPoint" || other.tag == "Brick" || other.tag == "Car") { return; }

        adivisedCar.fatalCollision = false;
    }
}
