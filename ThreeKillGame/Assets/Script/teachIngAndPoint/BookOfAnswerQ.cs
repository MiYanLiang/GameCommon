using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookOfAnswerQ : MonoBehaviour
{
    [SerializeField]
    Transform TeacherObj;
    [SerializeField]
    float liteTimesToClose = 2f;

    private int rightIndex;  //正确题目编号

    private bool isChoosed; //是否已经选择

    private void Start()
    {
        TeacherObj.gameObject.AddComponent<Button>().onClick.AddListener(delegate ()
        {
            CloseThisObj();
        });
    }

    private void OnEnable()
    {
        isChoosed = false;
    }

    /// <summary>
    /// 初始化题目
    /// </summary>
    public void InfoQustionOfBook()
    {
        int indexQuestion = Random.Range(0, LoadJsonFile.TestTableDates.Count);
        rightIndex = int.Parse(LoadJsonFile.TestTableDates[indexQuestion][1]);

        TeacherObj.GetChild(2).GetComponent<Text>().text = LoadJsonFile.TestTableDates[indexQuestion][2];

        for (int i = 3; i < 6; i++)
        {
            TeacherObj.GetChild(i).GetChild(0).GetComponent<Text>().color = Color.white;
            TeacherObj.GetChild(i).GetChild(0).GetComponent<Text>().text = LoadJsonFile.TestTableDates[indexQuestion][i];
        }
    }
    /// <summary>
    /// 选择答案
    /// </summary>
    /// <param name="num"></param>
    public void SelectAnswer(int num)
    {
        TeacherObj.GetChild(num + 2).GetChild(1).gameObject.SetActive(true);
        if (num != rightIndex)
        {
            TeacherObj.GetChild(rightIndex + 2).GetChild(0).GetComponent<Text>().color = Color.green;
            TeacherObj.GetChild(num + 2).GetChild(0).GetComponent<Text>().color = Color.red;
            Invoke("CloseThisObj", liteTimesToClose);
        }
        else
        {
            TeacherObj.GetChild(num + 2).GetChild(0).GetComponent<Text>().color = Color.green;
            Invoke("CloseThisObj", liteTimesToClose);
        }
        isChoosed = true;
    }

    /// <summary>
    /// 关闭答题界面
    /// </summary>
    private void CloseThisObj()
    {
        if (isChoosed)
        {
            TeacherObj.gameObject.SetActive(false);
        }
    }
}
