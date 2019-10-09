using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NamePanel : MonoBehaviour {

    public GameObject UpdateNameObj;//拿到修改名字的面板
    public GameObject headText;  //头像文本框
    public GameObject text;   //被修改的文本框
    public GameObject inputText;   //输入的文本框
    string inputName = "";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update()
    {
        inputName = inputText.GetComponent<Text>().text;
        if (inputName == "" || inputName == null)
        {
            inputName = "玩家";
        }
    }

    //打开修改名字面板
    public void OpenUpdateNamePanel()
    {
        UpdateNameObj.SetActive(true);
    }

    //关闭修改名字面板
    public void CloseUpdateNamePanel()
    {
        UpdateNameObj.SetActive(false);
    }

    //修改名字
    public void ChangeName()
    {
        text.GetComponent<Text>().text = inputName;
        headText.GetComponent<Text>().text = inputName[0].ToString();
    }

}
