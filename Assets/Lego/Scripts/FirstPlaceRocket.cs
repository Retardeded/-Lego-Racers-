using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class FirstPlaceRocket : MonoBehaviour
    {

        public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
        public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
        public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
        public float m_MaxDamage = 2000f;                    // The amount of damage done if the explosion is centred on a tank.
        public float m_ExplosionForce = 8000f;              // The amount of force added to a tank at the centre of the explosion.
        public float m_MaxLifeTime = 20f;                    // The time in seconds before the shell is removed.
        public float m_ExplosionRadius = 8f;
        public float heightOffset = 1.5f;
        public bool detonateMode = false;

        public Transform currentTarget;
        public GameObject magent;
        private GameObject copy;
        public int targetedCheckPointNumber;
        public float rotateSpeed = 100f;
        public float speed = 20f;
        public int gravityScale = -5;
        Rigidbody rb;
        bool chaseMode = false;
        List<CheckPointHandler.CheckPoint> availableCheckPoints;
        public float closeEnoughDistance = 30f;
        public float torque = 40000f;

        private void Start()
        {
            StartCoroutine(createMagneticField());
            availableCheckPoints = CheckPointHandler.checkPoints;
            if (targetedCheckPointNumber >= CheckPointHandler.numberOfCheckPoints)
                targetedCheckPointNumber = 0;

            currentTarget = availableCheckPoints[targetedCheckPointNumber].checkPointObj;
            rb = GetComponent<Rigidbody>();

            Destroy(gameObject, m_MaxLifeTime);
            if (currentTarget != null)
            {
                chaseMode = true;
                InvokeRepeating("CheckTarget", 0.2f, 0.2f);
            }
        }

        void CheckTarget()
        {

            if (Vector3.Magnitude(transform.position - CheckPointHandler.currentFirstPlace.position) < closeEnoughDistance)
            {
                currentTarget = CheckPointHandler.currentFirstPlace;
                detonateMode = true;
            }
            else if (Vector3.Magnitude(transform.position - availableCheckPoints[targetedCheckPointNumber].checkPointObj.position) < 1f)
            {
                if (targetedCheckPointNumber < CheckPointHandler.numberOfCheckPoints)
                    targetedCheckPointNumber++;
                else
                    targetedCheckPointNumber = 0;

                currentTarget = availableCheckPoints[targetedCheckPointNumber].checkPointObj;
            }
        }


        private void FixedUpdate()
        {
            if (chaseMode)
            {
                if(copy != null)
                copy.transform.position = transform.position;

                Vector3 direction = (currentTarget.position - Vector3.up * heightOffset) - rb.position;
                direction.Normalize();

                Vector3 rotateAmount = Vector3.Cross(direction, transform.forward);

                rb.angularVelocity = -rotateAmount * rotateSpeed;
                rb.velocity = transform.forward * speed;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "CheckPoint")
            {
            print(other.name);

                if( (other.tag == "IgnoreRocket" || other.tag == "Car") && !detonateMode) {
                print("Auto wskoczylo");
                return; }

                Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

                // Go through all the colliders...
                for (int i = 0; i < colliders.Length; i++)
                {
                    UsePower currentVictim = colliders[i].GetComponent<UsePower>();
                    if (currentVictim == null)
                        continue;

                    if (currentVictim.shieldActive)
                    {
                        break;
                        // future change might be needed in this place
                    }

                    Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                    float damage = CalculateDamage(targetRigidbody.position);

                    targetRigidbody.velocity *= 1f - (damage / m_MaxDamage);

                    targetRigidbody.AddForceAtPosition(Vector3.up * (m_ExplosionForce + damage * 1000), targetRigidbody.transform.position);

                    if (damage / m_MaxDamage > 0.6f || currentTarget == targetRigidbody.transform)
                    {
                        targetRigidbody.AddForceAtPosition(Vector3.up * (200 + damage) * 100, targetRigidbody.transform.position);
                        targetRigidbody.AddRelativeTorque(0f, torque * (160 + damage), 0f, ForceMode.VelocityChange);
                        StartCoroutine(currentVictim.RecoveryTime(20f));
                    }
                }
                m_ExplosionParticles.transform.parent = null;

                m_ExplosionParticles.Play();

                m_ExplosionAudio.Play();
                Destroy(gameObject);
            }
        }

        private float CalculateDamage(Vector3 targetPosition)
        {
            // Create a vector from the shell to the target.
            Vector3 explosionToTarget = targetPosition - transform.position;

            // Calculate the distance from the shell to the target.
            float explosionDistance = explosionToTarget.magnitude;

            // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
            float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

            // Calculate damage as this proportion of the maximum possible damage.
            float damage = relativeDistance * m_MaxDamage;

            // Make sure that the minimum damage is always 0.
            damage = Mathf.Max(0f, damage + 1f);

            return damage;
        }
        
        IEnumerator createMagneticField()
        {
            yield return new WaitForSeconds(1f);
            copy = Instantiate(magent, transform.position, Quaternion.identity);

        }

    private void OnDestroy()
    {
        ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
        Destroy(m_ExplosionParticles.gameObject, mainModule.duration);

        Destroy(copy);
    }

}
