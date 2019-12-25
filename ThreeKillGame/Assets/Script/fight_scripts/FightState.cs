using UnityEngine;

/// <summary>
/// 状态
/// </summary>
public class FightState
{
    /// <summary>
    /// 是否有特殊状态
    /// </summary>
    public bool GetHadSpState()
    {
        if (isDizzy)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 是否处于眩晕状态 -- 状态栏显示眩晕
    /// </summary>
    public bool isDizzy { get; set; }

    /// <summary>
    /// 是否处于连击状态 -- 状态栏显示战鼓风（风）
    /// </summary>
    public bool isBatter { get; set; }
    /// <summary>
    /// 连击次数
    /// </summary>
    public int batterNums { get; set; }

    /// <summary>
    /// 是否处于抵挡状态 -- 状态栏显示战鼓土（坚盾）
    /// </summary>
    public bool isWithStand { get; set; }
    /// <summary>
    /// 坚盾层数
    /// </summary>
    public int withStandNums { get; set; }

    /// <summary>
    /// 是否处于火攻状态 -- 状态栏显示战鼓火（火苗）
    /// </summary>
    public bool isFireAttack { get; set; }

    /// <summary>
    /// 是否处于战意状态 -- 状态栏显示战意（人杰兵种技能）
    /// </summary>
    public bool isFightMean { get; set; }

    /// <summary>
    /// 是否处于风遁状态 -- 状态栏显示飞兽（飞兽兵种技能）
    /// </summary>
    public bool isPopular { get; set; }
}

/// <summary>
/// 状态名
/// </summary>
public static class StateName
{
    /// <summary>
    /// 眩晕
    /// </summary>
    public static readonly string dizzyName = "state_dizzy";
    
    /// <summary>
    /// 眩晕特效
    /// </summary>
    public static readonly string xuanyunEffect = "xy_Effect";

    /// <summary>
    /// 连击
    /// </summary>
    public static readonly string batterName = "state_batter";

    /// <summary>
    /// 坚盾
    /// </summary>
    public static readonly string standName = "state_stand";

    /// <summary>
    /// 火攻
    /// </summary>
    public static readonly string fireAttackName = "state_zibao";

    /// <summary>
    /// 火攻群攻
    /// </summary>
    public static readonly string fireAllAttackName = "state_fireAttack";

    /// <summary>
    /// 战意（人杰兵种技能）
    /// </summary>
    public static readonly string fightMeanName = "state_fightMean";

    /// <summary>
    /// 风遁（飞兽兵种技能）
    /// </summary>
    public static readonly string popularName = "state_popular";
}

/// <summary>
/// 颜色数据类
/// </summary>
public static class ColorData
{
    /// <summary>
    /// 亮红色
    /// </summary>
    public static readonly Color red_Color = new Color(255f / 255f, 0f / 255f, 0f / 255f, 1);

    /// <summary>
    /// 蓝色
    /// </summary>
    public static readonly Color blue_Color = new Color(0f / 255f, 125f / 255f, 255f / 255f, 1);

    /// <summary>
    /// 蓝色（武将稀有度）
    /// </summary>
    public static readonly Color blue_Color_hero = new Color(48f / 255f, 127f / 255f, 192f / 255f, 1);

    /// <summary>
    /// 绿色（武将稀有度）
    /// </summary>
    public static readonly Color green_Color = new Color(49f / 255f, 193f / 255f, 82f / 255f, 1);

    /// <summary>
    /// 绿色（深色）
    /// </summary>
    public static readonly Color green_deep_Color = new Color(29f / 255f, 156f / 255f, 73f / 255f, 1);

    /// <summary>
    /// 紫色（武将稀有度）
    /// </summary>
    public static readonly Color purple_Color = new Color(215f / 255f, 37f / 255f, 236f / 255f, 1);

    /// <summary>
    /// 红色（武将稀有度）
    /// </summary>
    public static readonly Color red_Color_hero = new Color(227f / 255f, 16f / 255f, 16f / 255f, 1);

}