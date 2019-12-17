using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum StateOfAttack   //攻击状态
{
    ReadyForFight,  //0备战
    FightNow,       //1攻击
    FightOver       //2攻击结束
}

public class CardMove : MonoBehaviour
{
    public GameObject stateIcon;        //卡牌上状态小标预制件
    public GameObject cutHp_text;       //掉血文字
    public GameObject cutHp_text2;      //文字二类

    private FightState fight_State;     //特殊攻击状态
    public FightState Fight_State { get => fight_State; set => fight_State = value; }

    private int isPlayerOrEnemy;    //是己方卡牌，还是敌方卡牌 0己方(playerCards)，1敌方(enemyCards)
    public int IsPlayerOrEnemy { get => isPlayerOrEnemy; set => isPlayerOrEnemy = value; }

    private int armsSkillStatus;    //兵种技能激活状态 0-未激活; 1-激活3兵种; 2-激活6兵种
    public int ArmsSkillStatus { get => armsSkillStatus; set => armsSkillStatus = value; }

    private int realDamage; //造成的真实伤害

    public bool isFightInThisBout; //当前回合是否进行过攻击

    private string armsId; //武将兵种
    public string ArmsId { get => armsId; set => armsId = value; }

    private int fightNums;  //参与战斗回合数
    public int FightNums { get => fightNums; set => fightNums = value; }

    private int heroId; //武将ID
    public int HeroId { get => heroId; set => heroId = value; }

    private int grade;  //品阶
    public int Grade { get => grade; set => grade = value; }

    private int fullhealth;//满血量
    public int Fullhealth { get => fullhealth; set => fullhealth = value; }

    private int health; //血量
    public int Health { get => health; set => health = value; }

    private int defence;//防御
    public int Defence { get => defence; set => defence = value; }

    private int force;  //攻击力
    public int Force { get => force; set => force = value; }

    private StateOfAttack isAttack;  //记录武将的攻击状态
    public StateOfAttack IsAttack { get => isAttack; set => isAttack = value; }

    //更换攻击状态
    public void ChangeToFight(StateOfAttack state)
    {
        IsAttack = state;
    }

    private int enemyIndex; //要攻击的敌人编号
    public int EnemyIndex { get => enemyIndex; set => enemyIndex = value; }

    private GameObject enemyObj; //要攻击的敌人
    public GameObject EnemyObj { get => enemyObj; set => enemyObj = value; }

    private bool isAttack_first;    //是否为先手
    public bool IsAttack_first { get => isAttack_first; set => isAttack_first = value; }

    private float dodgeRate;    //闪避率
    public float DodgeRate { get => dodgeRate; set => dodgeRate = value; }
    private float realDodgeRate;    //实际闪避率

    private float thumpRate;    //重击率
    public float ThumpRate { get => thumpRate; set => thumpRate = value; }

    private float thumpDamage;  //重击伤害
    public float ThumpDamage { get => thumpDamage; set => thumpDamage = value; }

    private float critRate; //暴击率
    public float CritRate { get => critRate; set => critRate = value; }

    private float critDamage;   //暴击伤害
    public float CritDamage { get => critDamage; set => critDamage = value; }

    private float armorPenetrationRate; //破甲百分比
    public float ArmorPenetrationRate { get => armorPenetrationRate; set => armorPenetrationRate = value; }

    private bool isDizzy;   //是否被击晕
    public bool IsDizzy { get => isDizzy; set => isDizzy = value; }

    private bool isHadSpecialState;     //是否拥有特殊状态
    public bool IsHadSpecialState { get => isHadSpecialState; set => isHadSpecialState = value; }

    Vector3 vec = new Vector3();    //记录卡牌初始位置

    AudioSource audiosource;  //玩家卡牌音效

    int flag = 1;   //记录攻击的对手是玩家还是敌方
    bool isCalcul = false;  //是否计算过攻击位置
    Vector3 vec_Enemy;  //记录攻击位置

    private void Awake()
    {
        isFightInThisBout = false;
        FightNums = 0;  //攻击次数
    }

    private void Start()
    {
        //设置武将头像背景
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Image/RoleIcon/" + LoadJsonFile.RoleTableDatas[HeroId - 1][22], typeof(Sprite)) as Sprite;
        audiosource = GetComponent<AudioSource>();
        realDodgeRate = DodgeRate;  //真实闪避率初始化
        ArmsStaticSkillGet(ArmsId, ArmsSkillStatus);   //静态兵种技能
    }

    /// <summary>
    /// 其他必要值的赋值
    /// </summary>
    public void OtherDataSet()
    {
        transform.GetChild(8).GetChild(0).GetComponent<Text>().text = Grade.ToString();
        ChangeToFight(StateOfAttack.ReadyForFight);
        vec = gameObject.transform.position;
        realDamage = Force;
    }

