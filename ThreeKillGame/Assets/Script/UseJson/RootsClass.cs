﻿using System.Collections.Generic;

/// <summary>
/// 控制数据表Root
/// </summary>
public class Roots
{
    /// <summary>
    /// 经验等级表
    /// </summary>
    public List<LevelTableItem> LevelTable { get; set; }
    /// <summary>
    /// 难度表
    /// </summary>
    public List<DifficultyChooseItem> DifficultyChoose { get; set; }
    /// <summary>
    /// 势力信息表
    /// </summary>
    public List<ForcesTableItem> ForcesTable { get; set; }
    /// <summary>
    /// 兵种技能表
    /// </summary>
    public List<SoldierSkillTableItem> SoldierSkillTable { get; set; }
    /// <summary>
    /// 武将信息表
    /// </summary>
    public List<RoleTableItem> RoleTable { get; set; }
    /// <summary>
    /// 武将唯一索引表
    /// </summary>
    public List<RandowTableItem> RandowTable { get; set; }
    /// <summary>
    /// 羁绊表
    /// </summary>
    public List<FetterTableItem> FetterTable { get; set; }
    /// <summary>
    /// 战役表
    /// </summary>
    public List<BattleTableItem> BattleTable { get; set; }

    /// <summary>
    /// npc上阵位表
    /// </summary>
    public List<DifficultyItem> DifficultyTable { get; set; }

    /// <summary>
    /// 战鼓开启表
    /// </summary>
    public List<WarDrumTableItem> WarDrumTable { get; set; }

    /// <summary>
    /// 文本信息配置表
    /// </summary>
    public List<TipsTableItem> TipsTable { get; set; }

    /// <summary>
    /// 兵种种类表
    /// </summary>
    public List<SoldierTypeItem> SoldierType { get; set; }

    /// <summary>
    /// NPC和boss数据表
    /// </summary>
    public List<NPCTableItem> NPCTable { get; set; }

    /// <summary>
    /// Flags表
    /// </summary>
    public List<FlagsTableItem> flagsTable { get; set; }

    /// <summary>
    /// 大事件数据表
    /// </summary>
    public List<EventTableItem> EventTable { get; set; }
    
    /// <summary>
    /// 答题玩法表
    /// </summary>
    public List<TestTableItem> TestTable { get; set; }

    /// <summary>
    /// 鸡肋故事数据表
    /// </summary>
    public List<StoryATableItem> StoryATable { get; set; }

    /// <summary>
    /// 鸡肋事件影响数值表
    /// </summary>
    public List<EndingATableItem> EndingATable { get; set; }
}