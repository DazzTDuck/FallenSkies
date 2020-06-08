using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject notePanel;
    public GameObject textPanel;
    public GameObject player;
    public GameObject cam;

    public GameObject[] noteText;

    public void Button1(bool button = false)
    {
        if (button)
        {
            notePanel.SetActive(false);
            textPanel.SetActive(true);
            noteText[0].SetActive(true);
        }
    }
    public void Button2(bool button = false)
    {
        if (button)
        {
            notePanel.SetActive(false);
            textPanel.SetActive(true);
            noteText[1].SetActive(true);
        }
    }
    public void Button3(bool button = false)
    {
        if (button)
        {
            notePanel.SetActive(false);
            textPanel.SetActive(true);
            noteText[2].SetActive(true);
        }
    }
    public void Button4(bool button = false)
    {
        if (button)
        {
            notePanel.SetActive(false);
            textPanel.SetActive(true);
            noteText[3].SetActive(true);
        }
    }
    public void ButtonBack(bool button = false)
    {
        textPanel.SetActive(false);
        notePanel.SetActive(true);
        noteText[0].SetActive(false);
        noteText[1].SetActive(false);
        noteText[2].SetActive(false);
        noteText[3].SetActive(false);
    }
    public void ButtonExit(bool button = false)
    {
        notePanel.SetActive(false);
        player.GetComponent<Notes>().notePanelOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
