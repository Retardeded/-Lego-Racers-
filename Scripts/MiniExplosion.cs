using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniExplosion : MonoBehaviour {

    public float m_ExplosionForce = 8000f;       
    public float m_MaxLifeTime = 3f;
    public float speed = 1f;

    void Start () {
        Destroy(gameObject, m_MaxLifeTime);
        Invoke("DisableMethod", 1f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position += Vector3.up * speed * Time.fixedDeltaTime;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "CheckPoint")
        {
            if (other.tag == "Car")
            {
                Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

                targetRigidbody.velocity *= 0.4f;

                targetRigidbody.AddForceAtPosition(Vector3.up * m_ExplosionForce * targetRigidbody.mass, targetRigidbody.transform.position);
            }

            Destroy(gameObject);
        }
    }

    void DisableMethod()
    {
        gameObject.SetActive(false);
    }
}
