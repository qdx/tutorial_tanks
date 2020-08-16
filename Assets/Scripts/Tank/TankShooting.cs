﻿using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;

    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;                


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;

        //m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce)  / m_MaxChargeTime;
        m_Fired = false;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {

        m_AimSlider.value = m_MinLaunchForce;
        // Track the current state of the fire button and make decisions based on the current launch force.
        if(m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired){
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        } else if(Input.GetButtonDown(m_FireButton)){
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        } else if(!m_Fired && Input.GetButton(m_FireButton)){
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_CurrentLaunchForce = Mathf.Min(m_CurrentLaunchForce, m_MaxLaunchForce);

            m_AimSlider.value = m_CurrentLaunchForce;

        }else if(!m_Fired && Input.GetButtonUp(m_FireButton)){
            Fire();
            //m_Fired = false;
            //m_AimSlider.value = m_MinLaunchForce;
            //m_CurrentLaunchForce = m_MinLaunchForce;
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;
        Rigidbody shell = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shell.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}