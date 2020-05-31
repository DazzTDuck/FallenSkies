using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void MainMenuButton(int index)
    {
        switch (index)
        {
            case 0:
                //start
                //load starting level
                StartCoroutine("LoadingSceneStartLevel");
                break;
            case 1:
                //quit
                Application.Quit();
                break;
        }
    }

    IEnumerator LoadingSceneStartLevel()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(1);
    }
}
