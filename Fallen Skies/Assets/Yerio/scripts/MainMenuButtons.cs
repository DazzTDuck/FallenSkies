using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public void MainMenuButton(int index)
    {
        switch (index)
        {
            case 0:
                //start
                //load starting level
                break;
            case 1:
                //quit
                Application.Quit();
                break;
        }
    }
}
