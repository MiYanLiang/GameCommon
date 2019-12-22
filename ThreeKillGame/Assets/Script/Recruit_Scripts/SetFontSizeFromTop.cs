using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFontSizeFromTop : MonoBehaviour
{
    string txt;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("傻瓜");
        txt = GetComponent<Text>().text;
        if (txt.Length > 2)
        {
            GetComponent<Text>().fontSize = 50;
        }
        else
        {
            GetComponent<Text>().fontSize = 70;
        }
    }
}