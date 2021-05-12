using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplosion : MonoBehaviour {

    public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    public ParticleSystem [] m_ExplosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
    public float m_MaxDamage = 2000f;                    // The amount of damage done if the explosion is centred on a tank.
    public float m_ExplosionForce = 8000f;
    public float m_MaxLifeTime = 3f;                    // The time in seconds before the shell is removed.
    public float m_ExplosionRadius = 8f;

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
                //Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

                //targetRigidbody.velocity *= 0.1f;

                //targetRigidbody.AddForceAtPosition(Vector3.up * (m_MaxDamage * targetRigidbody.mass), targetRigidbody.transform.position, ForceMode.Impulse);

                Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
                bool strongShieldDetected = false;
                for (int i = 0; i < colliders.Length; i++)
                {
                    UsePower currentVictim = colliders[i].GetComponent<UsePower>();
                    if (currentVictim == null)
                        continue;

                    if (currentVictim.shieldActive)
                    {
                        Shield blockingShield = currentVictim.activatedShield.GetComponent<Shield>();
                        if (blockingShield.strongShield == true)
                        {
                            print("ComplicatedBBBB");
                            strongShieldDetected = true;
                            transform.Rotate(0f, 180f, 0f);
                            gravityScale = 0;
                        }
                        break;
                        // future change might be needed in this place
                    }

                    Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                    float damage = CalculateDamage(targetRigidbody.position);

                    currentVictim.carMovement.LimitSpin();
                    targetRigidbody.AddForceAtPosition(Vector3.up * (m_ExplosionForce + damage) * targetRigidbody.mass, targetRigidbody.transform.position, ForceMode.Impulse);
                }
                if (!strongShieldDetected)
                {
                    foreach (ParticleSystem particle in m_ExplosionParticles)
                    {

                        particle.transform.parent = null;

                        particle.Play();

                        //m_ExplosionAudio.Play();

                        ParticleSystem.MainModule mainModule = particle.main;
                        Destroy(particle.gameObject, mainModule.duration);
                    }

                    Destroy(gameObject);
                }
        }
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        float damage = relativeDistance * m_MaxDamage;

        // Make sure that the minimum damage is always 0.
        damage = Mathf.Max(0f, damage);
        print(damage.ToString());
        return damage;
    }
}
