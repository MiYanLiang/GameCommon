﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpControl : MonoBehaviour
{

    public GameObject helpPanel;//拿到修改名字的面板

    //打开帮助面板
    public void OpenHelpPanel()
    {
        helpPanel.SetActive(true);
    }

    //关闭帮助面板
    public void CloseHelpPanel()
    {
        helpPanel.SetActive(false);
    }
}