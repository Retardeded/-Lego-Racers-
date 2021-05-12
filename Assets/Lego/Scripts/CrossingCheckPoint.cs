using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CrossingCheckPoint : MonoBehaviour
    {

        public int checkPointNumber;

        void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Car")
            {
                DistanceTraveled carProgress = other.GetComponent<DistanceTraveled>();

                if (carProgress.currentCheckPoint == checkPointNumber - 1)
                    carProgress.currentCheckPoint = checkPointNumber;
                //else if (carProgress.currentCheckPoint == checkPointNumber)
                //     carProgress.currentCheckPoint = checkPointNumber - 1;
            }
        }
    }
