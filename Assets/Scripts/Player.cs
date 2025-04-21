﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Player : MonoBehaviour
{
    //Movement
    public float speed;
    private float gravity = 1f;
    public CharacterController crcon;
    public Transform groundcheck;
    public float distancetoground;
    public LayerMask groundLayerMask;
    Vector3 velocity;
    bool isGrounded;

    //Cam
    public float Sensitivity = 100f;
    public Transform playerBody;
    public Transform playerCam;
    float xRot = 0f;

    //Interaction
    private Transform InteractorSource;
    public float InteractRange;

    //CCTV
    bool using_CCTV;

    public void Set_using_CCTV(bool new_using_CCTV)
    {
        using_CCTV = new_using_CCTV;

        if (!new_using_CCTV)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        InteractorSource = playerCam;
    }
    
    void Update()
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

        float mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        if(!using_CCTV)
        {
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
    }
}