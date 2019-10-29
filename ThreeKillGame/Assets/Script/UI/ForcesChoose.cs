using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using OfficeOpenXml;    //引入使用EPPlus类库

public class ForcesChoose : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> forcesObj = new List<GameObject>();
    int count = 6;
    public List<int> getForces = new List<int>();  //拿到获取到的随机6个势力的下标
    List<string> forcesName = new List<string>();//存放6个势力的昵称
    List<string> forcesExplain = new List<string>();//存放6个势力的说明文字
    [HideInInspector]
    public int currentForcesIndex;
    bool canShow = false;
    public GameObject forcesText;
    void Start()
    {
        RandomList();
        ShowForces();
        //GetExcelFile1();
        for (int i = 0; i < getForces.Count; i++)
        {
            GetForcesName(getForces[i] + 1);
        }
        SetNameForObj();
        SetClickObj();
        //forcesObj[0].SetActive(true);
        //forcesText.GetComponent<Text>().text = "\u3000\u3000" + forcesExplain[0];
    }

    // Update is called once per frame
    private void Update()
    {
        //ShowSelected();
        //print(currentForcesIndex);
    }
    //读表
    //void GetExcelFile1()
    //{
    //    //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
    //    string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
    //    FileInfo fileinfo = new FileInfo(filePath);
    //    using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
    //    {
    //        ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets["ForcesTable"];
    //        for (int i = 0; i < getForces.Count; i++)
    //        {
    //            GetForcesName(worksheet1, getForces[i] + 1);
    //        }
    //    }
    //}
    //随机6个势力
    void RandomList()
    {
        //print(forcesObj.Count);
        while (getForces.Count < count)
        {
            int temp_num = Random.Range(0, forcesObj.Count);
            if (!getForces.Contains(temp_num))
            {
                getForces.Add(temp_num);
            }
        }
    }
    //显示6个势力
    void ShowForces()
    {
        for (int i = 0; i < getForces.Count; i++)
        {
            forcesObj[getForces[i]].SetActive(true);
        }
    }
    //读表获取势力名称显示
    void GetForcesName(int index)
    {
       //Debug.Log(LoadJsonFile.forcesTableDatas[2][2]);
        for (int i = 0; i < 11; i++)
        {
            //print(LoadJsonFile.forcesTableDatas[i][0]);
            if (int.Parse(LoadJsonFile.forcesTableDatas[i][0]) == index)
            {
                forcesName.Add(LoadJsonFile.forcesTableDatas[i][1]);
                forcesExplain.Add(LoadJsonFile.forcesTableDatas[i][2]);
            }
        }
    }
    //将势力名字赋值给前端
    void SetNameForObj()
    {
        for (int i = 0; i < getForces.Count; i++)
        {
            forcesObj[getForces[i]].GetComponentInChildren<Text>().text = forcesName[i];
        }
    }
    //给随机出来的势力添加点击事件
    void SetClickObj()
    {
        forcesObj[getForces[0]].GetComponent<Button>().onClick.AddListener(delegate ()
         {
             canShow = true;
             currentForcesIndex = getForces[0];
             forcesText.GetComponent<Text>().text = "\u3000\u3000" + forcesExplain[0];
         });
        forcesObj[getForces[1]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[1];
            forcesText.GetComponent<Text>().text = "\u3000\u3000" + forcesExplain[1];
        });
        forcesObj[getForces[2]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[2];
            forcesText.GetComponent<Text>().text = "\u3000\u3000" + forcesExplain[2];
        });
        forcesObj[getForces[3]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[3];
            forcesText.GetComponent<Text>().text = "\u3000\u3000" + forcesExplain[3];
        });
        forcesObj[getForces[4]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[4];
            forcesText.GetComponent<Text>().text = "\u3000\u3000" + forcesExplain[4];
        });
        forcesObj[getForces[5]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[5];
            forcesText.GetComponent<Text>().text = "\u3000\u3000" + forcesExplain[5];
        });
    }
    //选中圈显示
    void ShowSelected()
    {
        if (canShow)
        {
            for (int i = 0; i < forcesObj.Count; i++)
            {
                if (i == currentForcesIndex)
                {
                    forcesObj[i].transform.Find("Back").gameObject.SetActive(true);
                   //forcesText.GetComponent<Text>().text = "\u3000\u3000" + forcesExplain[0];
            }
                else
                {
                    forcesObj[i].transform.Find("Back").gameObject.SetActive(false);
                }
            }
        }
    }
}

