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

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        savedMoveSpeed = moveSpeed;
    }

    public void Update()
    {
        //movement
        //moveDir is where the direction is determined, by which key you press and the movement speed
        //transform.Translate makes it that the player moves
        moveDir = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
        transform.Translate(moveDir);
    }

    private void LateUpdate()
    {
        //the jumping of the player
        Jumping();
    }

    public void Jumping()
    {
        if (Input.GetButtonDown("Jump") && !hasJumped)
        {
            hasJumped = true;
            SlowMovement();
            if (moveDir.z == 0)
            {
                //jump up                
                playerRb.velocity = (new Vector3(0, jumpPower, 0));                
            }

            if (moveDir.z > 0)
            {
                //jump forward
                //add the input to the jump calculation to determine the speed of the jump
                //so the faster you're walking the further you jump  
                //also adds force like a boost jumping forward
                float extraJump = (moveDir.z * 1f) + jumpPower;
                playerRb.AddForce(transform.position + transform.forward * 90 * extraJump);
                playerRb.velocity =new Vector3(0, jumpPower - 1,0);               
            }

            if (moveDir.z < 0)
            {
                //jump backwards
                //add the input to the jump calculation to determine the speed of the jump
                //so the faster you're walking the further you jump 
                //also adds force like a boost jumping forward
                float extraJump = (moveDir.z * 1f) + jumpPower;
                playerRb.AddForce(transform.position - transform.forward * 90 * extraJump);
                playerRb.velocity = new Vector3(0, jumpPower - 1,0);               
            }

            if (moveDir.x > 0)
            {
                //jump right        
                //add the input to the jump calculation to determine the speed of the jump
                //so the faster you're walking the further you jump  
                //also adds force like a boost jumping forward
                float extraJump = (moveDir.x * 1f) + jumpPower;
                playerRb.AddForce(transform.position + transform.right * 90 * extraJump);
                playerRb.velocity = new Vector3(0, jumpPower - 1, 0);
            }

            if (moveDir.x < 0)
            {
                //jump left
                //add the input to the jump calculation to determine the speed of the jump
                //so the faster you're walking the further you jump
                //also adds force like a boost jumping forward
                float extraJump = (moveDir.x * 1f) + jumpPower;
                playerRb.AddForce(transform.position - transform.right * 90 * extraJump);
                playerRb.velocity = new Vector3(0, jumpPower - 1, 0);
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
            hasJumped = false;
            moveSpeed = savedMoveSpeed;
            timesWallJumped = 0;
        }

        if (other.gameObject.CompareTag("jumpableWall"))
        {
            if (timesWallJumped < timesThatCanWalljumpInARow)
            {
                hasJumped = false;
                SlowMovement();
            }
        }

        if (other == null)
        {
            hasJumped = false;
        }
    }
 
    public void SlowMovement()
    {
        moveSpeed /= 10;
    }

}
