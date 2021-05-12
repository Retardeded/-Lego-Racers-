using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class InterestingTargetDetector : MonoBehaviour {

        [SerializeField] OutsideSourcesMovement carMovement;
        [SerializeField] BotController botController;

    private void OnTriggerEnter(Collider other)
        {
            if (!botController.isChasingCheckPoint) { return; }

            if (other.tag == "Brick" && carMovement.isBoosted == 0)
            {
                if (other.GetComponent<WhiteBoost>() != null)
                {
                    if (carMovement.isBoosted == 0)
                    {
                        StartCoroutine(botController.ChangeChaseTarget(other.transform));
                    }
                }
                else if (other.GetComponent<Boost>().boostType == 4 || other.GetComponent<Boost>().boostType == 1)
                {
                    if (carMovement.isBoosted == 0)
                    {
                        StartCoroutine(botController.ChangeChaseTarget(other.transform));
                    }
                }
            }
        }
    }

