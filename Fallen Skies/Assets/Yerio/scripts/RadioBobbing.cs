using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBobbing : MonoBehaviour
{
    GameObject player;
    CamMove cam;
    float timer = 0;
    public float speed;
    public float amount;

    Vector3 currentLocation;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GetComponentInParent<CamMove>();
        currentLocation = transform.localPosition;
    }
    public void BobbingOnRadio()
    {
        timer += Time.deltaTime * speed;

        transform.localPosition = new Vector3(currentLocation.x, currentLocation.y + Mathf.Sin(timer) * amount, currentLocation.z);
    }
}
