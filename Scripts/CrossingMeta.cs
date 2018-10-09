using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CrossingMeta : MonoBehaviour {

    int numberOfCheckPoints;
    public int checkPointNumber;
    public TMP_Text[] nonStaticLapTexts;
    public static List<TMP_Text> lapTexts = new List<TMP_Text>();
    static string basicLapText;
    static string basicEndText;
    void Start () {
        numberOfCheckPoints = CheckPointHandler.numberOfCheckPoints;
        basicLapText = "LAP ";
        basicEndText = "/3";

        for(int i = 0; i < nonStaticLapTexts.Length; i++)
        {
            lapTexts.Add(nonStaticLapTexts[i]);
            lapTexts[i].text = basicLapText + "1" + basicEndText;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
        {
            DistanceTraveled carProgress = other.GetComponent<DistanceTraveled>();
            Dot_Truck_Controller carMovement = other.GetComponent<Dot_Truck_Controller>();


            if (carProgress.currentCheckPoint == numberOfCheckPoints)
            {
                carProgress.currentCheckPoint = 0;
                carProgress.currentLap++;
                if(carProgress.timeInSeconds > 30 || carProgress.timeInMinutes > 0)
                {
                    if(carMovement != null)
                    ChangeLapText(carProgress.currentLap, carMovement.m_PlayerNumber);
                    carProgress.ResetLapTime();
                }
            }
            
            else if (carProgress.currentCheckPoint < numberOfCheckPoints && carProgress.currentLap > 1)
            {
                carProgress.currentLap--;
                carProgress.currentCheckPoint = numberOfCheckPoints;
            }
            
        }
    }

    public static void ChangeLapText(int currentLap, int playerNumber)
    {
        lapTexts[playerNumber - 1].text = basicLapText + currentLap.ToString() + basicEndText;
    }
}
