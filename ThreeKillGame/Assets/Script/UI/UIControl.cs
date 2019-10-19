using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class UIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Text gold;       //玩家金币
    public CreateAndUpdate cau;
    int DifficultyType;
    int forcesId;
    void Start()
    {
        DifficultyType = PlayerPrefs.GetInt("DifficultyType");
        forcesId = PlayerPrefs.GetInt("forcesId");
        //print("DifficultyType:"+ DifficultyType+"........"+ "forcesId:"+ forcesId);
    }

    // Update is called once per frame
    void Update()
    {
        gold.text = "金：" + cau.money;
    }
}
