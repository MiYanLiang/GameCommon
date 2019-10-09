using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroDataControll : MonoBehaviour {

    public List<string> heroData = new List<string>(); //存储当前武将的数据
    
    // Use this for initialization
    void Start () {
        if (heroData!=null)
        {
            //显示英雄名
            gameObject.transform.GetComponentInChildren<Text>().text = heroData[1];
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
