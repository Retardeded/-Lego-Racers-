using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMovement : MonoBehaviour {

    public bool lowAmountOfFuel = false;
    public Vector3 startPos;
    Coroutine fuelCoroutine;
    private void OnEnable()
    {
        transform.localPosition = startPos;
        lowAmountOfFuel = false;
        if (fuelCoroutine != null)
            StopCoroutine(fuelCoroutine);

        fuelCoroutine = StartCoroutine(LowFuel());
    }

    void Update () {
        if(!lowAmountOfFuel)
        {
            if (transform.localPosition.y < Dot_Truck_Controller.maxFlyHeight)
                transform.localPosition += Vector3.up * 2 * Time.deltaTime;
        }
        else
        {
            transform.localPosition -= Vector3.up * 3 * Time.deltaTime;
        }
    }

    IEnumerator LowFuel()
    {
        yield return new WaitForSeconds(3.6f);
        lowAmountOfFuel = true;
    }
}
