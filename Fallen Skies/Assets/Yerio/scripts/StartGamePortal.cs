﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGamePortal : MonoBehaviour
{
    public int sceneIndex;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<LevelLoader>().TriggerStartLoading();
            StartCoroutine("LoadingSceneStartLevel");
        }
    }

    IEnumerator LoadingSceneStartLevel()
    {
        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(sceneIndex);
    }
}
