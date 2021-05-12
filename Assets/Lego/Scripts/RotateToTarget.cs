using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour {

    public Transform target;
    public GameObject thisAsTarget;
    public float maxDistance = 60f;
    Quaternion basicRotation;
    GameObject[] possibleTargets;
    int upRotationBoundary = 310;
    int downRotationBoundary = 50;

    void Start() {

        basicRotation = transform.localRotation;
        possibleTargets = GameObject.FindGameObjectsWithTag("Car");
        for(int i = 0; i < possibleTargets.Length; i++)
        {
            if (possibleTargets[i] == thisAsTarget)
            {
                possibleTargets[i] = null;
            }
        }
    }

    public Transform LookAt()
    {
        target = null;
        Transform currentTarget;
        float currentDistance = maxDistance;
        for (int i = 0; i < possibleTargets.Length; i++)
        {
            if (possibleTargets[i] == null)
                continue;
            currentTarget = possibleTargets[i].transform;
            transform.LookAt(currentTarget);
            if (Mathf.Abs(transform.localRotation.eulerAngles.y) < downRotationBoundary || Mathf.Abs(transform.localRotation.eulerAngles.y) > upRotationBoundary)
            {
                if (Vector3.Magnitude(transform.position - possibleTargets[i].transform.position) < currentDistance)
                {
                    currentDistance = Vector3.Magnitude(transform.position - possibleTargets[i].transform.position);
                    target = possibleTargets[i].transform;
                }
            }
        }
        if (target == null)
        {
            transform.localRotation = basicRotation;
            return null;
        }
        else
        {
            transform.LookAt(target);
            return target;
        }

    }


}
