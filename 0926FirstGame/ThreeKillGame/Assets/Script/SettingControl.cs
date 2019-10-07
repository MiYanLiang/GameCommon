using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingControl : MonoBehaviour {

	// Use this for initialization
    public GameObject settingPanel;//拿到设置面板
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    //关闭修改名字面板
    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }
}
