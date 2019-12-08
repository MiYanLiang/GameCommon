using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BeginSend : MonoBehaviour
{
    public GameObject Difficulty;
    int DifficultyType;
    public GameObject Prestige; //声望
    public GameObject forcesText; //主界面的最上方势力解说Text

    public void ClickBeginGame()
    {
        GetDifficultyType();

        int prestigeNum = int.Parse(Prestige.GetComponent<Text>().text);
        PlayerPrefs.SetInt("DifficultyType", DifficultyType);       //难度值1-4
        PlayerPrefs.SetInt("prestigeNum", prestigeNum);             //声望值
        PlayerPrefs.SetInt("battleId", ForcesChoose.battleId);      //战役id
        PlayerPrefs.SetInt("forcesId", ForcesChoose.playerForceId); //玩家势力ID
        SceneManager.LoadScene(1);
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
}
