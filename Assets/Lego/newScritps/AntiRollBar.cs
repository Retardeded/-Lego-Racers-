using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AntiRollBar : MonoBehaviour
{

    public WheelCollider wheelL;
    public WheelCollider wheelR;
    public float antiRoll = 5000f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }
    void FixedUpdate()
    {
        WheelHit hit;
        float travelL = 1f;
        float travelR = 1f;

        var groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y -wheelL.radius) / wheelL.suspensionDistance;

        var groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) /wheelR.suspensionDistance;

        var antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
            rb.AddForceAtPosition(wheelL.transform.up * -antiRollForce,
                   wheelL.transform.position);
        if (groundedR)
            rb.AddForceAtPosition(wheelR.transform.up * antiRollForce,
                   wheelR.transform.position);
    }

}
