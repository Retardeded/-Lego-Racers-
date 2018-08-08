using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueMovement : MonoBehaviour {

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    public GameObject FL;
    public GameObject FR;
    public GameObject BL;
    public GameObject BR;

    public float topSpeed = 250f;
    public float maxTorque = 200f;
    public float maxSteerAngle = 45f;
    public float currentSpeed;
    public float maxBrakeTorque = 2200;

    float forward;
    float turn;
    float brake;

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void FixedUpdate()
    {
        forward = Input.GetAxis("Vertical1");
        turn = Input.GetAxis("Horizontal1");
        brake = Input.GetAxis("Jump");

        wheelFL.steerAngle = maxSteerAngle * turn;
        wheelFR.steerAngle = maxSteerAngle * turn;

        currentSpeed = 2 * 22 / 7 * wheelBL.radius * wheelBL.rpm * 60 / 1000;

        if(currentSpeed < topSpeed)
        {
            wheelBL.motorTorque = maxTorque * forward;
            wheelBR.motorTorque = maxTorque * forward;
            wheelFL.motorTorque = maxTorque * forward;
            wheelFR.motorTorque = maxTorque * forward;
        }

        wheelBL.brakeTorque = maxBrakeTorque * brake;
        wheelBR.brakeTorque = maxBrakeTorque * brake;
        wheelFL.brakeTorque = maxBrakeTorque * brake;
        wheelFR.brakeTorque = maxBrakeTorque * brake;
    }
    // Update is called once per frame
    void Update () {

        Quaternion flq;
        Vector3 flv;
        wheelFL.GetWorldPose(out flv, out flq);
        FL.transform.position = flv;
        FL.transform.rotation = flq;

        Quaternion Blq;
        Vector3 Blv;
        wheelBL.GetWorldPose(out Blv, out Blq);
        BL.transform.position = Blv;
        BL.transform.rotation = Blq;

        Quaternion frq;
        Vector3 frv;
        wheelFR.GetWorldPose(out frv, out frq);
        FR.transform.position = frv;
        FR.transform.rotation = frq;

        Quaternion brq;
        Vector3 brv;
        wheelBR.GetWorldPose(out brv, out brq);
        BR.transform.position = brv;
        BR.transform.rotation = brq;
    }
}
