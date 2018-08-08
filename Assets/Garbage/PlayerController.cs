using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerController : MonoBehaviour {

    Rigidbody rb;
    public float speedForce = 50f;
    public float rotationSpeed = 100f;
    public float visibleDistance = 200f;
    public float frictionSlowDown = 20f;
    float MovementInputValue;
    float TurnInputValue;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        MovementInputValue = Input.GetAxis("Vertical");
        TurnInputValue = Input.GetAxis("Horizontal");

    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }


    private void Move()
    {
        Vector3 movement = transform.forward * MovementInputValue * speedForce * Time.deltaTime;
        Vector3 slowDown = Vector3.zero;
        if (rb.velocity.z >= 0f)
        slowDown = transform.forward * frictionSlowDown * Time.deltaTime;

        rb.velocity += movement - slowDown;
        //rb.MovePosition(rb.position + movement);
    }


    private void Turn()
    {
        float turn = TurnInputValue * rotationSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        rb.MoveRotation(rb.rotation * turnRotation);
    }
}
