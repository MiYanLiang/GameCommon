using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class UIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gold;
    public CreateAndUpdate cau;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gold.GetComponent<Text>().text = "金：" + cau.money;
    }
}
