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
        txt = this.GetComponent<Text>().text;
        if (txt.Length > 3)
        {
            this.GetComponent<Text>().fontSize = 50;
        }
        else
        {
            this.GetComponent<Text>().fontSize = 70;
        }
    }
}