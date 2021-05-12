using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMiniMapObjects : MonoBehaviour
{
    public GameObject [] miniMapObjects;
    void Start()
    {
        foreach (var obj in miniMapObjects)
        {
            obj.SetActive(true);
        }
    }

}
