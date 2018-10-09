using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpFormation : MonoBehaviour {

    public float timeDelay = 0.5f;
    public float maxLifeTime = 8f;

    public float yRot = 2f;

    public GameObject teleportPrefab;
    bool isMovingUp = true;
    float scaleFactor;
    public float wallDelay = 1f;
    int wallToActiveNumber;

    private void Start()
    {
        wallToActiveNumber = 1;
        Destroy(gameObject, maxLifeTime);
        Invoke("ActiveTeleportationPlace", timeDelay);
        StartCoroutine(TransparentWallFormation());
        scaleFactor = transform.localScale.z;
        yRot *= scaleFactor;
    }
    void Update()
    {
       // if (isMovingUp)
       //     MoveUp();

        Rotate();
    }

    IEnumerator TransparentWallFormation()
    {
        yield return new WaitForSeconds(0.4f);
        while(wallToActiveNumber < 4)
        {
            transform.GetChild(wallToActiveNumber).gameObject.SetActive(true);
            yield return new WaitForSeconds(wallDelay);
            wallToActiveNumber++;
        }
        print("ReadyToTP");
    }

    void ActiveTeleportationPlace()
    {
        isMovingUp = false;
        teleportPrefab.SetActive(true);
    }
    void MoveUp()
    {
        transform.position += 0.2f * scaleFactor * Vector3.up * Time.deltaTime;
    }
    void Rotate()
    {
        transform.Rotate(0f, yRot, 0f);
    }
}
