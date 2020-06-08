using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundKiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<OtherPlayerFunctions>().DoDamage(999f);
        }
    }
}
