using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notes : MonoBehaviour
{
    public GameObject cam;
    public GameObject[] note;
    public GameObject[] noteButton;
    public GameObject notePanel;
    public GameObject openText;

    public bool[] noteBool;

    public float range;

    public bool notePanelOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Pickup();
            Readable();
        }

        if (Input.GetKeyDown("n") && !notePanelOpen)
        {
            notePanel.SetActive(true);
            notePanelOpen = true;
            openText.SetActive(false);
            cam.GetComponent<CamMove>().sens = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Pickup()
    {
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range);

        if(hit.collider.CompareTag("note1"))
        {
            note[0].SetActive(false);
            noteBool[0] = true;
            openText.SetActive(true);
        }
        else if(hit.collider.CompareTag("note2"))
        {
            note[1].SetActive(false);
            noteBool[1] = true;
            openText.SetActive(true);
        }
        else if (hit.collider.CompareTag("note3"))
        {
            note[2].SetActive(false);
            noteBool[2] = true;
            openText.SetActive(true);
        }
        else if (hit.collider.CompareTag("note4"))
        {
            note[3].SetActive(false);
            noteBool[3] = true;
            openText.SetActive(true);
        }
    }

    public void Readable()
    {
        if (noteBool[0])
        {
            noteButton[0].SetActive(true);
        }
        if (noteBool[1])
        {
            noteButton[1].SetActive(true);
        }
        if (noteBool[2])
        {
            noteButton[2].SetActive(true);
        }
        if (noteBool[3])
        {
            noteButton[3].SetActive(true);
        }
    }
}
