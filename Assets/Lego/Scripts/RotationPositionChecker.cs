using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class RotationPositionChecker : MonoBehaviour
    {

        Transform carToCheck;
        DistanceTraveled carDistance;
        List<CheckPointHandler.CheckPoint> availableCheckPoints;
        [SerializeField] float waitTime = 2f;

        [SerializeField] int maxXRot = 40;
        [SerializeField] int maxZRot = 40;

        [SerializeField] int minYPosition = -2;
        Coroutine recoveryCoroutine;

        private void Start()
        {
            carToCheck = GetComponent<Transform>();
            carDistance = GetComponent<DistanceTraveled>();
            availableCheckPoints = CheckPointHandler.checkPoints;
        }
        void Update()
        {

            if ((carToCheck.rotation.eulerAngles.x > maxXRot && carToCheck.rotation.eulerAngles.x < 360 - maxXRot || carToCheck.position.y < minYPosition
                  || carToCheck.rotation.eulerAngles.z > maxZRot && carToCheck.rotation.eulerAngles.z < 360 - maxZRot)
                && recoveryCoroutine == null)
            {
                recoveryCoroutine = StartCoroutine(TimeToRecover());
            }
        }

        IEnumerator TimeToRecover()
        {
            yield return new WaitForSeconds(waitTime);

            if (carToCheck.rotation.eulerAngles.x > maxXRot && carToCheck.rotation.eulerAngles.x < 360 - maxXRot || carToCheck.position.y < minYPosition
                  || carToCheck.rotation.eulerAngles.z > maxZRot && carToCheck.rotation.eulerAngles.z < 360 - maxZRot)
            {
                carToCheck.position = availableCheckPoints[carDistance.currentCheckPoint].checkPointObj.position;
                carToCheck.transform.LookAt(availableCheckPoints[carDistance.currentCheckPoint + 1].checkPointObj.position);
            }
            recoveryCoroutine = null;

        }
    }

