using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


    public class RaceUI : MonoBehaviour
    {

        public GameObject[] positionsTimes;
        public GameObject[] positionsNames;
        public GameObject resultsUI;
        public GameObject finishUI;

        public static List<DistanceTraveled> carDistances = new List<DistanceTraveled>();

        private void Awake()
        {
            carDistances.Clear();
        }

        public void AddToFinalResult(DistanceTraveled carDistance)
        {
            carDistances.Add(carDistance);
        }
        public void ShowTimeResult()
        {
            resultsUI.SetActive(true);
            print("finshed cars: " + GameManager.finishedCars.ToString());
            for (int i = 0; i < carDistances.Count; i++)
            {
                int position = i + 1;
                int minutes = carDistances[i].seconds / 60;
                int seconds = carDistances[i].seconds - 60 * minutes;
                int miliseconds = carDistances[i].miliseconds;
                string secondsStr = seconds.ToString();
                string milisecondsStr = miliseconds.ToString();
                if (seconds < 10)
                {
                    secondsStr = "0" + secondsStr;
                }
                if (miliseconds < 10)
                {
                    milisecondsStr = "0" + milisecondsStr;
                }
                positionsTimes[position - 1].GetComponent<TMP_Text>().text = minutes.ToString() + ":" + secondsStr + ":" + milisecondsStr;
                positionsNames[position - 1].GetComponent<TMP_Text>().text = carDistances[i].carName;
            }
        }

        public void ShowFinish()
        {
            finishUI.SetActive(true);
        }
    }

