using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField] private float moveSpeed, groundDrag, jumpForce, jumpCooldown, airMultiplier;
    bool readyToJump;
    [SerializeField] private float walkSpeed, sprintSpeed;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool isGrounded;
    [SerializeField] private Transform orientation;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    [SerializeField] private Transform cameraPosition;
    Camera cam;
    [SerializeField] private float mouseSens;
    float xRot, yRot;
    [SerializeField] private GameObject player;

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        readyToJump = true;
        
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            player.transform.rotation = cam.transform.rotation;
            cam.transform.position = cameraPosition.position;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

            MyInput();
            SpeedControl();


            if (isGrounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            MovePlayer();


            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSens;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSens;

            yRot += mouseX;
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);

            cam.transform.rotation = Quaternion.Euler(xRot, yRot, 0);
            orientation.rotation = Quaternion.Euler(0, yRot, 0);
        }
    }

    private void MyInput()
    {
        if (photonView.IsMine)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");


            if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
    }

    private void MovePlayer()
    {
        if (photonView.IsMine)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


            if (isGrounded)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {

                    rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force);


                }
                else
                {
                    rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

                }


            }
            else if (!isGrounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        if (photonView.IsMine)
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        if (photonView.IsMine)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void ResetJump()
    {
        if (photonView.IsMine)
        {
            readyToJump = true;
        }
    }
}