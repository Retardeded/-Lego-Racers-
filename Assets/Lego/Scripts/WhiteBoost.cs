using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class WhiteBoost : MonoBehaviour
    {
        public float xRot = 1f;
        public float yRot = 1f;
        public float zRot = 1f;
        public float spawnTime = 6f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Car")
            {
                if (other.gameObject.GetComponent<BotController>())
                {
                BotController aiCar = other.gameObject.GetComponent<BotController>();
                    aiCar.importantObjectSpotted = false;
                    StopCoroutine(aiCar.AchivingImportantGoal(aiCar.leftrightdirection, 0.1f));
                    aiCar.isChasingCheckPoint = true;
                    other.gameObject.GetComponent<AIDecisionMaking>().MakeDecision();
                }

                if (other.gameObject.GetComponent<UsePower>().whiteBlocksNumber < 2)
                {
                    UsePower carToBoost = other.gameObject.GetComponent<UsePower>();
                    carToBoost.UpgradeBoost(carToBoost.whiteBlocksNumber + 1);
                    gameObject.SetActive(false);
                    Invoke("SpawnAgain", spawnTime);
                }
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