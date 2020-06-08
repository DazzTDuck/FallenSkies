using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public void TriggerStartLoading()
    {
        GetComponentInChildren<Animator>().SetTrigger("Start");
    }
}
