using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BeginSend : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Difficulty;
    public ForcesChoose FC;
    int DifficultyType;
    int forcesId;
    public GameObject Prestige; //声望
    int prestigeNum; //声望值
    //int forcePrestigeNum;//当前势力所需声望
    public GameObject forcesText; //主界面的最上方势力解说Text
    List<int> fetterId = new List<int>();  //六个势力的下标
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClickBeginGame()
    {
        GetDifficultyType();
        GetForcesId();
        //print("难度："+DifficultyType);
        //print("势力ID："+ forcesId);
        PlayerPrefs.SetInt("DifficultyType", DifficultyType);   //难度值1-4
        //PlayerPrefs.SetInt("forcesId", forcesId);                //玩家势力ID 1-11
        PlayerPrefs.SetInt("prestigeNum", prestigeNum);     //声望值
        if (forcesId < 1)
        {
            //forcesText.GetComponent<Text>().text = "\u3000\u3000" + "请在地图中选择一个势力。";
            forcesId = fetterId[0]+1;
            PlayerPrefs.SetInt("forcesId", forcesId);                //玩家势力ID 1-11
            SceneManager.LoadScene(1);
        }
        else
        {
            PlayerPrefs.SetInt("forcesId", forcesId);                //玩家势力ID 1-11
            if (int.Parse(LoadJsonFile.forcesTableDatas[FC.currentForcesIndex][3]) <= prestigeNum)
            {

                SceneManager.LoadScene(1);
            }
            else
            {
                forcesText.GetComponent<Text>().text = "\u3000\u3000" + "您的声望值不足，无法选中此势力。";
            }
        }
    }
    void GetDifficultyType()
    {
        if (Difficulty.GetComponent<Text>().text == "【萌新难度】")
        {
            DifficultyType = 1;
        }
        else if (Difficulty.GetComponent<Text>().text == "【普通难度】")
        {
            DifficultyType = 2;
        }
        else if (Difficulty.GetComponent<Text>().text == "【困难难度】")
        {
            DifficultyType = 3;
        }
        else if (Difficulty.GetComponent<Text>().text == "【炼狱难度】")
        {
            DifficultyType = 4;
        }
    }
    void GetForcesId()
    {
        forcesId = FC.currentForcesIndex+1;
        fetterId = FC.getForces;
        prestigeNum = int.Parse(Prestige.GetComponent<Text>().text);
    }
}
