using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class promptToNewPlayer : MonoBehaviour
{
    public static bool nowHadShowPrompt;    //记录当前有没有在指引的东西
    [Header("开启时长")]
    [SerializeField]
    float startShow = 5f;   //多久后提示

    bool isShow;    //是否提示过

    private void Awake()
    {
        isShow = false;
        nowHadShowPrompt = false;
    }

    private void OnEnable()
    {
        if (isShow)
        {
            Debug.Log("已经提示过" + transform.parent.gameObject.name);
        }
        else
        {
            Invoke("openStartShow", startShow);
        }
    }

    private void openStartShow()
    {
        if (nowHadShowPrompt)
        {
            Invoke("openStartShow", startShow);
        }
        else
        {
            isShow = true;
            isShowPrompt(true);
        }
    }

    //控制指引的显隐
    public void isShowPrompt(bool boo)
    {

        gameObject.GetComponent<Image>().enabled = boo;
        
        ChangeHadShowPromptState(boo);
       
    }

    //更改全局变量状态
    private void ChangeHadShowPromptState(bool boo)
    {
        nowHadShowPrompt = boo;
    }
}
