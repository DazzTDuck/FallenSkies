using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintTrigger : MonoBehaviour
{
    public GameObject player;
    public bool triggerd;

    // Start is called before the first frame update
    void Start()
    {
        triggerd = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !triggerd)
        {
            triggerd = true;
            Destroy(this.gameObject);
            player.GetComponent<Hints>().timer = 0;
            player.GetComponent<Hints>().DisplayHint();
        }
    }
}
