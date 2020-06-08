﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject cam;
    public GameObject portalEffect;
    public GameObject activateText;

    public bool hasKey;

    public float range;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && hasKey)
        {
            Raycast();
        }
    }

    public void Raycast()
    {
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range);
        if (hit.collider.CompareTag("portalCollider"))
        {
            portalEffect.SetActive(true);
            activateText.SetActive(false);
            hasKey = false;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("key"))
        {
            Destroy(other.gameObject);
            activateText.SetActive(true);
            hasKey = true;
        }
    }
}