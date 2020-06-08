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
            convStarted[0] = true;
            indexMax = 10;
            panel.SetActive(true);
            dialogueText.text = lines[0];
            startDialogue = true;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("convTrigger") && !convStarted[1])
        {
            convStarted[1] = true;
            indexMax = 9;
            panel.SetActive(true);
            dialogueText.text = lines2[0];
            startDialogue = true;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("convTrigger") && !convStarted[2])
        {
            convStarted[2] = true;
            indexMax = 8;
            panel.SetActive(true);
            dialogueText.text = lines3[0];
            startDialogue = true;
            Destroy(other.gameObject);
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

    public void DialogueEnd()
    {
        if(index == indexMax)
        {
            panel.SetActive(false);
            startDialogue = false;
            index = 0;
        }
    }
}
