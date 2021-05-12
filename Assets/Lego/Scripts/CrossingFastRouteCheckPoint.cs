using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CrossingFastRouteCheckPoint : MonoBehaviour
    {

        public int fastCheckPointNumber;

        private void Start()
        {
            fastCheckPointNumber = transform.GetSiblingIndex() + 1;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Car")
            {
                if (other.GetComponent<BotController>() != null)
                {
                    other.GetComponent<BotController>().fastCheckPointNumber = fastCheckPointNumber;
                }
            }
        }
    }
