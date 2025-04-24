﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool locked;
    private bool isInteractingWithUI = false;
    private Interactable currentInteractable;

    //Flashlight
    [SerializeField] GameObject flashlight;

    //Locks players movement and camera-movemnet
    public void LockPlayer(bool l)
    {
        locked = l;
    }

    public void UnlockCursor(bool l)
    {
        if (!l)
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

        if(!locked)
        {
            Vector3 move = transform.right * x + transform.forward * z;
            crcon.Move(move * speed * Time.deltaTime);
            velocity.y -= gravity * Time.deltaTime;
            crcon.Move(velocity);
        }


        //Camera Movement

        if(!locked)
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
            if (isInteractingWithUI)
            {
                if (currentInteractable.GetIsUIInteraction())
                {
                    currentInteractable.QuitUIInteraction();
                    if (currentInteractable.GetLocksPlayer())
                    {
                        LockPlayer(false);
                    }
                    if (currentInteractable.GetUnlocksCursor())
                    {
                        UnlockCursor(false);
                    }
                    isInteractingWithUI = false;
                    Debug.Log("Quit UI-Innteraction");
                }
            }
            else
            {
                Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
                if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
                {
                    if (hitInfo.collider.gameObject.TryGetComponent(out Interactable interactObj))
                    {
                        Debug.Log("Interacting with: " + interactObj.gameObject.name);
                        currentInteractable = interactObj;
                        if (interactObj.GetLocksPlayer())
                        {
                            LockPlayer(true);
                        }
                        if (interactObj.GetUnlocksCursor())
                        {
                            UnlockCursor(true);
                        }
                        if (interactObj.GetIsUIInteraction())
                        {
                            interactObj.UIInteract();
                            isInteractingWithUI = true;
                        }
                        else
                        {
                            //Interaction that requires time
                        }
                    }
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