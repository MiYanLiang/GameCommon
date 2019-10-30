using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class DropScroll : MonoBehaviour {

	// Use this for initialization
    [SerializeField]
    private Button[] btnList;               //提供多个按钮
    private RectTransform thisRT;

    [SerializeField]
    private GameObject scrollViewTmp;       //提供一个滚动视图模版
    private List<RectTransform> scrollViewList = new List<RectTransform>(); 
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {
        RectTransform rtt = null;

        foreach (var btn in btnList)
        {
            EventTriggerListener.Get(btn.gameObject).OnClick += BtnDropClickEvent;
            rtt = GameObject.Instantiate(scrollViewTmp).GetComponent<RectTransform>();
            scrollViewList.Add(rtt);
            rtt.gameObject.SetActive(false);
            rtt.transform.SetParent(this.transform, false);
        }

        thisRT = this.GetComponent<RectTransform>();
    }
    int num = 0;
    string goName1="";
    string goName2="";
    private void BtnDropClickEvent(GameObject go)
    {
        RectTransform rt = go.GetComponent<RectTransform>();
        RectTransform btnRt = null;
        float height = thisRT.sizeDelta.y;
        int index = rt.GetSiblingIndex();
    
        HideAllScrollView();
        scrollViewList[index].sizeDelta = new Vector2(thisRT.sizeDelta.x, height - btnList.Length * rt.sizeDelta.y);
        scrollViewList[index].anchoredPosition = new Vector2(0, -((index + 1) * rt.sizeDelta.y) + 45);//-((index + 1) * rt.sizeDelta.y
        scrollViewList[index].gameObject.SetActive(true);        

        for (int i = 0; i < btnList.Length; i++)
        {
            btnRt = btnList[i].GetComponent<RectTransform>();
            if (i > index)
            {
                btnRt.anchoredPosition = new Vector2(btnRt.anchoredPosition.x, -height + ((btnList.Length - i) * btnRt.sizeDelta.y));
            }
            else
            {
                btnRt.anchoredPosition = new Vector2(btnRt.anchoredPosition.x, -(i * btnRt.sizeDelta.y));
            }
        }
        if(num==0)
        {
            goName1=go.ToString();
            num++;
        }
        else if(num==1)
        {
            goName2=go.ToString();
            if (goName1 == goName2)
            {
                HideAllScrollView();

                for (int i = 0; i < btnList.Length; i++)
                {
                    btnRt = btnList[i].GetComponent<RectTransform>();
                    if (i > index)
                    {
                        btnRt.anchoredPosition = new Vector2(btnRt.anchoredPosition.x, ((btnList.Length - i-3) * btnRt.sizeDelta.y));
                    }
                    else
                    {
                        btnRt.anchoredPosition = new Vector2(btnRt.anchoredPosition.x, -(i * btnRt.sizeDelta.y));
                    }
                }
            }
            num--;
        }
        //Debug.Log(num);
    }


    private void HideAllScrollView()
    {
        for (int i = 0; i < scrollViewList.Count; i++)
        {
            scrollViewList[i].gameObject.SetActive(false);
        }
    }
}

public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public event VoidDelegate OnClick;

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();

        if (listener == null)
            listener = go.AddComponent<EventTriggerListener>();

        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
            OnClick(gameObject);
    }
}
