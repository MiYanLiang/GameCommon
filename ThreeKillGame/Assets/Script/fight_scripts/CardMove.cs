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
        //设置兵种背景
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Image/ArmsPicture/" + ArmsId, typeof(Sprite)) as Sprite;
        //攻击力显示
        //transform.GetChild(6).GetComponent<Text>().text = Force.ToString();
        //防御力显示
        //transform.GetChild(7).GetComponent<Text>().text = Defence.ToString();
        //品阶显示
        //transform.GetChild(8).GetChild(0).GetComponent<Text>().text = Grade.ToString();
        //攻击音效
        audiosource = GetComponent<AudioSource>();
        realDodgeRate = DodgeRate;  //真实闪避率初始化
        ArmsStaticSkillGet(ArmsId, ArmsSkillStatus);   //静态兵种技能
    }

    /// <summary>
    /// 其他必要值的赋值
    /// </summary>
    public void OtherDataSet()
    {
        ChangeToFight(StateOfAttack.ReadyForFight);
        vec = gameObject.transform.position;
        realDamage = Force;
    }


    private void Update()
    {
        if (IsAttack == StateOfAttack.FightNow && EnemyObj != null)
        {
            if (!isCalcul)
            {
                if (EnemyObj.GetComponent<CardMove>().IsPlayerOrEnemy == 0)
                    flag = 1;
                else
                    flag = -1;
                //计算要攻击后移动到的位置，然后移动
                vec_Enemy = EnemyObj.transform.position + (flag * (new Vector3(0, 160, 0)));
                //武将先移动到目标身前
                gameObject.transform.DOMove(vec_Enemy, FightControll.speedTime).SetAutoKill(false);
                isCalcul = true;
            }

            //攻击目标，武将先移动到目标身上
            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, vec_Enemy, FightControll.speedTime * Time.deltaTime);
            if (gameObject.transform.position == vec_Enemy)
            {
                ArmsDynamicSkillGet(ArmsId, ArmsSkillStatus);   //攻击+动态技能

                //完成攻击武将移动到原始位置
                gameObject.transform.DOMove(vec, FightControll.speedTime).SetAutoKill(false);

                ChangeToFight(StateOfAttack.FightOver);
            }
        }
        if (IsAttack == StateOfAttack.FightOver)
        {
            isCalcul = false;
            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, vec, FightControll.speedTime * Time.deltaTime);
            if (gameObject.transform.position == vec && IsAttack!= StateOfAttack.ReadyForFight)
            {
                //隐藏对手的掉血值和状态
                HideGameObjText(EnemyObj, false);
                //隐藏自身的血值和状态
                HideGameObjText(gameObject, false);

                //anim.Stop("fightCardStatus");
                FightCardSP.isFightNow = false;
                ChangeToFight(StateOfAttack.ReadyForFight);
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
                        //将造成伤害的30%转化为自身血量--//减少受到10%远程伤害
                        ShanShouSkill(0.3f);
                        gameObject.transform.GetChild(4).GetComponent<Text>().text = "嗜血";
                        gameObject.transform.GetChild(4).gameObject.SetActive(true);
                        Debug.Log("嗜血");
                        break;
                    case 2:
                        //将造成伤害的60%转化为自身血量--//减少受到20%远程伤害
                        ShanShouSkill(0.6f);
                        gameObject.transform.GetChild(4).GetComponent<Text>().text = "吞噬";
                        gameObject.transform.GetChild(4).gameObject.SetActive(true);
                        Debug.Log("吞噬");
                        break;
                }
                break;

            case "2":   //海兽
                switch (activeId)
                {
                    case 0:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        break;
                    case 1:
                        //受伤害回复5%的血量--//反弹20%近战伤害。
                        HaiShouSkill(0.05f);
                        gameObject.transform.GetChild(4).GetComponent<Text>().text = "毒刺";
                        gameObject.transform.GetChild(4).gameObject.SetActive(true);
                        Debug.Log("毒刺");
                        break;
                    case 2:
                        //受伤害回复5%的血量--//反弹40%近战伤害。
                        HaiShouSkill(0.05f);
                        gameObject.transform.GetChild(4).GetComponent<Text>().text = "刃甲";
                        gameObject.transform.GetChild(4).gameObject.SetActive(true);
                        Debug.Log("刃甲");
                        break;
                }
                break;

            case "3":   //飞兽
                Debug.Log("飞兽闪避率： "+ realDodgeRate*100+"%");
                switch (activeId)
                {
                    case 0:
                        NormalAttack(EnemyObj);
                        UpdateEnemyHp(EnemyObj);
                        break;
                    case 1:     
                        //每损失20%血量提升10%闪避
                        FeiShouSkill(0.1f);
                        break;
                    case 2:     
                        //每损失20%血量提升15%闪避
                        FeiShouSkill(0.15f);
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
                        //随机攻击3个不同目标，每个造成30%伤害，20%几率击晕1回合。
                        FuShenSkill(3, 0.3f, 0.2f);
                        break;
                    case 2:
                        //随机攻击3个不同目标，每个造成30%伤害，30%几率击晕1回合。
                        FuShenSkill(3, 0.3f, 0.3f);
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
                        //随机攻击3个目标，每个造成45%伤害，对同一目标最多攻击2次。
                        MoShenSkill(0.45f, 3);
                        break;
                    case 2:
                        //随机攻击4个目标，每个造成45%伤害，对同一目标最多攻击2次。
                        MoShenSkill(0.45f, 4);
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
                        //治疗2个血量最低的友方目标，治疗量为伤害的80%。
                        TianShenSkill(2, 0.8f);
                        break;
                    case 2:
                        //治疗3个血量最低的友方目标，治疗量为伤害的100%。
                        TianShenSkill(3, 1f);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// 山兽动态技能
    /// </summary>
    /// <param name="percentage">转化血量百分比</param>
    private void ShanShouSkill(float percentage)
    {
        NormalAttack(EnemyObj);
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
        UpdateOwnHp(addHp, gameObject);
    }

    /// <summary>
    /// 海兽动态技能
    /// </summary>
    /// <param name="percentage">恢复血量百分比</param>
    private void HaiShouSkill(float percentage)
    {
        NormalAttack(EnemyObj);
        int addHp = (int)(Fullhealth * percentage);
        if (addHp + Health > Fullhealth) //当恢复血量加当前血量大于总血量时
        {
            Health = Fullhealth;
        }
        else
        {
            Health = Health + addHp;
        }
        UpdateOwnHp(addHp, gameObject);
    }

    /// <summary>
    /// 飞兽动态技能
    /// </summary>
    /// <param name="percentage">每次提升的闪避率</param>
    private void FeiShouSkill(float percentage)
    {
        float loseHp = (Fullhealth - Health) / (float)Fullhealth;
        int num = (int)(loseHp / 0.2);
        if (num > 0)
        {
            if (ArmsSkillStatus == 1)
            {
                gameObject.transform.GetChild(4).GetComponent<Text>().text = "疾风";
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                Debug.Log("疾风");
            }
            if (ArmsSkillStatus == 2)
            {
                gameObject.transform.GetChild(4).GetComponent<Text>().text = "瞬闪";
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                Debug.Log("瞬闪");
            }
            //每损失20%血量提升闪避率
            realDodgeRate = DodgeRate + (percentage * num);
        }
        NormalAttack(EnemyObj);
        UpdateEnemyHp(EnemyObj);
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
                gameObject.transform.GetChild(4).GetComponent<Text>().text = "战魂";
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                Debug.Log("战魂");
            }
        }
    }

    /// <summary>
    /// 祖巫动态技能
    /// </summary>
    /// <param name="percent">伤害百分比</param>
    private void ZuWuSkill(float percent)
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
        NormalAttack(EnemyObj);
        UpdateEnemyHp(EnemyObj);
        realDamage = (int)(realDamage * percent);
        if (IsPlayerOrEnemy == 0)
        {
            if (EnemyIndex == 0)
            {
                if (FightCardSP.enemyCards[3] != null && FightCardSP.enemyCards[3].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.enemyCards[3]);
                    NormalAttackEffects(FightCardSP.enemyCards[3]);
                }
            }
            else if (EnemyIndex == 1)
            {
                if (FightCardSP.enemyCards[4] != null && FightCardSP.enemyCards[4].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.enemyCards[4]);
                    NormalAttackEffects(FightCardSP.enemyCards[4]);
                }
            }
            else if (EnemyIndex == 2)
            {
                if (FightCardSP.enemyCards[5] != null && FightCardSP.enemyCards[5].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.enemyCards[5]);
                    NormalAttackEffects(FightCardSP.enemyCards[5]);
                }
            }
            else if (EnemyIndex == 3)
            {
                if (FightCardSP.enemyCards[6] != null && FightCardSP.enemyCards[6].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.enemyCards[6]);
                    NormalAttackEffects(FightCardSP.enemyCards[6]);
                }
            }
            else if (EnemyIndex == 4)
            {
                if (FightCardSP.enemyCards[7] != null && FightCardSP.enemyCards[7].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.enemyCards[7]);
                    NormalAttackEffects(FightCardSP.enemyCards[7]);
                }
            }
            else if (EnemyIndex == 5)
            {
                if (FightCardSP.enemyCards[8] != null && FightCardSP.enemyCards[8].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.enemyCards[8]);
                    NormalAttackEffects(FightCardSP.enemyCards[8]);
                }
            }
        }
        else
        {
            if (EnemyIndex == 0)
            {
                if (FightCardSP.playerCards[3] != null && FightCardSP.playerCards[3].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.playerCards[3]);
                    NormalAttackEffects(FightCardSP.playerCards[3]);
                }
            }
            else if (EnemyIndex == 1)
            {
                if (FightCardSP.playerCards[4] != null && FightCardSP.playerCards[4].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.playerCards[4]);
                    NormalAttackEffects(FightCardSP.playerCards[4]);
                }
            }
            else if (EnemyIndex == 2)
            {
                if (FightCardSP.playerCards[5] != null && FightCardSP.playerCards[5].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.playerCards[5]);
                    NormalAttackEffects(FightCardSP.playerCards[5]);
                }
            }
            else if (EnemyIndex == 3)
            {
                if (FightCardSP.playerCards[6] != null && FightCardSP.playerCards[6].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.playerCards[6]);
                    NormalAttackEffects(FightCardSP.playerCards[6]);
                }
            }
            else if (EnemyIndex == 4)
            {
                if (FightCardSP.playerCards[7] != null && FightCardSP.playerCards[7].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.playerCards[7]);
                    NormalAttackEffects(FightCardSP.playerCards[7]);
                }
            }
            else if (EnemyIndex == 5)
            {
                if (FightCardSP.playerCards[8] != null && FightCardSP.playerCards[8].GetComponent<CardMove>().Health > 0)
                {
                    UpdateEnemyHp(FightCardSP.playerCards[8]);
                    NormalAttackEffects(FightCardSP.playerCards[8]);
                }
            }
        }
    }

    /// <summary>
    /// 散仙动态技能
    /// </summary>
    /// <param name="percent">伤害百分比</param>
    private void SanXianSkill(float percent)
    {
        audiosource.Play();
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
        realDamage = (int)(realDamage * percent);
        if (EnemyIndex<3)   //前排
        {
            if (IsPlayerOrEnemy == 0) // 该卡牌是己方(playerCards)，在敌方(enemyCards)中找目标
            {
                for (int i = 0; i < 3; i++)
                {
                    if (FightCardSP.enemyCards[i] != null && FightCardSP.enemyCards[i].GetComponent<CardMove>().Health > 0)
                    {
                        UpdateEnemyHp(FightCardSP.enemyCards[i]);
                        NormalAttackEffects(FightCardSP.enemyCards[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (FightCardSP.playerCards[i] != null && FightCardSP.playerCards[i].GetComponent<CardMove>().Health > 0)
                    {
                        UpdateEnemyHp(FightCardSP.playerCards[i]);
                        NormalAttackEffects(FightCardSP.playerCards[i]);
                    }
                }
            }
        }
        else
        {
            if (EnemyIndex<6)   //中排
            {
                if (IsPlayerOrEnemy == 0) 
                {
                    for (int i = 3; i < 6; i++)
                    {
                        if (FightCardSP.enemyCards[i] != null && FightCardSP.enemyCards[i].GetComponent<CardMove>().Health > 0)
                        {
                            UpdateEnemyHp(FightCardSP.enemyCards[i]);
                            NormalAttackEffects(FightCardSP.enemyCards[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 3; i < 6; i++)
                    {
                        if (FightCardSP.playerCards[i] != null && FightCardSP.playerCards[i].GetComponent<CardMove>().Health > 0)
                        {
                            UpdateEnemyHp(FightCardSP.playerCards[i]);
                            NormalAttackEffects(FightCardSP.playerCards[i]);
                        }
                    }
                }
            }
            else    //后排
            {
                if (IsPlayerOrEnemy == 0)
                {
                    for (int i = 6; i < 9; i++)
                    {
                        if (FightCardSP.enemyCards[i] != null && FightCardSP.enemyCards[i].GetComponent<CardMove>().Health > 0)
                        {
                            UpdateEnemyHp(FightCardSP.enemyCards[i]);  
                            NormalAttackEffects(FightCardSP.enemyCards[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 6; i < 9; i++)
                    {
                        if (FightCardSP.playerCards[i] != null && FightCardSP.playerCards[i].GetComponent<CardMove>().Health > 0)
                        {
                            UpdateEnemyHp(FightCardSP.playerCards[i]);
                            NormalAttackEffects(FightCardSP.playerCards[i]);
                        }
                    }
                }
            }
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
        realDamage = (int)(realDamage * percentage);
        List<int> arrs = new List<int>();   //记录卡牌下标
        //随机攻击3个不同目标，每个造成30%伤害，20%几率击晕1回合。
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
                    UpdateEnemyHp(FightCardSP.enemyCards[arr_index[i]]);
                    if (TakeSpecialAttack(stunProbability)) //判断是否可以眩晕对手
                    {
                        FightCardSP.enemyCards[arr_index[i]].GetComponent<CardMove>().IsDizzy = true;
                        FightCardSP.enemyCards[arr_index[i]].transform.GetChild(4).GetComponent<Text>().text = "眩晕";
                        FightCardSP.enemyCards[arr_index[i]].transform.GetChild(4).gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < arrs.Count; i++)
                {
                    UpdateEnemyHp(FightCardSP.enemyCards[arrs[i]]);
                    if (TakeSpecialAttack(stunProbability)) //判断是否触发眩晕
                    {
                        FightCardSP.enemyCards[arrs[i]].GetComponent<CardMove>().IsDizzy = true;
                        FightCardSP.enemyCards[arrs[i]].transform.GetChild(4).GetComponent<Text>().text = "眩晕";
                        FightCardSP.enemyCards[arrs[i]].transform.GetChild(4).gameObject.SetActive(true);
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
                    UpdateEnemyHp(FightCardSP.playerCards[arr_index[i]]);
                    if (TakeSpecialAttack(stunProbability))
                    {
                        FightCardSP.playerCards[arr_index[i]].GetComponent<CardMove>().IsDizzy = true;
                        FightCardSP.playerCards[arr_index[i]].transform.GetChild(4).GetComponent<Text>().text = "眩晕";
                        FightCardSP.playerCards[arr_index[i]].transform.GetChild(4).gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < arrs.Count; i++)
                {
                    UpdateEnemyHp(FightCardSP.playerCards[arrs[i]]);
                    if (TakeSpecialAttack(stunProbability)) //判断是否触发眩晕
                    {
                        FightCardSP.playerCards[arrs[i]].GetComponent<CardMove>().IsDizzy = true;
                        FightCardSP.playerCards[arrs[i]].transform.GetChild(4).GetComponent<Text>().text = "眩晕";
                        FightCardSP.playerCards[arrs[i]].transform.GetChild(4).gameObject.SetActive(true);
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
    private void MoShenSkill(float percent,int attackNum)
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
        realDamage = (int)(realDamage * percent);
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
                UpdateEnemyHp(FightCardSP.enemyCards[array_transform[i]]);
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
            int rondomNum = 0;
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
                UpdateEnemyHp(FightCardSP.playerCards[array_transform[i]]);
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
        int[] minHp = new int[nums];    //临时存储最少hp
        int[] arrs = new int[nums];     //存储受治疗单位的index
        bool isContinue = true;         //记录是否继续
        for (int i = 0; i < arrs.Length; i++)
        {
            arrs[i] = -1;   //初始化
            minHp[i] = 9999;
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
                        if (arrs[k]==j)
                        {
                            isContinue = false;
                        }
                    }
                    if (isContinue && FightCardSP.playerCards[j] != null && FightCardSP.playerCards[j].GetComponent<CardMove>().Health > 0)
                    {
                        if (minHp[i] > FightCardSP.playerCards[j].GetComponent<CardMove>().Health)
                        {
                            minHp[i] = FightCardSP.playerCards[j].GetComponent<CardMove>().Health;
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
                    UpdateOwnHp(addHp, FightCardSP.playerCards[arrs[i]]);
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
                        if (minHp[i] > FightCardSP.enemyCards[j].GetComponent<CardMove>().Health)
                        {
                            minHp[i] = FightCardSP.enemyCards[j].GetComponent<CardMove>().Health;
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
                    UpdateOwnHp(addHp, FightCardSP.enemyCards[arrs[i]]);
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
                //某数据加成
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
        //攻击音效播放
        audiosource.Play();
        //普通攻击特效展示
        NormalAttackEffects(enemyobject);
        //伤害计算
        realDamage = AttackTheEnemy(Force);
    }
    
    //攻击敌方武将，计算造成的战斗伤害数值
    private int AttackTheEnemy(int force)
    {
        //计算是否敌方闪避
        if (TakeSpecialAttack(EnemyObj.GetComponent<CardMove>().realDodgeRate))
        {
            Debug.Log("闪避");
            EnemyObj.transform.GetChild(4).GetComponent<Text>().text = "闪避";
            EnemyObj.transform.GetChild(4).gameObject.SetActive(true);
            //anim.Play("fightCardStatus");
            return 0;
        }
        //计算是否触发重击
        if (TakeSpecialAttack(ThumpRate))
        {
            Debug.Log("重击");
            EnemyObj.transform.GetChild(4).GetComponent<Text>().text = "重击";
            EnemyObj.transform.GetChild(4).gameObject.SetActive(true);
            //anim.Play("fightCardStatus");
            force = (int)(force * ThumpDamage);
        }
        else
        {
            //计算是否触发暴击
            if (TakeSpecialAttack(CritRate))
            {
                Debug.Log("暴击");
                EnemyObj.transform.GetChild(4).GetComponent<Text>().text = "暴击";
                EnemyObj.transform.GetChild(4).gameObject.SetActive(true);
                //anim.Play("fightCardStatus");
                force = (int)(force * CritDamage);
            }
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
        //显示造成伤害值
        obj.transform.GetChild(5).GetComponent<Text>().color = Color.red;
        obj.transform.GetChild(5).GetComponent<Text>().text = "-" + realDamage;
        obj.transform.GetChild(5).gameObject.SetActive(true);

        //敌方血条的计算和显示
        obj.GetComponent<CardMove>().Health = obj.GetComponent<CardMove>().Health - realDamage;
        if (obj.GetComponent<CardMove>().Health < 0)
            obj.GetComponent<CardMove>().Health = 0;
        obj.GetComponent<Slider>().value = 1 - (obj.GetComponent<CardMove>().Health) / ((float)obj.GetComponent<CardMove>().Fullhealth);
    }

    /// <summary>
    /// 自身血条的计算和显示
    /// </summary>
    private void UpdateOwnHp(int addHp, GameObject obj)
    {
        //显示恢复值
        obj.transform.GetChild(5).GetComponent<Text>().color = Color.green;
        obj.transform.GetChild(5).GetComponent<Text>().text = "+" + addHp;
        obj.transform.GetChild(5).gameObject.SetActive(true);
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
        obj.transform.GetChild(5).gameObject.SetActive(boo);
    }

    /// <summary>
    /// 普通攻击特效
    /// </summary>
    /// <param name="obj"></param>
    private void NormalAttackEffects(GameObject obj)
    {
        //受攻击后的抖动（持续时间，力量，震动，随机性，淡出）
        obj.transform.DOShakePosition(0.2f, 15, 3, 50, true);
        //特效播放
        Instantiate(Resources.Load("Prefab/fightEffect/putonggongji", typeof(GameObject)) as GameObject, obj.transform);
    }

}
