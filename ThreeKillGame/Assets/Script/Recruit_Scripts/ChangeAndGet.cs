using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAndGet : MonoBehaviour
{
    public GameObject btn;
    List<int> mycard = new List<int>();
    List<string> getCard = new List<string>();
    int price;  //武将价格
    int heroId; //武将ID:index
    int btnTag;
    int money;
    bool boolIndex;
    string btnNum;
    public GameObject hero_Card;    //英雄卡片预制件
    public Transform preparation;   //备战位
    public Transform jiugongge;     //上阵位
    List<string> heroData = new List<string>();
    List<int> ChickenRibsHeroId = new List<int>();  //存放所有的拥有的英雄Id
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void GetResidueCard()
    {
        if (SearchEmptyEpace(0) == -1) { return; }  //若没有空位直接返回
        btnNum = btn.GetComponentInChildren<Text>().text;
        btnTag = int.Parse(btn.name);
        print("btnName:"+btnTag);
        //mycard = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().myCard;//拿到脚本CreateAndUpdate中的myCard
        money = CreateAndUpdate.money;
        ChickenRibsHeroId = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().ChickenRibsHeroId;
    }

    public void ChangeBtnColor()
    {
        if (SearchEmptyEpace(0) == -1) { return; }  //若没有空位直接返回
        GetExcelFile1();
        ChickenRibsHeroId.Add(heroId);
        if (money >= price)
        {
            boolIndex = true;
            transform.Find("Image").gameObject.SetActive(true);
            transform.GetComponent<Button>().enabled = false;   //关闭点击事件
            //print(btnTag);
            money = money - price;
            CreateAndUpdate.money = money;
        }
        else
        {
            boolIndex = false;
            Debug.Log("呸，穷鬼，买不起");
        }
        if (boolIndex == true)
        {
            //GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().getCard.Add(btnNum);
            getCard.Add(btnNum);
        }
    }
    

    //购买英雄
    public void UpdateGetCard()
    {
        //getCard = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().getCard;
        if (boolIndex == true)
        {
            int num = 0;
            num = SearchEmptyEpace(num);
            if (num == -1) { return; }
            
            GetExcelFile2();
            print("heroId:" + heroId);
            Debug.Log("heroData,name//" + heroData[1]);
            ShowAndGradeHero(num);  //升阶或直接展示
        }
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
                        if (jiugongge.GetChild(gradeone[0]).childCount>0)
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
                    if (gradeone[i]<9)  //在九宫格中
                    {
                        Destroy(jiugongge.GetChild(gradeone[i]).GetChild(0).gameObject);   //销毁一阶武将卡牌
                    }
                    else    //在备战位中
                    {
                        Destroy(preparation.GetChild(gradeone[i]-9).GetChild(0).gameObject);
                    }
                }
                //放置二阶卡牌
                if (gradeone[0]<9)
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
            PutCardToPos(preparation.GetChild(num),1);
            //Debug.Log("");
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
        List<string> newData = LoadJsonFile.DeepClone<string>(heroData);    //list深拷贝
        newheroCard.GetComponent<HeroDataControll>().HeroData = newData;
        //设置品阶颜色表现和属性
        newheroCard.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = grade.ToString();
        newheroCard.GetComponent<HeroDataControll>().Grade_hero = grade;
        newheroCard.GetComponent<HeroDataControll>().BattleNums = 0;
        if (grade==1)
        {
            newheroCard.GetComponent<HeroDataControll>().Price_hero = price;
        }
        else
        {
            if (grade==2)
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
                newheroCard.transform.GetChild(0).GetComponent<Text>().color = new Color(49f / 255f, 193f / 255f, 82f / 255f, 1);  //绿色
                break;
            case 2:
                newheroCard.transform.GetChild(0).GetComponent<Text>().color = new Color(48f / 255f, 127f / 255f, 192f / 255f, 1); //蓝色
                break;
            case 3:
                newheroCard.transform.GetChild(0).GetComponent<Text>().color = new Color(215f / 255f, 37f / 255f, 236f / 255f, 1); //紫色
                break;
            case 4:
                newheroCard.transform.GetChild(0).GetComponent<Text>().color = new Color(227f / 255f, 16f / 255f, 16f / 255f, 1);  //红色
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
            Debug.Log("备战位已满");
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
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        //{
        //    ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
        //    ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            GetHeroId(btnTag);
            GetSpecificValue(heroId);
            //ChickenRibsHeroId.Add(heroId);
            //print(price);
        //}
    }
    //读取相应英雄的所有数据
    void GetExcelFile2()
    {
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        //{
            //ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            GetHeroDateFromId(heroId);
            //print(price);
        //}
    }

    //获取表中英雄的所有数据
    void GetHeroDateFromId(int id)
    {
        heroData.Clear();   //清空上一次所购买的英雄数据
        for (int i = 0; i < 88; i++)
        {
            for (int j = 0; j < 21; j++)
            {

                if (int.Parse(LoadJsonFile.RoleTableDatas[i][0]) == id)    //通过Id获取当前单元格在第几行
                {
                    heroData.Add(LoadJsonFile.RoleTableDatas[i][j]);
                }
            }
        }
    }

    //获取表中英雄的价格
    void GetSpecificValue(int id)
    {
        for (int i = 0; i < 88; i++)
        {
            if (int.Parse(LoadJsonFile.RoleTableDatas[i][0]) == id)    //通过Id获取当前单元格在第几行
            {
                price = int.Parse(LoadJsonFile.RoleTableDatas[i][5]);
            }
        }  
    }
    
    //在表一中拿到点击英雄的id
    void GetHeroId(int num)
    {
        for (int i = 0; i < 792; i++)
        {
            if (LoadJsonFile.RandowTableDates[i][1] == num.ToString())
            {
                heroId = int.Parse(LoadJsonFile.RandowTableDates[i][0]);
            }
        }
    }
}
