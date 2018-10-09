using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Boost : MonoBehaviour
    {

        public int boostType = 1;

        public float xRot = 1f;
        public float yRot = 1f;
        public float zRot = 1f;
        public float spawnTime = 6f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Car")
            {
                other.gameObject.GetComponent<UsePower>().SetBoost(boostType);
                if(other.gameObject.GetComponent<Dot_Truck_Controller>().isBot)
                {
                Dot_Truck_Controller aiCar = other.gameObject.GetComponent<Dot_Truck_Controller>();
                aiCar.importantObjectSpotted = false;
                StopCoroutine(aiCar.AchivingImportantGoal(aiCar.horizontalDirection, 0.1f));
                aiCar.isChasingCheckPoint = true;
                other.gameObject.GetComponent<AIDecisionMaking>().MakeDecision();
                }
                gameObject.SetActive(false);
                Invoke("SpawnAgain", spawnTime);
            }
        }

        // Update is called once per frame
        void Update()
        {
            Rotate();
        }

        void Rotate()
        {
            transform.Rotate(xRot, yRot, zRot);
        }

        void SpawnAgain()
        {
        gameObject.SetActive(true);
        }
    }
