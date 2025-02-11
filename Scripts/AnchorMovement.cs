﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorMovement : MonoBehaviour
{

    public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
    public float m_MaxLifeTime = 5f;                    // The time in seconds before the shell is removed.

    public Dot_Truck_Controller orginCar;
    public float hookTime = 0.5f;
    public Transform target;
    Transform hookPoint;
    public float rotateSpeed = 100f;
    public float speed = 20f;
    public int gravityScale = -5;
    Rigidbody rb;
    bool chaseMode = false;
    bool destroyMode = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = Vector3.Magnitude(rb.velocity);
        hookPoint = transform;
        Destroy(gameObject, m_MaxLifeTime);

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
        Debug.DrawLine(orginCar.transform.position, hookPoint.position, Color.red, 0.05f);

        if(!destroyMode)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "CheckPoint")
        {
            if (other.tag == "Car")
            {
                UsePower currentVictim = other.gameObject.GetComponent<UsePower>();

                if (currentVictim.shieldActive == false)
                {
                    orginCar.hookMode = true;
                    float correctHookTime = hookTime * Vector3.Magnitude(orginCar.transform.position - currentVictim.transform.position) / 5;
                    orginCar.SetHookTime(correctHookTime);
                    Rigidbody targetRigidbody = target.GetComponentInParent<Rigidbody>();
                    orginCar.victimRB = targetRigidbody;
                    targetRigidbody.AddForce(Vector3.up * targetRigidbody.mass, ForceMode.Impulse);
                    rb.velocity = Vector3.zero;
                    chaseMode = false;
                    destroyMode = true;
                    hookPoint = targetRigidbody.transform;

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
