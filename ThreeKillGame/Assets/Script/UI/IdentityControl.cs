using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdentityControl : MonoBehaviour {

    public GameObject IdentityPanel;//拿到身份的面板
    public Text tittleName;
    string[] productOrder = {"十等","九等","八等","七等","六等","五等","四等","三等","二等","一等" };
    string updateTittle;
    int clickNum;
    public void SetClickNum(int a)
    {
        clickNum = a;
    }
    public int GetClickNum()
    {
        return clickNum;
    }
	// Use this for initialization
    void Start()
    {
        clickNum = 0;
        tittleName.text = productOrder[0];
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(clickNum.ToString());

	}
    //打开身份面板
    public void OpenIdentityPanel()
    {
        IdentityPanel.SetActive(true);
    }

    //关闭身份面板
    public void CloseIdentityPanel()
    {
        IdentityPanel.SetActive(false);
    }

    //改变tittle
    public void ChangeRight()
    {
        clickNum++;
        if (clickNum < productOrder.Length)
        {
            updateTittle = productOrder[clickNum];
            tittleName.text = updateTittle;
        }
        else
        {
            clickNum = 0;
            updateTittle = productOrder[clickNum];
            tittleName.text = updateTittle;
        }
        Debug.Log(clickNum.ToString());
    }
    //public void ChangeLeft()
    //{
    //    if (clickNum > 0)
    //    {
    //        clickNum--;
    //        updateTittle = productOrder[clickNum];
    //        tittleName.text = updateTittle;
    //    }
    //    else
    //    {
    //        clickNum = 9;
    //        updateTittle = productOrder[clickNum];
    //        tittleName.text = updateTittle;
    //    }
    //        Debug.Log(clickNum.ToString());
    //}



}
