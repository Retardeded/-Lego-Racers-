using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AnchorMovement : MonoBehaviour
    {

        public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
        public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
        public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
        public float m_MaxLifeTime = 5f;                    // The time in seconds before the shell is removed.
        public CharacterJoint jointObject;
        CharacterJoint latestJoint;

        public OutsideSourcesMovement orginCar;
        public float hookTime = 0.5f;
        public Transform target;
        Transform hookPoint;
        public float rotateSpeed = 100f;
        public float speed = 20f;
        public int gravityScale = -5;
        Rigidbody rb;
        bool chaseMode = false;
        bool destroyMode = false;
        LineRenderer line;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            line = GetComponent<LineRenderer>();

            speed = Vector3.Magnitude(rb.velocity);
            hookPoint = transform;
            Destroy(gameObject, m_MaxLifeTime);

            if (target != null)
            {
                chaseMode = true;
                Invoke("CallOff", 1f);
            }

            //InvokeRepeating("CreateConnection", 0.1f, 0.1f);
    }

        void CallOff()
        {
            chaseMode = false;
        }

        private void FixedUpdate()
        {
            //Debug.DrawLine(orginCar.transform.position, hookPoint.position, Color.red);
            line.SetPosition(0, orginCar.transform.position);
            line.SetPosition(1, hookPoint.position);

            if (!destroyMode)
            {
                Vector3 gravity = gravityScale * Vector3.up;
                rb.AddForce(gravity, ForceMode.Acceleration);
                if (chaseMode)
                {
                    Vector3 direction = target.position - rb.position;
                    direction.Normalize();

                    Vector3 rotateAmount = Vector3.Cross(direction, transform.forward);

                    rb.angularVelocity = -rotateAmount * rotateSpeed;
                    rb.velocity = transform.forward * speed;
                }

            }
        }

        void CreateConnection()
        {
            CharacterJoint newJoint = Instantiate(jointObject, transform.position, transform.rotation);
            //newJoint.transform.SetParent(transform);
            if(latestJoint != null)
            {
            newJoint.connectedBody = latestJoint.GetComponent<Rigidbody>();
            }
            else
            {
            newJoint.connectedBody = orginCar.GetComponent<Rigidbody>();
            }
            latestJoint = newJoint;

        }


        private void OnTriggerEnter(Collider other)
        {
            if(destroyMode) { return; }

            if (other.tag != "CheckPoint" && other.tag != "Hook")
            {
                if (other.tag == "Car")
                {
                    UsePower currentVictim = other.gameObject.GetComponent<UsePower>();

                    if (currentVictim.shieldActive == false)
                    {
                        orginCar.hookMode = true;
                        float correctHookTime = hookTime * Vector3.Magnitude(orginCar.transform.position - currentVictim.transform.position)/5f;
                        orginCar.SetHookTime(correctHookTime);
                        Rigidbody targetRigidbody = target.GetComponentInParent<Rigidbody>();
                        orginCar.victimRB = targetRigidbody;
                        //targetRigidbody.AddForce(Vector3.up * targetRigidbody.mass, ForceMode.Impulse);
                        rb.velocity = Vector3.zero;
                        chaseMode = false;
                        destroyMode = true;
                        hookPoint = targetRigidbody.transform;
                        //CharacterJoint newJoint = Instantiate(jointObject, transform.position, transform.rotation);
                        //newJoint.connectedBody = targetRigidbody;

                    Destroy(gameObject, correctHookTime);
                    }
                    else
                        Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

    }
