using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class DifficultyClick : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject difficultyBtn;
    int difficultyType;
    public GameObject showText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //判断点击的是哪个按钮
    public void GetDifficultyType()
    {
        if (difficultyBtn.name == "Difficulty1")
        {
            showText.GetComponent<Text>().text = "【萌新难度】";
        }
        else if (difficultyBtn.name == "Difficulty2")
        {
            showText.GetComponent<Text>().text = "【普通难度】";
        }
        else if (difficultyBtn.name == "Difficulty3")
        {
            showText.GetComponent<Text>().text = "【困难难度】";
        }
        else if (difficultyBtn.name == "Difficulty4")
        {
            showText.GetComponent<Text>().text = "【炼狱难度】";
        }
    }
}
