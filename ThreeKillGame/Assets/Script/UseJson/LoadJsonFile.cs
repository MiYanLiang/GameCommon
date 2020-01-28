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
    private static readonly string tableNameStrs = "LevelTable;DifficultyChoose;ForcesTable;SoldierSkillTable;RoleTable;RandowTable;FetterTable;BattleTable;DifficultyTable;WarDrumTable;TipsTable;SoldierType;NPCTable;flagsTable;EventTable;TestTable;StoryATable;EndingATable";


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
    /// 战役表
    /// </summary>
    public static List<List<string>> BattleTableDates;

    /// <summary>
    /// npc上阵位开启表
    /// </summary>
    public static List<List<string>> DifficultyTableDates;

    /// <summary>
    /// 战鼓开启等级表
    /// </summary>
    public static List<List<string>> WarDrumTableDates;

    /// <summary>
    /// 文本信息配置表
    /// </summary>
    public static List<List<string>> TipsTableDates;

    /// <summary>
    /// 兵种信息表
    /// </summary>
    public static List<List<string>> SoldierTypeDates;

    /// <summary>
    /// NPC和boss数据表
    /// </summary>
    public static List<List<string>> NPCTableDates;

    /// <summary>
    /// Flags数据表
    /// </summary>
    public static List<List<string>> FlagsTableDates;

    /// <summary>
    /// Event大事件数据表
    /// </summary>
    public static List<List<string>> EventTableDates;

    /// <summary>
    /// Test答题玩法数据表
    /// </summary>
    public static List<List<string>> TestTableDates;

    /// <summary>
    /// 鸡肋故事数据表
    /// </summary>
    public static List<List<string>> StoryATableDates;

    /// <summary>
    /// 鸡肋事件影响数值表
    /// </summary>
    public static List<List<string>> EndingATableDates;


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
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
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
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].prestigeReward1);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].prestigeReward2);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].prestigeReward3);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].prestigeReward0);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].jiesuanGold);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].jiesuanHp);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].jiesuanChengfang);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].pyZhanyiAdd);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].pyShiqiAdd);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].pyChengfangAdd);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].pyMinxinAdd);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].npcZhanyiAdd);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].npcShiqiAdd);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].npcChengfangAdd);
                difficultyChooseDatas[i].Add(root.DifficultyChoose[i].npcMinXinAdd);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
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
                forcesTableDatas[i].Add(root.ForcesTable[i].cityName);
                forcesTableDatas[i].Add(root.ForcesTable[i].city);
                forcesTableDatas[i].Add(root.ForcesTable[i].cityIcon);
                forcesTableDatas[i].Add(root.ForcesTable[i].firstHeroId);
                forcesTableDatas[i].Add(root.ForcesTable[i].flagId);
                forcesTableDatas[i].Add(root.ForcesTable[i].exclusiveSoldierId);
                forcesTableDatas[i].Add(root.ForcesTable[i].zhanYi);
                forcesTableDatas[i].Add(root.ForcesTable[i].shiQi);
                forcesTableDatas[i].Add(root.ForcesTable[i].chengFang);
                forcesTableDatas[i].Add(root.ForcesTable[i].minXin);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
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
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
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
                RoleTableDatas[i].Add(root.RoleTable[i].isHero);
                RoleTableDatas[i].Add(root.RoleTable[i].roleIcon);
                RoleTableDatas[i].Add(root.RoleTable[i].addHpPercent);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
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
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
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
                FetterTableDates[i].Add(root.FetterTable[i].heroName);
                FetterTableDates[i].Add(root.FetterTable[i].attack);
                FetterTableDates[i].Add(root.FetterTable[i].defense);
                FetterTableDates[i].Add(root.FetterTable[i].soldierNum);
                FetterTableDates[i].Add(root.FetterTable[i].dodgeRate);
                FetterTableDates[i].Add(root.FetterTable[i].critRate);
                FetterTableDates[i].Add(root.FetterTable[i].critDamage);
                FetterTableDates[i].Add(root.FetterTable[i].thumpRate);
                FetterTableDates[i].Add(root.FetterTable[i].thumpDamage);
                FetterTableDates[i].Add(root.FetterTable[i].exposeArmor);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
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
                BattleTableDates[i].Add(root.BattleTable[i].startYear);
                BattleTableDates[i].Add(root.BattleTable[i].cityId);
                BattleTableDates[i].Add(root.BattleTable[i].adventurePrefab);
                BattleTableDates[i].Add(root.BattleTable[i].testPrefab);
                BattleTableDates[i].Add(root.BattleTable[i].battlePrefab);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
        }
        //加载npc上阵位开启表数据:DifficultyTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            DifficultyTableDates = new List<List<string>>(root.DifficultyTable.Count);
            for (int i = 0; i < root.DifficultyTable.Count; i++)
            {
                DifficultyTableDates.Add(new List<string>());
                DifficultyTableDates[i].Add(root.DifficultyTable[i].id);
                DifficultyTableDates[i].Add(root.DifficultyTable[i].enemyTotal);
                DifficultyTableDates[i].Add(root.DifficultyTable[i].openWeek1);
                DifficultyTableDates[i].Add(root.DifficultyTable[i].openWeek2);
                DifficultyTableDates[i].Add(root.DifficultyTable[i].openWeek3);
                DifficultyTableDates[i].Add(root.DifficultyTable[i].openWeek4);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
        }
        //战鼓开启等级表数据:WarDrumTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            WarDrumTableDates = new List<List<string>>(root.WarDrumTable.Count);
            for (int i = 0; i < root.WarDrumTable.Count; i++)
            {
                WarDrumTableDates.Add(new List<string>());
                WarDrumTableDates[i].Add(root.WarDrumTable[i].id);
                WarDrumTableDates[i].Add(root.WarDrumTable[i].warDrumName);
                WarDrumTableDates[i].Add(root.WarDrumTable[i].unlockLevel);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
        }
        //加载文本信息表数据:TipsTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            TipsTableDates = new List<List<string>>(root.TipsTable.Count);
            for (int i = 0; i < root.TipsTable.Count; i++)
            {
                TipsTableDates.Add(new List<string>());
                TipsTableDates[i].Add(root.TipsTable[i].id);
                TipsTableDates[i].Add(root.TipsTable[i].type);
                TipsTableDates[i].Add(root.TipsTable[i].text);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
        }
        //加载兵种信息表数据:SoldierType
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            SoldierTypeDates = new List<List<string>>(root.SoldierType.Count);
            for (int i = 0; i < root.SoldierType.Count; i++)
            {
                SoldierTypeDates.Add(new List<string>());
                SoldierTypeDates[i].Add(root.SoldierType[i].id);
                SoldierTypeDates[i].Add(root.SoldierType[i].soldierType);
                SoldierTypeDates[i].Add(root.SoldierType[i].logo);
                SoldierTypeDates[i].Add(root.SoldierType[i].soldierIntroduce);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
        }
        //加载NPC和boss数据表数据:NPCTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            NPCTableDates = new List<List<string>>(root.NPCTable.Count);
            for (int i = 0; i < root.NPCTable.Count; i++)
            {
                NPCTableDates.Add(new List<string>());
                NPCTableDates[i].Add(root.NPCTable[i].enemyId);
                NPCTableDates[i].Add(root.NPCTable[i].type);
                NPCTableDates[i].Add(root.NPCTable[i].enemyName);
                NPCTableDates[i].Add(root.NPCTable[i].beginning);
                NPCTableDates[i].Add(root.NPCTable[i].ending);
                NPCTableDates[i].Add(root.NPCTable[i].weeks);
                NPCTableDates[i].Add(root.NPCTable[i].seat1);
                NPCTableDates[i].Add(root.NPCTable[i].seat2);
                NPCTableDates[i].Add(root.NPCTable[i].seat3);
                NPCTableDates[i].Add(root.NPCTable[i].seat4);
                NPCTableDates[i].Add(root.NPCTable[i].seat5);
                NPCTableDates[i].Add(root.NPCTable[i].seat6);
                NPCTableDates[i].Add(root.NPCTable[i].seat7);
                NPCTableDates[i].Add(root.NPCTable[i].seat8);
                NPCTableDates[i].Add(root.NPCTable[i].seat9);
                NPCTableDates[i].Add(root.NPCTable[i].intro);
                NPCTableDates[i].Add(root.NPCTable[i].forceId);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable] + ".Json");
        }
        //加载FlagsTable数据:FlagsTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            FlagsTableDates = new List<List<string>>(root.flagsTable.Count);
            for (int i = 0; i < root.flagsTable.Count; i++)
            {
                FlagsTableDates.Add(new List<string>());
                FlagsTableDates[i].Add(root.flagsTable[i].flagId);
                FlagsTableDates[i].Add(root.flagsTable[i].flagIcon);
                FlagsTableDates[i].Add(root.flagsTable[i].flagEffect);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载EventTable数据:EventTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            EventTableDates = new List<List<string>>(root.EventTable.Count);
            for (int i = 0; i < root.EventTable.Count; i++)
            {
                EventTableDates.Add(new List<string>());
                EventTableDates[i].Add(root.EventTable[i].id);
                EventTableDates[i].Add(root.EventTable[i].type);
                EventTableDates[i].Add(root.EventTable[i].playerWV);
                EventTableDates[i].Add(root.EventTable[i].NPCWV);
                EventTableDates[i].Add(root.EventTable[i].duration);
                EventTableDates[i].Add(root.EventTable[i].zhanYi);
                EventTableDates[i].Add(root.EventTable[i].shiQi);
                EventTableDates[i].Add(root.EventTable[i].chengFang);
                EventTableDates[i].Add(root.EventTable[i].minXin);
                EventTableDates[i].Add(root.EventTable[i].introduction);
                EventTableDates[i].Add(root.EventTable[i].effectId);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载TestTable数据:TestTable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            TestTableDates = new List<List<string>>(root.TestTable.Count);
            for (int i=0;i<root.TestTable.Count;i++)
            {
                TestTableDates.Add(new List<string>());
                TestTableDates[i].Add(root.TestTable[i].id);
                TestTableDates[i].Add(root.TestTable[i].truth);
                TestTableDates[i].Add(root.TestTable[i].question);
                TestTableDates[i].Add(root.TestTable[i].answer1);
                TestTableDates[i].Add(root.TestTable[i].answer2);
                TestTableDates[i].Add(root.TestTable[i].answer3);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载StoryATable数据:StoryATable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            StoryATableDates = new List<List<string>>(root.StoryATable.Count);
            for (int i = 0; i < root.StoryATable.Count; i++)
            {
                StoryATableDates.Add(new List<string>());
                StoryATableDates[i].Add(root.StoryATable[i].id);
                StoryATableDates[i].Add(root.StoryATable[i].weightValue);
                StoryATableDates[i].Add(root.StoryATable[i].story);
                StoryATableDates[i].Add(root.StoryATable[i].storyBody);
                StoryATableDates[i].Add(root.StoryATable[i].exit);
                StoryATableDates[i].Add(root.StoryATable[i].option1);
                StoryATableDates[i].Add(root.StoryATable[i].option2);
                StoryATableDates[i].Add(root.StoryATable[i].option1ToEnding);
                StoryATableDates[i].Add(root.StoryATable[i].option2ToEnding);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }
        //加载EndingATable数据:EndingATable
        {
            jsonData = LoadJsonByName(tableNames[indexTable]);
            root = JsonMapper.ToObject<Roots>(jsonData);
            EndingATableDates = new List<List<string>>(root.EndingATable.Count);
            for (int i = 0; i < root.EndingATable.Count; i++)
            {
                EndingATableDates.Add(new List<string>());
                EndingATableDates[i].Add(root.EndingATable[i].id);
                EndingATableDates[i].Add(root.EndingATable[i].ending);
                EndingATableDates[i].Add(root.EndingATable[i].gold);
                EndingATableDates[i].Add(root.EndingATable[i].shiQi);
                EndingATableDates[i].Add(root.EndingATable[i].junLiang);
                EndingATableDates[i].Add(root.EndingATable[i].chengFang);
                EndingATableDates[i].Add(root.EndingATable[i].minXin);
            }
            indexTable++;
            //Debug.Log("Json文件加载成功---" + tableNames[indexTable++] + ".Json");
        }


        if (indexTable >= tableNames.Length)
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
        DontDestroyOnLoad(gameObject);//跳转场景等不销毁
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
            Debug.Log("Loading..." + fileName + "\nFrom:" + path);
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
        StreamReader streamreader = new StreamReader(Application.dataPath + "/StreamingAssets/Jsons/" + fileName + ".json");  //读取数据，转换成数据流
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