using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioHandler : MonoBehaviour
{
    public GameObject radio;
    bool isOn;

    private void Start()
    {
        isOn = false;
        StartCoroutine("TurnOffRadio");
    }

    public void RadioHandling()
    {
        if (!isOn)
        {
            //turn on
            isOn = true;
            StartCoroutine("TurnOnRadio");
        }
        else
        {
            //turn off
            StartCoroutine("TurnOffRadio");
            isOn = false;
        }
    }

    public IEnumerator TurnOnRadio()
    {
        radio.GetComponent<Animator>().enabled = true;      
        radio.GetComponent<Animator>().ResetTrigger("RadioOut");
        On();
        radio.GetComponent<Animator>().SetTrigger("RadioIn");

        yield return new WaitForSeconds(.5f);

        radio.GetComponent<Animator>().enabled = false;
    }

    public IEnumerator TurnOffRadio()
    {
        radio.GetComponent<Animator>().enabled = true;
        radio.GetComponent<Animator>().ResetTrigger("RadioIn");
        radio.GetComponent<Animator>().SetTrigger("RadioOut");

        yield return new WaitForSeconds(.5f);


        Off();
    }
    public void On()
    {
        radio.SetActive(true);
    }

    public void Off()
    {
        radio.SetActive(false);
    }
}
