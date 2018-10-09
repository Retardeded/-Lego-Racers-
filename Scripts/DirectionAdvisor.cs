using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionAdvisor : MonoBehaviour {

    [SerializeField] Dot_Truck_Controller carToAdvise;
    [SerializeField] int chanceForAgreement;
    [SerializeField] float valueOfTurn;
    [SerializeField] string nameOfThisPart;
    [SerializeField] bool avoidCars = false;
    [SerializeField] float customPathTime = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint" || (other.tag == "Car" && avoidCars == false) ) { return; }

        if (Random.Range(0f, 100f) < chanceForAgreement && other.tag != "Brick")
        {
            StopCoroutine(carToAdvise.AchivingImportantGoal(carToAdvise.horizontalDirection, customPathTime) );
            StartCoroutine(carToAdvise.AchivingImportantGoal(valueOfTurn, customPathTime) );
            print(nameOfThisPart.ToString());
            print(other.name.ToString());
            return;
        }

        if (other.tag == "Brick")
        {
            if(carToAdvise.importantObjectSpotted == false && carToAdvise.isBoosted == 0)
            {
                StartCoroutine(carToAdvise.AchivingImportantGoal(-valueOfTurn, customPathTime) );
                print(nameOfThisPart.ToString());
                print(other.name.ToString());
            }
        }
    }
}
