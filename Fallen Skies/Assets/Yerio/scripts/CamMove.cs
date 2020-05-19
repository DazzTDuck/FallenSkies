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

    bool canMoveCam;

    public GameObject pauseMenu;

    bool turnZRight;
    bool turnZLeft;
    bool turnZBack;

    bool getIfLanding;

    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.1f;
    public float crouchingBobbingSpeed = 10f;
    public float bobbingAmountCrouch = 0.02f;

    float timer = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        canMoveCam = true;
        getIfLanding = false;
    }
    private void Update()
    {
        if (!pauseMenu.GetComponentInChildren<AnimationUI>().isTweening && canMoveCam && !player.GetComponent<OtherPlayerFunctions>().isPaused)
        {
            currentX -= Input.GetAxis("Mouse Y") * sens;
            currentY += Input.GetAxis("Mouse X") * sens;
            //wallrunning
            if (turnZRight)
            {
                float add2 = 1;
                currentZ = Mathf.Clamp(currentZ, 0, 10);
                currentZ += add2;
            }

            if (turnZLeft)
            {
                float add = 1f;
                currentZ = Mathf.Clamp(currentZ, -10, 0);
                currentZ -= add;
            }
            //cam rotations and player rotations
            currentX = Mathf.Clamp(currentX, minY, maxY);
            Quaternion rot = Quaternion.Euler(currentX, currentY, currentZ);
            Quaternion rotPlayer = Quaternion.Euler(0, currentY, 0);
            Camera.main.transform.rotation = rot;
            player.transform.rotation = rotPlayer;

            //head bobbing and setting camera on the player
            if (Mathf.Abs(player.GetComponent<PlayerMovement>().moveDir.x) > 0.01f || Mathf.Abs(player.GetComponent<PlayerMovement>().moveDir.z) > 0.01f)
            {
                //Player is moving
                
                if(player.GetComponent<PlayerMovement>().isCrouching)
                {
                    timer += Time.deltaTime * crouchingBobbingSpeed;
                    transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + camOffset.y + Mathf.Sin(timer) * bobbingAmountCrouch, player.transform.position.z);
                }
                else
                {
                    timer += Time.deltaTime * walkingBobbingSpeed;
                    transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + camOffset.y + Mathf.Sin(timer) * bobbingAmount, player.transform.position.z);
                }
            }
            else
            {
               transform.position = player.transform.position;
               transform.position += camOffset;
            }                   

            if (turnZBack)
            {
                float subract = 0.7f;
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
    }
    public void CanMoveMouse()
    {
        canMoveCam = true;
    }
    public void CantMoveMouse()
    {
        canMoveCam = false;
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

    public void CameraLandingBounceTrigger()
    {
        StartCoroutine("Landing");
    }

    public IEnumerator Landing()
    {
        float saveY = gameObject.transform.position.y;
        getIfLanding = true;

        LeanTween.moveLocalY(gameObject, saveY - .2f, 0.2f).setEaseOutBack();

        yield return new WaitForSeconds(0.15f);

        LeanTween.moveLocalY(gameObject, saveY, 0.2f).setEaseLinear();

        yield return new WaitForSeconds(0.21f);

        player.GetComponent<PlayerMovement>().HasJumped(false);
        getIfLanding = false;
    }

    public bool GetIfLanding()
    {
        return getIfLanding;
    }
}
