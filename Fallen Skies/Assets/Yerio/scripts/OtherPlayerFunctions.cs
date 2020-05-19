using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayerFunctions : MonoBehaviour
{
    [Header("Pause Menu")]
    public Transform pauseMenu;
    public GameObject bg;
    public bool isPaused;
    //[HideInInspector]
    public bool inSettings;

    private void Awake()
    {
        isPaused = false;
        inSettings = false;
    }

    void Update()
    {
        if (!isPaused)
        {
            Pause();
        }
        else
        {
            Unpause();
        }

        //teleport button 
        if(Input.GetKeyDown(KeyCode.T))
        transform.position = GameObject.FindGameObjectWithTag("TeleportPoint").transform.position;
    }

    public void Pause()
    {
        if (Input.GetButtonDown("Cancel") && !pauseMenu.GetComponentInChildren<AnimationUI>().isTweening)
        {
            bg.SetActive(true);
            //put up the pause menu and stop time
            isPaused = true;
            //show cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //play UI animaiton
            foreach (var uiChild in pauseMenu.GetComponentsInChildren<AnimationUI>())
            {
                uiChild.OnButtonPressPlayAnimation();
            }
        }     
    }

    public void Unpause()
    {
        if (Input.GetButtonDown("Cancel") && !inSettings)
        {
            bg.SetActive(false);
            //get rid of pause menu and set time normal again
            isPaused = false;
            //cursor hidden
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //play UI animaiton
            foreach (var uiChild in pauseMenu.GetComponentsInChildren<AnimationUI>())
            {
                uiChild.OnCloseAnimation();
            }
        }
    }

    //this if for the UI resume button to unpause the game
    public void UnpauseUI()
    {
        bg.SetActive(false);
        //get rid of pause menu and set time normal again
        isPaused = false;
        //cursor hidden
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //play UI animaiton
        foreach (var uiChild in pauseMenu.GetComponentsInChildren<AnimationUI>())
        {
            uiChild.OnCloseAnimation();
        }
    }

    public void SettingsOn()
    {
        inSettings = true;
    }

    public void SettingsOff()
    {
        inSettings = false;
    }

}
