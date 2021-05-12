using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

    public class OutsideSourcesMovement : MonoBehaviour
    {
        public float nitroForce = 2000f;
        public float hookForce = 3f;
        public float boostFactor = 2f;
        float half = 0.5f;
        public int isBoosted = 0;
        public bool hookMode;

        public Transform carMeshPrefab;
        Vector3 carMeshBasicPositon = new Vector3(0f, -0.211f, 0f);
        public static float maxFlyHeight = 0.2f;
        public Rigidbody victimRB;

        Controller carUserControl;
        float boostTime = 4f;
        float hookTime = 0.5f;
        Rigidbody rb;
        private WheelCollider[] m_WheelColliders;
        float wheelDampingRate;


    private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Car")
            {
                if (other.gameObject.GetComponent<UsePower>().shieldActive)
                {
                    Shield potentialDangerShield = other.gameObject.GetComponent<UsePower>().activatedShield.GetComponent<Shield>();
                    if (potentialDangerShield.knockUpForce > 0)
                    {
                        print("strongshield");
                        //Transform attackerTransform = potenialDangerShield.GetComponentInParent<Transform>();
                        KnockUpSpin(potentialDangerShield);

                    }
                }
            }
        }

        private void KnockUpSpin(Shield potentialDangerShield)
        {
            rb.velocity = Vector3.zero;
            Vector3 direction = -4 * potentialDangerShield.transform.forward + 12 * Vector3.up;
            rb.AddForceAtPosition(direction * potentialDangerShield.knockUpForce * rb.mass, rb.transform.position);
            rb.AddRelativeTorque(0f, potentialDangerShield.torque * rb.mass, 0f, ForceMode.VelocityChange);
        }

        void Start()
        {
            carUserControl = GetComponent<CarUserControl>();
            m_WheelColliders = GetComponent<MovementController>().m_WheelColliders;
            rb = GetComponent<Rigidbody>();
            wheelDampingRate = m_WheelColliders[0].wheelDampingRate;
        }

        void FixedUpdate()
        {
            ApplyNitro();
            ApplyHook();

            for (int i = 0; i < m_WheelColliders.Length; i++)
            {
                if (m_WheelColliders[i].GetGroundHit(out WheelHit hit))
                {
                    ApplySand(i, hit);
                }
            }
        }

        private void ApplySand(int i, WheelHit hit)
        {
            if (hit.collider.gameObject.GetComponent<SandSlow>())
            {
                float wheelDamping = hit.collider.gameObject.GetComponent<SandSlow>().damperModifier;
                m_WheelColliders[i].wheelDampingRate = wheelDamping;

            }
            else
            {
                m_WheelColliders[i].wheelDampingRate = wheelDampingRate;
            }
        }

        private void ApplyHook()
        {
            if (hookMode)
            {
                if (hookTime <= 0f)
                {
                    hookMode = false;
                }
                float distanceBetween = Vector3.Magnitude(transform.position - victimRB.position);

                float totalBoost = boostFactor + Mathf.Pow(distanceBetween, 0.5f);
                HookBoost(totalBoost);
                HookSlowDown(totalBoost);
                hookTime -= Time.deltaTime;
            }
        }

        private void ApplyNitro()
        {
            if (isBoosted == 1)
            {
                if (boostTime <= 0f)
                {
                    isBoosted = 0;
                }

                NitroBoost(nitroForce * 0.9f);
                boostTime -= Time.deltaTime;
            }

            if (isBoosted == 2)
            {
                if (boostTime <= 0f)
                {
                    isBoosted = 0;
                }

                NitroBoost(nitroForce * 0.75f);
                boostTime -= Time.deltaTime;
            }

            if (isBoosted == 3)
            {
                if (boostTime <= 0f)
                {
                    isBoosted = 0;
                    carMeshPrefab.localPosition = carMeshBasicPositon;
                }

                NitroBoost(nitroForce * 0.25f);
                boostTime -= Time.deltaTime;
            }
        }

        void NitroBoost(float boostForce)
        {
            float forceToAdd = (boostForce * (boostTime / 2 + 0.5f) * rb.mass);
            forceToAdd = Mathf.Clamp(forceToAdd, 0f, 50000f);
            rb.AddForce(transform.forward * forceToAdd * boostTime);
            if (isBoosted == 3)
            {
                if (boostTime < 1)
                {
                    carMeshPrefab.transform.localPosition -= Vector3.up * Time.deltaTime;
                }
                else if (carMeshPrefab.transform.localPosition.y < maxFlyHeight)
                    carMeshPrefab.transform.localPosition += Vector3.up * Time.deltaTime * 0.5f;
            }
            //rb.AddForce(Vector3.down * forceToAdd * speed);
        }

        void HookBoost(float distance)
        {
            float pullFactor = carUserControl.leftrightdirection + 1f;
            float forceToAdd = ((Mathf.Pow(victimRB.velocity.magnitude, half) + distance) * victimRB.mass * hookForce * pullFactor);
            forceToAdd = Mathf.Clamp(forceToAdd, 0f, 200000f);
            rb.AddForce(transform.forward * forceToAdd);

        }

        void HookSlowDown(float distance)
        {
            float pullFactor = carUserControl.leftrightdirection + 2f;
            float forceToAdd = ((Mathf.Pow(victimRB.velocity.magnitude, half) + distance) * victimRB.mass * boostFactor * hookForce / pullFactor);
            forceToAdd = Mathf.Clamp(forceToAdd, 0f, 200000f);
            victimRB.AddForce(-transform.forward * forceToAdd);
        }

        public void SetBoostTime(float time)
        {
            boostTime = time;
        }

        public void SetHookTime(float time)
        {
            hookTime = time;
        }

        public void LimitSpin()
        {
        StartCoroutine(TmpConstarint());
        }

    public IEnumerator TmpConstarint()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        yield return new WaitForSeconds(0.1f);
        rb.constraints = RigidbodyConstraints.None;
    }
}

