using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrumSkillControll : MonoBehaviour
{
    public GameObject stateIcon;    //卡牌上状态小标预制件
    public Text drumText;           //战鼓数量显示
    public static int drumNums;     //可敲击次数

    private void OnEnable()
    {
        drumNums = 3;
        UpdateShowDrumText();   //刷新敲鼓次数显示
    }


    //战鼓技能--------------------------------------------

    /// <summary>
    /// 战鼓（风）技能
    /// 让暴击最高的单位获得一次连击			
    /// </summary>
    public void WindDrumSkill()
    {
        if (drumNums <= 0)
        {
            Debug.Log("战鼓敲击次数不足");
            return;
        }
        int index = -1;
        for (int i = 0; i < 9; i++)
        {
            if (FightCardSP.playerCards[i] != null && FightCardSP.playerCards[i].GetComponent<CardMove>().Health > 0)
            {
                if (!FightCardSP.playerCards[i].GetComponent<CardMove>().Fight_State.isBatter)  //没有连击状态
                {
                    //判断还没有记录过目标，或者，i位置的暴击率大于记录位置的暴击率
                    if (index == -1 || FightCardSP.playerCards[i].GetComponent<CardMove>().CritRate > FightCardSP.playerCards[index].GetComponent<CardMove>().CritRate)
                    {
                        index = i;
                    }
                }
            }
        }
        if (index == -1)
            return;
        //显示技能文字
        FightCardSP.playerCards[index].transform.GetChild(4).GetComponent<Text>().text = "连击";
        FightCardSP.playerCards[index].transform.GetChild(4).gameObject.SetActive(true);
        FightCardSP.playerCards[index].GetComponent<CardMove>().Fight_State.isBatter = true;
        FightCardSP.playerCards[index].GetComponent<CardMove>().Fight_State.batterNums = 2; //连击两次
        GameObject icon = Instantiate(stateIcon, FightCardSP.playerCards[index].transform.GetChild(9));
        icon.name = StateName.batterName;
        icon.GetComponent<Image>().sprite = Resources.Load("Image/state/" + StateName.batterName, typeof(Sprite)) as Sprite;
        drumNums--;
        UpdateShowDrumText();
    }

    /// <summary>
    /// 战鼓（雷）技能
    /// </summary>
    public void ThunderDrumSkill()
    {

    }

    /// <summary>
    /// 战鼓（水）技能
    /// </summary>
    public void WaterDrumSkill()
    {

    }

    /// <summary>
    /// 战鼓（火）技能
    /// </summary>
    public void FireDrumSkill()
    {

    }

    /// <summary>
    /// 战鼓（土）技能     
    /// 血量最低的单位加一个盾，免疫2次攻击
    /// </summary>
    public void DustDrumSkill()
    {
        if (drumNums<=0)
        {
            Debug.Log("战鼓敲击次数不足");
            return;
        }
        int index = -1; //记录目标下标
        for (int i = 0; i < 9; i++)
        {     
            if (FightCardSP.playerCards[i]!=null && FightCardSP.playerCards[i].GetComponent<CardMove>().Health>0 )
            {
                //没有坚盾状态
                if (!FightCardSP.playerCards[i].GetComponent<CardMove>().Fight_State.isWithStand)
                {
                    //判断还没有记录过目标，或者，i位置的血量少于记录位置的血量
                    if (index == -1 || FightCardSP.playerCards[i].GetComponent<CardMove>().Health < FightCardSP.playerCards[index].GetComponent<CardMove>().Health)
                    {
                        index = i;
                    }
                }
            }
        }
        if (index == -1)
            return;
        //显示技能文字
        FightCardSP.playerCards[index].transform.GetChild(4).GetComponent<Text>().text = "坚盾";
        FightCardSP.playerCards[index].transform.GetChild(4).gameObject.SetActive(true);
        //改变 目标的攻击状态 坚盾激活状态
        FightCardSP.playerCards[index].GetComponent<CardMove>().Fight_State.isWithStand = true;
        //设置坚盾层数为2
        FightCardSP.playerCards[index].GetComponent<CardMove>().Fight_State.withStandNums = 2;
        //实例化状态图标预制件(设置显示到目标身上)
        GameObject icon = Instantiate(stateIcon, FightCardSP.playerCards[index].transform.GetChild(9));
        //设置状态图标的name，用于之后查找销毁
        icon.name = StateName.standName;
        //加载状态图片资源
        icon.GetComponent<Image>().sprite = Resources.Load("Image/state/" + StateName.standName, typeof(Sprite)) as Sprite;
        //战鼓可敲击次数减一
        drumNums--;
        //次数显示刷新
        UpdateShowDrumText();
    }



    //战鼓技能end-----------------------------------------

    /// <summary>
    /// 战鼓数量刷新
    /// </summary>
    public void UpdateShowDrumText()
    {
        drumText.text = drumNums + "";
    }

}
