using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpFormation : MonoBehaviour {

    public float timeDelay = 0.5f;
    public float maxLifeTime = 8f;

    public float yRot = 2.5f;

    public GameObject teleportPrefab;
    bool isMovingUp = true;

    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
        Invoke("ActiveTeleportationPlace", timeDelay);
    }
    void Update()
    {
        if (isMovingUp)
            MoveUp();

        Rotate();
    }

    void ActiveTeleportationPlace()
    {
        isMovingUp = false;
        teleportPrefab.SetActive(true);
    }
    void MoveUp()
    {
        transform.position += 5 * Vector3.up * Time.deltaTime;
    }
    void Rotate()
    {
        transform.Rotate(0f, yRot, 0f);
    }
}
