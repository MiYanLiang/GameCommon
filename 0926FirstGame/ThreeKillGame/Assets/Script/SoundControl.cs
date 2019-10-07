using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour {

	// Use this for initialization
    public GameObject touchSound;
    public GameObject touchSoundClick;
    public GameObject music;
    public GameObject musicClick;
    public GameObject sound;
    public GameObject soundClick;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //打开及关闭触音
    public void OpenTouchSound()
    {
        touchSoundClick.SetActive(false);
        touchSound.SetActive(true);
    }
    public void CloseTouchSound()
    {
        touchSound.SetActive(false);
        touchSoundClick.SetActive(true);
    }
    //打开及关闭音乐
    public void OpenMusic()
    {
        musicClick.SetActive(false);
        music.SetActive(true);
    }
    public void CloseMusic()
    {
        music.SetActive(false);
        musicClick.SetActive(true);
    }
    //打开及关闭音效
    public void OpenSound()
    {
        soundClick.SetActive(false);
        sound.SetActive(true);
    }
    public void CloseSound()
    {
        sound.SetActive(false);
        soundClick.SetActive(true);
    }

}
