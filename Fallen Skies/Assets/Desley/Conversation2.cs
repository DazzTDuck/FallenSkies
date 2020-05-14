using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversation2 : MonoBehaviour
{
    public string[] lines2;
    public int index;
    public int indexMax;

    public bool startDialogue = false;
    public bool dialogueEnded = false;

    public Text dialogueText;
    public GameObject panel;

    public string audioLines;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && startDialogue && !dialogueEnded)
        {
            Dialogue();
            DialogueEnd();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("convTrigger2"))
        {
            panel.SetActive(true);
            dialogueText.text = lines2[0];
            startDialogue = true;
        }
    }

    public void Dialogue()
    {
        index += 1;
        dialogueText.text = lines2[index];
    }

    public void DialogueEnd()
    {
        if (index == indexMax)
        {
            panel.SetActive(false);
            dialogueEnded = true;
        }
    }
}
