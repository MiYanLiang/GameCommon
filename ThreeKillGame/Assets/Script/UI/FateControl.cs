using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FateControl : MonoBehaviour {

	// Use this for initialization
    public GameObject fatePanel;//拿到缘集的面板

    //打开身份面板
    public void OpenFatePanel()
    {
        fatePanel.SetActive(true);
    }

    //关闭身份面板
    public void CloseFatePanel()
    {
        fatePanel.SetActive(false);
    }
}