using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenRibsControll : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ChickenRibsPanel;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //点击隐藏鸡肋面板
    public void CloseChickenRibsPanel()
    {
        ChickenRibsPanel.SetActive(false);
    }
    //点击打开鸡肋面板
    public void OpenChickenRibsPanel()
    {
        ChickenRibsPanel.SetActive(true);
    }
}
