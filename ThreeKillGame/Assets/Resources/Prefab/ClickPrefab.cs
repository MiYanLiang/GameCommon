using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using OfficeOpenXml;    //引入使用EPPlus类库
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
        shieldSoldierName.Clear();
        mahoutName.Clear();
        halberdierName.Clear();
        lifeguardName.Clear();
        spearmanName.Clear();
        sowarName.Clear();
        counsellorName.Clear();
        sapperName.Clear();
        necromancerName.Clear();
        god_beastName.Clear();

        shieldSoldierId.Clear();
        mahoutId.Clear();
        halberdierId.Clear();
        lifeguardId.Clear();
        spearmanId.Clear();
        sowarId.Clear();
        counsellorId.Clear();
        sapperId.Clear();
        necromancerId.Clear();
        god_beastId.Clear();
        GameObject.Find("TopInformationBar").GetComponentInChildren<Text>().text = "";
        init_btn();
        if (this.GetComponentsInChildren<Text>()[0].text == "山兽")
        {
            for (int i = 0; i < shieldSoldierName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【盾兵】"+"\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000" + shieldSoldierName[i];
            }
            GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text +=  "\t" + "\u2000" + "[刃甲]" + "\u2000" + "3山兽上阵,血量提升10%，反弹20%近战伤害，减少受到10%远程伤害" + "\t" + "\u2000" + "[刺盾]" + "\u2000" + "6山兽上阵,血量提升20%，反弹40%近战伤害，减少受到20%远程伤害";
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "海兽")
        {
            for (int i = 0; i < mahoutName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【象兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "没有没有没有。。。";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000" + mahoutName[i];
            }
            GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\t"+"\u2000" + "[汲取]" + "\u2000" + "3海兽上阵,防御增加10点，将造成伤害的30%转化为自身血量" + "\t" + "\u2000" + "[嗜血]" + "\u2000" + "6海兽上阵,防御增加15点，将造成伤害的60%转化为自身血量";
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "飞兽")
        {
            for (int i = 0; i < halberdierName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【戟兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "没有没有没有。。。";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000" + halberdierName[i];
            }
            GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\t" + "\u2000" + "[灵巧]" + "\u2000" + "3飞兽上阵,血量提升10%，战斗中每损失20%血量，获得风之助，提升10%闪避" + "\t" + "\u2000" + "[瞬闪]" + "\u2000" + "6飞兽上阵,血量提升20%，战斗中每损失20%血量，获得风之助，提升15%闪避";
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "人杰")
        {
            for (int i = 0; i < lifeguardName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【禁卫】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "没有没有没有。。。";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000" + lifeguardName[i];
            }
            GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\t" + "\u2000" + "[血战]" + "\u2000" + "3人杰上阵,攻击提升10%，每次攻击提升20%伤害，10%防御，可叠加3次" + "\t" + "\u2000" + "[死战]" + "\u2000" + "6人杰上阵,攻击提升20%，每次攻击提升30%伤害，10%防御，可叠加3次";
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "祖巫")
        {
            for (int i = 0; i < spearmanName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【枪兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000" + spearmanName[i];
            }
            GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\t" + "\u2000" + "[穿刺]" + "\u2000" + "3祖巫上阵,攻击提升10%，突刺敌方后排50%伤害" + "\t" + "\u2000" + "[突刺]" + "\u2000" + "6祖巫上阵,攻击提升20%，突刺敌方后排80%伤害";
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "散仙")
        {
            for (int i = 0; i < sowarName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【骑兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000" + sowarName[i];
            }
            GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\t" + "\u2000" + "[横扫]" + "\u2000" + "3散仙上阵,攻击提升10%，攻击同一排敌人，每个造成75%伤害" + "\t" + "\u2000" + "[狂斩]" + "\u2000" + "6散仙上阵,攻击提升20%，攻击同一排敌人，每个造成100%伤害";
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "辅神")
        {
            for (int i = 0; i < counsellorName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【军师】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000" + counsellorName[i];
            }
            GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\t" + "\u2000" + "[火计]" + "\u2000" + "3辅神上阵,攻击提升10%，随机攻击2个目标，每个造成30%伤害，20%几率击晕1回合" + "\t" + "\u2000" + "[爆炎]" + "\u2000" + "6辅神上阵,攻击提升20%，随机攻击3个目标，每个造成45%伤害，20%几率击晕1回合";
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "魔神")
        {
            for (int i = 0; i < sapperName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【工兵】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000" + sapperName[i];
            }
            GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\t" + "\u2000" + "[乱射]" + "\u2000" + "3魔神上阵,攻击提升10%，随机攻击3个目标，每个造成45%伤害" + "\t" + "\u2000" + "[箭雨]" + "\u2000" + "6魔神上阵,攻击提升20%，随机攻击4个目标，每个造成50%伤害";
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "天神")
        {
            for (int i = 0; i < necromancerName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【方士】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000" + necromancerName[i];
            }
            GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\t" + "\u2000" + "[地遁]" + "\u2000" + "3天神上阵,攻击提升10%，治疗2个血量最低的友方目标，治疗量为方式伤害的80%" + "\t" + "\u2000" + "[天遁]" + "\u2000" + "6天神上阵,攻击提升20%，治疗3个血量最低的友方目标，治疗量为方式伤害的100%";
        }
        else if (this.GetComponentsInChildren<Text>()[0].text == "神兽")
        {
            for (int i = 0; i < god_beastName.Count; i++)
            {
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = "【神兽】" + "\u2000";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = "防守较强，适合放前排防御";
                GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u2000"+ god_beastName[i] ;
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
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        //{
            //ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            GetSoldiersAll();
            for (int i = 0; i < shieldSoldierId.Count; i++)
            {
                shieldSoldierName.Add(GetHeroNameFromId(shieldSoldierId[i]));
            }
            for (int i = 0; i < mahoutId.Count; i++)
            {
                mahoutName.Add(GetHeroNameFromId(mahoutId[i]));
            }
            for (int i = 0; i < halberdierId.Count; i++)
            {
                halberdierName.Add(GetHeroNameFromId( halberdierId[i]));
            }
            for (int i = 0; i < lifeguardId.Count; i++)
            {
                lifeguardName.Add(GetHeroNameFromId( lifeguardId[i]));
            }
            for (int i = 0; i < spearmanId.Count; i++)
            {
                spearmanName.Add(GetHeroNameFromId(spearmanId[i]));
            }
            for (int i = 0; i < sowarId.Count; i++)
            {
                sowarName.Add(GetHeroNameFromId(sowarId[i]));
            }
            for (int i = 0; i < counsellorId.Count; i++)
            {
                counsellorName.Add(GetHeroNameFromId(counsellorId[i]));
            }
            for (int i = 0; i < sapperId.Count; i++)
            {
                sapperName.Add(GetHeroNameFromId(sapperId[i]));
            }
            for (int i = 0; i < necromancerId.Count; i++)
            {
                necromancerName.Add(GetHeroNameFromId(necromancerId[i]));
            }
            for (int i = 0; i < god_beastId.Count; i++)
            {
                god_beastName.Add(GetHeroNameFromId(god_beastId[i]));
            }
        //}
    }
    void GetSoldiersAll()
    {
        int num = 3;
        for (int i = 0; i < 88; i++)
        {
            if (LoadJsonFile.RoleTableDatas[i][num] == "1")
            {
                shieldSoldierId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "2")
            {
                mahoutId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "3")
            {
                halberdierId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "4")
            {
                lifeguardId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "5")
            {
                spearmanId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "6")
            {
                sowarId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "7")
            {
                counsellorId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "8")
            {
                sapperId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "9")
            {
                necromancerId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "10")
            {
                god_beastId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
        }
    }
    string GetHeroNameFromId(int id)
    {
        string name = "";
        for (int i = 0; i < 88; i++)
        {
            if (int.Parse(LoadJsonFile.RoleTableDatas[i][0]) == id)
            {
                name = LoadJsonFile.RoleTableDatas[i][1];
            }
        }
        return name;
    }
}
