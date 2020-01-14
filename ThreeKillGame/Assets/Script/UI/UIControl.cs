using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public static bool isShowCutHpText;


    [SerializeField]
    Transform forcesParentTran; //势力城市父节点
    [SerializeField]
    Image playerForce_main;     //玩家势力头像主城
    [SerializeField]
    public Transform playerForce;      //玩家势力头像
    [HideInInspector]
    public static int playerForceId;  //玩家势力ID

    [SerializeField]
    GameObject npcForceCity;    //npc城市预制件

    public static List<int> enemy_forces;   //战役中其他势力id
    private List<GameObject> npcForcesObjs; //npc城市集合

    [SerializeField]
    Button fightBtn;    //战斗按钮

    public GameObject cameraAudio;//摄像机

    private int battleId;   //开局所选战役ID
    [SerializeField]
    public Text battle_Text;

    [SerializeField]
    Transform cityPos;  //城市位置

    [SerializeField]
    Image fightOrDefend; //进攻或防守图片

    [SerializeField]
    Transform selectForceIamge; //选择势力icon

    [SerializeField]
    GameObject topPower;    //大地图顶部展示势力信息

    [SerializeField]
    FightControll fightControll;

    [SerializeField]
    GameObject jiuGongGeObj_npc;   //npc所用九宫格

    [SerializeField]
    Transform npc_ShangZhenWei; //npc上阵位父节点

    [SerializeField]
    Transform endGameInfoList;  //游戏结束结算信息父控件

    [SerializeField]
    GameObject informationObj;  //游戏结束结算信息Textobj

    [SerializeField]
    Transform cityAttObj;   //玩家城池属性tran

    [SerializeField]
    Transform cityAttObj_world;   //大世界城池属性tran

    [HideInInspector]
    public List<int> cityAttValuePy;   //玩家城池属性值0士气1军粮2城防3民心

    [HideInInspector]
    public List<List<int>> cityAttValueNpc;    //NPC城池属性值

    private AudioSource backAudioSource;

    private int difficultNum;   //难度值记录

    public static int yearsIndex;   //记录周目数

    [HideInInspector]
    public int[] bigEvent = new int[6];     //大事件记录 0事件1周目2 - 5加成值

    [HideInInspector]
    public List<int[]> bigEventNpc;    //NPC大事件记录 0事件1周目2 - 5加成值

    private int npc_Count = 0;   //记录npc数量

    [SerializeField]
    Text bigEventInfoText;  //大事件描述文本

    [SerializeField]
    GameObject[] bigEvent_eftObj;   //0黄龙1闪电

    private void Awake()
    {
        yearsIndex = 0;

        isShowCutHpText = false;

        enemy_forces = new List<int>();

        battleId = PlayerPrefs.GetInt("battleId");

        Invoke("OpenPlayFightBtn", 0.5f);

        //加载声音
        int soundStades = PlayerPrefs.GetInt("soundStates");
        soundContrll_(soundStades);

        backAudioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        cityAttValuePy = new List<int>();
        cityAttValueNpc = new List<List<int>>();
        difficultNum = CreateAndUpdate.difficultNum;
        InitSelectForceIndex();
        battle_Text.text = "公元" + LoadJsonFile.BattleTableDates[battleId][4] + "年";
        initForces();   //初始化势力
        initEvent();   //初始化大事件
        gameObject.GetComponent<BookOfAnswerQ>().InfoQustionOfBook();   //答题初始化
    }

    /// <summary>
    /// 初始化势力表现
    /// </summary>
    private void initForces()
    {
        string[] str_Force = LoadJsonFile.BattleTableDates[battleId][3].Split(',');
        npc_Count = str_Force.Length;

        //玩家势力初始化
        playerForceId = PlayerPrefs.GetInt("forcesId");
        playerForce_main.sprite = Resources.Load("Image/calligraphy/Forces/" + LoadJsonFile.forcesTableDatas[playerForceId - 1][4], typeof(Sprite)) as Sprite; //设置玩家势力的头像
        playerForce_main.transform.parent.GetChild(5).GetComponent<Text>().text = LoadJsonFile.forcesTableDatas[playerForceId - 1][5];  //主城城市名显示
        playerForce_main.transform.parent.GetChild(6).GetComponent<Text>().text = "0";  //主城战力显示
        playerForce.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/Forces/" + LoadJsonFile.forcesTableDatas[playerForceId - 1][4], typeof(Sprite)) as Sprite; //设置玩家势力的头像
        playerForce.GetComponent<Image>().sprite = Resources.Load("Image/Map/" + LoadJsonFile.forcesTableDatas[playerForceId - 1][7], typeof(Sprite)) as Sprite;   //设置城池icon
        for (int i = 0; i < 4; i++) //城池属性设置
        {
            cityAttValuePy.Add((int.Parse(LoadJsonFile.forcesTableDatas[playerForceId - 1][11 + i]) + int.Parse(LoadJsonFile.difficultyChooseDatas[difficultNum - 1][22 + i])));
            cityAttObj.GetChild(i).GetChild(0).GetComponent<Text>().text = (cityAttValuePy[i] + bigEvent[2 + i]).ToString();
        }
        playerForce.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            updateTopPowerData(-1);
        });

        //npc势力初始化
        string[] str_Pos = LoadJsonFile.BattleTableDates[battleId][5].Split(',');
        int indexForce = 0;
        //实例化npc上阵九宫格集合
        fightControll.JiuGongGes = new List<Transform>(npc_Count - 1);
        //实例化城池集合
        npcForcesObjs = new List<GameObject>(npc_Count);
        //实例化npc血条集合
        fightControll.npcPlayer_hpSliders = new List<Slider>(npc_Count - 1);
        for (int i = 0; i < npc_Count; i++)
        {
            indexForce = int.Parse(str_Force[i]);
            if (indexForce != playerForceId)
            {
                enemy_forces.Add(indexForce);
                InitEnemyForce(indexForce, int.Parse(str_Pos[i]));
            }
            else
            {
                playerForce.position = cityPos.GetChild(int.Parse(str_Pos[i])).position;   //玩家城市位置设置
            }
            //实例化结束游戏的信息结算控件
            Instantiate(informationObj, endGameInfoList);
        }
        //实例化npc上阵武将数据集合
        fightControll.InitNpcDataList(npc_Count - 1);
    }

    /// <summary>
    /// 大地图初始化npc势力
    /// </summary>
    /// <param name="npcForceId">势力ID</param>
    /// <param name="posId">城市位置</param>
    private void InitEnemyForce(int npcForceId, int posId)
    {
        //实例化npc城市
        GameObject force_obj = Instantiate(npcForceCity, forcesParentTran);
        //设置势力图标
        force_obj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/Forces/" + LoadJsonFile.forcesTableDatas[npcForceId - 1][4], typeof(Sprite)) as Sprite;
        //设置城池图标
        force_obj.GetComponent<Image>().sprite = Resources.Load("Image/Map/" + LoadJsonFile.forcesTableDatas[npcForceId - 1][7], typeof(Sprite)) as Sprite;
        //城市位置设置
        force_obj.transform.position = cityPos.GetChild(posId).position;
        //点击事件设置
        int index = 0;
        for (; index < enemy_forces.Count; index++)
        {
            if (enemy_forces[index] == npcForceId)
                break;
        }
        List<int> cityAttList = new List<int>();
        for (int i = 0; i < 4; i++) //城池属性设置
        {
            cityAttList.Add((int.Parse(LoadJsonFile.forcesTableDatas[npcForceId - 1][11 + i]) + int.Parse(LoadJsonFile.difficultyChooseDatas[difficultNum - 1][26 + i])));
        }
        cityAttValueNpc.Add(cityAttList);
        force_obj.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            npcForceOnClick(index);
        });
        npcForcesObjs.Add(force_obj);   //添加到npc集合
        fightControll.npcPlayer_hpSliders.Add(force_obj.transform.GetChild(1).GetComponent<Slider>()); //添加npc血条集合
        //npc上阵九宫格实例化
        fightControll.JiuGongGes.Add(Instantiate(jiuGongGeObj_npc, npc_ShangZhenWei).transform);
    }

    //npc势力点击事件
    private void npcForceOnClick(int npcForceIndex)
    {
        //点击音效
        backAudioSource.clip = Resources.Load("Sounds/古筝" + Random.Range(1, 8), typeof(AudioClip)) as AudioClip;
        backAudioSource.Play();
        //顶部势力信息更新
        updateTopPowerData(npcForceIndex);

    }

    private int force_Index;    //势力选择记录
    //更新顶部信息
    public void updateTopPowerData(int index)   //-1为玩家, 0-n为npc
    {
        if (force_Index != index)
        {
            force_Index = index;
            int powerNums = 0;
            if (index != -1)
            {
                powerNums = fightControll.GetNowNPCPowerNums(index);
            }
            else
            {
                powerNums = transform.GetComponent<HeroIdChangeAndSave>().GetNowPlayerPowerNums();
            }

            int forceId = (index < 0) ? playerForceId : enemy_forces[index];
            topPower.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/Forces/" + LoadJsonFile.forcesTableDatas[forceId - 1][4], typeof(Sprite)) as Sprite;    //势力头像
            topPower.transform.GetChild(2).GetComponent<Text>().text = LoadJsonFile.forcesTableDatas[forceId - 1][5];   //显示城市名 
            topPower.transform.GetChild(3).GetComponent<Text>().text = "战力：" + powerNums;   //显示战力
            topPower.transform.GetChild(4).GetChild(0).GetComponent<Text>().fontSize = LoadJsonFile.forcesTableDatas[forceId - 1][1].Length > 2 ? 50 : 70;
            topPower.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = LoadJsonFile.forcesTableDatas[forceId - 1][1];
            topPower.transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = "\u2000\u2000\u2000" + LoadJsonFile.forcesTableDatas[forceId - 1][2];
            setTextContent(index);
            setCityAttText(index);
            setBigEventContent(index);
            fightControll.ChangeSelectForce(index);
        }
        else
        {
            //再次点击该势力城池
            if (index != -1)
            {

            }
            else
            {
                //回城
                InitSelectForceIndex();
                playerForce.parent.parent.GetChild(2).GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    /// <summary>
    /// 设置大事件提示框内容
    /// </summary>
    /// <param name="index"></param>
    private void setBigEventContent(int index)
    {
        string bigEventStr = "";
        int[] eventValue = (index < 0) ? bigEvent : bigEventNpc[index];
        if (eventValue[0] != 0)
        {
            int valueIndex = 0;
            for (int i = 2; i < 6; i++)
            {
                if (eventValue[i] != 0)
                {
                    valueIndex = Mathf.Abs(eventValue[i]);
                }
            }
            bigEventStr = LoadJsonFile.EventTableDates[eventValue[0]][1] + "\t" + string.Format(LoadJsonFile.EventTableDates[eventValue[0]][9], valueIndex);
        }
        else
        {
            bigEventStr = LoadJsonFile.EventTableDates[eventValue[0]][1] + "\t" + LoadJsonFile.EventTableDates[eventValue[0]][9];
        }
        bigEventInfoText.text = bigEventStr;
    }

    //设置顶部城池属性值显示
    private void setCityAttText(int index)
    {
        List<int> valueList = new List<int>();
        int[] eventValue = new int[6];
        if (index < 0)
        {
            valueList = cityAttValuePy;
            eventValue = bigEvent;
        }
        else
        {
            valueList = cityAttValueNpc[index];
            eventValue = bigEventNpc[index];
        }
        for (int i = 0; i < cityAttObj_world.childCount; i++)
        {
            cityAttObj_world.GetChild(i).GetChild(0).GetComponent<Text>().text = (valueList[i] + eventValue[2 + i]).ToString();
        }
    }

    //设置进攻防守战鼓的image
    private void setTextContent(int index)   //-1守，0-n攻
    {
        if (index < 0)
        {
            fightOrDefend.sprite = Resources.Load("Effect/UI/toBattle/守", typeof(Sprite)) as Sprite;
            selectForceIamge.position = playerForce.position;
        }
        else
        {
            fightOrDefend.sprite = Resources.Load("Effect/UI/toBattle/攻", typeof(Sprite)) as Sprite;
            selectForceIamge.position = npcForcesObjs[index].transform.position;
        }
    }

    //声音控制，继承main场景选择
    void soundContrll_(int soundStates)
    {
        if (soundStates == 1)
        {
            cameraAudio.GetComponent<AudioListener>().gameObject.SetActive(true);
        }
        else if (soundStates == 0)
        {
            cameraAudio.GetComponent<AudioListener>().gameObject.SetActive(false);
        }
    }

    //延时打开战斗按钮
    private void OpenPlayFightBtn()
    {
        fightBtn.enabled = true;
    }

    /// <summary>
    /// 更改城池掉血是否显示
    /// </summary>
    /// <param name="boo"></param>
    public void ChangeisShowCutHpText(bool boo)
    {
        isShowCutHpText = boo;
    }

    /// <summary>
    /// 重置势力选择
    /// </summary>
    public void InitSelectForceIndex()
    {
        force_Index = -2;
    }

    /// <summary>
    /// 初始化大事件
    /// </summary>
    private void initEvent()
    {
        //玩家大事件初始化
        bigEvent = SetEventValue(bigEvent, true);
        //设置事件特效
        SetEventEffect(bigEvent[0], playerForce.GetChild(4));
        //Npc大事件初始化
        bigEventNpc = new List<int[]>();
        for (int i = 0; i < npc_Count - 1; i++)
        {
            int[] npcEvent = new int[6];
            npcEvent = SetEventValue(npcEvent, false);
            SetEventEffect(npcEvent[0], npcForcesObjs[i].transform.GetChild(3));
            bigEventNpc.Add(npcEvent);
        }
    }

    /// <summary>
    /// 更新大事件
    /// </summary>
    public void UpdateBigEventState()
    {
        bigEvent[1] -= 1;
        if (bigEvent[1] <= 0)
        {
            bigEvent = SetEventValue(bigEvent, true);
            SetEventEffect(bigEvent[0], playerForce.GetChild(4));
        }
        for (int i = 0; i < npc_Count - 1; i++)
        {
            bigEventNpc[i][1] -= 1;
            if (bigEventNpc[i][1] <= 0)
            {
                bigEventNpc[i] = SetEventValue(bigEventNpc[i], false);
                SetEventEffect(bigEventNpc[i][0], npcForcesObjs[i].transform.GetChild(3));
            }
        }
    }

    /// <summary>
    /// 设置大事件值
    /// </summary>
    /// <param name="cityEvent"></param>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    private int[] SetEventValue(int[] cityEvent, bool isPlayer)
    {
        cityEvent[0] = SelectEvent(isPlayer);
        cityEvent[1] = int.Parse(LoadJsonFile.EventTableDates[cityEvent[0]][4]);
        List<int> valueList_npc = EventAddValueSet(cityEvent[0]);
        for (int j = 2; j < 6; j++)
        {
            cityEvent[j] = valueList_npc[j - 2];
        }
        return cityEvent;
    }

    /// <summary>
    /// 挑选大事件
    /// </summary>
    /// <param name="isPlayer">是否为玩家</param>
    /// <returns>事件索引</returns>
    private int SelectEvent(bool isPlayer)
    {
        int wvCount = 0;
        int indexEvent = -1;
        int indexTableRow = isPlayer ? 2 : 3;
        for (int i = 0; i < LoadJsonFile.EventTableDates.Count; i++)
        {
            wvCount += int.Parse(LoadJsonFile.EventTableDates[i][indexTableRow]);
        }
        int randValue = Random.Range(0, wvCount + 1);
        while (randValue >= 0)
        {
            indexEvent++;
            randValue -= int.Parse(LoadJsonFile.EventTableDates[indexEvent][indexTableRow]);
        }
        return indexEvent;
    }

    /// <summary>
    /// 事件加成值设定
    /// </summary>
    /// <param name="indexEvent">事件索引</param>
    /// <returns>加成值</returns>
    private List<int> EventAddValueSet(int indexEvent)
    {
        List<int> valueList = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            string str = LoadJsonFile.EventTableDates[indexEvent][5 + i];
            if (str != "")
            {
                string[] strArray = str.Split(',');
                int value = Random.Range(int.Parse(strArray[0]), int.Parse(strArray[1]));
                Debug.Log("事件 " + LoadJsonFile.EventTableDates[indexEvent][1] + " 的加成值为 " + value);
                valueList.Add(value);
            }
            else
            {
                valueList.Add(0);
            }
        }
        return valueList;
    }

    /// <summary>
    /// 设置事件特效
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tran"></param>
    private void SetEventEffect(int index, Transform tran)
    {
        int index_event = int.Parse(LoadJsonFile.EventTableDates[index][10]);
        if (index_event != -1)
        {
            if (tran.childCount > 0)
            {
                Destroy(tran.GetChild(0).gameObject);
            }
            else
            {
                Instantiate(bigEvent_eftObj[index_event], tran);
            }
        }
    }
}