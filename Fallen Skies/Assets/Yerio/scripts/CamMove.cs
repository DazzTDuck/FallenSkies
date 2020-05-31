using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    float currentY;
    float currentX;
    float currentZ;
    GameObject player;
    [Header("Offset for Camera")]
    public Vector3 camOffset;
    [Header("Sensetivity")]
    public float sens = 3;
    [Header("Cam Clamp")]
    public float maxY = 45f;
    public float minY = -55f;

    public GameObject pauseMenu;

    bool turnZRight;
    bool turnZLeft;
    bool turnZBack;

    bool getIfLanding;

    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.1f;
    public float crouchingBobbingSpeed = 10f;
    public float bobbingAmountCrouch = 0.02f;

    public float landingSpeed = 10f;
    public float landingAmount = .05f;
    

    float timer = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        getIfLanding = false;
    }
    private void Update()
    {
        GetMouseInput();

        WallStepping();

        MouseMovement();

        if (!player.GetComponent<OtherPlayerFunctions>().isPaused)
        {
            if(Mathf.Abs(player.GetComponent<PlayerMovement>().moveDir.x) > 0.01f 
                || Mathf.Abs(player.GetComponent<PlayerMovement>().moveDir.z) > 0.01f)
            {
                if (player.GetComponent<PlayerMovement>().isCrouching)
                {
                    HeadBobbing(crouchingBobbingSpeed, bobbingAmountCrouch, false);
                }
                else
                {
                    HeadBobbing(walkingBobbingSpeed, bobbingAmount, false);
                }
            }
            else
            {
                SetCameraToPlayer();
            }
        }

        CallLanding();

        CurrentZBackToNormal();
    }

    void GetMouseInput()
    {
        if (!player.GetComponent<OtherPlayerFunctions>().isPaused && !pauseMenu.GetComponentInChildren<AnimationUI>().isTweening)
        {
            currentX -= Input.GetAxis("Mouse Y") * sens;
            currentY += Input.GetAxis("Mouse X") * sens;
        }
    }

    void MouseMovement()
    {
        //cam rotations and player rotations
        currentX = Mathf.Clamp(currentX, minY, maxY);
        Quaternion rot = Quaternion.Euler(currentX, currentY, currentZ);
        Quaternion rotPlayer = Quaternion.Euler(0, currentY, 0);
        Camera.main.transform.rotation = rot;
        player.transform.rotation = rotPlayer;
    }

    void WallStepping()
    {
        if (turnZRight)
        {
            float add2 = .9f;
            currentZ = Mathf.Clamp(currentZ, 0, 10);
            currentZ += add2;
        }

        if (turnZLeft)
        {
            float add = .9f;
            currentZ = Mathf.Clamp(currentZ, -10, 0);
            currentZ -= add;
        }
    }

    void CurrentZBackToNormal()
    {
        if (turnZBack)
        {
            float subract = 0.3f;
            if (currentZ > 0)
            {
                currentZ -= subract;

                if (currentZ <= 0)
                {
                    turnZBack = false;
                    currentZ = 0;
                }
            }
            if (currentZ < 0)
            {
                currentZ += subract;

                if (currentZ >= 0)
                {
                    turnZBack = false;
                    currentZ = 0;
                }
            }
        }
    }

    public void TurnCamZ(bool right, bool left)
    {
        turnZRight = right;
        turnZLeft = left;
    }
    public void TurnCamZBack()
    {
        turnZRight = false;
        turnZLeft = false;
        turnZBack = true;
    }

    public void HeadBobbing(float speed, float amount, bool inverted)
    {
        if (inverted)
        {
            timer -= Time.deltaTime * speed;
        }
        else
        {
            timer += Time.deltaTime * speed;
        }
        transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + camOffset.y + Mathf.Sin(timer) * amount, player.transform.position.z);
    }

    void CallLanding()
    {
        if (getIfLanding)
        {
            HeadBobbing(landingSpeed, landingAmount, true);
        }
    }

    void SetCameraToPlayer()
    {
        //when not moving set pos of the camera on the player
        transform.position = player.transform.position;
        transform.position += camOffset;
    }

    public void CameraLandingBounceTrigger()
    {
        StartCoroutine("Landing");        
    }

    public IEnumerator Landing()
    {
        timer = 0f;
        getIfLanding = true;
        
        yield return new WaitForSeconds(0.38f);

        player.GetComponent<PlayerMovement>().HasJumped(false);
        getIfLanding = false;
    }

    public bool GetIfLanding()
    {
        return getIfLanding;
    }
}
