using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库
using System;

public class UIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Text gold;       //玩家金币
    public CreateAndUpdate cau;

    [SerializeField]
    Image playerForcePic;    //玩家势力头像
    private int playerForceId;  //玩家势力ID
    [SerializeField]
    Image[] otherForcePic;    //其他势力头像
    int[] array_forces = { 0, 0, 0, 0, 0 };   //其他势力id

    private void Awake()
    {
        int id_others = 0;
        playerForceId = PlayerPrefs.GetInt("forcesId");
        playerForcePic.sprite = Resources.Load("Image/calligraphy/" + playerForceId, typeof(Sprite)) as Sprite; //设置玩家势力的头像
        for (int i = 0; i < 5; i++)
        {
            //创建其他势力ID
            do
            {
                id_others = UnityEngine.Random.Range(1, 12);
            } while ((id_others==playerForceId)||IsHadForceId(id_others));
            array_forces[i] = id_others;
            //设置其他势力的头像
            otherForcePic[i].sprite = Resources.Load("Image/calligraphy/" + array_forces[i], typeof(Sprite)) as Sprite; 
        }

    }

    //判断是否已有这个势力
    private bool IsHadForceId(int id_others)
    {
        for (int i = 0; i < array_forces.Length; i++)
        {
            if (id_others==array_forces[i])
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        gold.text = "金：" + cau.money;
    }
}
