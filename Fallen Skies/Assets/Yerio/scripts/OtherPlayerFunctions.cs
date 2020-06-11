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

    //health
    public Slider healthBar;
    public static float health = 100f;

    public static Transform checkpoint;
   
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

            if (GetComponent<Notes>())
            {
                if (GetComponent<Notes>().notePanelOpen)
                {
                    FindObjectOfType<ButtonManager>().ButtonBack();
                    GetComponent<Notes>().notePanelOpen = false;
                    GetComponent<Notes>().notePanel.SetActive(false);
                }
            }

        }     
    }

    public void Unpause()
    {
        if (Input.GetButtonDown("Cancel") && !inSettings)
        {
            UnpauseUI();
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

    public void UpdateHealthBar()
    {
        healthBar.value = health;
    }

    public void DoDamage(float amount)
    {
        if(health - amount <= 0f)
        {
            //reset player
            health = 100f;
            if (checkpoint != null)
                transform.position = checkpoint.position;
            if(FindObjectOfType<GolemAI>())
            FindObjectOfType<GolemAI>().ResetFOV();

            if (GetComponent<Key>().hasKey) { GetComponent<Key>().hasKey = false; GetComponent<KeyRandomizer>().spawned = false; }
        }
        else
        {
            health -= amount;
        }
        UpdateHealthBar();

    }

    public void SetCheckpoint(Transform refernce)
    {
        checkpoint = refernce;
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
