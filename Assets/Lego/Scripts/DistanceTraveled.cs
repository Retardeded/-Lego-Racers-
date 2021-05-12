using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


    public class DistanceTraveled : MonoBehaviour
    {

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

        public int seconds;
        public int miliseconds;

        public string carName;
        string basicBreak;
        bool isRacing = true;

        [SerializeField] bool isOffScreen = true;
        [SerializeField] bool isBot = true;
        Coroutine plotTwistCoroutine;
        AIDecisionMaking carToAdvice;
        void Start()
        {
            carName = gameObject.name;
            if (lapTimeTextMiliSeconds == null)
            {
                isRacing = false;
            }

            basicBreak = ":";
            if (!isBot)
            {
                ResetLapTime();
            }
            if (isBot)
            {
                carToAdvice = GetComponent<AIDecisionMaking>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isRacing)
            {
                if (currentLap == 4)
                {
                    GameManager.finishedCars++;
                    RaceUI raceUi = FindObjectOfType<RaceUI>();
                    raceUi.AddToFinalResult(this);
                    isRacing = false;
                }
                if (!isOffScreen)
                    CountAndDisplayTime();
                else
                    CountTime();
                if (isBot && plotTwistCoroutine == null)
                {
                    plotTwistCoroutine = StartCoroutine(WaitingForPlotTwist());
                }
            }
        }
        IEnumerator WaitingForPlotTwist()
        {
            int pastPosition = currentPosition;
            yield return new WaitForSeconds(1.5f);
            if (pastPosition != currentPosition)
                carToAdvice.MakeDecision();

            plotTwistCoroutine = null;
        }
        private void CountAndDisplayTime()
        {
            timePassed += Time.deltaTime;
            int roundedTime = (int)(timePassed * 100);
            miliseconds = roundedTime;
            lapTimeTextMiliSeconds.text = (roundedTime).ToString();
            if (timePassed > 1)
            {
                timeInSeconds++;
                seconds++;
                if (timeInSeconds >= 10)
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

        private void CountTime()
        {
            timePassed += Time.deltaTime;
            int roundedTime = (int)(timePassed * 100);
            miliseconds = roundedTime;
            if (timePassed > 1)
            {
                seconds++;
                timePassed = 0;
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
