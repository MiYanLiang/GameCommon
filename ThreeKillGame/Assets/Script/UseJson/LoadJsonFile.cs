﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using LitJson;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadJsonFile : MonoBehaviour
{
    //Resources文件夹下
    public static readonly string Folder = "Jsons/";
    //存放json数据名
    private static readonly string tableNameStrs = "LevelTable;DifficultyChoose;ForcesTable;SoldierSkillTable;RoleTable;RandowTable;FetterTable;PrestigeTable;BattleTable";


    /// <summary>
    /// 经验等级表数据
    /// </summary>
    public static List<List<string>> levelTableDatas;

    /// <summary>
    /// 难度表数据
    /// </summary>
    public static List<List<string>> difficultyChooseDatas;

    /// <summary>
    /// 势力信息表数据
    /// </summary>
    public static List<List<string>> forcesTableDatas;

    /// <summary>
    /// 兵种技能表数据
    /// </summary>
    public static List<List<string>> soldierSkillTableDatas;

    /// <summary>
    /// 武将信息表数据
    /// </summary>
    public static List<List<string>> RoleTableDatas;

    /// <summary>
    /// 武将唯一索引表
    /// </summary>
    public static List<List<string>> RandowTableDates;

    /// <summary>
    /// 羁绊表
    /// </summary>
    public static List<List<string>> FetterTableDates;

    /// <summary>
    /// 结算声望表
    /// </summary>
    public static List<List<string>> PrestigeTableDates;

    /// <summary>
    /// 战役表
    /// </summary>
    public static List<List<string>> BattleTableDates;

    /// <summary>
    /// 加载json文件获取数据至链表中
    /// </summary>
    private void JsonDataToSheets(string[] tableNames)
    {
        //Json数据控制类
        Roots root = new Roots();
        //存放json数据
        string jsonData = string.Empty;
        //记录读取到第几个表
        int indexTable = 0;

        // 加载经验等级表数据:LevelTable
        {
            //读取json文件数据
            jsonData = LoadJsonByName(tableNames[indexTable]);
            //解析数据存放至Root中
            root = JsonMapper.ToObject<Roots>(jsonData);
            //实例化数据存储链表
            levelTableDatas = new List<List<string>>(root.LevelTable.Count);
            for (int i = 0; i < root.LevelTable.Count; i++)
            {
                //依次按属性存值
                levelTableDatas.Add(new List<string>());
                levelTableDatas[i].Add(root.LevelTable[i].index);
                levelTableDatas[i].Add(root.LevelTable[i].level);
                levelTableDatas[i].Add(root.LevelTable[i].experience);
                levelTableDatas[i].Add(root.LevelTable[i].money);
                levelTableDatas[i].Add(root.LevelTable[i].battleNum);
                levelTableDatas[i].Add(root.LevelTable[i].prepareNum);
            }
            Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载难度表数据:DifficultyChoose
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            difficultyChooseDatas = new List<List<string>>(root.DifficultyChoose.Count);
            for (int i = 0; i < root.DifficultyChoose.Count; i++)
            {
                difficultyChooseDatas.Add(new List<string>());
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].id);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].difficulty);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].playerHp);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].playerGold);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].playerMind);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].playerMorale);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].NPCHp);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].green1);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].blue1);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].purple1);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].orange1);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].green2);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].blue2);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].purple2);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].orange2);
            }
            Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载势力信息表数据:ForcesTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            //Debug.Log("///" + jsonData);
            root = JsonMapper.ToObject<Roots>(jsonData);
            forcesTableDatas = new List<List<string>>(root.ForcesTable.Count);
            for (int i = 0; i < root.ForcesTable.Count; i++)
            {
                forcesTableDatas.Add(new List<string>());
                forcesTableDatas[i].Add(root.ForcesTable[i].index);
                forcesTableDatas[i].Add(root.ForcesTable[i].forcesChoose);
                forcesTableDatas[i].Add(root.ForcesTable[i].forceIntro);
                forcesTableDatas[i].Add(root.ForcesTable[i].Prestige);
                forcesTableDatas[i].Add(root.ForcesTable[i].forcesSign);
                forcesTableDatas[i].Add(root.ForcesTable[i].forcesName);
            }
            Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载兵种技能表数据:SoldierSkillTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            soldierSkillTableDatas = new List<List<string>>(root.SoldierSkillTable.Count);
            for (int i = 0; i < root.SoldierSkillTable.Count; i++)
            {
                soldierSkillTableDatas.Add(new List<string>());
                soldierSkillTableDatas[i].Add(root.SoldierSkillTable[i].index);
                soldierSkillTableDatas[i].Add(root.SoldierSkillTable[i].skillName);
                soldierSkillTableDatas[i].Add(root.SoldierSkillTable[i].skillStaticIntro);
                soldierSkillTableDatas[i].Add(root.SoldierSkillTable[i].skillDynamicIntro);
                soldierSkillTableDatas[i].Add(root.SoldierSkillTable[i].corresArms);
            }
            Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载武将信息表数据:RoleTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            RoleTableDatas = new List<List<string>>(root.RoleTable.Count);
            for (int i = 0; i < root.RoleTable.Count; i++)
            {
                RoleTableDatas.Add(new List<string>());
                RoleTableDatas[i].Add(root.RoleTable[i].index);
                RoleTableDatas[i].Add(root.RoleTable[i].roleName);
                RoleTableDatas[i].Add(root.RoleTable[i].force);
                RoleTableDatas[i].Add(root.RoleTable[i].soldierKind);
                RoleTableDatas[i].Add(root.RoleTable[i].rarity);
                RoleTableDatas[i].Add(root.RoleTable[i].recruitingMoney);
                RoleTableDatas[i].Add(root.RoleTable[i].attack);
                RoleTableDatas[i].Add(root.RoleTable[i].defense);
                RoleTableDatas[i].Add(root.RoleTable[i].soldierNum);
                RoleTableDatas[i].Add(root.RoleTable[i].dodgeRate);
                RoleTableDatas[i].Add(root.RoleTable[i].critRate);
                RoleTableDatas[i].Add(root.RoleTable[i].critDamage);
                RoleTableDatas[i].Add(root.RoleTable[i].thumpRate);
                RoleTableDatas[i].Add(root.RoleTable[i].thumpDamage);
                RoleTableDatas[i].Add(root.RoleTable[i].exposeArmor);
                RoleTableDatas[i].Add(root.RoleTable[i].equipmentId);
                RoleTableDatas[i].Add(root.RoleTable[i].allusionId);
                RoleTableDatas[i].Add(root.RoleTable[i].equipmentSkillId);
                RoleTableDatas[i].Add(root.RoleTable[i].soldierSkillId);
                RoleTableDatas[i].Add(root.RoleTable[i].fetterSkillId);
                RoleTableDatas[i].Add(root.RoleTable[i].roleIntro);
            }
            Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载武将唯一索引数据:RandowTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            RandowTableDates = new List<List<string>>(root.RandowTable.Count);
            for (int i = 0; i < root.RandowTable.Count; i++)
            {
                RandowTableDates.Add(new List<string>());
                RandowTableDates[i].Add(root.RandowTable[i].id);
                RandowTableDates[i].Add(root.RandowTable[i].num);
            }
            Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载羁绊数据:FetterTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            //Debug.Log("///"+ jsonData);
            root = JsonMapper.ToObject<Roots>(jsonData);
            FetterTableDates = new List<List<string>>(root.FetterTable.Count);
            for (int i = 0; i < root.FetterTable.Count; i++)
            {
                FetterTableDates.Add(new List<string>());
                FetterTableDates[i].Add(root.FetterTable[i].id);
                FetterTableDates[i].Add(root.FetterTable[i].fetterName);
                FetterTableDates[i].Add(root.FetterTable[i].rolesId);
                FetterTableDates[i].Add(root.FetterTable[i].attackPCT);
                FetterTableDates[i].Add(root.FetterTable[i].defensePCT);
                FetterTableDates[i].Add(root.FetterTable[i].soldierNumPCT);
                FetterTableDates[i].Add(root.FetterTable[i].attack);
                FetterTableDates[i].Add(root.FetterTable[i].defense);
                FetterTableDates[i].Add(root.FetterTable[i].soldierNum);
                FetterTableDates[i].Add(root.FetterTable[i].heroName);
                FetterTableDates[i].Add(root.FetterTable[i].dodgeRate);
                FetterTableDates[i].Add(root.FetterTable[i].critRate);
                FetterTableDates[i].Add(root.FetterTable[i].critDamage);
                FetterTableDates[i].Add(root.FetterTable[i].thumpRate);
                FetterTableDates[i].Add(root.FetterTable[i].thumpDamage);
                FetterTableDates[i].Add(root.FetterTable[i].exposeArmor);
            }
            Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载结算声望表数据:PrestigeTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            PrestigeTableDates = new List<List<string>>(root.PrestigeTable.Count);
            for (int i = 0; i < root.PrestigeTable.Count; i++)
            {
                PrestigeTableDates.Add(new List<string>());
                PrestigeTableDates[i].Add(root.PrestigeTable[i].id);
                PrestigeTableDates[i].Add(root.PrestigeTable[i].degree);
                PrestigeTableDates[i].Add(root.PrestigeTable[i].first);
                PrestigeTableDates[i].Add(root.PrestigeTable[i].second);
                PrestigeTableDates[i].Add(root.PrestigeTable[i].third);
                PrestigeTableDates[i].Add(root.PrestigeTable[i].fail);
            }
            Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载战役表数据:BattleTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            BattleTableDates = new List<List<string>>(root.BattleTable.Count);
            for (int i = 0; i < root.BattleTable.Count; i++)
            {
                BattleTableDates.Add(new List<string>());
                BattleTableDates[i].Add(root.BattleTable[i].id);
                BattleTableDates[i].Add(root.BattleTable[i].battle);
                BattleTableDates[i].Add(root.BattleTable[i].battleIntro);
                BattleTableDates[i].Add(root.BattleTable[i].forceId);
            }
            Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }


        if (indexTable>= tableNames.Length)
            Debug.Log("所有Json数据加载成功。");
        else
            Debug.Log("还有Json数据未进行加载。");
    }

    /// <summary>
    /// 深拷贝List等
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="List">The list.</param>
    /// <returns>List{``0}.</returns>
    public static List<T> DeepClone<T>(object List)
    {
        using (Stream objectStream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, List);
            objectStream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(objectStream) as List<T>;
        }
    }


    private void Awake()
    {
        string[] arrStr = tableNameStrs.Split(';');
        if (arrStr.Length > 0)
            JsonDataToSheets(arrStr);   //传递Json文件名进行加载
        else
            Debug.Log("////请检查Json表名");
    }
    

    /// <summary>
    /// 通过json文件名获取json数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string LoadJsonByName(string fileName)
    {
        string path = string.Empty;
        string data = string.Empty;
        if (Application.isPlaying)
        {
            path = Path.Combine(Folder, fileName);  //合并文件路径
            var asset = Resources.Load<TextAsset>(path);
            Debug.Log("Loading..." + fileName + " from---" + path);
            if (asset == null)
            {
                Debug.LogError("No text asset could be found at resource path: " + path);
                return null;
            }
            data = asset.text;
            Resources.UnloadAsset(asset);
        }
        else
        {
#if UNITY_EDITOR
            path = Application.dataPath + "/Resources/" + Folder + "/" + fileName + ".json";
            Debug.Log("Loading JsonFile " + fileName + " from: " + path);
            var asset1 = System.IO.File.ReadAllText(path);
            data = asset1;
#endif
        }
        return data;
    }

    /// <summary>
    /// 通过StreamReader读取json,json存在StreamingAssets文件夹下
    /// </summary>
    public JsonReader LoadJsonUseStreamReader(string fileName)
    {
        StreamReader streamreader = new StreamReader(Application.dataPath + "/StreamingAssets/Jsons/"+ fileName + ".json");  //读取数据，转换成数据流
        JsonReader js = new JsonReader(streamreader);   //再转换成json数据
        //Root r = JsonMapper.ToObject<Root>(js);     //读取
        //for (int i = 0; i < r.LevelTable.Count; i++)  //遍历获取数据
        //{
        //    textone.text += r.LevelTable[i].experience + "   ";
        //}
        streamreader.Close();
        return js;
    }

    /// <summary>
    /// 通过WWW方法读取Json数据，json存在StreamingAssets文件夹下
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public string LoadJsonUseWWW(string fileName)
    {
        string localPath = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
        {
            //localPath = Application.streamingAssetsPath + "/" + path;
            localPath = "jar:file://" + Application.dataPath + "!/assets/" + fileName + ".json";
        }
        else
        {
            localPath = "file:///" + Application.streamingAssetsPath + "/" + fileName + ".json";
        }
        WWW www = new WWW(localPath);     //格式必须是"ANSI"，不能是"UTF-8"
        if (www.error != null)
        {
            Debug.LogError("error : " + localPath);
            return null;          //读取文件出错
        }
        return www.text;
    }

}