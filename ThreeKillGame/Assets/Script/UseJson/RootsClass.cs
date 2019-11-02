using System.Collections.Generic;

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
    /// 结算声望表
    /// </summary>
    public List<PrestigeTableItem> PrestigeTable { get; set; }
    /// <summary>
    /// 战役表
    /// </summary>
    public List<BattleTableItem> BattleTable { get; set; }
}



