using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Text gold;          //玩家金币

    [SerializeField]
    Image playerForcePic;      //玩家势力头像
    [HideInInspector]
    public static int playerForceId;  //玩家势力ID
    [SerializeField]
    Image[] otherForcePic;     //其他势力头像
    [HideInInspector]
    public static int[] array_forces = { 0, 0, 0, 0, 0 };   //其他势力id
    [SerializeField]
    Button fightBtn;
    public GameObject cameraAudio;//摄像机

    private int battleId;   //开局所选战役ID
    [SerializeField]
    public Text battle_Text;

    private void Awake()
    {
        int id_others = 0;
        playerForceId = PlayerPrefs.GetInt("forcesId");
        playerForcePic.sprite = Resources.Load("Image/calligraphy/Forces/" + playerForceId, typeof(Sprite)) as Sprite; //设置玩家势力的头像
        for (int i = 0; i < 5; i++)
        {
            //创建其他势力ID
            do
            {
                id_others = Random.Range(1, 12);
            } while ((id_others==playerForceId)||IsHadForceId(id_others));
            array_forces[i] = id_others;
            //设置其他势力的头像
            otherForcePic[i].sprite = Resources.Load("Image/calligraphy/Forces/" + array_forces[i], typeof(Sprite)) as Sprite; 
        }
        Invoke("OpenPlayFightBtn", 0.5f);

        //加载声音
        int soundStades = PlayerPrefs.GetInt("soundStates");
        soundContrll_(soundStades);
    }

    private void Start()
    {
        battleId = PlayerPrefs.GetInt("battleId");
        battle_Text.text = "公元" + LoadJsonFile.BattleTableDates[battleId][4] + "年";
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

    // Update is called once per frame
    void Update()
    {
        gold.text = CreateAndUpdate.money + "";   //玩家金钱显示
    }
}