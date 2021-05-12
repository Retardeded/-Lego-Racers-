using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Vehicles.Car;

    public class DirectionAdvisor : MonoBehaviour
    {

        [SerializeField] OutsideSourcesMovement carMovement;    
        [SerializeField] BotController botController;
        UsePower usePower;
        [SerializeField] int chanceForAgreement;
        [SerializeField] float valueOfTurn;
        [SerializeField] string nameOfThisPart;
        [SerializeField] bool avoidCars = false;
        [SerializeField] float customPathTime = 0.2f;
        [SerializeField] int maxWhiteBlockNumber = 2;

        private void Start()
        {
            usePower = botController.GetComponent<UsePower>();
        }

    private void OnTriggerStay(Collider other)
        {
            if(botController.recoveringFromCollision != null) { return; }

            if (other.tag == "CheckPoint" || (other.tag == "Car" && avoidCars == false)) { return; }

            if (Random.Range(0f, 100f) < chanceForAgreement && other.tag != "Brick")
            {
                StopCoroutine(botController.AchivingImportantGoal(botController.leftrightdirection, customPathTime));
                StartCoroutine(botController.AchivingImportantGoal(valueOfTurn, customPathTime));
                return;
            }

            if (other.tag == "Brick")
            {
                if (botController.importantObjectSpotted == false && carMovement.isBoosted == 0)
                {
                    if (other.GetComponent<WhiteBoost>() != null && botController.GetComponent<UsePower>().whiteBlocksNumber < maxWhiteBlockNumber)
                    {
                        StartCoroutine(botController.AchivingImportantGoal(-valueOfTurn, customPathTime));
                    }
                    else if (other.GetComponent<Boost>() != null && !botController.rightAfterPickUp)
                    {
                        StartCoroutine(botController.AchivingImportantGoal(-valueOfTurn, customPathTime));
                    }
                }
            }
        }
    }
