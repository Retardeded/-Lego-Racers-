using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    bool isTriggered = false;

    public float torque = 400f;

    float curseTime = 1f;
    Rigidbody victim;
    private void Awake()
    {
        gameObject.SetActive(false);
        transform.localPosition += Vector3.back * 1.3f;
        Invoke("SetActive", 0.05f);
    }

    void SetActive()
    {
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
        {
            victim = other.gameObject.GetComponent<Rigidbody>();
            if (victim.GetComponent<UsePower>().shieldActive == true)
                OnShieldEnter();
            if(victim.GetComponent<Dot_Truck_Controller>().isBoosted != 3)
            {
                isTriggered = true;
                Destroy(gameObject, curseTime);
            }
        }

    }
    void OnShieldEnter()
    {
        Destroy(gameObject);
    }
    void Curse(Rigidbody rb)
    {
        rb.AddRelativeTorque(0f, torque, 0f, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        if (isTriggered)
        {
            Curse(victim);
        }
    }
}
