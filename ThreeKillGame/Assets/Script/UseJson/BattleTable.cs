public class BattleTableItem
{
    /// <summary>
    /// 战役ID
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 战役名称
    /// </summary>
    public string battle { get; set; }
    /// <summary>
    /// 战役简介
    /// </summary>
    public string battleIntro { get; set; }
    /// <summary>
    /// 相关势力ID
    /// </summary>
    public string forceId { get; set; }
    /// <summary>
    /// 起始年份
    /// </summary>
    public string startYear { get; set; }
    /// <summary>
    /// 城池坐标
    /// </summary>
    public string cityId { get; set; }
    /// <summary>
    /// 白虎事件坐标
    /// </summary>
    public string adventurePrefab { get; set; }
    /// <summary>
    /// 问答事件坐标
    /// </summary>
    public string testPrefab { get; set; }
    /// <summary>
    /// 剧情战斗事件坐标
    /// </summary>
    public string battlePrefab { get; set; }
}