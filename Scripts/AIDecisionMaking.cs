using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionMaking : MonoBehaviour {

    UsePower carPowerSystem;
    DistanceTraveled carDistanceTraveled;
    Dot_Truck_Controller carMovement;

    [SerializeField] CarDetector potentialVictimDetector;
    [SerializeField] CarDetector potentialDangerDetector;
    [SerializeField] CarDetector potentialLoserDetector;

    [SerializeField] int chanceForUsingBomb = 20;
    [SerializeField] int chanceForUsingShield = 20;
    [SerializeField] int chanceForUsingTrap = 20;
    [SerializeField] int chanceForUsingBoost = 35;

    [SerializeField] float delay = 0.3f;

    [SerializeField] int carNumber = 1;
    int checkPointBeforeWarp;

    private void Start()
    {
        carPowerSystem = GetComponent<UsePower>();
        carDistanceTraveled = GetComponent<DistanceTraveled>();
        carMovement = GetComponent<Dot_Truck_Controller>();
    }
    private void Update()
    {
        if(carMovement.warpMode)
        {
            if (checkPointBeforeWarp != carDistanceTraveled.currentCheckPoint)
                carMovement.warpMode = false;
        }
    }
    public void MakeDecision()
    {
        StartCoroutine(CalculateDecision());
    }

    IEnumerator CalculateDecision()
    {
        if(carPowerSystem.currentBoostType == 1)
        {
            if(carPowerSystem.whiteBlocksNumber != 2)
            {
                potentialVictimDetector.gameObject.SetActive(true);
                yield return new WaitForSeconds(delay);
                if (potentialVictimDetector.successfulDetection)
                {
                    carPowerSystem.decisionMade = true;
                }
                else if (Random.Range(0f, 100f) < chanceForUsingBomb)
                {
                    carPowerSystem.decisionMade = true;
                }
                potentialVictimDetector.gameObject.SetActive(false);
            }
            else
            {
                if (carDistanceTraveled.currentPosition > 1)
                    carPowerSystem.decisionMade = true;
            }

        }
        else if (carPowerSystem.currentBoostType == 2)
        {
            potentialDangerDetector.gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
            if(potentialDangerDetector.successfulDetection)
            {
                if(carPowerSystem.shieldActive == false)
                carPowerSystem.decisionMade = true;
            }
            potentialDangerDetector.gameObject.SetActive(false);
        }
        else if (carPowerSystem.currentBoostType == 3)
        {
            if(carPowerSystem.whiteBlocksNumber != 2)
            {
                potentialLoserDetector.gameObject.SetActive(true);
                yield return new WaitForSeconds(delay);
                if (potentialLoserDetector.successfulDetection)
                {
                    carPowerSystem.decisionMade = true;
                }
                potentialLoserDetector.gameObject.SetActive(false);
            }
            else
            {
                carMovement.warpMode = true;
                yield return new WaitForSeconds(delay);
                carPowerSystem.decisionMade = true;
                checkPointBeforeWarp = carDistanceTraveled.currentCheckPoint;
            }
        }
        else if (carPowerSystem.currentBoostType == 4)
        {
            if (Random.Range(0f, 100f) < chanceForUsingBoost + chanceForUsingBoost / 2 * carPowerSystem.whiteBlocksNumber)
            {
                if(carPowerSystem.whiteBlocksNumber > carMovement.isBoosted)
                carPowerSystem.decisionMade = true;
            }
        }
    }
}
