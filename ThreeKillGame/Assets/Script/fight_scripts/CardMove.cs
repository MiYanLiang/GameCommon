using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StateOfAttack
{
    ReadyForFight,  //0备战
    FightNow,       //1攻击
    FightOver       //2攻击结束
}

public class CardMove : MonoBehaviour
{
    //private int moveSpeed = 500;    //卡牌移动速度
    
    private int realDamage; //造成的真实伤害

    public bool isFightInThisBout; //当前回合是否进行过攻击

    private string armsId; //武将兵种
    public string ArmsId { get => armsId; set => armsId = value; }

    private int fightNums;  //参与战斗周目数
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

    private GameObject enemyindex; //要攻击的敌人
    public GameObject Enemyindex { get => enemyindex; set => enemyindex = value; }

    private bool isAttack_first;    //是否为先手
    public bool IsAttack_first { get => isAttack_first; set => isAttack_first = value; }

    private float dodgeRate;    //闪避率
    public float DodgeRate { get => dodgeRate; set => dodgeRate = value; }

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

    Vector3 vec = new Vector3();    //记录卡牌初始位置

    Animator anim_Emey; //敌方动画控制器

    //Animation anim = new Animation();

    private void Awake()
    {
        isFightInThisBout = false;
        FightNums = 0;
    }

    private void Start()
    {
        //设置兵种背景
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Image/ArmsPicture/" + ArmsId, typeof(Sprite)) as Sprite;
        //攻击力显示
        transform.GetChild(6).GetComponent<Text>().text = Force.ToString();
        //防御力显示
        transform.GetChild(7).GetComponent<Text>().text = Defence.ToString();
        //品阶显示
        transform.GetChild(8).GetChild(0).GetComponent<Text>().text = Grade.ToString();
    }


    /// <summary>
    /// 其他必要值的赋值
    /// </summary>
    public void OtherDataSet()
    {
        ChangeToFight(StateOfAttack.ReadyForFight);
        vec = gameObject.transform.position;
        realDamage = Force;
        //Debug.Log("//vec//" + vec);
        //Debug.Log("//force//" + force);
    }

    int flag = 1;   //记录攻击的对手是玩家还是敌方
    bool isCalcul = false;  //是否计算过攻击位置
    Vector3 vec_Enemy;  //记录攻击位置

    private void Update()
    {
        if (IsAttack == StateOfAttack.FightNow && Enemyindex != null)
        {
            if (!isCalcul)
            {
                if (Enemyindex.transform.position.y > 1000)
                    flag = -1;
                else
                    flag = 1;
                //计算要攻击后移动到的位置
                vec_Enemy = Enemyindex.transform.position + (flag * (new Vector3(0, 160, 0)));
                isCalcul = true;
                anim_Emey = Enemyindex.GetComponent<Animator>();
            }
            
            //攻击目标，武将先移动到目标身上
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, vec_Enemy, FightControll.moveSpeed * Time.deltaTime);
            if (gameObject.transform.position == vec_Enemy)
            {

                realDamage = AttackTheEnemy(Force);   //得到造成的真实伤害
                if (realDamage > 0) //显示造成伤害值
                {
                    enemyindex.transform.GetChild(5).GetComponent<Text>().text = "-" + realDamage;
                    enemyindex.transform.GetChild(5).gameObject.SetActive(true);
                }
                
                //敌方血条的计算和显示
                Enemyindex.GetComponent<CardMove>().Health = Enemyindex.GetComponent<CardMove>().Health - realDamage;
                if (Enemyindex.GetComponent<CardMove>().Health < 0)
                    Enemyindex.GetComponent<CardMove>().Health = 0;
                Enemyindex.GetComponent<Slider>().value = ((float)Enemyindex.GetComponent<CardMove>().Health) / ((float)Enemyindex.GetComponent<CardMove>().Fullhealth);
            }
        }
        if (IsAttack == StateOfAttack.FightOver)
        {

            if (flag == -1)
                anim_Emey.SetTrigger("heroFightCardShake1");
            else
                anim_Emey.SetTrigger("heroFightCardShake");

            isCalcul = false;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, vec, FightControll.moveSpeed * Time.deltaTime);
            if (gameObject.transform.position == vec && IsAttack!= StateOfAttack.ReadyForFight)
            {
                enemyindex.transform.GetChild(4).gameObject.SetActive(false);
                enemyindex.transform.GetChild(5).gameObject.SetActive(false);
                //anim.Stop("fightCardStatus");
                FightCardSP.isFightNow = false;
                ChangeToFight(StateOfAttack.ReadyForFight);
            }
        }
    }

    //攻击敌方武将，计算造成的战斗伤害数值
    private int AttackTheEnemy(int force)
    {
        ChangeToFight(StateOfAttack.FightOver);
        //计算是否敌方闪避
        if (TakeSpecialAttack(Enemyindex.GetComponent<CardMove>().DodgeRate))
        {
            Debug.Log("闪避");
            Enemyindex.transform.GetChild(4).GetComponent<Text>().text = "闪避";
            enemyindex.transform.GetChild(4).gameObject.SetActive(true);
            //anim.Play("fightCardStatus");
            return 0;
        }
        //计算是否触发重击
        if (TakeSpecialAttack(ThumpRate))
        {
            Debug.Log("重击");
            Enemyindex.transform.GetChild(4).GetComponent<Text>().text = "重击";
            enemyindex.transform.GetChild(4).gameObject.SetActive(true);
            //anim.Play("fightCardStatus");
            force = (int)(force * ThumpDamage);
        }
        else
        {
            //计算是否触发暴击
            if (TakeSpecialAttack(CritRate))
            {
                Debug.Log("暴击");
                Enemyindex.transform.GetChild(4).GetComponent<Text>().text = "暴击";
                enemyindex.transform.GetChild(4).gameObject.SetActive(true);
                //anim.Play("fightCardStatus");
                force = (int)(force * CritDamage);
            }
        }

        //添加破甲值的计算     攻击*（（70*2）/（70+防御*(1-破甲百分比)））
        return (int)(force * (140 / (70 + Enemyindex.GetComponent<CardMove>().Defence * (1 - ArmorPenetrationRate))));
    }


    //计算是否触发特殊攻击状态
    public bool TakeSpecialAttack(float odds)
    {
        int num = Random.Range(1, 101);  //随机取1-100中一个数
        if (num <= odds * 100)
            return true;
        else
            return false;
    }
}
