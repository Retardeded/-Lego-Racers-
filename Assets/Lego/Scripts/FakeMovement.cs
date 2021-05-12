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
            if (transform.localPosition.y < OutsideSourcesMovement.maxFlyHeight)
                transform.localPosition += Vector3.up  * Time.deltaTime;
        }
        else
        {
            transform.localPosition -= Vector3.up * Time.deltaTime * 0.5f;
        }
    }

    IEnumerator LowFuel()
    {
        yield return new WaitForSeconds(3f);
        lowAmountOfFuel = true;
    }
}
