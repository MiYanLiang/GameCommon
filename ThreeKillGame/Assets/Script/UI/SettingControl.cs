using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour
{

    // Use this for initialization
    public GameObject settingPanel;//拿到设置面板
    public GameObject soundTxt;//声音文本

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
        int soundStades = PlayerPrefs.GetInt("soundStates");
        if (soundStades == 1)
        {
            soundTxt.GetComponent<Text>().text = "声音    开";
        }
        else if (soundStades == 0)
        {
            soundTxt.GetComponent<Text>().text = "声音    关";
        }
    }

    //关闭修改名字面板
    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }
}