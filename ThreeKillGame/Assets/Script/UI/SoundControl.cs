using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{

    // Use this for initialization
    public GameObject showSoundTxt;
    public GameObject cameraAudio;
    int soundStates;

    public void ClickSoundSet()
    {
        if (showSoundTxt.GetComponent<Text>().text == "声音    开")
        {
            soundStates = 0;
            cameraAudio.GetComponent<AudioListener>().gameObject.SetActive(false);
            showSoundTxt.GetComponent<Text>().text = "声音    关";
        }
        else if (showSoundTxt.GetComponent<Text>().text == "声音    关")
        {
            soundStates = 1;
            cameraAudio.GetComponent<AudioListener>().gameObject.SetActive(true);
            showSoundTxt.GetComponent<Text>().text = "声音    开";
        }
        PlayerPrefs.SetInt("soundStates", soundStates);
        print("..................soundStates:" + soundStates);
    }
}
