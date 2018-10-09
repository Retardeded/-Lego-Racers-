using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestingTargetDetector : MonoBehaviour {

    [SerializeField] Dot_Truck_Controller carToAdivse;

    private void OnTriggerEnter(Collider other)
    {
        if(!carToAdivse.isChasingCheckPoint) { return; }

        if(other.tag == "Brick")
        {
            if(other.GetComponent<WhiteBoost>() != null)
            {
                if(carToAdivse.isBoosted == 0)
                {
                    StartCoroutine(carToAdivse.ChangeChaseTarget(other.transform));
                    print("chasemode " + other.name.ToString());
                }
            }
            else if(other.GetComponent<Boost>().boostType == 4 || other.GetComponent<Boost>().boostType == 1)
            {
                if (carToAdivse.isBoosted == 0)
                {
                    StartCoroutine(carToAdivse.ChangeChaseTarget(other.transform));
                    print("chasemode " + other.name.ToString());
                }
            }
        }
    }
}
