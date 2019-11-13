public class FightState
{
    /// <summary>
    /// 是否有特殊状态
    /// </summary>
    public bool SetHadSpState()
    {
        if (isDizzy || isBatter || isWithStand || isFireAttack)
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

}