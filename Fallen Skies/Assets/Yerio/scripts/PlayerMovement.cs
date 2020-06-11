using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    float savedMoveSpeed;
    public Vector3 moveDir;

    [Header("Jumping")]
    public float jumpPower;
    public int timesThatCanWalljumpInARow;
    bool hasJumped;
    int timesWallJumped;
    Rigidbody playerRb;
    bool isOnWall;
    bool hasTurned = false;

    [Header("Crouching")]
    public float crouchMovement;
    public float slideForce;
    [HideInInspector]
    public bool isCrouching;
    float currentHeight;

    [Header("Player Walking")]
    public float stepDistance;
    float walkTimer = 0f;

    public GameObject pauseMenu;

    public GameObject cam;

    float timeInAir;
    bool isInAir;
    float timer = 0f;
    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        savedMoveSpeed = moveSpeed;
        currentHeight = GetComponent<CapsuleCollider>().height;
        timeInAir = 0f;
    }

    public void FixedUpdate()
    {       
        MovePlayer();
    }

    private void Update()
    {
        //times the air time
        if (isInAir)
            timeInAir += Time.deltaTime;

        //step sounds
        if (!hasJumped && !isInAir)
            walkTimer += Time.deltaTime;


        if (walkTimer > stepDistance && moveDir.sqrMagnitude > 0 && !GetComponent<OtherPlayerFunctions>().isPaused && !isCrouching)
        {
            FindObjectOfType<AudioManager>().PlaySound("Footstep");
            walkTimer = 0f;
        }

        //the jumping of the player
        Jumping();
        //crouching of the player
        Crouching();

        //check if movespeed is normal when not jumping or crouching
        //if so reset movespeed;
        if (!isCrouching && !hasJumped && moveSpeed != savedMoveSpeed && !isOnWall)
        {
            moveSpeed = savedMoveSpeed;
        }

    }

    void MovePlayer()
    {
        //movement
        //moveDir is where the direction is determined, by which key you press and the movement speed
        //transform.Translate makes it that the player moves
        if (!GetComponent<OtherPlayerFunctions>().isPaused && !pauseMenu.GetComponentInChildren<AnimationUI>().isTweening)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal") * Time.fixedDeltaTime * moveSpeed, 0, Input.GetAxis("Vertical") * Time.fixedDeltaTime * moveSpeed);
            transform.Translate(moveDir);
        }
    }

    public void Crouching()
    {
        if (!GetComponent<OtherPlayerFunctions>().isPaused)
        {
            if (Input.GetButtonDown("Crouch"))
            {
                isCrouching = true;
                GetComponent<CapsuleCollider>().height = currentHeight / 1.5f;
                cam.GetComponent<CamMove>().camOffset.y -= 0.2f;
                if (!hasJumped)
                {
                    moveSpeed /= crouchMovement;
                }
                SlidePlayer();

                //make AI radius Smaller
                if (FindObjectOfType<GolemAI>())
                {
                    if(!FindObjectOfType<GolemAI>().foundPlayer)
                    FindObjectOfType<GolemAI>().maxRadius /= 1.5f;
                }
                    

            }
            if (Input.GetButtonUp("Crouch") && isCrouching)
            {
                isCrouching = false;
                GetComponent<CapsuleCollider>().height = currentHeight;
                cam.GetComponent<CamMove>().camOffset.y += 0.2f;
                if (FindObjectOfType<GolemAI>())
                {
                    if(FindObjectOfType<GolemAI>().savedRadius > FindObjectOfType<GolemAI>().maxRadius)
                        FindObjectOfType<GolemAI>().maxRadius *= 1.5f;
                }
            }
        }    
    }

    public void SlidePlayer()
    {
        if(isCrouching && moveDir.z > 0 && !hasJumped)
        {
            playerRb.AddForce(transform.position + transform.forward * slideForce);
        }
    }

    public void Jumping()
    {
        //check if player is crouching
        //if true lower the jumppower otherwise set it to the saved jumppower
        //that is to make sure the jumppower is always set back when player isn't crouching anymore;     
        if (!GetComponent<OtherPlayerFunctions>().isPaused)
        {
            if (Input.GetButtonDown("Jump") && !hasJumped && !isCrouching)
            {
                //plays sound
                FindObjectOfType<AudioManager>().PlaySound("PlayerJump");
                
                hasJumped = true;
                SlowMovementWhenJumping();
                if (moveDir.z == 0)
                {
                    //jump up                
                    playerRb.velocity = (new Vector3(0, jumpPower * 1.7f, 0));
                }

                if (moveDir.z > 0)
                {
                    //jump forward
                    //add the input to the jump calculation to determine the speed of the jump
                    //so the faster you're walking the further you jump  
                    //also adds force like a boost jumping forward
                    float extraJump = (moveDir.z) + jumpPower;
                    playerRb.AddForce(transform.forward * 50 * extraJump);
                    playerRb.velocity = new Vector3(0, jumpPower * 1.2f, 0);
                }

               /*
                if (moveDir.z < 0)
                {
                    //jump backwards
                    //add the input to the jump calculation to determine the speed of the jump
                    //so the faster you're walking the further you jump 
                    //also adds force like a boost jumping forward
                    float extraJump = (moveDir.z) + jumpPower;
                    playerRb.AddForce(transform.position - transform.forward * 40 * extraJump);
                    playerRb.velocity = new Vector3(0, jumpPower, 0);
                }

                if (moveDir.x > 0)
                {
                    //jump right        
                    //add the input to the jump calculation to determine the speed of the jump
                    //so the faster you're walking the further you jump  
                    //also adds force like a boost jumping forward
                    float extraJump = (moveDir.x) + jumpPower;
                    playerRb.AddForce(transform.position + transform.right * 45);
                    playerRb.velocity = new Vector3(0, jumpPower + 1, 0);
                }

                if (moveDir.x < 0)
                {
                    //jump left
                    //add the input to the jump calculation to determine the speed of the jump
                    //so the faster you're walking the further you jump
                    //also adds force like a boost jumping forward
                    float extraJump = (moveDir.x) + jumpPower;
                    playerRb.AddForce(transform.position - transform.right * 45);
                    playerRb.velocity = new Vector3(0, jumpPower + 1, 0);
                }
                */
            }
        }
    }

    //jump resetting
    //also that when you hit an object with a tag "jumpableWall" its resets the jump
    //and it has a limit of how many times you can jump on a wall
    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject.CompareTag("ground"))
        {           
            timesWallJumped = 0;
            isOnWall = false;
            isInAir = false;
            if (hasJumped || timeInAir > 0.5f)
            {
                cam.GetComponent<CamMove>().CameraLandingBounceTrigger();                
            }
            timeInAir = 0;
        }

        if (other.gameObject.CompareTag("jumpableWall"))
        {
            isInAir = false;
            if (timesWallJumped < timesThatCanWalljumpInARow)
            {
                hasJumped = false;
                timesWallJumped++;
                isOnWall = true;

                Ray rayLeft = new Ray(transform.position, -transform.right);
                Ray rayRight = new Ray(transform.position, transform.right);
                RaycastHit hit;
                //turn cam
                //send raycast left and right
                if(Physics.Raycast(rayLeft, out hit, 3f))
                {
                   cam.GetComponent<CamMove>().TurnCamZ(false, true);
                    //Debug.Log("left");
                }
                if (Physics.Raycast(rayRight, out hit, 3f))
                {
                    cam.GetComponent<CamMove>().TurnCamZ(true, false);
                    //Debug.Log("right");
                }
                hasTurned = true;
                
            }
        }

        if (other == null)
        {                      
            //hasJumped = false;
            if (hasTurned)
            {
                cam.GetComponent<CamMove>().TurnCamZBack();
                
                hasTurned = false;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("ground"))
        {          
            timer += Time.deltaTime;
            if(timer > 0.2f && !cam.GetComponent<CamMove>().GetIfLanding())
            {
                hasJumped = false;
                timer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("jumpableWall"))
        {
            isInAir = true;
            if (hasTurned)
            {
                cam.GetComponent<CamMove>().TurnCamZBack();
                hasTurned = false;
            }
        }

        if (other.gameObject.CompareTag("ground"))
        {
            isInAir = true;            
        }
    }


    public void SlowMovementWhenJumping()
    {
        if (!isCrouching && moveSpeed == savedMoveSpeed)
        {
            moveSpeed /= 5.5f;
        }
    }

    private void OnDrawGizmos()
    {
        Ray rayLeft = new Ray(transform.position, -transform.right);
        Ray rayRight = new Ray(transform.position, transform.right);

        //Ray rayTest = new Ray(transform.localPosition, transform.right);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawRay(rayTest);

        //Gizmos.DrawRay(rayLeft);
        //Gizmos.DrawRay(rayRight);
    }

    public void HasJumped(bool state)
    {
        hasJumped = state;
    }

}
