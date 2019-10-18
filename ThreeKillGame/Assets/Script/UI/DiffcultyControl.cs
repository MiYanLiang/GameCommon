using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffcultyControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DiffcultyPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
