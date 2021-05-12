using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class BotController : Controller
    {
        private Rigidbody rb;

        public int fastCheckPointNumber = 0;
        public int criticalCheckPoint;
        [SerializeField] GameObject optimalCheckPointsObject;
        public Transform[] optimalCheckPoints;
        float forwardDirection = 1f;


        public bool importantObjectSpotted = false;
        public bool fatalCollision = false;
        public Coroutine recoveringFromCollision;
        Coroutine checkingDeadEnd;
        Coroutine resetingStacks;
        [SerializeField] float recoveryTime = 0.5f;
        float checkTime = 3f;
        public float recoverDirection = 0f;
        public int fatalStacks = 1;
        [SerializeField] float speedlessVelocity = 0.2f;
        [SerializeField] float deadEndTime = 0.1f;
        bool forwardDeadEnd = true;

        public bool isChasingCheckPoint = true;
        public bool warpMode = false;
        public bool rightAfterPickUp = false;

        public Transform nonCheckPointTarget;
        public Transform checkPointTarget;
        public float velocity;

        void Start()
        {
        isBot = true;
        rb = GetComponent<Rigidbody>();
            optimalCheckPoints = new Transform[optimalCheckPointsObject.transform.childCount + 1];
            for (int i = 0; i < optimalCheckPointsObject.transform.childCount; i++)
            {
                optimalCheckPoints[i] = optimalCheckPointsObject.transform.GetChild(i);
            }
        optimalCheckPoints[optimalCheckPointsObject.transform.childCount] = optimalCheckPointsObject.transform.GetChild(0);
    }

        // Update is called once per frame
        void FixedUpdate()
        {
            TurnToCorrectTarget();
            ApplyCorrectForwardForce();
            m_Car.Move(leftrightdirection, forbackdirection, forbackdirection, 0f);
            if(fatalStacks > 1 && resetingStacks == null)
            {
            print("reset1");
            resetingStacks = StartCoroutine(FatalStacksReset(checkTime));
            }
    }

        private void ApplyCorrectForwardForce()
        {
            if (!warpMode)
            {
            velocity = rb.velocity.magnitude;
                
                if(rb.velocity.magnitude <= speedlessVelocity)
                {
                if(checkingDeadEnd == null)
                    checkingDeadEnd = StartCoroutine(PotentialDeadEnd(deadEndTime));
                }

                if (rb.velocity.magnitude <= speedlessVelocity && fatalCollision)
                {
                    if (recoveringFromCollision == null)
                        recoveringFromCollision = StartCoroutine(RecoverFromFatalCollision(recoveryTime));
                }
                else if (recoveringFromCollision == null)
                {
                forbackdirection = forwardDirection;
                }
            }
            else
            {
                rb.velocity *= 0.5f;
                forbackdirection = 0f;
            }
        }

        private void TurnToCorrectTarget()
        {
            if (!importantObjectSpotted)
            {
                Vector3 rawDirection;
                if (isChasingCheckPoint)
                {
                    int checkPointIndex = fastCheckPointNumber;
                    if (checkPointIndex > optimalCheckPointsObject.transform.childCount)
                    {
                        checkPointIndex = 0;
                    }
                checkPointTarget = optimalCheckPoints[checkPointIndex];
                    rawDirection = optimalCheckPoints[checkPointIndex].position - transform.position;
                    rawDirection.Normalize();
                    //if (Random.Range(0f, 100f) < 10)
                    //{
                    //    print("R" + rawDirection.ToString());
                    //    print(transform.forward.ToString());
                    //}
                }
                else
                {
                    rawDirection = nonCheckPointTarget.position - transform.position;
                    rawDirection.Normalize();
                }
                CalculatingCorrectTurn(rawDirection);
            }
        }

        private void CalculatingCorrectTurn(Vector3 rawDirection)
        {
            float determinantValue = transform.forward.x * rawDirection.z - transform.forward.z * rawDirection.x;
        //float cosValue = transform.forward.x * rawDirection.x + transform.forward.z * rawDirection.z;
        float axb = transform.forward.x * transform.forward.z + rawDirection.x * rawDirection.z;
        float aAbs = Mathf.Sqrt(transform.forward.x * transform.forward.x + transform.forward.z * transform.forward.z);
        float bAbs = Mathf.Sqrt(rawDirection.x * rawDirection.x + rawDirection.z * rawDirection.z);
        float cos = axb / (aAbs * bAbs);
        float alfa = Mathf.Acos(cos) * (1/Mathf.PI);
        if (determinantValue > 0)
            alfa = -alfa;

        leftrightdirection = alfa;   //*(1/2+cosValue);

            if (recoveringFromCollision != null)
            {
                leftrightdirection = recoverDirection;
            }

        //leftrightdir = -determinantValue;
        }

        public IEnumerator AchivingImportantGoal(float valueOfTurn, float customPathTime)
        {
            importantObjectSpotted = true;
        if (recoveringFromCollision == null)
            leftrightdirection = valueOfTurn;
        else
            leftrightdirection = -valueOfTurn;

            yield return new WaitForSeconds(customPathTime);
            importantObjectSpotted = false;
        }
        public IEnumerator PotentialDeadEnd(float deadEndTime)
        {
            yield return new WaitForSeconds(deadEndTime);
            if (rb.velocity.magnitude <= speedlessVelocity)
                fatalCollision = true;

            checkingDeadEnd = null;
        }
        public IEnumerator RecoverFromFatalCollision(float recoveryTime)
        {
        fatalStacks *= 2;
        if (forwardDeadEnd)
        {
            forbackdirection = -forwardDirection;
        }
        else
            forbackdirection = forwardDirection;
            
            yield return new WaitForSeconds(recoveryTime * fatalStacks);
        criticalCheckPoint = fastCheckPointNumber;
        if(fatalStacks > 4)
            forwardDeadEnd = !forwardDeadEnd;
            fatalCollision = false;
            recoveringFromCollision = null;

        if (fatalStacks > 16)
            fatalStacks = 1;

    }



        public IEnumerator FatalStacksReset(float checkTime)
        {
        print("reset2");
        yield return new WaitForSeconds(checkTime);
        if (criticalCheckPoint != fastCheckPointNumber)
        {
            print("reset3");
            fatalStacks = 1;
            forwardDeadEnd = true;
        }
        resetingStacks = null;
        }
        public IEnumerator ChangeChaseTarget(Transform newTarget)
        {
            isChasingCheckPoint = false;
            nonCheckPointTarget = newTarget;
            yield return new WaitForSeconds(0.5f);
            if (isChasingCheckPoint == false)
                isChasingCheckPoint = true;
        }
    public IEnumerator LimitingGreed()
    {
        yield return new WaitForSeconds(0.5f);
        rightAfterPickUp = false;
    }
}

