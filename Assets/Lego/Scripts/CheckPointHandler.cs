using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

    public class CheckPointHandler : MonoBehaviour
    {

        public class CheckPoint
        {
            public float distanceToMeta;
            public Transform checkPointObj;
            public int checkPointNumber;
        }

        public class CarStats
        {
            public float carTravelScore;
            public int carNumber;
        }

        public Transform[] checkPointsObj;
        public static List<CheckPoint> checkPoints = new List<CheckPoint>();
        public static int numberOfCheckPoints;
        public static int numberOfCars;

        public Transform[] carPositionsInGame;
        public int[] sortedCarPositions;
        public TMP_Text[] positionsTexts;
        public List<DistanceTraveled> carDistances = new List<DistanceTraveled>();

        public static Transform currentFirstPlace;

        private void Awake()
        {
            numberOfCheckPoints = checkPointsObj.Length - 2;
            checkPoints.Clear();
        }

        void Start()
        {

            numberOfCars = carPositionsInGame.Length;

            for (int i = 0; i < carPositionsInGame.Length; i++)
            {
                carDistances.Add(carPositionsInGame[i].GetComponent<DistanceTraveled>());
            }

            for (int i = 0; i < checkPointsObj.Length - 1; i++)
            {
                float distanceToMeta = 0;
                for (int j = i; j < checkPointsObj.Length - 1; j++)
                {
                    distanceToMeta += Vector3.Magnitude(checkPointsObj[j + 1].transform.position - checkPointsObj[j].transform.position);
                }
                CheckPoint currentCheckPoint = new CheckPoint();
                currentCheckPoint.distanceToMeta = distanceToMeta;
                currentCheckPoint.checkPointObj = checkPointsObj[i];

                int k = 0;
                while (k < checkPoints.Count)
                {
                    if (checkPoints[k].distanceToMeta > currentCheckPoint.distanceToMeta)
                        k++;
                    else
                        break;
                }

                checkPoints.Insert(k, currentCheckPoint);
            }
            for (int i = 0; i < checkPoints.Count; i++)
            {
                if (i == 0)
                {
                    CrossingMeta currentPoint = checkPoints[i].checkPointObj.GetComponent<CrossingMeta>();
                    currentPoint.checkPointNumber = i;
                    checkPoints[i].checkPointNumber = i;
                }
                else
                {
                    CrossingCheckPoint currentPoint = checkPoints[i].checkPointObj.GetComponent<CrossingCheckPoint>();
                    currentPoint.checkPointNumber = i;
                    checkPoints[i].checkPointNumber = i;
                }

            }

            InvokeRepeating("CountingCarPositions", 0.2f, 0.2f);
        }

        void CountingCarPositions()
        {

            List<CarStats> carStats = new List<CarStats>();
            for (int i = 0; i < carPositionsInGame.Length; i++)
            {
                CarStats stats = new CarStats();
                stats.carNumber = i;
                carStats.Add(stats);
            }

            for (int i = 0; i < carPositionsInGame.Length; i++)
            {

                carStats[i].carTravelScore += carDistances[i].currentLap * 2868;
                carStats[i].carTravelScore -= checkPoints[carDistances[i].currentCheckPoint].distanceToMeta;
                if (carDistances[i].currentCheckPoint == 0)
                {
                    float currentDist = Vector3.Magnitude(carPositionsInGame[i].position - checkPoints[carDistances[i].currentCheckPoint + 1].checkPointObj.transform.position);
                    carStats[i].carTravelScore -= currentDist;
                }
                else
                {
                    float currentDist = Vector3.Magnitude(carPositionsInGame[i].position - checkPoints[carDistances[i].currentCheckPoint].checkPointObj.transform.position);
                    carStats[i].carTravelScore += currentDist;
                }

            }
            // decreasing sort // highest num -> 1st pos

            for (int i = 1; i < carStats.Count; i++)
            {
                int j = i - 1;
                CarStats currentScore = carStats[i];
                while (j >= 0 && carStats[j].carTravelScore < currentScore.carTravelScore)
                {
                    carStats[j + 1] = carStats[j];
                    j--;
                }
                carStats[j + 1] = currentScore;
            }

            for (int i = 0; i < carPositionsInGame.Length; i++)
            {
                sortedCarPositions[carStats[i].carNumber] = i + 1;
                if (sortedCarPositions[carStats[i].carNumber] == 1)
                {
                    currentFirstPlace = carPositionsInGame[carStats[i].carNumber];
                }
                if (carStats[i].carNumber < positionsTexts.Length)
                {
                    positionsTexts[carStats[i].carNumber].text = (i + 1).ToString();
                }

                carDistances[carStats[i].carNumber].currentPosition = i + 1;
            }

        }
    }
