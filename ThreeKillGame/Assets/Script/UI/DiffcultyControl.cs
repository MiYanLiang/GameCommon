using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffcultyControl : MonoBehaviour
{
    public GameObject DiffcultyPanel;

    //关闭Diffculty面板
    public void CloseDiffcultyPanel()
    {
        DiffcultyPanel.SetActive(false);
    }
    //打开Diffculty面板
    public void OpenDiffcultyPanel()
    {
        DiffcultyPanel.SetActive(true);
    }
}
