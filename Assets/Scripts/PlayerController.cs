using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.Experimental.Rendering;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed, groundDrag, jumpForce, jumpCooldown, airMultiplier;
    bool readyToJump;
    [SerializeField] private float walkSpeed, sprintSpeed;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool isGrounded;
    [SerializeField] private Transform orientation, thirdPersonCam;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    [SerializeField] private Rigidbody rb;
    [Header("Camera Stuff")]
    [SerializeField] private Transform cameraPosition;
    Camera cam;
    [SerializeField] private float mouseSens;
    float xRot, yRot;
    [SerializeField] private GameObject player, hand, shootingPos, endPos, playerModel, head;
    bool persp, fallen;
    [SerializeField] private float pushTime, shootTime, coyoteTime;
    bool holding;
    private LineRenderer lineRenderer;
    [SerializeField] private LayerMask whatisWall;
    private bool wallRunning;
    private Vector3 dir; 
    private GameObject gun; 
    [SerializeField] private int team; 
    private bool justJumped;
    [SerializeField] private Material red, blue;
    [SerializeField] private Animator playeranimator;
    
    [SerializeField] private Transform playerHolder; 
    private Quaternion initialRotationPlayer, initialRotationPlayerModel; 

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        readyToJump = true;
        cam.transform.position = cameraPosition.position;
        persp = true;
        fallen = false;
        pushTime = 2;
        shootTime = .4f;
        gameController.instance.addToList(photonView.ViewID); 
        holding = false;
        initialRotation = this.transform.rotation; 
        initialRotationPlayerModel = playerModel.transform.rotation;
       // lineRenderer = gameObject.AddComponent<LineRenderer>();
        //lineRenderer.positionCount = 2; // Two points for start and end
        //lineRenderer.startWidth = 0.1f; // Adjust the width of the line
       // lineRenderer.endWidth = 0.1f;

    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, whatIsGround);
     
        
        if (photonView.IsMine && !fallen)
        {
             //Input & Speed 
            MyInput();
            SpeedControl();
            

            //Animation triggers 
            if(moveDirection != Vector3.zero)
            {
                playeranimator.SetBool("isMoving", true);
            }
            else
            {
                playeranimator.SetBool("isMoving", false);
            }
            

            //Coyote Timing 
            if(!justJumped&& !isGrounded){
                coyoteTime -= Time.deltaTime;
                if(coyoteTime < 0){
                    coyoteTime = 0;
                }
                    
                
            }
             if(isGrounded){ //if the playerModel is grounded, then simply reset Coyote time constantly. Also adds drag
                coyoteTime = 2f;
                justJumped = false; 
                rb.drag = groundDrag;
            }
            else
                rb.drag = 0;
        
            if(playerModel != null) //if there is a player model, set it's rotation 
            {
                head.transform.rotation = Quaternion.Euler(-180, yRot, 0);
            }

          //if(gun != null)
               // dir = Vector3.Lerp(endPos.transform.forward, hand.transform.forward, 1000f);


           
           // lineRenderer.SetPosition(0, hand.transform.position);
            //lineRenderer.SetPosition(1, endPos.transform.position);
       
            //Shooting
            if(holding && Input.GetMouseButton(0))
            {
                shootTime -= Time.deltaTime; 
                if(shootTime <=0)
                    shoot();
            }

           if (Input.GetKey(KeyCode.F)) //Pushing functionality. Use F key to push. 
            {
                pushTime -= Time.deltaTime;
                if(pushTime <= 0)
                {push();}
            }

            if (Input.GetKey(KeyCode.E))
            {pickup();} //Pickup function

            if (Input.GetKey(KeyCode.G))
            { drop(); } // dropping function 

            //Limited third person functionality. Doesn't work very well right now. 
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (persp)
                    persp = false;
                else
                    persp = true; 
            }
            if (persp == true)
            { cam.transform.position = cameraPosition.transform.position; }
            else if (persp == false)
            { cam.transform.position = thirdPersonCam.transform.position; }


           if (Input.GetKey(KeyCode.P))
            { 
                fallen = true; 
            
             if(horizontalInput > 0 || verticalInput > 0) //If it's moving, then put a force in the direction it's moving. 
                 rb.AddForce((new Vector3(moveDirection.x+2, moveDirection.y, moveDirection.z+2))*2, ForceMode.Impulse);
             else{
                rb.AddForce(transform.forward*2, ForceMode.Impulse);
             }
            }

            
            
           
        }
        else if (fallen && photonView.IsMine)
        {
            if (persp == true)
            { cam.transform.position = cameraPosition.transform.position; }
            else if (persp == false)
            { cam.transform.position = thirdPersonCam.transform.position; }
            cam.transform.rotation = player.transform.rotation;
            rb.freezeRotation = false;
            if (Input.GetKey(KeyCode.T) && fallen == true && isGrounded)
            {
                fallen = false;
                this.transform.rotation = initialRotationPlayer;
                playerModel.transform.rotation = initialRotationPlayerModel;
                //playerModel.transform.Rotate(-playerModel.transform.rotation.x,-playerModel.transform.rotation.y, -playerModel.transform.rotation.z);
                rb.AddForce(transform.up * 7, ForceMode.Impulse);
                rb.freezeRotation = true;
                /* This basically sets the player back up after it's fallen. */
            }
        }
    }
   
    private void FixedUpdate()
    {
        if (photonView.IsMine && !fallen)
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
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
            {
                Jump();
                readyToJump = false;
                Invoke(nameof(ResetJump), jumpCooldown);
            }
            else if(Input.GetKey(jumpKey) && coyoteTime > 0 && !justJumped && !isGrounded){
                justJumped = true; 
                Jump();
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
                { rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force); }
                else
                {rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);}


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
    public void pickup()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, .5f, 0));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
           if(hit.collider.gameObject.tag == "gun" && holding == false)
            {
                hit.collider.gameObject.GetPhotonView().RPC("bePickedUp", RpcTarget.All, photonView.ViewID);
                gun = hit.collider.gameObject;
                holding = true; 
            }
        }
    }
    public void drop()
    {
        hand.transform.GetChild(0).gameObject.GetPhotonView().RPC("beDropped", RpcTarget.All);
        holding = false;
        gun = null;
    }
    public void push()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, .5f, 0));
        ray.origin = cam.transform.position;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.tag == "player")
                {
                    Debug.Log("hitting player");
                    hit.collider.gameObject.GetPhotonView().RPC("pushPerson", RpcTarget.All, cam.transform.forward);
                }
            }
        pushTime = 2;
    }
    [PunRPC]
    public void pushPerson(Vector3 rot)
    {bePushed(rot);}
    public void bePushed(Vector3 rot)
    {
        if (photonView.IsMine)
        {
            fallen = true;
            rb.AddForce(rot * 100f); 
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(0, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {readyToJump = true;}
    public Transform getHand()
    {return hand.transform;}
    public void shoot()
    {
        GameObject obj = PhotonNetwork.Instantiate("shootingObject",gun.transform.GetChild(2).transform.position, Quaternion.identity, 0);
        obj.GetPhotonView().RPC("shooting", RpcTarget.All, photonView.ViewID);
        shootTime = 0.4f;
    }
  public Vector3 cameraVector(){
        Vector3 directionVector = endPos.transform.position - hand.transform.position;
        return directionVector;
  } 
  public bool getWallRunning(){
        return wallRunning;
  }
   public bool setWallRunning(bool wallTrue){
        return wallTrue;
  }
  public Vector3 getDir(){
    return dir; 
  }
  
}