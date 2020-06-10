using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRandomizer : MonoBehaviour
{
    public GameObject key;

    public Vector3[] location;

    public int randomizer;

    public bool spawned;

    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            SpawnKey();
        }
    }

    public void Randomize()
    {
        randomizer = Random.Range(0, location.Length);
        spawned = false;
    }

    public void SpawnKey()
    {
        if(randomizer == 0)
        {
            Instantiate(key, location[0], Quaternion.identity);
            spawned = true;
        }
        else if (randomizer == 1)
        {
            Instantiate(key, location[1], Quaternion.identity);
            spawned = true;
        }
        else if (randomizer == 2)
        {
            Instantiate(key, location[2], Quaternion.identity);
            spawned = true;
        }
    }
}
