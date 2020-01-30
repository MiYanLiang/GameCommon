using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryA : MonoBehaviour
{
    [SerializeField]
    Transform StoryAObject;

    public void InitializeStory()
    {
        int storyId = Random.Range(0,LoadJsonFile.StoryATableDates.Count);

        //故事标题
        StoryAObject.GetChild(2).GetComponent<Text>().text = LoadJsonFile.StoryATableDates[storyId][2];
        //故事主体
        StoryAObject.GetChild(3).GetComponent<Text>().text = LoadJsonFile.StoryATableDates[storyId][3];
        //选项
        for (int i=4; i<6; i++)
        {
            StoryAObject.GetChild(i).GetChild(0).GetComponent<Text>().text = LoadJsonFile.StoryATableDates[storyId][i+1];
        }
        //exit
        StoryAObject.GetChild(6).GetChild(0).GetComponent<Text>().text = LoadJsonFile.StoryATableDates[storyId][4];
    }
}
