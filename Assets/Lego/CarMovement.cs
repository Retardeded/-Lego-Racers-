using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {

    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_Speed = 12f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;

    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;

    public float balance = 1.05f;
    public float balanceFactor = 1.05f;
    public float velocity;

    float helper = 1.2f;

    public float torque = 400f;
    public float downForce = 400f;
    public bool isCursed = false;
    float curseTime = 1f;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;
        InvokeRepeating("CheckBalance", 0.3f, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
    }

    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        Turn();
        if (isCursed)
        {
            Curse();
            curseTime -= Time.fixedDeltaTime;
            if (curseTime <= 0f)
                isCursed = false;
        }
    }

    private void CheckBalance()
    {
        velocity = m_Rigidbody.velocity.z;
        balanceFactor = balance;
        for(int i = 3; i < velocity; i += 5)
        {
            balanceFactor *= balance;
        }
    }

    private void Move()
    {
        Vector3 force = new Vector3(0f, 0f, m_MovementInputValue * m_Speed * Time.fixedDeltaTime / balanceFactor);
        if (velocity < 10f)
            force *= helper;
        m_Rigidbody.AddRelativeForce(force);
    }

    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.fixedDeltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
       // m_Rigidbody.AddRelativeTorque(0f, turn, 0f);
    }

    void Curse()
    {
        m_Rigidbody.AddRelativeTorque(0f, torque, 0f, ForceMode.VelocityChange);
        m_Rigidbody.AddRelativeForce(0f, 0f, -1f * downForce, ForceMode.Acceleration);

    }
}
