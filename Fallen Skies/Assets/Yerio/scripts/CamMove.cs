using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    float currentY;
    float currentX;
    GameObject player;
    [Header("Offset for Camera")]
    public Vector3 camOffset;
    [Header("Sensetivity")]
    public float sens = 3;
    [Header("Cam Clamp")]
    public float maxY = 45f;
    public float minY = -55f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        currentX -= Input.GetAxis("Mouse Y") * sens;
        currentY += Input.GetAxis("Mouse X") * sens;

        currentX = Mathf.Clamp(currentX, minY, maxY);
        Quaternion rot = Quaternion.Euler(currentX, currentY, 0);
        Quaternion rotPlayer = Quaternion.Euler(0, currentY, 0);
        Camera.main.transform.rotation = rot;
        player.transform.localRotation = rotPlayer;

        transform.position = player.transform.position;
        transform.position += camOffset;
    }
}
