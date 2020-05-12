using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        canMoveCam = true;
    }
    private void Update()
    {
        if (!pauseMenu.GetComponentInChildren<AnimationUI>().isTweening && canMoveCam && !player.GetComponent<OtherPlayerFunctions>().isPaused)
        {
            currentX -= Input.GetAxis("Mouse Y") * sens;
            currentY += Input.GetAxis("Mouse X") * sens;
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

            currentX = Mathf.Clamp(currentX, minY, maxY);
            Quaternion rot = Quaternion.Euler(currentX, currentY, currentZ);
            Quaternion rotPlayer = Quaternion.Euler(0, currentY, 0);
            Camera.main.transform.rotation = rot;
            player.transform.rotation = rotPlayer;


            transform.position = player.transform.position;
            transform.position += camOffset;


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
        LeanTween.moveLocalY(gameObject, saveY - .2f, 0.2f).setEaseOutBack();

        yield return new WaitForSeconds(0.1f);

        LeanTween.moveLocalY(gameObject, saveY, 0.2f).setEaseLinear();

        yield return new WaitForSeconds(0.1f);

        player.GetComponent<PlayerMovement>().HasJumped(false);
    }
}
