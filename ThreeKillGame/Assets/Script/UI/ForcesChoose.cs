using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ForcesChoose : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> forcesObj = new List<GameObject>();
    int count = 6;
    public List<int> getForces = new List<int>();  //拿到获取到的随机6个势力的下标
    List<string> forcesName = new List<string>();//存放6个势力的昵称
    List<string> forcesExplain = new List<string>();//存放6个势力的说明文字
    [HideInInspector]
    public int currentForcesIndex;
    public GameObject currentPrestige;
    bool canShow = false;
    public GameObject forcesText;
    int currentPrestige_;
    public GameObject cameraAudio;
    int currentPrestigeTo;

    [SerializeField]
    private float playTextSpeed = 3f;

    //战役
    public GameObject battleName;
    public static int battleId;
    void Start()
    {
        currentPrestigeTo = PlayerPrefs.GetInt("prestigeNum");
        RandomBattle();
        int soundStates = PlayerPrefs.GetInt("soundStates");
        soundContrll_(soundStates);
    }

    // Update is called once per frame
    private void Update()
    {
        ShowSelected();
        currentPrestige.GetComponent<Text>().text = PlayerPrefs.GetInt("prestigeNum").ToString();//随时刷新声望值
        //RandomBattle();
        //print(currentForcesIndex);
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
    //随机6个势力
    void RandomList()
    {
        //print(forcesObj.Count);
        //提前通过战役添加对应势力
        if (battleId == 0)
        {
            getForces.Add(0);
            getForces.Add(1);
        }
        else if (battleId == 1)
        {
            getForces.Add(1);
            getForces.Add(2);
        }
        else if (battleId == 2)
        {
            getForces.Add(0);
            getForces.Add(1);
            getForces.Add(2);
        }
        else if (battleId == 3)
        {
            getForces.Add(0);
            getForces.Add(1);
        }
        else if (battleId == 4)
        {
            getForces.Add(0);
            getForces.Add(1);
            getForces.Add(2);
        }
        else if (battleId == 5)
        {
            getForces.Add(0);
            getForces.Add(1);
            getForces.Add(2);
        }
        while (getForces.Count < count)
        {
            int temp_num = Random.Range(0, forcesObj.Count);
            if (!getForces.Contains(temp_num))
            {
                getForces.Add(temp_num);
            }
        }
        for (int i = 0; i < getForces.Count; i++)
        {
            print("势力ID:"+getForces[i]);
        }
        print("战役id:"+battleId);
    }
    //显示6个势力
    void ShowForces()
    {
        for (int i = 0; i < getForces.Count; i++)
        {
            forcesObj[getForces[i]].transform.Find("UnlockPrestige").gameObject.SetActive(true);
            forcesObj[getForces[i]].SetActive(true);
            //print(currentPrestige_);
            if (currentPrestige_ < int.Parse(forcesObj[getForces[i]].GetComponentsInChildren<Text>()[2].text))
            {
                forcesObj[getForces[i]].transform.Find("UnlockPrestige").gameObject.SetActive(true);
                forcesObj[getForces[i]].GetComponentsInChildren<Image>()[1].overrideSprite = Resources.Load("Image/initialImage/lock",typeof(Sprite)) as Sprite;
            }
            else
            {
                forcesObj[getForces[i]].transform.Find("UnlockPrestige").gameObject.SetActive(false);
            }
        }
    }
    //读表获取势力名称显示
    void GetForcesName(int index)
    {
        for (int i = 0; i < 11; i++)
        {
            if (int.Parse(LoadJsonFile.forcesTableDatas[i][0]) == index)
            {
                forcesName.Add(LoadJsonFile.forcesTableDatas[i][1]);
                forcesExplain.Add(LoadJsonFile.forcesTableDatas[i][2]);
            }
        }
    }

    //将势力名字赋值给前端
    void SetNameForObj()
    {
        for (int i = 0; i < getForces.Count; i++)
        {
            forcesObj[getForces[i]].GetComponentInChildren<Text>().text = forcesName[i];
        }
    }

    //给随机出来的势力添加点击事件
    void SetClickObj()
    {
        currentForcesIndex = -2;
        forcesObj[getForces[0]].GetComponent<Button>().onClick.AddListener(delegate ()
         {
             canShow = true;
             currentForcesIndex = getForces[0];
             ShowTextOfForcesData(forcesExplain[0]);
         });
        forcesObj[getForces[1]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[1];
            ShowTextOfForcesData(forcesExplain[1]);
        });
        forcesObj[getForces[2]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[2];
            ShowTextOfForcesData(forcesExplain[2]);
        });
        forcesObj[getForces[3]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[3];
            ShowTextOfForcesData(forcesExplain[3]);
        });
        forcesObj[getForces[4]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[4];
            ShowTextOfForcesData(forcesExplain[4]);
        });
        forcesObj[getForces[5]].GetComponent<Button>().onClick.AddListener(delegate ()
        {
            canShow = true;
            currentForcesIndex = getForces[5];
            ShowTextOfForcesData(forcesExplain[5]);
        });
    }

    //选中圈显示
    void ShowSelected()
    {
        if (canShow)
        {
            for (int i = 0; i < forcesObj.Count; i++)
            {
                if (i == currentForcesIndex)
                {
                    forcesObj[i].transform.Find("Back").gameObject.SetActive(true);
                }
                else
                {
                    forcesObj[i].transform.Find("Back").gameObject.SetActive(false);
                }
            }
        }
    }

    //随机战役
    public void RandomBattle()
    {
        currentForcesIndex = -1;//防止选中圈刷新时不清除
        //将Back关闭
        for (int i = 0; i < forcesObj.Count; i++)
        {
            forcesObj[i].transform.Find("Back").gameObject.SetActive(false);
        }
        //拿到当前声望
        currentPrestige_ = currentPrestigeTo;
        print("声望值:" + currentPrestige_);
        for (int i = 0; i < getForces.Count; i++)
        {
            forcesObj[getForces[i]].SetActive(false);
        }
        getForces.Clear();
        forcesName.Clear();
        forcesExplain.Clear();
        int temp_num = 0;
        int battleCount = LoadJsonFile.BattleTableDates.Count;
        temp_num = Random.Range(0, battleCount);
        battleId = temp_num;
        battleName.GetComponent<Text>().text = LoadJsonFile.BattleTableDates[battleId][1];  //战役名称
        ShowTextOfForcesData(LoadJsonFile.BattleTableDates[battleId][2]); //战役解释
        //调整字体大小
        string battleName_ = battleName.GetComponent<Text>().text;
        print(battleName_.Length);
        if (battleName_.Length < 5)
        {
            battleName.GetComponent<Text>().fontSize = 90;
        }
        else if (battleName_.Length >= 5)
        {
            battleName.GetComponent<Text>().fontSize = 80;
        }
        RandomList();
        ShowForces();
        for (int i = 0; i < getForces.Count; i++)
        {
            GetForcesName(getForces[i] + 1);
        }
        SetNameForObj();
        SetClickObj();
    }

    /// <summary>
    /// 文字播放
    /// </summary>
    /// <param name="data"></param>
    private void ShowTextOfForcesData(string data)
    {
        forcesText.GetComponent<Text>().DOPause();
        forcesText.GetComponent<Text>().text = "";
        forcesText.GetComponent<Text>().DOText("\u3000\u3000" + data, playTextSpeed).SetEase(Ease.Linear).SetAutoKill(false);
    }
}