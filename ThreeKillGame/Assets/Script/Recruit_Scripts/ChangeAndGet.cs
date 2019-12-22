using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAndGet : MonoBehaviour
{
    //public GameObject btn;
    List<int> mycard = new List<int>();
    List<string> getCard = new List<string>();
    int price;  //武将价格
    int heroId; //武将ID:index
    int btnTag;
    bool boolIndex;
    string btnNum;
    public GameObject hero_Card;    //英雄卡片预制件
    public Transform preparation;   //备战位
    public Transform jiugongge;     //上阵位
    List<string> heroData = new List<string>();
    List<int> ChickenRibsHeroId = new List<int>();  //存放所有的拥有的英雄Id

    private CreateAndUpdate createAndUpdate;

    private void Start()
    {
        createAndUpdate = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>();

        CreateStartHero();  //创建加载初始武将
    }

    /// <summary>
    /// 创建加载初始武将
    /// </summary>
    private void CreateStartHero()
    {
        string[] ids = LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][8].Split(',');
        for (int i = 0; i < ids.Length; i++)
        {
            heroId = (int)float.Parse(ids[i]);  //初始武将id获取
            PutCardToPos(preparation.GetChild(i), 1);   //放置到备战位一号位置，一阶
            HeroIdChangeAndSave.pos_heroId[9 + i] = heroId;
        }
    }

    public void GetResidueCard(GameObject btn)
    {
        int num = SearchEmptyEpace(0);
        if (num == -1) { return; }  //若没有空位直接返回
        btnNum = btn.GetComponentInChildren<Text>().text;
        btnTag = int.Parse(btn.name);
        ChickenRibsHeroId = createAndUpdate.ChickenRibsHeroId;
        ChangeBtnColor(btn.transform);
        UpdateGetCard(num);
    }

    private void ChangeBtnColor(Transform tran)
    {
        GetExcelFile1();
        ChickenRibsHeroId.Add(heroId);
        if (CreateAndUpdate.money >= price)
        {
            boolIndex = true;
            tran.GetChild(tran.childCount - 1).gameObject.SetActive(true);  //显示已招图片
            //tran.GetComponent<Button>().enabled = false;   //关闭点击事件
            CreateAndUpdate.money = CreateAndUpdate.money - price;
            createAndUpdate.goldText.text = CreateAndUpdate.money.ToString();
        }
        else
        {
            boolIndex = false;
            //Debug.Log("呸，穷鬼，买不起");
            createAndUpdate.GoldNotEnough(LoadJsonFile.TipsTableDates[3][2]);
        }
        if (boolIndex == true)
        {
            getCard.Add(btnNum);
        }
    }

    //购买英雄
    private void UpdateGetCard(int num)
    {
        if (boolIndex == true)
        {
            if (num == -1) { return; }
            ShowAndGradeHero(num);  //升阶或直接展示
        }
    }

    //展示武将信息
    public void ShowHeroInformation(GameObject btn)
    {
        GameObject topBar = GameObject.Find("TopInformationBar");
        btnNum = btn.GetComponentInChildren<Text>().text;
        btnTag = int.Parse(btn.name);
        GetExcelFile1();
        List<string> HeroDate = LoadJsonFile.DeepClone<string>(LoadJsonFile.RoleTableDatas[heroId - 1]);
        List<List<string>> fetterInformation = new List<List<string>>();
        List<string> heroIdDate = new List<string>();
        string heroKindName = GetHeroTypeName(HeroDate[3]);  //获取兵种名
        topBar.GetComponentInChildren<Text>().text = "";
        int heroId_ = heroId;
        heroIdDate.Add(heroId_.ToString());
        fetterInformation = GameObject.Find("FettrrControl").GetComponent<FetterContronl>().init_One(heroIdDate);
        //显示点击英雄的名字  及  阶数（阶数尚未得到）
        topBar.GetComponentsInChildren<Text>()[1].fontSize = HeroDate[1].Length > 2 ? 50 : 70;
        topBar.GetComponentsInChildren<Text>()[1].text = HeroDate[1] + "\u1500";

        //显示兵种及英雄相关属性
        topBar.GetComponentsInChildren<Text>()[2].text = heroKindName + "\u2000" + "攻击" + HeroDate[6] + "\u2000" + "防御" + HeroDate[7] + "\u2000" + "士兵" + HeroDate[8];
        topBar.GetComponentsInChildren<Text>()[3].text = HeroDate[20];
        //显示羁绊内容
        //if (fetterInformation.Count > 0)
        //{
        //    for (int j = 0; j < fetterInformation.Count; j++)
        //    {
        //        for (int i = 0; i < fetterInformation[j].Count; i++)
        //        {
        //            topBar.GetComponentsInChildren<Text>()[0].text += "[" + fetterInformation[j][1] + "]" + fetterInformation[j][9] + "同时上阵时," + "攻击+" + fetterInformation[j][3] + "%" + "," + "防御+" + fetterInformation[j][4] + "%" + "," + "士兵+" + fetterInformation[j][5] + "%" + "\t";
        //            break;
        //        }
        //    }
        //}
        //else
        //{
        //    topBar.GetComponentInChildren<Text>().text = "\u3000" + "此英雄无羁绊";
        //}
        heroIdDate.Clear();
        fetterInformation.Clear();
        HeroDate.Clear();
    }

    /// <summary>
    /// 获取兵种名
    /// </summary>
    /// <param name="heroKindType">类型编号</param>
    /// <returns></returns>
    private string GetHeroTypeName(string heroKindType)
    {
        int index = int.Parse(heroKindType);
        return LoadJsonFile.SoldierTypeDates[index - 1][1];
    }

    /// <summary>
    /// 购买英雄后，判断是否需要升阶和展示
    /// </summary>
    /// <param name="num"></param>
    private void ShowAndGradeHero(int num)
    {
        int count_pos_heroId = HeroIdChangeAndSave.pos_heroId.Length;   //记录总位置数

        //判断该英雄卡牌数量是否到达升阶要求
        List<int> gradeone = new List<int>();   //一阶单位 父亲位置索引号
        List<int> gradetwo = new List<int>();   //二阶单位
        int index_pos = 0;  //索引位置
        while (index_pos < count_pos_heroId)
        {
            if (HeroIdChangeAndSave.pos_heroId[index_pos]== heroId)
            {
                if (index_pos<9)
                {
                    if (jiugongge.GetChild(index_pos).GetChild(0).GetComponent<HeroDataControll>().Grade_hero == 1)
                    {
                        gradeone.Add(index_pos);    //要销毁的卡牌的父亲位置 0-16
                    }
                    if (jiugongge.GetChild(index_pos).GetChild(0).GetComponent<HeroDataControll>().Grade_hero == 2)
                    {
                        gradetwo.Add(index_pos);
                    }
                }
                else
                {
                    if (preparation.GetChild(index_pos-9).GetChild(0).GetComponent<HeroDataControll>().Grade_hero == 1)
                    {
                        gradeone.Add(index_pos);
                    }
                    if (preparation.GetChild(index_pos-9).GetChild(0).GetComponent<HeroDataControll>().Grade_hero == 2)
                    {
                        gradetwo.Add(index_pos);
                    }
                }
            }
            index_pos++;
        }
        index_pos = 0;  //归零

        if (gradeone.Count >= 2)
        {
            if (gradetwo.Count >= 2)
            {
                //升级三阶
                for (int i = 0; i < 2; i++)
                {
                    HeroIdChangeAndSave.pos_heroId[gradeone[i]] = 0;
                    if (gradeone[i] < 9)  //在九宫格中
                    {
                        Destroy(jiugongge.GetChild(gradeone[i]).GetChild(0).gameObject);   //销毁一阶武将卡牌
                        if (jiugongge.GetChild(gradeone[0]).childCount > 0)
                            Destroy(jiugongge.GetChild(gradeone[0]).GetChild(0).gameObject);   //销毁新生成的二阶武将卡牌
                    }
                    else    //在备战位中
                    {
                        Destroy(preparation.GetChild(gradeone[i] - 9).GetChild(0).gameObject);
                        if (preparation.GetChild(gradeone[0] - 9).childCount > 0)
                            Destroy(jiugongge.GetChild(gradeone[0]).GetChild(0).gameObject);   //销毁新生成的二阶武将卡牌
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    HeroIdChangeAndSave.pos_heroId[gradetwo[i]] = 0;
                    if (gradetwo[i] < 9)
                    {
                        Destroy(jiugongge.GetChild(gradeone[i]).GetChild(0).gameObject);   //销毁二阶武将卡牌
                    }
                    else
                    {
                        Destroy(preparation.GetChild(gradeone[i] - 9).GetChild(0).gameObject);
                    }
                }
                //放置三阶卡牌
                if (gradetwo[0] < 9)
                {
                    PutCardToPos(jiugongge.GetChild(gradetwo[0]), 3);
                }
                else
                {
                    PutCardToPos(preparation.GetChild(gradetwo[0] - 9), 3);
                }
                HeroIdChangeAndSave.pos_heroId[gradetwo[0]] = heroId;
            }
            else
            {
                //升级二阶
                for (int i = 0; i < 2; i++)
                {
                    HeroIdChangeAndSave.pos_heroId[gradeone[i]] = 0;
                    if (gradeone[i] < 9)  //在九宫格中
                    {
                        Destroy(jiugongge.GetChild(gradeone[i]).GetChild(0).gameObject);   //销毁一阶武将卡牌
                    }
                    else    //在备战位中
                    {
                        Destroy(preparation.GetChild(gradeone[i] - 9).GetChild(0).gameObject);
                    }
                }
                //放置二阶卡牌
                if (gradeone[0] < 9)
                {
                    PutCardToPos(jiugongge.GetChild(gradeone[0]), 2);
                }
                else
                {
                    PutCardToPos(preparation.GetChild(gradeone[0] - 9), 2);
                }
                HeroIdChangeAndSave.pos_heroId[gradeone[0]] = heroId;
            }
        }
        else
        {
            //购买展示一阶
            PutCardToPos(preparation.GetChild(num), 1);
            HeroIdChangeAndSave.pos_heroId[num + 9] = heroId;   //记录购买武将的id信息
        }
        index_pos = 0;
        gradeone.Clear();
        gradetwo.Clear();
    }

    /// <summary>
    /// 放置卡牌
    /// </summary>
    /// <param name="card_parant">父亲位置</param>
    private void PutCardToPos(Transform card_parant, int grade)
    {
        //实例化武将卡牌到备战位,并传递数据过去
        GameObject newheroCard = Instantiate(hero_Card, card_parant);
        newheroCard.transform.position = card_parant.position;
        List<string> newData = LoadJsonFile.DeepClone<string>(LoadJsonFile.RoleTableDatas[heroId - 1]);    //list深拷贝
        newheroCard.GetComponent<HeroDataControll>().HeroData = newData;
        //设置品阶颜色表现和属性
        newheroCard.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = grade.ToString();
        newheroCard.GetComponent<HeroDataControll>().Grade_hero = grade;
        newheroCard.GetComponent<HeroDataControll>().BattleNums = 0;
        if (grade == 1)
        {
            newheroCard.GetComponent<HeroDataControll>().Price_hero = price;
        }
        else
        {
            if (grade == 2)
            {
                newheroCard.GetComponent<HeroDataControll>().Price_hero = price * 2;
            }
            else
            {
                newheroCard.GetComponent<HeroDataControll>().Price_hero = price * 6;
            }
        }
        //设置文字颜色，体现卡牌稀有度
        switch (int.Parse(newheroCard.GetComponent<HeroDataControll>().HeroData[4]))
        {
            case 1:
                //newheroCard.transform.GetChild(0).GetComponent<Text>().color = ColorData.green_Color;  //绿色
                newheroCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = ColorData.green_Color;  //绿色
                break;
            case 2:
                //newheroCard.transform.GetChild(0).GetComponent<Text>().color = ColorData.blue_Color_hero; //蓝色
                newheroCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = ColorData.blue_Color_hero; //蓝色
                break;
            case 3:
                //newheroCard.transform.GetChild(0).GetComponent<Text>().color = ColorData.purple_Color; //紫色
                newheroCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = ColorData.purple_Color; //紫色
                break;
            case 4:
                //newheroCard.transform.GetChild(0).GetComponent<Text>().color = ColorData.red_Color_hero;  //红色
                newheroCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = ColorData.red_Color_hero;  //红色
                break;
        }
        newheroCard.GetComponent<HeroDataControll>().HeroData[6] = (Mathf.Pow(2, grade - 1) * int.Parse(newheroCard.GetComponent<HeroDataControll>().HeroData[6])).ToString();
        newheroCard.GetComponent<HeroDataControll>().HeroData[8] = (Mathf.Pow(2, grade - 1) * int.Parse(newheroCard.GetComponent<HeroDataControll>().HeroData[8])).ToString();

        newheroCard.GetComponent<HeroDataControll>().ShowThisHeroData();
    }

    /// <summary>
    /// 查询备战位是否有空位
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private int SearchEmptyEpace(int num)
    {
        int nums = 0;   //记录备战位武将数量
        for (int i = 0; i < preparation.childCount; i++)
        {
            if (preparation.GetChild(i).childCount>0)
                nums++;
        }
        if (nums>=CreateAndUpdate.prepareNum)
        {
            //Debug.Log("备战位已满");
            createAndUpdate.GoldNotEnough(LoadJsonFile.TipsTableDates[5][2]);
            return -1;
        }
        else
        {
            while (preparation.GetChild(num).childCount > 0)
                num++;
        }
        return num;
    }

    //读表
    void GetExcelFile1()
    {
        GetHeroId(btnTag);
        GetSpecificValue(heroId);
    }

    //获取表中英雄的所有数据（暂时弃用）
    private void GetHeroDateFromId(int id)
    {
        heroData.Clear();   //清空上一次所购买的英雄数据
        int heroNums = LoadJsonFile.RoleTableDatas.Count;
        for (int i = 0; i < heroNums; i++)
        {
            if (int.Parse(LoadJsonFile.RoleTableDatas[i][0]) == id)    //通过Id获取当前单元格在第几行
            {
                heroData = LoadJsonFile.DeepClone<string>(LoadJsonFile.RoleTableDatas[i]);
            }
        }
    }

    //获取表中英雄的价格
    private void GetSpecificValue(int id)
    {
        int heroNums = LoadJsonFile.RoleTableDatas.Count;
        for (int i = 0; i < heroNums; i++)
        {
            if (int.Parse(LoadJsonFile.RoleTableDatas[i][0]) == id)    //通过Id获取当前单元格在第几行
            {
                price = int.Parse(LoadJsonFile.RoleTableDatas[i][5]);
            }
        }  
    }

    //在表一中拿到点击英雄的id
    private void GetHeroId(int num)
    {
        heroId = int.Parse(LoadJsonFile.RandowTableDates[num - 1][0]);
    }
}