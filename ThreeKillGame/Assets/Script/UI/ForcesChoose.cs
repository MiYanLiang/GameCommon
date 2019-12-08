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
    
    public GameObject cameraAudio;

    [SerializeField]
    private float playTextSpeed = 3f;

    public static int battleId;     //所选战役id
    public static int playerForceId;//玩家选择势力id

    void Start()
    {
        prestigeText.text = PlayerPrefs.GetInt("prestigeNum").ToString();
        soundContrll_(PlayerPrefs.GetInt("soundStates"));
        RandomBattle();
        playerForceId = 1;
    }

    /// <summary>
    /// 随机战役
    /// </summary>
    public void RandomBattle()
    {
        battleId = Random.Range(0, LoadJsonFile.BattleTableDates.Count);
        battleName.text = LoadJsonFile.BattleTableDates[battleId][1];  //战役名称
        ShowTextOfForcesData(LoadJsonFile.BattleTableDates[battleId][2]); //战役解释
    }

    /// <summary>
    /// 选择势力
    /// </summary>
    private void SelectForce()
    {

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