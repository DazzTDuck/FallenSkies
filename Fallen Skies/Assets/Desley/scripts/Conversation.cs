using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversation : MonoBehaviour
{
    public string[] lines;
    public string[] lines2;
    public string[] lines3;

    public bool[] convStarted;

    public int index;
    public int indexMax;

    public bool startDialogue = false;
    public bool dialogueEnded = false;

    public GameObject panel;
    public Text dialogueText;

    public string audioLines;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && startDialogue)
        {
            Dialogue();
            DialogueEnd();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("convTrigger") && !convStarted[0])
        {
            FindObjectOfType<RadioHandler>().RadioHandling();
            convStarted[0] = true;
            indexMax = 10;
            panel.SetActive(true);
            dialogueText.text = lines[0];
            startDialogue = true;
            Destroy(other.gameObject);
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
        }
       if (other.gameObject.CompareTag("convTrigger2") && !convStarted[1])
        {
            FindObjectOfType<RadioHandler>().RadioHandling();
            convStarted[1] = true;
            indexMax = 9;
            panel.SetActive(true);
            dialogueText.text = lines2[0];
            startDialogue = true;
            Destroy(other.gameObject);
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
            this.gameObject.GetComponent<KeyRandomizer>();
        }
    }

    public void Dialogue()
    {
        index += 1;
        if (convStarted[0] && !convStarted[1] && !convStarted[2])
        {
            dialogueText.text = lines[index];
        }
        else if (convStarted[0] && convStarted[1] && !convStarted[2])
        {
            dialogueText.text = lines2[index];
        }
        else if (convStarted[0] && convStarted[1] && convStarted[2])
        {
            dialogueText.text = lines3[index];
        }
    }

    public void LastDialogue()
    {
        if (!convStarted[2])
        {
            FindObjectOfType<RadioHandler>().RadioHandling();
            convStarted[2] = true;
            indexMax = 8;
            panel.SetActive(true);
            dialogueText.text = lines3[0];
            startDialogue = true;
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public void DialogueEnd()
    {
        if(index == indexMax)
        {
            FindObjectOfType<RadioHandler>().RadioHandling();
            panel.SetActive(false);
            startDialogue = false;
            index = 0;
            this.gameObject.GetComponent<PlayerMovement>().enabled = true;
        }
    }
}
