using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class FatalCollisionDetector : MonoBehaviour
    {

        [SerializeField] BotController adivisedCar;
        [SerializeField] bool leftColider;
        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "CheckPoint" || other.tag == "Brick") { return; }

            adivisedCar.fatalCollision = true;
            if (leftColider)
                adivisedCar.recoverDirection = -1f;
            else
                adivisedCar.recoverDirection = 1f;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "CheckPoint" || other.tag == "Brick" || other.tag == "Car") { return; }

            adivisedCar.fatalCollision = false;
        }
    }