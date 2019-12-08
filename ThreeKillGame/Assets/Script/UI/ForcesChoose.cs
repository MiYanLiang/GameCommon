using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ForcesChoose : MonoBehaviour
{
    [SerializeField]
    Text prestigeText;    //声望值显示
    [SerializeField]
    Text battleName;      //战役名显示文本
    [SerializeField]
    Text forcesText;      //介绍文本

    [SerializeField]
    GameObject forceObj;    //势力图标预制件
    [SerializeField]
    Transform cityTran;     //城市坐标
    [SerializeField]
    Transform selectIcon;   //选择图标
    [SerializeField]
    Transform forcesTran;   //势力集合
    [SerializeField]
    Transform prompTran;    //提示
    [SerializeField]
    private GameObject tipsText;    //提示text预制件

    public GameObject cameraAudio;

    [SerializeField]
    private float playTextSpeed = 3f;

    public static int battleId;     //所选战役id
    public static int playerForceId;//玩家选择势力id

    private int prestigeNum;    //声望值记录
    void Start()
    {
        prestigeNum = PlayerPrefs.GetInt("prestigeNum");
        prestigeText.text = prestigeNum.ToString();
        soundContrll_(PlayerPrefs.GetInt("soundStates"));
        RandomBattle();
    }

    /// <summary>
    /// 随机战役
    /// </summary>
    public void RandomBattle()
    {
        for (int i = 0; i < forcesTran.childCount; i++)
        {
            Destroy(forcesTran.GetChild(i).gameObject);
        }

        int battleIndex = battleId;
        while (battleIndex == battleId) //随机出不同战役
        {
            battleIndex = Random.Range(0, LoadJsonFile.BattleTableDates.Count);
        }
        battleId = battleIndex;

        battleName.text = LoadJsonFile.BattleTableDates[battleId][1];  //战役名称
        ShowTextOfForcesData(LoadJsonFile.BattleTableDates[battleId][2]); //战役解释
        string[] forcesStr = LoadJsonFile.BattleTableDates[battleId][3].Split(',');  //战役势力集合
        string[] forcesPos = LoadJsonFile.BattleTableDates[battleId][5].Split(',');  //战役势力位置集合

        for (int i = 0; i < forcesStr.Length; i++)  //默认选择势力
        {
            if (int.Parse(LoadJsonFile.forcesTableDatas[int.Parse(forcesStr[i]) - 1][3]) <= prestigeNum)
            {
                playerForceId = int.Parse(forcesStr[i]);
                selectIcon.position = cityTran.GetChild(int.Parse(forcesPos[i])).position;
            }
        }

        for (int i = 0; i < forcesStr.Length; i++)
        {
            int forceId = int.Parse(forcesStr[i]);
            GameObject force = Instantiate(forceObj, forcesTran);
            force.transform.position = cityTran.GetChild(int.Parse(forcesPos[i])).position;
            force.GetComponent<Image>().sprite = Resources.Load("Image/Map/" + LoadJsonFile.forcesTableDatas[forceId - 1][7], typeof(Sprite)) as Sprite;
            force.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/Forces/" + forceId, typeof(Sprite)) as Sprite;
            force.GetComponent<Button>().onClick.AddListener(delegate () {
                SelectForce(forceId);
            });
            if (prestigeNum>= int.Parse(LoadJsonFile.forcesTableDatas[forceId - 1][3]))
            {
                force.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                force.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = LoadJsonFile.forcesTableDatas[forceId - 1][3];
            }
        }
    }

    /// <summary>
    /// 选择势力
    /// </summary>
    private void SelectForce(int forceId)
    {
        int posId = 0;
        string[] forcesStr = LoadJsonFile.BattleTableDates[battleId][3].Split(',');  //战役势力集合
        string[] forcesPos = LoadJsonFile.BattleTableDates[battleId][5].Split(',');  //战役势力位置集合
        for (int i = 0; i < forcesStr.Length; i++)
        {
            if (forceId == int.Parse(forcesStr[i]))
            {
                posId = int.Parse(forcesPos[i]);
                break;
            }
        }
        if (prestigeNum >= int.Parse(LoadJsonFile.forcesTableDatas[forceId - 1][3]))
        {
            playerForceId = forceId;
            selectIcon.position = cityTran.GetChild(posId).position;
        }
        else
        {
            GoldNotEnough(LoadJsonFile.TipsTableDates[6][2]);
        }
        ShowTextOfForcesData(LoadJsonFile.forcesTableDatas[forceId - 1][2]); //战役解释
    }

    /// <summary>
    /// 纯文字提示文本
    /// </summary>
    public void GoldNotEnough(string str)
    {
        if (prompTran.childCount < 1)
        {
            GameObject tipObj = Instantiate(tipsText, prompTran);
            tipObj.GetComponent<Text>().text = str;
        }
    }

    //声音控制
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

    /// <summary>
    /// 文字播放
    /// </summary>
    /// <param name="data"></param>
    private void ShowTextOfForcesData(string data)
    {
        forcesText.DOPause();
        forcesText.text = "";
        forcesText.DOText("\u3000\u3000" + data, playTextSpeed).SetEase(Ease.Linear).SetAutoKill(false);
    }
}