public class FetterTableItem
{
    /// <summary>
    /// 羁绊id
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 羁绊名称
    /// </summary>
    public string fetterName { get; set; }
    /// <summary>
    /// 所需武将id
    /// </summary>
    public string rolesId { get; set; }
    /// <summary>
    /// 攻击加成
    /// </summary>
    public string attackPCT { get; set; }
    /// <summary>
    /// 防御加成
    /// </summary>
    public string defensePCT { get; set; }
    /// <summary>
    /// 血量加成
    /// </summary>
    public string soldierNumPCT { get; set; }
    /// <summary>
    /// 攻击
    /// </summary>
    public string attack { get; set; }
    /// <summary>
    /// 防御
    /// </summary>
    public string defense { get; set; }
    /// <summary>
    /// 血量
    /// </summary>
    public string soldierNum { get; set; }
    /// <summary>
    /// 羁绊所需英雄名称
    /// </summary>
    public string heroName { get; set; }
    /// <summary>
    /// 闪避率
    /// </summary>
    public string dodgeRate { get; set; }
    /// <summary>
    /// 暴击率
    /// </summary>
    public string critRate { get; set; }
    /// <summary>
    /// 暴击伤害
    /// </summary>
    public string critDamage { get; set; }
    /// <summary>
    /// 重击率
    /// </summary>
    public string thumpRate { get; set; }
    /// <summary>
    /// 重击伤害
    /// </summary>
    public string thumpDamage { get; set; }
    /// <summary>
    /// 破甲
    /// </summary>
    public string exposeArmor { get; set; }
}