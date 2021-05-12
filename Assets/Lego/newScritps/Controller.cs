using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    protected MovementController m_Car; // the car controller we want to use
    public float leftrightdirection;
    public float forbackdirection;
    public bool isBot;

    void Awake()
    {
        m_Car = GetComponent<MovementController>();
    }

}
