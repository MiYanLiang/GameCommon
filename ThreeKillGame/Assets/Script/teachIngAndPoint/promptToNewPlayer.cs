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
    [Header("动画索引")]
    [SerializeField]
    int animClipIndex = 0;  //动画片段索引

    [SerializeField]
    AnimationClip[] handAnimClips;

    bool isShow;    //是否提示过

    bool booIndex;  

    GameObject tipHandObj;

    Animator anim;

    private void Awake()
    {
        booIndex = false;
        isShow = false;
        nowHadShowPrompt = false;
        tipHandObj = transform.GetChild(0).gameObject;
        anim = tipHandObj.GetComponent<Animator>();
    }

    private void Start()
    {
        
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
            if (animClipIndex < handAnimClips.Length)
            {
                string str = handAnimClips[animClipIndex].name;
                anim.SetTrigger(str);
                Debug.Log("动画" + animClipIndex + " : " + str);
            }
        }
    }

    //控制指引的显隐
    public void isShowPrompt(bool boo)
    {
        if (booIndex != boo)
        {
            booIndex = boo;
            //gameObject.GetComponent<Image>().enabled = boo;
            tipHandObj.SetActive(boo);

            ChangeHadShowPromptState(boo);
        }
    }

    //更改全局变量状态
    private void ChangeHadShowPromptState(bool boo)
    {
        nowHadShowPrompt = boo;
    }
}
