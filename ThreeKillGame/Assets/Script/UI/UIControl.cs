using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Text gold;          //玩家金币

    [SerializeField]
    Image playerForce_main;     //玩家势力头像主城
    [SerializeField]
    Transform playerForce;      //玩家势力头像
    [HideInInspector]
    public static int playerForceId;  //玩家势力ID
    [SerializeField]
    Transform[] otherForce;     //其他势力头像
    [HideInInspector]
    public static int[] array_forces = { 0, 0, 0, 0, 0 };   //其他势力id
    [SerializeField]
    Button fightBtn;
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

    private void Awake()
    {
        Invoke("OpenPlayFightBtn", 0.5f);

        //加载声音
        int soundStades = PlayerPrefs.GetInt("soundStates");
        soundContrll_(soundStades);
    }

    private void Start()
    {
        initForces();
        battleId = PlayerPrefs.GetInt("battleId");
        battle_Text.text = "公元" + LoadJsonFile.BattleTableDates[battleId][4] + "年";
    }

    /// <summary>
    /// 初始化势力表现
    /// </summary>
    private void initForces()
    {
        int id_others = 0;
        playerForceId = PlayerPrefs.GetInt("forcesId");
        playerForce_main.sprite = Resources.Load("Image/calligraphy/Forces/" + playerForceId, typeof(Sprite)) as Sprite; //设置玩家势力的头像
        playerForce.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/Forces/" + playerForceId, typeof(Sprite)) as Sprite; //设置玩家势力的头像
        playerForce.GetComponent<Image>().sprite = Resources.Load("Image/Map/" + LoadJsonFile.forcesTableDatas[playerForceId - 1][7], typeof(Sprite)) as Sprite;   //设置城池icon
        playerForce.position = cityPos.GetChild(int.Parse(LoadJsonFile.forcesTableDatas[playerForceId - 1][6])).position;   //城市位置设置
        string str = "";
        int forceCount = LoadJsonFile.forcesTableDatas.Count + 1;
        for (int i = 0; i < 5; i++)
        {
            //创建其他势力ID
            do
            {
                id_others = Random.Range(1, forceCount);
            } while ((id_others == playerForceId) || IsHadForceId(id_others));
            array_forces[i] = id_others;
            //设置其他势力的头像
            otherForce[i].GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/Forces/" + array_forces[i], typeof(Sprite)) as Sprite;
            otherForce[i].GetComponent<Image>().sprite = Resources.Load("Image/Map/" + LoadJsonFile.forcesTableDatas[array_forces[i] - 1][7], typeof(Sprite)) as Sprite;   //设置城池icon
            otherForce[i].position = cityPos.GetChild(int.Parse(LoadJsonFile.forcesTableDatas[array_forces[i] - 1][6])).position;   //城市位置设置
            str += array_forces[i] + LoadJsonFile.forcesTableDatas[array_forces[i] - 1][5] + "; ";
        }
        Debug.Log("npc势力ID: " + str);
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

    //设置进攻防守战鼓的image
    public void setTextContent(int index)   //-1守，0-5攻
    {
        if (index<0)
        {
            fightOrDefend.sprite = Resources.Load("Effect/UI/toBattle/守", typeof(Sprite)) as Sprite;
            selectForceIamge.position = playerForce.position;
        }
        else
        {
            fightOrDefend.sprite = Resources.Load("Effect/UI/toBattle/攻", typeof(Sprite)) as Sprite;
            selectForceIamge.position = otherForce[index].position;
        }
    }

    private void LateUpdate()
    {
        gold.text = CreateAndUpdate.money + "";   //玩家金钱显示
    }
}