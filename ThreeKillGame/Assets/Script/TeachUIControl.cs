using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachUIControl : MonoBehaviour
{
    [SerializeField]
    Transform teachUIObj;

    private int indexBakeImage; //大背景索引
    private int indexContent;   //小内容索引

    private int backCount;
    private int contentCount;

    [Header("开始引导的背景索引")]
    [SerializeField]
    int firstBackImage = 0;

    private void Start()
    {
        indexContent = 0;
        indexBakeImage = firstBackImage;
        backCount = teachUIObj.childCount;
        if (teachUIObj.GetChild(indexBakeImage))
        {
            contentCount = teachUIObj.GetChild(indexBakeImage).childCount;
        }
        else
        {
            contentCount = 0;
        }
        teachUIObj.GetChild(indexBakeImage).gameObject.SetActive(true);
        PlayTeachUI();
    }

    public void OnClickForNextGuide()
    {
        if (indexContent >= teachUIObj.GetChild(indexBakeImage).childCount)
        {
            teachUIObj.GetChild(indexBakeImage).GetChild(indexContent - 1).gameObject.SetActive(false);
            teachUIObj.GetChild(indexBakeImage).gameObject.SetActive(false);
            indexBakeImage++;
            indexContent = 0;
            if (indexBakeImage < backCount)
            {
                teachUIObj.GetChild(indexBakeImage).gameObject.SetActive(true);
                PlayTeachUI();
            }
            else
            {
                indexBakeImage = 0;
                teachUIObj.gameObject.SetActive(false);
            }
        }
        else
        {
            teachUIObj.GetChild(indexBakeImage).GetChild(indexContent - 1).gameObject.SetActive(false);
            PlayTeachUI();
        }
    }

    /// <summary>
    /// 播放内容
    /// </summary>
    private void PlayTeachUI()
    {
        GameObject obj = teachUIObj.GetChild(indexBakeImage).GetChild(indexContent).gameObject;
        obj.SetActive(true);
        indexContent++;
    }
}