    /// <summary>
    /// 结束战斗状态
    /// </summary>
    private void ChangeToFightOver()
    {
        ChangeToFight(StateOfAttack.FightOver);
        gameObject.transform.position = vec;
    }
    /// <summary>
    /// 复原
    /// </summary>
    private void ComeBackTranForm()
    {
        gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), FightControll.speedTime * 0.5f).SetAutoKill(false);
    }

    private void Update()
    {
        //攻击前摇
        if (IsAttack == StateOfAttack.FightNow && EnemyObj != null)   //如果卡牌攻击状态为攻击 并且 目标敌人存在 
        {
            //选定目标
            if (!isCalcul)
            {
                if (IsHadSpecialState)  //进入特殊攻击
                {
                    if (Fight_State.isDizzy == true)//自身处于眩晕状态
                    {
                        ReleaseDizzyState(gameObject);   //接触眩晕
                        FightCardSP.isFightNow = false;     //通知战斗总控制代码此卡牌攻击结束
                        ChangeToFight(StateOfAttack.ReadyForFight);     //直接进入备战
                        return;
                    }
                }
                else
                {
                    if ((ArmsId == "7" || ArmsId == "8" || ArmsId == "9") && ArmsSkillStatus > 0 && Fight_State.isFireAttack == false)  //远程兵种技能
                    {
                        ArmsDynamicSkillGet(ArmsId, ArmsSkillStatus);
                        gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), FightControll.speedTime * 0.9f).SetAutoKill(false);
                        Invoke("ComeBackTranForm", FightControll.speedTime * 0.9f);
                        Invoke("ChangeToFightOver", FightControll.speedTime * 2f);
                    }
                    else
                    {
                        if (EnemyObj.GetComponent<CardMove>().IsPlayerOrEnemy == 0) //选定目标位置参数
                            flag = 1;
                        else
                            flag = -1;

                        vec_Enemy = EnemyObj.transform.position + (flag * (new Vector3(0, FightControll.fightPosCha, 0)));       //计算要攻击后移动到的位置
                        gameObject.transform.DOMove(vec_Enemy, FightControll.speedTime).SetAutoKill(false); //武将开始往目标身前移动
                    }
                }
                isCalcul = true;
            }

            //攻击目标
            if (1f > (gameObject.transform.position - vec_Enemy).magnitude)     //到达目标敌人面前位置
            {
                if (Fight_State.isFireAttack == true)
                {
                    FireFightSkill();//火攻
                }
                else
                {
                    ArmsDynamicSkillGet(ArmsId, ArmsSkillStatus);   //普通攻击+动态技能
                }

                gameObject.transform.DOMove(vec, FightControll.speedTime).SetAutoKill(false);   //完成攻击,武将开始往原始位置移动
                ChangeToFight(StateOfAttack.FightOver);                //更改攻击状态为攻击结束，进入攻击后摇
            }
        }

        //攻击后摇
        if (IsAttack == StateOfAttack.FightOver)    //如果卡牌攻击状态为攻击结束
        {
            isCalcul = false;
            if (1f > (gameObject.transform.position - vec).magnitude && IsAttack != StateOfAttack.ReadyForFight) //卡牌回到起始位置 并且 攻击状态不是准备状态
            {
                if (Fight_State.isBatter == true)//处于连击状态
                {
                    BatterFightSkill(); //连击技能
                    return;
                }

                FightCardSP.isFightNow = false;     //通知战斗总控制代码此卡牌攻击结束

                ChangeToFight(StateOfAttack.ReadyForFight);     //改变这个卡牌的攻击状态为 准备状态
            }
        }
    }

    /// <summary>
    /// 动态兵种技能（卡牌在每次攻击时调用）
    /// </summary>
    /// <param name="armsId">兵种id</param>
    /// <param name="activeId">激活状态</param>
    private void ArmsDynamicSkillGet(string armsId, int activeId)
    {
        switch (armsId)
        {
            case "1":   //山兽
                switch (activeId)
                {
                    case 0:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        break;
                    case 1:
                        //将造成伤害的40%转化为自身血量
                        ShanShouSkill(0.4f, 1);
                        break;
                    case 2:
                        //将造成伤害的60%转化为自身血量
                        ShanShouSkill(0.6f, 2);
                        break;
                }
                break;

            case "2":   //海兽
                switch (activeId)
                {
                    default:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        if (activeId > 0)
                        {
                            NormalAttackEffects(gameObject, 10);   //反伤特效
                        }
                        break;
                }
                break;

            case "3":   //飞兽
                switch (activeId)
                {
                    default:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        if (activeId > 0)
                        {
                            NormalAttackEffects(EnemyObj, 12);
                        }
                        break;
                }
                break;

            case "4":   //人杰
                switch (activeId)
                {
                    case 0:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        break;
                    case 1:
                        //每次攻击提升20%攻击，10%防御，可叠加3次。
                        RenJieSkill(0.2f, 0.1f);
                        break;
                    case 2:
                        //每次攻击提升30%攻击，15%防御，可叠加3次。
                        RenJieSkill(0.3f, 0.15f);
                        break;
                }
                break;

            case "5":   //祖巫
                switch (activeId)
                {
                    case 0:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        break;
                    case 1:
                        //突刺敌方后排50%伤害。
                        ZuWuSkill(0.5f);
                        break;
                    case 2:
                        //突刺敌方后排80%伤害。
                        ZuWuSkill(0.8f);
                        break;
                }
                break;

            case "6":   //散仙
                switch (activeId)
                {
                    case 0:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        break;
                    case 1:
                        //攻击同一排敌人，每个造成50%伤害。
                        SanXianSkill(0.5f);
                        break;
                    case 2:
                        //攻击同一排敌人，每个造成75%伤害。
                        SanXianSkill(0.75f);
                        break;
                }
                break;

            case "7":   //辅神
                switch (activeId)
                {
                    case 0:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        break;
                    case 1:
                        //随机攻击3个不同目标，每个造成20%伤害，30%几率击晕1回合。
                        FuShenSkill(3, 0.2f, 0.3f);
                        break;
                    case 2:
                        //随机攻击4个不同目标，每个造成25%伤害，30%几率击晕1回合。
                        FuShenSkill(4, 0.25f, 0.3f);
                        break;
                }
                break;

            case "8":   //魔神
                switch (activeId)
                {
                    case 0:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        break;
                    case 1:
                        //随机攻击3个目标，每个造成50%伤害，对同一目标最多攻击2次。
                        MoShenSkill(0.5f, 3);
                        break;
                    case 2:
                        //随机攻击4个目标，每个造成50%伤害，对同一目标最多攻击2次。
                        MoShenSkill(0.5f, 4);
                        break;
                }
                break;

            case "9":   //天神
                switch (activeId)
                {
                    case 0:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        break;
                    case 1:
                        //治疗2个血量最低的友方目标，治疗量为伤害的50%。
                        TianShenSkill(2, 0.5f);
                        break;
                    case 2:
                        //治疗3个血量最低的友方目标，治疗量为伤害的40%。
                        TianShenSkill(3, 0.4f);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// 山兽动态技能
    /// </summary>
    /// <param name="percentage">转化血量百分比</param>   
    /// <param name="index">兵种激活状态</param>
    private void ShanShouSkill(float percentage, int index)
    {
        if (index < 2)
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "虎扑";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("虎扑");
        }
        else
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "狂战";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("狂战");
        }
        NormalAttackEffects(EnemyObj, 9);
        UpdateEnemyHp(EnemyObj);
        int addHp = (int)(realDamage * percentage);
        if (addHp + Health > Fullhealth) //当吸血量加当前血量大于总血量时
        {
            Health = Fullhealth;
        }
        else
        {
            Health = Health + addHp;
        }
        shanAddHp = addHp;
        //吸血数值文字
        if (transform.GetChild(5).childCount < 1)
        {
            GameObject cutHpObj = Instantiate(cutHp_text2, transform.GetChild(5));
            cutHpObj.GetComponent<Text>().color = ColorData.green_deep_Color;
            cutHpObj.GetComponent<Text>().text = "+" + shanAddHp;
            transform.GetComponent<Slider>().value = 1 - transform.GetComponent<CardMove>().Health / (float)transform.GetComponent<CardMove>().Fullhealth;
        }
        else
        {
            Invoke("ShanShouAddHp", FightControll.speedTime * 1.5f);
        }
    }
    int shanAddHp = 0;
    private void ShanShouAddHp()
    {
        GameObject cutHpObj = Instantiate(cutHp_text, transform.GetChild(5));
        cutHpObj.GetComponent<Text>().color = ColorData.green_deep_Color;
        cutHpObj.GetComponent<Text>().text = "+" + shanAddHp;

        //血条的显示
        transform.GetComponent<Slider>().value = 1 - transform.GetComponent<CardMove>().Health / (float)transform.GetComponent<CardMove>().Fullhealth;
    }

    /// <summary>
    /// 海兽动态技能
    /// </summary>
    /// <param name="obj">敌人卡牌</param>
    /// <param name="index">兵种激活状态</param>
    private void HaiShouSkill(GameObject obj, int index)
    {
        if (index <= 0)
            return;
        int hurt = 0;
        if (obj.GetComponent<CardMove>().ArmsSkillStatus == 1)
        {
            hurt = (int)(realDamage * 0.2f);
            Debug.Log("刺甲");
            obj.transform.GetChild(4).GetComponent<Text>().text = "刺甲";
        }
        else
        {
            hurt = (int)(realDamage * 0.4f);
            Debug.Log("刃甲");
            obj.transform.GetChild(4).GetComponent<Text>().text = "刃甲";
        }
        Health -= hurt;     //反伤
        transform.GetComponent<Slider>().value = 1 - Health / (float)Fullhealth;
        obj.transform.GetChild(4).gameObject.SetActive(true);
        fanShangHurt = hurt;
        //NormalAttackEffects(obj, 10);   //反伤特效
        if (transform.GetChild(5).childCount < 1)
        {
            GameObject cutHpObj = Instantiate(cutHp_text2, transform.GetChild(5));
            cutHpObj.GetComponent<Text>().color = Color.red;
            cutHpObj.GetComponent<Text>().text = "-" + fanShangHurt;
        }
        else
        {
            Invoke("haishouAdd", FightControll.speedTime * 1f);
        }
    }
    int fanShangHurt = 0;
    private void haishouAdd()
    {
        GameObject cutHpObj = Instantiate(cutHp_text, transform.GetChild(5));
        cutHpObj.GetComponent<Text>().color = Color.red;
        cutHpObj.GetComponent<Text>().text = "-" + fanShangHurt;
    }

    /// <summary>
    /// 飞兽动态技能
    /// </summary>
    /// <param name="percenHp">每次损失的血量</param>
    /// <param name="percentage">每次提升的闪避率</param>
    public void FeiShouSkill(float percenHp,float percentage)
    {
        float loseHp = (Fullhealth - Health) / (float)Fullhealth;
        int num = (int)(loseHp / percenHp);
        if (num > 0)
        {
            if (ArmsSkillStatus == 1)
            {
                gameObject.transform.GetChild(4).GetComponent<Text>().text = "飞闪";
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                //Debug.Log("飞闪");
            }
            if (ArmsSkillStatus == 2)
            {
                gameObject.transform.GetChild(4).GetComponent<Text>().text = "疾风";
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                //Debug.Log("疾风");
            }
            //每损失20%血量提升闪避率
            realDodgeRate = DodgeRate + (percentage * num);
            if (!Fight_State.isPopular) //判断是否有风遁状态
            {
                //添加风遁状态
                Fight_State.isPopular = true;
                GameObject icon = Instantiate(stateIcon, transform.GetChild(9));
                icon.name = StateName.popularName;
                icon.GetComponent<Image>().sprite = Resources.Load("Image/state/" + StateName.popularName, typeof(Sprite)) as Sprite;
            }
        }
        Debug.Log("飞兽闪避率："+ realDodgeRate*100+"%");
    }

    private int overlayNum_RenJie = 0; //记录 人杰 技能效果叠加次数
    /// <summary>
    /// 人杰动态技能
    /// </summary>
    /// <param name="percent_Force">攻击加成百分比</param>
    /// <param name="percent_Defence">防御加成百分比</param>
    private void RenJieSkill(float percent_Force, float percent_Defence)
    {
        NormalAttack(EnemyObj);
        UpdateEnemyHp(EnemyObj);
        NormalAttackEffects(EnemyObj, 11);
        if (overlayNum_RenJie >= 3)   //只能叠加3次
            return;
        else
        {
            overlayNum_RenJie++;
            Force += (int)(percent_Force * Force);
            Defence += (int)(percent_Defence * Defence);
            if (ArmsSkillStatus == 1)
            {
                gameObject.transform.GetChild(4).GetComponent<Text>().text = "战意";
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                Debug.Log("战意");
            }
            if (ArmsSkillStatus == 2)
            {
                gameObject.transform.GetChild(4).GetComponent<Text>().text = "英魂";
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                Debug.Log("英魂");
            }
            if (!Fight_State.isFightMean) //判断是否有战意状态
            {
                //添加战意状态
                Fight_State.isFightMean = true;
                GameObject icon = Instantiate(stateIcon, transform.GetChild(9));
                icon.name = StateName.fightMeanName;
                icon.GetComponent<Image>().sprite = Resources.Load("Image/state/" + StateName.fightMeanName, typeof(Sprite)) as Sprite;
            }
        }
    }

    /// <summary>
    /// 祖巫动态技能
    /// </summary>
    /// <param name="percent">伤害百分比</param>
    private void ZuWuSkill(float percent)
    {
        realDamage = Force;
        realDamage = SkillRealDamage(realDamage, EnemyObj, false);  //计算技能伤害
        UpdateEnemyHp(EnemyObj);
        NormalAttackEffects(EnemyObj, 3);
        if (EnemyIndex < 6)
        {
            realDamage = Force;
            realDamage = (int)(realDamage * percent);

            ZuWuSkillSkillSecond(IsPlayerOrEnemy == 0 ?
                FightCardSP.enemyCards[EnemyIndex + 3] : FightCardSP.playerCards[EnemyIndex + 3]
                );
        }
    }

    /// <summary>
    /// 祖巫穿刺第二段伤害
    /// </summary>
    /// <param name="obj"></param>
    private void ZuWuSkillSkillSecond(GameObject obj)
    {
        if (obj != null && obj.GetComponent<CardMove>().Health > 0)
        {
            if (ArmsSkillStatus == 1)
            {
                gameObject.transform.GetChild(4).GetComponent<Text>().text = "穿刺";
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                Debug.Log("穿刺");
            }
            if (ArmsSkillStatus == 2)
            {
                gameObject.transform.GetChild(4).GetComponent<Text>().text = "突刺";
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                Debug.Log("突刺");
            }
            realDamage = SkillRealDamage(realDamage, obj, false);
            UpdateEnemyHp(obj);
            NormalAttackEffects(obj, 3);
        }
    }

    /// <summary>
    /// 散仙动态技能
    /// </summary>
    /// <param name="percent">伤害百分比</param>
    private void SanXianSkill(float percent)
    {
        realDamage = Force;
        int damageNote = (int)(realDamage * percent);
        if (ArmsSkillStatus == 1)
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "横扫";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("横扫");
        }
        if (ArmsSkillStatus == 2)
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "狂斩";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("狂斩");
        }

        int index = EnemyIndex / 3; //0前排1中排2后排
        if (IsPlayerOrEnemy == 0) // 该卡牌是己方(playerCards)，在敌方(enemyCards)中找目标
        {
            for (int i = index * 3; i < (index + 1) * 3; i++)
            {
                SanXianSkillAdd(FightCardSP.enemyCards[i], damageNote);
            }
        }
        else
        {
            for (int i = index * 3; i < (index + 1) * 3; i++)
            {
                SanXianSkillAdd(FightCardSP.playerCards[i], damageNote);
            }
        }
    }
    private void SanXianSkillAdd(GameObject obj, int damageNote)
    {
        if (obj != null && obj.GetComponent<CardMove>().Health > 0)
        {
            realDamage = SkillRealDamage(damageNote, obj, false);
            UpdateEnemyHp(obj);
            NormalAttackEffects(obj, 4);
        }
    }

    /// <summary>
    /// 辅神动态技能
    /// </summary>
    /// <param name="nums">攻击目标数</param>
    /// <param name="percentage">伤害百分比</param>
    /// <param name="stunProbability">击晕效果触发概率</param>
    private void FuShenSkill(int nums, float percentage, float stunProbability)
    {
        if (ArmsSkillStatus == 1)
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "雷震";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("雷震");
        }
        if (ArmsSkillStatus == 2)
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "天怒";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("天怒");
        }
        realDamage = Force;
        int damageNote = (int)(realDamage * percentage);
        List<int> arrs = new List<int>();   //记录卡牌下标
        //随机攻击3个不同目标，每个造成30%伤害，20%机率击晕1回合。
        if (IsPlayerOrEnemy == 0)
        {
            for (int i = 0; i < 9; i++)
            {
                if (FightCardSP.enemyCards[i] != null && FightCardSP.enemyCards[i].GetComponent<CardMove>().Health > 0)
                {
                    arrs.Add(i);
                }
            }
            if (arrs.Count > nums) //敌人卡牌数大于3张时
            {
                int[] arr_index = new int[nums];    //存放要攻击的对象卡牌下标
                for (int i = 0; i < nums; i++)
                {
                    arr_index[i] = -1;
                }
                int index;
                bool isContinue;
                for (int i = 0; i < nums; i++)
                {
                    do
                    {
                        isContinue = false;
                        index = Random.Range(0, arrs.Count);
                        for (int j = 0; j < i; j++)
                        {
                            if (arr_index[j] == arrs[index])
                            {
                                isContinue = true;
                            }
                        }
                    } while (isContinue);
                    arr_index[i] = arrs[index];
                }
                for (int i = 0; i < nums; i++)
                {
                    realDamage = SkillRealDamage(damageNote, FightCardSP.enemyCards[arr_index[i]], false);
                    UpdateEnemyHp(FightCardSP.enemyCards[arr_index[i]]);
                    NormalAttackEffects(FightCardSP.enemyCards[arr_index[i]], 6);
                    if (TakeSpecialAttack(stunProbability)) //判断是否可以眩晕对手
                    {
                        DizzyEnemyCards(FightCardSP.enemyCards[arr_index[i]]);      //执行眩晕效果
                    }
                }
            }
            else
            {
                for (int i = 0; i < arrs.Count; i++)
                {
                    realDamage = SkillRealDamage(damageNote, FightCardSP.enemyCards[arrs[i]], false);
                    UpdateEnemyHp(FightCardSP.enemyCards[arrs[i]]);
                    NormalAttackEffects(FightCardSP.enemyCards[arrs[i]], 6);
                    if (TakeSpecialAttack(stunProbability)) //判断是否触发眩晕
                    {
                        DizzyEnemyCards(FightCardSP.enemyCards[arrs[i]]);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                if (FightCardSP.playerCards[i] != null && FightCardSP.playerCards[i].GetComponent<CardMove>().Health > 0)
                {
                    arrs.Add(i);
                }
            }
            if (arrs.Count > nums) //敌人卡牌数大于3张时
            {
                int[] arr_index = new int[nums];    //存放要攻击的对象卡牌下标
                for (int i = 0; i < nums; i++)
                {
                    arr_index[i] = -1;
                }
                int index;
                bool isContinue;
                for (int i = 0; i < nums; i++)
                {
                    do
                    {
                        isContinue = false;
                        index = Random.Range(0, arrs.Count);
                        for (int j = 0; j < i; j++)
                        {
                            if (arr_index[j] == arrs[index])
                            {
                                isContinue = true;
                            }
                        }
                    } while (isContinue);
                    arr_index[i] = arrs[index];
                }
                for (int i = 0; i < nums; i++)
                {
                    realDamage = SkillRealDamage(damageNote, FightCardSP.playerCards[arr_index[i]], false);
                    UpdateEnemyHp(FightCardSP.playerCards[arr_index[i]]);
                    NormalAttackEffects(FightCardSP.playerCards[arr_index[i]], 6);
                    if (TakeSpecialAttack(stunProbability))
                    {
                        DizzyEnemyCards(FightCardSP.playerCards[arr_index[i]]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < arrs.Count; i++)
                {
                    realDamage = SkillRealDamage(damageNote, FightCardSP.playerCards[arrs[i]], false);
                    UpdateEnemyHp(FightCardSP.playerCards[arrs[i]]);
                    NormalAttackEffects(FightCardSP.playerCards[arrs[i]], 6);
                    if (TakeSpecialAttack(stunProbability)) //判断是否触发眩晕
                    {
                        DizzyEnemyCards(FightCardSP.playerCards[arrs[i]]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 魔神动态技能
    /// </summary>
    /// <param name="percent">伤害百分比</param>
    ///  /// <param name="attackNum">攻击目标个数</param>  3兵种是3  6兵种是4
    private void MoShenSkill(float percent, int attackNum)
    {
        if (ArmsSkillStatus == 1)
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "荒火";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("荒火");
        }
        if (ArmsSkillStatus == 2)
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "炎爆";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("炎爆");
        }
        realDamage = Force;
        int damageNote = (int)(realDamage * percent);
        if (IsPlayerOrEnemy == 0)
        {
            List<int> arrayGo = new List<int>(); //位置数列，为同一目标最多攻击两次做准备
            for (int i = 0; i < 9; i++)
            {
                if (FightCardSP.enemyCards[i] != null && FightCardSP.enemyCards[i].GetComponent<CardMove>().Health > 0)
                {
                    arrayGo.Add(i);
                    arrayGo.Add(i);
                }
            }
            int rondomNum = 0;
            if (arrayGo.Count < 3)
            {
                rondomNum = arrayGo.Count;
            }
            else
            {
                rondomNum = attackNum;
            }
            List<int> array_down = new List<int>();//临时存放随机的三个数,arrayGo的下标
            List<int> array_transform = new List<int>();//临时存放三个的位置
            for (int i = 0; i < rondomNum; i++)
            {
                int temp_num = Random.Range(0, arrayGo.Count);
                if (!array_down.Contains(temp_num))
                {
                    array_down.Add(temp_num);
                }
                else
                {
                    i--;
                }
            }
            for (int i = 0; i < rondomNum; i++)
            {
                array_transform.Add(arrayGo[array_down[i]]);
            }
            //依次攻击目标
            for (int i = 0; i < array_transform.Count; i++)
            {
                realDamage = SkillRealDamage(damageNote, FightCardSP.enemyCards[array_transform[i]], false);
                UpdateEnemyHp(FightCardSP.enemyCards[array_transform[i]]);
                NormalAttackEffects(FightCardSP.enemyCards[array_transform[i]], 5);
            }
        }
        else
        {
            List<int> arrayGo = new List<int>(); //位置数列，为同一目标最多攻击两次做准被
            for (int i = 0; i < 8; i++)
            {
                if (FightCardSP.playerCards[i] != null && FightCardSP.playerCards[i].GetComponent<CardMove>().Health > 0)
                {
                    arrayGo.Add(i);
                    arrayGo.Add(i);
                }
            }
            int rondomNum = 0;      //要攻击的敌人数量
            if (arrayGo.Count < 3)  //敌方上阵不够3个处理
            {
                rondomNum = arrayGo.Count;
            }
            else
            {
                rondomNum = attackNum;
            }
            List<int> array_down = new List<int>();//临时存放随机的三个数,arrayGo的下标
            List<int> array_transform = new List<int>();//临时存放三个的位置
            for (int i = 0; i < rondomNum; i++)
            {
                int temp_num = Random.Range(0, arrayGo.Count);
                if (!array_down.Contains(temp_num))
                {
                    array_down.Add(temp_num);
                }
                else
                {
                    i--;
                }
            }
            for (int i = 0; i < rondomNum; i++)
            {
                array_transform.Add(arrayGo[array_down[i]]);
            }
            for (int i = 0; i < array_transform.Count; i++)
            {
                realDamage = SkillRealDamage(damageNote, FightCardSP.playerCards[array_transform[i]], false);
                UpdateEnemyHp(FightCardSP.playerCards[array_transform[i]]);
                NormalAttackEffects(FightCardSP.playerCards[array_transform[i]], 5);
            }
        }
    }

    /// <summary>
    /// 天神动态技能
    /// </summary>
    /// <param name="nums">治疗个数</param>
    /// <param name="percentage">治疗量是伤害的百分比</param>
    private void TianShenSkill(int nums, float percentage)
    {
        if (ArmsSkillStatus == 1)
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "复生";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("复生");
        }
        if (ArmsSkillStatus == 2)
        {
            gameObject.transform.GetChild(4).GetComponent<Text>().text = "回天";
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            Debug.Log("回天");
        }
        float[] minHp = new float[nums];    //临时存储最少百分比hp
        int[] arrs = new int[nums];     //存储受治疗单位的index
        bool isContinue = true;         //记录是否继续
        for (int i = 0; i < arrs.Length; i++)
        {
            arrs[i] = -1;   //初始化
            minHp[i] = 2f;
        }
        if (IsPlayerOrEnemy == 0)   //治疗己方（playerCards）
        {
            //选取回血目标
            for (int i = 0; i < nums; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < nums; k++)
                    {
                        if (arrs[k] == j)
                        {
                            isContinue = false;
                        }
                    }
                    if (isContinue && FightCardSP.playerCards[j] != null && FightCardSP.playerCards[j].GetComponent<CardMove>().Health > 0)
                    {
                        if (minHp[i] > FightCardSP.playerCards[j].GetComponent<CardMove>().Health / (float)FightCardSP.playerCards[j].GetComponent<CardMove>().Fullhealth)
                        {
                            minHp[i] = FightCardSP.playerCards[j].GetComponent<CardMove>().Health / (float)FightCardSP.playerCards[j].GetComponent<CardMove>().Fullhealth;
                            arrs[i] = j;
                        }
                    }
                    isContinue = true;
                }
            }
            //为目标回血
            int addHp = (int)(Force * percentage);
            for (int i = 0; i < nums; i++)
            {
                if (arrs[i] != -1)
                {
                    if (addHp + FightCardSP.playerCards[arrs[i]].GetComponent<CardMove>().Health > FightCardSP.playerCards[arrs[i]].GetComponent<CardMove>().Fullhealth)
                        FightCardSP.playerCards[arrs[i]].GetComponent<CardMove>().Health = FightCardSP.playerCards[arrs[i]].GetComponent<CardMove>().Fullhealth;
                    else
                        FightCardSP.playerCards[arrs[i]].GetComponent<CardMove>().Health = addHp + FightCardSP.playerCards[arrs[i]].GetComponent<CardMove>().Health;
                    addHp = SkillRealDamage(addHp, gameObject, true);
                    UpdateOwnHp(addHp, FightCardSP.playerCards[arrs[i]]);
                    NormalAttackEffects(FightCardSP.playerCards[arrs[i]], 8);
                }
            }
        }
        else        //治疗敌方（enemyCards）
        {
            for (int i = 0; i < nums; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < nums; k++)
                    {
                        if (arrs[k] == j)
                        {
                            isContinue = false;
                        }
                    }
                    if (isContinue && FightCardSP.enemyCards[j] != null && FightCardSP.enemyCards[j].GetComponent<CardMove>().Health > 0)
                    {
                        if (minHp[i] > FightCardSP.enemyCards[j].GetComponent<CardMove>().Health / (float)FightCardSP.enemyCards[j].GetComponent<CardMove>().Fullhealth)
                        {
                            minHp[i] = FightCardSP.enemyCards[j].GetComponent<CardMove>().Health / (float)FightCardSP.enemyCards[j].GetComponent<CardMove>().Fullhealth;
                            arrs[i] = j;
                        }
                    }
                    isContinue = true;
                }
            }
            //为目标回血
            int addHp = (int)(Force * percentage);
            for (int i = 0; i < nums; i++)
            {
                if (arrs[i] != -1)
                {
                    if (addHp + FightCardSP.enemyCards[arrs[i]].GetComponent<CardMove>().Health > FightCardSP.enemyCards[arrs[i]].GetComponent<CardMove>().Fullhealth)
                        FightCardSP.enemyCards[arrs[i]].GetComponent<CardMove>().Health = FightCardSP.enemyCards[arrs[i]].GetComponent<CardMove>().Fullhealth;
                    else
                        FightCardSP.enemyCards[arrs[i]].GetComponent<CardMove>().Health = addHp + FightCardSP.enemyCards[arrs[i]].GetComponent<CardMove>().Health;
                    addHp = SkillRealDamage(addHp, gameObject, true);
                    UpdateOwnHp(addHp, FightCardSP.enemyCards[arrs[i]]);
                    NormalAttackEffects(FightCardSP.enemyCards[arrs[i]], 8);
                }
            }
        }
    }
    
    /// <summary>
    /// 静态兵种技能数据加成（开始战斗卡牌加载时调用一次）
    /// </summary>
    /// <param name="armsId">兵种id</param>
    /// <param name="activeId">激活状态</param>
    private void ArmsStaticSkillGet(string armsId, int activeId)
    {
        switch (armsId)
        {
            case "1":   //山兽
                switch (activeId)
                {
                    case 0:
                        break;
                    case 1:     //防御提升10%
                        Defence = (int)(1.1 * (float)Defence);
                        break;
                    case 2:     //防御提升20%
                        Defence = (int)(1.2 * (float)Defence);
                        break;
                }
                break;
            case "2":   //海兽
                switch (activeId)
                {
                    case 0:
                        break;
                    case 1:     //血量提升10%
                        Fullhealth = (int)(1.1 * (float)Fullhealth);
                        Health = (int)(1.1 * (float)Health);
                        break;
                    case 2:     //血量提升20%
                        Fullhealth = (int)(1.2 * (float)Fullhealth);
                        Health = (int)(1.2 * (float)Health);
                        break;
                }
                break;
            case "3":   //飞兽
                switch (activeId)
                {
                    case 0:
                        break;
                    case 1:     //攻击提升10%
                        Force = (int)(1.1 * (float)Force);
                        break;
                    case 2:     //攻击提升20%
                        Force = (int)(1.2 * (float)Force);
                        break;
                }
                break;
            case "4":   //人杰
                switch (activeId)
                {
                    case 0:
                        break;
                    case 1:     //血量提升10%
                        Fullhealth = (int)(1.1 * (float)Fullhealth);
                        Health = (int)(1.1 * (float)Health);
                        break;
                    case 2:     //血量提升20%
                        Fullhealth = (int)(1.2 * (float)Fullhealth);
                        Health = (int)(1.2 * (float)Health);
                        break;
                }
                break;
            case "5":   //祖巫
                switch (activeId)
                {
                    case 0:
                        break;
                    case 1:     //攻击提升10%
                        Force = (int)(1.1 * (float)Force);
                        break;
                    case 2:     //攻击提升20%
                        Force = (int)(1.2 * (float)Force);
                        break;
                }
                break;
            case "6":   //散仙
                switch (activeId)
                {
                    case 0:
                        break;
                    case 1:     //攻击提升10%
                        Force = (int)(1.1 * (float)Force);
                        break;
                    case 2:     //攻击提升20%
                        Force = (int)(1.2 * (float)Force);
                        break;
                }
                break;
            case "7":   //辅神
                switch (activeId)
                {
                    case 0:
                        break;
                    case 1:     //攻击提升10%
                        Force = (int)(1.1 * (float)Force);
                        break;
                    case 2:     //攻击提升20%
                        Force = (int)(1.2 * (float)Force);
                        break;
                }
                break;
            case "8":   //魔神
                switch (activeId)
                {
                    case 0:
                        break;
                    case 1:     //暴击率提升10%
                        CritRate = 1.1f * CritRate;
                        break;
                    case 2:     //暴击率提升20%
                        CritRate = 1.1f * CritRate;
                        break;
                }
                break;
            case "9":   //天神
                switch (activeId)
                {
                    case 0:
                        break;
                    case 1:     //攻击提升10%
                        Force = (int)(1.1 * (float)Force);
                        break;
                    case 2:     //攻击提升20%
                        Force = (int)(1.2 * (float)Force);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// 普通攻击效果
    /// </summary>
    private void NormalAttack(GameObject enemyobject)
    {
        //伤害计算
        realDamage = AttackTheEnemy(Force);
    }

    /// <summary>
    /// 计算技能攻击基础伤害
    /// </summary>
    /// <param name="force">原始攻击力</param>
    /// <param name="obj">对象</param>
    /// <param name="isZhiliao">是否是治疗</param>
    /// <returns></returns>
    private int SkillRealDamage(int force, GameObject obj, bool isZhiliao)
    {
        //计算是否敌方闪避（不是治疗情况）
        if (!isZhiliao && TakeSpecialAttack(obj.GetComponent<CardMove>().realDodgeRate))
        {
            Debug.Log("闪避");
            return 0;
        }
        //计算是否触发重击
        if (TakeSpecialAttack(ThumpRate))
        {
            Debug.Log("重击");
            force = (int)(force * ThumpDamage);
        }
        else
        {
            //计算是否触发暴击
            if (TakeSpecialAttack(CritRate))
            {
                Debug.Log("暴击");
                force = (int)(force * CritDamage);
            }
        }
        if (isZhiliao)
        {
            return force;
        }
        else
        {
            //添加破甲值的计算     攻击*（（70*2）/（70+防御*(1-破甲百分比)））
            return (int)(force * (140 / (70 + obj.GetComponent<CardMove>().Defence * (1 - ArmorPenetrationRate))));
        }
    }


    //攻击敌方武将，计算造成的战斗伤害数值
    private int AttackTheEnemy(int force)
    {
        int index_attack = 0;
        //计算是否敌方闪避
        if (TakeSpecialAttack(EnemyObj.GetComponent<CardMove>().realDodgeRate))
        {
            Debug.Log("闪避");
            EnemyObj.transform.GetChild(10).GetComponent<Text>().text = "闪避";
            EnemyObj.transform.GetChild(10).GetComponent<Text>().color=ColorData.blue_Color;
            EnemyObj.transform.GetChild(10).gameObject.SetActive(true);
            EnemyObj.transform.GetComponent<AudioSource>().clip = Resources.Load("Effect/FightSounds/闪避", typeof(AudioClip)) as AudioClip;
            EnemyObj.transform.GetComponent<AudioSource>().Play();
            return 0;
        }
        //计算是否触发重击
        if (TakeSpecialAttack(ThumpRate))
        {
            index_attack = 1;
            Debug.Log("重击");
            EnemyObj.transform.GetChild(10).GetComponent<Text>().text = "重击";
            EnemyObj.transform.GetChild(10).GetComponent<Text>().color=ColorData.red_Color;
            EnemyObj.transform.GetChild(10).gameObject.SetActive(true);
            force = (int)(force * ThumpDamage);
        }
        else
        {
            //计算是否触发暴击
            if (TakeSpecialAttack(CritRate))
            {
                index_attack = 2;
                Debug.Log("暴击");
                EnemyObj.transform.GetChild(10).GetComponent<Text>().text = "暴击";
                EnemyObj.transform.GetChild(10).GetComponent<Text>().color=ColorData.red_Color;
                EnemyObj.transform.GetChild(10).gameObject.SetActive(true);
                //anim.Play("fightCardStatus");
                force = (int)(force * CritDamage);
            }
        }
        if (ArmsSkillStatus > 0 && (ArmsId == "3" || ArmsId == "4"))
        {
            //飞兽和禁卫激活兵种技能不播放普通攻击特效
        }
        else
        {
            //攻击特效
            NormalAttackEffects(EnemyObj, index_attack);
        }
        //添加破甲值的计算     攻击*（（70*2）/（70+防御*(1-破甲百分比)））
        return (int)(force * (140 / (70 + EnemyObj.GetComponent<CardMove>().Defence * (1 - ArmorPenetrationRate))));
    }


    /// <summary>
    /// 计算是否触发特殊攻击状态
    /// </summary>
    /// <param name="odds">触发概率</param>
    /// <returns></returns>
    public bool TakeSpecialAttack(float odds)
    {
        int num = Random.Range(1, 101);  //随机取1-100中一个数
        if (num <= odds * 100)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 对对手造成伤害，动态表现等
    /// </summary>
    private void UpdateEnemyHp(GameObject obj)
    {
        //判断目标是否有坚盾状态
        if (obj.GetComponent<CardMove>().Fight_State.isWithStand)
        {
            obj.transform.GetChild(10).GetComponent<Text>().text = "抵挡";
            obj.transform.GetChild(10).GetComponent<Text>().color = ColorData.blue_Color;
            obj.transform.GetChild(10).gameObject.SetActive(true);
            obj.transform.GetComponent<AudioSource>().clip = Resources.Load("Effect/FightSounds/抵挡", typeof(AudioClip)) as AudioClip;
            obj.transform.GetComponent<AudioSource>().Play();
            obj.GetComponent<CardMove>().Fight_State.withStandNums--;
            if (obj.GetComponent<CardMove>().Fight_State.withStandNums <= 0)
            {
                obj.GetComponent<CardMove>().Fight_State.isWithStand = false;           //取消坚盾状态
                obj.GetComponent<CardMove>().Fight_State.withStandNums = 0;
                Destroy(obj.transform.GetChild(9).Find(StateName.standName).gameObject);//消除坚盾状态图标
            }
            realDamage = 0;
        }
        else
        {
            GameObject cutHpObj = Instantiate(cutHp_text, obj.transform.GetChild(5));
            cutHpObj.GetComponent<Text>().color = Color.red;
            cutHpObj.GetComponent<Text>().text = "-" + realDamage;
        }

        //敌方血条的计算和显示
        if (obj.GetComponent<CardMove>().Health > 0)
        {
            obj.GetComponent<CardMove>().Health = obj.GetComponent<CardMove>().Health - realDamage;
            if (obj.GetComponent<CardMove>().Health < 0)    //死亡
            {
                obj.GetComponent<CardMove>().Health = 0;
                if (IsPlayerOrEnemy == 0)
                {
                    DrumSkillControll.drumNums++;   //战鼓敲击次数加1
                    DrumSkillControll.isChange = true;
                }
            }
            obj.GetComponent<Slider>().value = 1 - (obj.GetComponent<CardMove>().Health) / ((float)obj.GetComponent<CardMove>().Fullhealth);
        }

        if (obj.GetComponent<CardMove>().ArmsId == "2" && obj.GetComponent<CardMove>().ArmsSkillStatus > 0) //如果攻击的敌人是海兽 并且 兵种激活
        {
            HaiShouSkill(obj, obj.GetComponent<CardMove>().ArmsSkillStatus);    //触发海兽技能
        }
        if (obj.GetComponent<CardMove>().ArmsId == "3" && obj.GetComponent<CardMove>().ArmsSkillStatus > 0) //如果攻击的敌人是飞兽 并且 兵种激活
        {
            if (obj.GetComponent<CardMove>().ArmsSkillStatus == 1)
            {
                //每损失10%血量提升5%闪避
                obj.GetComponent<CardMove>().FeiShouSkill(0.1f, 0.05f);
            }
            else
            {
                //每损失10%血量提升8%闪避
                obj.GetComponent<CardMove>().FeiShouSkill(0.1f, 0.08f);
            }
        }
    }

    /// <summary>
    /// 自身血条的计算和显示
    /// </summary>
    private void UpdateOwnHp(int addHp, GameObject obj)
    {
        GameObject cutHpObj = Instantiate(cutHp_text, obj.transform.GetChild(5));
        cutHpObj.GetComponent<Text>().color = ColorData.green_deep_Color;
        cutHpObj.GetComponent<Text>().text = "+" + addHp;
        
        //血条的显示
        obj.GetComponent<Slider>().value = 1 - obj.GetComponent<CardMove>().Health / (float)obj.GetComponent<CardMove>().Fullhealth;
    }

    /// <summary>
    /// 隐藏卡牌对象的状态文字等
    /// </summary>
    /// <param name="obj"></param>
    private void HideGameObjText(GameObject obj, bool boo)
    {
        obj.transform.GetChild(4).gameObject.SetActive(boo);
    }

    /// <summary>
    /// 普通攻击特效
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="index">0普通攻击1重击2暴击3穿刺4横扫5火攻6雷震7眩晕8受治疗9山兽10反伤11战意12飞兽</param>
    private void NormalAttackEffects(GameObject obj, int index)
    {
        //如果攻击的敌人是海兽 并且 兵种激活
        if (EnemyObj.GetComponent<CardMove>().ArmsId == "2" && EnemyObj.GetComponent<CardMove>().ArmsSkillStatus > 0 
            || index == 5
            || index == 6
            || index == 9
            || index == 11
            || EnemyObj.GetComponent<CardMove>().Fight_State.isWithStand){
        }
        else
        {
            //受攻击后的抖动（持续时间，力量，震动，随机性，淡出）
            obj.transform.DOShakePosition(0.2f, 15, 3, 50, true);
        }
        //特效播放
        switch (index)
        {
            case 0:
                //攻击音效播放
                audiosource.clip = Resources.Load("Effect/FightSounds/普通攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/普通攻击", typeof(GameObject)) as GameObject, obj.transform);
                break;
            case 1:
                audiosource.clip = Resources.Load("Effect/FightSounds/暴击攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/暴击", typeof(GameObject)) as GameObject, obj.transform);
                break;
            case 2:
                audiosource.clip = Resources.Load("Effect/FightSounds/暴击攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/暴击", typeof(GameObject)) as GameObject, obj.transform);
                break;
            case 3:
                audiosource.clip = Resources.Load("Effect/FightSounds/5枪兵攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                GameObject effect = Instantiate(Resources.Load("Prefab/fightEffect/5枪兵攻击", typeof(GameObject)) as GameObject, obj.transform);
                if (IsPlayerOrEnemy == 1)
                {
                    effect.transform.localEulerAngles += new Vector3(0, 0, 180);
                }
                break;
            case 4:
                audiosource.clip = Resources.Load("Effect/FightSounds/6戟兵攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/6戟兵攻击", typeof(GameObject)) as GameObject, obj.transform);
                break;
            case 5:
                audiosource.clip = Resources.Load("Effect/FightSounds/8统帅攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/8统帅攻击", typeof(GameObject)) as GameObject, obj.transform);
                break;
            case 6:
                audiosource.clip = Resources.Load("Effect/FightSounds/7谋士攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/7谋士攻击", typeof(GameObject)) as GameObject, obj.transform);
                break;
            case 7:
                GameObject xuanyunEffect = Instantiate(Resources.Load("Prefab/fightEffect/眩晕", typeof(GameObject)) as GameObject, obj.transform);
                xuanyunEffect.name = StateName.xuanyunEffect;
                break;
            case 8:
                audiosource.clip = Resources.Load("Effect/FightSounds/9方士攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/9方士攻击", typeof(GameObject)) as GameObject, obj.transform);
                break;
            case 9:
                audiosource.clip = Resources.Load("Effect/FightSounds/1虎卫攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/1虎卫攻击", typeof(GameObject)) as GameObject, obj.transform);
                break;
            case 10:
                audiosource.clip = Resources.Load("Effect/FightSounds/2陷阵攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                break;
            case 11:
                audiosource.clip = Resources.Load("Effect/FightSounds/4禁卫攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/4禁卫攻击", typeof(GameObject)) as GameObject, obj.transform);
                break;
            case 12:
                audiosource.clip = Resources.Load("Effect/FightSounds/3飞甲攻击", typeof(AudioClip)) as AudioClip;
                audiosource.Play();
                Instantiate(Resources.Load("Prefab/fightEffect/3飞甲攻击", typeof(GameObject)) as GameObject, obj.transform);
                break;
        }
    }

    /// <summary>
    /// 眩晕敌人
    /// </summary>
    /// <param name="obj"></param>
    private void DizzyEnemyCards(GameObject obj)
    {
        obj.GetComponent<CardMove>().IsDizzy = true;
        obj.transform.GetChild(10).GetComponent<Text>().text = "眩晕";
        obj.transform.GetChild(10).GetComponent<Text>().color = ColorData.red_Color;
        obj.transform.GetChild(10).gameObject.SetActive(true);
        if (!obj.GetComponent<CardMove>().Fight_State.isDizzy)
        {
            obj.GetComponent<CardMove>().Fight_State.isDizzy = true;
            GameObject icon = Instantiate(stateIcon, obj.transform.GetChild(9));
            icon.name = StateName.dizzyName;    //设置名字为眩晕状态名(用于销毁)
            icon.GetComponent<Image>().sprite = Resources.Load("Image/state/" + StateName.dizzyName, typeof(Sprite)) as Sprite;
            NormalAttackEffects(obj, 7);    //播放眩晕特效
        }
    }

    /// <summary>
    /// 解除眩晕状态
    /// </summary>
    /// <param name="obj"></param>
    private void ReleaseDizzyState(GameObject obj)
    {
        Destroy(obj.transform.GetChild(9).Find(StateName.dizzyName).gameObject);  //消除眩晕状态图标
        Destroy(obj.transform.Find(StateName.xuanyunEffect).gameObject);    //消除眩晕特效
        obj.GetComponent<CardMove>().Fight_State.isDizzy = false;    //接触眩晕
    }

    /// <summary>
    /// 连击技能
    /// </summary>
    private void BatterFightSkill()
    {
        transform.GetChild(10).GetComponent<Text>().text = "连击";
        transform.GetChild(10).GetComponent<Text>().color = ColorData.red_Color;
        transform.GetChild(10).gameObject.SetActive(true);
        Fight_State.batterNums--;
        if (Fight_State.batterNums <= 1)
        {
            Destroy(transform.GetChild(9).Find(StateName.batterName).gameObject);  //消除眩晕状态图标
            Fight_State.isBatter = false;
        }
        ChangeToFight(StateOfAttack.FightNow);  //切换此卡牌攻击状态再次为战斗
    }

    /// <summary>
    /// 火攻技能--全体造成30%伤害
    /// </summary>
    private void FireFightSkill()
    {
        transform.GetChild(10).GetComponent<Text>().text = "群攻";
        transform.GetChild(10).GetComponent<Text>().color = ColorData.red_Color;
        transform.GetChild(10).gameObject.SetActive(true);
        realDamage = (int)((float)Force * 0.3);
        if (Fight_State.isFireAttack == true)
        {
            for (int i = 0; i < 9; i++)
            {
                if (FightCardSP.enemyCards[i] != null && FightCardSP.enemyCards[i].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.enemyCards[i]);
                    NormalAttackEffects(FightCardSP.enemyCards[i], 5);
                }
            }
            Destroy(transform.GetChild(9).Find(StateName.fireAttackName).gameObject);  //消除火攻状态图标
            Fight_State.isFireAttack = false;
        }
    }
}
