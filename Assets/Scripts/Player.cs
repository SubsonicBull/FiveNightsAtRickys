﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Player : MonoBehaviour
{
    //Movement
    [SerializeField] float speed;
    private float gravity = 1f;
    private CharacterController crcon;
    private Transform groundcheck;
    private float distancetoground;
    [SerializeField] LayerMask groundLayerMask;
    private Vector3 velocity;
    private bool isGrounded;

    //Cam
    [SerializeField] float Sensitivity = 100f;
    private Transform playerBody;
    [SerializeField] Transform playerCam;
    private float xRot = 0f;

    //Interaction
    private Transform InteractorSource;
    [SerializeField] float InteractRange;

    //CCTV
    private bool using_CCTV;

    //Flashlight
    private GameObject flashlight;

    public void Set_using_CCTV(bool new_using_CCTV)
    {
        using_CCTV = new_using_CCTV;

        if (!using_CCTV)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Cursor.SetCursor(null, screenCenter, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        { 
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Cursor.SetCursor(null, screenCenter, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Awake()
    {
        crcon = GetComponent<CharacterController>();
        groundcheck = transform.Find("Groundcheck");
        playerBody = GetComponent<Transform>();
        flashlight = GetComponentInChildren<Light>().GetComponentInParent<Transform>().gameObject;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        InteractorSource = playerCam;
    }

    void Update()
    {
        if(!PauseMenu.gamePaused)
        {
            FakeUpdate();
        }
    }

    void FakeUpdate()
    {
        //Player Movement

        isGrounded = Physics.CheckSphere(groundcheck.position, distancetoground, groundLayerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(!using_CCTV)
        {
            Vector3 move = transform.right * x + transform.forward * z;
            crcon.Move(move * speed * Time.deltaTime);
            velocity.y -= gravity * Time.deltaTime;
            crcon.Move(velocity);
        }


        //Camera Movement

        if(!using_CCTV)
        {
            float mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;

            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);

            playerCam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
        

        //Interaction

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }


        //Flashlight

        if(Input.GetKeyDown(KeyCode.F))
        {
            if (flashlight.activeSelf)
            {
                flashlight.SetActive(false);
            }
            else
            {
                flashlight.SetActive(true);
            }
        }
    }
}