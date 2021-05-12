using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;

        public static int enemySpeedBoost = 0;
        public static int sceneToLoad = 1;
        public static int finishedCars = 0;
        public static bool finishedRace = false;

        Coroutine endOfRaceCoroutine;
        [SerializeField] float waitingForScoreBoard = 2f;
        [SerializeField] float waitingForEndOfRace = 8f;
        static int enemyBoostScaler = 8;


        void Awake()
        {
            if (instance == null)
                instance = this;

            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            finishedCars = 0;
            finishedRace = false;
        }

        private void Update()
        {
            if (finishedCars > 0 && !finishedRace)
                EndOfRace();
        }

        public static void SetDifficultyOptions(int levelDifficulty)
        {
            enemySpeedBoost = enemyBoostScaler * levelDifficulty;
        }

        void EndOfRace()
        {
            FindObjectOfType<RaceUI>().ShowFinish();
            StartCoroutine(WaitingForScoreBoard());
            StartCoroutine(WaitingForRaceEnd());
            finishedRace = true;

        }

        IEnumerator WaitingForScoreBoard()
        {
            yield return new WaitForSeconds(waitingForScoreBoard);
            FindObjectOfType<RaceUI>().ShowTimeResult();
        }

        IEnumerator WaitingForRaceEnd()
        {
            yield return new WaitForSeconds(waitingForEndOfRace);
            FindObjectOfType<PauseMenu>().LoadMenu();
        }

        void PrintSceneToLoad()
        {
            print(GameManager.sceneToLoad);
        }

    }
