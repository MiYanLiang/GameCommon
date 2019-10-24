using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库
using System.Linq;  //去除重复

public class ClickPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    ////////////////////////////////////////////////////////存放某类型兵种的名字
    List<string> shieldSoldierName = new List<string>();  //盾兵
    List<string> mahoutName = new List<string>();  //象兵
    List<string> halberdierName = new List<string>();  //戟兵
    List<string> lifeguardName = new List<string>();  //禁卫
    List<string> spearmanName = new List<string>();  //枪兵
    List<string> sowarName = new List<string>();  //骑兵
    List<string> counsellorName = new List<string>();  //军师
    List<string> sapperName = new List<string>();  //工兵
    List<string> necromancerName = new List<string>();  //方士
    List<string> god_beastName = new List<string>();//神兽
///////////////////////////////////////////////////////////存放某类型兵种的所有id
    List<int> shieldSoldierId = new List<int>();  //盾兵
    List<int> mahoutId = new List<int>();  //象兵
    List<int> halberdierId = new List<int>();  //戟兵
    List<int> lifeguardId = new List<int>();  //禁卫
    List<int> spearmanId = new List<int>();  //枪兵
    List<int> sowarId = new List<int>();  //骑兵
    List<int> counsellorId = new List<int>();  //军师
    List<int> sapperId = new List<int>();  //工兵
    List<int> necromancerId = new List<int>();  //方士
    List<int> god_beastId = new List<int>();  //神兽
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickSoldiersPrefab()
    {
        GameObject.Find("TopInformationBar").GetComponentInChildren<Text>().text = "";
        init_btn();
        if (this.GetComponentsInChildren<Text>()[0].text == "盾兵")
        {
            for (int i = 0; i < shieldSoldierName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【盾兵】"+"\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += shieldSoldierName[i] + "\u2000";
            }
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "象兵")
        {
            for (int i = 0; i < mahoutName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【象兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "没有没有没有。。。";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += mahoutName[i] + "\u2000";
            }
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "戟兵")
        {
            for (int i = 0; i < halberdierName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【戟兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "没有没有没有。。。";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += halberdierName[i] + "\u2000";
            }
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "禁卫")
        {
            for (int i = 0; i < lifeguardName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【禁卫】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "没有没有没有。。。";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += lifeguardName[i] + "\u2000";
            }
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "枪兵")
        {
            for (int i = 0; i < spearmanName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【枪兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += spearmanName[i] + "\u2000";
            }
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "骑兵")
        {
            for (int i = 0; i < sowarName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【骑兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += sowarName[i] + "\u2000";
            }
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "军师")
        {
            for (int i = 0; i < counsellorName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【军师】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += counsellorName[i] + "\u2000";
            }
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "工兵")
        {
            for (int i = 0; i < sapperName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【工兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += sapperName[i] + "\u2000";
            }
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "方士")
        {
            for (int i = 0; i < necromancerName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【方士】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += necromancerName[i] + "\u2000";
            }
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "神兽")
        {
            for (int i = 0; i < god_beastId.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【神兽】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += god_beastId[i] + "\u2000";
            }
        }

    }
    void init_btn()
    {
        GetExcelFile3();
        for (int i = 0; i < shieldSoldierName.Count; i++)
        {
            //print(shieldSoldierName[i]);
        }
    }
    void GetExcelFile3()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            GetSoldiersAll(worksheet1);
            for (int i = 0; i < shieldSoldierId.Count; i++)
            {
                shieldSoldierName.Add(GetHeroNameFromId(worksheet1, shieldSoldierId[i]));
            }
            for (int i = 0; i < mahoutId.Count; i++)
            {
                mahoutName.Add(GetHeroNameFromId(worksheet1, mahoutId[i]));
            }
            for (int i = 0; i < halberdierId.Count; i++)
            {
                halberdierName.Add(GetHeroNameFromId(worksheet1, halberdierId[i]));
            }
            for (int i = 0; i < lifeguardId.Count; i++)
            {
                lifeguardName.Add(GetHeroNameFromId(worksheet1, lifeguardId[i]));
            }
            for (int i = 0; i < spearmanId.Count; i++)
            {
                spearmanName.Add(GetHeroNameFromId(worksheet1, spearmanId[i]));
            }
            for (int i = 0; i < sowarId.Count; i++)
            {
                sowarName.Add(GetHeroNameFromId(worksheet1, sowarId[i]));
            }
            for (int i = 0; i < counsellorId.Count; i++)
            {
                counsellorName.Add(GetHeroNameFromId(worksheet1, counsellorId[i]));
            }
            for (int i = 0; i < sapperId.Count; i++)
            {
                sapperName.Add(GetHeroNameFromId(worksheet1, sapperId[i]));
            }
            for (int i = 0; i < necromancerId.Count; i++)
            {
                necromancerName.Add(GetHeroNameFromId(worksheet1, necromancerId[i]));
            }
            for (int i = 0; i < god_beastId.Count; i++)
            {
                god_beastName.Add(GetHeroNameFromId(worksheet1, god_beastId[i]));
            }
        }
    }
    void GetSoldiersAll(ExcelWorksheet worksheet)
    {
        int num = 4;
        for (int i = 1; i < 89 + 1; i++)
        {
            if (i > 1)
            {
                if (worksheet.Cells[i, num].Value.ToString() == "1")
                {
                    shieldSoldierId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "2")
                {
                    mahoutId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "3")
                {
                    halberdierId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "4")
                {
                    lifeguardId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "5")
                {
                    spearmanId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "6")
                {
                    sowarId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "7")
                {
                    counsellorId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "8")
                {
                    sapperId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "9")
                {
                    necromancerId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "10")
                {
                    god_beastId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
            }
        }
    }
    string GetHeroNameFromId(ExcelWorksheet worksheet, int id)
    {
        string name = "";
        for (int i = 1; i < 89 + 1; i++)
        {
            if (i > 1)
            {
                if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == id)
                {
                    name = worksheet.Cells[i, 2].Value.ToString();
                }
            }
        }
        return name;
    }
}
