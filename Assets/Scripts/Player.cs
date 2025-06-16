﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    //UI
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TMP_Text interactionDescribtionText;
    [SerializeField] private ProgressBar progressbar;
    [SerializeField] private TMP_Text interactionNameText;

    //Movement
    [SerializeField] float speed;
    private float gravity = 1f;
    private CharacterController crcon;
    private Transform groundcheck;
    private float distancetoground;
    [SerializeField] LayerMask groundLayerMask;
    private Vector3 velocity;
    private Vector2 xzVelocity = new Vector2(0, 0);
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
    private bool interacting = false;
    private Interactable currentInteractable;
    private float interactionTimer = 0f;

    //Flashlight
    [SerializeField] GameObject flashlight;

    //Locks/Unlocks players movement and camera-movement
    public void LockPlayer(bool l)
    {
        locked = l;
    }

    //Unlocks/Locks Cursor
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

    void DisplayInteractionUI()
    {
        interactionUI.SetActive(true);
    }

    void HideInteractionUI()
    {
        interactionUI.SetActive(false);
    }

    void InteractionDone()
    {
        HideInteractionUI();
        if (currentInteractable.GetLocksPlayer())
        {
            LockPlayer(false);
        }
        if (currentInteractable.GetUnlocksCursor())
        {
            UnlockCursor(false);
        }
        interacting = false;
        currentInteractable.Done.RemoveListener(InteractionDone);
    }

    void Awake()
    {
        crcon = GetComponent<CharacterController>();
        groundcheck = transform.Find("Groundcheck");
        playerBody = GetComponent<Transform>();
        flashlight.SetActive(false);
        ActionMaster.SetFlashlightOn(false);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        InteractorSource = playerCam;

        HideInteractionUI();
    }

    void Update()
    {
        if(!PauseMenu.gamePaused)
        {
            DoUpdate();
        }
    }

    void DoUpdate()
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
            xzVelocity = new Vector2(move.x, move.z);
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


        //Checking for interactables in front of player to display Name
        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, InteractRange) && !interacting && !isInteractingWithUI)
        {
            if (hit.collider.gameObject.TryGetComponent(out Interactable interactable))
            {
                interactionNameText.gameObject.SetActive(true);
                string interName = interactable.GetInteractionName();
                interactionNameText.text = interName + " [e]";
            }
            else
            {
                if (interactionNameText.gameObject.activeSelf)
                {
                    interactionNameText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (interactionNameText.gameObject.activeSelf)
            {
                interactionNameText.gameObject.SetActive(false);
            }
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
                            //Interaction with UI
                            interactObj.UIInteract();
                            isInteractingWithUI = true;
                        }
                        else
                        {
                            //Interaction that requires time
                            DisplayInteractionUI();
                            interactionTimer = currentInteractable.GetDuration();
                            interactionDescribtionText.text = currentInteractable.GetDescribtion();
                            currentInteractable.Done.AddListener(InteractionDone);
                            currentInteractable.StartInteraction();
                            interacting = true;
                        }
                    }
                }
            }          
        }

        //Interaction with duration
        if (Input.GetKey(KeyCode.E) && interacting)
        {
            progressbar.SetProgress(interactionTimer / currentInteractable.GetDuration());
            interactionTimer -= Time.deltaTime;
        }
        else
        {
            if (interacting)
            {
                currentInteractable.StoppInteraction();
                InteractionDone();
            }
        }


        //Toggle Flashlight

        if(Input.GetKeyDown(KeyCode.F))
        {
            if (flashlight.activeSelf)
            {
                flashlight.SetActive(false);
                ActionMaster.SetFlashlightOn(false);
            }
            else
            {
                flashlight.SetActive(true);
                ActionMaster.SetFlashlightOn(true);
            }
        }
    }

    public Vector3 GetXZVelocity()
    {
        return xzVelocity;
    }

    public bool GetIsLocked()
    {
        return locked;
    }
}