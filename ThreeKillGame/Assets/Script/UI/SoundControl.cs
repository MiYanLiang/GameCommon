using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public GameObject showSoundTxt;
    private int soundStates;

    [SerializeField]
    Transform[] transforms;

    private void Awake()
    {
        soundStates = 1;
        soundStates = PlayerPrefs.GetInt("soundStates");
    }

    private void Start()
    {
        if (soundStates == 1)
        {
            showSoundTxt.GetComponent<Text>().text = "声音    关";
        }
        else
        {
            showSoundTxt.GetComponent<Text>().text = "声音    开";
        }
        ChangeSoundState();
    }

    public void ClickSoundSet()
    {
        if (soundStates == 1)
        {
            soundStates = 0;
            showSoundTxt.GetComponent<Text>().text = "声音    关";
        }
        else
        {
            soundStates = 1;
            showSoundTxt.GetComponent<Text>().text = "声音    开";
        }
        PlayerPrefs.SetInt("soundStates", soundStates);
        ChangeSoundState();
    }

    private void ChangeSoundState()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            foreach (var item in transforms[i].GetComponentsInChildren<AudioSource>())
            {
                if (soundStates == 0)
                {
                    item.spatialBlend = item.volume;
                    item.volume = item.volume * soundStates;
                    //item.Pause();
                }
                else
                {
                    item.volume = item.spatialBlend;
                    item.spatialBlend = 0;
                    //item.Play();
                }
            }
        }
    }
}
