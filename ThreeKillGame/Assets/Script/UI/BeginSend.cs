using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库
using UnityEngine.SceneManagement;
public class BeginSend : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Difficulty;
    public ForcesChoose FC;
    int DifficultyType;
    int forcesId;
    List<int> fetterId = new List<int>();
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
        PlayerPrefs.SetInt("DifficultyType", DifficultyType);
        PlayerPrefs.SetInt("forcesId",forcesId);
        PlayerPrefs.SetInt("forcesId0",fetterId[0]+1);
        PlayerPrefs.SetInt("forcesId1", fetterId[1]+1);
        PlayerPrefs.SetInt("forcesId2", fetterId[2]+1);
        PlayerPrefs.SetInt("forcesId3", fetterId[3]+1);
        PlayerPrefs.SetInt("forcesId4", fetterId[4]+1);
        PlayerPrefs.SetInt("forcesId5", fetterId[5]+1);
        //SceneManager.LoadScene(1);
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
    }
}
