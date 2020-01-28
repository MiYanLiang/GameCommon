public class StoryATableItem
{
    /// <summary>
    /// 战役ID
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 权重
    /// </summary>
    public string weightValue { get; set; }
    /// <summary>
    /// 故事名
    /// </summary>
    public string story { get; set; }
    /// <summary>
    /// 故事介绍
    /// </summary>
    public string storyBody { get; set; }
    /// <summary>
    /// 跳过
    /// </summary>
    public string exit { get; set; }
    /// <summary>
    /// 选项1
    /// </summary>
    public string option1 { get; set; }
    /// <summary>
    /// 选项2
    /// </summary>
    public string option2 { get; set; }
    /// <summary>
    /// 选项1指向
    /// </summary>
    public string option1ToEnding { get; set; }
    /// <summary>
    /// 选项2指向
    /// </summary>
    public string option2ToEnding { get; set; }
}
