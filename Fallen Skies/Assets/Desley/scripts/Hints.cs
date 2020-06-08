using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hints : MonoBehaviour
{
    public GameObject hintPanel;
    public Text hintText;

    public string[] lines;

    public int index;

    public float timer;
    public bool timerReached;

    // Start is called before the first frame update
    void Start()
    {
        index = -1;
        timerReached = true;
        hintPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerReached)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 10)
        {
            timerReached = true;
            timer = 0;
            CloseHint();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("hintTrigger"))
        {
            index++;
            DisplayHint();
            timerReached = false;
            Destroy(other.gameObject);
        }
    }

    public void DisplayHint()
    {
        hintPanel.SetActive(true);
        hintText.text = lines[index];
    }

    public void CloseHint()
    {
        hintPanel.SetActive(false);
    }

}
