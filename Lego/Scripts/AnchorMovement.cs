using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorMovement : MonoBehaviour
{

    public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
    public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.

    public Dot_Truck_Controller orginCar;
    public float hookTime = 0.5f;
    public Transform target;
    public float rotateSpeed = 100f;
    public float speed = 20f;
    public int gravityScale = -5;
    Rigidbody rb;
    bool chaseMode = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        speed = Vector3.Magnitude(rb.velocity);

        if (target != null)
        {
            chaseMode = true;
            Invoke("CallOff", 1f);
        }
    }

    void CallOff()
    {
        chaseMode = false;
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, orginCar.transform.position, Color.red, 0.05f);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
        {
            UsePower currentVictim = other.gameObject.GetComponent<UsePower>();

            if (currentVictim.shieldActive == false)
            {
                orginCar.hookMode = true;
                orginCar.SetHookTime(hookTime);
                orginCar.victimRB = target.GetComponentInParent<Rigidbody>();
                transform.SetParent(other.transform);
                chaseMode = false;
                rb.velocity = Vector3.zero;
            }
            else
                Destroy(gameObject);
        }
        Destroy(gameObject, m_MaxLifeTime);
    }
}
