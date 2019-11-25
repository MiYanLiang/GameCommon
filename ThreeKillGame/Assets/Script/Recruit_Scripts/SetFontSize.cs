using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetFontSize : MonoBehaviour
{
    string txt;

    void Update()
    {
        txt = this.GetComponent<Text>().text;
        if (txt.Length > 2)
        {
            this.GetComponent<Text>().fontSize = 40;
        }
        else
        {
            this.GetComponent<Text>().fontSize = 50;
        }
    }
}