using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Warp : MonoBehaviour
    {

        public float timeDelay = 4f;
        public LayerMask carMask;
        public float teleportRadius = 1f;
        public float warppingForce = 10000f;
        public float warpDistance = 20f;
        public WarpFormation warpFormationPrefab;
        List<CheckPointHandler.CheckPoint> availableCheckPoints;
        float yRot;
        float scaleFactor;

        private void OnTriggerStay(Collider other)
        {
            if (yRot == warpFormationPrefab.yRot)
            {
                warpFormationPrefab.yRot--;
                warpFormationPrefab.wallDelay += 0.6f;
            }
        }
        void Start()
        {
            scaleFactor = transform.localScale.z;
            warpDistance *= scaleFactor;
            teleportRadius *= scaleFactor;
            availableCheckPoints = CheckPointHandler.checkPoints;
            yRot = warpFormationPrefab.yRot;
            StartCoroutine(WarpActivation());
        }

        IEnumerator WarpActivation()
        {
            yield return new WaitForSeconds(timeDelay);
            Collider[] colliders = Physics.OverlapSphere(transform.position, teleportRadius, carMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Rigidbody>() != null)
                {
                    Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
                    targetRigidbody.AddForce(targetRigidbody.transform.up * warppingForce, ForceMode.Impulse);
                }
            }
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < colliders.Length; i++)
            {
                float remainingWarpDistance = warpDistance;
                DistanceTraveled targetTravelScore = colliders[i].GetComponent<DistanceTraveled>();

                while (remainingWarpDistance > 0)
                {
                    if (targetTravelScore.currentCheckPoint == CheckPointHandler.numberOfCheckPoints)
                    {
                        targetTravelScore.currentCheckPoint = -1;
                        targetTravelScore.currentLap++;
                        CrossingMeta.ChangeLapText(targetTravelScore.currentLap, targetTravelScore.GetComponent<Dot_Truck_Controller>().m_PlayerNumber);
                    }
                    Transform nextCheckPoint = availableCheckPoints[targetTravelScore.currentCheckPoint + 1].checkPointObj;
                    float distanceCrossed = Vector3.Magnitude(targetTravelScore.transform.position - nextCheckPoint.position);
                    if (remainingWarpDistance > distanceCrossed)
                    {
                        targetTravelScore.transform.position = nextCheckPoint.position;
                        targetTravelScore.currentCheckPoint++;

                    }
                    else
                    {
                        Vector3 remainDistance = (nextCheckPoint.position - targetTravelScore.transform.position);
                        targetTravelScore.transform.position += remainDistance * (remainingWarpDistance / distanceCrossed) * 0.7f;

                        targetTravelScore.transform.LookAt(nextCheckPoint);
                        Rigidbody targetRigidbody = targetTravelScore.GetComponent<Rigidbody>();
                        targetRigidbody.velocity = Vector3.zero;
                        targetRigidbody.velocity = (nextCheckPoint.position - transform.position).normalized * 6 * scaleFactor;
                        Dot_Truck_Controller carMovement = targetTravelScore.GetComponent<Dot_Truck_Controller>();
                        if (carMovement.isBot)
                        {
                            float currentMinDist = 100f;
                            int secondMinDistIndex = 1;
                            int correctFastCheckPointNum = 1;
                            for (int j = 1; j < carMovement.optimalCheckPoints.Length; j++)
                            {
                                float currentDistance = Vector3.Distance(carMovement.transform.position, carMovement.optimalCheckPoints[j].position);
                                if (currentDistance < currentMinDist)
                                {
                                    secondMinDistIndex = correctFastCheckPointNum;
                                    correctFastCheckPointNum = j;
                                    currentMinDist = currentDistance;
                                }
                            }
                            if (correctFastCheckPointNum > secondMinDistIndex)
                                carMovement.fastCheckPointNumber = correctFastCheckPointNum;
                            else
                                carMovement.fastCheckPointNumber = secondMinDistIndex;

                        }
                    }

                    remainingWarpDistance -= distanceCrossed;
                }

            }
            gameObject.SetActive(false);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
