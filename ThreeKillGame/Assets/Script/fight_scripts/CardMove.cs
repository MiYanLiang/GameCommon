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
    private int moveSpeed = 500;    //卡牌移动速度
    
    private int realDamage; //造成的真实伤害

    public bool isFightInThisBout; //当前回合是否进行过攻击

    private int fullhealth;//满血量
    public int Fullhealth { get => fullhealth; set => fullhealth = value; }

    private int health; //血量
    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    private int defence;//防御
    public int Defence
    {
        get
        {
            return defence;
        }

        set
        {
            defence = value;
        }
    }

    private int force;  //攻击力
    public int Force
    {
        get
        {
            return force;
        }

        set
        {
            force = value;
        }
    }

    private StateOfAttack isAttack;  //记录武将的攻击状态
    public StateOfAttack IsAttack
    {
        get
        {
            return isAttack;
        }
        set
        {
            isAttack = value;
        }
    }
    //更换攻击状态
    public void ChangeToFight(StateOfAttack state)
    {
        IsAttack = state;
    }

    private GameObject enemyindex; //要攻击的敌人
    public GameObject Enemyindex
    {
        get
        {
            return enemyindex;
        }

        set
        {
            enemyindex = value;
        }
    }

    private bool isAttack_first;    //是否为先手
    public bool IsAttack_first
    {
        get
        {
            return isAttack_first;
        }
        set
        {
            isAttack_first = value;
        }
    }

    private float dodgeRate;    //闪避率
    public float DodgeRate
    {
        get
        {
            return dodgeRate;
        }

        set
        {
            dodgeRate = value;
        }
    }

    private float thumpRate;    //重击率
    public float ThumpRate
    {
        get
        {
            return thumpRate;
        }

        set
        {
            thumpRate = value;
        }
    }

    private float thumpDamage;  //重击伤害
    public float ThumpDamage
    {
        get
        {
            return thumpDamage;
        }

        set
        {
            thumpDamage = value;
        }
    }

    private float critRate; //暴击率
    public float CritRate
    {
        get
        {
            return critRate;
        }

        set
        {
            critRate = value;
        }
    }

    private float critDamage;   //暴击伤害
    public float CritDamage
    {
        get
        {
            return critDamage;
        }

        set
        {
            critDamage = value;
        }
    }
    
    private float armorPenetrationRate; //破甲百分比
    public float ArmorPenetrationRate
    {
        get
        {
            return armorPenetrationRate;
        }

        set
        {
            armorPenetrationRate = value;
        }
    }


    Vector3 vec = new Vector3();    //记录卡牌初始位置

    private void Awake()
    {
        isFightInThisBout = false;
        Health = 100;
        Fullhealth = health;
        Force = 60;
        Defence = 5;
        ChangeToFight(StateOfAttack.ReadyForFight);
        vec = gameObject.transform.position;
        realDamage = force;
    }

    private void Update()
    {
        if (IsAttack == StateOfAttack.FightNow && enemyindex != null)
        {
            //攻击目标，武将先移动到目标身上
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, enemyindex.transform.position, moveSpeed * Time.deltaTime);
            if (gameObject.transform.position == enemyindex.transform.position)
            {
                realDamage = AttackTheEnemy(force);   //得到造成的真实伤害

                //敌方血条的计算和显示
                enemyindex.GetComponent<CardMove>().Health = enemyindex.GetComponent<CardMove>().Health - realDamage;
                if (enemyindex.GetComponent<CardMove>().Health < 0)
                    enemyindex.GetComponent<CardMove>().Health = 0;
                enemyindex.GetComponent<Slider>().value = ((float)enemyindex.GetComponent<CardMove>().Health) / ((float)enemyindex.GetComponent<CardMove>().Fullhealth);
            }
        }
        if (IsAttack == StateOfAttack.FightOver)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, vec, moveSpeed * Time.deltaTime);
            if (gameObject.transform.position == vec)
            {
                FightCardSP.isFightNow = false;
                ChangeToFight(0);
            }
        }
    }

    //攻击敌方武将，计算造成的战斗伤害数值
    private int AttackTheEnemy(int force)
    {
        ChangeToFight(StateOfAttack.FightOver);
        //计算是否敌方闪避
        if (TakeSpecialAttack(enemyindex.GetComponent<CardMove>().dodgeRate))
        {
            Debug.Log("闪避");
            return 0;
        }
        //计算是否触发重击
        if (TakeSpecialAttack(thumpRate))
        {
            Debug.Log("重击");
            force = (int)(force * thumpDamage);
        }
        else
        {
            //计算是否触发暴击
            if (TakeSpecialAttack(critRate))
            {
                Debug.Log("暴击");
                force = (int)(force * critDamage);
            }
        }

        //添加破甲值的计算
        return force - (int)(enemyindex.GetComponent<CardMove>().defence * (1 - armorPenetrationRate));
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
