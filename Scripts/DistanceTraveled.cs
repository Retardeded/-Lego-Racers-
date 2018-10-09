using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceTraveled : MonoBehaviour {

    public int currentLap = 0;
    public int currentCheckPoint = 0;
    public float distanceToNextCheckPoint;
    public TMP_Text lapTimeTextMiliSeconds;
    public TMP_Text lapTimeTextSeconds;
    public TMP_Text lapTimeTextMinutes;
    public float timePassed;
    public int timeInSeconds;
    public int timeInMinutes;
    public int currentPosition;
    string basicBreak;
    [SerializeField] bool isOffScreen = true;

    void Start ()
    {
        basicBreak = ":";
        ResetLapTime();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!isOffScreen)
            CountAndDisplayTime();
    }

    private void CountAndDisplayTime()
    {
        timePassed += Time.deltaTime;
        int roundedTime = (int)(timePassed * 100);
        lapTimeTextMiliSeconds.text = (roundedTime).ToString();
        if (timePassed > 1)
        {
            timeInSeconds++;
            if(timeInSeconds >= 10)
            {
                lapTimeTextSeconds.text = timeInSeconds.ToString() + basicBreak;
            }
            else
            {
                lapTimeTextSeconds.text = "0" + timeInSeconds.ToString() + basicBreak;
            }
            timePassed = 0;
        }
        if (timeInSeconds >= 60)
        {
            timeInMinutes++;
            lapTimeTextMinutes.text = timeInMinutes.ToString() + basicBreak;
            timeInSeconds = 0;
            lapTimeTextSeconds.text = timeInSeconds.ToString() + basicBreak;
        }
    }

    public void ResetLapTime()
    {
        timePassed = 0f;
        timeInSeconds = 0;
        timeInMinutes = 0;
        lapTimeTextMiliSeconds.text = timePassed.ToString();
        lapTimeTextSeconds.text = "0" + timeInSeconds.ToString() + basicBreak;
        lapTimeTextMinutes.text = timeInMinutes.ToString() + basicBreak;
    }
}
