using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftControl : MonoBehaviour {

	// Use this for initialization
    public Text tittleName;
    string[] productOrder = { "十等", "九等", "八等", "七等", "六等", "五等", "四等", "三等", "二等", "一等" };
    string updateTittle;
    public IdentityControl iden;
    int clickNum;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ChangeLeft()
    {
        clickNum = iden.GetClickNum();
        if (clickNum > 0)
        {
            clickNum--;
            updateTittle = productOrder[clickNum];
            tittleName.text = updateTittle;
        }
        else
        {
            clickNum = 9;
            updateTittle = productOrder[clickNum];
            tittleName.text = updateTittle;
        }
        //Debug.Log(clickNum.ToString());
        iden.SetClickNum(clickNum);
    }
}
