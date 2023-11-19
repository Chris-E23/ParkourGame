using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallRunning : MonoBehaviour
{
    [Header("Wall running")]
   
    [SerializeField] LayerMask whatIsWall, whatIsGround;
    [SerializeField] private float wallRunForce, maxWallRunTime, wallRunTimer;
   
    [Header("Input")]
    private float hInput, vInput; 

    [Header("Detection")]
    [SerializeField] private float wallCheckDistance, minJumpHeight;
    private RaycastHit leftWallhit, rightWallhit;
    private bool wallLeft, wallRight; 

    [Header("References")]
    public Transform orientation;
    private PlayerController pc;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
    }
    void Update()
    {
        CheckForWall();
        StateMachine();
    }
    private void FixedUpdate(){
        if(pc.getWallRunning()){
            WallRunningMovement();
        }
    }
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);

    }
    private bool AboveGround(){
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }
    private void StateMachine()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if((wallLeft || wallRight) && vInput > 0 && AboveGround()){
            //wallRun here 
            if(!pc.getWallRunning()){
                StartWallRun();
            }

        }
        else{

            if(pc.getWallRunning()){
                StopWallRun();
            }
        }
    }

    private void StartWallRun(){

         pc.setWallRunning(true);
    }

    private void WallRunning(){


    }
    private void WallRunningMovement(){
        rb.useGravity=false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
   
    if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        wallForward = -wallForward;
    rb.AddForce(wallForward * wallRunForce, ForceMode.Force);


    if(!(wallLeft && hInput > 0 ) && !(wallRight && hInput < 0))
        rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }

    private void StopWallRun(){
        pc.setWallRunning(true);
    }

  
}
