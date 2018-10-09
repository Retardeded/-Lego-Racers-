using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplosion : MonoBehaviour {

    public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
    public float m_MaxDamage = 2000f;                    // The amount of damage done if the explosion is centred on a tank.
    public float m_MaxLifeTime = 3f;                    // The time in seconds before the shell is removed.

    public GameObject placeWithExplosives;
    public float gravityScale = -0.4f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, m_MaxLifeTime);
    }

    private void FixedUpdate()
    {
        Vector3 gravity = gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "CheckPoint")
        {
            if (other.tag == "Car")
            {
                Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

                targetRigidbody.velocity *= 0.1f;

                targetRigidbody.AddForceAtPosition(Vector3.up * (m_MaxDamage * targetRigidbody.mass), targetRigidbody.transform.position, ForceMode.Impulse);


            }

            else
            {
                Instantiate(placeWithExplosives, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
