using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMove : MonoBehaviour {

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

    private int isAttack;  //攻击状态,0备战,1攻击,2攻击结束


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


    Vector2 vec = new Vector2();    //记录卡牌初始位置

    private void Awake()
    {
        Health = 100;
        Force = 20;
        Defence = 5;
        isAttack = 0;
        vec = gameObject.transform.position;
    }

    public void ChangeAttackState() //更换攻击状态
    {
        if (Health>0)
        {
            isAttack = 1;
        }
    }

    private void AttackFun()    //攻击目标
    {
        Vector2.Lerp(gameObject.transform.position,enemyindex.transform.position,10);
        if (gameObject.transform.position == enemyindex.transform.position)
        {
            isAttack = 2;
            Debug.Log("攻击结束");
        }
    }

    private void BackToStartPos()
    {
        Vector2.Lerp(gameObject.transform.position, vec, 10);
    }

    private void Update()
    {
        if (isAttack==1 && enemyindex!=null)
        {
            AttackFun();    //开始攻击
        }
        if (isAttack==2)
        {
            isAttack = 0;
            Invoke("BackToStartPos", 0.3f); //停留一定时间后返回原位置
        }
    }
}
