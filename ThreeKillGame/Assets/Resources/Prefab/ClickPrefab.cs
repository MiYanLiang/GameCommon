using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickPrefab : MonoBehaviour
{
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

    public void ClickSoldiersPrefab()
    {
        List<List<string>> arrs_name = new List<List<string>>(LoadJsonFile.SoldierTypeDates.Count);
        shieldSoldierName.Clear();
        mahoutName.Clear();
        halberdierName.Clear();
        lifeguardName.Clear();
        spearmanName.Clear();
        sowarName.Clear();
        counsellorName.Clear();
        sapperName.Clear();
        necromancerName.Clear();

        arrs_name.Add(shieldSoldierName);
        arrs_name.Add(mahoutName);
        arrs_name.Add(halberdierName);
        arrs_name.Add(lifeguardName);
        arrs_name.Add(spearmanName);
        arrs_name.Add(sowarName);
        arrs_name.Add(counsellorName);
        arrs_name.Add(sapperName);
        arrs_name.Add(necromancerName);

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
        GameObject topBar = GameObject.Find("TopInformationBar");
        topBar.GetComponentInChildren<Text>().text = "";
        init_btn();

        for (int i = 0; i < LoadJsonFile.SoldierTypeDates.Count; i++)
        {
            if (this.GetComponentsInChildren<Text>()[0].text == LoadJsonFile.SoldierTypeDates[i][1])
            {
                for (int j = 0; j < arrs_name[i].Count; j++)
                {
                    topBar.GetComponentsInChildren<Text>()[0].text += "\u2000" + arrs_name[i][j];
                    topBar.GetComponentsInChildren<Text>()[1].text = LoadJsonFile.SoldierTypeDates[i][1] + "\u2000";
                    topBar.GetComponentsInChildren<Text>()[2].text = LoadJsonFile.SoldierTypeDates[i][3];
                    topBar.GetComponentsInChildren<Text>()[3].text = "";
                }
                topBar.GetComponentsInChildren<Text>()[0].text += "\t" + "\u2000" + "3兵技" + "\u2000" + "[" + LoadJsonFile.soldierSkillTableDatas[0 + i * 2][1] + "]" + "\u2000" + LoadJsonFile.soldierSkillTableDatas[0 + i * 2][2] + LoadJsonFile.soldierSkillTableDatas[0 + i * 2][3]
                                                                + "\t" + "\u2000" + "6兵技" + "\u2000" + "[" + LoadJsonFile.soldierSkillTableDatas[1 + i * 2][1] + "]" + "\u2000" + LoadJsonFile.soldierSkillTableDatas[1 + i * 2][2] + LoadJsonFile.soldierSkillTableDatas[1 + i * 2][3];
            }
        }
    }

    void init_btn()
    {
        GetExcelFile3();
    }

    void GetExcelFile3()
    {
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
            halberdierName.Add(GetHeroNameFromId(halberdierId[i]));
        }
        for (int i = 0; i < lifeguardId.Count; i++)
        {
            lifeguardName.Add(GetHeroNameFromId(lifeguardId[i]));
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
    }

    void GetSoldiersAll()
    {
        int num = 3;
        int heroNums = LoadJsonFile.RoleTableDatas.Count;
        for (int i = 0; i < heroNums; i++)
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
        }
    }

    private string GetHeroNameFromId(int id)
    {
        return LoadJsonFile.RoleTableDatas[id - 1][1];
    }
}