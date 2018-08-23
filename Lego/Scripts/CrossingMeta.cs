using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingMeta : MonoBehaviour {

    int numberOfCheckPoints;
    public int checkPointNumber;

    void Start () {
        numberOfCheckPoints = CheckPointHandler.numberOfCheckPoints;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
        {
            DistanceTraveled carProgress = other.GetComponent<DistanceTraveled>();

            if (carProgress.currentCheckPoint == numberOfCheckPoints)
            {
                carProgress.currentLap++;
                carProgress.currentCheckPoint = 0;
            }
            else if (carProgress.currentCheckPoint < numberOfCheckPoints && carProgress.currentLap > 0)
            {
                carProgress.currentLap--;
                carProgress.currentCheckPoint = numberOfCheckPoints;
            }
        }
    }
}
