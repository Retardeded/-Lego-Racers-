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
    public Transform orginCar;
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
        if(other.tag != "CheckPoint")
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
            bool strongShieldDetected = false;
            for (int i = 0; i < colliders.Length; i++)
            {
                UsePower currentVictim = colliders[i].GetComponent<UsePower>();
                if (currentVictim == null )
                    continue;

                if (currentVictim.shieldActive)
                {
                    Shield blockingShield = currentVictim.activatedShield.GetComponent<Shield>();
                    if (blockingShield.strongShield == true && target == other.transform)
                    {
                        print("Complicated");
                        strongShieldDetected = true;
                        transform.Rotate(0f, 180f, 0f);
                        target = orginCar;
                        gravityScale = 0;
                    }
                    break;
                    // future change might be needed in this place
                }

                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                float damage = CalculateDamage(targetRigidbody.position);
                print(damage.ToString());
            
                targetRigidbody.velocity *= 1f - (damage / m_MaxDamage);

                targetRigidbody.AddForceAtPosition(Vector3.up * (m_ExplosionForce + damage) * targetRigidbody.mass, targetRigidbody.transform.position, ForceMode.Impulse);
                print("dmgDONE " + (damage / m_MaxDamage).ToString());
            }
            if(!strongShieldDetected)
            {
                m_ExplosionParticles.transform.parent = null;

                m_ExplosionParticles.Play();

                m_ExplosionAudio.Play();

                ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
                Destroy(m_ExplosionParticles.gameObject, mainModule.duration);

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
        return damage;
    }
}