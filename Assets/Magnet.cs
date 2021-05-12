using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float magneticForce = 5f;
    public Rigidbody target;



    // Update is called once per frame
    void Update()
    {
        if(target != null) { target.AddForce((transform.position - target.position) * magneticForce * Time.deltaTime); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Car")
        {
            target = other.gameObject.GetComponent<Rigidbody>();
            StartCoroutine(pullTime());
        }
    }

    IEnumerator pullTime()
    {
        yield return new WaitForSeconds(3f);
        target = null;
    }
}
