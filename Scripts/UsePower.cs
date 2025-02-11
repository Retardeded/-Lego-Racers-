﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UsePower : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
        public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
        public float m_MaxChargeTime = 0.75f;

        private string m_FireButton;                // The input axis that is used for launching shells.
        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.

        public int currentBoostType = 0;
        public int whiteBlocksNumber = 0;
        public bool shieldActive = false;
        public GameObject activatedShield;
        GameObject activatedBoost;
        Coroutine shieldCoroutine;
        Coroutine boostCoroutine;
        public GameObject trapPrefab;
        public GameObject warpFormationPrefab;
        public float boostTime = 2f;
        public bool strongSpeedUpAvailable = true;
        public Rigidbody anchorPrefab;
        public Rigidbody chaseBombPrefab;

        public Rigidbody explosionBarrelPrefab;

        public Transform carMeshPrefab;
        Vector3 carMeshBasicPositon;
        float scaleFactor;

        public Sprite[] blockImages;
        public Sprite[] whiteBlockImages;
        public Image shownImage;
        public Image shownWhiteImage;
        Dot_Truck_Controller carMovement;
        DistanceTraveled distanceTraveled;

        public bool decisionMade = false;

        public void SetBoost(int value)
        {
            currentBoostType = value;
            shownImage.sprite = blockImages[value-1];
            shownImage.color = new Color(255, 255, 255, 255);
        }
        public void UpgradeBoost(int value)
        {
            whiteBlocksNumber = value;
            shownWhiteImage.sprite = whiteBlockImages[value-1];
            shownWhiteImage.color = new Color(255, 255, 255, 255);
    }
        void ResetBoost()
        {
            currentBoostType = 0;
            whiteBlocksNumber = 0;
            shownWhiteImage.color = new Color(255, 255, 255, 0);
            shownImage.color = new Color(255, 255, 255, 0);
        }

    private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            m_CurrentLaunchForce = m_MinLaunchForce;
            //gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }


        private void Start()
        {
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;

            // The rate that the launch force charges up is the range of possible forces by the max charge time.
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

            carMovement = GetComponent<Dot_Truck_Controller>();
            distanceTraveled = GetComponent<DistanceTraveled>();
            carMeshBasicPositon = carMeshPrefab.localPosition;
            scaleFactor = transform.localScale.z;
        }
        

        private void Update()
    {
        if (Input.GetButtonDown(m_FireButton))
        {
            UseCorrectPower();
        }
        else if (decisionMade)
        {
            UseCorrectPower();
            decisionMade = false;
        }
    }

    private void UseCorrectPower()
    {
        if (currentBoostType == 1)
        {
            if (whiteBlocksNumber == 2)
                ChaseFirstBomb();
            else if (whiteBlocksNumber == 1)
                AnchorLanuch();
            else
                BombLanuch();
        }
        else if (currentBoostType == 2)
        {
            if (whiteBlocksNumber == 2)
                OrangeShield();
            else if (whiteBlocksNumber == 1)
                GreenShield();
            else
                BlueShield();

        }
        else if (currentBoostType == 3)
        {
            if (whiteBlocksNumber == 2)
                WarpFormation();
            else if (whiteBlocksNumber == 1)
                GunpowderBarrel();
            else
                PlantTrap();
        }
        else if (currentBoostType == 4)
        {
            if (strongSpeedUpAvailable)
            {
                if (whiteBlocksNumber == 2)
                    FlyingTurboBoost();
                else if (whiteBlocksNumber == 1)
                    DoubleTurboBoost();
                else
                    TurboBoost();
            }
            else
            {
                TurboBoost();
            }
        }
    }

    private void TurboBoost()
    {
        carMeshPrefab.localPosition = carMeshBasicPositon;
        carMovement.SetBoostTime(boostTime);
        carMovement.isBoosted = 1;
        if (boostCoroutine != null)
            StopCoroutine(boostCoroutine);
        boostCoroutine = StartCoroutine(BoostDuration(3, boostTime));

        ResetBoost();
    }

    private void DoubleTurboBoost()
    {
        carMeshPrefab.localPosition = carMeshBasicPositon;
        carMovement.SetBoostTime(boostTime * 1.2f);
        carMovement.isBoosted = 2;
        if (boostCoroutine != null)
            StopCoroutine(boostCoroutine);
        boostCoroutine = StartCoroutine(BoostDuration(4, boostTime * 1.2f));

        ResetBoost();
    }

    private void FlyingTurboBoost()
    {
        carMovement.SetBoostTime(boostTime * 2f);
        carMovement.isBoosted = 3;
        if (boostCoroutine != null)
            StopCoroutine(boostCoroutine);
        boostCoroutine = StartCoroutine(BoostDuration(5, boostTime * 2f));

        ResetBoost();
    }

    IEnumerator BoostDuration(int childNumber, float duration)
    {
        if (activatedBoost != null)
        {
            activatedBoost.SetActive(false);
        }

        activatedBoost = gameObject.transform.GetChild(childNumber).gameObject;
        activatedBoost.SetActive(true);
        yield return new WaitForSeconds(duration);
        activatedBoost.SetActive(false);
    }

    void WarpFormation()
    {
        Instantiate(warpFormationPrefab, transform.position + 12.7f * Vector3.down * scaleFactor, transform.rotation);
        ResetBoost();
    }
    private void GunpowderBarrel()
    {
        FireBarrel();
        ResetBoost();
    }
    private void PlantTrap()
    {   
        Instantiate(trapPrefab, transform.position, transform.rotation);
        ResetBoost();
    }

    private void FireBarrel()
    {
        Vector3 directionVector = transform.forward * 8;
        Rigidbody barrel =
                Instantiate(explosionBarrelPrefab, m_FireTransform.position + directionVector * scaleFactor * -2, transform.rotation) as Rigidbody;

        barrel.velocity = m_MinLaunchForce * -transform.forward;
    }

    void OrangeShield()
    {
        if(shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(ShieldDuration(whiteBlocksNumber, 9f));
        ResetBoost();
    }
    void GreenShield()
    {
       if (shieldCoroutine != null)
           StopCoroutine(shieldCoroutine);
       shieldCoroutine = StartCoroutine(ShieldDuration(whiteBlocksNumber, 7f) );
       ResetBoost();
    }

    void BlueShield()
    {
        if (shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(ShieldDuration(whiteBlocksNumber, 5f));
        ResetBoost();
        
    }

    IEnumerator ShieldDuration(int childNumber, float duration)
    {
        if(activatedShield != null)
        {
            activatedShield.SetActive(false);
        }

        activatedShield = gameObject.transform.GetChild(childNumber).gameObject;
        activatedShield.SetActive(true);
        shieldActive = true;
        yield return new WaitForSeconds(duration);
        activatedShield.SetActive(false);
        shieldActive = false;
    }
    private void ChaseFirstBomb()
    {
        Rigidbody rocketInstance;

            rocketInstance =
            Instantiate(chaseBombPrefab, m_FireTransform.position + 2 * Vector3.up * scaleFactor, m_FireTransform.rotation) as Rigidbody;

            FirstPlaceRocket chaseRocket = rocketInstance.GetComponent<FirstPlaceRocket>();
            chaseRocket.speed = m_MinLaunchForce;
            chaseRocket.targetedCheckPointNumber = distanceTraveled.currentCheckPoint + 1;

        ResetBoost();
    }
    void AnchorLanuch()
    {
        Transform target = m_FireTransform.GetComponent<RotateToTarget>().LookAt();
        Rigidbody anchorInstance;

        anchorInstance =
            Instantiate(anchorPrefab, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        anchorInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward * 2f;
        AnchorMovement anchor = anchorInstance.GetComponent<AnchorMovement>();
        anchor.orginCar = carMovement;
        if (target != null)
        {
            anchor.target = target;
            carMovement.fixedMotorTorque -= 15;
        }

        ResetBoost();
    }
    private void BombLanuch()
    {
        m_CurrentLaunchForce = m_MaxLaunchForce;

        m_ShootingAudio.clip = m_ChargingClip;
        m_ShootingAudio.Play();
        FireBomb();
    }

        private void FireBomb()
        {
            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

        Rigidbody shellInstance;
        Transform target = m_FireTransform.GetComponent<RotateToTarget>().LookAt();
        if (target != null)
        {
            print("Aim");
            //m_FireTransform.Rotate(-2f, 0f, 0f);
            shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
            ShellExplosion bomb = shellInstance.GetComponent<ShellExplosion>();
            bomb.target = target;
            bomb.orginCar = transform;
        }
        else
        {
            print("No");
            shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

            ShellExplosion bomb = shellInstance.GetComponent<ShellExplosion>();
            bomb.orginCar = transform;
        }
        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

        // Reset the launch force.  This is a precaution in case of missing button events.
        ResetBoost();
        }

        public IEnumerator RecoveryTime(float time)
        {
        yield return new WaitForSeconds(time);
        strongSpeedUpAvailable = true;
        }
        
    }