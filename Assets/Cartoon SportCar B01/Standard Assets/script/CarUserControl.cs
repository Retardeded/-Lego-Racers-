using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (MovementController))]
    public class CarUserControl : Controller
    {
        private string verticalButton;
        private string horizontalButton;

        private void Start()
        {
            verticalButton = "Vertical" + m_Car.m_CarNumber;
            horizontalButton = "Horizontal" + m_Car.m_CarNumber;
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            leftrightdirection = Input.GetAxis(horizontalButton);
            forbackdirection = Input.GetAxis(verticalButton);
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Brake1");
            m_Car.Move(leftrightdirection, forbackdirection, handbrake, 0f);
            //m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(leftrightdirection, forbackdirection, forbackdirection, 0f);
#endif
        }
    }
}
