using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject cam;
    public GameObject portalOff;
    public GameObject portalOn;
    public GameObject activateText;

    public bool hasKey;

    public float range;

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
        if(hit.collider.CompareTag("KeySlot"))
        {
            portalOff.SetActive(false);
            portalOn.SetActive(true);
            activateText.SetActive(false);
            hasKey = false;
            this.GetComponent<PlayerMovement>().enabled = false;
            this.GetComponent<Conversation>().LastDialogue();
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
