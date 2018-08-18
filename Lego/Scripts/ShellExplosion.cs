using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
    public float m_MaxDamage = 2000f;                    // The amount of damage done if the explosion is centred on a tank.
    public float m_ExplosionForce = 8000f;              // The amount of force added to a tank at the centre of the explosion.
    public float m_MaxLifeTime = 5f;                    // The time in seconds before the shell is removed.
    public float m_ExplosionRadius = 8f;                // The maximum distance away from the explosion tanks can be and are still affected.

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
        Destroy(gameObject, m_MaxLifeTime);
        if(target != null)
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
        // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        // Go through all the colliders...
        for (int i = 0; i < colliders.Length; i++)
        {
            UsePower currentVictim = colliders[i].GetComponent<UsePower>();
            if (currentVictim == null || colliders[i].material.dynamicFriction != 1f)
                continue;

            if (currentVictim.shieldActive)
            {
                break;
                // future change might be needed in this place
            }

            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            float damage = CalculateDamage(targetRigidbody.position);
            print(damage.ToString());
            //targetRigidbody.AddExplosionForce (m_ExplosionForce * damage, transform.position - Vector3.down*2, m_ExplosionRadius);

            targetRigidbody.velocity *= 1f - (damage / m_MaxDamage);

            targetRigidbody.AddForceAtPosition(Vector3.up * (m_ExplosionForce + damage * 1000f), targetRigidbody.transform.position);
            print("dmgDONE " + (damage / m_MaxDamage).ToString());
        }

        // Unparent the particles from the shell.
        m_ExplosionParticles.transform.parent = null;

        // Play the particle system.
        m_ExplosionParticles.Play();

        // Play the explosion sound effect.
        m_ExplosionAudio.Play();

        // Once the particles have finished, destroy the gameobject they are on.
        ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
        Destroy(m_ExplosionParticles.gameObject, mainModule.duration);

        // Destroy the shell.
        
        Destroy(gameObject);
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
        damage = Mathf.Max(0f, damage + 10f);
        /* if (damage > 60f)
            damage = 60f;
        */
        return damage;
    }
}